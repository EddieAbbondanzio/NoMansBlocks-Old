﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Lobby.Match {
    /// <summary>
    /// Represents a game match of a net lobby. This allows
    /// for various game modes and more.
    /// </summary>
    public class NetMatch {
        #region Properties
        /// <summary>
        /// The game mode of the match.
        /// </summary>
        public IGameMode GameMode { get; private set; }

        /// <summary>
        /// The teams of the lobby.
        /// </summary>
        public List<NetTeam> Teams { get; private set; }

        /// <summary>
        /// The timer of the match.
        /// </summary>
        public NetMatchTimer Timer { get; private set; }
        
        /// <summary>
        /// The time according to the server
        /// of when the match began.
        /// </summary>
        public double NetStartTime { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new match for the players of the
        /// lobby to play in.
        /// </summary>
        /// <param name="gameMode">The game mode of the match.</param>
        public NetMatch(IGameMode gameMode) {
            GameMode = gameMode;
        }
        #endregion

        #region Publics
        /// <summary>
        /// Start the match and allow players to begin
        /// playing.
        /// </summary>
        public void StartMatch() {
            Timer.Start();
        }

        /// <summary>
        /// Close out the match, and return the players
        /// back to the menu.
        /// </summary>
        public void StopMatch() {
            Timer.Stop();
        }
        #endregion
    }
}