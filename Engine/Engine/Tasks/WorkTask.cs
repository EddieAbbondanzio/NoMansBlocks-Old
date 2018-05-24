using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Engine.Tasks {
    /// <summary>
    /// Base class for tasks that are ran by the task scheduler.
    /// </summary>
    public abstract class WorkTask {
        /// <summary>
        /// Who created the task
        /// </summary>
        public ITaskCreator Creator { get; private set; }

        /// <summary>
        /// The type of task it is.
        /// </summary>
        public abstract TaskType Type { get; }

        /// <summary>
        /// Create a new instance of a work task
        /// </summary>
        public WorkTask(ITaskCreator creator) {
            Creator = creator;
        }

        /// <summary>
        /// Execute and call back to the creator that it completed.
        /// </summary>
        public void ExecuteTask() {
            Execute();
            Creator.TaskCompleted(this);
        }

        /// <summary>
        /// Alert the creator that this task has completed.
        /// </summary>
        protected abstract void Execute();
    }
}
