using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network {
    /// <summary>
    /// Interface for communicating across the network
    /// with other clients via test.
    /// </summary>
    public class NetChat {
        #region Properties
        /// <summary>
        /// The name to use for chat.
        /// </summary>
        public string ChatName { get; private set; }
        #endregion
    }
}
