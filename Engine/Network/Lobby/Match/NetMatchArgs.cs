using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Lobby.Match {
    /// <summary>
    /// Arguments to pass a net match through
    /// an event.
    /// </summary>
    public class NetMatchArgs : EventArgs {
        #region Properties
        /// <summary>
        /// The match being passed.
        /// </summary>
        public NetMatch Match { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new set of arguments 
        /// for a net match.
        /// </summary>
        /// <param name="match">The net match in question.</param>
        public NetMatchArgs(NetMatch match) {
            Match = match;
        }
        #endregion
    }
}
