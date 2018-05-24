using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Utilities;
using Voxelated.Network.Messages;
using Lidgren.Network;
using Voxelated.Network.Lobby;
using System.Threading;

namespace Voxelated.Network {
    /// <summary>
    /// Handles mantaining all of the connections that are
    /// connected to the server. The client manager does not
    /// need this as it only has 1 connection (to server).
    /// </summary>
    public class NetClientConnectionManager {
        #region Members
        /// <summary>
        /// Reference back to the network mannager
        /// of the server.
        /// </summary>
        private NetServerManager serverManager;

        /// <summary>
        /// Queue of available player ids to use.
        /// </summary>
        private ThreadableQueue<byte> availableIds;

        /// <summary>
        /// The connections of the clients that are currently
        /// connected.
        /// </summary>
        private NetClientConnectionList clientConnections;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// New instance of a client connection manager.
        /// The server uses this to track client connections,
        /// approve new ones, and kick others.
        /// </summary>
        public NetClientConnectionManager() {
            if (!VoxelatedEngine.Engine.NetManager.IsServer) {
                throw new Exception("Only a server can have a net client manager.");
            }

            serverManager = VoxelatedEngine.Engine.NetManager as NetServerManager;
            clientConnections = new NetClientConnectionList();

            //Fill the queue
            availableIds = new ThreadableQueue<byte>();
            for (byte i = 0; i < serverManager.Settings.Capacity; i++) {
                availableIds.Enqueue(i);
            }

            //Subscribe to connection messages
            NetManager.OnConnectionMessage += OnConnectionMessage;
        }

        /// <summary>
        /// Destructor. Called when being disposed.
        /// </summary>
        ~NetClientConnectionManager(){
            NetManager.OnConnectionMessage -= OnConnectionMessage;
        }
        #endregion

        #region Publics
        /// <summary>
        /// Retrieve the player id for the clients connection.
        /// </summary>
        /// <param name="connection">The connection to look for.</param>
        /// <returns>If the client is not found 255 is returned.</returns>
        public byte GetPlayerId(NetConnection connection) {
            return clientConnections.GetConnectionByUniqueId(connection.RemoteUniqueIdentifier)?.PlayerId ?? byte.MaxValue;
        }

        /// <summary>
        /// Retrieve the permissions level for the clients connection.
        /// </summary>
        /// <param name="connection">The clients connection.</param>
        /// <returns>Returns guest if not found.</returns>
        public NetPermissions GetPermissionsLevel(NetConnection connection) {
            return clientConnections.GetConnectionByUniqueId(connection.RemoteUniqueIdentifier)?.Permissions ?? NetPermissions.Guest;
        }

        /// <summary>
        /// Kick a connection from the server.
        /// </summary>
        /// <param name="playerName">The name of the
        /// player to kick.</param>
        public void KickConnectionByPlayerName(string playerName) {
            byte playerId = serverManager.Lobby.GetPlayerIdByName(playerName);
            NetClientConnection connection = clientConnections.GetConnectionByPlayerId(playerId);

            if (connection != null) {
                KickConnection(connection);
            }
        }

        /// <summary>
        /// Kick a connection from the server.
        /// </summary>
        /// <param name="playerId">The player id to look for.</param>
        public void KickConnectionByPlayerId(byte playerId) {
            NetClientConnection connection = clientConnections.GetConnectionByPlayerId(playerId);

            if(connection != null) {
                KickConnection(connection);
            }
        }

        /// <summary>
        /// Kick a connection from the server.
        /// </summary>
        /// <param name="uniqueId">The unique id to look for.</param>
        public void KickConnectionByUniqueId(long uniqueId) {
            NetClientConnection connection = clientConnections.GetConnectionByUniqueId(uniqueId);

            if(connection != null) {
                KickConnection(connection);
            }
        }

        /// <summary>
        /// Get the list of lidgren net connections
        /// for all of the clients currently connected to
        /// the network.
        /// </summary>
        /// <returns>The list of clients currently
        /// in the lobby.</returns>
        public List<NetConnection> GetConnections() {
            return clientConnections.Connections;
        }

