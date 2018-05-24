using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Voxelated.Network.Lobby;
using Voxelated.Serialization;

namespace Voxelated.Test.Network {
    /// <summary>
    /// Unit tests for the NetPlayer class.
    /// </summary>
    [TestClass]
    public class NetPlayerTests {
        /// <summary>
        /// Tests creating a new player and verifying all members are
        /// properly set up.
        /// </summary>
        [TestMethod]
        public void CreateNewPlayerTest() {
            byte id = 100;
            string name = "Bert";
            NetTeamColor color = NetTeamColor.Red;

            NetPlayer player = new NetPlayer(id, name, color);

            Assert.AreEqual(id, player.Id);
            Assert.AreEqual(name, player.NickName);
            Assert.AreEqual(color, player.Team);
        }

        /// <summary>
        /// Serialize a net player into it's
        /// byte array and back.
        /// </summary>
        [TestMethod]
        public void SerializePlayerTest() {
            byte id = 100;
            string name = "Bert";
            NetTeamColor color = NetTeamColor.Red;

            NetPlayer player = new NetPlayer(id, name, color);

            byte[] bytes = player.Serialize();

            NetPlayer rebuiltPlayer = new NetPlayer(bytes, 0);
            Assert.AreEqual(player, rebuiltPlayer);
        }

        [TestMethod]
        public void BufferSerializePlayerTest() {
            byte id = 100;
            string name = "Bert";
            NetTeamColor color = NetTeamColor.Red;
            NetPlayer player = new NetPlayer(id, name, color);

            ByteBuffer buffer = new ByteBuffer();
            buffer.Write(player);

            buffer.ResetPointerIndex();
            NetPlayer rebuiltPlayer = buffer.ReadSerializableObject() as NetPlayer;

            Assert.AreEqual(player, rebuiltPlayer);
        }
    }
}
