using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Voxelated.Utilities;

namespace Voxelated.Serialization {
    /// <summary>
    /// The Write methods of the byte buffer class.
    /// </summary>
    public partial class ByteBuffer {
        #region Write Methods
        /// <summary>
        /// Write a byte to the buffer.
        /// </summary>
        /// <param name="value">The byte to write.</param>
        /// <param name="bitCount">How many bits to write it with.</param>
        public void Write(byte value, int bitCount = 8) {
            if(bitCount < 1 || bitCount > 8) {
                throw new ArgumentOutOfRangeException("BitCount", "Must be greater than 0, and less than 8.");
            }

            ValidateWriteAction(bitCount);

            BitManipulator.WriteBits(bytes, new byte[] { value }, currentIndex, bitCount);
            currentIndex += bitCount;
        }

        /// <summary>
        /// Write a short to the buffer.
        /// </summary>
        /// <param name="value">The short to write.</param>
        /// <param name="bitCount">How many bits to write it with.</param>
        public void Write(short value, int bitCount = 16) {
            if (bitCount < 1 || bitCount > 16) {
                throw new ArgumentOutOfRangeException("BitCount", "Must be greater than 0, and less than 16.");
            }

            Write((ushort)value, bitCount);
        }

        /// <summary>
        /// Write an ushort to the buffer.
        /// </summary>
        /// <param name="value">The ushort to write.</param>
        /// <param name="bitCount">How many bits to write it with.</param>
        public void Write(ushort value, int bitCount = 16) {
            if (bitCount < 1 || bitCount > 16) {
                throw new ArgumentOutOfRangeException("BitCount", "Must be greater than 0, and less than 16.");
            }

            ValidateWriteAction(bitCount);

            byte[] b = SerializeUtils.Serialize(value);
            BitManipulator.WriteBits(bytes, b, currentIndex, bitCount);

            currentIndex += bitCount;
        }

        /// <summary>
        /// Write an int to the buffer.
        /// </summary>
        /// <param name="value">The int to write.</param>
        /// <param name="bitCount">How many bits to write it with.</param>
        public void Write(int value, int bitCount = 32) {
            if (bitCount < 1 || bitCount > 32) {
                throw new ArgumentOutOfRangeException("BitCount", "Must be greater than 0, and less than 32.");
            }

            Write((uint)value, bitCount);
        }

        /// <summary>
        /// Write an uint to the buffer.
        /// </summary>
        /// <param name="value">The uint to write.</param>
        /// <param name="bitCount">How many bits to write it with.</param>
        public void Write(uint value, int bitCount = 32) {
            if (bitCount < 1 || bitCount > 32) {
                throw new ArgumentOutOfRangeException("BitCount", "Must be greater than 0, and less than 32.");
            }

            ValidateWriteAction(bitCount);

            byte[] b = SerializeUtils.Serialize(value);
            BitManipulator.WriteBits(bytes, b, currentIndex, bitCount);

            currentIndex += bitCount;
        }

        /// <summary>
        /// Write a long to the buffer.
        /// </summary>
        /// <param name="value">The long to write.</param>
        /// <param name="bitCount">How many bits to write it with.</param>
        public void Write(long value, int bitCount = 64) {
            if (bitCount < 1 || bitCount > 64) {
                throw new ArgumentOutOfRangeException("BitCount", "Must be greater than 0, and less than 64.");
            }

            Write((ulong)value, bitCount);
        }

        /// <summary>
        /// Write a ulong to the buffer.
        /// </summary>
        /// <param name="value">The long to write.</param>
        /// <param name="bitCount">How many bits to write it with.</param>
        public void Write(ulong value, int bitCount = 64) {
            if (bitCount < 1 || bitCount > 64) {
                throw new ArgumentOutOfRangeException("BitCount", "Must be greater than 0, and less than 64.");
            }

            ValidateWriteAction(bitCount);

            byte[] b = SerializeUtils.Serialize(value);
            BitManipulator.WriteBits(bytes, b, currentIndex, bitCount);

            currentIndex += bitCount;
        }

        /// <summary>
        /// Write a float to the buffer.
        /// </summary>
        /// <param name="value">The float to write.</param>
        public void Write(float value) {
            ValidateWriteAction(32);

            byte[] b = SerializeUtils.Serialize(value);
            BitManipulator.WriteBits(bytes, b, currentIndex, 32);

            currentIndex += 32;
        }

