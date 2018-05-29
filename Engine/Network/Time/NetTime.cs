using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Time {
    /// <summary>
    /// Synchronized time across all clients
    /// and the server. Used to track lobby states.
    /// </summary>
    public class NetTime {
        #region Members
        /// <summary>
        /// How long it's been since the last time sync.
        /// </summary>
        private float timeSinceLastSync;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new NetTime instance.
        /// </summary>
        private NetTime() {
        }
        #endregion

        #region Publics
        /// <summary>
        /// Get the server's net time.
        /// </summary>
        /// <returns>The Server's net time, 0 if not
        /// connected to a server.</returns>
        public DateTime GetServerTime() {
            return DateTime.Now;
        }

        /// <summary>
        /// Get the local net peer's time.
        /// </summary>
        /// <returns>The current time of the local
        /// net peer.</returns>
        public DateTime GetLocalTime() {
            return DateTime.Now;
        }
        #endregion
    }
}
