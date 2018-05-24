using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network {
    /// <summary>
    /// Controls access to commands.
    /// </summary>
    public enum NetPermissions : byte {
        Server = 3,
        Trusted = 2,
        Player = 1,
        Guest = 0
    }
}