        /// <summary>
        /// Write a double to the buffer.
        /// </summary>
        /// <param name="value">The double to write.</param>
        public void Write(double value) {
            ValidateWriteAction(64);

            byte[] b = SerializeUtils.Serialize(value);
            BitManipulator.WriteBits(bytes, b, currentIndex, 64);

            currentIndex += 64;
        }

        /// <summary>
        /// Write a bool to the buffer in the form of a bit.
        /// </summary>
        /// <param name="value">The bool to write.</param>
        public void Write(bool value) {
            ValidateWriteAction(8);

            byte[] b = SerializeUtils.Serialize(value);
            BitManipulator.WriteBits(bytes, b, currentIndex, 8);

            currentIndex += 8;
        }

        /// <summary>
        /// Write a char to the buffer.
        /// </summary>
        /// <param name="value">The char to write.</param>
        public void Write(char value) {
            ValidateWriteAction(8);

            byte[] b = SerializeUtils.Serialize(value);
            BitManipulator.WriteBits(bytes, b, currentIndex, 8);

            currentIndex += 8;
        }

        /// <summary>
        /// Write a string to the buffer.
        /// </summary>
        /// <param name="value">The string to write.</param>
        public void Write(string value) {
            if (value == null) {
                return;
            }

            byte[] b = SerializeUtils.Serialize(value);

            ValidateWriteAction(b.Length * 8);
            BitManipulator.WriteBits(bytes, b, currentIndex, b.Length * 8);

            currentIndex += b.Length * 8;
        }

        /// <summary>
        /// Write an SerializableObject to the buffer.
        /// </summary>
        /// <param name="value">The SerializableObject to write.</param>
        public void Write(SerializableObject value) {
            if (value == null) {
                return;
            }

            byte[] objBytes = value.Serialize();
            ValidateWriteAction((objBytes.Length) * 8);

            //Then write the object to the buffer
            BitManipulator.WriteBits(bytes, objBytes, currentIndex, objBytes.Length * 8);
            currentIndex += objBytes.Length * 8;
        }

        /// <summary>
        /// Write a byte[] to the buffer.
        /// </summary>
        /// <param name="value">The byte[] to write.</param>
        public void Write(byte[] value) {
            //Don't write nothing
            if(value == null || value.Length == 0) {
                return;
            }

            ValidateWriteAction(value.Length * 8);
            BitManipulator.WriteBits(bytes, value, currentIndex, value.Length * 8);

            currentIndex += value.Length * 8;
        }

        /// <summary>
        /// Write a byte list to the buffer.
        /// </summary>
        /// <param name="value"></param>
        public void Write(List<byte> value) {
            //Don't attempt to write nothing.
            if(value == null || value.Count == 0) {
                return;
            }

            ValidateWriteAction(value.Count * 8);
            BitManipulator.WriteBits(bytes, value.ToArray(), currentIndex, value.Count * 8);

            currentIndex += value.Count * 8;
        }

        /// <summary>
        /// Write a DateTime to the buffer.
        /// </summary>
        /// <param name="value">The DateTime to write.</param>
        public void Write(DateTime value) {
            if(value == null) {
                return;
            }

            ValidateWriteAction(64);

            byte[] b = SerializeUtils.Serialize(value);
            BitManipulator.WriteBits(bytes, b, currentIndex, 64);

            currentIndex += 64;
        }

        /// <summary>
        /// Write an IPEndPoint to the buffer.
        /// </summary>
        /// <param name="value">The IPEndPoint to write.</param>
        public void Write(IPEndPoint value) {
            if (value == null) {
                return;
            }

            byte[] b = SerializeUtils.Serialize(value);
            ValidateWriteAction(b.Length * 8);

            BitManipulator.WriteBits(bytes, b, currentIndex, b.Length * 8);
            currentIndex += b.Length * 8;
        }

        /// <summary>
        /// Write a Vector2 to the buffer. This takes 8 bytes.
        /// </summary>
        /// <param name="value">The Vector2 to write.</param>
        public void Write(Vector2 value) {
            ValidateWriteAction(64);

            byte[] b = SerializeUtils.Serialize(value);
            BitManipulator.WriteBits(bytes, b, currentIndex, 64);

            currentIndex += 64;
        }

