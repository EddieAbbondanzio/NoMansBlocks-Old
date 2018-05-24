using Microsoft.VisualStudio.TestTools.UnitTesting;
using Voxelated.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Net;

namespace Voxelated.Tests.Utilities {
    /// <summary>
    /// Test methods for the SerializeUtils class. Checks converting values to
    /// bytes and back
    /// </summary>
    [TestClass()]
    public class SerializeUtilsTests {
        /// <summary>
        /// Test serializing byte values to byte arrays
        /// and back.
        /// </summary>
        [TestMethod()]
        public void SerializeByteTest() {
            byte[] testValues = new byte[] { 0, 16, byte.MaxValue };

            foreach(byte testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal);
                byte rebuildVal = SerializeUtils.GetByte(bytes, 0);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Test serializing short values to byte arrays
        /// and back.
        /// </summary>
        [TestMethod()]
        public void SerializeShortTest() {
            short[] testValues = new short[] { 0, 1000, short.MaxValue, short.MinValue };

            foreach (short testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal);
                short rebuildVal = SerializeUtils.GetShort(bytes, 0);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Serialize a custom sized short to bytes and back.
        /// </summary>
        [TestMethod]
        public void SerializeFixedShortTest() {
            short[] testValues = { 767, -250 };

            foreach (short testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal, 11);
                short rebuildVal = SerializeUtils.GetShort(bytes, 0, 11);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Test serializing ushort values to byte arrays
        /// and back.
        /// </summary>
        [TestMethod()]
        public void SerializeUShortTest() {
            ushort[] testValues = new ushort[] { 0, 1000, ushort.MaxValue, ushort.MinValue };

            foreach (ushort testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal);
                ushort rebuildVal = SerializeUtils.GetUShort(bytes, 0);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Test serializing int values to byte arrays
        /// and back.
        /// </summary>
        [TestMethod()]
        public void SerializeIntTest() {
            int[] testValues = new int[] { 0, 1000, int.MaxValue, int.MinValue };

            foreach (int testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal);
                int rebuildVal = SerializeUtils.GetInt(bytes, 0);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Serialize a custom sized int to bytes and back.
        /// </summary>
        [TestMethod]
        public void SerializeFixedIntTest() {
            int[] testValues = { 767, -250 };

            foreach (short testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal, 11);
                int rebuildVal = SerializeUtils.GetInt(bytes, 0, 11);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Test serializing uint values to byte arrays
        /// and back.
        /// </summary>
        [TestMethod()]
        public void SerializeUIntTest() {
            uint[] testValues = new uint[] { 0, 1000, uint.MaxValue, uint.MinValue };

            foreach (uint testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal);
                uint rebuildVal = SerializeUtils.GetUInt(bytes, 0);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Test serializing long values to byte arrays
        /// and back.
        /// </summary>
        [TestMethod()]
        public void SerializeLongTest() {
            long[] testValues = new long[] { 0, 1000, long.MaxValue, long.MinValue };

            foreach (long testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal);
                long rebuildVal = SerializeUtils.GetLong(bytes, 0);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Serialize a custom sized long to bytes and back.
        /// </summary>
        [TestMethod]
        public void SerializeFixedLongTest() {
            long[] testValues = { 767, -250 };

            foreach (short testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal, 11);
                long rebuildVal = SerializeUtils.GetLong(bytes, 0, 11);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Test serializing ulong values to byte arrays
        /// and back.
        /// </summary>
        [TestMethod()]
        public void SerializeULongTest() {
            ulong[] testValues = new ulong[] { 0, 1000, ulong.MaxValue, ulong.MinValue };

            foreach (ulong testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal);
                ulong rebuildVal = SerializeUtils.GetULong(bytes, 0);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Test serializing bool values to byte arrays
        /// and back.
        /// </summary>
        [TestMethod()]
        public void SerializeBoolTest() {
            bool[] testValues = new bool[] { true, false };

            foreach (bool testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal);
                bool rebuildVal = SerializeUtils.GetBool(bytes, 0);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Test serializing char values to byte arrays
        /// and back.
        /// </summary>
        [TestMethod()]
        public void SerializeCharTest() {
            char[] testValues = new char[] { 'a', 'Z','1', 'P' };

            foreach (char testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal);
                char rebuildVal = SerializeUtils.GetChar(bytes, 0);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Test serializing float values to byte arrays
        /// and back.
        /// </summary>
        [TestMethod()]
        public void SerializeFloatTest() {
            float[] testValues = new float[] { 0, 1000.2f, float.MaxValue, float.MinValue };

            foreach (float testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal);
                float rebuildVal = SerializeUtils.GetFloat(bytes, 0);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Test serializing double values to byte arrays
        /// and back.
        /// </summary>
        [TestMethod()]
        public void SerializeDoubleTest() {
            double[] testValues = new double[] { 0, 1000, double.MaxValue, double.MinValue };

            foreach (double testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal);
                double rebuildVal = SerializeUtils.GetDouble(bytes, 0);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Test serializing string values to byte arrays
        /// and back.
        /// </summary>
        [TestMethod()]
        public void SerializeStringTest() {
            string[] testValues = new string[] { "hello", "world", "", "    ", "POO" };

            foreach (string testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal);
                string rebuildVal = SerializeUtils.GetString(bytes, 0);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Test serializing date time values to byte arrays
        /// and back.
        /// </summary>
        [TestMethod()]
        public void SerializeDateTimeTest() {
            DateTime[] testValues = new DateTime[] { DateTime.Now, DateTime.MinValue, DateTime.MaxValue };

            foreach (DateTime testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal);
                DateTime rebuildVal = SerializeUtils.GetDateTime(bytes, 0);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Test serializing ip values to byte arrays
        /// and back.
        /// </summary>
        [TestMethod()]
        public void SerializeIPEndPointTest() {
            IPEndPoint[] testValues = new IPEndPoint[] { new IPEndPoint(10000, 200), new IPEndPoint(new IPAddress(3000), 499) };

            foreach (IPEndPoint testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal);
                IPEndPoint rebuildVal = SerializeUtils.GetIPEndPoint(bytes, 0);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Test serializing vector 2 values to byte arrays
        /// and back.
        /// </summary>
        [TestMethod()]
        public void SerializeVector2Test() {
            Vector2[] testValues = new Vector2[] { Vector2.up, Vector2.zero, new Vector2(20, 40.29f) };

            foreach (Vector2 testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal);
                Vector2 rebuildVal = SerializeUtils.GetVector2(bytes, 0);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Test serializing vector 2 int values to byte arrays
        /// and back.
        /// </summary>
        [TestMethod()]
        public void SerializeVector2IntTest() {
            Vector2Int[] testValues = new Vector2Int[] { Vector2Int.up, Vector2Int.zero, new Vector2Int(20, 40) };

            foreach (Vector2Int testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal);
                Vector2Int rebuildVal = SerializeUtils.GetVector2Int(bytes, 0);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Test serializing vector 3 values to byte arrays
        /// and back.
        /// </summary>
        [TestMethod()]
        public void SerializeVector3Test() {
            Vector3[] testValues = new Vector3[] { Vector3.up, Vector3.zero, new Vector3(20, 40.29f, 10.9f) };

            foreach (Vector3 testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal);
                Vector3 rebuildVal = SerializeUtils.GetVector3(bytes, 0);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Test serializing vector 3 int values to byte arrays
        /// and back.
        /// </summary>
        [TestMethod()]
        public void SerializeVector3IntTest() {
            Vector3Int[] testValues = new Vector3Int[] { Vector3Int.up, Vector3Int.zero, new Vector3Int(20, 19, -2) };

            foreach (Vector3Int testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal);
                Vector3Int rebuildVal = SerializeUtils.GetVector3Int(bytes, 0);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }

        /// <summary>
        /// Test serializing color32 values to byte arrays
        /// and back.
        /// </summary>
        [TestMethod()]
        public void SerializeColor32Test() {
            Color32[] testValues = new Color32[] { Color.black, Color.cyan, new Color32(0,0,0,0), new Color32(10, 20, 30, 40) };

            foreach (Color32 testVal in testValues) {
                byte[] bytes = SerializeUtils.Serialize(testVal);
                Color32 rebuildVal = SerializeUtils.GetColor32(bytes, 0);

                Assert.AreEqual(testVal, rebuildVal);
            }
        }
    }
}