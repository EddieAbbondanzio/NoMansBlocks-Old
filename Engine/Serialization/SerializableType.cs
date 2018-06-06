using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Serialization {
    /// <summary>
    /// Unique identifier for each type of serializable
    /// object. Allows for a unique byte to identify each
    /// object when serialized as a byte array.
    /// </summary>
    public enum SerializableType : byte {
        NetPlayer        = 0,
        NetPlayerStats   = 1,
        NetTeam          = 2,
        NetLobbySettings = 3,
        TimerFactory     = 4,
        Timer            = 5,
    }
}