        /// <summary>
        /// The number of connections currently in the list.
        /// </summary>
        /// <returns>How many connected clients there are.</returns>
        public int ConnectionCount() {
            return clientConnections.Count;
        }
        #endregion

        #region Message Handler(s)
        /// <summary>
        /// Event handler for anytime a message with
        /// a category of connection is recieved over the
        /// network.
        /// </summary>
        /// <param name="sender">Always null since these come
        /// from the NetMessageProcessor.</param>
        /// <param name="e">The message recieved.</param>
        private void OnConnectionMessage(object sender, NetMessageArgs e) {
            switch (e.Message.Type) {
                //Player wants to join
                case NetMessageType.ConnectionRequest:
                    NetConnection senderConnection = e.Message.SenderConnection;

                    //Check if the new connection is banned or not.
                    if (IsConnectionBanned(senderConnection)) {
                        LoggerUtils.Log("NetConnectionManager: Blocked banned connection at: " + senderConnection.ToString());
                        senderConnection.Deny("You are banned!");
                    }
                    else {
                        senderConnection.Approve();
                    }
                    break;

                //Player has successfully connected
                case NetMessageType.Connect:
                    ProcessConnectMessage(e.Message as ConnectMessage);
                    break;

                //Player has left the lobby
                case NetMessageType.Disconnect:
                    ProcessDisconnectMessage(e.Message as DisconnectMessage);
                    break;

                default:
                    LoggerUtils.LogWarning("Invalid connection message recieved");
                    break;
            }
        }

        /// <summary>
        /// Process a connect message that was recieved from
        /// a client.
        /// </summary>
        /// <param name="connectMsg"></param>
        private void ProcessConnectMessage(ConnectMessage connectMsg) {
            if (connectMsg == null) {
                return;
            }

            byte playerId = availableIds.Dequeue();
            NetClientConnection newConnection = new NetClientConnection(connectMsg.SenderConnection, serverManager.Settings.DefaultPermissions, playerId);

            //Add the new player to the lobby this send an alert to others
            serverManager.Lobby.AddNewClient(newConnection, playerId, connectMsg.Name);

            //Add it to the list of client connections, and give it the lobby
            clientConnections.Add(newConnection);
            LoggerUtils.Log("NetClientConnectionManager: Added Connection: " + newConnection.ToString(), LogLevel.Debug);
        }

        /// <summary>
        /// Process a disconnect message that was recieved
        /// from a client and remove their connection.
        /// </summary>
        /// <param name="disconectMsg">The disconnect message that was
        /// recieved from a client.</param>
        private void ProcessDisconnectMessage(DisconnectMessage disconectMsg) {
            if (disconectMsg == null) {
                return;
            }

            long uniqueId = disconectMsg.SenderConnection.RemoteUniqueIdentifier;

            NetClientConnection connection = clientConnections.GetConnectionByUniqueId(uniqueId);

            //If there connection was not found, they were already kicked.
            if (connection != null) {
                serverManager.Lobby.RemoveClient(connection);

                //Free up the player id, and remove the connection.
                availableIds.Enqueue(connection.PlayerId);
                clientConnections.Remove(connection);

                LoggerUtils.Log("NetClientConnectionManager: Removed Connection: " + connection.ToString(), LogLevel.Debug);
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Disconnect a connection from the server and send out an 
        /// alert to the other connection.s
        /// </summary>
        /// <param name="connection">The connection to kick.</param>
        private void KickConnection(NetClientConnection connection) {
            if (connection != null) {
                LoggerUtils.Log("NetClientConnectionManager: Kicking connection: " + connection.ToString());
                connection.Connection.Disconnect("You were kicked!");

                availableIds.Enqueue(connection.PlayerId);
                clientConnections.Remove(connection);
                serverManager.Lobby.RemoveClient(connection);
            }
        }

        /// <summary>
        /// Checks if the connection is in the ban list.
        /// </summary>
        private bool IsConnectionBanned(NetConnection connection) {
            return false;
        }
        #endregion
    }
}
