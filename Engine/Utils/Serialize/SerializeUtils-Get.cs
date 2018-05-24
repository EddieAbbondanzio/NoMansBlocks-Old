using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Voxelated.Network;
using Voxelated.Network.Lobby;
using Voxelated.Serialization;
using Voxelated.Utilities;

namespace Voxelated.Utilities {
    /// <summary>
    /// Partial of SerializeUtils Class. This contains the GetTYPE()
    /// methods.
    /// </summary>
    public static partial class SerializeUtils {
        #region Integer Type Getters
        /// <summary>
        /// Deserialize a byte from it's byte format of 1 - 8 bits.
        /// </summary>
        /// <param name="bytes">The encoded bytes.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <param name="bitCount">The number of bits its in.</param>
        /// <returns>The decoded byte.</returns>
        public static byte GetByte(byte[] bytes, int startBit, int bitCount = 8) {
            //Ensure it's a valid bitCount.
            if (bitCount > 8) {
                bitCount = 8;
            }

            //Performance boost.
            if(startBit % 8 == 0 && bitCount == 8) {
                return bytes[startBit / 8];
            }

            byte[] bits = BitManipulator.ReadBits(bytes, startBit, bitCount);
            return bits[0];
        }

        /// <summary>
        /// Deserialize a short from it's byte format of 1 - 16 bits.
        /// </summary>
        /// <param name="bytes">The encoded bytes.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <param name="bitCount">The number of bits its in.</param>
        /// <returns>The decoded short.</returns>
        public static short GetShort(byte[] bytes, int startBit, int bitCount = 16) {
            //Ensure it's a valid bitCount.
            if (bitCount > 16) {
                bitCount = 16;
            }

            ushort unsignedVal = GetUShort(bytes, startBit, bitCount);

            //When less than 16 bits we need to pad it out with 1's if negative.
            if (bitCount < 16 && BitManipulator.ReadBit(bytes, startBit + bitCount - 1) == 1) {
                ushort bitMask = (ushort)~((1 << bitCount) - 1);
                unsignedVal |= bitMask;
            }

            return (short)unsignedVal;
        }

        /// <summary>
        /// Deserialize an ushort from it's byte format of 1 - 16 bits.
        /// </summary>
        /// <param name="bytes">The encoded bytes.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <param name="bitCount">The number of bits its in.</param>
        /// <returns>The decoded ushort.</returns>
        public static ushort GetUShort(byte[] bytes, int startBit, int bitCount = 16) {
            //Ensure it's a valid bitCount.
            if (bitCount > 16) {
                bitCount = 16;
            }

            int startByte = startBit / 8;

            //Easy way out.
            if (startBit % 8 == 0 && bitCount == 16) {
                return BitConverter.ToUInt16(bytes, startByte);
            }
            else {
                //Read in the desired # of bits then rebuild the value.
                byte[] b = BitManipulator.ReadBits(bytes, startBit, bitCount);
                ushort val = b[0];

                //Pull in the second byte if there is one.
                if(b.Length > 1) {
                    val |= (ushort)(b[1] << 8);
                }

                return val;
            }
        }

        /// <summary>
        /// Deserialize a int from it's byte format of 1 - 32 bits.
        /// </summary>
        /// <param name="bytes">The encoded bytes.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <param name="bitCount">The number of bits its in.</param>
        /// <returns>The decoded int.</returns>
        public static int GetInt(byte[] bytes, int startBit, int bitCount = 32) {
            //Ensure it's a valid bitCount.
            if (bitCount > 32) {
                bitCount = 32;
            }

            uint unsigedVal = GetUInt(bytes, startBit, bitCount);

            //When working with less than 32 bits, we may need to pad 1s for negatives
            if(bitCount < 32 && BitManipulator.ReadBit(bytes, startBit + bitCount - 1) == 1) {
                uint bitMask = (uint)~((1 << bitCount) - 1);
                unsigedVal |= bitMask;
            }

            return (int)unsigedVal;
        }

        /// <summary>
        /// Deserialize an uint from it's byte format of 1 - 32 bits.
        /// </summary>
        /// <param name="bytes">The encoded bytes.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <param name="bitCount">The number of bits its in.</param>
        /// <returns>The decoded uint.</returns>
        public static uint GetUInt(byte[] bytes, int startBit, int bitCount = 32) {
            //Ensure it's a valid bitCount.
            if (bitCount > 32) {
                bitCount = 32;
            }

            int startByte = startBit / 8;

            //Easy way out.
            if (startBit % 8 == 0 && bitCount == 32) {
                return BitConverter.ToUInt32(bytes, startByte);
            }
            else {
                byte[] b = BitManipulator.ReadBits(bytes, startBit, bitCount);
                uint val = b[0];

                //Add each byte to it.
                for(int i = 1; i < b.Length; i++) {
                    val |= (uint)(b[i] << (i * 8));
                }

                return val;
            }
        }

