using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network.Lobby;
using Voxelated.Network.Messages;
using Voxelated.Utilities;

namespace Voxelated.Network.Client {
    /// <summary>
    /// A client instance for the game network. Allows
    /// player to communicate with the server.
    /// </summary>
    public sealed class NetClientManager : NetManager {
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
        public NetClientSettings Settings { get; private set; }

        /// <summary>
        /// Maintains the connection with the server
        /// for this client.
        /// </summary>
        public NetClientConnectionHandler ConnectionHandler { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Creates a new instance of a net client manager
        /// with the pre-defined permissions level.
        /// </summary>
        /// <param name="settings">The settings for the manager to follow.</param>
        public NetClientManager(NetClientSettings settings) : base(settings) {
            Settings = settings;
            ConnectionHandler = new NetClientConnectionHandler(this, netManager);
        }
        #endregion

        #region Publics
        /// <summary>
        /// Connect to a server using the inputted nickname as
        /// the player name.
        /// </summary>
        /// <param name="serverAddress">The server's ip address.</param>
        public void Connect(NetEndPoint serverAddress) {
            netManager.Start();

            LoggerUtils.Log("NetClientManager: Connecting to server at + " + serverAddress.ToString(), LogLevel.Release);
            ConnectionHandler.SendConnectionRequest(serverAddress);

        }

        /// <summary>
        /// Disconnect from the server, if connected to any...
        /// </summary>
        public void Disconnect() {
            if (netManager != null) {
                LoggerUtils.Log("NetClientManager: Disconnecting from server.", LogLevel.Release);
                netManager.Stop();
            }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Send a message over the network to the server. It encodes the message into
        /// a NetOutGoingMessage through lidgren.
        /// </summary>
        /// <param name="message">The message to send out.</param>
        /// <param name="method">What method to send it over the wire.</param>
        public override void SendMessage(NetMessage message, SendOptions method) {
            byte[] bytes = message?.Serialize();

            if(bytes != null && netManager.PeersCount > 0) {
                netManager.GetFirstPeer().Send(bytes, method);
            }
            else { 
                LoggerUtils.LogWarning("NetClientManager: Unable to send message to server.");
            }
        }
        #endregion
    }
}
