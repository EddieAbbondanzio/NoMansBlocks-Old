using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using Voxelated.Engine.Mesh;
using Voxelated.Engine.Tasks;
using Voxelated.Engine.Mesh.Render;
using Voxelated.Utilities;
using Voxelated.Events;
using Voxelated.Serialization;
using System.Diagnostics;

namespace Voxelated.Terrain {
    /// <summary>
    /// Handles loading and updating of the chunks in the world.
    /// This runs a thread pool for generating and rendering meshes.
    /// </summary>
    public class ChunkManager {
        #region Members
        /// <summary>
        /// The world script that owns this
        /// manager.
        /// </summary>
        private World world;

        /// <summary>
        /// The chunks of the world.
        /// Call SetChunk, and GetChunk to access them
        /// in 3d index format.
        /// </summary>
        private Chunk[] chunks;

        /// <summary>
        /// The semaphore lock object.
        /// </summary>
        private readonly object lockObj;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new headless chunk manager.
        /// </summary>
        public ChunkManager(World world) {

            //Store the dimensions info.
            lockObj = new object();
            this.world = world;

            //Create the chunk array
            Vect3Int chunkDims = WorldSettings.FullChunkSize;
            chunks = new Chunk[chunkDims.X * chunkDims.Y * chunkDims.Z];

            //Generate the chunk array, and set the reference to each one.
            for (int x = 0; x < chunkDims.X; x++) {
                for (int y = 0; y < chunkDims.Y; y++) {
                    for (int z = 0; z < chunkDims.Z; z++) {
                        Vect3Int chunkPos = new Vect3Int(x * Chunk.ChunkSize,
                                                         y * Chunk.ChunkSize,
                                                         z * Chunk.ChunkSize);

                        Chunk chunk = new Chunk(this, chunkPos);

                        int chunkIndex = GetChunkIndex(x, y, z);
                        chunks[chunkIndex] = chunk;
                    }
                }
            }

            WorldHandler.OnWorldLoaded += WorldHandler_OnWorldLoaded;
        }

        /// <summary>
        /// When a world is loaded, get the chunks
        /// </summary>
        private void WorldHandler_OnWorldLoaded(object sender, WorldArgs e) {
            WorldContext content = e.WorldData;

            foreach(Chunk chunk in chunks) {
                Block[] blocks = content.GetBlocks(chunk.Position);
                chunk.SetBlocks(blocks);
                chunk.IsModified = true;
            }
        }

        /// <summary>
        /// Runs an update on the chunk manager.
        /// </summary>
        public void Update() {
            if(chunks == null) {
                return;
            }

            foreach (Chunk chunk in chunks) {
                if (chunk.IsModified) {
                    world.MeshGenerator.GenerateMesh(chunk, MeshType.Render);
                    chunk.IsModified = false;
                }
            }
        }
        #endregion

        #region Publics
        /// <summary>
        /// Convert the empty map back to air.
        /// </summary>
        public void Clear() {
            foreach(Chunk chunk in chunks) {
                chunk.Clear();
            }
        }

        /// <summary>
        /// Retrieve the block at the desired world coordinate.
        /// </summary>
        public Block GetBlock(Vect3Int pos) {
            if(MathUtils.InRange(Vect3Int.Zero, WorldSettings.FullBlockSize, pos)) {
                Chunk chunk = GetChunk(pos / Chunk.ChunkSize);
                return chunk.GetBlock(pos % Chunk.ChunkSize);
            }
            else {
                if(pos.Y == WorldSettings.FullBlockSize.Y) {
                    return Block.Air;
                }
                else {
                    return Block.GetColorBlock(Color16.BlackPerl);
                }
            }
        }

        /// <summary>
        /// Retrieve the block at the desired world position.
        /// </summary>
        public Block GetBlock(int x, int y, int z) {
            if (MathUtils.InRange(Vect3Int.Zero, WorldSettings.FullBlockSize, new Vect3Int(x, y, z))) {
                Vect3Int chunkPos = new Vect3Int(x, y, z);
                Vect3Int blockPos = new Vect3Int(x, y, z);

                chunkPos /= Chunk.ChunkSize;
                blockPos %= Chunk.ChunkSize;

                Chunk chunk = GetChunk(chunkPos);
                return chunk.GetBlock(blockPos);
            }
            else {
                if (y == WorldSettings.FullBlockSize.Y) {
                    return Block.Air;
                }
                else {
                    return Block.GetColorBlock(Color16.BlackPerl);
                }
            }
        }

