using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Lobby.Match {
    /// <summary>
    /// Indicator of how the teams are set up
    /// in the game mode.
    /// </summary>
    public enum TeamMode {
        FreeForAll = 0,
        Dual       = 1,
        Quad       = 2,
    }
}
