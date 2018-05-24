using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Voxelated.Serialization;
using Voxelated.Utilities;

namespace Voxelated.Utilities {
    /// <summary>
    /// Partial of SerializeUtils Class. This contains the Serialize()
    /// methods.
    /// </summary>
    public static partial class SerializeUtils {
        #region Integer Types Serializers
        /// <summary>
        /// Serialize a byte using 1-8 bits.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <param name="bitCount">The number of bits to use.</param>
        /// <returns>The value converted to a byte array.</returns>
        public static byte[] Serialize(byte value, int bitCount = 8) {
            //Ensure it's a valid bitCount.
            if(bitCount > 8) {
                bitCount = 8;
            }

            if(bitCount < 8) {
                byte bitMask = (byte)((1 << bitCount) - 1);
                value &= bitMask;
            }

            return new byte[] { value };
        }

        /// <summary>
        /// Serialize a short into 1 - 16 bits.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <param name="bitCount">The number of bits to use.</param>
        /// <returns>The value converted to a byte array.</returns>
        public static byte[] Serialize(short value, int bitCount = 16) {
            //Ensure it's a valid bitCount.
            if (bitCount > 16) {
                bitCount = 16;
            }

            return Serialize((ushort)value, bitCount);
        }

        /// <summary>
        /// Serialize a ushort into 1 - 16 bits.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <param name="bitCount">The number of bits to use.</param>
        /// <returns>The value converted to a byte array.</returns>
        public static byte[] Serialize(ushort value, int bitCount = 16) {
            //Ensure it's a valid bitCount.
            if (bitCount > 16) {
                bitCount = 16;
            }

            //Get the value bytes.
            byte[] values = BitConverter.GetBytes(value);

            //Check for an easy way out.
            if(bitCount == 16) {
                return values;
            }
            else {
                //Create the new byte array.
                int byteCount = BitManipulator.ByteCountForBits(bitCount);
                byte[] bytes = new byte[byteCount];

                //Write to it.
                BitManipulator.WriteBits(bytes, values, 0, bitCount);
                return bytes;
            }
        }

        /// <summary>
        /// Serialize a int into 1 - 32 bits.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <param name="bitCount">The number of bits to use.</param>
        /// <returns>The value converted to a byte array.</returns>
        public static byte[] Serialize(int value, int bitCount = 32) {
            //Ensure it's a valid bitCount.
            if (bitCount > 32) {
                bitCount = 32;
            }

            return Serialize((uint)value, bitCount);
        }

        /// <summary>
        /// Serialize an uint into 1 - 32 bits.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <param name="bitCount">The number of bits to use.</param>
        /// <returns>The value converted to a byte array.</returns>
        public static byte[] Serialize(uint value, int bitCount = 32) {
            //Ensure it's a valid bitCount.
            if (bitCount > 32) {
                bitCount = 32;
            }

            byte[] values = BitConverter.GetBytes(value);

            //Easy way out.
            if (bitCount == 32) {
                return values;
            }
            else {
                //Create the new byte array.
                int byteCount = BitManipulator.ByteCountForBits(bitCount);
                byte[] bytes = new byte[byteCount];

                //Write to it.
                BitManipulator.WriteBits(bytes, values, 0, bitCount);
                return bytes;
            }
        }

        /// <summary>
        /// Serialize a long into 1 - 64 bits.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <param name="bitCount">The number of bits to use.</param>
        /// <returns>The value converted to a byte array.</returns>
        public static byte[] Serialize(long value, int bitCount = 64) {
            //Ensure it's a valid bitCount.
            if (bitCount > 64) {
                bitCount = 64;
            }

            return Serialize((ulong)value, bitCount);
        }