        /// <summary>
        /// Deserialize a int from it's byte format of 1 - 32 bits.
        /// </summary>
        /// <param name="bytes">The encoded bytes.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <param name="bitCount">The number of bits its in.</param>
        /// <returns>The decoded int.</returns>
        public static long GetLong(byte[] bytes, int startBit, int bitCount = 64) {
            //Ensure it's a valid bitCount.
            if (bitCount > 64) {
                bitCount = 64;
            }

            ulong unsigedVal = GetULong(bytes, startBit, bitCount);

            //When working with less than 32 bits, we may need to pad 1s for negatives
            if (bitCount < 64 && BitManipulator.ReadBit(bytes, startBit + bitCount - 1) == 1) {
                ulong bitMask = (ulong)~((1 << bitCount) - 1);
                unsigedVal |= bitMask;
            }

            return (long)unsigedVal;
        }

        /// <summary>
        /// Deserialize an uint from it's byte format of 1 - 32 bits.
        /// </summary>
        /// <param name="bytes">The encoded bytes.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <param name="bitCount">The number of bits its in.</param>
        /// <returns>The decoded uint.</returns>
        public static ulong GetULong(byte[] bytes, int startBit, int bitCount = 64) {
            //Ensure it's a valid bitCount.
            if (bitCount > 64) {
                bitCount = 64;
            }

            //Easy way out.
            if (startBit % 8 == 0 && bitCount == 64) {
                return BitConverter.ToUInt64(bytes, startBit / 8);
            }
            else {
                byte[] b = BitManipulator.ReadBits(bytes, startBit, bitCount);
                ulong val = b[0];

                //Add each byte to it.
                for (int i = 1; i < b.Length; i++) {
                    val |= ((ulong)b[i] << (i * 8));
                }

                return val;
            }
        }
        #endregion

        #region Bool Methods
        /// <summary>
        /// Decodes a bool value from its serialized byte data.
        /// </summary>
        /// <param name="bytes">The byte array to extract the bool from.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <returns>The bool value extracted from the array.</returns>
        public static bool GetBool(byte[] bytes, int startBit) {
            return BitManipulator.ReadBit(bytes, startBit) == 1;
        }
        #endregion

        #region Char Methods
        /// <summary>
        /// Decodes a char value from its serialized byte data.
        /// </summary>
        /// <param name="bytes">The byte array to extract the char from.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <returns>The char value extracted from the array.</returns>
        public static char GetChar(byte[] bytes, int startBit) {
            if(startBit % 8 == 0) {
                byte b = bytes[startBit / 8];
                return Convert.ToChar(b);
            }
            else {
                byte b = BitManipulator.ReadBits(bytes, startBit, 8)[0];
                return Convert.ToChar(b);
            }
        }
        #endregion

        #region Float Methods
        /// <summary>
        /// Decodes a float value from its serialized byte data.
        /// </summary>
        /// <param name="bytes">The byte array to extract the float from.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <returns>The float value extracted from the array.</returns>
        public static float GetFloat(byte[] bytes, int startBit) {
            if (startBit % 8 == 0) {
                return BitConverter.ToSingle(bytes, startBit / 8);
            }
            else {
                byte[] b = BitManipulator.ReadBits(bytes, startBit, 32);
                return BitConverter.ToSingle(b, 0);
            }
        }
        #endregion

        #region Double Methods
        /// <summary>
        /// Decodes a double value from its serialized byte data.
        /// </summary>
        /// <param name="bytes">The byte array to extract the double from.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <returns>The double value extracted from the array.</returns>
        public static double GetDouble(byte[] bytes, int startBit) {
            if(startBit % 8 == 0) {
                return BitConverter.ToDouble(bytes, startBit / 8);
            }
            else {
                byte[] b = BitManipulator.ReadBits(bytes, startBit, 64);
                return BitConverter.ToSingle(b, 0);
            }

        }
        #endregion

