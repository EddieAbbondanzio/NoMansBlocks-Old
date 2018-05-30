using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network {
    /// <summary>
    /// Arguments for any NetManager events. Allows for
    /// the passing of NetPeers around if needed.
    /// </summary>
    public class NetPeerArgs : EventArgs {
        #region Properties
        /// <summary>
        /// The newly connected net peer.
        /// </summary>
        public NetPeer Peer { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new set of Net Manager
        /// event args.
        /// </summary>
        /// <param name="peer">The relevant net peer.</param>
        public NetPeerArgs(NetPeer peer) {
            Peer = peer;
        }
        #endregion
    }
}
