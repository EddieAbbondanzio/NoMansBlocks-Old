using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Terrain.Generation {
    /// <summary>
    /// Represents a structure that can be generated into
    /// the world by a world generator
    /// </summary>
    public class Prefab {
        #region Properties
        /// <summary>
        /// How large the prefab is in terms of blocks
        /// </summary>
        private Vect3Int Size { get; set; }

        /// <summary>
        /// The blocks of the prefab. These are referenced
        /// via their indexes similar to a GIF. A prefab
        /// can have up to 256 unique block colors. Air
        /// is always set as 0, so colors start at 1.
        /// </summary>
        private Block[] Blocks { get; set; }

        /// <summary>
        /// The structure of the prefab.
        /// </summary>
        public byte[] BluePrint { get; set; }
        #endregion

        #region Constructor(s)
        public Prefab(Block[] blocks, byte[] structure, Vect3Int size) {
            Blocks = blocks;
            BluePrint = structure;
            Size = size;
        }
        #endregion

        #region Publics
        /// <summary>
        /// Convert the prefab into a 3d block array that can be
        /// generated into the world easily
        /// </summary>
        public Block[,,] ToBlocks() {
            Block[,,] blocks = new Block[Size.X, Size.Y, Size.Z];
            int n = 0;

            for(int x = 0; x < Size.X; x++) {
                for(int y = 0; y < Size.Y; y++) {
                    for(int z = 0; z < Size.Z; z++) {
                        Block block = BluePrint[n] == 0 ? Block.Air : Blocks[BluePrint[n]];
                        blocks[x, y, z] = block;
                        n++;
                    }
                }
            }

            return blocks;
        }
        #endregion
    }
}
