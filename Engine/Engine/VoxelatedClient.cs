using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Engine;
using Voxelated.Engine.Mesh.Render;
using Voxelated.Network;

namespace Voxelated {
    /// <summary>
    /// Client instance of the engine. While it
    /// may not seem very different, new stuff 
    /// will be added to server later.
    /// 4/3/18
    /// </summary>
    public class VoxelatedClient : VoxelatedEngine {
        #region Components
        /// <summary>
        /// The rendering component of the client.
        /// </summary>
        public IMeshRenderer Renderer { get; protected set; }

        /// <summary>
        /// The network manager of the client
        /// </summary>
        public override NetManager NetManager { get; protected set; }
        #endregion

        #region Properties
        /// <summary>
        /// Clients always have a rendering component.
        /// </summary>
        public override bool IsRenderingEnabled {
            get { return true; }
        }

        /// <summary>
        /// Default settings of the client. Will allow these
        /// to be modified later on.
        /// </summary>
        public override VoxelatedSettings Settings {
            get { return VoxelatedSettings.ClientSettings; }
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new instance of a voxelated
        /// engine client with the meshrenderer attached.
        /// </summary>
        public VoxelatedClient(IMeshRenderer meshRenderer) : base () {
            if(meshRenderer == null) {
                throw new ArgumentNullException("Mesh Renderer cannot be null!");
            }

            Renderer = meshRenderer;
            NetManager = new NetClientManager(new NetClientSettings("Bert"));
        }
        #endregion
    }
}
