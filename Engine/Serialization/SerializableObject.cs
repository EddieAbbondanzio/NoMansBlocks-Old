using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network.Lobby;
using Voxelated.Utilities;

namespace Voxelated.Serialization {
    /// <summary>
    /// Base class for creating objects that can be serialized
    /// into byte arrays and back.
    /// </summary>
    public abstract class SerializableObject {
        #region Properties
        /// <summary>
        /// Flag to indicate what kind of object
        /// is being serialized.
        /// </summary>
        protected abstract SerializableType Type { get; }

        /// <summary>
        /// How many bytes are needed to serialize the object.
        /// This boost performance by not having to
        /// resize the buffer that it is being written to.
        /// If size is unknow leave at 0.
        /// </summary>
        protected virtual int ByteSize { get; }
        #endregion

        #region Publics
        /// <summary>
        /// Serialize the object into a byte
        /// array that can be used to rebuild it later
        /// on.
        /// </summary>
        /// <returns>The encoded object.</returns>
        public byte[] Serialize() {
            ByteBuffer buffer;

            if(ByteSize == 0) {
                buffer = new ByteBuffer();
            }
            else {
                buffer = new ByteBuffer(ByteSize * 8 + 32);
            }

            //Write the identifier, then the contents to the buffer.
            buffer.Write((byte)Type);
            buffer.SetPointerIndex(32);

            SerializeContent(buffer);

            //Jump back to start and write the number of bytes used.
            //4 is subtracted from to account for the 3 of byte length
            //and the byte 1 indentifier.
            buffer.SetPointerIndex(8);
            buffer.Write(buffer.ByteLength - 4, 24);

            return buffer.Serialize();
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Write the content of the object to the byte buffer.
        /// </summary>
        /// <param name="buffer">The byte buffer to write to.</param>
        protected abstract void SerializeContent(ByteBuffer buffer);
        #endregion

        #region Statics
        /// <summary>
        /// Retrieve the serializable object type from the
        /// encoded object header.
        /// </summary>
        /// <param name="bytes">The byte array containing the object.</param>
        /// <param name="startBit">The object's first bit position.</param>
        /// <returns>The object's type.</returns>
        public static SerializableType GetType(byte[] bytes, int startBit) {
            byte b = SerializeUtils.GetByte(bytes, startBit);

            if(Enum.IsDefined(typeof(SerializableType), b)) {
                return (SerializableType)b;
            }
            else {
                throw new ArgumentOutOfRangeException(string.Format("Value: {0} is not a defined Serializable Type.", b));
            }
        }

        /// <summary>
        /// Retrieves the byte count from the serialized object header.
        /// </summary>
        /// <param name="bytes">The bytes of the object.</param>
        /// <param name="startBit">The starting bit of the object's
        /// header.</param>
        /// <returns>The number of bytes used.</returns>
        public static int GetByteCount(byte[] bytes, int startBit) {
            return SerializeUtils.GetInt(bytes, startBit + 8, 24);
        }

        /// <summary>
        /// Retrieves the content bytes of the serializable
        /// object.
        /// </summary>
        /// <param name="bytes">The bytes of the object.</param>
        /// <param name="startBit">The first bit of the header.</param>
        /// <param name="type">Only used to valdiate the bytes.</param>
        /// <returns>The object's contents via a buffer.</returns>
        public static ByteBuffer GetContent(byte[] bytes, int startBit, SerializableType type) {
            //Ensure it's actually a player
            if (GetType(bytes, startBit) != type) {
                throw new ArgumentException("Incorrect byte data passed for Serializable Type: " + type.ToString());
            }

            //Prep the byte buffer to read from.
            int bitCount = GetByteCount(bytes, startBit) * 8 ;
            return new ByteBuffer(bytes, startBit + 32, bitCount);
        }

        /// <summary>
        /// Rebuild a serializable object from it's
        /// byte array.
        /// </summary>
        /// <param name="buffer">The buffer holding the object.</param>
        /// <returns>The rebuilt object.</returns>
        public static SerializableObject RebuildObject(ByteBuffer buffer) {
            SerializableType objectType = (SerializableType)buffer.PeekByte();

            SerializableObject obj = null;
            switch (objectType) {
                case SerializableType.NetPlayer:
                    obj = new NetPlayer(buffer);
                    break;

                case SerializableType.NetPlayerStats:
                    obj = new NetPlayerStats(buffer);
                    break;

                case SerializableType.NetTeam:
                    obj = new NetTeam(buffer);
                    break;

                case SerializableType.NetLobbySettings:
                    obj = new NetLobbySettings(buffer);
                    break;

                case SerializableType.TimerFactory:
                    obj = new TimerFactory(buffer);
                    break;
            }

            return obj;
        }
        #endregion
    }
}
