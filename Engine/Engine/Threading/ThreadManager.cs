using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Voxelated.Engine.Tasks;
using Voxelated.Terrain;
using Voxelated.Utilities;

namespace Voxelated.Engine.Threading {
    /// <summary>
    /// Manager class for handling creating, and stopping new threads.
    /// </summary>
    public class ThreadManager {
        #region Thread Class
        /// <summary>
        /// Worker thread extends standard c# threads to have 
        /// a task queue, and get tasks from other threads to help.
        /// Only the ThreadManager can create new threads.
        /// </summary>
        private class WorkerThread : IWorkerThread {
            #region Properties
            /// <summary>
            /// Reference back to the manager.
            /// </summary>
            private ThreadManager ThreadManager { get; set; }

            /// <summary>
            /// The tasks waiting to be ran on the thread
            /// </summary>
            private ThreadableQueue<WorkTask> TaskQueue { get; set; }

            /// <summary>
            /// If the worker thread is currently active.
            /// </summary>
            public bool IsRunning { get; private set; }

            /// <summary>
            /// If the thread has any tasks waiting to run.
            /// </summary>
            public bool HasTasks { get { return TaskQueue.Count > 0; } }

            /// <summary>
            /// Returns how many tasks are waiting to be ran on the thread.
            /// </summary>
            public int TaskCount { get { return TaskQueue.Count; } }
            #endregion

            #region Members
            /// <summary>
            /// How often the thread should check for work.
            /// </summary>
            private ThreadWorkPriority priority;

            /// <summary>
            /// The worker thread.
            /// </summary>
            private Thread thread;
            #endregion

            #region Constructor(s)
            /// <summary>
            /// Create a new worker thread with the specified priority.
            /// </summary>
            /// <param name="manager">The manager that controls the thread.</param>
            /// <param name="priority">How often the thread should perform work.</param>
            public WorkerThread(ThreadManager manager, ThreadWorkPriority priority) {
                //Create the task queue
                this.priority = priority;
                this.IsRunning = true;

                //Start up the thread
                thread = new Thread(ThreadedWork);
                thread.IsBackground = true;
                thread.Start();
            }
            #endregion

            #region Publics
            /// <summary>
            /// Set the queue reference for where the
            /// thread should get jobs from.
            /// </summary>
            /// <param name="tasks">The job source.</param>
            public void SetWorkSource(ThreadableQueue<WorkTask> tasks) {
                this.TaskQueue = tasks;
            }

            /// <summary>
            /// Stops the thread
            /// </summary>
            public void Stop() {
                if (!IsRunning) {
                    return;
                }

                IsRunning = false;
                thread.Join();
            }
            #endregion

            #region Helpers
            /// <summary>
            /// The thread loop
            /// </summary>
            private void ThreadedWork() {
                IsRunning = true;

                while (IsRunning) {
                    //Run a local task
                    if(TaskQueue != null && TaskQueue.Count > 0) {
                        WorkTask currTask = TaskQueue.Dequeue();

                        if(currTask != null) {
                            currTask.ExecuteTask();
                        }
                    }
                    //No work. Sleep a bit.
                    else {
                        Thread.Sleep((byte)priority);
                    }
                }
            }
            #endregion
        }
        #endregion

        #region Members
        /// <summary>
        /// The private list of all the threads out there.
        /// </summary>
        private List<WorkerThread> workerThreads;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Instantiate a new thread manager with the specified
        /// number of worker threads.
        /// </summary>
        /// <param name="poolSize">The number of threads to keep in the pool.</param>
        public ThreadManager(int poolSize) {
            if(poolSize <= 0) {
                throw new ArgumentOutOfRangeException("PoolSize", "Must be greater than 0!");
            }
            else if(poolSize > Environment.ProcessorCount) {
                throw new ArgumentOutOfRangeException("PoolSize", "Should not be larger than CPU core count!");
            }

            LoggerUtils.Log(string.Format("Thread Manager: Started {0} threads.", poolSize), LogLevel.Debug);
            workerThreads = new List<WorkerThread>();

            //Add the pool threads
            for (int i = 0; i < poolSize; i++) {
                workerThreads.Add(new WorkerThread(this, ThreadWorkPriority.High));
            }
        }
        #endregion

        #region Publics
        /// <summary>
        /// Stop all the threads in the pool.
        /// </summary>
        public void Stop() {
            LoggerUtils.Log(string.Format("Thread Manager: Stopped {0} threads.", workerThreads.Count), LogLevel.Debug);
            workerThreads.ForEach(t => t.Stop());
        }

        /// <summary>
        /// Returns an interface reference to each
        /// of the worker threads in the pool.
        /// </summary>
        /// <returns></returns>
        public List<IWorkerThread> GetWorkerThreads() {
            return workerThreads.ToList<IWorkerThread>();
        }
        #endregion
    }
}
