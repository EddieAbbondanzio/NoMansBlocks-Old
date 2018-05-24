using System;
using System.Linq;
using System.Threading;
using Voxelated.Utilities;
using Voxelated.Serialization;
using System.Collections.Generic;
using UnityEngine;
using Voxelated.Events;
using Voxelated.Engine.Mesh;
using Voxelated.Engine.Mesh.Render;

namespace Voxelated.Terrain {
    /// <summary>
    /// This handles all of the visual aspects of the game.
    /// Parent class of all the blocks in the world. The world is divided
    /// into chunks to allow for smaller meshes that can be built seperately vs having
    /// to generate a world mesh all at once.
    /// </summary>
    public class World {
        #region Properties
        /// <summary>
        /// The name of the world. This is typically the
        /// file name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// If the world can be modified or not.
        /// </summary>
        public bool EditingEnabled { get; private set; }
        #endregion

        #region Components
        /// <summary>
        /// Controls updating, and rendering chunks.
        /// </summary>
        public ChunkManager ChunkManager;

        /// <summary>
        /// Handles the building and rendering
        /// of water meshes in the world.
        /// </summary>
        public WaterManager WaterManager;

        /// <summary>
        /// Handles the rigidbodies of the world.
        /// </summary>
        public FragmentManager FragmentManager;

        /// <summary>
        /// The mesh generator of the world.
        /// </summary>
        public MeshGenerator MeshGenerator;

        /// <summary>
        /// Handles loading / saving worlds
        /// </summary>
        public WorldHandler WorldHandler;
        #endregion

        #region Members
        /// <summary>
        /// If the world has been populated or not yet.
        /// </summary>
        private bool isPopulated;

        /// <summary>
        /// Semaphore object for this class.
        /// </summary>
        private readonly object lockObj;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new world that has rendering disabled
        /// </summary>
        public World() {
            //Initialize new properties
            lockObj = new object();
            isPopulated = false;

            ChunkManager = new ChunkManager(this);
            WaterManager = new WaterManager(this);
            MeshGenerator = new MeshGenerator();
            WorldHandler = new WorldHandler();
        }
        #endregion

        #region World Control
        /// <summary>
        /// Perform an update on the entire voxel world.
        /// </summary>
        public void Update() {
            //if (!isPopulated) {
            //    return;
            //}

            //Runs updates on any chunks that need it.
            ChunkManager.Update();
        }

        /// <summary>
        /// Set the editing status of the world.
        /// </summary>
        public void EnableEditing(bool enabled) {
            lock (lockObj) {
                EditingEnabled = enabled;
            }
        }
        #endregion

        #region Block Interaction.
        /// <summary>
        /// Returns the block at world coordinate (x,y,z).
        /// If no block is found it returns an air block.
        /// </summary>
        public Block GetBlock(int x, int y, int z) {
            return ChunkManager.GetBlock(x, y, z);
        }

        /// <summary>
        /// Set the block at world coordinate (x,y,z)
        /// </summary>
        public void SetBlock(int x, int y, int z, Block block) {
            if (!EditingEnabled) {
                return;
            }

            ChunkManager.SetBlock(x, y, z, block);
        }

        /// <summary>
        /// Damage a block in the world by the specific
        /// damage value.
        /// </summary>
        public void DamageBlock(int x, int y, int z, byte damage) {
            throw new NotImplementedException();
        }
        #endregion
    }
}