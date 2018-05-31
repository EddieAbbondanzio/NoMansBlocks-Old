using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Lobby.Match {
    /// <summary>
    /// Controls picking, loading, and transfering
    /// the lobby into matches.
    /// </summary>
    public class NetMatchManager {
        #region Properties
        /// <summary>
        /// Is there a match currently being played?
        /// </summary>
        public bool MatchInProgress {
            get {
                return CurrentMatch != null;
            }
        }

        /// <summary>
        /// The current match of the manager. Can be null.
        /// </summary>
        public NetMatch CurrentMatch { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new manager that will handle playing
        /// matches for the players.
        /// </summary>
        public NetMatchManager() {

        }
        #endregion

        #region Publics
        /// <summary>
        /// Start a new match for the players
        /// to play in.
        /// </summary>
        /// <param name="match">The match to play.</param>
        public void StartMatch(NetMatch match) {
            CurrentMatch = match;
        }

        /// <summary>
        /// Stop the current match and return
        /// the players to the lobby.
        /// </summary>
        public void StopMatch() {

        }
        #endregion
    }
}
