using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib;
using LiteNetLib.Utils;
using Voxelated.Utilities;

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
        public DisconnectReason Reason { get; private set; }

        /// <summary>
        /// Additional text message of why client was
        /// disconnected. Can be empty.
        /// </summary>
        public string Message { get; private set; } = string.Empty;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Build an incoming message from the server
        /// regarding why you were disconnected.
        /// </summary>
        /// <param name="inMsg">The incoming lidgren message.</param>
        public DisconnectedMessage(NetPeer sender, DisconnectInfo info) : base(sender) {
            Reason = info.Reason;

            if(Reason == DisconnectReason.DisconnectPeerCalled && info.AdditionalData.AvailableBytes > 0) {
                byte[] msgBytes = info.AdditionalData.GetRemainingBytes();
                Message = SerializeUtils.GetString(msgBytes, 0);
            }
        }
        #endregion
    }
}
