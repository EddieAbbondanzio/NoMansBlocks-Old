using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Engine.Tasks {
    /// <summary>
    /// Interface to represent classes that can create work tasks
    /// </summary>
    public interface ITaskCreator {
        /// <summary>
        /// Called by the task scheduler to let
        /// a task creator know one of it's tasks
        /// have been finished.
        /// </summary>
        void TaskCompleted(WorkTask task);
    }
}
