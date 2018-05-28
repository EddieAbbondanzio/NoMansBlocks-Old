using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network.Lobby;
using Voxelated.Network.Client;
using Voxelated.Network.Messages;
using Voxelated.Utilities;
using Voxelated.Serialization;
using LiteNetLib;
using Voxelated.Network.Server;

namespace Voxelated.Network.Lobby {
    /// <summary>
    /// Represents a collection of players in a server.
    /// </summary>
    public class NetLobby {
        #region Properties
        /// <summary>
        /// Number of players currently in the lobby.
        /// </summary>
        public int PlayerCount { get { return Players.Count; } }

        /// <summary>
        /// The players of the lobby.
        /// </summary>
        public List<NetPlayer> Players { get; private set; }

        /// <summary>
        /// The chat interface of the lobby. Handles sending out
        /// and recieving messages between players.
        /// </summary>
        public NetChatMessager ChatMessager { get; private set; }

        /// <summary>
        /// The various settings of the lobby such as intermission
        /// time and more.
        /// </summary>
        public NetLobbySettings Settings { get; private set; }
        #endregion

        #region Members
        /// <summary>
        /// The player that's local to this instance. If the instance
        /// is a server then this is null.
        /// </summary>
        private NetPlayer localPlayer;

        /// <summary>
        /// Cached reference to the network manager of the engine.
        /// </summary>
        private NetManager netManager;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new lobby.
        /// </summary>
        public NetLobby(NetManager netManager) {
            this.netManager = netManager;

            //Create the new members
            localPlayer = null;
            Players      = new List<NetPlayer>();
            ChatMessager = new NetChatMessager(netManager);

            NetMessageListener.OnLobbyMessage      += OnLobbyMessage;
            NetMessageListener.OnConnectionMessage += OnConnectionMessage;
        }

        /// <summary>
        /// Called when the resouces are released. Removes the event 
        /// subscriptions to prevent a memory leak.
        /// </summary>
        ~NetLobby() {
            NetMessageListener.OnLobbyMessage      -= OnLobbyMessage;
            NetMessageListener.OnConnectionMessage -= OnConnectionMessage;
        }
        #endregion

        #region Message Hanldlers
        /// <summary>
        /// Fired when a player alert for a player leaving
        /// or joining is recieved.
        /// </summary>
        /// <param name="sender">Always null.</param>
        /// <param name="e">The message that was recieved.</param>
        private void OnLobbyMessage(object sender, NetMessageArgs e) {
            switch (e.Message?.Type) {
                //Joined lobby message
                case NetMessageType.LobbySync:
                    LobbySyncMessage syncMessage = e.Message as LobbySyncMessage;

                    if(syncMessage != null) {
                        Players = syncMessage.Players;
                        localPlayer = Players.Find(p => p.Id == syncMessage.PlayerId);
                        ChatMessager.SetChatName(localPlayer.NickName);
                    }
                    break;

                //A new player joined the lobby.
                case NetMessageType.PlayerJoined:
                    PlayerJoinedMessage joinedAlert = e.Message as PlayerJoinedMessage;

                    if(joinedAlert != null) {
                        AddNewPlayer(joinedAlert.PlayerId, joinedAlert.PlayerName);
                    }
                    break;

                //An existing player is leaving.
                case NetMessageType.PlayerLeft:
                    PlayerLeftMessage leftAlert = e.Message as PlayerLeftMessage;

                    if(leftAlert != null) {
                        RemovePlayer(leftAlert.PlayerId);
                    }
                    break;
            }
        }

        /// <summary>
        /// Listens for a disconnected message. When one is recieved it resets
        /// the lobby.
        /// </summary>
        /// <param name="sender">Always null.</param>
        /// <param name="e">THe message recieved</param>
        private void OnConnectionMessage(object sender, NetMessageArgs e) {
            //if(e.Message.Type == NetMessageType.Disconnected) {
            //    DisconnectedMessage disconnectedMsg = e.Message as DisconnectedMessage;
            //    LoggerUtils.Log("Disconnected from the server. Reason: " + disconnectedMsg?.Reason);

            //    //Empty out the player list, and re add local
            //    Players.Clear();
            //    Players.Add(localPlayer);

            //    //Reset player name
            //    NetClientManager clientManager = netManager as NetClientManager;
            //    localPlayer.NickName = clientManager.Settings.Name;
            //}
        }
        #endregion

