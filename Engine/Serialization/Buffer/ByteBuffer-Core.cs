using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Utilities;

namespace Voxelated.Serialization {
    /// <summary>
    /// Dynamically adjustable byte collection for
    /// serializing data over the network.
    public partial class ByteBuffer {
        #region Constants 
        /// <summary>
        /// Default message to display when an out of bounds read is called.
        /// </summary>
        private const string ReadErrorMessage = "Trying to read bytes that exceed the Byte Buffers Size. Calling Read() twice?";

        /// <summary>
        /// Default start size for a byte buffer.
        /// </summary>
        private const int StartByteSize = 4;
        #endregion

        #region Properties
        /// <summary>
        /// If the buffer is in readonly mode.
        /// </summary>
        public bool IsReadOnly { get; protected set; }

        /// <summary>
        /// The number of bytes in the buffer.
        /// </summary>
        public int ByteLength { get { return bytes.Length; } }

        /// <summary>
        /// The current position of the pointer (in bits).
        /// </summary>
        public int PointerIndex { get { return currentIndex; } }
        #endregion

        #region Members
        /// <summary>
        /// The next available byte for writing to.
        /// </summary>
        private int currentIndex;

        /// <summary>
        /// How many bits are currently in the buffer.
        /// </summary>
        private int currentLength;

        /// <summary>
        /// The bytes stored in the buffer. First four bytes is 
        /// always length.
        /// </summary>
        private byte[] bytes;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new byte buffer. Initial size is 256 bytes,
        /// but it will expand as needed. For optimal performance
        /// use the constructor that accepts size parameter.
        /// </summary>
        public ByteBuffer(bool readOnly = false) {
            //Create the new byte array and store it's size in it.
            bytes = new byte[StartByteSize];
            currentIndex = 0;
            currentLength = StartByteSize * 8;
            IsReadOnly = readOnly;
        }

        /// <summary>
        /// Create a new buffer with a predefined size.
        /// </summary>
        /// <param name="size">How many bytes the buffer will
        /// be initialized to. In bits</param>
        public ByteBuffer(int bitLength, bool readOnly = false) {
            int byteLength = BitManipulator.ByteCountForBits(bitLength);

            bytes = new byte[byteLength];
            currentIndex = 0;
            currentLength = bitLength;
            IsReadOnly = readOnly;
        }

        /// <summary>
        /// Create a new byte buffer from a byte array.
        /// </summary>
        /// <param name="bytes">The byte array to convert
        /// into the byte buffer.</param>
        public ByteBuffer(byte[] bytes, bool readOnly = true) {
            this.bytes = bytes;
            currentIndex = 0;
            currentLength = bytes.Length * 8;
            IsReadOnly = readOnly;
        }

        /// <summary>
        /// Build a buffer from a subarray of a byte[].
        /// </summary>
        /// <param name="bytes">The encoded byte buffer.</param>
        /// <param name="startBit">The first bit of the buffer.</param>
        /// <param name="bitLength">How many bits long it is.</param>
        public ByteBuffer(byte[] bytes, int startBit, int bitLength) {
            this.bytes = SerializeUtils.GetBytes(bytes, startBit, bitLength);
            currentIndex = 0;       //This is not a bug. DON'T do: currentIndex = startBit
            currentLength = bitLength;
            IsReadOnly = true;
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Expand the buffer by an additional 8 bytes
        /// </summary>
        private void Expand() {
            currentLength += 64;
            Array.Resize(ref bytes, bytes.Length + 8);
        }

        /// <summary>
        /// Expand the buffer by an additional number of bytes.
        /// </summary>
        /// <param name="byteAmount">The amount of bytes to append to it.</param>
        private void Expand(int byteAmount) {
            currentLength += byteAmount * 8;
            Array.Resize(ref bytes, bytes.Length + byteAmount);
        }

        /// <summary>
        /// If the buffer has enough space to fit an additional
        /// number of bits.
        /// </summary>
        /// <param name="bitCount">The number of bits to see if it will fit.</param>
        /// <returns>True if the buffer has space.</returns>
        private bool HasSpaceFor(int bitCount) {
            return (currentLength - currentIndex) >= bitCount;
        }

        /// <summary>
        /// Validates that the buffer can be written to and if
        /// needed it resizes the buffer to fit the object.
        /// </summary>
        /// <param name="bitCount">The number of bits to check for space.</param>
        private void ValidateWriteAction(int bitCount) {
            if (IsReadOnly) {
                throw new InvalidOperationException("Buffer is in Read-Only mode. Cannot be written to.");
            }

            if (!HasSpaceFor(bitCount)) {
                Expand(BitManipulator.ByteCountForBits(bitCount));
            }
        }

        /// <summary>
        /// Validate if the buffer can handle a read operation of the
        /// specified bit count.
        /// </summary>
        /// <param name="bitCount">The number of bits to check for reading.</param>
        private void ValidateReadAction(int bitCount) {
            if (!HasSpaceFor(bitCount)) {
                throw new IndexOutOfRangeException("Buffer does not contain enough bits to read!");
            }
        }
        #endregion

        #region Pointer Methods
        /// <summary>
        /// Manually set the current bit index pointer.
        /// Don't modify this unless you know what your doing.
        /// </summary>
        /// <param name="index">The bit position to set
        /// the pointer to.</param>
        public void SetPointerIndex(int index) {
            currentIndex = index;
        }

        /// <summary>
        /// Reset the current bit index pointer back to the start.
        /// </summary>
        public void ResetPointerIndex() {
            currentIndex = 0;
        }

        /// <summary>
        /// Skip a fixed number of bits when writing to 
        /// and give them a default value of 0.
        /// </summary>
        /// <param name="bitCount">The number of bits to
        /// skip.</param>
        public void SkipWritingBits(int bitCount) {
            ValidateWriteAction(bitCount);

            currentIndex += bitCount;
        }

        /// <summary>
        /// Skip reading a fixed number of bits.
        /// </summary>
        /// <param name="bitCount">The number of bits
        /// to ignore.</param>
        public void SkipReadingBits(int bitCount) {
            ValidateReadAction(bitCount);

            currentIndex += bitCount;
        }
        #endregion

        #region Serialization
        /// <summary>
        /// Convert the byte buffer into a byte[]
        /// for sending out over the network
        /// </summary>
        /// <returns></returns>
        public virtual byte[] Serialize() {
            return bytes;
        }
        #endregion
    }
}
