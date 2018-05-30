using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Messages {
    /// <summary>
    /// Time sync message from the server to the client.
    /// </summary>
    public class TimeSyncMessage : NetMessage {
        #region Properties
        /// <summary>
        /// The type of message it is.
        /// </summary>
        public override NetMessageType Type {
            get { return NetMessageType.TimeSync; }
        }

        /// <summary>
        /// The category of the message
        /// </summary>
        public override NetMessageCategory Category {
            get { return NetMessageCategory.Time; }
        }

        /// <summary>
        /// The server's time when the time sync
        /// was sent out.
        /// </summary>
        public double ServerTime { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new outgoing time sync message.
        /// </summary>
        public TimeSyncMessage(double time) : base(72) {
            ServerTime = time;
            buffer.Write(time);
        }

        /// <summary>
        /// Rebuild an incoming time sync message.
        /// </summary>
        /// <param name="sender">The server that sent the message.</param>
        /// <param name="reader">The data of the message.</param>
        public TimeSyncMessage(NetPeer sender, NetDataReader reader) :base (sender, reader) {
            ServerTime = buffer.ReadDouble();
        }
        #endregion
    }
}
