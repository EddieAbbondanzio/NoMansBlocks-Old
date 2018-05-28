using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network.Messages;
using Voxelated.Utilities;

namespace Voxelated.Network.Client {
    /// <summary>
    /// Maintains the connection with the server to
    /// the client.
    /// </summary>
    public class NetClientConnectionHandler {
        #region Members
        /// <summary>
        /// Reference back to the network manager
        /// that owns this.
        /// </summary>
        private NetClientManager clientManager;

        private LiteNetLib.NetManager netManager;
        #endregion

        #region Constructor(s)
        public NetClientConnectionHandler(NetClientManager clientManager, LiteNetLib.NetManager netManager) {
            this.clientManager = clientManager;
            this.netManager = netManager;

            NetMessageListener.OnConnectionMessage += OnConnectionMessage;
        }

        /// <summary>
        /// Prevent memory leaks.
        /// </summary>
        ~NetClientConnectionHandler() {
            NetMessageListener.OnConnectionMessage -= OnConnectionMessage;
        }
        #endregion

        #region Message Handlers
        /// <summary>
        /// Event handler for anytime a connection category
        /// message is recieved by the client.
        /// </summary>
        /// <param name="sender">THe NetMessageListener</param>
        /// <param name="e">The message recieved.</param>
        private void OnConnectionMessage(object sender, NetMessageArgs e) {
            switch (e.Message?.Type) {
                //Our connection was accepted by the server.
                case NetMessageType.ConnectionAccepted:
                    ConnectionAcceptedMessage acceptedMsg = e.Message as ConnectionAcceptedMessage;

                    if(acceptedMsg != null) {
                        LoggerUtils.Log("NetClientConnectionHandler: Server Accepted Connection", LogLevel.Debug);
                        LoggerUtils.Log("NetClientConnectionHandler: Server Name: " + acceptedMsg.ServerName);

                        //Send the server the clients name
                        ClientGreetingMessage greetMsg = new ClientGreetingMessage(clientManager.Settings.Name);
                        clientManager.SendMessage(greetMsg, SendOptions.ReliableOrdered);
                    }
                    break;

                //Server rejected the client.
                case NetMessageType.Disconnected:
                    DisconnectedMessage disconnectedMsg = e.Message as DisconnectedMessage;

                    if(disconnectedMsg != null) {
                        LoggerUtils.Log("NetClientConnectionHandler: Server Disconnected", LogLevel.Debug);
                        LoggerUtils.Log("NetClientConnectionHandler: Reason: " + disconnectedMsg.Reason, LogLevel.Debug);
                    }
                    break;
            }
        }
        #endregion

        #region Publics
        /// <summary>
        /// Send the connection request message to the 
        /// server inputted.
        /// </summary>
        /// <param name="serverAddress">The IP address of the server.</param>
        public void SendConnectionRequest(NetEndPoint serverAddress) {
            if (serverAddress != null && clientManager.ConnectionCount == 0) {
                netManager.Connect(serverAddress);
            }
        }
        #endregion
    }
}
