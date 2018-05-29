using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Messages {
    /// <summary>
    /// Request from a client to the server for a time sync.
    /// </summary>
    public class TimeSyncRequestMessage : NetMessage {
        #region Properties
        /// <summary>
        /// The type of message it is.
        /// </summary>
        public override NetMessageType Type {
            get { return NetMessageType.TimeSyncRequest; }
        }

        /// <summary>
        /// The category of the message
        /// </summary>
        public override NetMessageCategory Category {
            get { return NetMessageCategory.Time; }
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Decode an incoming time sync request message. No payload
        /// as there's nothing to send in.
        /// </summary>
        /// <param name="sender">The client requesting the time sync.</param>
        public TimeSyncRequestMessage(NetPeer sender) : base(sender) {
        }
        #endregion
    }
}
