using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network.Messages;
using Voxelated.Serialization;
using Voxelated.Utilities;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Voxelated.Network.Messages {
    /// <summary>
    /// Base class to derive any incoming or outgoing message from
    /// the network. Contains some base data such as sent net time
    /// and sender connection.
    /// </summary>
    public abstract class NetMessage {
        #region Properties
        /// <summary>
        /// Flag to tell the reciever of the message how to parse the 
        /// payload in it.
        /// </summary>
        public abstract NetMessageType Type { get; }

        /// <summary>
        /// Flag to tell the network interface where to give the message
        /// to. 
        /// </summary>
        public abstract NetMessageCategory Category { get; }

        /// <summary>
        /// The sender of who sent this message.
        /// </summary>
        public NetPeer Sender { get; protected set; }

        /// <summary>
        /// If the message was recieved from over the network.
        /// </summary>
        public bool IsIncoming { get; private set; }
        #endregion

        #region Members
        /// <summary>
        /// The buffer that is used to write data to. This is kept
        /// hidden to control what exactly gets written to the message.
        /// </summary>
        protected ByteBuffer buffer;
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new outgoing net message to be sent over the server.
        /// </summary>
        protected NetMessage() {
            buffer = new ByteBuffer(8);

            //Serialize the type.
            buffer.Write((byte)Type);
        }

        /// <summary>
        /// Create a new outgoing net message with a specific size byte buffer.
        /// Call this over empty constructor when possible as it is faster.
        /// </summary>
        /// <param name="bitCount">The number of bits to pre
        /// create in the buffer.</param>
        protected NetMessage(int bitCount) {
            buffer = new ByteBuffer(bitCount);

            //Serialize the header info.
            buffer.Write((byte)Type);
        }

        /// <summary>
        /// Create a new message that was recieved 
        /// with no data on it.
        /// </summary>
        /// <param name="sender"></param>
        protected NetMessage(NetPeer sender) {
            Sender = sender;
        }

        /// <summary>
        /// Read in the content of the new message that was recieved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="reader"></param>
        /// <param name="isReadOnly"></param>
        protected NetMessage(NetPeer sender, NetDataReader reader, bool isReadOnly = true) {
            //Log who sent it.
            if(sender != null) {
                Sender = sender;
            }

            try {
                //Pull in content if it has any.
                if ((reader.PeekByte() & 128) == 0) {
                    //Get content
                    byte[] bytes = reader.GetRemainingBytes();
                    buffer = new ByteBuffer(bytes, isReadOnly);
                    buffer.SetPointerIndex(8);
                }
            }
            catch(Exception e) {
                LoggerUtils.LogError(e.ToString());
            }
        }
        #endregion

        #region Serialization
        /// <summary>
        /// Convert the message into a byte array for sending
        /// out over the network.
        /// </summary>
        /// <returns>The encoded message</returns>
        public byte[] Serialize() {
            if (!IsIncoming && buffer.PointerIndex == 8) {
                buffer.SetPointerIndex(7);
                buffer.Write(true); //Bool bit to indicate no data.
            }

            return buffer.Serialize();
        }

        /// <summary>
        /// Rebuild a message that was sent over the network.
        /// </summary>
        /// <param name="sender">The net peer who sent the message</param>
        /// <param name="reader">The data of the message recieved</param>
        /// <returns>The decoded net message.</returns>
        public static NetMessage Deserialize(NetPeer sender, NetDataReader reader) {
            //Need to remove the bit that signals an empty message first.
            byte rawType = reader.PeekByte();
            NetMessageType msgType = (NetMessageType)(rawType & 127);
            NetMessage netMsg = null;

            switch (msgType) {
                case NetMessageType.ConnectionRequest:
                    netMsg = new ConnectionRequestMessage(sender);
                    break;

                case NetMessageType.ConnectionAccepted:
                    netMsg = new ConnectionAcceptedMessage(sender, reader);
                    break;

                case NetMessageType.ClientGreeting:
                    netMsg = new ClientGreetingMessage(sender, reader);
                    break;

                case NetMessageType.LobbySync:
                    netMsg = new LobbySyncMessage(sender, reader);
                    break;

                case NetMessageType.LobbyChat:
                    netMsg = new LobbyChatMessage(sender, reader);
                    break;

                case NetMessageType.Command:
                    netMsg = new CommandMessage(sender, reader);
                    break;

                case NetMessageType.PlayerJoined:
                    netMsg = new PlayerJoinedMessage(sender, reader);
                    break;

                case NetMessageType.PlayerLeft:
                    netMsg = new PlayerLeftMessage(sender, reader);
                    break;

                case NetMessageType.TimeSyncRequest:
                    netMsg = new TimeSyncRequestMessage(sender, reader);
                    break;

                case NetMessageType.TimeSync:
                    netMsg = new TimeSyncMessage(sender, reader);
                    break;

                case NetMessageType.ActiveTimersSync:
                    netMsg = new ActiveTimersSync(sender, reader);
                    break;



                default:
                    LoggerUtils.Log("NetMessage: Deserialize(): Bad type: " + msgType);
                    break;
            }

            return netMsg;
        }
        #endregion
    }
}
