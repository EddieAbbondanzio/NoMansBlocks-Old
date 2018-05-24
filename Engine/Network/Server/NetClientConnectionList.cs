using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network {
    /// <summary>
    /// Data structure to hold client connections.
    /// It allows for an O(n) lookup via unique identifier
    /// or player id. It also returns a list of client 
    /// connections instantly.
    /// </summary>
    public class NetClientConnectionList {
        #region Properties
        /// <summary>
        /// All of the client connections.
        /// </summary>
        public List<NetConnection> Connections {
            get {
                return connections;
            }
        }

        /// <summary>
        /// The number of clients currently in the list.
        /// </summary>
        public int Count {
            get {
                return connections.Count;
            }
        }
        #endregion

        #region Members
        /// <summary>
        /// All of the client connections retrievable via their remote
        /// unique identifier.
        /// </summary>
        private Dictionary<long, NetClientConnection> connectionsByUniqueId;

        /// <summary>
        /// All of the client connections retrievable via their
        /// player id.
        /// </summary>
        private Dictionary<byte, NetClientConnection> connectionsByPlayerId;

        /// <summary>
        /// The collection of lidgren network connections.
        /// </summary>
        private List<NetConnection> connections;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new container to hold client
        /// connections.
        /// </summary>
        public NetClientConnectionList() {
            connectionsByUniqueId = new Dictionary<long, NetClientConnection>();
            connectionsByPlayerId = new Dictionary<byte, NetClientConnection>();
            connections = new List<NetConnection>();
        }
        #endregion

        #region Publics
        /// <summary>
        /// Add a new client to the list.
        /// </summary>
        /// <param name="connection">The client connection to add.</param>
        public void Add(NetClientConnection connection) {
            //Ensure a connection was passed first.
            if (connection == null) {
                throw new ArgumentNullException("Argument: connection is null!");
            }

            //First ensure it's not already in the list.
            if (!connections.Contains(connection.Connection)) {

                //Then add it.
                connectionsByUniqueId.Add(connection.Connection.RemoteUniqueIdentifier, connection);
                connectionsByPlayerId.Add(connection.PlayerId, connection);
                connections.Add(connection.Connection);
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
            if (connections.Remove(connection.Connection)) {

                //Then remove it from the dictionaries
                connectionsByUniqueId[connection.Connection.RemoteUniqueIdentifier] = null;
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
        public NetClientConnection GetConnectionByUniqueId(long id) {
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
        public NetClientConnection GetConnectionByPlayerId(byte id) {
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
