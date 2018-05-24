using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network.Messages;
using Voxelated.Serialization;
using Voxelated.Utilities;

namespace Voxelated.Network {
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
        /// The network connection of the message sender. Careful
        /// this can be null if it's a debug or info message.
        /// </summary>
        public NetConnection SenderConnection { get; protected set; }
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
            buffer = new ByteBuffer();

            //Serialize the header info.
            buffer.Write((byte)Type);
            buffer.SkipWritingBits(32);
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
            buffer.SkipWritingBits(32);
        }

        /// <summary>
        /// Create a new incoming message that has a payload.
        /// </summary>
        /// <param name="inMsg">The lidgren message with the data in it.</param>
        protected NetMessage(NetIncomingMessage inMsg) {
            if (inMsg.SenderConnection != null) {
                SenderConnection = inMsg.SenderConnection;
            }

            if(inMsg.MessageType == NetIncomingMessageType.Data) {
                int byteCount = inMsg.ReadInt32();

                //We read 5 less bytes since message type, and byte count have already been
                //read out.
                buffer = new ByteBuffer(inMsg.ReadBytes(byteCount - 5));
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
            //Jump back to the header and write how many bytes are in it.
            buffer.SetPointerIndex(8);
            buffer.Write(buffer.ByteLength);
            return buffer.Serialize();

        }
        #endregion

        #region Statics
        /// <summary>
        /// Convert an incoming message into a usable NetMessage.
        /// </summary>
        /// <param name="inMsg">The message that was recieved
        /// from over the network.</param>
        /// <returns>The decoded network message.</returns>
        public static NetMessage DecodeMessage(NetIncomingMessage inMsg) {
            try {
                switch (inMsg.MessageType) {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        return DecodeInfoMessage(inMsg);

                    case NetIncomingMessageType.ConnectionApproval:
                    case NetIncomingMessageType.StatusChanged:
                        return DecodeConnectionMessage(inMsg);

                    case NetIncomingMessageType.Data:
                    case NetIncomingMessageType.UnconnectedData:
                        return DecodeDataMessage(inMsg);

                    default:
                        return null;
                }
            }
            catch (Exception e) {
                LoggerUtils.LogError("NetMessage.DecodeMessage(): " + e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Rebuild an information message that was recieved 
        /// from over network.
        /// </summary>
        /// <param name="inMsg">The message that it was delivered on.</param>
        /// <returns>The decoded info message. This contains a 
        /// text based message of some network info.</returns>
        private static NetMessage DecodeInfoMessage(NetIncomingMessage inMsg) {
            if(inMsg != null) {
                return new InfoMessage(inMsg);
            }
            else {
                return null; 
            }
        }

        /// <summary>
        /// Rebuild a connection message such as new client connecting or leaving.
        /// This is ignored when not server.
        /// </summary>
        /// <param name="inMsg">The message that it was delivered on.</param>
        /// <returns>The decoded connection update message.</returns>
        private static NetMessage DecodeConnectionMessage(NetIncomingMessage inMsg) {
            if (inMsg == null) {
                return null;
            }

            //New client wants to connect
            if (inMsg.MessageType == NetIncomingMessageType.ConnectionApproval) {
                return new ConnectionRequestMessage(inMsg);
            }
            //Client has fully connected, or wants to disconnect
            else if (inMsg.MessageType == NetIncomingMessageType.StatusChanged) {
                switch ((NetConnectionStatus) inMsg.ReadByte()) {
                    case NetConnectionStatus.Connected:
                        if (VoxelatedEngine.Engine.NetManager.IsServer) {
                            return new ConnectMessage(inMsg);
                        }
                        else {
                            return null;
                        }

                    case NetConnectionStatus.Disconnected:
                        if (VoxelatedEngine.Engine.NetManager.IsServer) {
                            return new DisconnectMessage(inMsg);
                        }
                        else {
                            return new DisconnectedMessage(inMsg);
                        }
                }
            }

            return null;
        }

        /// <summary>
        /// Decode a data message from the network. These are the important messages
        /// as they have valuable payloads on them.
        /// </summary>
        /// <param name="inMsg">The message it was delivered on.</param>
        /// <returns>The decoded data message.</retuns>
        private static NetMessage DecodeDataMessage(NetIncomingMessage inMsg) {
            if(inMsg == null) {
                return null;
            }

            //Pull in header info.
            NetMessageType msgType = (NetMessageType)inMsg.ReadByte();
            switch (msgType) {
                case NetMessageType.LobbyChat:
                    return new LobbyChatMessage(inMsg);

                case NetMessageType.Command:
                    return new CommandMessage(inMsg);

                case NetMessageType.LobbySync:
                    return new LobbySyncMessage(inMsg);

                case NetMessageType.PlayerJoined:
                    return new PlayerJoinedMessage(inMsg);

                case NetMessageType.PlayerLeft:
                    return new PlayerLeftMessage(inMsg);

                default:
                    return null;
            }
        }
        #endregion
    }
}