        #region String Methods
        /// <summary>
        /// Decodes a string value from its serialized byte data.
        /// </summary>
        /// <param name="bytes">The byte array to extract the string from.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <returns>The string value extracted from the array.</returns>
        public static string GetString(byte[] bytes, int startBit) {
            ushort strLen = GetUShort(bytes, startBit);

            if (startBit % 8 == 0) {
                return Encoding.UTF8.GetString(bytes, (startBit / 8) + 2, strLen);
            }
            else {
                byte[] b = BitManipulator.ReadBits(bytes, startBit, strLen * 8);
                return Encoding.UTF8.GetString(b, 0, strLen);
            }
        }
        #endregion

        #region Byte[] Methods
        /// <summary>
        /// Extract a byte subarray from the byte array.
        /// </summary>
        /// <param name="bytes">The soruce to extract from.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <param name="bitCount">The number of bytes o read.</param>
        /// <returns>The byte subarray.</returns>
        public static byte[] GetBytes(byte[] bytes, int startBit, int bitCount) {
            if (startBit % 8 == 0 && bitCount % 8 == 0) {
                return bytes.SubArray(startBit / 8, bitCount / 8);
            }
            else {
                return BitManipulator.ReadBits(bytes, startBit, bitCount);
            }
        }
        #endregion

        #region DateTime Methods
        /// <summary>
        /// Decodes a DateTime back into it's original format from
        /// a binary serialized array.
        /// </summary>
        /// <param name="bytes">The encoded DateTime</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <returns>The extracted DateTime.</returns>
        public static DateTime GetDateTime(byte[] bytes, int startBit) {
            long longTime = GetLong(bytes, startBit);
            return DateTime.FromBinary(longTime);
        }
        #endregion

        #region IP Methods
        /// <summary>
        /// Decode an IPEndPoint from it's serialized byte format.
        /// </summary>
        /// <param name="bytes">The encoded IPEndPoint.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <returns>The decoded IPEndPoint.</returns>
        public static IPEndPoint GetIPEndPoint(byte[] bytes, int startBit) {
            //Find the address info.
            int addressByteCount = bytes[startBit / 8] * 8;
            byte[] addressBytes = GetBytes(bytes, startBit + 8, addressByteCount);

            //Extract the data.
            IPAddress address = new IPAddress(addressBytes);
            int port = GetInt(bytes, startBit + addressByteCount + 8);

            return new IPEndPoint(address, port);
        }
        #endregion

        #region Vector2 Methods
        /// <summary>
        /// Decodes a Vector2 from it's byte[8].
        /// </summary>
        /// <param name="bytes">The byte array containing the Vector2.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <returns>The Vector2 that was deserialized.</returns>
        public static Vector2 GetVector2(byte[] bytes, int startBit) {
            float x = GetFloat(bytes, startBit);
            float y = GetFloat(bytes, startBit + 32);

            return new Vector2(x, y);
        }
        #endregion

        #region Vector3 Methods
        /// <summary>
        /// Decodes a Vector3 from it's byte[12].
        /// </summary>
        /// <param name="bytes">The byte array containing the Vector3.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <returns>The Vector3 that was deserialized.</returns>
        public static Vector3 GetVector3(byte[] bytes, int startBit) {
            float x = GetFloat(bytes, startBit);
            float y = GetFloat(bytes, startBit + 32);
            float z = GetFloat(bytes, startBit + 64);

            return new Vector3(x, y, z);
        }
        #endregion

        #region Vector2Int Methods
        /// <summary>
        /// Decodes a Vector2Int from it's byte[8].
        /// </summary>
        /// <param name="bytes">The byte array containing the Vector2Int.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <param name="bitCount">The number of bits its in.</param>
        /// <returns>The Vector2Int that was deserialized.</returns>
        public static Vector2Int GetVector2Int(byte[] bytes, int startBit, int bitCount = 64) {
            //Ensure valid bit count.
            if(bitCount > 64) {
                bitCount = 64;
            }

            if(bitCount % 2 != 0) {
                throw new ArgumentException("BitCount must be divisible by 2.");
            }

            int bitsPerComp = bitCount / 2;

            int x = GetInt(bytes, startBit, bitsPerComp);
            int y = GetInt(bytes, startBit + bitsPerComp, bitsPerComp);

            return new Vector2Int(x, y);
        }
        #endregion

