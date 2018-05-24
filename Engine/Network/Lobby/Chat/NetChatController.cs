using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network.Messages;
using Voxelated.Utilities;

namespace Voxelated.Network.Lobby {
    /// <summary>
    /// Handles sending out and validating chat 
    /// messages. Only a server has this.
    /// </summary>
    public class NetChatController {
        #region Members
        /// <summary>
        /// Cached reference to the network manager.
        /// </summary>
        private NetServerManager serverManager;
        #endregion 

        #region Constructor(s)
        /// <summary>
        /// Create a new chat handler for communicating with
        /// the other players in the lobby.
        /// </summary>
        public NetChatController(NetServerManager serverManager) {
            this.serverManager = serverManager;
        }
        #endregion

        #region Publics
        /// <summary>
        /// Validate a lobby chat message from a client
        /// before forwarding it out to the other clients.
        /// </summary>
        /// <param name="lobbyMsg">The incoming message.</param>
        /// <returns>True if it hasn't been modded.</returns>
        public bool ValidateLobbyChatMessage(LobbyChatMessage lobbyMsg) {
            if(lobbyMsg == null) {
                return false;
            }

            //Get player id
            byte senderId = serverManager.ClientManager.GetPlayerId(lobbyMsg.SenderConnection);

            //Their id wasn't found.
            if(senderId == byte.MaxValue) {
                return false;
            }

            //Just double check the name is correct.
            return lobbyMsg.SenderName == serverManager.Lobby.GetPlayerNameById(senderId);
        }

        /// <summary>
        /// Vaidate a team chat message from a client before
        /// forwarding it out to other clients.
        /// </summary>
        /// <param name="teamMsg">The incoming message.</param>
        /// <returns>True if it hasn't been modded.</returns>
        public bool ValidateTeamChatMessage(TeamChatMessage teamMsg) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Forward the chat message out to every client. Make sure
        /// it's validated first!
        /// </summary>
        /// <param name="lobbyMsg">The lobby chat message to forward.</param>
        public void SendOutLobbyChatMessage(LobbyChatMessage lobbyMsg) {
            if (lobbyMsg == null) {
                return;
            }

            //Forward it to every client.
            serverManager.SendMessage(lobbyMsg, Lidgren.Network.NetDeliveryMethod.ReliableOrdered, NetChannel.Chat);
        }

        /// <summary>
        /// Forward the team chat message out to every client. Make sure it's
        /// validated first.
        /// </summary>
        /// <param name="teamMsg">The team chat message to forward.</param>
        public void SendOutTeamChatMessage(TeamChatMessage teamMsg) {
            throw new NotImplementedException();
        }
        #endregion
    }
}
