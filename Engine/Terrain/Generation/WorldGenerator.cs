using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelated;
using Voxelated.Terrain;
using System.Threading.Tasks;
using Voxelated.Utilities;

namespace Voxelated.Terrain.Generation {
    /// <summary>
    /// World generator that is capable of building several different
    /// world styles.
    /// </summary>
    public abstract class WorldGenerator {
        #region Properties
        /// <summary>
        /// The type of world that the generator creates
        /// </summary>
        public abstract WorldType WorldType { get; }
        #endregion

        #region Members
        /// <summary>
        /// The world being generated.
        /// </summary>
        protected WorldContext worldContext;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new world generator for world
        /// with the name worldName.
        /// </summary>
        public WorldGenerator(string worldName) {
            worldContext = new WorldContext(worldName);
        }
        #endregion

        #region Publics
        /// <summary>
        /// Generate the world, and return it.
        /// </summary>
        public abstract WorldContext Generate(int seed = 1337);
        #endregion

        #region Helpers
        /// <summary>
        /// Build a prefab in the world with it's left
        /// corner located at world position (x,y,z)
        /// </summary>
        protected void BuildPrefab(int x, int y, int z, Prefab prefab) {
            Block[,,] prefabBlocks = prefab.ToBlocks();

            for(int px = 0; px < prefabBlocks.GetLength(0); px++) {
                for(int py = 0; py < prefabBlocks.GetLength(1); py++) {
                    for(int pz = 0; pz < prefabBlocks.GetLength(2); pz++) {
                        worldContext.SetBlock(x + px, y + py, z + pz, prefabBlocks[px, py, pz]);
                    }
                }
            }
        }
        #endregion

        #region Statics
        /// <summary>
        /// Generate an empty world for building in.
        /// </summary>
        public static WorldContext GenerateWorld(WorldType type, string worldName) {
            LoggerUtils.Log("WorldGenerator: Generating a " + type.ToString() + " world");
            WorldContext context = null;

            //The different options.
            switch (type) {
                case WorldType.Empty:
                    context = new EmptyWorldGenerator(worldName).Generate();
                    break;
                case WorldType.Desert:
                    context = new DesertWorldGenerator(worldName).Generate();
                    break;
            }

            return context;
        }
        #endregion
    }
}