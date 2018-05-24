using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelated.Terrain;

namespace Voxelated.Serialization {
    /// <summary>
    /// Interface to represent a world source that has world data in it.
    /// </summary>
    public class WorldContent {
        #region Properties
        /// <summary>
        /// The name of the world
        /// </summary>
        public string WorldName { get; private set; }

        /// <summary>
        /// The water colors of the water mesh
        /// </summary>
        public Color32[] WaterColors {
            get {
                lock (lockObj) {
                    return waterColors.ToArray();
                }
            }
        }

        /// <summary>
        /// The blocks of the world.
        /// </summary>
        public Chunk[] Chunks {
            get {
                lock (lockObj) {
                    return chunks.ToArray();
                }
            }
        }
        #endregion

        #region Members
        /// <summary>
        /// The chunks of the world.
        /// </summary>
        private List<Chunk> chunks;

        /// <summary>
        /// The water colors of the world.
        /// </summary>
        private List<Color32> waterColors;

        /// <summary>
        /// Semaphore lock object.
        /// </summary>
        private readonly object lockObj;
        #endregion

        #region Constructor(s)
        public WorldContent(string name) {
            WorldName = name;
            waterColors = new List<Color32>();
            chunks = new List<Chunk>();

            lockObj = new object();
        }
        #endregion

        #region Publics
        /// <summary>
        /// Add a chunk to the chunks list
        /// </summary>
        public void AddChunk(Chunk chunk) {
            lock (lockObj) {
                chunks.Add(chunk);
            }
        }

        /// <summary>
        /// Generate the water colors via sampling of blocks at the y = 0 level.
        /// </summary>
        public void GenerateWaterColors() {

        }
        #endregion
    }
}
