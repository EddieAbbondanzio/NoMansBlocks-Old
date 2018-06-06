using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Utilities;

namespace Voxelated.Network.Messages {
    /// <summary>
    /// Sends the TimerFactory from the server to sync
    /// up a client with the current timers in use.
    /// </summary>
    internal class ActiveTimersSync : NetMessage {
        #region Properties
        /// <summary>
        /// The type of message it is.
        /// </summary>
        public override NetMessageType Type {
            get { return NetMessageType.ActiveTimersSync; }
        }

        /// <summary>
        /// The category of the message
        /// </summary>
        public override NetMessageCategory Category {
            get { return NetMessageCategory.Time; }
        }
        
        /// <summary>
        /// The timers of the server.
        /// </summary>
        public TimerFactory TimerFactory { get; private set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new outgoing timers sync message.
        /// </summary>
        /// <param name="factory">The timer factory to send out.</param>
        public ActiveTimersSync(TimerFactory factory) {
            TimerFactory = factory;

            buffer.Write(factory);
        }

        /// <summary>
        /// Rebuild an incoming active timers sync message.
        /// </summary>
        /// <param name="sender">The server that sent the message.</param>
        /// <param name="reader">The data of the message.</param>
        public ActiveTimersSync(NetPeer sender, NetDataReader reader) : base (sender, reader) {
            try {
                TimerFactory = buffer.ReadSerializableObject() as TimerFactory;
            }
            catch(Exception e) {
                LoggerUtils.LogError(e.ToString());
            }
        }
        #endregion

    }
}
