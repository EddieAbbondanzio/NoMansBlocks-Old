using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Engine.Mesh {
    /// <summary>
    /// The various types of meshes that can be generated.
    /// </summary>
    public enum MeshType : byte {
        Render = 0,
        Collision = 1,
    }
}
