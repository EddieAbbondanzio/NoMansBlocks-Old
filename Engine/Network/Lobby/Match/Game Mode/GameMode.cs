using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Utilities;

namespace Voxelated.Network.Lobby.Match {
    /// <summary>
    /// Template for creating new game modes
    /// for players to play in lobbies.
    /// </summary>
    public sealed class GameMode {
        public static GameMode TeamDeathmatch = new GameMode() {
            Name = "Team Deathmatch",
            Description = "",
        };

        #region Constants
        /// <summary>
        /// Maximum character length for the name.
        /// </summary>
        public const int NameLengthLimit = 24;

        /// <summary>
        /// Maximum character length for the description.
        /// </summary>
        public const int DescriptionLengthLimit = 176;
        #endregion

        #region Properties
        /// <summary>
        /// If the game mode is a pre-defined, or user
        /// created one.
        /// </summary>
        public bool IsCustom { get; private set; }

        /// <summary>
        /// The name of the game mode.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The description of the game mode.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// How many seconds the game mode can run for.
        /// </summary>
        public uint TimeLimit { get; private set; }

        /// <summary>
        /// The score needed to win the match (if any).
        /// </summary>
        public uint ScoreLimit { get; private set; }

        /// <summary>
        /// What kind of objective is to be achieved
        /// in this game mode.
        /// </summary>
        public ObjectiveType ObjectiveType { get; private set; }

        /// <summary>
        /// The team mode of the game.
        /// </summary>
        public TeamMode TeamMode { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Used by this class to create new standard
        /// game modes.
        /// </summary>
        private GameMode() {
        }

        /// <summary>
        /// Allows for creating new game modes.
        /// </summary>
        /// <param name="name">The name of the game mode.</param>
        /// <param name="description">The description explaining the game mode.</param>
        /// <param name="type">The type of objective.</param>
        /// <param name="mode">How many teams.</param>
        /// <param name="timeLimit">How many seconds per match.</param>
        /// <param name="scoreLimit">How many points are required to win.</param>
        public GameMode(string name, string description, ObjectiveType type, TeamMode mode, uint timeLimit, uint scoreLimit) {
            Name          = StringUtils.Clamp(name, NameLengthLimit);
            Description   = StringUtils.Clamp(description, DescriptionLengthLimit);
            ObjectiveType = type;
            TeamMode      = mode;
            TimeLimit     = timeLimit;
            ScoreLimit    = scoreLimit;
        }
        #endregion
    }
}
