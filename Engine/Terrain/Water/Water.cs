using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelated.Utilities;
using Voxelated.Engine.Mesh;

namespace Voxelated.Terrain {
    /// <summary>
    /// Parent class that controls the water mesh
    /// colors.
    /// </summary>
    public class Water {
        #region Constants
        /// <summary>
        /// How big the mesh is in block terms
        /// </summary>
        public const int WaterSize = 64;

        /// <summary>
        /// How large a water tile is.
        /// </summary>
        public const int WaterTileSize = 4;
        #endregion

        #region Properties
        /// <summary>
        /// The key used to find it's render components
        /// </summary>
        public string RenderKey { get { return "Water" + Position.ToString(); } }

        /// <summary>
        /// The position in world space of the chunk
        /// </summary>
        public Vect3Int Position { get; private set; }
        #endregion

        #region Members
        /// <summary>
        /// The colors of the tiles in the water mesh.
        /// </summary>
        private Color32[,] waterTiles;

        /// <summary>
        /// Semaphore object to ensure thread safe.
        /// </summary>
        private readonly object lockObj;
        #endregion

        #region Constructor(s)
        public Water(Vect3Int position) {
            lockObj = new object();
            Position = position;
        }
        #endregion

        #region Publics
        /// <summary>
        /// Populate the water mesh with random colors.
        /// </summary>
        public void Populate(Color32[] waterColors) {
            lock (lockObj) {
                //Build the tile color array
                waterTiles = new Color32[WaterSize / WaterTileSize, WaterSize / WaterTileSize];

                for (int x = 0; x < waterTiles.GetLength(0); x++) {
                    for (int z = 0; z < waterTiles.GetLength(1); z++) {
                        waterTiles[x, z] = waterColors[MathUtils.Random.Next(0, waterColors.Length)];
                    }
                }
            }
        }
        #endregion

        #region Rendering
        /// <summary>
        /// Generates the mesh for rendering.
        /// </summary>
        public MeshData GenerateMesh() {
            lock (lockObj) {
                MeshData meshData = new MeshData();

                for (int x = 0; x < waterTiles.GetLength(0); x++) {
                    for (int z = 0; z < waterTiles.GetLength(1); z++) {
                        //Load up tile info
                        Vector3 tilePos = new Vector3(x * WaterTileSize, 0.4f, z * WaterTileSize);
                        Color32 tileColor = waterTiles[x, z];
                        Vect2Int tileSize = new Vect2Int(WaterTileSize - 1, WaterTileSize - 1);

                        //Add it to the mesh
                       // meshData.AddColoredFace(tilePos, tileSize, tileColor, Direction.up);
                    }
                }
                return meshData;
            }
        }
        #endregion

        #region Statics
        /// <summary>
        /// Generate 8 random water colors based on the same.
        /// </summary>
        public static Color32[] GenerateRandomWaterColors(Color32 sample) {
            int variance = 4;

            Color32[] waterColors = new Color32[8];
            waterColors[0] = sample;
            for (int i = 1; i < waterColors.Length; i++) {
                byte r = (byte)Mathf.Min(sample.r + MathUtils.Random.Next(-variance, variance), 255);
                byte g = (byte)Mathf.Min(sample.g + MathUtils.Random.Next(-variance, variance), 255);
                byte b = (byte)Mathf.Min(sample.b + MathUtils.Random.Next(-variance, variance), 255);
                waterColors[i] = new Color32(r, g, b, 255);
            }

            return waterColors;
        }
        #endregion
    }
}