using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network {
    /// <summary>
    /// Allows for a broader definition of messages. Reduces
    /// how many stupid message handlers are needed.
    /// </summary>
    public enum NetMessageCategory : byte {
        Info       = 0,
        Connection = 1,
        Lobby      = 2,
        Chat       = 3,
        Time       = 4,
    }
}
