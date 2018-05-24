using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Voxelated.Network.Messages {
    /// <summary>
    /// Message to indicate that a client wishes to connect
    /// to the server.
    /// </summary>
    public class ConnectionRequestMessage : NetMessage {
        #region Properties
        /// <summary>
        /// The type of message it is.
        /// </summary>
        public override NetMessageType Type {
            get { return NetMessageType.ConnectionRequest; }
        }

        /// <summary>
        /// The category of the message
        /// </summary>
        public override NetMessageCategory Category {
            get { return NetMessageCategory.Connection; }
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new incoming connection message.
        /// </summary>
        /// <param name="inMsg">The lidgren in message to
        /// decode it from.</param>
        public ConnectionRequestMessage(NetIncomingMessage inMsg) : base(inMsg) {
            //Validate inputs
            if (inMsg == null) {
                throw new ArgumentNullException("inMsg", "Cannot be null!");
            }
        }
        #endregion
    }
}