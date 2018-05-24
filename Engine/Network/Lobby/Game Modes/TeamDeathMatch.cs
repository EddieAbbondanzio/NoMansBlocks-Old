using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Lobby {
    /// <summary>
    /// Team deathmatch is a game where
    /// teams compete to reach
    /// the target score. Which ever
    /// teams hits it first wins.
    /// </summary>
    public class TeamDeathMatch : IGameMode {
        #region Properties
        /// <summary>
        /// Games default to a 30 minute time limit
        /// for now.
        /// </summary>
        public int TimeLimit { get { return 30; } }

        /// <summary>
        /// Each kill earns the team 100 points. 7500
        /// wins the match
        /// </summary>
        public int ScoreLimit { get { return 7500; } }

        /// <summary>
        /// The name to display in the scoreboard screen / menu
        /// </summary>
        public string Name { get { return "Team Death Match"; } }

        /// <summary>
        /// Text based description to show in the lobby
        /// </summary>
        public string Description { get { return "Teams face off in a match against each other. First team to reach the score limt wins. Points are earned by killing members of the other team."; } }

        /// <summary>
        /// For now team count defaults to 2 for team death match
        /// </summary>
        public int TeamCount { get { return 2; } }
        #endregion
    }
}
