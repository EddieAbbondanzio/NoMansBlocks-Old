using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network {
    /// <summary>
    /// Container for various settings of a server
    /// </summary>
    public struct NetServerSettings {
        #region Statics
        /// <summary>
        /// Default settings for a game
        /// server to use.
        /// </summary>
        public static NetServerSettings Default = new NetServerSettings("Server", 32, NetPermissions.Player);
        #endregion

        #region Properties
        /// <summary>
        /// The name of the server
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// How many players can be in 
        /// the server at once.
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// The permissions level all clients
        /// are given by default.
        /// </summary>
        public NetPermissions DefaultPermissions { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new settings container for a server.
        /// </summary>
        public NetServerSettings(string name, int cap, NetPermissions defaultPerms) {
            Name = name;
            Capacity = cap;
            DefaultPermissions = defaultPerms;
        }
        #endregion
    }
}