        /// <summary>
        /// Write a Vector3 to the buffer. This takes 12 bytes.
        /// </summary>
        /// <param name="value">The Vector3 to write.</param>
        public void Write(Vector3 value) {
            ValidateWriteAction(96);

            byte[] b = SerializeUtils.Serialize(value);
            BitManipulator.WriteBits(bytes, b, currentIndex, 96);

            currentIndex += 96;
        }

        /// <summary>
        /// Write a Vector2Int to the buffer. This takes 8 bytes.
        /// </summary>
        /// <param name="value">The Vector2Int to write.</param>
        /// <param name="bitCount">How many bits to write each component with.</param>
        public void Write(Vector2Int value, int bitCount = 64) {
            if (bitCount < 1 || bitCount > 64) {
                throw new ArgumentOutOfRangeException("BitCount", "Must be greater than 0, and less than 64.");
            }

            ValidateWriteAction(bitCount);

            byte[] b = SerializeUtils.Serialize(value);
            BitManipulator.WriteBits(bytes, b, currentIndex, 64);

            currentIndex += 64;
        }

        /// <summary>
        /// Write a Vector3Int to the buffer. This takes 12 bytes.
        /// </summary>
        /// <param name="value">The Vector2Int to write.</param>
        /// <param name="bitCount">How many bits to write each component with.</param>
        public void Write(Vector3Int value, int bitCount = 96) {
            if (bitCount < 1 || bitCount > 96) {
                throw new ArgumentOutOfRangeException("BitCount", "Must be greater than 0, and less than 96.");
            }

            ValidateWriteAction(bitCount);

            byte[] b = SerializeUtils.Serialize(value);
            BitManipulator.WriteBits(bytes, b, currentIndex, bitCount);

            currentIndex += bitCount;
        }

        /// <summary>
        /// Write a Vect2Int to the buffer. This takes 8 bytes.
        /// </summary>
        /// <param name="value">The Vect2Int to write.</param>
        /// <param name="bitCount">How many bits to write each component with.</param>
        public void Write(Vect2Int value, int bitCount = 64) {
            if (bitCount < 1 || bitCount > 64) {
                throw new ArgumentOutOfRangeException("BitCount", "Must be greater than 0, and less than 64.");
            }

            ValidateWriteAction(bitCount);

            byte[] b = SerializeUtils.Serialize(value, bitCount);
            BitManipulator.WriteBits(bytes, b, currentIndex, bitCount);

            currentIndex += bitCount;
        }

        /// <summary>
        /// Write a Vect3Int to the buffer. This takes 12 bytes.
        /// </summary>
        /// <param name="value">The Vect3Int to write.</param>
        /// <param name="bitCount">How many bits to write each component with.</param>
        public void Write(Vect3Int value, int bitCount = 96) {
            if (bitCount < 1 || bitCount > 96) {
                throw new ArgumentOutOfRangeException("BitCount", "Must be greater than 0, and less than 96.");
            }

            ValidateWriteAction(bitCount);

            byte[] b = SerializeUtils.Serialize(value, bitCount);
            BitManipulator.WriteBits(bytes, b, currentIndex, bitCount);

            currentIndex += bitCount;
        }

        /// <summary>
        /// Write a Color16 to the buffer. This takes 2 bytes.
        /// </summary>
        public void Write(Color16 value) {
            ValidateWriteAction(16);

            byte[] b = SerializeUtils.Serialize(value);
            BitManipulator.WriteBits(bytes, b, currentIndex, 16);

            currentIndex += 16;
        }

        /// <summary>
        /// Write a Color32 to the buffer. This takes 4 bytes.
        /// </summary>
        /// <param name="value">The color to write.</param>
        public void Write(Color32 value) {
            ValidateWriteAction(32);

            byte[] b = SerializeUtils.Serialize(value);
            BitManipulator.WriteBits(bytes, b, currentIndex, 32);

            currentIndex += 32;
        }

        /// <summary>
        /// Returns the pointer back to a regular byte index.
        /// </summary>
        public void WritePadBits() {
            throw new NotImplementedException();
        }
        #endregion
    }
}
