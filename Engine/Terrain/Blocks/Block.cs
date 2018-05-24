using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using Voxelated.Serialization;
using System.Collections.Generic;
using Voxelated.Utilities;

namespace Voxelated.Terrain {
    /// <summary>
    /// Block.
    /// The most basic class in the engine. This represents a single block of the world.
    /// </summary>
    [Serializable]
    public struct Block {
        #region BitMasks
        /// <summary>
        /// Bitwise mask for retrieving the owner information.
        /// </summary>
        private const byte OBitMask = 0xFC; // 1111 1100 

        /// <summary>
        /// Bitwise mask to retrieve the type info.
        /// </summary>
        private const byte TBitMask = 0x3;  // 0000 0011 
        #endregion

        #region Static Instances
        /// <summary>
        /// An air block. It's invisible. Duh..
        /// </summary>
        public static Block Air = new Block();

        /// <summary>
        /// A water block.
        /// </summary>
        public static Block Water = GetLiquidBlock(Color16.PeterRiver);
        #endregion

        #region Block Info
        /// <summary>
        /// The color of the block.
        /// </summary>
        public Color16 Color { get; private set; }

        /// <summary>
        /// What kind of block it is.
        /// </summary>
        public BlockType Type {
            get {
                return (BlockType)(info & TBitMask);
            }
            private set {
                info &= OBitMask;
                info |= (byte)((byte)value & TBitMask);
            }
        }
    
        /// <summary>
        /// Who placed the block
        /// </summary>
        public byte Owner {
            get { return (byte)(info & OBitMask); }
            private set {
                info &= TBitMask;
                info |= (byte)(value & OBitMask);
            }
        }

        /// <summary>
        /// How fast a player can move on / in
        /// this block.
        /// </summary>
        public float SpeedModifier {
            get {
                if(Type == BlockType.Liquid) {
                    return 0.75f;
                } else if(Type == BlockType.Sprite) {

                    //Later on well hardcode some values in here
                    //to slow down players running through tall
                    //grass etc..

                    return 1.0f;
                }
                else {
                    return 1.0f;
                }
            }
        }

        /// <summary>
        /// The alpha value, if true, block
        /// is invisible.
        /// </summary>
        public bool IsSolid {
            get { return Type == BlockType.Solid; }
        }

        /// <summary>
        /// If it is an air block or not.
        /// </summary>
        public bool IsAir {
            get { return Type == BlockType.Air; }
        }

        /// <summary>
        /// Various data that might be stored on the block.
        /// Could be anything from health, to sprite id.
        /// </summary>
        public byte MetaData {
            get { return metaData; }
        }
        #endregion

        #region Members
        /// <summary>
        /// The owner, and type of the block
        /// </summary>
        private byte info;

        /// <summary>
        /// Special byte that can be used for anything.
        /// This varies in use based off the type of 
        /// the block.
        /// </summary>
        private byte metaData;
        #endregion

        #region Instance Creators
        /// <summary>
        /// Returns a new liquid block. 
        /// </summary>
        public static Block GetLiquidBlock(Color16 color) {
            Block block = new Block();
            block.Color = color;
            block.Type = BlockType.Liquid;
            block.metaData = 8;

            return block;
        }

        /// <summary>
        /// Returns a new color block with the specified health. If health
        /// equals 7 then the block is invincible.
        /// </summary>
        public static Block GetColorBlock(Color16 color, byte health = 6) {
            Block block = new Block();
            block.Color = color;
            block.Type = BlockType.Solid;
            block.metaData = health;

            return block;
        }

        /// <summary>
        /// Creates a block from its byte array
        /// </summary>
        public Block(byte[] bytes, int startIndex = 0) {
            Color = SerializeUtils.GetColor16(bytes, startIndex);
            info = bytes[startIndex + 2];
            metaData = bytes[startIndex + 3];
        }
        #endregion

        #region Operators
        /// <summary>
        /// Tests if two blocks are equivalent.
        /// </summary>
        public static bool operator ==(Block a, Block b) {
            return a.Equals(b);
        }
        
        /// <summary>
        /// Tests if two blocks are not equal.
        /// </summary>
        public static bool operator !=(Block a, Block b) {
            return !a.Equals(b);
        }

        /// <summary>
        /// Checks if two blocks are equal to each other.
        /// </summary>
        public override bool Equals(object obj) {
            //Check it's actually a block first
            if(!(obj is Block)) {
                return false;
            }

            //Now test it. We don't care about ownership.
            Block block = (Block)obj;
            return block.info == info && block.Color == Color;
        }

        /// <summary>
        /// Generates a unique hash code.
        /// </summary>
        public override int GetHashCode() {
            return info.GetHashCode() + Color.GetHashCode() + metaData.GetHashCode();
        }

        /// <summary>
        /// Converts the block into a print friendly format
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return "(Block) Type: " + Type.ToString() + ", Color: " + Color.ToString() + ", Info: " + info + ", Meta: " + metaData;
        }
        #endregion

        #region ISerializable
        /// <summary>
        /// Convert the block into a serialiable byte array.
        /// </summary>
        public byte[] Serialize() {
            byte[] colorBytes = SerializeUtils.Serialize(Color);
            return new byte[] { colorBytes[0], colorBytes[1], info, metaData };
        }

        /// <summary>
        /// Serialize the block into a pre-existing byte array.
        /// </summary>
        /// <param name="bytes">The array to store it in.</param>
        /// <param name="startIndex">The first byte location.</param>
        public void SerializeInto(byte[] bytes, int startIndex = 0) {
            byte[] b = Serialize();
            ArrayUtils.CopyInto(b, bytes, startIndex);
        }

        /// <summary>
        /// Serialize the block into a pre-existing byte list.
        /// </summary>
        /// <param name="bytes">The list to store it in.</param>
        /// <param name="startIndex">The first byte location.</param>
        public void SerializeInto(List<byte> bytes, int startIndex = 0) {
            byte[] b = Serialize();
            ListUtils.CopyInto(b, bytes, startIndex);
        }

        /// <summary>
        /// Serialize the block to the end of a byte list.
        /// </summary>
        /// <param name="bytes">The byte list to append it to.</param>
        public void SerializeAppend(List<byte> bytes) {
            byte[] b = Serialize();
            ListUtils.CopyInto(b, bytes, bytes.Count);
        }
        #endregion
    }
}

