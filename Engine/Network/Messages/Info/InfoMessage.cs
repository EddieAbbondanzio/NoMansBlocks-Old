using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voxelated.Network.Messages {
    /// <summary>
    /// Represents several of the debug and warning messages from
    /// lidgren.
    /// </summary>
    public class InfoMessage : NetMessage {
        #region Properties
        /// <summary>
        /// The type of message it is.
        /// </summary>
        public override NetMessageType Type {
            get { return NetMessageType.Info; }
        }

        /// <summary>
        /// What category the message falls into
        /// </summary>
        public override NetMessageCategory Category {
            get { return NetMessageCategory.Info; }
        }

        /// <summary>
        /// The information of the message
        /// </summary>
        public string Information { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Decode a recieved info message.
        /// </summary>
        public InfoMessage(NetEndPoint senderIP, int errorCode) : base() {
            if(senderIP != null) {
                Information = "NetError from sender: " + senderIP.ToString() + " code: " + errorCode;
            }
            else {
                Information = "NetError code: " + errorCode;
            }
        }
        #endregion
    }
}
