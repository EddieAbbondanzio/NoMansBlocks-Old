using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Threading;
using Voxelated.Utilities;
using Voxelated;
using Voxelated.Terrain;
using System.Threading.Tasks;

namespace Voxelated.Serialization {
    /// <summary>
    /// File Converter
    /// 
    /// Helper class that handles converting VXL files from the Ace of Spades beta.
    /// When a file is converted it will generate the chunk objects with their blocks and 
    /// send it to the world for loading.
    /// </summary>
    public static class WorldConverter {
        #region Constants
        private const string vxlFileExtension = "vxl";
        private const string vxlFileDirectory = "Vxl";
        #endregion

        #region Members
        /// <summary>
        /// If the block is solid or not.
        /// </summary>
        private static bool[,,] blockType;

        /// <summary>
        /// The colors of the blocks. Air blocks default to black
        /// </summary>
        private static Color32[,,] blockColor;
        #endregion

        #region Publics
        /// <summary>
        /// Convert in a world from .vxl and return
        /// the world load.
        /// </summary>
        public static WorldContext ConvertWorld(string worldName) {
            string fullFileName = worldName + "." + vxlFileExtension;

            //Load file and build blocks array
            byte[] worldBytes = FileUtils.LoadFile(vxlFileDirectory, fullFileName, false);
            WorldContext content = GenerateMapFromVXL(worldName, worldBytes);

            return content;
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Figures out what the average water color from the vxl map is. 
        /// THis should be sent to the world water script to render.
        /// </summary>
        private static Color32[] CalculateAverageWaterColors(BlockContainer blocks) {
            Color32 averageColor = new Color32();
            int workCount = 0;

            for (int x = 0; x < blocks.GetLength(0); x++) {
                for (int z = 0; z < blocks.GetLength(2); z++) {
                    Block blockAbove = blocks[x, 1, z];
                    Block blockCurr = blocks[x, 0, z];

                    //Only proceed if block above is air. This means We have a water block.
                    if (blockAbove.IsAir) {
                        workCount++;

                        //Simple moving average.
                        averageColor.r = (byte)((averageColor.r * (workCount - 1) + blockCurr.Color.R) / workCount);
                        averageColor.g = (byte)((averageColor.g * (workCount - 1) + blockCurr.Color.G) / workCount);
                        averageColor.b = (byte)((averageColor.b * (workCount - 1) + blockCurr.Color.B) / workCount);
                    }
                }
            }

            averageColor.a = 255;
            return Water.GenerateRandomWaterColors(averageColor);
        }

        /// <summary>
        /// Takes the .vxl data and attempts to convert it into a
        /// block array of my format. 
        /// Source: https://silverspaceship.com/aosmap/aos_file_format.html
        /// </summary>
        private static WorldContext GenerateMapFromVXL(string worldName, byte[] vxl) {
            blockType = new bool[512, 512, 64];     //false for air, true for solid
            blockColor = new Color32[512, 512, 64]; //Block color of each block. If air then it is (0,0,0,0)

            //Working variables: n is current index, x,y,z are coordinates.
            int n = 0;
            int x, y, z;

            //Work across map from bottom left to top right.
            for (y = 0; y < 512; y++) {
                for (x = 0; x < 512; x++) {

                    //Set column full of air
                    for (z = 0; z < 64; z++)
                        blockType[x, y, z] = true;

                    z = 0;                                          //Move to top of column
                    while (true) {
                        //UInt32 *color;
                        int i;                                      //Worker variable.
                        int number_4byte_chunks = vxl[n + 0];
                        int top_color_start = vxl[n + 1];
                        int top_color_end = vxl[n + 2];         //inclusive
                        int bottom_color_start;
                        int bottom_color_end;                       //exclusive
                        int len_top;
                        int len_bottom;

                        for (i = z; i < top_color_start; i++)
                            blockType[x, y, i] = false;

                        //Move to start of color run color goes from i to N -1
                        int c = n + 4;

                        //Set top run colors.
                        for (z = top_color_start; z <= top_color_end; z++) {
                            blockColor[x, y, z] = new Color32(vxl[c + 2], vxl[c + 1], vxl[c], 255);
                            c += 4;
                        }

                        len_bottom = top_color_end - top_color_start + 1;

                        // Last span has length = 0 so we need to infer number of 4-byte chunks from the length of the color data
                        if (number_4byte_chunks == 0) {
                            n += 4 * (len_bottom + 1);
                            break;
                        }

                        // infer the number of bottom colors in next span from chunk length and move to next span.
                        len_top = (number_4byte_chunks - 1) - len_bottom;
                        n += vxl[n] * 4;

                        bottom_color_end = vxl[n + 3];
                        bottom_color_start = bottom_color_end - len_top;

                        //Set bottom run colors.
                        for (z = bottom_color_start; z < bottom_color_end; ++z) {
                            blockColor[x, y, z] = new Color32(vxl[c + 2], vxl[c + 1], vxl[c], 255);
                            c += 4;
                        }
                    }
                }
            }

            Vect3Int blockWorldPos = new Vect3Int();
            WorldContext worldContext = new WorldContext(worldName);

            for (x = 0; x < WorldSettings.FullBlockSize.X; x++) {
                for(y = 0; y < WorldSettings.FullBlockSize.Y; y++) {
                    for(z = 0; z < WorldSettings.FullBlockSize.Z; z++) {
                        blockWorldPos = new Vect3Int(x, z, y);
                        Block block = GetBlock(blockWorldPos);

                        //Set the block in the context
                        worldContext.SetBlock(x, y, z, block);
                    }
                }
            }



            ////Create all the chunks of the world content
            //for (int cx = 0; cx < WorldSettings.FullChunkSize.x; cx++) {
            //    for (int cy = 0; cy < WorldSettings.FullChunkSize.y; cy++) {
            //        for (int cz = 0; cz < WorldSettings.FullChunkSize.z; cz++) {
            //            //Create the chunk
            //            Vect3Int chunkPos = new Vect3Int(cx * Chunk.ChunkSize, cy * Chunk.ChunkSize, cz * Chunk.ChunkSize);
            //            Chunk chunk = new Chunk(chunkPos);

            //            //Populate the chunk content
            //            //for (int bx = 0; bx < Chunk.ChunkSize; bx++) {
            //            //    for (int by = 0; by < Chunk.ChunkSize; by++) {
            //            //        for (int bz = 0; bz < Chunk.ChunkSize; bz++) {
            //            //            blockLocalPos = new Vect3Int(bx, by, bz);
            //            //            blockWorldPos = new Vect3Int(chunkPos.x + bx, chunkPos.z + bz, chunkPos.y + by);

            //            //            Block block = GetBlock(blockWorldPos);
            //            //            chunk.SetBlock(blockLocalPos, block);
            //            //        }
            //            //    }
            //            //}

            //            //Add it to the world content
            //           // worldContent.AddChunk(chunk);
            //        }
            //    }
            //}
           
            //Clear the work vars
            blockType = null;
            blockColor = null;

            //Dat final touch. Sha sha..
            //worldContent.GenerateWaterColors();
            return worldContext;
        }

        /// <summary>
        /// Get the block at the world position.
        /// </summary>
        private static Block GetBlock(Vect3Int pos) {
            //Prevent invalid access.
            if(blockType == null || blockColor == null) {
                LoggerUtils.LogError("WorldConverter: Error: no block data present.");
                return Block.Air;
            }

            //Perform back magic shit
            Vect3Int blockPos = GetBlockPos(pos.X, pos.Y, pos.Z);

            //Solid case
            if (blockType[blockPos.X, blockPos.Y, blockPos.Z]) {
                Color32 currColor = blockColor[blockPos.X, blockPos.Y, blockPos.Z];

                return Block.GetColorBlock(currColor, (byte)(pos.Y == 0 ? 7 : 6));
            }
            else {
                return Block.Air;
            }
        }

        /// <summary>
        /// Generates position to handle mirroring edges.
        /// </summary>
        private static Vect3Int GetBlockPos(int x, int y, int z) {
            if(x < 32) {
                x = 31 - x;
            }
            else if(x >= 512) {
                x = 511 - (x - 511);
            }
            else {
                x = x - 32;
            }

            if(y < 32) {
                y = 31 - y;
            }
            else if(y >= 512) {
                y = 511 - (y - 511);
            }
            else {
                y = y - 32;
            }

            z = 63 - z;

            return new Vect3Int(x, y,z);
        }
        #endregion
    }
}