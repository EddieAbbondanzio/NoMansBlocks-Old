using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Voxelated.Network.Lobby;
using Voxelated.Utilities;
using System.Net;

namespace Voxelated.Test.Serialization {
    /// <summary>
    /// Tests for various advanced objects to ensure they are properly
    /// serializing to bytes and back.
    /// </summary>
    [TestClass]
    public class ObjectSerializeTest {

        /// <summary>
        /// Tests is a Vect2Int is serialized and back properly.
        /// </summary>
        [TestMethod]
        public void SerializeVect2IntTest() {
            Vect2Int pos = new Vect2Int(10, 20);

            byte[] bytes = SerializeUtils.Serialize(pos);
            Vect2Int rebuiltPos = SerializeUtils.GetVect2Int(bytes, 0);

            Assert.AreEqual(pos, rebuiltPos);
        }

        [TestMethod]
        public void SerializeFixedVect2IntTest() {
            Vect2Int pos = new Vect2Int(10, 5);
            byte[] bytes = SerializeUtils.Serialize(pos, 10);
            Vect2Int rebuiltPos = SerializeUtils.GetVect2Int(bytes, 0, 10);

            Assert.AreEqual(pos, rebuiltPos);
        }

        /// <summary>
        /// Tests if a Vect3Int is serialized and back properly.
        /// </summary>
        [TestMethod]
        public void SerializeVect3IntTest() {
            Vect3Int pos = new Vect3Int(-10, 20, 30);

            byte[] bytes = SerializeUtils.Serialize(pos);
            Vect3Int rebuiltPos = SerializeUtils.GetVect3Int(bytes, 0);

            Assert.AreEqual(pos, rebuiltPos);
        }

        /// <summary>
        /// Tests if a Vect3Int is serialized and back properly.
        /// </summary>
        [TestMethod]
        public void SerializeFixedVect3IntTest() {
            Vect3Int pos = new Vect3Int(-10, 20, 30);

            byte[] bytes = SerializeUtils.Serialize(pos, 21);
            Vect3Int rebuiltPos = SerializeUtils.GetVect3Int(bytes, 0, 21);

            Assert.AreEqual(pos, rebuiltPos);
        }

        /// <summary>
        /// Test converting a Color16 into bytes and back.
        /// </summary>
        [TestMethod]
        public void SerializeColorTest() {
            Color16 color = Color16.Amethyst;

            byte[] bytes = SerializeUtils.Serialize(color);
            Color16 rebuiltColor = SerializeUtils.GetColor16(bytes, 0);

            Assert.AreEqual(color, rebuiltColor);
        }
    
    }
}
