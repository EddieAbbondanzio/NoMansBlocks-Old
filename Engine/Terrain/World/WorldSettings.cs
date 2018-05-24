using System;
using UnityEngine;
using Voxelated.Terrain;

namespace Voxelated.Terrain {
    /// <summary>
    /// Various settings of the world script.
    /// </summary>
    [Serializable]
    public class WorldSettings {
        //Raycast control
        public const int BlockLayer = 8;
        public const int BlockLayerMask = 1 << BlockLayer;
        public const int FragmentLayer = 10;
        public const int FragmentLayerMask = 1 << FragmentLayer;
        public const int BlockAndFragMask = BlockLayerMask | FragmentLayerMask;

        #region Visual Effects
        /// <summary>
        /// Number of chunks the world mirrors at the edges to 
        /// give the illusion of an infinite world.
        /// </summary>
        public const int BorderChunkSize = 1;

        /// <summary>
        /// Number of chunks the world mirrors at the edges to 
        /// give the illusion of an infinite world.
        /// </summary>
        public const int BorderBlockSize = 32;

        /// <summary>
        /// The y coordinate of the map where water meshes will
        /// be rendered at. This should stay as 1.
        /// </summary>
        public const int WaterLevel = 1;

        #endregion

        #region World Dimensions
        /// <summary>
        /// Playable block dimensions of the world. This is the blocks area in betweem the hologram grids.
        /// </summary>
        public static Vect3Int PlayableBlockSize {
            get {
                return new Vect3Int(512, 64, 512);
            }
        }

        /// <summary>
        /// Playable chunk dimensions of the world. This is how many chunks are in between the hologram grids.
        /// </summary>
        public static Vect3Int PlayableChunkSize {
            get {
                return new Vect3Int(512 / Chunk.ChunkSize,
                                    64 / Chunk.ChunkSize,
                                    512 / Chunk.ChunkSize);
            }
        }

        /// <summary>
        /// Full block dimensions of the world. This is literally every block of the world. Including the blocks that
        /// are outside of the hologram grids. (576, 64, 576)
        /// </summary>
        public static Vect3Int FullBlockSize {
            get {
                return new Vect3Int(512 + (BorderChunkSize * Chunk.ChunkSize * 2),
                                    64,
                                    512 + (BorderChunkSize * Chunk.ChunkSize * 2));
            }
        }

        /// Full chunk dimensions of the world. This is literally every chunk of the world. Including the chunk that
        /// are outside of the hologram grids.
        /// </summary>
        public static Vect3Int FullChunkSize {
            get {
                return new Vect3Int((512 / Chunk.ChunkSize) + (BorderChunkSize * 2),
                                    64 / Chunk.ChunkSize,
                                    (512 / Chunk.ChunkSize) + (BorderChunkSize * 2));
            }
        }

        /// <summary>
        /// The number of chunks located in the world.
        /// </summary>
        public static int FullChunkCount {
            get {
                return FullChunkSize.X * FullChunkSize.Y * FullChunkSize.Z;
            }
        }

        /// <summary>
        /// How many water objects are in the world.
        /// </summary>
        public static Vect2Int FullWaterSize {
            get {
                return new Vect2Int((512 / Water.WaterSize) + (BorderChunkSize),      //This is bad....
                                      (512 / Water.WaterSize) + (BorderChunkSize));
            }
        }
        #endregion
    }
}