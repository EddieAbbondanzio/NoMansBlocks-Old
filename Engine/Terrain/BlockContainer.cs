using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Voxelated.Engine.Mesh;
using Voxelated.Engine.Mesh.Render;
using Voxelated.Serialization;
using Voxelated.Utilities;

namespace Voxelated.Terrain {
    /// <summary>
    /// 3D grid collection of blocks in the world. This provides some basic
    /// functionality such as QuickMesh, GreedyMesh, and serializing to and
    /// from bytes.
    /// </summary>
    public class BlockContainer  {
        #region Properties
        /// <summary>
        /// The 3D dimensions of the container.
        /// </summary>
        public Vect3Int Size { get; private set; }

        /// <summary>
        /// The key used to locate it's Unity mesh components.
        /// </summary>
        public virtual string RenderKey { get; }


        /// <summary>
        /// Set / Get the block at the inputted location. In the event an out
        /// of bounds access is made a default value of an air block will be
        /// returned.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        /// <returns>The block at location (x,y,z) in the container.</returns>
        public Block this[int x, int y, int z] {
            get {
                return GetBlock(x, y, z);
            }
            set {
                SetBlock(x, y, z, value);
            }
        }
        #endregion

        #region Members
        /// <summary>
        /// A flat 3D array of the blocks in the container.
        /// </summary>
        private Block[] blocks;

        /// <summary>
        /// Multi-threading semaphore lock object.
        /// </summary>
        protected readonly object lockObj;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new BlockContaienr of the specified
        /// size. This leaves RenderKey as null.
        /// </summary>
        /// <param name="size">The block dimensions of the container.</param>
        public BlockContainer(Vect3Int size) {
            Size    = size;
            lockObj = new object();
        }

        /// <summary>
        /// Create a new BlockContaienr of the specified
        /// size. This leaves RenderKey as null.
        /// </summary>
        /// <param name="x">The x coordinate size.</param>
        /// <param name="y">The y coordinate size.</param>
        /// <param name="z">The z coordiante size.</param>
        public BlockContainer(int x, int y, int z) {
            Size    = new Vect3Int(x, y, z);
            lockObj = new object();
        }

        /// <summary>
        /// Reconstruct a block container from it's serialized
        /// byte array.
        /// </summary>
        /// <param name="bytes">The bytes to extract info from.</param>
        /// <param name="startIndex">The first byte location of the contaner.</param>
        public BlockContainer(byte[] bytes, int startIndex = 0) {
            if(bytes == null) {
                throw new ArgumentNullException("Argument bytes cannot be null!");
            }

            Size   = new Vect3Int(bytes[0], bytes[1], bytes[2]);
            blocks = new Block[Size.X * Size.Y * Size.Z];
        }

        #endregion

        #region Publics
        /// <summary>
        /// Replace every block in the container with
        /// an air block.
        /// </summary>
        public void Clear() {
            lock (lockObj) {
                blocks = null;
            }
        }

        /// <summary>
        /// Set the block contents of the container. If this doesn't match the
        /// container size an exception will be thrown.
        /// </summary>
        /// <param name="blocks">The blocks to set within the container.</param>
        public void SetBlocks(Block[] blocks) {
            int currSize = Size.X * Size.Y * Size.Z;

            //Check to ensure that the blocks are the same size as the container.
            if(currSize != blocks.Length) {
                throw new ArgumentException("Block Array must be of the same size as the container");
            }

            lock (lockObj) {
                this.blocks = blocks;
            }
        }

        /// <summary>
        /// Retrieve the block dimension of the desired axis.
        /// </summary>
        /// <param name="i">The axis to check, x: 0, y: 1, z: 2</param>
        /// <returns>The number of blocks in that dimension.</returns>
        public int GetLength(int i) {
            if(i >= 0 && i < 3) {
                return Size[i];
            }
            else {
                throw new ArgumentException("Invalid index access");
            }
        }

        /// <summary>
        /// Set the block at the desired location. If not withins
        /// the bound of the container the function does nothing.
        /// </summary>
        /// <param name="pos">The blocks position.</param>
        /// <param name="block">The block value to set.</param>
        public virtual void SetBlock(Vect3Int pos, Block block) {
            //If the container is only air, it stays null.
            if(blocks == null && !block.IsAir) {
                blocks = new Block[Size.X * Size.Y * Size.Z];
            }

            //Get the flattened 3d index.
            int blockIndex = GetBlockIndex(pos.X, pos.Y, pos.Z);

            //Ensure the array has the position first.
            if (ArrayUtils.ContainsIndex(blocks, blockIndex)) {
                lock (lockObj) {
                    blocks[blockIndex] = block;
                }
            }
        }

