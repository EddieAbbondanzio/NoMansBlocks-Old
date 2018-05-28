using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network {
    /// <summary>
    /// Right now this only has name
    /// but will be expanded upon more
    /// later on.
    /// </summary>
    public sealed class NetClientSettings : NetManagerSettings {
        #region Properties
        /// <summary>
        /// The name to use for the players nickname
        /// </summary>
        public override string Name { get { return name; } }

        /// <summary>
        /// Clients only allow for 1 connection (the server).
        /// </summary>
        public override int ConnectionLimit { get { return 1; } }
        #endregion

        #region Members
        /// <summary>
        /// The name of the client. Other players
        /// will see this.
        /// </summary>
        private string name;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Settings to use for the network client.
        /// </summary>
        /// <param name="name">The name to play under.</param>
        public NetClientSettings(string name) {
            this.name = name;
        }
        #endregion
    }
}