        /// <summary>
        /// Set a single block in the world.
        /// </summary>
        public void SetBlock(Vect3Int pos, Block block) {
            SetBlock(pos.X, pos.Y, pos.Z, block);
        }

        /// <summary>
        /// Set a single block in the world.
        /// </summary>
        public void SetBlock(int x, int y, int z, Block block) {
            if (MathUtils.InRange(Vect3Int.Zero, WorldSettings.FullBlockSize, new Vect3Int(x,y,z))) {
                Vect3Int chunkPos = new Vect3Int(x, y, z);
                Vect3Int blockPos = new Vect3Int(x, y, z);

                chunkPos /= Chunk.ChunkSize;
                blockPos %= Chunk.ChunkSize;

                //Set the block in the chunk
                Chunk chunk = GetChunk(chunkPos);
                chunk.SetBlock(blockPos, block);

                //Check if neighbors need to be updated.
               // UpdateNeighbors(x, y, z);
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Mark neighbors as modified if it's an edge case block
        /// </summary>
        private void UpdateNeighbors(int x, int y, int z) {
            Vect3Int chunkPos = new Vect3Int(x / Chunk.ChunkSize, y / Chunk.ChunkSize, z / Chunk.ChunkSize);
            Vect3Int blockPos = new Vect3Int(x % Chunk.ChunkSize, x % Chunk.ChunkSize, x % Chunk.ChunkSize);

            //Handle X neighbors
            if(blockPos.X == 0) {
                GetChunk(chunkPos.X - 1, chunkPos.Y, chunkPos.Z).IsModified = true;
            } else if(blockPos.X == 31) {
                GetChunk(chunkPos.X + 1, chunkPos.Y, chunkPos.Z).IsModified = true;
            }

            //Handle Y neighbors
            if (blockPos.Y == 0) {
                GetChunk(chunkPos.X, chunkPos.Y - 1, chunkPos.Z).IsModified = true;
            }
            else if (blockPos.Y == 31) {
                GetChunk(chunkPos.X, chunkPos.Y + 1, chunkPos.Z).IsModified = true;
            }

            //Handle Z neighbors
            if (blockPos.Z == 0) {
                GetChunk(chunkPos.X, chunkPos.Y, chunkPos.Z - 1).IsModified = true;
            }
            else if (blockPos.X == 31) {
                GetChunk(chunkPos.X, chunkPos.Y, chunkPos.Z + 1).IsModified = true;
            }
        }

        /// <summary>
        /// Get the chunk at the 3d array index.
        /// </summary>
        private Chunk GetChunk(Vect3Int pos) {
            return GetChunk(pos.X, pos.Y, pos.Z);
        }

        /// <summary>
        /// Get the chunk at the 3d array index.
        /// </summary>
        private Chunk GetChunk(int x, int y, int z) {
            int chunkIndex = GetChunkIndex(x, y, z);

            if(ArrayUtils.ContainsIndex(chunks, chunkIndex)) {
                lock (lockObj) {
                    return chunks[chunkIndex];
                }
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// Set the chunk at the 3d array index.
        /// </summary>
        private void SetChunk(Vect3Int pos, Chunk chunk) {
            SetChunk(pos.X, pos.Y, pos.Z, chunk);
        }

        /// <summary>
        /// Set the chunk at the 3d array index.
        /// </summary>
        private void SetChunk(int x, int y, int z, Chunk chunk) {
            int chunkIndex = GetChunkIndex(x, y, z);

            if (ArrayUtils.ContainsIndex(chunks, chunkIndex)) {
                chunk.Manager = this;
                chunk.IsModified = true;

                lock (lockObj) {
                    chunks[chunkIndex] = chunk;
                }
            }
        }

        /// <summary>
        /// Get the 1d chunk index from a 3d one.
        /// </summary>
        public int GetChunkIndex(int x, int y, int z) {
            Vect3Int chunkDims = WorldSettings.FullChunkSize;
            return (chunkDims.Y * chunkDims.X * z) + (chunkDims.X * y) + x;
        }
        #endregion
    }
}
