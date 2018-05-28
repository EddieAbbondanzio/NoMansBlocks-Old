using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Utilities;
using Voxelated.Network.Messages;
using Voxelated.Network.Lobby;
using System.Threading;
using LiteNetLib;

namespace Voxelated.Network.Server {
    /// <summary>
    /// Handles mantaining all of the connections that are
    /// connected to the server. The client manager does not
    /// need this as it only has 1 connection (to server).
    /// </summary>
    public class NetServerConnectionHandler {
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
        public NetServerConnectionHandler(NetServerManager serverManager) {
            this.serverManager = serverManager;
            clientConnections = new NetClientConnectionList();

            //Fill the queue
            availableIds = new ThreadableQueue<byte>();
            for (byte i = 0; i < serverManager.Settings.ConnectionLimit; i++) {
                availableIds.Enqueue(i);
            }

            //Subscribe to connection messages
            NetMessageListener.OnConnectionMessage += OnConnectionMessage;
        }

        /// <summary>
        /// Destructor. Called when being disposed.
        /// </summary>
        ~NetServerConnectionHandler(){
            NetMessageListener.OnConnectionMessage -= OnConnectionMessage;
        }
        #endregion

        #region Message Handler(s)
        /// <summary>
        /// Event handler for anytime a message with
        /// a category of connection is recieved over the
        /// network.
        /// </summary>
        /// <param name="sender">The NetMessageListener.</param>
        /// <param name="e">The message recieved.</param>
        private void OnConnectionMessage(object sender, NetMessageArgs e) {
            switch (e.Message?.Type) {
                //Player wants to join
                case NetMessageType.ConnectionRequest:
                    ConnectionRequestMessage connReqMsg = e.Message as ConnectionRequestMessage;

                    if (connReqMsg != null) {
                        NetEndPoint senderAddress = connReqMsg.Sender.EndPoint;

                        //If the connection isn't banned, okay them.
                        if (!IsConnectionBanned(senderAddress)) {
                            ConnectionAcceptedMessage connAcptMsg = new ConnectionAcceptedMessage(serverManager.Settings.ServerName, serverManager.Settings.ServerDescription);
                            serverManager.SendMessage(connAcptMsg, SendOptions.ReliableOrdered);
                        }
                        //Banned connection, reject.
                        else {
                            serverManager.KickClient(connReqMsg.Sender, "You are banned from this server.");
                        }
                    }
                    break;

                //Player has been approved, and sent us a name.
                case NetMessageType.ClientGreeting:
                    ClientGreetingMessage greetMsg = e.Message as ClientGreetingMessage;

                    if (greetMsg != null) {
                        byte playerId = availableIds.Dequeue();
                        NetClientConnection newConnection = new NetClientConnection(greetMsg.Sender, serverManager.Settings.DefaultPermissions, playerId);

                        //Add the new player to the lobby this send an alert to others
                        serverManager.Lobby.AddNewClient(newConnection, playerId, greetMsg.Name);

                        //Add it to the list of client connections, and give it the lobby
                        clientConnections.Add(newConnection);
                        LoggerUtils.Log("NetClientConnectionManager: Added Connection: " + newConnection.ToString(), LogLevel.Debug);
                    }
                    break;

                //Player has left.
                case NetMessageType.Disconnected:
                    DisconnectedMessage disConnMsg = e.Message as DisconnectedMessage;

                    if (disConnMsg != null) {
                        long connId = disConnMsg.Sender.ConnectId;
                        NetClientConnection connection = clientConnections.GetClientByConnectionId(connId);

                        //If there connection was not found, they were already kicked.
                        if (connection != null) {
                            serverManager.Lobby.RemoveClient(connection);

                            //Free up the player id, and remove the connection.
                            availableIds.Enqueue(connection.PlayerId);
                            clientConnections.Remove(connection);

                            LoggerUtils.Log("NetClientConnectionManager: Removed Connection: " + connection.ToString(), LogLevel.Debug);
                        }
                    }
                    break;
            }
        }
        #endregion

        #region Publics
        /// <summary>
        /// Retrieve the player id for the clients connection.
        /// </summary>
        /// <param name="connection">The connection to look for.</param>
        /// <returns>If the client is not found 255 is returned.</returns>
        public byte GetPlayerId(NetPeer connection) {
            return clientConnections.GetClientByConnectionId(connection.ConnectId)?.PlayerId ?? byte.MaxValue;
        }

        /// <summary>
        /// Retrieve the permissions level for the clients connection.
        /// </summary>
        /// <param name="connection">The clients connection.</param>
        /// <returns>Returns guest if not found.</returns>
        public NetPermissions GetPermissionsLevel(NetPeer connection) {
            return clientConnections.GetClientByConnectionId(connection.ConnectId)?.Permissions ?? NetPermissions.Guest;
        }

        /// <summary>
        /// Kick a connection from the server.
        /// </summary>
        /// <param name="playerName">The name of the
        /// player to kick.</param>
        public void KickConnectionByPlayerName(string playerName, string reason) {
            byte playerId = serverManager.Lobby.GetPlayerIdByName(playerName);
            NetClientConnection connection = clientConnections.GetClientByPlayerId(playerId);

            if (connection != null) {
                KickConnection(connection, reason);
            }
        }

        /// <summary>
        /// Kick a connection from the server.
        /// </summary>
        /// <param name="playerId">The player id to look for.</param>
        public void KickConnectionByPlayerId(byte playerId, string reason) {
            NetClientConnection connection = clientConnections.GetClientByPlayerId(playerId);

            if(connection != null) {
                KickConnection(connection, reason);
            }
        }

        /// <summary>
        /// Kick a connection from the server.
        /// </summary>
        /// <param name="uniqueId">The unique id to look for.</param>
        public void KickConnectionByUniqueId(long uniqueId, string reason) {
            NetClientConnection connection = clientConnections.GetClientByConnectionId(uniqueId);

            if(connection != null) {
                KickConnection(connection, reason);
            }
        }

        /// <summary>
        /// Get the list of lidgren net connections
        /// for all of the clients currently connected to
        /// the network.
        /// </summary>
        /// <returns>The list of clients currently
        /// in the lobby.</returns>
        public List<NetPeer> GetConnections() {
            return clientConnections.NetPeers;
        }

        /// <summary>
        /// The number of connections currently in the list.
        /// </summary>
        /// <returns>How many connected clients there are.</returns>
        public int ConnectionCount() {
            return clientConnections.Count;
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Disconnect a connection from the server and send out an 
        /// alert to the other connection.s
        /// </summary>
        /// <param name="connection">The connection to kick.</param>
        /// <param name="reason">The connection to kick.</param>
        private void KickConnection(NetClientConnection connection, string reason) {
            if (connection != null) {
                LoggerUtils.Log("NetClientConnectionManager: Kicking connection: " + connection.ToString());
                serverManager.KickClient(connection.Peer, reason);

                availableIds.Enqueue(connection.PlayerId);
                clientConnections.Remove(connection);
                serverManager.Lobby.RemoveClient(connection);
            }
        }

        /// <summary>
        /// Checks if the connection is in the ban list.
        /// </summary>
        private bool IsConnectionBanned(NetEndPoint ipAddress) {
            return false;
        }
        #endregion
    }
}
