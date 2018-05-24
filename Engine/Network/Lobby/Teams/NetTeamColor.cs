using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Lobby {
    /// <summary>
    /// Represents the possible team colors in the game
    /// </summary>
    public enum NetTeamColor : byte {
        None      = 0,
        Red       = 1,
        Blue      = 2,
        Green     = 3,
        Yellow    = 4,
        Spectator = 255
    }
}
