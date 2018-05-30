using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Voxelated.Network.Messages;
using Voxelated.Network;

namespace Voxelated.Test.Network {
    /// <summary>
    /// Tests related to NetMessages.
    /// </summary>
    [TestClass]
    public class NetMessageTests {
        [TestMethod]
        public void NetMessageSerializeTest() {
            LobbyChatMessage lobbyMsg = new LobbyChatMessage("Bert", "Hello World!");
            byte[] bytes = lobbyMsg.Serialize();
        }

        [TestMethod]
        public void TimeSyncSerializeTest() {
            TimeSyncRequestMessage syncReq = new TimeSyncRequestMessage();
            byte[] bytes = syncReq.Serialize();

            NetMessageType type = (NetMessageType)(bytes[0] & 127);
            Assert.AreEqual(syncReq.Type, type);
        }
    }
}
