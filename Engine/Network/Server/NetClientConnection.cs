using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Voxelated.Network {
    /// <summary>
    /// Represents the info of a client connected
    /// to the server.
    /// </summary>
    public class NetClientConnection {
        #region Properties
        /// <summary>
        /// The lidgren netconnection of the client
        /// </summary>
        public NetConnection Connection { get; private set; }

        /// <summary>
        /// The permissions level of the client. This
        /// controls what commands / more they have
        /// access to.
        /// </summary>
        public NetPermissions Permissions { get; set; }

        /// <summary>
        /// The player id of the client.
        /// </summary>
        public byte PlayerId { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new  client connection
        /// to store data on a specific connected
        /// client.
        /// </summary>
        public NetClientConnection(NetConnection connection, NetPermissions permissions, byte playerId) {
            Connection = connection;
            Permissions = permissions;
            PlayerId = playerId;
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Convert the connection into a text friendly format.
        /// </summary>
        public override string ToString() {
            return string.Format("{0}: Player Id: {1} Permissions Level: {2}", Connection.RemoteEndPoint.ToString(), PlayerId, Permissions.ToString());
        }
        #endregion
    }
}
