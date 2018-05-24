using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Voxelated.Events;

namespace Voxelated.Terrain {
    public class WaterManager {

        public Color32[] WaterColors { get; set; }


        /// <summary>
        /// Fired each time a water mesh is populated.
        /// </summary>
        public event EventHandler<WaterPopulatedArgs> OnWaterPopulated;
        /// <summary>
        /// The water of the world.
        /// </summary>
        private Water[,] Waters;

        public WaterManager(World world) {
            //Waters = new Water[waterSize.x, waterSize.y];

            //for (int x = 0; x < waterSize.x; x++) {
            //    for (int z = 0; z < waterSize.y; z++) {
            //        Vector3Int waterPos = new Vector3Int(x, 0, z) * Water.WaterSize;
            //        Waters[x, z] = new Water(waterPos);
            //    }
            //}


        }

        public WaterManager(Vect2Int size) {

        }

        public void PopulateWater(Color32[] colors) {

        }

        public void GenerateWater() {

            //for (int x = 0; x < Waters.GetLength(0); x++) {
            //    for (int z = 0; z < Waters.GetLength(1); z++) {
            //        Water water = Waters[x, z];
            //        water.Populate(WaterColors);

            //        //Fire off event. World Renderer is listening...
            //        if (OnWaterPopulated != null) {
            //            OnWaterPopulated(this, new WaterPopulatedArgs(water));
            //        }
            //    }
            //}
        }
    }
}
