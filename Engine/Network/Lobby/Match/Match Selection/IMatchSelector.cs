using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Lobby.Match {
    /// <summary>
    /// Utitlity for selecting a map and
    /// game mode for players to play on.
    /// </summary>
    public interface IMatchSelector {
        #region Events
        /// <summary>
        /// When a match has been picked by the selector
        /// and is ready to be played by the lobby.
        /// </summary>
        event EventHandler<NetMatchArgs> OnMatchSelected;
        #endregion

        #region Properties
        /// <summary>
        /// What method is used to 
        /// determine what maps +
        /// game modes.
        /// </summary>
        SelectorMode Mode { get; }
        #endregion

        #region Publics
        /// <summary>
        /// Figures out the next match. The match
        /// isn't returned since some options
        /// require user input therefore aren't instant.
        /// </summary>
        void GetNextMatch();
        #endregion
    }
}
