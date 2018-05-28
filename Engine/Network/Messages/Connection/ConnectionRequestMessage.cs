using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using LiteNetLib;
using LiteNetLib.Utils;

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
        /// Create a bew ConnectionRequestMessage to inform
        /// the server that a new client wishes to connect.
        /// </summary>
        /// <param name="sender">The NetPeer of the new client.</param>
        public ConnectionRequestMessage(NetPeer sender) : base(sender) {
           
        }
        #endregion
    }
}