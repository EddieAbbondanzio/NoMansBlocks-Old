using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib;
using LiteNetLib.Utils;
using Voxelated.Utilities;
using Voxelated.Network.Messages;

namespace Voxelated.Network {
    /// <summary>
    /// Event listener for the NetManager. Handles firing off
    /// events for messagess that were recieved over the network.
    /// </summary>
    public class NetMessageListener : INetEventListener {
        #region Debugging
        /// <summary>
        /// If extra logs should be printed for debugging use.
        /// </summary>
        public bool debugLog = true;
        #endregion

        #region Message Events
        /// <summary>
        /// Server Only. Covers message such as new connection, connection leaving
        /// or a client wants to conect.
        /// </summary>
        public static event EventHandler<NetMessageArgs> OnConnectionMessage;

        /// <summary>
        /// When a debug or log message is recieved 
        /// from over the network.
        /// </summary>
        public static event EventHandler<NetMessageArgs> OnInfoMessage;

        /// <summary>
        /// When a lobby message such as player joined or left
        /// is recieved from over the network.
        /// </summary>
        public static event EventHandler<NetMessageArgs> OnLobbyMessage;

        /// <summary>
        /// WHen a chat message is recieved from the network.
        /// </summary>
        public static event EventHandler<NetMessageArgs> OnChatMessage;
        #endregion

        #region Message Handlers
        /// <summary>
        /// Network error (on send or receive)
        /// </summary>
        /// <param name="endPoint">From endPoint (can be null)</param>
        /// <param name="socketErrorCode">Socket error code</param>
        public void OnNetworkError(NetEndPoint endPoint, int socketErrorCode) {
            LoggerUtils.ConditionalLog("NetMessageListener: Recieved Network Error: " + socketErrorCode, LogLevel.Debug, debugLog);
            InfoMessage infoMsg = new InfoMessage(endPoint, socketErrorCode);

            if (infoMsg != null && OnInfoMessage != null) {
                OnInfoMessage(this, new NetMessageArgs(infoMsg));
            }
        }

        /// <summary>
        /// New remote peer connected to server
        /// </summary>
        /// <param name="peer">Connected peer object</param>
        public void OnPeerConnected(NetPeer peer) {
            //Although LiteNetLib claims them as connected, this is only
            //the first step in the connection process.
            if (VoxelatedEngine.Engine.NetManager.IsServer) {
                ConnectionRequestMessage connReqMsg = new ConnectionRequestMessage(peer);

                //Debug
                LoggerUtils.ConditionalLog("NetMessageListener: New peer connected from: " + peer.EndPoint, LogLevel.Debug, debugLog);

                if(connReqMsg != null && OnConnectionMessage != null) {
                    OnConnectionMessage(this, new NetMessageArgs(connReqMsg));
                }
            }
        }

        /// <summary>
        /// Peer disconnected
        /// </summary>
        /// <param name="peer">disconnected peer</param>
        /// <param name="disconnectInfo">additional info about reason, errorCode or data received with disconnect message</param>
        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
            LoggerUtils.ConditionalLog("NetMessageListener: Peer with ip: " + peer.EndPoint + " disconnected.", LogLevel.Debug, debugLog);
            DisconnectedMessage disconnectedMsg = new DisconnectedMessage(peer, disconnectInfo);

            if (disconnectedMsg != null && OnConnectionMessage != null) {
                OnConnectionMessage(this, new NetMessageArgs(disconnectedMsg));
            }
        }

        /// <summary>
        /// Latency information updated
        /// </summary>
        /// <param name="peer">Peer with updated latency</param>
        /// <param name="latency">latency value in milliseconds</param>
        public void OnNetworkLatencyUpdate(NetPeer peer, int latency) {
            //LoggerUtils.ConditionalLog("NetMessageListener Latency Update for: " + peer.EndPoint + " current latency: " + latency, LogLevel.Debug, debugLog);
        }

        /// <summary>
        /// Received some data
        /// </summary>
        /// <param name="peer">From peer</param>
        /// <param name="reader">DataReader containing all received data</param>
        public void OnNetworkReceive(NetPeer peer, NetDataReader reader) {
            LoggerUtils.ConditionalLog("NetMessageListener: Recived message from: " + peer.EndPoint + " of length: " + reader.AvailableBytes, LogLevel.Debug, debugLog);

            NetMessage netMsg = null;

            //Pull in header info.
            NetMessageType msgType = (NetMessageType) reader.PeekByte();
            LoggerUtils.ConditionalLog("NetMessageListener: Recieved Message Type: " + msgType, LogLevel.Debug, debugLog);

            switch (msgType) {
                case NetMessageType.ConnectionRequest:
                    netMsg = new ConnectionRequestMessage(peer);
                    break;

                case NetMessageType.ConnectionAccepted:
                    netMsg = new ConnectionAcceptedMessage(peer, reader);
                    break;

                case NetMessageType.ClientGreeting:
                    netMsg = new ClientGreetingMessage(peer, reader);
                    break;

                case NetMessageType.LobbySync:
                    netMsg = new LobbySyncMessage(peer, reader);
                    break;

                case NetMessageType.LobbyChat:
                    netMsg = new LobbyChatMessage(peer, reader);
                    break;

                case NetMessageType.Command:
                    netMsg = new CommandMessage(peer, reader);
                    break;

                case NetMessageType.PlayerJoined:
                    netMsg = new PlayerJoinedMessage(peer, reader);
                    break;

                case NetMessageType.PlayerLeft:
                    netMsg = new PlayerLeftMessage(peer, reader);
                    break;
            }


            //Fire off appropriate event here
            switch (netMsg?.Category) {
                case NetMessageCategory.Chat:
                    if(OnChatMessage != null) {
                        OnChatMessage(this, new NetMessageArgs(netMsg));
                    }

                    break;

                case NetMessageCategory.Lobby:
                    if(OnLobbyMessage != null) {
                        OnLobbyMessage(this, new NetMessageArgs(netMsg));
                    }
                    break;
            }
        }

        /// <summary>
        /// Received unconnected message
        /// </summary>
        /// <param name="remoteEndPoint">From address (IP and Port)</param>
        /// <param name="reader">Message data</param>
        /// <param name="messageType">Message type (simple, discovery request or responce)</param>
        public void OnNetworkReceiveUnconnected(NetEndPoint remoteEndPoint, NetDataReader reader, UnconnectedMessageType messageType) {
            //Do nothing
        }
        #endregion
    }
}
