using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Voxelated.Network.Messages;
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
    }
}
