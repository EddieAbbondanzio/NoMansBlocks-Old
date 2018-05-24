using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Serialization;
using Voxelated.Terrain;

namespace Voxelated.Events {
    /// <summary>
    /// Args for world events
    /// </summary>
    public class WorldArgs : EventArgs {
        #region Properties
        /// <summary>
        /// The content of the world
        /// </summary>
        public WorldContext WorldData { get; set; }
        #endregion

        /// <summary>
        /// Create a new instance of world event args.
        /// </summary>
        public WorldArgs(WorldContext content) {
            WorldData = content;
        }
    }
}
