using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Voxelated.Serialization;
using System.Net;
using Voxelated.Network.Lobby;
using UnityEngine;

namespace Voxelated.Test.Serialization {
    /// <summary>
    /// Unit Tests for the ByteBuffer class.
    /// </summary>
    [TestClass]
    public class ByteBufferTest {
        /// <summary>
        /// Test writing several bools to the buffer and
        /// then retrieve their values.
        /// </summary>
        [TestMethod]
        public void BufferWriteBoolTest() {
            bool[] values = new bool[] { false, true, true, false, false };
            ByteBuffer buffer = new ByteBuffer(16);

            //Write the bools to the buffer.
            for (int i = 0; i < values.Length; i++) {
                buffer.Write(values[i]);
            }

            buffer.ResetPointerIndex();

            //Read back the bools.
            bool[] rebuiltValues = new bool[values.Length];
            for (int j = 0; j < rebuiltValues.Length; j++) {
                rebuiltValues[j] = buffer.ReadBool();
            }

            CollectionAssert.AreEqual(values, rebuiltValues);
        }

        /// <summary>
        /// Test writing a char to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteCharTest() {
            char[] values = new char[] { 'a', 'B', '1', '.' };
            ByteBuffer buffer = new ByteBuffer(16);

            //Write the chars
            for(int i = 0; i < values.Length; i++) {
                buffer.Write(values[i]);
            }

            buffer.ResetPointerIndex();

            for(int i = 0; i < values.Length; i++) {
                char value = buffer.ReadChar();
                Assert.AreEqual(values[i], value);
            }
        }

        /// <summary>
        /// Test writing several bools to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteByteTest() {
            byte[] values = new byte[] { 0, 16, 8, 10, 255 };
            ByteBuffer buffer = new ByteBuffer(16);

            //Write all the bytes to the buffer.
            for(int i = 0; i < values.Length; i++) {
                buffer.Write(values[i]);
            }

            buffer.ResetPointerIndex();

            //Read back the values.
            byte[] rebuiltVals = new byte[values.Length];
            for(int i = 0; i < values.Length; i++) {
                rebuiltVals[i] = buffer.ReadByte();
            }

