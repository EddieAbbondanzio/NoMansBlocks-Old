using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib;

namespace Voxelated.Network.Server {
    /// <summary>
    /// Data structure to hold client connections.
    /// It allows for an O(n) lookup via unique identifier
    /// or player id. It also returns a list of client 
    /// connections instantly.
    /// </summary>
    public class NetClientConnectionList {
        #region Properties
        /// <summary>
        /// All of the NetPeers of the clients currently
        /// in the lobby.
        /// </summary>
        public List<NetPeer> NetPeers {
            get {
                return peers;
            }
        }

        /// <summary>
        /// The number of clients currently in the lobby.
        /// </summary>
        public int Count {
            get {
                return peers.Count;
            }
        }
        #endregion

        #region Members
        /// <summary>
        /// All of the client connections retrievable via their 
        /// connection id.
        /// </summary>
        private Dictionary<long, NetClientConnection> connectionsByUniqueId;

        /// <summary>
        /// All of the client connections retrievable via their
        /// player id.
        /// </summary>
        private Dictionary<byte, NetClientConnection> connectionsByPlayerId;

        /// <summary>
        /// The list of LiteNetLib NetPeers for every client.
        /// </summary>
        private List<NetPeer> peers;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new empty client connection list.
        /// </summary>
        public NetClientConnectionList() {
            connectionsByUniqueId = new Dictionary<long, NetClientConnection>();
            connectionsByPlayerId = new Dictionary<byte, NetClientConnection>();
            peers = new List<NetPeer>();
        }
        #endregion

        #region Publics
        /// <summary>
        /// Add a new client's connection to the connection
        /// list.
        /// </summary>
        /// <param name="connection">The new client's connection.</param>
        public void Add(NetClientConnection connection) {
            //Ensure a connection was passed first.
            if (connection == null) {
                throw new ArgumentNullException("Argument: connection is null!");
            }

            //First ensure it's not already in the list.
            if (!peers.Contains(connection.Peer)) {

                //Then add it.
                connectionsByUniqueId.Add(connection.Peer.ConnectId, connection);
                connectionsByPlayerId.Add(connection.PlayerId, connection);
                peers.Add(connection.Peer);
            }
        }

        /// <summary>
        /// Remove the client connection from the list.
        /// </summary>
        /// <param name="connection">The connection to remove.</param>
        public void Remove(NetClientConnection connection) {
            //Ensure a connection was passed first.
            if (connection == null) {
                throw new ArgumentNullException("Argument: connection is null!");
            }

            //Try to remove it from the list first.
            if (peers.Remove(connection.Peer)) {

                //Then remove it from the dictionaries
                connectionsByUniqueId[connection.Peer.ConnectId] = null;
                connectionsByPlayerId[connection.PlayerId] = null;
            }
        }

        /// <summary>
        /// Checks if the list contains a player with
        /// the following unique id.
        /// </summary>
        /// <param name="id">The id to look for.</param>
        /// <returns>True if a connection with the
        /// unique id exists.</returns>
        public bool ContainsUniqueId(long id) {
            return connectionsByUniqueId.ContainsKey(id);
        }

        /// <summary>
        /// Checks if the list contains a player with
        /// the following player id.
        /// </summary>
        /// <param name="id">The player id to look for.</param>
        /// <returns>True if a connection with the player
        /// id exists in the list.</returns>
        public bool ContainsPlayerId(byte id) {
            return connectionsByPlayerId.ContainsKey(id);
        }

        /// <summary>
        /// Get a client connection via it's unique
        /// remote identifier.
        /// </summary>
        /// <param name="id">The unique id to hunt for.</param>
        /// <returns>The connection with the following id. Null if
        /// not found.</returns>
        public NetClientConnection GetClientByConnectionId(long id) {
            NetClientConnection connection;

            connectionsByUniqueId.TryGetValue(id, out connection);
            return connection;
        }

        /// <summary>
        /// Get a client connection via it's player id.
        /// </summary>
        /// <param name="id">The player id to hunt for.</param>
        /// <returns>The connection with the following id. Null if
        /// not found.</returns>
        public NetClientConnection GetClientByPlayerId(byte id) {
            NetClientConnection connection;

            connectionsByPlayerId.TryGetValue(id, out connection);
            return connection;
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Convert the list into a nice text summary.
        /// </summary>
        public override string ToString() {
            StringBuilder stringBuilder = new StringBuilder();

            foreach(NetClientConnection connection in connectionsByPlayerId.Values) {
                stringBuilder.AppendLine(connection.ToString());
            }

            return stringBuilder.ToString();
        }
        #endregion
    }
}
