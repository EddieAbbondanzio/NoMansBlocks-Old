using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib;

namespace Voxelated.Network.Server {
    /// <summary>
    /// Represents the info of a client connected
    /// to the server.
    /// </summary>
    public class NetClientConnection {
        #region Properties
        /// <summary>
        /// The LiteNetLib NetPeer of the client's
        /// connection to the server.
        /// </summary>
        public NetPeer Peer { get; private set; }

        /// <summary>
        /// The permissions level of the client. This
        /// controls what commands / more they have
        /// access to.
        /// </summary>
        public NetPermissions Permissions { get; set; }

        /// <summary>
        /// The player id of the client.
        /// </summary>
        public byte PlayerId { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new NetClientConnection with a status of initialied.
        /// Allows for some additional tracking info on top of the LiteNetLib
        /// NetPeer class.
        /// </summary>
        /// <param name="peer">The LiteNetLib network peer.</param>
        /// <param name="permissions">The player permissions of the connection.</param>
        /// <param name="playerId">The unique player id of the connection.</param>
        public NetClientConnection(NetPeer peer, NetPermissions permissions, byte playerId) {
            Peer = peer;
            Permissions = permissions;
            PlayerId = playerId;
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Convert the connection into a text friendly format.
        /// </summary>
        public override string ToString() {
            return string.Format("{0}: Player Id: {1} Permissions Level: {2}", Peer.EndPoint.ToString(), PlayerId, Permissions.ToString());
        }
        #endregion
    }
}
