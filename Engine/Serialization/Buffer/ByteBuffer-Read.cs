using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Voxelated.Network;
using Voxelated.Network.Lobby;
using Voxelated.Utilities;

namespace Voxelated.Serialization {
    /// <summary>
    /// The Read methods of the ByteBuffer class.
    /// </summary>
    public partial class ByteBuffer {
        #region Read Methods
        /// <summary>
        /// Read a byte from the buffer.
        /// </summary>
        /// <param name="bitCount">How many bits to read the value from with.</param>
        /// <returns>The current byte.</returns>
        public byte ReadByte(int bitCount = 8) {
            ValidateReadAction(bitCount);

            byte value = SerializeUtils.GetByte(bytes, currentIndex, bitCount);
            currentIndex += bitCount;

            return value;
        }

        /// <summary>
        /// Read a short from the buffer.
        /// </summary>
        /// <param name="bitCount">How many bits to read the value from with.</param>
        /// <returns>The short at the current buffer index.</returns>
        public short ReadShort(int bitCount = 16) {
            ValidateReadAction(bitCount);

            short value = SerializeUtils.GetShort(bytes, currentIndex, bitCount);
            currentIndex += bitCount;

            return value;
        }

        /// <summary>
        /// Read a ushort from the buffer.
        /// </summary>
        /// <param name="bitCount">How many bits to read the value from with.</param>
        /// <returns>The ushort at the current buffer index.</returns>
        public ushort ReadUShort(int bitCount = 16) {
            ValidateReadAction(bitCount);

            ushort value = SerializeUtils.GetUShort(bytes, currentIndex, bitCount);
            currentIndex += bitCount;

            return value;
        }

        /// <summary>
        /// Read a int from the buffer.
        /// </summary>
        /// <param name="bitCount">How many bits to read the value from with.</param>
        /// <returns>The int at the current buffer index.</returns>
        public int ReadInt(int bitCount = 32) {
            ValidateReadAction(bitCount);

            int value = SerializeUtils.GetInt(bytes, currentIndex, bitCount);
            currentIndex += bitCount;

            return value;
        }

        /// <summary>
        /// Read a uint from the buffer.
        /// </summary>
        /// <param name="bitCount">How many bits to read the value from with.</param>
        /// <returns>The uint at the current buffer index.</returns>
        public uint ReadUInt(int bitCount = 32) {
            ValidateReadAction(bitCount);

            uint value = SerializeUtils.GetUInt(bytes, currentIndex, bitCount);
            currentIndex += bitCount;

            return value;
        }

        /// <summary>
        /// Read a long from the bugffer.
        /// </summary>
        /// <param name="bitCount">How many bits to read the value from with.</param>
        /// <returns>The long that was read from the buffer.</returns>
        public long ReadLong(int bitCount = 64) {
            ValidateReadAction(bitCount);

            long value = SerializeUtils.GetLong(bytes, currentIndex, bitCount);
            currentIndex += bitCount;

            return value;
        }

        /// <summary>
        /// Read a ulong from the bugffer.
        /// </summary>
        /// <param name="bitCount">How many bits to read the value from with.</param>
        /// <returns>The long that was read from the buffer.</returns>
        public ulong ReadULong(int bitCount = 64) {
            ValidateReadAction(bitCount);

            ulong value = SerializeUtils.GetULong(bytes, currentIndex, bitCount);
            currentIndex += bitCount;

            return value;
        }

        /// <summary>
        /// Read a float from the buffer.
        /// </summary>
        /// <returns>The float at the current buffer index.</returns>
        public float ReadFloat() {
            ValidateReadAction(32);

            float value = SerializeUtils.GetFloat(bytes, currentIndex);
            currentIndex += 32;

            return value;
        }

        /// <summary>
        /// Read a double from the buffer.
        /// </summary>
        /// <returns>The uint at the current buffer index.</returns>
        public double ReadDouble() {
            ValidateReadAction(64);

            double value = SerializeUtils.GetDouble(bytes, currentIndex);
            currentIndex += 64;

            return value;
        }

        /// <summary>
        /// Read a bool from the buffer.
        /// </summary>
        /// <returns>The current bool.</returns>
        public bool ReadBool() {
            ValidateReadAction(8);

            bool value = SerializeUtils.GetBool(bytes, currentIndex);
            currentIndex += 8;

            return value;
        }

        /// <summary>
        /// Read a character from the buffer.
        /// </summary>
        /// <returns>The current character.</returns>
        public char ReadChar() {
            ValidateReadAction(8);

            char value = SerializeUtils.GetChar(bytes, currentIndex);
            currentIndex += 8;

            return value;
        }

        /// <summary>
        /// Read a string from the buffer.
        /// </summary>
        /// <returns>The string at the current buffer index.</returns>
        public string ReadString() {
            string value = SerializeUtils.GetString(bytes, currentIndex);
            currentIndex += (value.Length + 2) * 8;

            return value;
        }

        /// <summary>
        /// Read a byte[] from the buffer.
        /// </summary>
        /// <param name="bitCount">The number of bits to read.</param>
        /// <returns>The byte subarray from the buffer.</returns>
        public byte[] ReadBytes(int bitCount) {
            ValidateReadAction(bitCount);

            byte[] value = SerializeUtils.GetBytes(bytes, currentIndex, bitCount);
            currentIndex += bitCount * 8;

            return value;
        }

