using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Voxelated.Engine.Tasks;
using Voxelated.Events;
using Voxelated.Serialization;
using Voxelated.Terrain.Generation;
using Voxelated.Utilities;

namespace Voxelated.Terrain {
    /// <summary>
    /// Manager class that handles loading and saving of worlds
    /// from their various input sources.
    /// </summary>
    public class WorldHandler : ITaskCreator {
        /// <summary>
        /// Task to handle saving a world to file.
        /// </summary>
        private class SaveWorldTask : WorkTask {
            #region Properties
            /// <summary>
            /// The type of task it is.
            /// </summary>
            public override TaskType Type {
                get { return TaskType.SaveWorld; }
            }

            /// <summary>
            /// The name of the file to save it under.
            /// </summary>
            public string FileName { get; private set; }

            /// <summary>
            /// If the job completed properly or not.
            /// </summary>
            public bool Success { get; private set; }
            #endregion

            #region Constructor(s)
            /// <summary>
            /// Create a new save world to file task.
            /// </summary>
            public SaveWorldTask(ITaskCreator creator, string fileName) : base(creator) {
                FileName = fileName;
            }
            #endregion

            #region Publics
            /// <summary>
            /// Perform the save to file action.
            /// </summary>
            protected override void Execute() {
                //Save it to file
                Success = WorldFileIO.SaveWorldToFile(FileName);
            }
            #endregion
        }

        /// <summary>
        /// Task to handle loading in a world from various sources.
        /// </summary>
        private class LoadWorldTask : WorkTask {
            #region Properties
            /// <summary>
            /// The type of task it is.
            /// </summary>
            public override TaskType Type {
                get { return TaskType.LoadWorld; }
            }

            /// <summary>
            /// What kind of world it is.
            /// </summary>
            public WorldType WorldType { get; private set; }

            /// <summary>
            /// The name of the world
            /// </summary>
            public string WorldName { get; private set; }

            /// <summary>
            /// The world content that was loaded in.
            /// </summary>
            public WorldContext WorldContent { get; private set; }
            #endregion

            #region Constructor(s)
            /// <summary>
            /// Create a new load task that pulls in the world of name
            /// from the source type.
            /// </summary>
            public LoadWorldTask(ITaskCreator creator, WorldType type, string name) : base (creator) {
                WorldType = type;
                WorldName = name;
            }
            #endregion

            #region Publics
            /// <summary>
            /// Load in the world from it's source
            /// </summary>
            protected override void Execute() {
                switch (WorldType) {
                    case WorldType.Converted:
                        WorldContent = WorldConverter.ConvertWorld(WorldName);
                        break;

                    case WorldType.Loaded:
                        WorldContent = WorldFileIO.LoadWorldFile(WorldName);
                        break;

                    case WorldType.Empty:
                    case WorldType.Desert:
                    case WorldType.Valley:
                    case WorldType.Forest:
                    case WorldType.City:
                        WorldContent = WorldGenerator.GenerateWorld(WorldType, WorldName);
                        break;
                }
            }
            #endregion
        }

        #region Events
        /// <summary>
        /// Fired when a world has completed saving. Doesn't
        /// matter if it failed.
        /// </summary>
        public static event EventHandler OnWorldSaved;

        /// <summary>
        /// Fired when a world is loaded from some source. The world
        /// contents are included in the args.
        /// </summary>
        public static event EventHandler<WorldArgs> OnWorldLoaded;
        #endregion

        #region Publics
        /// <summary>
        /// Load in a world from several
        /// different sources.
        /// </summary>
        public void Load(WorldType type, string worldName) {
            LoggerUtils.Log("WorldHandler: Loading world: " + worldName);

            //Create a new task and send it to the task scheduler
            LoadWorldTask loadTask = new LoadWorldTask(this, type, worldName);
            TaskScheduler.AddTask(loadTask);
        }

        /// <summary>
        /// Save a world to .bxl file.
        /// </summary>
        public void Save(string fileName) {
            //Create a new task and send it off to the scheduler
            SaveWorldTask saveTask = new SaveWorldTask(this, fileName);
            TaskScheduler.AddTask(saveTask);
        }

        /// <summary>
        /// Clear the contents of the current world.
        /// </summary>
        public void Clear() {
            VoxelatedEngine.Engine.World.ChunkManager.Clear();
        }

        /// <summary>
        /// Called when one of it's tasks is completed.
        /// </summary>
        public void TaskCompleted(WorkTask task) {
            //Save World task completed
            if(task.Type == TaskType.SaveWorld) {
                HandleSaveWorldTask(task);
            }
            //Load world task completed
            else if(task.Type == TaskType.LoadWorld) {
                HandleLoadWorldTask(task);
            }
            //catch all
            else {
                LoggerUtils.LogWarning("WorldHandler: Invalid task of type: " + task.Type.ToString());
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Process the results of a save world task
        /// </summary>
        private void HandleSaveWorldTask(WorkTask task) {
            SaveWorldTask saveTask = task as SaveWorldTask;

            if (saveTask == null) {
                LoggerUtils.LogError("WorldHandler: Invalid save task received");
                return;
            }

            //Log that the world failed to save to file.
            if (!saveTask.Success) {
                LoggerUtils.LogError("WorldHandler: Failed to save world to file.");
            }
            else {
                LoggerUtils.Log("WorldHandler: Saved " + saveTask.FileName);
            }

            //Fire off the event
            OnWorldSaved?.Invoke(this, null);
        }

        /// <summary>
        /// Process a world
        /// </summary>
        private void HandleLoadWorldTask(WorkTask task) {
            LoadWorldTask loadTask = task as LoadWorldTask;

            if(loadTask == null) {
                LoggerUtils.LogError("WorldHandler: Invalid load task recieved");
            }

            if(loadTask.WorldContent == null) {
                LoggerUtils.LogError("WorldHandler: Failed to load world.");
            }
            else {
                LoggerUtils.Log("WorldHandler: Loaded " + loadTask.WorldName);

                //Fire off the event if a world was pulled in.
                OnWorldLoaded?.Invoke(this, new WorldArgs(loadTask.WorldContent));
            }
        }
        #endregion
    }
}
