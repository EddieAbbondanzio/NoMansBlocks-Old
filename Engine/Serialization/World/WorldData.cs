//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Voxelated.Terrain;

//namespace Voxelated.Serialization {
//    /// <summary>
//    /// Useful class to serialize parts or all of a world. This is used for saving and loading
//    /// and transmitting data over the network.
//    /// </summary>
//    public class WorldData : ISerializable {

//        /// <summary>
//        /// Offset of the worldContent. This is in chunk values
//        /// </summary>
//        public Vect2Int Position { get; private set; }

//        /// <summary>
//        /// Compressed byte arrays of the chunks in the world.
//        /// </summary>
//        public WorldColumn[,] WorldColumns { get; private set; }

//        public WorldData(World world, Vect2Int size, Vect2Int pos) {
//            this.Position = pos;
//            this.WorldColumns = ColumnizeWorld(world, size, pos);
//        }

//        public WorldData(WorldColumn[,] columns, Vect2Int pos) {
//            this.WorldColumns = columns;
//            this.Position = pos;
//        }

//        /// <summary>
//        /// Convert the world into the world columns that represent it.
//        /// </summary>
//        private WorldColumn[,] ColumnizeWorld(World world, Vect2Int size, Vect2Int pos) {
//            WorldColumn[,] columns = new WorldColumn[size.x * Chunk.ChunkSize, size.y * Chunk.ChunkSize];

//            //Build each new worldColumn and save it to this.
//            for (int x = 0; x < columns.GetLength(0); x++) {
//                for (int z = 0; z < columns.GetLength(1); z++) {
//                    //Actual column position in the world.
//                    Vect2Int worldPos = new Vect2Int((pos.x * Chunk.ChunkSize) + x, (pos.y * Chunk.ChunkSize) + z);

//                    //Get current blocks and build a new column.
//                    Block[] blocks = GetBlocksInWorldAt(world, (int) worldPos.x, (int)worldPos.y);
//                    WorldColumn currColumn = new WorldColumn(blocks);

//                    columns[x, z] = currColumn;
//                }
//            }
//            return columns;
//        }

//        /// <summary>
//        /// Retrieves the y coordinate of blocks at the position x,z in the map.
//        /// </summary>
//        private Block[] GetBlocksInWorldAt(World world, int x, int z) {
//            Block[] blocks = new Block[WorldSettings.FullBlockSize.y];

//            //Ignore blocks at y == 0 of world.
//            for (int b = 0; b < blocks.Length; b++) {
//                blocks[b] = world.GetBlock(x, b, z);
//            }

//            return blocks;
//        }

//        /// <summary>
//        /// Check if the desired coordinate is actually valid in the array
//        /// </summary>
//        private bool IsValidColumnIndex(WorldColumn[,] columns, int x, int z) {
//            if (x >= 0 && x < columns.GetLength(0) &&
//                z >= 0 && z < columns.GetLength(1))
//                return true;
//            else
//                return false;
//        }

//        /// <summary>
//        /// Convert all the blocks in the world into a byte array
//        /// </summary>
//        public byte[] Serialize() {
//            List<byte> blockBytes = new List<byte>();

//            blockBytes.Add((byte)(WorldColumns.GetLength(0) / Chunk.ChunkSize));
//            blockBytes.Add((byte)(WorldColumns.GetLength(1) / Chunk.ChunkSize));

//            blockBytes.Add((byte)Position.x);
//            blockBytes.Add((byte)Position.y);

//            //Store the world columns.
//            for (int x = 0; x < WorldColumns.GetLength(0); x++) {
//                for (int z = 0; z < WorldColumns.GetLength(1); z++) {
//                    blockBytes.AddRange(WorldColumns[x,z].Serialize());
//                }
//            }

//            return blockBytes.ToArray();
//        }

//        /// <summary>
//        /// Rebuild the world file from the byte array that was loaded in from file.
//        /// </summary>
//        public static WorldData BuildFromSerializedData(byte[] bytes) {
//            //Load World size data
//            Vect2Int size = new Vect2Int(bytes[0], bytes[1]);
//            Vect2Int pos = new Vect2Int(bytes[2], bytes[3]);
//            WorldColumn[,] columns = new WorldColumn[size.x * Chunk.ChunkSize, size.y * Chunk.ChunkSize];

//            //Work through each byte
//            int currByte = 4;
//            int currCol = 0;

//            //While there are still columns to visit.
//            while(currCol < columns.GetLength(0) * columns.GetLength(1)) {
//                //Coordinate of the column
//                int colX = currCol / columns.GetLength(0);
//                int colZ = currCol % columns.GetLength(0);

//                //Prep the column
//                WorldColumn column = new WorldColumn(bytes[currByte]);
//                currByte++;

//                //Has intervals that need to be loaded.
//                if (!column.IsAir) {
//                    //Work through each interval.
//                    for(int i = 0; i < column.IntervalCount; i++) {
//                        byte intInfo = bytes[currByte];
//                        currByte++;

//                        byte intLength = WorldInterval.IntervalLength(intInfo);

//                        if (WorldInterval.IsAirInterval(intInfo)) {
//                            column.Intervals[i] = new WorldInterval(Block.Air, intLength);
//                        }
//                        else {
//                            byte[] intBlock = new byte[] { bytes[currByte], bytes[currByte + 1], bytes[currByte + 2], bytes[currByte + 3] };
//                            column.Intervals[i] = new WorldInterval(new Block(intBlock), intLength);

//                            currByte += 4;
//                        }
//                    }
//                }

//                columns[colX, colZ] = column;
//                currCol++;
//            }

//            return new WorldData(columns, pos);
//        }

//        /// <summary>
//        /// Convert the world content int oa block Container
//        /// for loading the world.
//        /// </summary>
//        public BlockContainer ToBlocks() {
//          //  BlockContainer blocks = new BlockContainer(WorldSettings.FullBlockSize);

//            for (int x = 0; x < WorldColumns.GetLength(0); x++) {
//                for (int z = 0; z < WorldColumns.GetLength(1); z++) {
//                    WorldColumn column = WorldColumns[x, z];
//                    Block[] colBlocks = column.ToBlocks();

//                    //Iterate through all the blocks in the column and add it to the container.
//                    for (int y = 0; y < colBlocks.Length; y++) {
//                        VoxelatedEngine.Instance.World.SetBlock(x, y, z, colBlocks[y]);
//                        //blocks[x, y, z] = y == 0 ? Block.Bedrock : colBlocks[y];
//                    }
//                }
//            }

//            return null;
//        }
//    }
//}