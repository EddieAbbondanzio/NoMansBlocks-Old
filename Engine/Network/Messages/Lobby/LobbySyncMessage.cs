﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network.Lobby;
using Voxelated.Network.Lobby.Match;
using Voxelated.Utilities;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Voxelated.Network.Messages {
    /// <summary>
    /// Initial message send to clients containing the lobby
    /// state so they can be synced up.
    /// </summary>
    public class LobbySyncMessage : NetMessage {
        #region Properties
        /// <summary>
        /// The type of message it is.
        /// </summary>
        public override NetMessageType Type {
            get { return NetMessageType.LobbySync; }
        }

        /// <summary>
        /// The category of the message
        /// </summary>
        public override NetMessageCategory Category {
            get { return NetMessageCategory.Lobby; }
        }

        /// <summary>
        /// The id of the local player.
        /// </summary>
        public byte PlayerId { get; private set; }

        /// <summary>
        /// The list of players currently in the 
        /// lobby.
        /// </summary>
        public List<NetPlayer> Players { get; private set; }
        #endregion

        #region Constructor(s)
        public LobbySyncMessage(byte playerId, NetLobby lobby) : base() {
            PlayerId = playerId;
            Players = lobby.Players;

            buffer.Write(PlayerId);
            buffer.Write((byte)lobby.PlayerCount);
            lobby.Players.ForEach(p => buffer.Write(p));
        }

        /// <summary>
        /// Decode an incoming lobby sync message  that
        /// was recieved.
        /// </summary>
        public LobbySyncMessage(NetPeer sender, NetDataReader reader) : base(sender, reader) {
            PlayerId = buffer.ReadByte();
            //Pull in the player info.
            Players = new List<NetPlayer>();
            int playerCount = buffer.ReadByte();

            try {
                LoggerUtils.Log("Player count is " + playerCount);
                for (int p = 0; p < playerCount; p++) {
                    NetPlayer player = buffer.ReadSerializableObject() as NetPlayer;

                    if (player != null) {
                        Players.Add(player);
                    }
                }
            }
            catch(Exception e) {
                LoggerUtils.LogError(e.ToString());
            }

            LoggerUtils.Log("LobbySyncMessage: " + Players[0].ToString());
        }
        #endregion
    }
}
