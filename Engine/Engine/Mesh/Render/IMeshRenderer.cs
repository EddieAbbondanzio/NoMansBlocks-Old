using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Voxelated.Engine.Mesh;

namespace Voxelated.Engine.Mesh.Render {
    public interface IMeshRenderer {
        /// <summary>
        /// Render the mesh to the screen.
        /// </summary>
        void RenderMesh(MeshData mesh);
    }
}
