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

        /// <summary>
        /// The time according to server time that the message
        /// was recieved at.
        /// </summary>
        public double RecievedTime { get; private set; }
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
            buffer = new ByteBuffer(40);

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
                //Pull in the data of the message.
                byte[] payload = reader.Data;
                buffer = new ByteBuffer(payload, isReadOnly);
                buffer.SetPointerIndex(8);
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
            //If it's an outgoing message, jump back to the header and
            //write the size.
            if (!IsIncoming && buffer.PointerIndex != 40) {
                if(buffer.PointerIndex > 40) {
                    buffer.SetPointerIndex(8);
                    buffer.Write(buffer.ByteLength);
                }
                else {
                    buffer.Write((byte)Type);
                }
            }

            return buffer.Serialize();
        }
        #endregion
    }
}
