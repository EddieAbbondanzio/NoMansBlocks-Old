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
using Voxelated.Network.Lobby.Match;

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

        /// <summary>
        /// The manager that handles running matches for the lobby.
        /// </summary>
        public NetMatchManager MatchManager { get; private set; }

        /// <summary>
        /// Handles picking matches for the lobby to play.
        /// </summary>
        public IMatchSelector MatchSelector { get; private set; }
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
        /// Create a new lobby that adheres to the following settings.
        /// </summary>
        /// <param name="netManager">The net manager of the instance.</param>
        /// <param name="settings">The settings to use for the lobby.</param>
        public NetLobby(NetManager netManager, NetLobbySettings settings) {
            this.netManager = netManager;
            Settings = settings;

            //Create the new members
            localPlayer = null;
            Players = new List<NetPlayer>();
            ChatMessager = new NetChatMessager(netManager);
            MatchManager = new NetMatchManager();

            //Get the match selector.
            switch (Settings.MatchSelectionMode) {
                case SelectorMode.Auto:
                    MatchSelector = new NetMatchGenerator();
                    break;

                case SelectorMode.Manual:
                    MatchSelector = new NetMatchBuilder();
                    break;

                case SelectorMode.Vote:
                    MatchSelector = new NetMatchVoter();
                    break;
            }

            NetMessageListener.OnLobbyMessage += OnLobbyMessage;
        }

        /// <summary>
        /// Called when the resouces are released. Removes the event 
        /// subscriptions to prevent a memory leak.
        /// </summary>
        ~NetLobby() {
            NetMessageListener.OnLobbyMessage -= OnLobbyMessage;
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

        #endregion

        #region Publics
        /// <summary>
        /// Start the intermission before picking a 
        /// match for the players to play. 
        /// </summary>
        public void StartIntermission() {
            //mark time
            //Send out state sync to clients
            //count down to next phase
        }

        /// <summary>
        /// Uses the match selector to select a match
        /// and then load the match on the server and
        /// get clients ready.
        /// </summary>
        public void SelectNextMatch() {

        }

        /// <summary>
        /// Move the lobby into the match.
        /// </summary>
        public void StartMatch() {

        }

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
