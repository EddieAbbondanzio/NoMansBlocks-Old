using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Lobby.Match {
    /// <summary>
    /// Allows players to vote on a selection
    /// of map choices that they want to play.
    /// </summary>
    public class NetMatchVoter : IMatchSelector {
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
