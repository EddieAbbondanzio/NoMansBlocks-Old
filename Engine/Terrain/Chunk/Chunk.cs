using System;
using UnityEngine;
using Voxelated.Utilities;
using Voxelated.Engine.Mesh;

namespace Voxelated.Terrain {
    /// <summary>
    /// Chunks are cubic sections of the world. They hold a 3d array of blocks
    /// that are used to generate a mesh from.
    /// </summary>
    public class Chunk : BlockContainer {
        #region Constants
        /// <summary>
        /// How big the chunk is in block dimensions.
        /// </summary>
        public const int ChunkSize = 32;

        /// <summary>
        /// How many blocks are in a chunk
        /// </summary>
        public const int BlockCount = 32 * 32 * 32;

        /// <summary>
        /// The size of the chunk in Vector form
        /// </summary>
        public static readonly Vect3Int ChunkDimensions = new Vect3Int(32, 32, 32);
        #endregion

        #region Properties
        /// <summary>
        /// The key used to find it's render components
        /// </summary>
        public override string RenderKey { get { return "Chunk" + Position.ToString(); } }

        /// <summary>
        /// The position in world space of the chunk
        /// </summary>
        public Vect3Int Position { get; private set; }

        /// <summary>
        /// If the chunk has been modified since it was last rendered.
        /// </summary>
        public bool IsModified {
            get {
                lock (lockObj) {
                    return isModified;
                }
            }
            set {
                lock (lockObj) {
                    isModified = value;
                }
            }
        }

        /// <summary>
        /// The manager of the chunk. Can be null.
        /// </summary>
        public ChunkManager Manager {
            get {
                lock (lockObj) {
                    return chunkManager;
                }
            }
            set {
                lock (lockObj) {
                    chunkManager = value;
                }
            }
        }
        #endregion

        #region Members
        /// <summary>
        /// The chunk manager that owns this chunk.
        /// </summary>
        private ChunkManager chunkManager;

        /// <summary>
        /// If the chunk has been modified since it was last rendered.
        /// </summary>
        private bool isModified;
        #endregion

        #region Constructor(s)
        public Chunk(Vect3Int worldPos) : base(ChunkDimensions) {
            Position = worldPos;
            IsModified = false;
        }

        /// <summary>
        /// Create a new chunk at the world position inputted.
        /// </summary>
        public Chunk(ChunkManager manager, Vect3Int worldPos) : base(ChunkDimensions) {
            chunkManager = manager;
            Position = worldPos;
            isModified = false;
        }

        /// <summary>
        /// Create a new chunk at the world position inputted.
        /// </summary>
        public Chunk(ChunkManager manager, int x, int y, int z) : base(ChunkDimensions) {
            chunkManager = manager;
            Position = new Vect3Int(x,y,z);
            isModified = false;
        }

        public Chunk(byte[] bytes, int startIndex = 0) : base(bytes,startIndex) {

        }
        #endregion

        #region Publics
        /// <summary>
        /// Gets the block at the specified location.
        /// </summary>
        public override Block GetBlock(Vect3Int pos) {
            if(MathUtils.InRange(Vect3Int.Zero, ChunkDimensions, pos)) {
                return base.GetBlock(pos);
            }
            //Not in this chunk. Figure out where to go.
            else {
                return chunkManager.GetBlock(Position + pos);
            }
        }

        /// <summary>
        /// Gets the block at the specified location.
        /// </summary>
        public override Block GetBlock(int x, int y, int z) {
            if (MathUtils.InRange(Vect3Int.Zero, ChunkDimensions, new Vect3Int(x,y,z))) {
                return base.GetBlock(x, y, z);
            }
            //Not in this chunk. Figure out what neighbor to retrieve it from
            else {
                return chunkManager.GetBlock(Position.X + x, Position.Y + y, Position.Z + z);
            }
        }


        /// <summary>
        /// Damage the block at the inputted location.
        /// </summary>
        public void DamageBlock(Vect3Int pos, byte damage) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Damage the block at the inputted location.
        /// </summary>
        public void DamageBlock(int x, int y, int z, byte damage) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the block at the specified chunk position (x,y,z).
        /// If the position is not within this chunk nothing happens.
        /// </summary>
        public override void SetBlock(Vect3Int pos, Block block) {
            if(MathUtils.InRange(Vect3Int.Zero, ChunkDimensions, pos)) {
                base.SetBlock(pos, block);
                isModified = true;
            }
            else {
                chunkManager.SetBlock(Position + pos, block);
            }
        }

        /// <summary>
        /// Sets the block at the specified chunk position (x,y,z).
        /// If the position is not within this chunk nothing happens.
        /// </summary>
        public override void SetBlock(int x, int y, int z, Block block) {
            if (MathUtils.InRange(Vect3Int.Zero, ChunkDimensions, new Vect3Int(x, y, z))) {
                base.SetBlock(x, y, z, block);
                isModified = true;
            }
            else {
                chunkManager.SetBlock(Position.X + x, Position.Y + y, Position.Z + z, block);
            }
        }
        #endregion
    }
}