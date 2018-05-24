using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network.Messages;
using Voxelated.Utilities;

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
        public NetChatMessager() {
            if (VoxelatedEngine.Engine.NetManager.IsServer) {
                chatController = new NetChatController();
            }

            NetManager.OnChatMessage += OnChatMessage;
        }

        /// <summary>
        /// Frees up resources when destroying.
        /// </summary>
        ~NetChatMessager() {
            NetManager.OnChatMessage -= OnChatMessage;
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

                    //Server needs to validate message
                    if(chatController?.ValidateLobbyChatMessage(lobbyMsg) ?? false) {
                        chatController.SendOutLobbyChatMessage(lobbyMsg);
                    }
                    else {
                        return;
                    }

                    //Everyone prints message to screen.
                    DisplayMessage(lobbyMsg.SenderName, lobbyMsg.Message);
                    break;

                case NetMessageType.TeamChat:
                    TeamChatMessage teamMsg = e.Message as TeamChatMessage;

                    //Server needs to validate
                    if(chatController?.ValidateTeamChatMessage(teamMsg) ?? false) {
                        chatController.SendOutTeamChatMessage(teamMsg);
                    }
                    else {
                        return;
                    }

                    //Everyone prints message to screen.
                    DisplayMessage(teamMsg.SenderName, teamMsg.Message);
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
            VoxelatedEngine.Engine.NetManager.SendMessage(lobbyMsg, Lidgren.Network.NetDeliveryMethod.ReliableOrdered, NetChannel.Chat);
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
            VoxelatedEngine.Engine.NetManager.SendMessage(teamMsg, Lidgren.Network.NetDeliveryMethod.ReliableOrdered, NetChannel.Chat);
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Display a chat message to the local user.
        /// </summary>
        /// <param name="senderName">The name of who sent it.</param>
        /// <param name="message">The message they sent.</param>
        private void DisplayMessage(string senderName, string message) {
            LoggerUtils.Log(senderName + ": " + message);
        }
        #endregion
    }
}
