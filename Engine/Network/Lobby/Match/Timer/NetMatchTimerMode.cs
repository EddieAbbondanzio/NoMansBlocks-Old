using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Lobby.Match {
    /// <summary>
    /// The possible options that a NetMatch timer can follow.
    /// For a standard count down time limit use CountDown. For
    /// a match that never ends up Inifinite. This will simply
    /// count up and display how long the match has been
    /// going on for.
    /// </summary>
    public enum NetMatchTimerMode : byte {
        CountDown = 0,
        Infinite = 1,
    }
}
