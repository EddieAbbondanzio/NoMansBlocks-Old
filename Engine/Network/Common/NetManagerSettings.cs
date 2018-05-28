using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network {
    /// <summary>
    /// Settings for the NetManager class. Controls
    /// values such as the connection count limit
    /// and more.
    /// </summary>
    public abstract class NetManagerSettings {
        #region Constants
        /// <summary>
        /// The unique key to allow other instances to know that this
        /// is a similar instance.
        /// </summary>
        public const string ConnectionKey = "Voxelated-0.77";
        #endregion

        #region Properties
        /// <summary>
        /// The name to be visible under.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// How many other peers can be connected
        /// to the network manager at once.
        /// </summary>
        public abstract int ConnectionLimit { get; }
        #endregion
    }
}
