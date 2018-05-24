using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lidgren.Network;

namespace Voxelated.Events {
    /// <summary>
    /// Event arguments for a message recieved over the 
    /// network event.
    /// </summary>
    public class MessageRecievedArgs : EventArgs {
        /// <summary>
        /// The message recieved.
        /// </summary>
        public NetIncomingMessage Message { get; private set; }

        public MessageRecievedArgs(NetIncomingMessage inMsg) {
            Message = inMsg;
        }
    }
}
