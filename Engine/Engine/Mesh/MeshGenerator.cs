using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Voxelated.Engine.Tasks;
using Voxelated.Engine.Mesh;
using Voxelated.Events;
using Voxelated.Engine.Mesh.Render;
using Voxelated.Terrain;
using Voxelated.Utilities;
using UnityEngine;

namespace Voxelated.Engine.Mesh {
    /// <summary>
    /// Handles performing greedy mesh optimizations on 
    /// voxel meshes.
    /// </summary>
    public class MeshGenerator : ITaskCreator {
        /// <summary>
        /// Represents a mesh task that can be sent 
        /// to the task scheduler for execution.
        /// </summary>
        private class MeshTask : WorkTask {
            #region Properties
            /// <summary>
            /// The type of job.
            /// </summary>
            public override TaskType Type { get { return TaskType.Meshing; } }

            /// <summary>
            /// What kind of mesh to generate
            /// </summary>
            public MeshType MeshType { get; private set; }

            /// <summary>
            /// The outputted mesh.
            /// </summary>
            public MeshData GeneratedMesh { get; private set; }
            #endregion

            #region Members
            /// <summary>
            /// The blocks to generate the mesh from
            /// </summary>
            private BlockContainer blocks;
            #endregion

            #region Constructor(s)
            /// <summary>
            /// Create a new mesh generating task that is to
            /// be sent to the task scheduler
            /// </summary>
            public MeshTask(ITaskCreator creator, BlockContainer blocks, MeshType type) : base (creator) {
                this.blocks = blocks;
                MeshType = type;
            }
            #endregion

            #region Protected
            /// <summary>
            /// Build the mesh and store it.
            /// </summary>
            protected override void Execute() {
                GeneratedMesh = blocks.GreedyMesh(MeshType);
            }
            #endregion
        }

        #region Properties
        /// <summary>
        /// The mesh renderer of the client.
        /// </summary>
        private IMeshRenderer renderer;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new mesh generator
        /// </summary>
        public MeshGenerator() {
            if (VoxelatedEngine.Engine.IsRenderingEnabled) {
                VoxelatedClient client = VoxelatedEngine.Engine as VoxelatedClient;

                if(client != null) {
                    renderer = client.Renderer;
                }
            }
        }
        #endregion

        #region Publics
        /// <summary>
        /// Generate a new mesh
        /// </summary>
        public void GenerateMesh(BlockContainer blocks, MeshType type) {
            MeshTask task = new MeshTask(this, blocks, type);

            //Send it to the task manager.
            TaskScheduler.AddTask(task);
        }

        /// <summary>
        /// Called when one the mesh tasks has been completed
        /// by the task scheduler.
        /// </summary>
        public void TaskCompleted(WorkTask task) {
            if(task.Type == TaskType.Meshing) {
                MeshTask meshTask = task as MeshTask;

                if(meshTask == null) {
                    return;
                }

                MeshData mesh = meshTask.GeneratedMesh;

                //If rendering is enabled (which it should be) send it to the mesh renderer.
                if (mesh != null && renderer != null) {
                    renderer.RenderMesh(mesh);
                }
            }
        }
        #endregion
    }
}
