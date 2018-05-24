using System;

namespace Voxelated.Terrain {
    /// <summary>
    /// The source that the world is being loaded from.
    /// </summary>
    [Serializable]
    public enum WorldType {
        Loaded = 0,
        Converted = 1,
        Empty = 2,
        Desert = 3,
        Valley = 4,
        Forest = 5,
        City = 6
    }
}