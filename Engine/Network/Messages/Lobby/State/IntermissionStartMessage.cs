using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Network.Lobby;

namespace Voxelated.Network.Messages {
    /// <summary>
    /// Lobby state sync message. Indicates to players in the
    /// lobby that the lobby has transitioned into the Intermission
    /// state before selecting a match.
    /// </summary>
    public class IntermissionStartMessage : NetMessage {
        #region Properties
        /// <summary>
        /// The type of message it is.
        /// </summary>
        public override NetMessageType Type {
            get { return NetMessageType.IntermissionStart; }
        }

        /// <summary>
        /// The category of the message
        /// </summary>
        public override NetMessageCategory Category {
            get { return NetMessageCategory.Lobby; }
        }

        /// <summary>
        /// When the intermission started.
        /// </summary>
        public double StartTime { get; private set; }

        /// <summary>
        /// How long the intermission will run for.
        /// </summary>
        public IntermissionDuration Duration { get; private set; }
        #endregion

        #region Public
        /// <summary>
        /// Outgoing state sync for lobby to
        /// tell players that the lobby has
        /// entered intermission.
        /// </summary>
        /// <param name="startTime">The time that the lobby started intermission.</param>
        /// <param name="duration">How long the intermission will run for.</param>
        public IntermissionStartMessage(double startTime, IntermissionDuration duration) : base(72) {
            StartTime = startTime;
            Duration  = duration;

            buffer.Write(startTime);
            buffer.Write((byte)duration);
        }

        /// <summary>
        /// Decode a state sync message from the 
        /// server telling the client that the lobby
        /// is now in intermission mode.
        /// </summary>
        /// <param name="sender">The server.</param>
        /// <param name="reader">The data of the message.</param>
        public IntermissionStartMessage(NetPeer sender, NetDataReader reader) : base(sender, reader) {
            StartTime = buffer.ReadDouble();
            Duration = (IntermissionDuration)buffer.ReadByte();
        }
        #endregion
    }
}
