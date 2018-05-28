using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network {
    /// <summary>
    /// Container for various settings of a server
    /// </summary>
    public sealed class NetServerSettings : NetManagerSettings {
        #region Properties
        /// <summary>
        /// The name of the server
        /// </summary>
        public override string Name { get { return "Server"; } }

        /// <summary>
        /// The number of players that are allowed to connect
        /// to the server.
        /// </summary>
        public override int ConnectionLimit { get { return connectionLimit; } }

        /// <summary>
        /// The permissions level all clients
        /// are given by default.
        /// </summary>
        public NetPermissions DefaultPermissions { get; private set; }
        #endregion

        #region Members
        /// <summary>
        /// How many clients can be connected to the server.
        /// </summary>
        private int connectionLimit;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new settings list for a server. 
        /// </summary>
        /// <param name="playerCapacity">How many players can join in.</param>
        /// <param name="defaultPerms">The default permissions level given to new joinees.</param>
        public NetServerSettings(int playerCapacity, NetPermissions defaultPerms) {
            connectionLimit = playerCapacity;
            DefaultPermissions = defaultPerms;
        }
        #endregion
    }
}
