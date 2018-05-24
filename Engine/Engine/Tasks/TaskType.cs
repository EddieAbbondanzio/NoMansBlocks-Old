using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Engine.Tasks {
    /// <summary>
    /// Represents what kind of task it is.
    /// </summary>
    public enum TaskType : byte {
        Meshing = 0,
        SaveWorld = 1,
        LoadWorld = 2,
    }
}
