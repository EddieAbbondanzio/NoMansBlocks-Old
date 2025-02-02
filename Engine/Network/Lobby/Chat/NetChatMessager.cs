﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network.Messages;
using Voxelated.Utilities;
using LiteNetLib;
using Voxelated.Network.Server;

namespace Voxelated.Network.Lobby {
    public class NetChatMessager {
        #region Properties
        /// <summary>
        /// This is the local players name.
        /// </summary>
        public string ChatName { get; private set; }

        /// <summary>
        /// The team to send messages to.
        /// </summary>
        public NetTeamColor Team { get; private set; }
        #endregion

        #region Members
        /// <summary>
        /// Only exists on server instances.
        /// </summary>
        private NetChatController chatController;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new network chat
        /// message that can send out and recieve chat
        /// messages over the network.
        /// </summary>
        public NetChatMessager(NetManager netManager) {
            if (netManager.IsServer) {
                chatController = new NetChatController(netManager as NetServerManager);
            }

            NetMessageListener.OnChatMessage += OnChatMessage;
        }

        /// <summary>
        /// Frees up resources when destroying.
        /// </summary>
        ~NetChatMessager() {
            NetMessageListener.OnChatMessage -= OnChatMessage;
        }
        #endregion

        #region Message Handlers
        /// <summary>
        /// Fired anytime a chat message is recieved.
        /// </summary>
        /// <param name="sender">Always null.</param>
        /// <param name="e">The message recieved</param>
        private void OnChatMessage(object sender, NetMessageArgs e) {
            switch (e.Message?.Type) {
                case NetMessageType.LobbyChat:
                    LobbyChatMessage lobbyMsg = e.Message as LobbyChatMessage;

                    if (chatController == null) {
                        LoggerUtils.Log(lobbyMsg.ToString());
                    }
                    else if (chatController.ValidateLobbyChatMessage(lobbyMsg)) {
                        chatController.SendOutLobbyChatMessage(lobbyMsg);
                        LoggerUtils.Log(lobbyMsg.ToString());
                    }

                    break;

                case NetMessageType.TeamChat:
                    TeamChatMessage teamMsg = e.Message as TeamChatMessage;
                    
                    if(chatController == null) {
                        LoggerUtils.Log(teamMsg.ToString());
                    }
                    else if(chatController.ValidateTeamChatMessage(teamMsg)) {
                        chatController.SendOutTeamChatMessage(teamMsg);
                        LoggerUtils.Log(teamMsg.ToString());
                    }

                    break;
            }
        }
        #endregion

        #region Publics
        /// <summary>
        /// Set the chat name to send messages as.
        /// </summary>
        /// <param name="name">The name messsages will
        /// appear sent as.</param>
        public void SetChatName(string name) {
            ChatName = name;
        }

        /// <summary>
        /// Set the team to send team messages to.
        /// </summary>
        /// <param name="team">The local players team.</param>
        public void SetTeam(NetTeamColor team) {
            Team = team;
        }

        /// <summary>
        /// Send a message to every player in the lobby.
        /// </summary>
        /// <param name="senderName">The nick name to send it as.</param>
        /// <param name="message">The message to send out.</param>
        public void SendLobbyChatMessage(string message) {
            if(ChatName == null) {
                return;
            }

            LobbyChatMessage lobbyMsg = new LobbyChatMessage(ChatName, message);
            VoxelatedEngine.Engine.NetManager.SendMessage(lobbyMsg, SendOptions.ReliableOrdered);
        }

        /// <summary>
        /// Send a message to everyone in your team.
        /// </summary>
        /// <param name="senderName">The nick name to send it as.</param>
        /// <param name="message">The message to send to your team.</param>
        public void SendTeamChatMesssage(string message) {
            //If no team, why send a team message?
            if(ChatName == null || Team == NetTeamColor.None) {
                return;
            }

            TeamChatMessage teamMsg = new TeamChatMessage(Team, ChatName, message);
            VoxelatedEngine.Engine.NetManager.SendMessage(teamMsg, SendOptions.ReliableOrdered);
        }
        #endregion
    }
}
