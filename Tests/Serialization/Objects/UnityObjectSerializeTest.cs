using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityEngine;
using Voxelated.Utilities;

namespace Voxelated.Test.Serialization {
    /// <summary>
    /// Unit tests for serializng unity based objects.
    /// </summary>
    [TestClass]
    public class UnityObjectSerializeTest {
        /// <summary>
        /// Test if a Vector2 is converted to and back from
        /// bytes properly.
        /// </summary>
        [TestMethod]
        public void SerializeVector2Test() {
            Vector2 value = new Vector2(10f, 20f);

            byte[] b = SerializeUtils.Serialize(value);
            Vector2 rebuiltVal = SerializeUtils.GetVector2(b, 0);

            Assert.AreEqual(value, rebuiltVal);
        }

        /// <summary>
        /// Tests if a Vector3 is converted to bytes
        /// and back properly.
        /// </summary>
        [TestMethod]
        public void SerializeVector3Test() {
            Vector3 value = new Vector3(10.2f, 24.1f, 36.3f);

            byte[] b = SerializeUtils.Serialize(value);
            Vector3 rebuiltVal = SerializeUtils.GetVector3(b, 0);

            Assert.AreEqual(value, rebuiltVal);
        }

        /// <summary>
        /// Test if a Vector2 is converted to and back from
        /// bytes properly.
        /// </summary>
        [TestMethod]
        public void SerializeVector2IntTest() {
            Vector2Int value = new Vector2Int(10, 20);

            byte[] b = SerializeUtils.Serialize(value);
            Vector2Int rebuiltVal = SerializeUtils.GetVector2Int(b, 0);

            Assert.AreEqual(value, rebuiltVal);
        }

        /// <summary>
        /// Tests if a Vector3 is converted to bytes
        /// and back properly.
        /// </summary>
        [TestMethod]
        public void SerializeVector3IntTest() {
            Vector3Int value = new Vector3Int(50, 25, 36);

            byte[] b = SerializeUtils.Serialize(value);
            Vector3Int rebuiltVal = SerializeUtils.GetVector3Int(b, 0);

            Assert.AreEqual(value, rebuiltVal);
        }
    }
}
