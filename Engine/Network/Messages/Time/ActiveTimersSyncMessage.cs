using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Messages {
    /// <summary>
    /// Handles giving a new client all of the 
    /// active timers being handled by time synchronizer.
    /// Typically will only be 1-4 timers at MAX.
    /// </summary>
    public class ActiveTimersSyncMessage {
    }
}
