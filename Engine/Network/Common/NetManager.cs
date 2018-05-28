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
    /// Base class for server and client network managers. Requires
    /// a set of methods to be implemented in them to allow for abstracting
    /// away from the NetClient and NetServer of lidgren.
    /// </summary>
    public abstract class NetManager {
        #region Events
        /// <summary>
        /// Server Only. Covers message such as new connection, connection leaving
        /// or a client wants to conect.
        /// </summary>
        public static event EventHandler<NetMessageArgs> OnConnectionMessage;

        /// <summary>
        /// When a debug or log message is recieved 
        /// from over the network.
        /// </summary>
        public static event EventHandler<NetMessageArgs> OnInfoMessage;

        /// <summary>
        /// When a lobby message such as player joined or left
        /// is recieved from over the network.
        /// </summary>
        public static event EventHandler<NetMessageArgs> OnLobbyMessage;

        /// <summary>
        /// WHen a chat message is recieved from the network.
        /// </summary>
        public static event EventHandler<NetMessageArgs> OnChatMessage;
        #endregion

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
        #endregion

        #region Members
        /// <summary>
        /// Allows for listening to the network manager.
        /// </summary>
        protected EventBasedNetListener netEventListener;

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
        protected NetManager(INetEventListener netEventListener, NetManagerSettings settings) {
            netManager = new LiteNetLib.NetManager(netEventListener, NetManagerSettings.ConnectionKey);
            netManager.
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
