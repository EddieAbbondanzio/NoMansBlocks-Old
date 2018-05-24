using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network {
    /// <summary>
    /// Arguments to pass a net message through an event
    /// </summary>
    public class NetMessageArgs : EventArgs {
        #region Properties
        /// <summary>
        /// The message in question.
        /// </summary>
        public NetMessage Message { get; private set; }
        #endregion

        #region Constructor(s)
        public NetMessageArgs(NetMessage message) {
            Message = message;
        }
        #endregion
    }
}