        #region Publics
        /// <summary>
        /// Add a new client to the lobby. This sends out a sync message to
        /// all other clients to let them know a new player joined.
        /// </summary>
        /// <param name="client">The connection of the new player.</param>
        /// <param name="playerId">The unique player id of the new player.</param>
        /// <param name="nickName">The unique nick name of the player that
        /// other players will see.</param>
        public void AddNewClient(NetClientConnection client, byte playerId, string nickName) {
            //Only server can call this
            if (!(netManager?.IsServer ?? false)) {
                return;
            }

            //Check that the id isn't already in use.
            if(Players.FindIndex(p => p.Id == playerId) != -1) {
                throw new ArgumentException("Id is already in used by another player");
            }

            //Ensure their name is unique.
            string uniqueName = nickName;
            for (int i = 0; Players.FindIndex(p => p.NickName == uniqueName) != -1; i++) {
                uniqueName = nickName + i;
            }

            AddNewPlayer(playerId, uniqueName);

            //If other clients exist, send them an alert of the new player.
            if (PlayerCount > 1) {
                PlayerJoinedMessage playerJoinedMsg = new PlayerJoinedMessage(playerId, uniqueName);
                netManager.SendMessage(playerJoinedMsg, SendOptions.ReliableOrdered);
            }

            //Send the new client the lobby sync message
            LobbySyncMessage syncMsg = new LobbySyncMessage(playerId, this);
            netManager.SendMessage(syncMsg, client.Peer, SendOptions.ReliableOrdered);
        }

        /// <summary>
        /// Remove a client from the lobby. This alerts all other
        /// players that the player has left.
        /// </summary>
        /// <param name="client">The client's connection
        /// to remove from the lobby.</param>
        public void RemoveClient(NetClientConnection client) {
            //Only server can call this
            if (!(netManager?.IsServer ?? false)) {
                return;
            }

            //Find the player
            if(Players.FindIndex(p => p.Id == client.PlayerId) != -1) {
                RemovePlayer(client.PlayerId);

                if (Players.Count > 0) {
                    PlayerLeftMessage playerLeftMsg = new PlayerLeftMessage(client.PlayerId);
                    netManager.SendMessage(playerLeftMsg, SendOptions.ReliableOrdered);
                }
            }
        }

        /// <summary>
        /// Retrieves the player Id of the player whose name
        /// matches that of what was inputted.
        /// </summary>
        /// <param name="name">The name of the player to look for.</param>
        /// <returns>The player's id if found. Else 255 is returned.</returns>
        public byte GetPlayerIdByName(string name) {
            NetPlayer player = Players.Find(p => p.NickName == name);
            return player?.Id ?? byte.MaxValue;
        }

        /// <summary>
        /// Retrieves the nick name of the
        /// player with the corresponding player id.
        /// </summary>
        /// <param name="playerId">The player id to look for.</param>
        /// <returns>The nick name of the player found.</returns>
        public string GetPlayerNameById(byte playerId) {
            NetPlayer player = Players.Find(p => p.Id == playerId);
            return player?.NickName ?? null;
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Add a new player to the lobby.
        /// </summary>
        /// <param name="playerId">The new players id.</param>
        /// <param name="nickName">The new players visible name.</param>
        private void AddNewPlayer(byte playerId, string nickName) {
            //Now create their new player
            NetPlayer player = new NetPlayer(playerId, nickName);
            Players.Add(player);

            LoggerUtils.Log("NetLobby: Added: " + player.ToString(), LogLevel.Debug);
            LoggerUtils.Log("NetLobby: New Lobby Size: " + PlayerCount, LogLevel.Debug);
        }

        /// <summary>
        /// Remove a player from the lobby via their id.
        /// </summary>
        private void RemovePlayer(byte id) {
            NetPlayer player = Players.Find(p => p.Id == id);

            //Don't remove a player that doesn't exist.
            if (player != null) {
                Players.Remove(player);

                LoggerUtils.Log("NetLobby: Removed: " + player.ToString(), LogLevel.Debug);
                LoggerUtils.Log("NetLobby: New Lobby Size: " + PlayerCount, LogLevel.Debug);
            }
        }
        #endregion
    }
}
