using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Voxelated.Terrain;

namespace Voxelated.Events {
    /// <summary>
    /// Arguments that are passed when a water mesh
    /// is populated
    /// </summary>
    public class WaterPopulatedArgs : EventArgs {
        public Water Water { get; private set; }

        public WaterPopulatedArgs(Water water) {
            Water = water;
        }
    }
}