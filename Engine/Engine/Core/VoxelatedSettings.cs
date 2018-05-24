using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxelated.Utilities;

namespace Voxelated.Engine {
    /// <summary>
    /// Wrapper for the various settings the
    /// engine needs to know about.
    /// </summary>
    public struct VoxelatedSettings {
        #region Statics
        /// <summary>
        /// Default settings for a server instance of the voxelated
        /// game engine. Client needs to handle rendering meshes
        /// so it gets more workers in it's thread pool.
        /// </summary>
        public static VoxelatedSettings ClientSettings = new VoxelatedSettings(60, 60, Environment.ProcessorCount);

        /// <summary>
        /// Default settings for a server instance of the
        /// voxelated game engine. Server doesn't have rendering
        /// and doesn't need as many cores.
        /// </summary>
        public static VoxelatedSettings ServerSettings = new VoxelatedSettings(60, 15, MathUtils.Min(2, Environment.ProcessorCount));
        #endregion

        /// <summary>
        /// How many times per second the engine updates.
        /// </summary>
        public int UpdateTicksPerSecond { get; private set; }

        /// <summary>
        /// How many times per second the network
        /// system updates. If this is greater than
        /// UpdateTickerPerSecond an error will be thrown.
        /// It must also be a factor of UTPS.
        /// </summary>
        public int NetworkTicksPerSecond { get; private set; }

        /// <summary>
        /// How many threads the task scheduler has.
        /// </summary>
        public int WorkerThreadCount { get; private set; }

        /// <summary>
        /// Wrapper for the settings.
        /// </summary>
        public VoxelatedSettings(int updateTickRate, int netTickRate, int workerThreadCount) {
            //Verify tick settings
            if (netTickRate > updateTickRate) {
                throw new Exception("Network Ticks must be less than update ticks!");
            }
            else if (updateTickRate % netTickRate != 0) {
                throw new Exception("Network Ticks must be a factor of Update Ticks!");
            }

            UpdateTicksPerSecond  = updateTickRate;
            NetworkTicksPerSecond = netTickRate;
            WorkerThreadCount     = workerThreadCount;
        }
    }
}