        /// <summary>
        /// Serialize an ulong into 1 - 64 bits.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <param name="bitCount">The number of bits to use.</param>
        /// <returns>The value converted to a byte array.</returns>
        public static byte[] Serialize(ulong value, int bitCount = 64) {
            //Ensure it's a valid bitCount.
            if (bitCount > 64) {
                bitCount = 64;
            }

            byte[] values = BitConverter.GetBytes(value);

            //Easy way out.
            if (bitCount == 64) {
                return values;
            }
            else {
                //Create the new byte array.
                int byteCount = BitManipulator.ByteCountForBits(bitCount);
                byte[] bytes = new byte[byteCount];

                //Write to it.
                BitManipulator.WriteBits(bytes, values, 0, bitCount);
                return bytes;
            }
        }
        #endregion

        #region Bool Methods
        /// <summary>
        /// Convert a bool into a byte
        /// </summary>
        /// <param name="value">The bool to encode.</param>
        /// <param name="bitCount">The number of bits to use.</param>
        /// <returns>The serialized byte array of the bool.</returns>
        public static byte[] Serialize(bool value) {
            return new byte[] { (byte)(value ? 1 : 0) };
        }
        #endregion

        #region Char Methods
        /// <summary>
        /// Convert a char into a byte.
        /// </summary>
        /// <param name="value">The char to encode.</param>
        /// <returns>The serialized byte array of the char.</returns>
        public static byte[] Serialize(char value) {
            return new byte[] { Convert.ToByte(value) };
        }
        #endregion

        #region Float Methods
        /// <summary>
        /// Convert a float to a 4 byte array
        /// </summary>
        /// <param name="value">The float to encode.</param>
        /// <returns>The serialized byte array of the float.</returns>
        public static byte[] Serialize(float value) {
            return BitConverter.GetBytes(value);
        }
        #endregion

        #region Double Methods
        /// <summary>
        /// Convert a double into a byte array.
        /// </summary>       
        /// <param name="value">The double to encode.</param>
        /// <returns>The serialized byte array of the double.</returns>
        public static byte[] Serialize(double value) {
            return BitConverter.GetBytes(value);
        }
        #endregion

        #region String Methods
        /// <summary>
        /// Convert a string into a byte array. The
        /// length of this will vary. But first 2 bytes
        /// are a short representing string length
        /// </summary>
        /// <param name="value">The string to encode.</param>
        /// <returns>The serialized byte array of the string.</returns>
        public static byte[] Serialize(string value) {
            if ((value.Length) > ushort.MaxValue) {
                throw new ArgumentException("String exceeds max length of: " + ushort.MaxValue);
            }

            //Strings are encoded via ascii (We append 2 spaces to make room for length short)
            byte[] strBytes = Encoding.UTF8.GetBytes("  " + value);
            byte[] lenBytes = Serialize((ushort)value.Length);
            ArrayUtils.CopyInto(lenBytes, strBytes);

            return strBytes;
        }
        #endregion

        #region DateTime Methods
        /// <summary>
        /// Convert a DateTime into a byte array.
        /// </summary>
        /// <param name="value">The DateTime to serialize.</param>
        /// <returns>The serialized DateTime.</returns>
        public static byte[] Serialize(DateTime value) {
            long longTime = value.ToBinary();
            return Serialize(longTime);
        }
        #endregion

        #region IP Methods
        /// <summary>
        /// Convert a IPEndPoint into a byte array.
        /// </summary>
        /// <param name="value">The IPEndPoint to serialize.</param>
        /// <returns>The serialized IPEndPoint.</returns>
        public static byte[] Serialize(IPEndPoint value) {
            byte[] addressBytes = value.Address.GetAddressBytes();
            byte[] portBytes = Serialize(value.Port);

            byte[] bytes = new byte[addressBytes.Length + portBytes.Length + 1];
            int addressBitCount = addressBytes.Length * 8;

            //Write the address length, address and lastly port.
            BitManipulator.WriteBits(bytes, Serialize((byte)addressBytes.Length), 0, 8);
            BitManipulator.WriteBits(bytes, addressBytes, 8, addressBitCount);
            BitManipulator.WriteBits(bytes, portBytes, 8 + addressBitCount, 32);
            return bytes;
        }
        #endregion

