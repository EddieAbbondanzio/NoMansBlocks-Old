using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Lobby.Match {
    /// <summary>
    /// Allows the admins of the lobby control
    /// what match is going to be played next.
    /// </summary>
    public class NetMatchBuilder : IMatchSelector {
        #region
        #endregion
        public SelectorMode Mode {
            get {
                throw new NotImplementedException();
            }
        }

        public event EventHandler<NetMatchArgs> OnMatchSelected;

        public void GetNextMatch() {
            throw new NotImplementedException();
        }
    }
}
