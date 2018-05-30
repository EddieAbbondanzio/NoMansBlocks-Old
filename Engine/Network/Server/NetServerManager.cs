using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Utilities;
using Voxelated.Network.Messages;
using Voxelated.Network.Lobby;
using LiteNetLib;

namespace Voxelated.Network.Server {
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
        public NetServerConnectionHandler ConnectionHandler { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new network manager for a server with
        /// the following settings.
        /// </summary>
        /// <param name="settings">The settings for the server to adhere to.</param>
        public NetServerManager(NetServerSettings settings) : base(settings) {
            Settings = settings;
            ConnectionHandler = new NetServerConnectionHandler(this);
        }
        #endregion

        #region Publics
        /// <summary>
        /// Host a new server.
        /// </summary>
        public void Host() {
            if(netManager != null) {
                LoggerUtils.Log("NetServerManager: Starting new server on port 14242.", LogLevel.Release);

                netManager.Start(14242);
            }
        }

        /// <summary>
        /// Shut down the server.
        /// </summary>
        public void Stop() {
            if(netManager != null) {
                LoggerUtils.Log("NetServerManager: Stopping server", LogLevel.Release);

                netManager.Stop();
            }
        }

        /// <summary>
        /// Kick the player from the server.
        /// </summary>
        /// <param name="peer">The client to kick.</param>
        /// <param name="reason">Why they were kicked.</param>
        public void KickClient(NetPeer peer, string reason) {
            if(netManager != null) {
                byte[] message = SerializeUtils.Serialize(reason);
                netManager.DisconnectPeer(peer, message);
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
        public override void SendMessage(NetMessage message, SendOptions method) {
            byte[] bytes = message?.Serialize();

            if(bytes != null && netManager.PeersCount > 0) {
                netManager.SendToAll(bytes, method);
            }
            else {
                LoggerUtils.Log("NetServerManager: Unable to send message of type: " + message.Type + " out.");
            }
        }
        #endregion
    }
}
