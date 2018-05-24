using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Voxelated.Engine.Threading;
using Voxelated.Utilities;

namespace Voxelated.Engine.Tasks {
    /// <summary>
    /// Handles the worker threads that run the tasks
    /// and when tasks should be ran.
    /// </summary>
    public class TaskScheduler {
        #region Components
        /// <summary>
        /// Handles starting and stopping the worker threads.
        /// </summary>
        private ThreadManager threadManager;
        #endregion

        #region Members
        /// <summary>
        /// The singleton instance of the task scheduler.
        /// </summary>
        private static TaskScheduler scheduler;

        /// <summary>
        /// How many threads the pool will run.
        /// </summary>
        private int threadCount;

        /// <summary>
        /// The threads to perform tasks on. There's
        /// 1 per core of the computer running the engine.
        /// </summary>
        private List<IWorkerThread> workerThreads;

        /// <summary>
        /// Tasks that are waiting to be ran.
        /// </summary>
        private ThreadableQueue<WorkTask> taskQueue;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new task scheduler that maintains a worker
        /// thread pool that can be given tasks to perform.
        /// </summary>
        /// <param name="threadCount">The number of threads to
        /// keep in the pool.</param>
        private TaskScheduler(int threadCount) {
            taskQueue = new ThreadableQueue<WorkTask>();
            threadManager = new ThreadManager(threadCount);

            //Create the workers and set their work source.
            workerThreads = threadManager.GetWorkerThreads();
            workerThreads.ForEach(wt => wt.SetWorkSource(taskQueue));
        }
        #endregion

        #region Publics
        /// <summary>
        /// Starts up the thread pool.
        /// </summary>
        public static void Start() {
            if (scheduler == null) {
                int threadCount = VoxelatedEngine.Engine.Settings.WorkerThreadCount;
                scheduler = new TaskScheduler(threadCount);
            }
        }

        /// <summary>
        /// Shuts down the threadpool and merges
        /// worker threads back into the main thread.
        /// </summary>
        public static void Stop() {
            if(scheduler != null) {
                scheduler.threadManager.Stop();
                scheduler = null;
            }
        }

        /// <summary>
        /// Adds another work task to the task scheduler
        /// to perform.
        /// </summary>
        /// <param name="task">The task to do.</param>
        public static void AddTask(WorkTask task) {
            if(task == null) {
                throw new ArgumentNullException("Task cannot be null!");
            }

            if(scheduler != null) {
                scheduler.taskQueue.Enqueue(task);
            }
        }
        #endregion
    }
}
