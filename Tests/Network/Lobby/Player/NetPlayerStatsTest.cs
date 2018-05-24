using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Voxelated.Network.Lobby;

namespace Voxelated.Test.Network {
    /// <summary>
    /// Unit tests related to the NetPlayerStats class.
    /// </summary>
    [TestClass]
    public class NetPlayerStatsTest {
        /// <summary>
        /// Test creating a new stat class and checking
        /// everything for an equivalence of 0.
        /// </summary>
        [TestMethod]
        public void CreateNewStatsTest() {
            NetPlayerStats playerStats = new NetPlayerStats();

            int[] expected = new int[5];

            int[] values = new int[5];
            values[0] = playerStats.Kills;
            values[1] = playerStats.Deaths;
            values[2] = (int)playerStats.BlocksPlaced;
            values[3] = (int)playerStats.BlocksDestroyed;
            values[4] = playerStats.Score;

            CollectionAssert.AreEqual(expected, values);
        }

        /// <summary>
        /// Test serializinga player stats to bytes
        /// ang back.
        /// </summary>
        [TestMethod]
        public void SerializeStatsTest() {
            NetPlayerStats playerStats = new NetPlayerStats() {
                Kills = 100,
                Deaths = 101,
                BlocksPlaced = 1024,
                BlocksDestroyed = 200,
                Score = -1
            };

            byte[] bytes = playerStats.Serialize();

            NetPlayerStats rebuiltStats = new NetPlayerStats(bytes, 0);
            Assert.AreEqual(playerStats, rebuiltStats);
        }
    }
}
