using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Terrain.Generation {
    /// <summary>
    /// Generator to create an empty world instance.
    /// </summary>
    public class EmptyWorldGenerator : WorldGenerator {
        #region Properties
        /// <summary>
        /// The type of world it generates
        /// </summary>
        public override WorldType WorldType {
            get {
                return WorldType.Empty;
            }
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new empty world generator.
        /// </summary>
        public EmptyWorldGenerator(string worldName) :base(worldName) {

        }
        #endregion

        #region Publics
        /// <summary>
        /// Generate the new empty world.
        /// </summary>
        /// <returns></returns>
        public override WorldContext Generate(int seed = 1337) {
            for (int x = 0; x < WorldSettings.FullBlockSize.X; x++) {
                for (int z = 0; z < WorldSettings.FullBlockSize.Z; z++) {
                    worldContext.SetBlock(x, 0, z, Block.GetColorBlock(Color16.BlackPerl));
                }
            }

            return base.worldContext;
        }
        #endregion
    }
}
