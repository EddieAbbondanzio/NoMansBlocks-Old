using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voxelated.Engine.Threading {
    /// <summary>
    /// How often the thread should run
    /// </summary>
    public enum ThreadWorkPriority : byte {
        Low = 40,
        Medium = 20,
        High = 10
    }
}
