using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelated.Utilities;
using Voxelated.Terrain;
using System.Threading.Tasks;

namespace Voxelated.Serialization {
    /// <summary>
    /// Handles loading and saving of world files to their
    /// proper location and format.
    /// </summary>
    public static class WorldFileIO {
        #region Constants
        public const string worldFileExtension = "bxl";
        public const string worldFileDirectory = "Worlds";
        #endregion

        #region Publics
        /// <summary>
        /// Save a world file to memory.
        /// </summary>
        public static bool SaveWorldToFile(string fileName) {
            return false;
            //WorldFile worldFile = new WorldFile(VoxelatedEngine.Instance.World);
            //byte[] worldBytes = worldFile.Serialize();

            //string fullFileName = fileName + "." + worldFileExtension;
            //return FileUtils.SaveFile(worldFileDirectory, fullFileName, worldBytes, true, true);
        }

        /// <summary>
        /// Load a world file from memory
        /// </summary>
        public static WorldContext LoadWorldFile(string fileName) {
            return null;
            //string fullFileName = fileName + "." + worldFileExtension;
            //byte[] worldBytes = FileUtils.LoadFile(worldFileDirectory, fullFileName, true);

            //return new WorldContext(null);//WorldFile.BuildFromSerializedData(worldBytes);
        }
        #endregion
    }
}
