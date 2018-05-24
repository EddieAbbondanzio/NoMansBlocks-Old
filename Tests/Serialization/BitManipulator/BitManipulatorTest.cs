using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Voxelated.Serialization;

namespace Voxelated.Test.Serialization {
    /// <summary>
    /// Unit Tests for the BitManipulator utility. These verify that
    /// the methods in it work to prevent bigger headaches down the road.
    /// </summary>
    [TestClass]
    public class BitManipulatorTest {
        #region Read Tests
        /// <summary>
        /// Test reading of a single bit from a byte.
        /// </summary>
        [TestMethod]
        public void ReadBitInByteTest() {
            byte b = 16;
            byte val = BitManipulator.ReadBit(b, 4);

            Assert.AreEqual(1, val);
        }

        /// <summary>
        /// Test reading of a single bit from an array.
        /// </summary>
        [TestMethod]
        public void ReadBitInArrayTest() {
            byte[] bytes = new byte[] { 0, 240, 15, 0 };
            byte val = BitManipulator.ReadBit(bytes, 14);

            Assert.AreEqual(1, val);
        }

        /// <summary>
        /// Attempt to read a single normal byte from the array.
        /// </summary>
        [TestMethod]
        public void ReadSingleByteTest() {
            byte[] bytes = new byte[] { 0, 240, 15, 0 };
            byte[] b = BitManipulator.ReadBits(bytes, 8, 8);

            Assert.AreEqual(bytes[1], b[0]);
        }

        /// <summary>
        /// Attempt to read half a byte (nibble) from the second byte in the
        /// array. This should return a value of 15.
        /// </summary>
        [TestMethod]
        public void ReadHalfByteTest() {
            byte[] bytes = new byte[] { 0, 240, 15, 0 };
            byte[] b = BitManipulator.ReadBits(bytes, 12, 4);

            Assert.AreEqual(15, b[0]);
        }

        /// <summary>
        /// Read a byte that starts in one, and finishes in another
        /// byte within the array.
        /// </summary>
        [TestMethod]
        public void ReadByteBetweenTwoTest() {
            byte[] bytes = new byte[] { 0, 240, 15, 0 };
            byte[] b = BitManipulator.ReadBits(bytes, 12, 8);

            Assert.AreEqual(255, b[0]);
        }

        /// <summary>
        /// Read a byte that starts in one, and finishes in another
        /// byte within the array.
        /// </summary>
        [TestMethod]
        public void ReadBitsBetweenTwoTest() {
            byte[] bytes = new byte[] { 0, 240, 15, 0 };
            byte[] b = BitManipulator.ReadBits(bytes, 12, 6);

            Assert.AreEqual(63, b[0]);
        }

        [TestMethod]
        public void ReadMoreBitsTest() {
            byte[] bytes = new byte[] { 0, 240, 15, 0 };
            byte[] b = BitManipulator.ReadBits(bytes, 18, 4);

            Assert.AreEqual(3, b[0]);
        }
        #endregion

        #region Write Tests
        /// <summary>
        /// Test writing of a single bit from a byte.
        /// </summary>
        [TestMethod]
        public void WriteitInByteTest() {
            byte b = 0;
            BitManipulator.WriteBit(ref b, 1, 3);
            Assert.AreEqual(8, b);
        }

        /// <summary>
        /// Test writing of a single bit from an array.
        /// </summary>
        [TestMethod]
        public void WriteBitInArrayTest() {
            byte[] bytes = { 0, 0 };
            BitManipulator.WriteBit(bytes, 1, 11);

            Assert.AreEqual(8, bytes[1]);
        }

        /// <summary>
        /// Test writing a single byte to the array.
        /// </summary>
        [TestMethod]
        public void WriteSingleByteTest() {
            byte[] bytes = new byte[] { 0, 240, 15, 0 };
            BitManipulator.WriteBits(bytes, new byte[] { 15 }, 8, 8);

            Assert.AreEqual(15, bytes[1]);
        }

        /// <summary>
        /// Test writing 2 bytes with a 1 to 1 relationship
        /// </summary>
        [TestMethod]
        public void WriteTwoBytesTest() {
            byte[] bytes = new byte[] { 0, 240, 240, 0 };
            BitManipulator.WriteBits(bytes, new byte[] { 235, 15 }, 0, 12);

            Assert.AreEqual(235, bytes[0]);
            Assert.AreEqual(255, bytes[1]);
        }

        /// <summary>
        /// Tests writing a nibble to the byte array.
        /// </summary>
        [TestMethod]
        public void WriteHalfByteTest() {
            byte[] bytes = new byte[] { 0, 240, 15, 0 };
            BitManipulator.WriteBits(bytes, new byte[] { 15 }, 8, 4);

            Assert.AreEqual(255, bytes[1]);
        }

        /// <summary>
        /// Write a byte that starts in one, and finishes in another
        /// byte within the array.
        /// </summary>
        [TestMethod]
        public void WriteByteBetweenTwoTest() {
            byte[] bytes = new byte[] { 0, 240, 0, 0 };
            BitManipulator.WriteBits(bytes, new byte[] { 240 }, 12, 8);

            Assert.AreEqual(15, bytes[2]);
        }

        /// <summary>
        /// Writes several bits between the two
        /// </summary>
        [TestMethod]
        public void WriteBitsBetweenTwoTest() {
            byte[] bytes = new byte[] { 0, 240, 0, 0 };
            BitManipulator.WriteBits(bytes, new byte[] { 126 }, 15, 7);

            Assert.AreEqual(112, bytes[1]);
            Assert.AreEqual(63, bytes[2]);
        }

        [TestMethod]
        public void WriteLotsOfBitsTest() {
            byte[] bytes = new byte[] { 0, 240, 15, 0 };
            byte[] vals = new byte[] { 15, 15, 255 };
            BitManipulator.WriteBits(bytes, vals, 4, 22);

            Assert.AreEqual(240, bytes[0]);
            Assert.AreEqual(240, bytes[1]);
            Assert.AreEqual(240, bytes[2]);
            Assert.AreEqual(3, bytes[3]);
        }
        #endregion
    }
}
