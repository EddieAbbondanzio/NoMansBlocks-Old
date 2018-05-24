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
    /// The Peek methods of the Byte Buffer.
    /// </summary>
    public partial class ByteBuffer {
        #region Peek Methods
        /// <summary>
        /// Read the current byte from the buffer but don't increment the
        /// pointer.
        /// </summary>
        /// <param name="bitCount">The number of bits to peek.</param>
        /// <returns>The current byte value.</returns>
        public byte PeekByte(int bitCount = 8) {
            ValidateReadAction(bitCount);

            return SerializeUtils.GetByte(bytes, currentIndex, bitCount);
        }

        /// <summary>
        /// Read the current ushort from the buffer but don't
        /// increment the pointer.
        /// </summary>
        /// <param name="bitCount">The number of bits to peek.</param>
        /// <returns>The current ushort value.</returns>
        public short PeekShort(int bitCount = 16) {
            ValidateReadAction(bitCount);

            return (short)PeekUShort();
        }

        /// <summary>
        /// Read the current ushort from the buffer but don't
        /// increment the pointer.
        /// </summary>
        /// <param name="bitCount">The number of bits to peek.</param>
        /// <returns>The current ushort value.</returns>
        public ushort PeekUShort(int bitCount = 16) {
            ValidateReadAction(bitCount);

            return SerializeUtils.GetUShort(bytes, currentIndex, bitCount);
        }

        /// <summary>
        /// Read the current int32 from the buffer but don't increment
        /// the bit pointer.
        /// </summary>
        /// <param name="bitCount">The number of bits to peek.</param>
        /// <returns>The current integer value.</returns>
        public int PeekInt(int bitCount = 32) {
            ValidateReadAction(bitCount);

            return (int)PeekUInt();
        }

        /// <summary>
        /// Read the current uint32 from the buffer but don't increment
        /// the bit pointer.
        /// </summary>
        /// <param name="bitCount">The number of bits to peek.</param>
        /// <returns>The current uint value.</returns>
        public uint PeekUInt(int bitCount = 32) {
            ValidateReadAction(bitCount);

            return SerializeUtils.GetUInt(bytes, currentIndex, bitCount);
        }

        /// <summary>
        /// Read the current long from the buffer but don't increment
        /// the bit pointer.
        /// </summary>
        /// <param name="bitCount">The number of bits to peek.</param>
        /// <returns>The long at the current bit index.</returns>
        public long PeekLong(int bitCount = 64) {
            ValidateReadAction(bitCount);

            return (long)PeekULong();
        }

        /// <summary>
        /// Read the current ulong from the buffer but don't increment
        /// the bit pointer.
        /// </summary>
        /// <param name="bitCount">The number of bits to peek.</param>
        /// <returns>The long at the current bit index.</returns>
        public ulong PeekULong(int bitCount = 64) {
            ValidateReadAction(bitCount);

            return SerializeUtils.GetULong(bytes, currentIndex, bitCount);
        }

        /// <summary>
        /// Read the current float from the buffer but don't increment
        /// the bit pointer.
        /// </summary>
        /// <returns>The float at the current bit index.</returns>
        public float PeekFloat() {
            return SerializeUtils.GetFloat(bytes, currentIndex);
        }

        /// <summary>
        /// Read the current double from the buffer but don't increment
        /// the bit pointer.
        /// </summary>
        /// <returns>The double at the current bit index.</returns>
        public double PeekDouble() {
            return SerializeUtils.GetDouble(bytes, currentIndex);
        }


        /// <summary>
        /// Read a bool from the buffer but don't increment the pointer.
        /// </summary>
        /// <returns>The current bool value.</returns>
        public bool PeekBool() {
            return SerializeUtils.GetBool(bytes, currentIndex);
        }

        /// <summary>
        /// Read the current string from the buffer but don't increment
        /// the bit pointer.
        /// </summary>
        /// <returns>The string at the current bit index.</returns>
        public string PeekString() {
            return SerializeUtils.GetString(bytes, currentIndex);
        }

        /// <summary>
        /// Read a char from the buffer but don't increment the pointer.
        /// </summary>
        /// <returns>The current char value.</returns>
        public char PeekChar() {
            return SerializeUtils.GetChar(bytes, currentIndex);
        }

        /// <summary>
        /// Retrieves the fixed amount of bytes starting
        /// at the current bit index, but does not shift the pointer.
        /// </summary>
        /// <param name="byteCount">Number of bytes to peek.</param>
        /// <returns>The current bytes.</returns>
        public byte[] PeekBytes(int byteCount) {
            return SerializeUtils.GetBytes(bytes, currentIndex, byteCount);
        }

        /// <summary>
        /// Read the current DateTime from the buffer but don't increment
        /// the bit pointer.
        /// </summary>
        /// <returns>The DateTime at the current bit index.</returns>
        public DateTime PeekDateTime() {
            return SerializeUtils.GetDateTime(bytes, currentIndex);
        }

        /// <summary>
        /// Read the current IPEndPoint from the buffer but don't increment
        /// the bit pointer.
        /// </summary>
        /// <returns>The IPEndPoint at the current bit index.</returns>
        public IPEndPoint PeekIPEndPoint() {
            return SerializeUtils.GetIPEndPoint(bytes, currentIndex);
        }


        /// <summary>
        /// Read the current SerializableObject from the buffer but don't increment
        /// the bit pointer.
        /// </summary>
        /// <returns>The SerializableObject at the current bit index.</returns>
        public SerializableObject PeekSerializableObject() {
            SerializableObject value = SerializeUtils.GetSerializableObject(bytes, currentIndex);
            return value;
        }

        /// <summary>
        /// Read the current Vector2 from the buffer but don't increment
        /// the bit pointer.
        /// </summary>
        /// <returns>The Vector2 at the current bit index.</returns>
        public Vector2 PeekVector2() {
            ValidateReadAction(64);

            return SerializeUtils.GetVector2(bytes, currentIndex);
        }

        /// <summary>
        /// Read the current Vector3 from the buffer but don't increment
        /// the bit pointer.
        /// </summary>
        /// <returns>The Vector3 at the current bit index.</returns>
        public Vector3 PeekVector3() {
            ValidateReadAction(96);

            return SerializeUtils.GetVector3(bytes, currentIndex);
        }

        /// <summary>
        /// Read the current Vector2Int from the buffer but don't increment
        /// the bit pointer.
        /// </summary>
        /// <param name="bitCount">The number of bits to peek.</param>
        /// <returns>The Vector2Int at the current bit index.</returns>
        public Vector2Int PeekVector2Int(int bitCount = 32) {
            ValidateReadAction(bitCount);

            return SerializeUtils.GetVector2Int(bytes, currentIndex);
        }

        /// <summary>
        /// Read the current Vector3Int from the buffer but don't increment
        /// the bit pointer.
        /// </summary>
        /// <param name="bitCount">The number of bits to peek.</param>
        /// <returns>The Vector3Int at the current bit index.</returns>
        public Vector3Int PeekVector3Int(int bitCount = 32) {
            ValidateReadAction(bitCount);

            return SerializeUtils.GetVector3Int(bytes, currentIndex);
        }

        /// <summary>
        /// Read the current Vect2Int from the buffer but don't increment
        /// the bit pointer.
        /// </summary>
        /// <param name="bitCount">The number of bits to peek.</param>
        /// <returns>The Vect2Int at the current bit index.</returns>
        public Vect2Int PeekVect2Int(int bitCount = 32) {
            ValidateReadAction(bitCount);

            return SerializeUtils.GetVect2Int(bytes, currentIndex);
        }

        /// <summary>
        /// Read the current Vect3Int from the buffer but don't increment
        /// the bit pointer.
        /// </summary>
        /// <param name="bitCount">The number of bits to peek.</param>
        /// <returns>The Vect3Int at the current bit index.</returns>
        public Vect3Int PeekVect3Int(int bitCount = 32) {
            ValidateReadAction(bitCount);

            return SerializeUtils.GetVect3Int(bytes, currentIndex);
        }

        /// <summary>
        /// Read a Color32 from the buffer at the current pointer.
        /// </summary>
        /// <returns>The color that was read in.</returns>
        public Color16 PeekColor16() {
            ValidateReadAction(16);

            return SerializeUtils.GetColor16(bytes, currentIndex);
        }

        /// <summary>
        /// Read a Color32 from the buffer at the current pointer.
        /// </summary>
        /// <returns>The color that was read in.</returns>
        public Color32 PeekColor32() {
            ValidateReadAction(32);

            return SerializeUtils.GetColor32(bytes, currentIndex);
        }


        #endregion
    }

}
