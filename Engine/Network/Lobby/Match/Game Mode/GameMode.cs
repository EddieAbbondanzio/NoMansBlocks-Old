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
        #region Defaults
        /// <summary>
        /// Free for all death match.
        /// </summary>
        public static GameMode Deathmatch = new GameMode(
            "Deathmatch",
            "Free for All. First to reach the score limit by killing others wins.",
            ObjectiveType.Deathmatch,
            TeamMode.FreeForAll,
            0,
            5000
            );

        /// <summary>
        /// Standard Team Death Match found in most first person shooters.
        /// </summary>
        public static GameMode TeamDeathmatch = new GameMode(
            "Team Deathmatch",
            "Two teams face off against each other. The first team to reach the score limit by killing"
            + " opponents of the opposite team first wins.",
            ObjectiveType.Deathmatch,
            TeamMode.Dual,
            0,
            7500
            );
        
        /// <summary>
        /// Team capture the flag.
        /// </summary>
        public static GameMode CaptureTheFlag = new GameMode(
            "Capture the Flag",
            "Two teams attempt to capture the other teams flag.",
            ObjectiveType.CaptureTheFlag,
            TeamMode.Dual,
            0,
            500
            );
        
        /// <summary>
        /// One round demolition.
        /// </summary>
        public static GameMode Demolition = new GameMode(
            "Demolition",
            "Face off in an asymetrical game mode where teams take turns playing offense, and defense. Win the game by planting the bomb in the enemies spawn.",
            ObjectiveType.Demolition,
            TeamMode.Dual,
            0,
            100
            );
        #endregion

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
