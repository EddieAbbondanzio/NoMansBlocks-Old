using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Utilities {
    /// <summary>
    /// What output format should be used for
    /// writing log statements
    /// </summary>
    public enum LogOutput : byte {
        Unity = 0,
        Console = 1,
        FileOnly = 2
    }
}
