using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network.Lobby;
using Voxelated.Network.Messages;
using Voxelated.Utilities;

namespace Voxelated.Network {
    /// <summary>
    /// A client instance for the game network. Allows
    /// player to communicate with the server.
    /// </summary>
    public class NetClientManager : NetManager {
        #region Properties
        /// <summary>
        /// If the client is a server. Which is never
        /// true.
        /// </summary>
        public override bool IsServer {
            get { return false; }
        }

        /// <summary>
        /// The preferred settings of the client.
        /// </summary>
        public NetClientSettings Settings { get; set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Creates a new instance of a net client manager
        /// with the pre-defined permissions level.
        /// </summary>
        /// <param name="settings">The settings for the manager to follow.</param>
        public NetClientManager(NetClientSettings settings) : base(NetPermissions.Player) {
            Settings = settings;
        }
        #endregion

        #region Publics
        /// <summary>
        /// Connect to a server using the inputted nickname as
        /// the player name.
        /// </summary>
        /// <param name="serverAddress">The server's ip address.</param>
        public void Connect(IPEndPoint serverAddress) {
            //Set up the configuration.
            var config = new NetPeerConfiguration("Voxelated") {
                //SimulatedMinimumLatency = 0.2f,
                //SimulatedLoss = 0.1f
                MaximumConnections = 1,
                AutoFlushSendQueue = false
            };

            config.EnableMessageType(NetIncomingMessageType.UnconnectedData);
            config.DisableMessageType(NetIncomingMessageType.ConnectionApproval);

            //Get the client going.
            netPeer = new NetClient(config);
            netPeer.Start();

            NetOutgoingMessage hailMsg = netPeer.CreateMessage();
            hailMsg.Write(Settings.Name);

            LoggerUtils.Log("NetClientManager: Connecting to server at + " + serverAddress.ToString(), LogLevel.Release);
            netPeer.Connect(serverAddress, hailMsg);
        }

        /// <summary>
        /// Disconnect from the server, if connected to any...
        /// </summary>
        public void Disconnect() {
            if (netPeer != null) {
                LoggerUtils.Log("NetClientManager: Disconnecting from server.", LogLevel.Release);
                netPeer.Shutdown("Bye!");

                Lobby = null;
            }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Send a message over the network. It encodes the message into
        /// a NetOutGoingMessage through lidgren.
        /// </summary>
        /// <param name="message">The message to send out.</param>
        /// <param name="method">What method to send it over the wire.</param>
        /// <param name="channel">The channel to send it on.</param>
        public override void SendMessage(NetMessage message, NetDeliveryMethod method, NetChannel channel) {
            NetOutgoingMessage outMsg = netPeer.CreateMessage();
            outMsg.Write(message.Serialize());

            if (outMsg != null && netPeer != null) {
                LoggerUtils.Log("NetClient: Sending message of type: " + message.Type);
                netPeer.SendMessage(outMsg, netPeer.Connections, method, (int)channel);
            }
        }
        #endregion
    }
}
