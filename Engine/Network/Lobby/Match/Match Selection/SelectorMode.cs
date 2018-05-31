using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Lobby.Match {
    /// <summary>
    /// Various options that can be used to pick
    /// a game mode + map for a match.
    /// </summary>
    public enum SelectorMode : byte {
        Auto   = 0,
        Vote   = 1,
        Manual = 2,
    }
}