        #region Vector2 Methods
        /// <summary>
        /// Convert a Unity Vector2 into a byte array.
        /// </summary>
        /// <param name="value">The Vector2 to serialize.</param>
        /// <returns>The encoded Vector2 in a byte[8].</returns>
        public static byte[] Serialize(Vector2 value) {
            byte[] bytes = new byte[8];
            byte[] xBytes = Serialize(value.x);
            byte[] yBytes = Serialize(value.y);

            //Write the bits to the array.
            BitManipulator.WriteBits(bytes, xBytes, 0, 32);
            BitManipulator.WriteBits(bytes, yBytes, 32, 32);
            return bytes;
        }
        #endregion

        #region Vector3 Methods
        /// <summary>
        /// Convert a Unity Vector3 into a byte array.
        /// </summary>
        /// <param name="value">The Vector3 to serialize.</param>
        /// <returns>The encoded Vector3 in a byte[12].</returns>
        public static byte[] Serialize(Vector3 value) {
            byte[] bytes = new byte[12];
            byte[] xBytes = Serialize(value.x);
            byte[] yBytes = Serialize(value.y);
            byte[] zBytes = Serialize(value.z);

            //Write the bits to the array. 
            BitManipulator.WriteBits(bytes, xBytes, 0, 32);
            BitManipulator.WriteBits(bytes, yBytes, 32, 32);
            BitManipulator.WriteBits(bytes, zBytes, 64, 32);

            return bytes;
        }
        #endregion

        #region Vector2Int Methods
        /// <summary>
        /// Convert a Unity Vector2Int into a byte array.
        /// </summary>
        /// <param name="value">The Vector2Int to serialize.</param>
        /// <param name="bitCount">The number of bits to use.</param>
        /// <returns>The encoded Vector2Int in a byte[8].</returns>
        public static byte[] Serialize(Vector2Int value, int bitCount = 64) {
            //Ensure valid bit count.
            if (bitCount > 64) {
                bitCount = 64;
            }

            if (bitCount % 2 != 0) {
                throw new ArgumentException("BitCount must be divisible by 2.");
            }

            int bitsPerComp = bitCount / 2;
            int byteCount = BitManipulator.ByteCountForBits(bitCount);

            byte[] bytes = new byte[byteCount];
            byte[] xBytes = Serialize(value.x, bitsPerComp);
            byte[] yBytes = Serialize(value.y, bitsPerComp);

            //Write the bits to the array.
            BitManipulator.WriteBits(bytes, xBytes,           0, bitsPerComp);
            BitManipulator.WriteBits(bytes, yBytes, bitsPerComp, bitsPerComp);
            return bytes;
        }
        #endregion

        #region Vector3Int Methods
        /// <summary>
        /// Convert a Unity Vector3Int into a byte array.
        /// </summary>
        /// <param name="value">The Vector3Int to serialize.</param>
        /// <param name="bitCount">The number of bits to use.</param>
        /// <returns>The encoded Vector3Int in a byte[12].</returns>
        public static byte[] Serialize(Vector3Int value, int bitCount = 96) {
            //Ensure valid bit count.
            if (bitCount > 96) {
                bitCount = 96;
            }

            //Ensure it can be divided by 3.
            if (bitCount % 3 != 0) {
                throw new ArgumentException("BitCount must be divisible by 3.");
            }

            int bitsPerComp = bitCount / 3;
            int byteCount = BitManipulator.ByteCountForBits(bitCount);

            byte[] bytes = new byte[byteCount];
            byte[] xBytes = Serialize(value.x, bitsPerComp);
            byte[] yBytes = Serialize(value.y, bitsPerComp);
            byte[] zBytes = Serialize(value.z, bitsPerComp);

            //Write the bits to the array.
            BitManipulator.WriteBits(bytes, xBytes, 0,               bitsPerComp);
            BitManipulator.WriteBits(bytes, yBytes, bitsPerComp,     bitsPerComp);
            BitManipulator.WriteBits(bytes, zBytes, 2 * bitsPerComp, bitsPerComp);
            return bytes;
        }
        #endregion

