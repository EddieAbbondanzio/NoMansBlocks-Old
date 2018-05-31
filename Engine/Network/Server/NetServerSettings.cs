using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Utilities;

namespace Voxelated.Network {
    /// <summary>
    /// Container for various settings of a server
    /// </summary>
    public sealed class NetServerSettings : NetManagerSettings {
        #region Constants
        /// <summary>
        /// Maximum character length for the name.
        /// </summary>
        public const int NameLengthLimit = 24;

        /// <summary>
        /// Maximum character length for the description.
        /// </summary>
        public const int DescriptionLengthLimit = 176;
        #endregion

        #region Properties
        /// <summary>
        /// The chat name of the server
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

        /// <summary>
        /// The name of the server. Visible to the client.
        /// </summary>
        public string ServerName { get; private set; }

        /// <summary>
        /// The text description of the server.
        /// </summary>
        public string ServerDescription { get; private set; }
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
        /// <param name="serverName">The text name that will be shown to the client when connecting.</param>
        /// <param name="serverDescription">The description that the client will see when joining.</param>
        /// <param name="playerCapacity">How many players can join in.</param>
        /// <param name="defaultPerms">The default permissions level given to new joinees.</param>
        public NetServerSettings(string serverName, string serverDescription, int playerCapacity, NetPermissions defaultPerms) {
            ServerName = StringUtils.Clamp(serverName, NameLengthLimit);
            ServerDescription = StringUtils.Clamp(serverDescription, DescriptionLengthLimit);
            connectionLimit = playerCapacity;
            DefaultPermissions = defaultPerms;
        }
        #endregion
    }
}