            CollectionAssert.AreEqual(values, rebuiltVals);
        }

        /// <summary>
        /// Tests writing several bytes with different bit counts and
        /// reading them back.
        /// </summary>
        [TestMethod]
        public void BufferWriteFixedByteTest() {
            byte[] values = new byte[] { 0, 16, 8, 10, 2 };
            ByteBuffer buffer = new ByteBuffer(16);

            //Write all the bytes to the buffer.
            for (int i = 0; i < values.Length; i++) {
                buffer.Write(values[i], 5);
            }

            buffer.ResetPointerIndex();

            //Read back the values.
            byte[] rebuiltVals = new byte[values.Length];
            for (int i = 0; i < values.Length; i++) {
                rebuiltVals[i] = buffer.ReadByte(5);
            }

            CollectionAssert.AreEqual(values, rebuiltVals);
        }

        /// <summary>
        /// Test writing a ushort to the buffer and
        /// then retrieve the values.
        /// </summary>
        [TestMethod]
        public void BufferWriteUShortTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            ushort[] values = { ushort.MinValue, ushort.MaxValue, 100, 32000 };

            //Write the values
            for (int i = 0; i < values.Length; i++) {
                buffer.Write(values[i]);
            }

            buffer.ResetPointerIndex();

            ushort[] rebuiltVals = new ushort[values.Length];
            for(int i = 0; i < values.Length; i++) {
                rebuiltVals[i] = buffer.ReadUShort();
            }

            CollectionAssert.AreEqual(values, rebuiltVals);
        }

        /// <summary>
        /// Write some ushorts to the buffer and read them back.
        /// </summary>
        [TestMethod]
        public void BufferWriteFixedUShortTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            ushort[] values = { ushort.MinValue, ushort.MaxValue, 100, 12000 };
            int[] bitCounts = { 16, 16, 7, 15 };

            //Write the values
            for(int i = 0; i < values.Length; i++) {
                buffer.Write(values[i], bitCounts[i]);
            }

            buffer.ResetPointerIndex();

            ushort[] rebuiltVals = new ushort[values.Length];
            for(int i = 0; i < values.Length; i++) {
                rebuiltVals[i] = buffer.ReadUShort(bitCounts[i]);
            }

            CollectionAssert.AreEqual(values, rebuiltVals);
        }

        /// <summary>
        /// Test writing a short to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteShortTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            short[] values = { short.MinValue, short.MaxValue, 0, 299 };

            //Write the values
            for (int i = 0; i < values.Length; i++) {
                buffer.Write(values[i]);
            }

            buffer.ResetPointerIndex();

            short[] rebuiltVals = new short[values.Length];
            for (int i = 0; i < values.Length; i++) {
                rebuiltVals[i] = buffer.ReadShort();
            }

            CollectionAssert.AreEqual(values, rebuiltVals);
        }

        /// <summary>
        /// Write several shorts with custom bit count to
        /// the buffer and read them back.
        /// </summary>
        [TestMethod]
        public void BufferWriteFixedShortTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            short[] values = { short.MinValue, short.MaxValue, 0, 299 };
            int[] bitCounts = { 16, 16, 7, 15 };

            //Write the values
            for (int i = 0; i < values.Length; i++) {
                buffer.Write(values[i], bitCounts[i]);
            }

            buffer.ResetPointerIndex();

            short[] rebuiltVals = new short[values.Length];
            for (int i = 0; i < values.Length; i++) {
                rebuiltVals[i] = buffer.ReadShort(bitCounts[i]);
            }

            CollectionAssert.AreEqual(values, rebuiltVals);
        }

        /// <summary>
        /// Test writing a uint to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteUIntTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            uint[] values = { uint.MinValue, uint.MaxValue, 100, 23000 };

            //Write the values
            for (int i = 0; i < values.Length; i++) {
                buffer.Write(values[i]);
            }

            buffer.ResetPointerIndex();

            uint[] rebuiltVals = new uint[values.Length];
            for (int i = 0; i < values.Length; i++) {
                rebuiltVals[i] = buffer.ReadUInt();
            }

            CollectionAssert.AreEqual(values, rebuiltVals);
        }

        /// <summary>
        /// Write several uints with custom bit count to
        /// the buffer and read them back.
        /// </summary>
        [TestMethod]
        public void BufferWriteFixedUIntTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            uint[] values = { uint.MinValue, uint.MaxValue, 100, 23000 };
            int[] bitCounts = { 32, 32, 7, 31 };

            //Write the values
            for (int i = 0; i < values.Length; i++) {
                buffer.Write(values[i], bitCounts[i]);
            }

            buffer.ResetPointerIndex();

            uint[] rebuiltVals = new uint[values.Length];
            for (int i = 0; i < values.Length; i++) {
                rebuiltVals[i] = buffer.ReadUInt(bitCounts[i]);
            }

            CollectionAssert.AreEqual(values, rebuiltVals);
        }

        /// <summary>
        /// Test writing int to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteIntTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            int[] values = { int.MinValue, int.MaxValue, 100, 23000 };

            //Write the values
            for (int i = 0; i < values.Length; i++) {
                buffer.Write(values[i]);
            }

            buffer.ResetPointerIndex();

            int[] rebuiltVals = new int[values.Length];
            for (int i = 0; i < values.Length; i++) {
                rebuiltVals[i] = buffer.ReadInt();
            }

            CollectionAssert.AreEqual(values, rebuiltVals);
        }

        /// <summary>
        /// Write several ints with custom bit count to
        /// the buffer and read them back.
        /// </summary>
        [TestMethod]
        public void BufferWriteFixedIntTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            int[] values = { int.MinValue, int.MaxValue, 50, 23000 };
            int[] bitCounts = { 32, 32, 7, 31 };

            //Write the values
            for (int i = 0; i < values.Length; i++) {
                buffer.Write(values[i], bitCounts[i]);
            }

            buffer.ResetPointerIndex();

            int[] rebuiltVals = new int[values.Length];
            for (int i = 0; i < values.Length; i++) {
                rebuiltVals[i] = buffer.ReadInt(bitCounts[i]);
            }

            CollectionAssert.AreEqual(values, rebuiltVals);
        }

        /// <summary>
        /// Test writes a long to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteLongTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            long[] values = { long.MinValue, long.MaxValue, 0, 200 };

            //Write the values
            for (int i = 0; i < values.Length; i++) {
                buffer.Write(values[i]);
            }

            buffer.ResetPointerIndex();

            long[] rebuiltVals = new long[values.Length];
            for (int i = 0; i < values.Length; i++) {
                rebuiltVals[i] = buffer.ReadLong();
            }

            CollectionAssert.AreEqual(values, rebuiltVals);
        }

        /// <summary>
        /// Write several longs with custom bit count to
        /// the buffer and read them back.
        /// </summary>
        [TestMethod]
        public void BufferWriteFixedLongTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            long[] values = { long.MinValue, long.MaxValue, 0, 200 };
            int[] bitCounts = { 64, 64, 3, 14 };

            //Write the values
            for (int i = 0; i < values.Length; i++) {
                buffer.Write(values[i], bitCounts[i]);
            }

            buffer.ResetPointerIndex();

            long[] rebuiltVals = new long[values.Length];
            for (int i = 0; i < values.Length; i++) {
                rebuiltVals[i] = buffer.ReadLong(bitCounts[i]);
            }

            CollectionAssert.AreEqual(values, rebuiltVals);
        }

        /// <summary>
        /// Test writes an ulong to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteULongTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            ulong[] values = { ulong.MinValue, ulong.MaxValue, 0, 200 };

            //Write the values
            for (int i = 0; i < values.Length; i++) {
                buffer.Write(values[i]);
            }

            buffer.ResetPointerIndex();

            ulong[] rebuiltVals = new ulong[values.Length];
            for (int i = 0; i < values.Length; i++) {
                rebuiltVals[i] = buffer.ReadULong();
            }

            CollectionAssert.AreEqual(values, rebuiltVals);
        }

        /// <summary>
        /// Write several ulongs with custom bit count to
        /// the buffer and read them back.
        /// </summary>
        [TestMethod]
        public void BufferWriteFixedULongTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            ulong[] values = { ulong.MinValue, ulong.MaxValue, 20000, 200 };
            int[] bitCounts = { 64, 64, 32, 14 };

            //Write the values
            for (int i = 0; i < values.Length; i++) {
                buffer.Write(values[i], bitCounts[i]);
            }

            buffer.ResetPointerIndex();

            ulong[] rebuiltVals = new ulong[values.Length];
            for (int i = 0; i < values.Length; i++) {
                rebuiltVals[i] = buffer.ReadULong(bitCounts[i]);
            }

            CollectionAssert.AreEqual(values, rebuiltVals);
        }

        /// <summary>
        /// Test writes a float to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteFloatTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            float value = 3.2000f;
            buffer.Write(value);

            float test = BitConverter.ToSingle(new byte[] { 205, 204, 76, 64 }, 0);

            buffer.ResetPointerIndex();
            float readVal = buffer.ReadFloat();

            Assert.AreEqual(value, readVal);
        }

        /// <summary>
        /// Test writes a double to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteDoubleTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            double value = 1.2983;
            buffer.Write(value);

            buffer.ResetPointerIndex();
            double readVal = buffer.ReadDouble();

            Assert.AreEqual(value, readVal);
        }

        /// <summary>
        /// Test writing a date time to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteDateTimeTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            DateTime value = DateTime.Now;
            buffer.Write(value);

            buffer.ResetPointerIndex();
            DateTime readVal = buffer.ReadDateTime();

            Assert.AreEqual(value, readVal);
        }

        /// <summary>
        /// Test writing several bools to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteIPEndPointTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            IPEndPoint value = new IPEndPoint(new IPAddress(200000), 200);
            buffer.Write(value);

            buffer.ResetPointerIndex();
            IPEndPoint readVal = buffer.ReadIPEndPoint();

            Assert.AreEqual(value, readVal);
        }

        /// <summary>
        /// Test writes a string to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteStringTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            string value = "The fat brown cat lorem ipsum";
            buffer.Write(value);

            buffer.ResetPointerIndex();
            string readVal = buffer.ReadString();

            Assert.AreEqual(value, readVal);
        }

        /// <summary>
        /// Test writes a ISerializable Object to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteISerializableTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            NetPlayerStats value = new NetPlayerStats();
            value.Kills = 100;
            value.Deaths = 200;
            value.BlocksDestroyed = 2000;
            value.BlocksPlaced = 199;
            buffer.Write(value);

            buffer.ResetPointerIndex();
            NetPlayerStats readVal = buffer.ReadSerializableObject() as NetPlayerStats;

            Assert.AreEqual(value, readVal);
        }

        /// <summary>
        /// Test writes a Vector2 to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteVector2Test() {
            ByteBuffer buffer = new ByteBuffer(16);
            Vector2 value = new Vector2(5.02f, 1.2f);
            buffer.Write(value);

            buffer.ResetPointerIndex();
            Vector2 readVal = buffer.ReadVector2();

            Assert.AreEqual(value, readVal);
        }

        /// <summary>
        /// Test writes a Vector3  to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteVector3Test() {
            ByteBuffer buffer = new ByteBuffer(16);
            Vector3 value = new Vector3(5.02f, 1.2f, 4.0f);
            buffer.Write(value);

            buffer.ResetPointerIndex();
            Vector3 readVal = buffer.ReadVector3();

            Assert.AreEqual(value, readVal);
        }

        /// <summary>
        /// Test writes a Vector2Int to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteVector2IntTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            Vector2Int value = new Vector2Int(1, 2);
            buffer.Write(value);

            buffer.ResetPointerIndex();
            Vector2Int readVal = buffer.ReadVector2Int();

            Assert.AreEqual(value, readVal);
        }

        /// <summary>
        /// Test writes a Vector3Int to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteVector3IntTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            Vector3Int value = new Vector3Int(2, 3, 4);
            buffer.Write(value);

            buffer.ResetPointerIndex();
            Vector3Int readVal = buffer.ReadVector3Int();

            Assert.AreEqual(value, readVal);
        }

        /// <summary>
        /// Test writes a Vect2Int to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteVect2IntTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            Vect2Int value = new Vect2Int(45, 20);
            buffer.Write(value);

            buffer.ResetPointerIndex();
            Vect2Int readVal = buffer.ReadVect2Int();

            Assert.AreEqual(value, readVal);
        }

        /// <summary>
        /// Test writes a Vect3Int to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteVect3IntTest() {
            ByteBuffer buffer = new ByteBuffer(16);
            Vect3Int value = new Vect3Int(200, 300, 512);
            buffer.Write(value);

            buffer.ResetPointerIndex();
            Vect3Int readVal = buffer.ReadVect3Int();

            Assert.AreEqual(value, readVal);
        }

        /// <summary>
        /// Test writes several bytes to the buffer and
        /// then retrieve the values.
        /// </summary>
        [TestMethod]
        public void BufferWriteBytesTest() {
            ByteBuffer buffer = new ByteBuffer(32);
            byte[] value = new byte[] { 0, 255, 120, 170 };
            buffer.Write(value);

            buffer.ResetPointerIndex();
            byte[] readVal = buffer.ReadBytes(32);

            CollectionAssert.AreEqual(value, readVal);
        }

        /// <summary>
        /// Test writes a Color16 to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteColor16Test() {
            ByteBuffer buffer = new ByteBuffer(16);
            Color16 value = Color16.Clouds;
            buffer.Write(value);

            buffer.ResetPointerIndex();
            Color16 readVal = buffer.ReadColor16();

            Assert.AreEqual(value, readVal);
        }

        /// <summary>
        /// Test writes a Color32s to the buffer and
        /// then retrieve the value.
        /// </summary>
        [TestMethod]
        public void BufferWriteColor32Test() {
            ByteBuffer buffer = new ByteBuffer(16);
            Color32 value = new Color32(120, 120, 240, 255);
            buffer.Write(value);

            buffer.ResetPointerIndex();
            Color32 readVal = buffer.ReadColor32();

            Assert.AreEqual(value, readVal);
        }

        /// <summary>
        /// Test wrutes a byte with less than 8 bits.
        /// </summary>
        [TestMethod]
        public void BufferWriteFixedByteText() {
            ByteBuffer buffer = new ByteBuffer(16);
            byte val = 15;
            buffer.Write(val, 4);
            buffer.Write(12, 4);

            buffer.ResetPointerIndex();
            byte rebuiltVal = buffer.ReadByte(3);
            byte two = buffer.ReadByte(3);

            Assert.AreEqual(7, rebuiltVal);
            Assert.AreEqual(1, two);
        }
    }
}
