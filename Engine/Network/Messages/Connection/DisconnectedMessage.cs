using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Messages {
    public class DisconnectedMessage  : NetMessage {
        #region Properties
        /// <summary>
        /// The type of message it is.
        /// </summary>
        public override NetMessageType Type {
            get { return NetMessageType.Disconnected; }
        }

        /// <summary>
        /// What category the message falls into
        /// </summary>
        public override NetMessageCategory Category {
            get { return NetMessageCategory.Connection; }
        }

        /// <summary>
        /// The reason why the client was disconnected.
        /// </summary>
        public string Reason { get; set; }
        #endregion

        /// <summary>
        /// Build an incoming message from the server
        /// regarding why you were disconnected.
        /// </summary>
        /// <param name="inMsg">The incoming lidgren message.</param>
        public DisconnectedMessage(NetIncomingMessage inMsg) : base(inMsg) {
            Reason = inMsg.ReadString() ?? string.Empty;
        }
    }
}
