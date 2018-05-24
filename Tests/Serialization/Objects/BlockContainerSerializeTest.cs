using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Voxelated.Terrain;
using Voxelated;

namespace VoxelatedTest.Serialization {
    /// <summary>
    /// Tests for block containers and chunks related to converting
    /// their data to bytes and back.
    /// </summary>
    [TestClass]
    public class BlockContainerSerializeTest {
        /// <summary>
        /// Test if a block container converted to bytes
        /// and back is still the same.
        /// </summary>
        [TestMethod]
        public void BlocksSerialzeTest() {
            Vect3Int size = new Vect3Int(4, 4, 4);
            BlockContainer blocks = new BlockContainer(size);
            
            //Set some blocks in it.
            for(int x = 0; x < blocks.Size.X; x++) {
                for(int y = 0; y < blocks.Size.Y; y++) {
                    blocks.SetBlock(x, y, 0, Block.GetColorBlock(Color16.Asbestos));
                }
            }

            //Set some blocks in it.
            for (int x = 0; x < blocks.Size.X; x++) {
                for (int y = 0; y < blocks.Size.Y; y++) {
                    blocks.SetBlock(x, y, 1, Block.GetColorBlock(Color16.Asbestos));
                }
            }

            //Set some blocks in it.
            for (int x = 0; x < blocks.Size.X; x++) {
                for (int y = 0; y < blocks.Size.Y; y++) {
                    blocks.SetBlock(x, y, 1, Block.GetColorBlock(Color16.BelizeHole));
                }
            }

            //Convert it to bytes
            byte[] blockBytes = blocks.Serialize();
        }
    }
}
