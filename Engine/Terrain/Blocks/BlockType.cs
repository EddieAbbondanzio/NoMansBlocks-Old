using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Terrain {
    /// <summary>
    /// Represents the 4 types of blocks possible
    /// </summary>
    public enum BlockType : byte {
        Air = 0,            //No shit
        Solid = 1,          //Regular blocks
        Liquid = 2,         //Just water for now but maybe more later
        Sprite = 3,       //Grass and shit
    }
}