        #region Vect2Int Methods
        /// <summary>
        /// Convert a Unity Vect2Int into a byte array.
        /// </summary>
        /// <param name="value">The Vect2Int to serialize.</param>
        /// <param name="bitCount">The number of bits to use.</param>
        /// <returns>The encoded Vect2Int in a byte[12].</returns>
        public static byte[] Serialize(Vect2Int value, int bitCount = 64) {
            //Ensure valid bit count.
            if (bitCount > 64) {
                bitCount = 64;
            }

            if (bitCount % 2 != 0) {
                throw new ArgumentException("BitCount must be divisible by 2.");
            }

            int bitsPerComp = bitCount / 2;
            int byteCount = BitManipulator.ByteCountForBits(bitCount);

            byte[] bytes = new byte[byteCount];
            byte[] xBytes = Serialize(value.X, bitsPerComp);
            byte[] yBytes = Serialize(value.Y, bitsPerComp);

            //Write the bits to the array.
            BitManipulator.WriteBits(bytes, xBytes, 0, bitsPerComp);
            BitManipulator.WriteBits(bytes, yBytes, bitsPerComp, bitsPerComp);
            return bytes;
        }
        #endregion

        #region Vect3Int Methods
        /// <summary>
        /// Convert a Unity Vect3Int into a byte array.
        /// </summary>
        /// <param name="value">The Vect3Int to serialize.</param>
        /// <param name="bitCount">The number of bits to use.</param>
        /// <returns>The encoded Vect3Int in a byte[12].</returns>
        public static byte[] Serialize(Vect3Int value, int bitCount = 96) {
            //Ensure valid bit count.
            if (bitCount > 96) {
                bitCount = 96;
            }

            //Ensure it can be divided by 3.
            if (bitCount % 3 != 0) {
                throw new ArgumentException("BitCount must be divisible by 3.");
            }

            int bitsPerComp = bitCount / 3;
            int byteCount = BitManipulator.ByteCountForBits(bitCount);

            byte[] bytes = new byte[byteCount];
            byte[] xBytes = Serialize(value.X, bitsPerComp);
            byte[] yBytes = Serialize(value.Y, bitsPerComp);
            byte[] zBytes = Serialize(value.Z, bitsPerComp);

            //Write the bits to the array.
            BitManipulator.WriteBits(bytes, xBytes, 0, bitsPerComp);
            BitManipulator.WriteBits(bytes, yBytes, bitsPerComp, bitsPerComp);
            BitManipulator.WriteBits(bytes, zBytes, 2 * bitsPerComp, bitsPerComp);
            return bytes;
        }
        #endregion

        #region Color16 Methods
        /// <summary>
        /// Convert a Color16 into a 2 byte array.
        /// </summary>
        /// <param name="value">The color to serialize.</param>
        /// <returns>The encoded color.</returns>
        public static byte[] Serialize(Color16 value) {
            return Serialize(value.ToUShort());
        }
        #endregion

        #region Color32 Methods
        /// <summary>
        /// Convert a Color32 into a 4 byte array.
        /// </summary>
        /// <param name="value">The color to serialize.</param>
        /// <returns>The encoded color.</returns>
        public static byte[] Serialize(Color32 value) {
            return new byte[] { value.r, value.g, value.b, value.a };
        }
        #endregion

        #region Serializable Object Methods
        /// <summary>
        /// Serialize an object that derives from the
        /// SerializableObject abstract class into a byte array.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <returns>The serialized byte array.</returns>
        public static byte[] Serialize(SerializableObject value) {
            return value.Serialize();
        }
        #endregion
    }
}
