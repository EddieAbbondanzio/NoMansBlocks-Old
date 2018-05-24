using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voxelated {
    /// <summary>
    /// Enum to represent all 6 cardinal directions. Directions
    /// that have a component of -1 are considered backfaces.
    /// </summary>
    public enum Direction : byte {
        /// <summary>
        /// Direction with a value of (0, 0, 1).
        /// </summary>
        north = 5,

        /// <summary>
        /// Direction with a value of (1, 0, 0).
        /// </summary>
        east = 3,

        /// <summary>
        /// Direction with a value of (0, 0, -1).
        /// </summary>
        south = 2,

        /// <summary>
        /// Direction with a value of (-1, 0, 0).
        /// </summary>
        west = 0,

        /// <summary>
        /// Direction with a value of (0, 1, 0).
        /// </summary>
        up = 4,

        /// <summary>
        /// Direction with a vlaue of (0, -1, 0).
        /// </summary>
        down = 1
    }
}
