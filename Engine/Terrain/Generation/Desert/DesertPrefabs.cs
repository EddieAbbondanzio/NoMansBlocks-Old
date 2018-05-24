using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Terrain.Generation {
    /// <summary>
    /// Container class for the collection of possible
    /// desert terrain prefabs.
    /// </summary>
    public static class DesertPrefabs {
        #region Cactus
        //public static Prefab CactusSmall = new Prefab() {
        //    Blocks = new Block[] { new Block(0, 204, 0), new Block(102, 204, 0), new Block(0, 153, 76) },
        //    str
        //};


        static byte[] test2 = { 0, 0, 0,
                                0, 1, 0,
                                0, 0, 0,

                               0, 0, 0,
                               0, 1, 0,
                               0, 0, 0,

                               0, 0, 0,
                               0, 1, 0,
                               0, 1, 0,

        };

        public static Prefab Cactus = new Prefab(new Block[] { Block.GetColorBlock(new Color16(0, 50, 0)) }, test2, new Vect3Int(3, 3, 3));


        //   0 0 0 0 0
        //   0 0 0 0 0 
        //   1 1 1 1 1
        //   0 0 0 0 0 

        //   0 0 0 0 0
        //   0 0 0 0 0 
        //   0 1 1 1 0
        //   0 0 0 0 0
        //

        //public static Prefab test = new Prefab(
        //    new Block[] { new Block(0, 204, 0), new Block(102, 204, 0), new Block(0, 153, 76) },
        //    new byte[,,] { { 0, 0, 0 }, { 0, 0, 0 } }
        //    );

        #endregion

    }
}
