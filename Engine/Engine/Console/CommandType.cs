using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Engine.Console {
    /// <summary>
    /// Controls where the command can be called
    /// from. Common allows a command
    /// to be called via server, or client.
    /// </summary>
    [Serializable]
    public enum CommandType {
        Server = 0,
        Client = 1,
        Common = 2,
    }
}
