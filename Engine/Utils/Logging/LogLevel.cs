using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Utilities {
    /// <summary>
    /// What level of detail should be outputted regarding
    /// how much to log.
    /// </summary>
    public enum LogLevel : byte {
        Release = 1,
        Debug = 2,
    }
}
