using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelated;
using Voxelated.Terrain;
using NoMansBlocks.Prefab;
using Voxelated.Utilities;

namespace NoMansBlocks.Voxel {
    /// <summary>
    /// Responsible for retrieving the game objects
    /// from the prefab controller and position them correctly.
    /// It also sends the mesh filters + colliders to the
    /// Mesh Handler.
    /// </summary>
    public class ObjectWrangler {
        #region Properties
        /// <summary>
        /// The collection of chunk game objects in use.
        /// </summary>
        public List<GameManager> ChunkObjects { get; private set; }
        #endregion

        #region Publics
        // Use this for initialization
        public void FindChunks() {
            ChunkObjects = new List<GameManager>();

            Vect3Int worldSize = WorldSettings.FullBlockSize;
            for(int x = 0; x < worldSize.X; x += Chunk.ChunkSize) {
                for(int y = 0; y < worldSize.Y; y += Chunk.ChunkSize) {
                    for(int z = 0; z < worldSize.Z; z += Chunk.ChunkSize) {
                        Vect3Int chunkPos = new Vect3Int(x, y, z);

                        GameObject chunkObj = GameManager.PrefabController.GetPooledInstance(PrefabType.Chunk, (Vector3)chunkPos, true);

                        if(chunkObj != null) {
                            MeshFilter chunkFilter = chunkObj.GetComponent<MeshFilter>();
                            MeshCollider chunkCollider = chunkObj.GetComponent<MeshCollider>();

                            if(chunkFilter == null || chunkCollider == null) {
                                LoggerUtils.LogError("ChunkWrangler: Chunk gameobject at " + chunkPos + " is missing a component.");
                                return;
                            }

                            string chunkKey = "Chunk" + chunkPos.ToString();

                            GameManager.MeshHandler.AddMeshFilter(chunkKey, chunkFilter);
                            GameManager.MeshHandler.AddMeshCollider(chunkKey, chunkCollider);
                        }
                    }
                }
            }
        }
        #endregion
    }
}