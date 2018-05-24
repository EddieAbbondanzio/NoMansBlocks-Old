using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxelated.Network {
    /// <summary>
    /// The sequence channel to use for lidgren.
    /// </summary>
    public enum NetChannel : int {
        Lobby = 0,
        Chat = 1,
        Blocks = 2,
    }
}