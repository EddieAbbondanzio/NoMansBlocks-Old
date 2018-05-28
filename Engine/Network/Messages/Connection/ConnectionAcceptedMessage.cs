using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib.Utils;

namespace Voxelated.Network.Messages {
    /// <summary>
    /// Message to alert client that it's connection
    /// has been accepted by a server.
    /// </summary>
    public class ConnectionAcceptedMessage : NetMessage {
        #region Properties
        /// <summary>
        /// The type of message it is.
        /// </summary>
        public override NetMessageType Type {
            get { return NetMessageType.ConnectionAccepted; }
        }

        /// <summary>
        /// The category of the message
        /// </summary>
        public override NetMessageCategory Category {
            get { return NetMessageCategory.Connection; }
        }

        /// <summary>
        /// The name of the server being connected to.
        /// </summary>
        public string ServerName { get; private set; }

        /// <summary>
        /// The description of the server being connection to.
        /// </summary>
        public string ServerDescription { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new outgoing connection accepted message.
        /// </summary>
        public ConnectionAcceptedMessage(string serverName, string serverDescription) : base() {
            ServerName = serverName;
            ServerDescription = serverDescription;

            buffer.Write(ServerName);
            buffer.Write(serverDescription);
        }

        /// <summary>
        /// Create a new incoming connection
        /// accepted message.
        /// </summary>
        /// <param name="sender">The server who accepted the client.</param>
        public ConnectionAcceptedMessage(NetPeer sender, NetDataReader reader) : base(sender, reader) {
            ServerName = buffer.ReadString();
            ServerDescription = buffer.ReadString();
        }
        #endregion
    }
}
