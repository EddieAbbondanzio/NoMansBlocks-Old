using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Voxelated.Utilities;
using Voxelated.Network.Messages;
using Voxelated.Network.Lobby;

namespace Voxelated.Network {
    /// <summary>
    /// Base class for server and client network managers. Requires
    /// a set of methods to be implemented in them to allow for abstracting
    /// away from the NetClient and NetServer of lidgren.
    /// </summary>
    public abstract class NetManager {
        #region Events
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

        #region Properties
        /// <summary>
        /// If the manager is running as a server,
        /// or a client. Just controls handling extra stuff
        /// is all.
        /// </summary>
        public abstract bool IsServer { get; }

        /// <summary>
        /// The lobby that the manager is connected to.
        /// </summary>
        public NetLobby Lobby { get; protected set; }

        /// <summary>
        /// The permissions that this network manager
        /// has in the lobby.
        /// </summary>
        public NetPermissions Permissions { get; protected set; }
        #endregion

        #region Members
        /// <summary>
        /// The lidgren net 
        /// </summary>
        protected NetPeer netPeer;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new NetManager with the specified level of 
        /// permissions.
        /// </summary>
        /// <param name="permissions">The permissions level the manager has.</param>
        public NetManager(NetPermissions permissions) {
            Permissions = permissions;
            Lobby = new NetLobby(this);
        }
        #endregion

        #region Publics
        /// <summary>
        /// Check to see if any messages have come in from the network.
        /// </summary>
        public void CheckForMessages() {
            if (netPeer != null) {
                NetIncomingMessage inMsg;

                while ((inMsg = netPeer.ReadMessage()) != null) {
                    NetMessage netMsg = NetMessage.DecodeMessage(inMsg);
             
                    switch (netMsg?.Category) {
                        case NetMessageCategory.Connection:
                            if (OnConnectionMessage != null) {
                                OnConnectionMessage(null, new NetMessageArgs(netMsg));
                            }
                            break;

                        case NetMessageCategory.Info:
                            if (OnInfoMessage != null) {
                                OnInfoMessage(null, new NetMessageArgs(netMsg));
                            }
                            break;

                        case NetMessageCategory.Lobby:
                            if (OnLobbyMessage != null) {
                                OnLobbyMessage(null, new NetMessageArgs(netMsg));
                            }
                            break;

                        case NetMessageCategory.Chat:
                            if(OnChatMessage != null) {
                                OnChatMessage(null, new NetMessageArgs(netMsg));
                            }
                            break;
                    }

                    netPeer.Recycle(inMsg);
                }
            }
        }

        /// <summary>
        /// Flush the outgoing queue, and send out all waiting messages.
        /// </summary>
        public void SendOutMessages() {
            if(netPeer != null) {
                netPeer.FlushSendQueue();
            }
        }

        /// <summary>
        /// Retrieve the server time. If no serer is found,
        /// 0 is returned.
        /// </summary>
        /// <returns>The current NetTime of the server.</returns>
        public double GetServerTime() {
            //Server time is always current net time.
            if (IsServer) {
                return NetTime.Now;
            }
            else if((netPeer?.ConnectionsCount ?? 0) > 0){
                return netPeer.Connections[0].GetRemoteTime(NetTime.Now);
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// Convert a local time into a server time.
        /// </summary>
        /// <param name="localTime">The local time to convert.</param>
        /// <returns>The server time variant.</returns>
        public double GetServerTime(double localTime) {
            //When on the server, this already is server time.
            if (IsServer) {
                return localTime;
            }
            else if ((netPeer?.ConnectionsCount ?? 0) > 0) {
                return netPeer.Connections[0].GetRemoteTime(localTime);
            }
            else {
                return 0;
            }
        }
        #endregion

        #region Message Senders
        /// <summary>
        /// Send a message over the network. It encodes the message into
        /// a NetOutGoingMessage through lidgren.
        /// </summary>
        /// <param name="message">The message to send out.</param>
        /// <param name="method">What method to send it over the wire.</param>
        /// <param name="channel">The channel to send it on.</param>
        public abstract void SendMessage(NetMessage message, NetDeliveryMethod method, NetChannel channel);

        /// <summary>
        /// Send a message to a single client in the lobby.
        /// </summary>
        /// <param name="message">The message to send out.</param>
        /// <param name="target">The client to send it to.</param>
        /// <param name="method">What method to send it over the wire.</param>
        /// <param name="channel">The channel to send it on.</param>
        public void SendMessage(NetMessage message, NetConnection target, NetDeliveryMethod method, NetChannel channel) {
            NetOutgoingMessage outMsg = netPeer.CreateMessage();
            outMsg.Write(message.Serialize());

            if (outMsg != null && netPeer != null) {
                netPeer.SendMessage(outMsg, target, method, (int)channel);
            }
        }
        #endregion
    }
}
