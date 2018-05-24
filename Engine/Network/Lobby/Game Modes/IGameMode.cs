using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Lobby {
    /// <summary>
    /// Represents a game mode for a net lobby
    /// </summary>
    public interface IGameMode {
        #region Properties
        /// <summary>
        /// How many minutes the game runs for.
        /// If -1, game never ends
        /// </summary>
        int TimeLimit { get; }

        /// <summary>
        /// The score needed to win the game
        /// </summary>
        int ScoreLimit { get; }

        /// <summary>
        /// The name to display the game mode
        /// as.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Text based description to show
        /// in the menu describing what the game
        /// mode is, and how to play.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// How many teams there are
        /// </summary>
        int TeamCount { get; }
        #endregion
    }
}
