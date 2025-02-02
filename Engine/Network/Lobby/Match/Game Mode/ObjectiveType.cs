﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Lobby.Match {
    /// <summary>
    /// The supported game modes of the engine.
    /// </summary>
    [Flags]
    public enum ObjectiveType : byte {
        Deathmatch     = 1,
        CaptureTheFlag = 2,
        Demolition     = 4,
    }
}
