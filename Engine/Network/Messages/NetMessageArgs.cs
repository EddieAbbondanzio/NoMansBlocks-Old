using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network.Messages;

namespace Voxelated.Network.Messages {
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
        /// <summary>
        /// Create a new set of event arguments
        /// for a message that was recieved.
        /// </summary>
        /// <param name="message">The incoming message.</param>
        public NetMessageArgs(NetMessage message) {
            Message = message;
        }
        #endregion
    }
}
