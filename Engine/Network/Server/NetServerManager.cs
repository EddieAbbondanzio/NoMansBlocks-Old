using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Utilities;
using Voxelated.Network.Messages;
using Voxelated.Network.Lobby;

namespace Voxelated.Network {
    /// <summary>
    /// A server instance for the game network.
    /// </summary>
    public sealed class NetServerManager : NetManager {
        #region Properties
        /// <summary>
        /// If the manager is a server. Which a server
        /// always is...
        /// </summary>
        public override bool IsServer {
            get { return true; }
        }

        /// <summary>
        /// The settings of the server
        /// </summary>
        public NetServerSettings Settings { get; private set; }

        /// <summary>
        /// The active clients and their connections.
        /// </summary>
        public NetClientConnectionManager ClientManager { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new network manager for a server with
        /// the following settings.
        /// </summary>
        /// <param name="settings">The settings for the server to adhere to.</param>
        public NetServerManager(NetServerSettings settings) : base(new NetServerEventListener(), settings) {
            Settings = settings;
            ClientManager = new NetClientConnectionManager(this);
        }
        #endregion

        #region Publics
        /// <summary>
        /// Host a new server.
        /// </summary>
        public void Host() {
            //Build the configuration
            var config = new NetPeerConfiguration("Voxelated") {
                Port = 14242,
                //SimulatedMinimumLatency = 0.2f,
                //SimulatedLoss = 0.1f
                MaximumConnections = Settings.Capacity,
                AutoFlushSendQueue = false
            };
            
            //These aren't enabled by default. Lame-o!
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            //Set up the server
            netPeer = new NetServer(config);
            netPeer.Start();
        }

        /// <summary>
        /// Shut down the server.
        /// </summary>
        public void Stop() {
            if(netPeer != null) {
                LoggerUtils.Log("NetServerManager: Stopping server...", LogLevel.Debug);
                netPeer.Shutdown("Bye!");
                netPeer = null;

                Lobby = null;
            }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Send a message to every client in the lobby.
        /// </summary>
        /// <param name="message">The message to send out.</param>
        /// <param name="method">What method to send it over the wire.</param>
        /// <param name="channel">The channel to send it on.</param>
        public override void SendMessage(NetMessage message, NetDeliveryMethod method, NetChannel channel) {
            NetOutgoingMessage outMsg = netPeer.CreateMessage();
            outMsg.Write(message.Serialize());

            //Don't send messages when no one's listening.
            if (ClientManager.ConnectionCount() == 0) {
                return;
            }

            if(outMsg != null && netPeer != null) {
                netPeer.SendMessage(outMsg, ClientManager.GetConnections(), method, (int)channel);
            }
        }
        #endregion
    }
}
