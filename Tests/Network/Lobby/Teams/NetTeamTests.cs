using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Voxelated.Network.Lobby;

namespace Voxelated.Test.Network {
    /// <summary>
    /// Unit tests pertaining to NetTeams.
    /// </summary>
    [TestClass]
    public class NetTeamTests {
        /// <summary>
        /// Add a player to a team and check to see 
        /// if they are found.
        /// </summary>
        [TestMethod]
        public void AddMemberTest() {
            NetPlayer player = new NetPlayer(1, "Bert", NetTeamColor.Spectator);
            NetTeam team = new NetTeam(NetTeamColor.Red);
            team.AddMember(player);

            Assert.AreEqual(NetTeamColor.Red, player.Team);
        }

        /// <summary>
        /// Serialize the team into a byte array and back.
        /// </summary>
        [TestMethod]
        public void SerializeTeamTest() {
            NetPlayer playerA = new NetPlayer(100, "Frank");
            NetPlayer playerB = new NetPlayer(200, "Beans");

            NetTeam team = new NetTeam(NetTeamColor.Blue);
            team.AddMember(playerA);
            team.AddMember(playerB);
            team.Score = 1337;

            byte[] bytes = team.Serialize();
            NetTeam rebuiltTeam = new NetTeam(bytes, 0);

            Assert.AreEqual(team, rebuiltTeam);
        }

    }
}
