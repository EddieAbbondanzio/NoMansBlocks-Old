using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Voxelated.Network.Messages {
    /// <summary>
    /// Message that a client has successfully connected
    /// to the server. (They have been approved).
    /// </summary>
    public class ConnectMessage : NetMessage {
        #region Properties
        /// <summary>
        /// The type of message it is.
        /// </summary>
        public override NetMessageType Type {
            get { return NetMessageType.Connect; }
        }

        /// <summary>
        /// What category the message falls into
        /// </summary>
        public override NetMessageCategory Category {
            get { return NetMessageCategory.Connection; }
        }

        /// <summary>
        /// The nickname they want to use.
        /// </summary>
        public string Name { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Rebuild a connect message from an incoming message
        /// that was recieved over the network.
        /// </summary>
        /// <param name="inMsg">The lidgren in message.</param>
        public ConnectMessage(NetIncomingMessage inMsg) : base(inMsg) {
            //Validate inputs
            if(inMsg == null) {
                throw new ArgumentNullException("inMsg", "Cannot be null!");
            }

            Name = inMsg.SenderConnection.RemoteHailMessage.ReadString();
        }
        #endregion
    }
}