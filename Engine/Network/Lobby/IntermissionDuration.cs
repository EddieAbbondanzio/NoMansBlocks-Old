using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Lobby {
    /// <summary>
    /// How long intermission is between matches.
    /// </summary>
    public enum IntermissionDuration : byte {
        Short    = 30,
        Normal   = 60,
        Long     = 90,
        Infinite = 255,
    }
}