        #region Vector3Int Methods
        /// <summary>
        /// Decodes a Vector3Int from it's byte[12].
        /// </summary>
        /// <param name="bytes">The byte array containing the Vector3Int.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <param name="bitCount">The number of bits its in.</param>
        /// <returns>The Vector3Int that was deserialized.</returns>
        public static Vector3Int GetVector3Int(byte[] bytes, int startBit, int bitCount = 96) {
            //Ensure valid bit count.
            if (bitCount > 96) {
                bitCount = 96;
            }

            //Ensure it can be divided by 3.
            if (bitCount % 3 != 0) {
                throw new ArgumentException("BitCount must be divisible by 3.");
            }

            int bitsPerComp = bitCount / 3;

            int x = GetInt(bytes, startBit, bitsPerComp);
            int y = GetInt(bytes, startBit + bitsPerComp, bitsPerComp);
            int z = GetInt(bytes, startBit + 2 * bitsPerComp, bitsPerComp);

            return new Vector3Int(x, y, z);
        }
        #endregion

        #region Vect2Int Methods
        /// <summary>
        /// Decodes a Vect2Int from it's byte[8].
        /// </summary>
        /// <param name="bytes">The byte array containing the Vect2Int.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <param name="bitCount">The number of bits its in.</param>
        /// <returns>The Vect2Int that was deserialized.</returns>
        public static Vect2Int GetVect2Int(byte[] bytes, int startBit, int bitCount = 64) {
            //Ensure valid bit count.
            if (bitCount > 64) {
                bitCount = 64;
            }

            if (bitCount % 2 != 0) {
                throw new ArgumentException("BitCount must be divisible by 2.");
            }

            int bitsPerComp = bitCount / 2;

            int x = GetInt(bytes, startBit, bitsPerComp);
            int y = GetInt(bytes, startBit + bitsPerComp, bitsPerComp);

            return new Vect2Int(x, y);
        }
        #endregion

        #region Vect3Int Methods
        /// <summary>
        /// Decodes a Vect3Int from it's byte[12].
        /// </summary>
        /// <param name="bytes">The byte array containing the Vect3Int.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <param name="bitCount">The number of bits its in.</param>
        /// <returns>The Vect3Int that was deserialized.</returns>
        public static Vect3Int GetVect3Int(byte[] bytes, int startBit, int bitCount = 96) {
            //Ensure valid bit count.
            if (bitCount > 96) {
                bitCount = 96;
            }

            //Ensure it can be divided by 3.
            if (bitCount % 3 != 0) {
                throw new ArgumentException("BitCount must be divisible by 3.");
            }

            int bitsPerComp = bitCount / 3;

            int x = GetInt(bytes, startBit, bitsPerComp);
            int y = GetInt(bytes, startBit + bitsPerComp, bitsPerComp);
            int z = GetInt(bytes, startBit + 2 * bitsPerComp, bitsPerComp);

            return new Vect3Int(x, y, z);
        }
        #endregion

        #region Color16 Methods
        /// <summary>
        /// Decodes a Color16 from it's byte[2].
        /// </summary>
        /// <param name="bytes">The byte array containing the Color16.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <returns>The Color16 that was deserialized.</returns>
        public static Color16 GetColor16(byte[] bytes, int startBit) {
            ushort val = GetUShort(bytes, startBit);
            return new Color16(val);
        }
        #endregion

        #region Color32 Methods
        /// <summary>
        /// Decodes a Color32 from it's byte[4].
        /// </summary>
        /// <param name="bytes">The byte array containing the Color32.</param>
        /// <param name="startBit">The first bit of the value.</param>
        /// <returns>The Color32 that was deserialized.</returns>
        public static Color32 GetColor32(byte[] bytes, int startBit) {
            byte r = GetByte(bytes, startBit);
            byte g = GetByte(bytes, startBit + 8);
            byte b = GetByte(bytes, startBit + 16);
            byte a = GetByte(bytes, startBit + 24);

            return new Color32(r, g, b, a);
        }
        #endregion

        #region Serializable Object Methods
        /// <summary>
        /// Deserialize an object that derives
        /// from SerializableObject. The actual object is recreated
        /// but to prevent redundant methods only a base class
        /// reference is returned.
        /// </summary>
        /// <param name="bytes">The encoded object.</param>
        /// <param name="startBit">The first bit of the object.</param>
        /// <returns>The deserialized objet.</returns>
        public static SerializableObject GetSerializableObject(byte[] bytes, int startBit) {
            SerializableType objectType = (SerializableType) GetByte(bytes, startBit);

            switch (objectType) {
                case SerializableType.NetPlayer:
                    return new NetPlayer(bytes, startBit);

                case SerializableType.NetPlayerStats:
                    return new NetPlayerStats(bytes, startBit);

                case SerializableType.NetTeam:
                    return new NetTeam(bytes, startBit);

                default:
                    return null;
            }
        }
        #endregion
    }
}
