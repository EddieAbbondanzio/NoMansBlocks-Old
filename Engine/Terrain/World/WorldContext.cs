using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Utilities;

namespace Voxelated.Terrain {
    /// <summary>
    /// This is a way to allow for the world generator
    /// to build worlds that are then passed to the 
    /// world script. It contains all the info such 
    /// as the blocks for each chunk, and the water.
    /// </summary>
    public class WorldContext {
        #region Properties
        /// <summary>
        /// The name of the world
        /// </summary>
        public string Name { get; private set; }
        #endregion

        #region Members
        /// <summary>
        /// The blocks of the world.
        /// </summary>
        private Block[][] blocks;

        /// <summary>
        /// The semaphore lock object.
        /// </summary>
        private readonly object lockObj;
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new world context.
        /// </summary>
        public WorldContext(string name) {
            Name = name;
            blocks = new Block[WorldSettings.FullChunkCount][];
            lockObj = new object();
        }
        #endregion

        #region Publics
        /// <summary>
        /// Set the block at the desired location
        /// </summary>
        public void SetBlock(int x, int y, int z, Block block) {
            if(!MathUtils.InRange(Vect3Int.Zero, WorldSettings.FullBlockSize, new Vect3Int(x, y, z))) {
                return;
            }

            Vect3Int blockPos = new Vect3Int(x % Chunk.ChunkSize,
                                             y % Chunk.ChunkSize,
                                             z % Chunk.ChunkSize);
            Vect3Int chunkPos = new Vect3Int(x / Chunk.ChunkSize,
                                             y / Chunk.ChunkSize,
                                             z / Chunk.ChunkSize);

            int chunkIndex = GetChunkIndex(chunkPos);
            int blockIndex = GetBlockIndex(blockPos);

            //If the block array is null, create it.
            if (blocks[chunkIndex] == null) {
                lock (lockObj) {
                    blocks[chunkIndex] = new Block[Chunk.BlockCount];
                }
            }

            lock (lockObj) {
                blocks[chunkIndex][blockIndex] = block;
            }
        }

        /// <summary>
        /// Set the block at the desired location.
        /// </summary>
        public void SetBlock(Vect3Int pos, Block block) {
            if (!MathUtils.InRange(Vect3Int.Zero, WorldSettings.FullBlockSize, pos)) {
                return;
            }

            Vect3Int blockPos = pos % Chunk.ChunkSize;
            Vect3Int chunkPos = pos / Chunk.ChunkSize;

            int chunkIndex = GetChunkIndex(chunkPos);
            int blockIndex = GetBlockIndex(blockPos);

            //If the block array is null, create it.
            if(blocks[chunkIndex] == null) {
                blocks[chunkIndex] = new Block[Chunk.BlockCount];
            }

            lock (lockObj) {
                blocks[chunkIndex][blockIndex] = block;
            }
        }

        /// <summary>
        /// Get the blocks for the chunk location at
        /// world position (x,y,z);
        /// </summary>
        public Block[] GetBlocks(int x, int y, int z) {
            Vect3Int chunkPos = new Vect3Int(x / Chunk.ChunkSize,
                                             y / Chunk.ChunkSize,
                                             z / Chunk.ChunkSize);

            int chunkIndex = GetChunkIndex(chunkPos);
            lock (lockObj) {
                return blocks[chunkIndex] ?? new Block[Chunk.BlockCount];
            }
        }

        /// <summary>
        /// Get the blocks for the chunk location
        /// at world position pos.
        /// </summary>
        public Block[] GetBlocks(Vect3Int pos) {
            pos /= Chunk.ChunkSize;

            int chunkIndex = GetChunkIndex(pos);
            lock (lockObj) {
                return blocks[chunkIndex] ?? new Block[Chunk.BlockCount];
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Get the 1d block index from it's chunk local position.
        /// </summary>
        private int GetBlockIndex(Vect3Int pos) {
            return (Chunk.ChunkSize * Chunk.ChunkSize * pos.Z) + (Chunk.ChunkSize * pos.Y) + pos.X;
        }

        /// <summary>
        /// Get the 1d chunk index from a 3d one.
        /// </summary>
        private int GetChunkIndex(Vect3Int pos) {
            Vect3Int chunkDims = WorldSettings.FullChunkSize;
            return (chunkDims.Y * chunkDims.X * pos.Z) + (chunkDims.X * pos.Y) + pos.X;
        }
        #endregion
    }
}
