using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Voxelated.Utilities;

namespace Voxelated.Test.Utils {
    /// <summary>
    /// All unit tests related to the StringUtils class.
    /// </summary>
    [TestClass]
    public class StringUtilsTests {
        /// <summary>
        /// Test of shortening up a string using the
        /// StringUtils.Clamp() method.
        /// </summary>
        [TestMethod]
        public void StringUtilsClampLongerTest() {
            string str = "FARTS";
            string clamped = StringUtils.Clamp(str, 2);

            Assert.AreEqual("FA", clamped);
        }

        /// <summary>
        /// Test of a string that is the same length 
        /// as the max in StringUtils.Clamp().
        /// </summary>
        [TestMethod]
        public void StringUtilsClampSameLengthTest() {
            string str = "FARTS";
            string clamped = StringUtils.Clamp(str, 5);

            Assert.AreEqual(str, clamped);
        }

        /// <summary>
        /// Test of StringUtils.Clamp() for a string that 
        /// </summary>
        [TestMethod]
        public void StringUtilsClampShorterTest() {
            string str = "FARTS";
            string clamped = StringUtils.Clamp(str, 16);

            Assert.AreEqual(str, clamped);
        }
    }
}
