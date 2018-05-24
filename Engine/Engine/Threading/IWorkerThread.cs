using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Voxelated.Engine.Tasks;

namespace Voxelated.Engine.Threading {
    /// <summary>
    /// Represents a worker thread that can 
    /// run tasks on itself.
    /// </summary>
    public interface IWorkerThread {
        /// <summary>
        /// Set where the thread pulls jobs from
        /// </summary>
        void SetWorkSource(ThreadableQueue<WorkTask> tasks);

        /// <summary>
        /// How many tasks it has.
        /// </summary>
        int TaskCount { get; }

        void Stop();
    }
}