        /// <summary>
        /// Set the block at the desired location. If not withins
        /// the bound of the container the function does nothing.
        /// </summary>
        /// <param name="x">The x coordinate of the block.</param>
        /// <param name="y">The y coordinate of the block.</param>
        /// <param name="z">The z coordinate of the block.</param>
        /// <param name="block">The block value to set.</param>
        public virtual void SetBlock(int x, int y, int z, Block block) {
            //If the container is only air, it stays null.
            if(blocks == null && !block.IsAir) {
                blocks = new Block[Size.X * Size.Y * Size.Z];
            }
         
            //Get the flattened 3d index.
            int blockIndex = GetBlockIndex(x, y, z);

            //Ensure the array has the position first.
            if (ArrayUtils.ContainsIndex(blocks, blockIndex)) {
                lock (lockObj) {
                    blocks[blockIndex] = block;
                }
            }
        }

        /// <summary>
        /// Retrieves the block at the desired position. If the position
        /// is out of bounds a default value of an air block is returned.
        /// </summary>
        /// <param name="pos">The position to get the block from.</param>
        /// <returns>The block at the specified position.</returns>
        public virtual Block GetBlock(Vect3Int pos) {
            //Chunk is still full of air, stop.
            if(blocks == null) {
                return Block.Air;
            }

            //Check its within bounds first
            if(MathUtils.InRange(Vect3Int.Zero, Size, pos)) {
                //Get the flattened 3d array index.
                int blockIndex = GetBlockIndex(pos);

                lock (lockObj) {
                    return blocks[blockIndex];
                }
            }
            else {
                return Block.Air;
            }
        }

        /// <summary>
        /// Retrieves the block at the desired position. If the position
        /// is out of bounds a default value of an air block is returned.
        /// </summary>
        /// <param name="x">The x coordinate of the block.</param>
        /// <param name="y">The y coordinate of the block.</param>
        /// <param name="z">The z coordinate of the block.</param>
        /// <returns>The block at location (x,y,z).</returns>
        public virtual Block GetBlock(int x, int y, int z) {
            //Container is full of air. Stop.
            if (blocks == null) {
                return Block.Air;
            }
            
            //Check the position is actually within the container.
            if (MathUtils.InRange(Vect3Int.Zero, Size, new Vect3Int(x, y, z))){

                //Get the flattened 3d array index.
                int blockIndex = GetBlockIndex(x, y, z);
                lock (lockObj) {
                    return blocks[blockIndex];
                }
            }
            else {
                return Block.Air;
            }
        }

        /// <summary>
        /// Generate a fast mesh that only culls hidden faces. Try to call
        /// GreedyMesh() instead as it's more efficient, but this method trades off
        /// quality for less time.
        /// </summary>
        /// <param name="type">If it is a render mesh, or collision mesh.</param>
        /// <returns>The meshdata of the mesh generated.</returns>
        public MeshData QuickMesh(MeshType type) {
            //Check that the container has a render key before wasting cpu resources.
            if(RenderKey == null) {
                throw new InvalidOperationException("Render Key must be defined before a mesh can be generated!");
            }

            //Create some of the work vars
            MeshData mesh = new MeshData(RenderKey);
            Block    currBlock;
            Vect3Int currPos;

            //Just air in it.
            if(blocks == null) {
                return mesh;
            }

            //Visit every block 6 times (once for each face).
            for(int d = 0; d < 6; d++) {
                for (int x = 0; x < Size.X; x++) {
                    for (int y = 0; y < Size.Y; y++) {
                        for (int z = 0; z < Size.Z; z++) {
                            currPos = new Vect3Int(x, y, z);
                            currBlock = GetBlock(currPos);
                            
                            if(currBlock.Type == BlockType.Sprite) {
                                mesh.AddSprite((Vector3)currPos, currBlock.MetaData);
                            }
                            else if(IsBlockFaceVisible(x,y,z, (Direction)d)) {
                                mesh.AddColoredFace((Vector3)currPos, Vect2Int.Zero, currBlock.Color, (Direction)d);
                            }
                        }
                    }
                }
            }

            return mesh;
        }

