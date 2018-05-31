using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Lobby.Match {
    /// <summary>
    /// Generates random game mode and map combos
    /// to play in a lobby.
    /// </summary>
    public class NetMatchGenerator : IMatchSelector {
        #region Events
        /// <summary>
        /// Fired when a random match has been generated.
        /// </summary>
        public event EventHandler<NetMatchArgs> OnMatchSelected;
        #endregion

        #region Members

        #endregion

        #region Properties
        /// <summary>
        /// What kind of method is used to generate matches.
        /// </summary>
        public SelectorMode Mode {
            get {
                return SelectorMode.Auto;
            }
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new match generator.
        /// </summary>
        public NetMatchGenerator() {

        }
        #endregion

        #region Public
        /// <summary>
        /// Run the generator to calculate the next net 
        /// match for the lobby to play. Subscribe to the On
        /// MatchSelected event to recieve the match when
        /// it's ready.
        /// </summary>
        public void GetNextMatch() {
            throw new NotImplementedException();
        }
        #endregion
    }
}
