using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib.Utils;

namespace Voxelated.Network.Messages {
    public class ClientGreetingMessage : NetMessage {
        #region Properties
        /// <summary>
        /// The type of message it is.
        /// </summary>
        public override NetMessageType Type {
            get { return NetMessageType.ClientGreeting; }
        }

        /// <summary>
        /// The category of the message
        /// </summary>
        public override NetMessageCategory Category {
            get { return NetMessageCategory.Connection; }
        }

        /// <summary>
        /// The name of the client that just connected.
        /// </summary>
        public string Name { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new client greeting message to
        /// send to a server.
        /// </summary>
        /// <param name="name">The name of this client.</param>
        public ClientGreetingMessage(string name) : base() {
            Name = name;

            buffer.Write(name);
        }

        /// <summary>
        /// Decode a client message that was recieved from over the
        /// network.
        /// </summary>
        /// <param name="sender">The client that sent the message.</param>
        /// <param name="reader">The payload.</param>
        public ClientGreetingMessage(NetPeer sender, NetDataReader reader) : base(sender, reader) {
            Name = buffer.ReadString();
        }
        #endregion
    }
}
