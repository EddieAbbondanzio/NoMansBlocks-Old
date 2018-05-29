using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Utilities;
using Voxelated.Network.Messages;
using Voxelated.Network.Lobby;
using LiteNetLib;

namespace Voxelated.Network {
    /// <summary>
    /// Base class for server and client network managers. 
    /// </summary>
    public abstract class NetManager {
        #region Properties
        /// <summary>
        /// If the manager is running as a server,
        /// or a client. Just controls handling extra stuff
        /// is all.
        /// </summary>
        public abstract bool IsServer { get; }

        /// <summary>
        /// The lobby that the manager is connected to.
        /// </summary>
        public NetLobby Lobby { get; protected set; }

        /// <summary>
        /// How many connections are currently active on the manager.
        /// </summary>
        public int ConnectionCount { get { return netManager?.PeersCount ?? 0; } }
        #endregion

        #region Members
        /// <summary>
        /// Allows for listening to the network manager.
        /// </summary>
        public NetMessageListener NetMessageListener;

        /// <summary>
        /// The LiteNetLib network peer.
        /// </summary>
        protected LiteNetLib.NetManager netManager;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new instance of the NetManager. Provides common
        /// functionality between server + client.
        /// </summary>
        /// <param name="settings">Some basic settings to follow.</param>
        protected NetManager(NetManagerSettings settings) {
            NetMessageListener = new NetMessageListener();
            netManager = new LiteNetLib.NetManager(NetMessageListener, NetManagerSettings.ConnectionKey);
            Lobby = new NetLobby(this);
        }
        #endregion

        #region Publics
        /// <summary>
        /// Check to see if any messages have come in from the network.
        /// </summary>
        public void CheckForMessages() {
            if(netManager != null) {
                netManager.PollEvents();
            }
        }

        /// <summary>
        /// Flush the outgoing queue, and send out all waiting messages.
        /// </summary>
        public void SendOutMessages() {
            if (netManager != null) {
                netManager.Flush();
            }
        }
        #endregion

        #region Message Senders
        /// <summary>
        /// Send a message over the network. It encodes the message into
        /// a NetOutGoingMessage through lidgren.
        /// </summary>
        /// <param name="message">The message to send out.</param>
        /// <param name="method">What method to send it over the wire.</param>
        public abstract void SendMessage(NetMessage message, SendOptions method);

        /// <summary>
        /// Send a message to a single client in the lobby.
        /// </summary>
        /// <param name="message">The message to send out.</param>
        /// <param name="target">The peer to send it to.</param>
        /// <param name="method">What method to send it over the wire.</param>
        public void SendMessage(NetMessage message, NetPeer target, SendOptions method) {
            byte[] bytes = message?.Serialize();

            if(bytes != null) {
                target.Send(bytes, method);
            }
            else {
                LoggerUtils.LogWarning("NetManager: Failed to build outgoing message.");
            }
        }
        #endregion
    }
}