        /// <summary>
        /// Generates an optimized mesh using a greedy mesh algorithm that
        /// combies faces of adjancet blocks that are identical in color.
        /// This is derived from 0FPS's algorithm.
        /// https://github.com/roboleary/GreedyMesh/blob/master/src/mygame/Main.java
        /// </summary>
        /// <param name="type">If it is a render mesh, or collision mesh.</param>
        /// <returns>The meshdata of the mesh generated.</returns>
        public MeshData GreedyMesh(MeshType type) {
            //Check that the container has a render key before wasting cpu resources.
            if (RenderKey == null) {
                throw new InvalidOperationException("Render Key must be defined before a mesh can be generated!");
            }

            MeshData mesh = new MeshData(RenderKey);

            //Just air in it.
            if (blocks == null) {
                return mesh;
            }

            //This tracks if we merged the blocks in the slice.
            bool[,] merged;

            //Work vars
            Vect3Int a, b, q, m, n, o;
            Vector3[] verts;
            int i, j;

            Block startBlock;

            //This is just beautiful. It counts 0-2 then 0-2 again. The second run through back is true.
            bool back = false;
            for (int d = 0, t = 0; d < 3 && t <= 1; t += d == 2 ? 1 : 0, d += d == 2 ? -2 : 1, back = t > 0) {
                i = (d + 1) % 3;
                j = (d + 2) % 3;

                a = new Vect3Int();
                b = new Vect3Int();

                //This is the axis we will slice on
                for (a[d] = 0; a[d] < Size[d]; a[d]++) {
                    merged = new bool[Size[i], Size[j]];

                    //These are dem slices we're building
                    for (a[i] = 0; a[i] < Size[i]; a[i]++) {
                        for (a[j] = 0; a[j] < Size[j]; a[j]++) {
                            startBlock = GetBlock(a);

                            //If its a sprite block add it to the mesh.
                            if (startBlock.Type == BlockType.Sprite) {
                                mesh.AddSprite((Vector3)a, startBlock.MetaData);
                                merged[a[i], a[j]] = true;
                            }

                            //If this block has already been merged, is air, or not visible skip it.
                            if (merged[a[i], a[j]] || !startBlock.IsSolid || !IsBlockFaceVisible(a, d, back)) {
                                continue;
                            }

                            //Reset the work var
                            q = new Vect3Int();

                            //Figure out the width, then save it
                            for (b = a, b[j]++; b[j] < Size[j] && CompareStep(a, b, d, back, type) && !merged[b[i], b[j]]; b[j]++) { }
                            q[j] = b[j] - a[j];

                            //Figure out the height, then save it
                            for (b = a, b[i]++; b[i] < Size[i] && CompareStep(a, b, d, back, type) && !merged[b[i], b[j]]; b[i]++) {
                                for (b[j] = a[j]; b[j] < Size[j] && CompareStep(a, b, d, back, type) && !merged[b[i], b[j]]; b[j]++) { }

                                //If we didn't reach the end then its not a good add.
                                if (b[j] - a[j] < q[j]) {
                                    break;
                                }
                                else {
                                    b[j] = a[j];
                                }
                            }
                            q[i] = b[i] - a[i];

                            //Now we add the quad to the mesh
                            m = new Vect3Int();
                            m[i] = q[i];

                            n = new Vect3Int();
                            n[j] = q[j];

                            //We need to add a slight offset when working with front faces.
                            o = a;
                            o[d] += back ? 0 : 1;

                            //Draw the face to the mesh
                            verts = new Vector3[] { (Vector3)o, (Vector3)(o + m), (Vector3)(o + m + n), (Vector3)(o + n) };
                            mesh.AddColoredFace(verts, startBlock.Color, back);

                            //Mark it merged
                            for (int f = 0; f < q[i]; f++) {
                                for (int g = 0; g < q[j]; g++) {
                                    merged[a[i] + f, a[j] + g] = true;
                                }
                            }
                        }
                    }
                }
            }

            return mesh;
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Tests 2 blocks for equivalency. When the MeshType is render, color
        /// of each block is also checked.
        /// </summary>
        /// <param name="a">The first blocks position.</param>
        /// <param name="b">The second blocks position.</param>
        /// <param name="d">The face of the blocks to test. (0-5)</param>
        /// <param name="back">If the face is a back face or not.</param>
        /// <param name="type">What type of compare check to perform.</param>
        /// <returns>True if both blocks match based off MeshType criteria.</returns>
        private bool CompareStep(Vect3Int a, Vect3Int b, int d, bool back, MeshType type) {
            Block blockA = GetBlock(a);
            Block blockB = GetBlock(b);

            if(type == MeshType.Render) {
                return blockA == blockB && !blockB.IsAir && IsBlockFaceVisible(b, d, back);
            }
            else {
                return !blockB.IsAir && IsBlockFaceVisible(b, d, back);
            }
        }

        /// <summary>
        /// Checks if the block's face in direction a is visible
        /// to the player or not. (If it's neighbor is solid).
        /// </summary>
        /// <param name="pos">The position of the block to check at.</param>
        /// <param name="a">The side of the block to test.</param>
        /// <param name="back">If the sice a back face or not.</param>
        /// <returns>True if the neighbor block is air.</returns>
        private bool IsBlockFaceVisible(Vect3Int pos, int a, bool back) {
            pos[a] += back ? -1 : 1;
            return !GetBlock(pos).IsSolid;
        }

        /// <summary>
        /// Checks if the block's face in direction a is visible
        /// to the player or not. (If it's neighbor is solid).
        /// </summary>
        /// <param name="x">The x coordinate of the block.</param>
        /// <param name="y">The y coordinate of the block.</param>
        /// <param name="z">The z coordinate of the block.</param>
        /// <param name="direction">The direction of the face to check.</param>
        /// <returns>True if the face is visible.</returns>
        private bool IsBlockFaceVisible(int x, int y, int z, Direction direction) {
            if (GetBlock(x, y, z).IsAir) {
                return false;
            }

            switch (direction) {
                case Direction.north:
                    return !GetBlock(x, y, z + 1).IsSolid;
                case Direction.south:
                    return !GetBlock(x, y, z - 1).IsSolid;
                case Direction.east:
                    return !GetBlock(x + 1, y, z).IsSolid;
                case Direction.west:
                    return !GetBlock(x - 1, y, z).IsSolid;
                case Direction.up:
                    return !GetBlock(x, y + 1, z).IsSolid;
                case Direction.down:
                    return !GetBlock(x, y - 1, z).IsSolid;
                default:
                    return false;
            }
        }
        /// <summary>
        /// Flatten the 3D position of the block into a 1D array index.
        /// </summary>
        /// <param name="pos">The position to convert into a 1d index.</param>
        /// <returns>The flattened position.</returns>
        private int GetBlockIndex(Vect3Int pos) {
            return (Size.Y * Size.X * pos.Z) + (Size.X * pos.Y) + pos.X;
        }

        /// <summary>
        /// Flatten the 3D position of the block into a 1D array index.
        /// https://stackoverflow.com/questions/7367770/how-to-flatten-or-index-3d-array-in-1d-array
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y cooridnate.</param>
        /// <param name="z">The z coordinate.</param>
        /// <returns>The flattened position.</returns>
        private int GetBlockIndex(int x, int y, int z) {
            return (Size.Y * Size.X * z) + (Size.X * y) + x;
        }
        #endregion

        #region ISerializable
        /// <summary>
        /// Serialize the block container into a byte array that can be saved to file
        /// or sent over the network.
        /// </summary>
        /// <returns>The block container serialized into a byte array.</returns>
        public virtual byte[] Serialize() {
            List<byte> bytes = new List<byte>();

            //Write the render key, and size to the bytes.
            //SerializeUtils.SerializeAppend(bytes, RenderKey);
            //Size.SerializeAppend(bytes);

            ////Pack it column by column
            //for (int x = 0; x < Size.X; x++) {
            //    for(int z = 0; z < Size.Z; z++) {
            //        bytes.AddRange(SerializeColumn(x, z));
            //    }
            //}

            return bytes.ToArray();
        }

        /// <summary>
        /// Convert a single block column of the container into a run based
        /// encoding format.
        /// </summary>
        /// <param name="x">The x coordinate of the column.</param>
        /// <param name="z">The z coordinate of the column.</param>
        /// <returns>The serialized column of the container.</returns>
        protected byte[] SerializeColumn(int x, int z) {
            List<byte> bytes = new List<byte>();

            //First figure out how much air there is at the top
            byte airCount = 0;
            while(GetBlock(x, Size.Y - 1 - airCount, z) == Block.Air && airCount < Size.Y) {
                airCount++;
            }

            //Column is only air, return that.
            if(airCount == Size.Y) {
                return new byte[] { airCount };
            }
            //Has blocks too, add air count to byte array and move on
            else {
                bytes.Add(airCount);
            }

            //Calculate block runs now
            Block lastBlock = GetBlock(x, Size.Y - airCount - 1, z);
            Block currBlock = Block.Air;

            byte currL = 1;
            for(int y = Size.Y - airCount - 2; y >= 0; y--) {
                currBlock = GetBlock(x, y, z);

                if(currBlock == lastBlock) {
                    currL++;
                }
                else {
                    bytes.Add(currL);
                    bytes.AddRange(lastBlock.Serialize());
                }

                //Save block for next test
                lastBlock = currBlock;
            }

            //Write last interval
            bytes.Add(currL);
            bytes.AddRange(currBlock.Serialize());

            return bytes.ToArray();
        }
        #endregion
    }
}
