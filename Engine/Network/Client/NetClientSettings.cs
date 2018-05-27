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
    public class NetClientSettings {
        #region Properties
        /// <summary>
        /// The name to use for the players nickname
        /// </summary>
        public string Name { get; private set; } = string.Empty;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new instance of the settings
        /// for the client.
        /// </summary>
        public NetClientSettings(string name) {
            Name = name;
        }
        #endregion
    }
}