        /// <summary>
        /// Read a DateTime from the buffer.
        /// </summary>
        /// <returns>The DateTime at the current buffer index.</returns>
        public DateTime ReadDateTime() {
            ValidateReadAction(64);

            DateTime value = SerializeUtils.GetDateTime(bytes, currentIndex);
            currentIndex += 64;

            return value;
        }

        /// <summary>
        /// Read a IPEndPoint from the buffer.
        /// </summary>
        /// <returns>The IPEndPoint at the current buffer index.</returns>
        public IPEndPoint ReadIPEndPoint() {
            IPEndPoint value = SerializeUtils.GetIPEndPoint(bytes, currentIndex);

            //Lol fix this junk
            currentIndex += SerializeUtils.Serialize(value).Length * 8;
            return value;
        }

        /// <summary>
        /// Read an ISeralizable object from the buffer. Caller is 
        /// responsible for casting it into the proper object.
        /// </summary>
        /// <returns>The bytes of the ISerializable object at the current bit index.</returns>
        public SerializableObject ReadSerializableObject() {
            SerializableType objectType = (SerializableType)PeekByte();

            SerializableObject obj = null;
            switch (objectType) {
                case SerializableType.NetPlayer:
                    obj = new NetPlayer(this);
                    break;

                case SerializableType.NetPlayerStats:
                    obj = new NetPlayerStats(this);
                    break;

                case SerializableType.NetTeam:
                    obj = new NetTeam(this);
                    break;

                case SerializableType.NetLobbySettings:
                    obj = new NetLobbySettings(this);
                    break;
            }

            return obj;
        }

        /// <summary>
        /// Read a Vector2 from the buffer.
        /// </summary>
        /// <returns>The Vector2 at the current buffer index.</returns>
        public Vector2 ReadVector2() {
            ValidateReadAction(64);

            Vector2 value = SerializeUtils.GetVector2(bytes, currentIndex);
            currentIndex += 64;

            return value;
        }

        /// <summary>
        /// Read a Vector3 from the buffer.
        /// </summary>
        /// <returns>The Vector3 at the current buffer index.</returns>
        public Vector3 ReadVector3() {
            ValidateReadAction(96);

            Vector3 value = SerializeUtils.GetVector3(bytes, currentIndex);
            currentIndex += 96;

            return value;
        }

        /// <summary>
        /// Read a Vector2Int from the buffer.
        /// </summary>
        /// <param name="bitCount">How many bits to read the value from with.</param>
        /// <returns>The Vector2Int at the current buffer index.</returns>
        public Vector2Int ReadVector2Int(int bitCount = 64) {
            ValidateReadAction(bitCount);

            Vector2Int value = SerializeUtils.GetVector2Int(bytes, currentIndex);
            currentIndex += bitCount;

            return value;
        }

        /// <summary>
        /// Read a Vector3Int from the buffer.
        /// </summary>
        /// <param name="bitCount">How many bits to read the value from with.</param>
        /// <returns>The Vector3Int at the current buffer index.</returns>
        public Vector3Int ReadVector3Int(int bitCount = 96) {
            ValidateReadAction(bitCount);

            Vector3Int value = SerializeUtils.GetVector3Int(bytes, currentIndex);
            currentIndex += bitCount;

            return value;
        }

        /// <summary>
        /// Read a Vect2Int from the buffer.
        /// </summary>
        /// <param name="bitCount">How many bits to read the value from with.</param>
        /// <returns>The Vect2Int at the current buffer index.</returns>
        public Vect2Int ReadVect2Int(int bitCount = 64) {
            ValidateReadAction(bitCount);

            Vect2Int value = SerializeUtils.GetVect2Int(bytes, currentIndex);
            currentIndex += bitCount;

            return value;
        }

        /// <summary>
        /// Read a Vect3Int from the buffer.
        /// </summary>
        /// <param name="bitCount">How many bits to read the value from with.</param>
        /// <returns>The Vect3Int at the current buffer index.</returns>
        public Vect3Int ReadVect3Int(int bitCount = 96) {
            ValidateReadAction(bitCount);

            Vect3Int value = SerializeUtils.GetVect3Int(bytes, currentIndex);
            currentIndex += bitCount;

            return value;
        }

        /// <summary>
        /// Read a Color16 from the buffer at the current pointer.
        /// </summary>
        /// <returns>The color that was read in.</returns>
        public Color16 ReadColor16() {
            ValidateReadAction(16);

            Color16 value = SerializeUtils.GetColor16(bytes, currentIndex);
            currentIndex += 16;

            return value;
        }

        /// <summary>
        /// Read a Color32 from the buffer at the current pointer.
        /// </summary>
        /// <returns>The color that was read in.</returns>
        public Color32 ReadColor32() {
            ValidateReadAction(32);

            Color32 value = SerializeUtils.GetColor32(bytes, currentIndex);
            currentIndex += 32;

            return value;
        }

        /// <summary>
        /// Reads past the buffer bits.
        /// </summary>
        public void ReadPadBits() {
            throw new NotImplementedException();
        }
        #endregion
    }
}
