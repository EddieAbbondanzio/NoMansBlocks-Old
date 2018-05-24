using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Voxelated.Utilities;

namespace Voxelated.Terrain.Generation {
    /// <summary>
    /// Generator to create a desert themed world
    /// </summary>
    public class DesertWorldGenerator : WorldGenerator {
        #region Colors
        /// <summary>
        /// The sand colors to use
        /// </summary>
        public static Color32[] SandColors = new Color32[] {new Color32(240, 230, 140, 255),
                                                            new Color32(238, 232, 170, 255),
                                                            new Color32(228, 217, 125, 255),
                                                            new Color32(225, 216, 107, 255),
                                                            new Color32(216, 207, 115, 255),
                                                            new Color32(228, 216, 107, 255),
                                                            new Color32(242, 230, 124, 255),
                                                            new Color32(239, 226, 108, 255)};
        
        /// <summary>
        /// The cactus colors (green) lol
        /// </summary>
        public static Color32[] CactusColor = new Color32[] { new Color32(47, 124, 51, 255),
                                                              new Color32(34, 102, 38, 255),
                                                              new Color32(40, 104, 43, 255),
                                                              new Color32(42, 117, 45, 255) };
        
        
        #endregion

        #region Properties
        /// <summary>
        /// The type of world it generates
        /// </summary>
        public override WorldType WorldType {
            get {
                return WorldType.Desert;
            }
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new desert world themed world
        /// generator
        /// </summary>
        public DesertWorldGenerator(string worldName) : base(worldName) {

        }
        #endregion

        #region Publics
        /// <summary>
        /// Generate a new desert world
        /// </summary>
        public override WorldContext Generate(int seed = 1337) {
            FastNoise baseHeightNoise = new FastNoise(seed);
            baseHeightNoise.SetNoiseType(FastNoise.NoiseType.Perlin);
            baseHeightNoise.SetFrequency(0.0075f);

            FastNoise margHeightNoise = new FastNoise(seed * 31);
            margHeightNoise.SetNoiseType(FastNoise.NoiseType.Perlin);
            margHeightNoise.SetFrequency(0.014f);

            for (int x = 0; x < WorldSettings.FullBlockSize.X; x++) {
                for (int z = 0; z < WorldSettings.FullBlockSize.Z; z++) {
                    float baseH = (baseHeightNoise.GetNoise(x, z) + 1.0f) / 2.0f;
                    float margH = (margHeightNoise.GetNoise(x, z) + 1.0f) / 2.0f;

                    float elev = MathUtils.Max(baseH, margH);
                    int height = (int)(WorldSettings.FullBlockSize.Y * (elev)) - 10;

                    //This builds the sand terrain
                    for (int y = 0; y < height; y++) {
                        int colorTest = MathUtils.Random.Next(0, 8);
                        worldContext.SetBlock(x,y ,z, Block.GetColorBlock(SandColors[colorTest]));
                    }

                    //Cacti spawner
                    if (MathUtils.Random.Next(1, 600) == 40 && height < 55) {
                        GenerateCactus(x, height, z);
                    }

                    worldContext.SetBlock(x, 0, z, Block.GetColorBlock(Color16.BlackPerl));
                }
            }

            return base.worldContext;
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Generates a small cactus at the world point.
        /// </summary>
        public void GenerateCactus(int x, int y, int z) {
            Color16 color = CactusColor[MathUtils.Random.Next(0, 4)];
            Block cactBlock = Block.GetColorBlock(color);

            //base of it
            worldContext.SetBlock(x, y, z, cactBlock);
            worldContext.SetBlock(x, y + 1, z, cactBlock);
            worldContext.SetBlock(x, y + 2, z, cactBlock);
            worldContext.SetBlock(x, y + 3, z, cactBlock);
            worldContext.SetBlock(x, y + 4, z, cactBlock);
            worldContext.SetBlock(x, y + 5, z, cactBlock);
            worldContext.SetBlock(x, y + 6, z, cactBlock);

            int dir = MathUtils.Random.Next(0, 4);

            switch (dir) {
                case 0:
                    worldContext.SetBlock(x, y + 3, z + 1, cactBlock);
                    worldContext.SetBlock(x, y + 3, z + 2, cactBlock);
                    worldContext.SetBlock(x, y + 4, z + 2, cactBlock);
                    worldContext.SetBlock(x, y + 5, z + 2, cactBlock);
                    worldContext.SetBlock(x, y + 3, z - 1, cactBlock);
                    worldContext.SetBlock(x, y + 3, z - 2, cactBlock);
                    worldContext.SetBlock(x, y + 4, z - 2, cactBlock);
                    worldContext.SetBlock(x, y + 5, z - 2, cactBlock);
                    break;

                case 1:
                    worldContext.SetBlock(x, y + 3, z - 1, cactBlock);
                    worldContext.SetBlock(x, y + 3, z - 2, cactBlock);
                    worldContext.SetBlock(x, y + 4, z - 2, cactBlock);
                    worldContext.SetBlock(x, y + 5, z - 2, cactBlock);
                    break;

                case 2:
                    worldContext.SetBlock(x, y + 3, z - 1, cactBlock);
                    worldContext.SetBlock(x, y + 3, z - 2, cactBlock);
                    worldContext.SetBlock(x, y + 4, z - 2, cactBlock);
                    worldContext.SetBlock(x, y + 5, z - 2, cactBlock);
                    worldContext.SetBlock(x + 1, y + 3, z, cactBlock);
                    worldContext.SetBlock(x + 2, y + 3, z, cactBlock);
                    worldContext.SetBlock(x + 2, y + 4, z, cactBlock);
                    worldContext.SetBlock(x + 2, y + 5, z, cactBlock);
                    break;

                case 3:
                    worldContext.SetBlock(x - 1, y + 3, z, cactBlock);
                    worldContext.SetBlock(x - 2, y + 3, z, cactBlock);
                    worldContext.SetBlock(x - 2, y + 4, z, cactBlock);
                    worldContext.SetBlock(x - 2, y + 5, z, cactBlock);
                    break;
            }


        }
        #endregion
    }
}
