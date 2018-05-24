using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Voxelated.Network.Messages {
    /// <summary>
    /// Message to indicate that a client is disconnecting
    /// from the server. Contains no payload, as it is simply
    /// an alert.
    /// </summary>
    public class DisconnectMessage : NetMessage {
        #region Properties
        /// <summary>
        /// The type of message it is.
        /// </summary>
        public override NetMessageType Type {
            get { return NetMessageType.Disconnect; }
        }

        /// <summary>
        /// What category the message falls into
        /// </summary>
        public override NetMessageCategory Category {
            get { return NetMessageCategory.Connection; }
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Build an incoming message from a client
        /// that they are disconnecting.
        /// </summary>
        /// <param name="inMsg">The incoming lidgren message.</param>
        public DisconnectMessage(NetIncomingMessage inMsg) : base(inMsg) {
         
        }
        #endregion
    }
}