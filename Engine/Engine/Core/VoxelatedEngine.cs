using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Voxelated.Network;
using Voxelated.Engine.Mesh.Render;
using Voxelated.Terrain;
using Voxelated.Utilities;
using Voxelated.Engine.Tasks;
using Voxelated.Engine.Mesh;
using Voxelated.Menus;
using Voxelated.Engine.Console;
using Voxelated.Engine;
using Voxelated.Network.Time;

namespace Voxelated {
    /// <summary>
    /// Core of the Voxelated Engine. Create a new instance of this to control
    /// everything voxel specific. This does nearly everything but doesn't
    /// handle actually rendering.
    /// </summary>
    public abstract class VoxelatedEngine {
        #region Components
        /// <summary>
        /// The static and dynamic voxel
        /// terrain of the world.
        /// </summary>
        public World World { get; protected set; }

        /// <summary>
        /// The task management system for the engine.
        /// </summary>
        public TaskScheduler TaskScheduler { get; protected set; }

        /// <summary>
        /// The command console manager. Controls parsing
        /// and issue commands.
        /// </summary>
        public CommandConsole Console { get; protected set; }

        /// <summary>
        /// Handles network interaction.
        /// </summary>
        public abstract NetManager NetManager { get; protected set; }

        /// <summary>
        /// The time of the engine.
        /// </summary>
        public NetTime Time { get; protected set; }
        #endregion

        #region Properties
        /// <summary>
        /// If the engine is running or not.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// If rendering is currently enabled for this instance
        /// of the engine.
        /// </summary>
        public abstract bool IsRenderingEnabled { get; }

        /// <summary>
        /// Timing, and work settings of the engine.
        /// </summary>
        public abstract VoxelatedSettings Settings { get; }
        #endregion

        #region Members
        /// <summary>
        /// The singleton instance reference to the engine.
        /// </summary>
        public static VoxelatedEngine Engine;
        /// <summary>
        /// How much time should (idealy) pass between ticks.
        /// </summary>
        private TimeSpan TargetElapsedTime;

        /// <summary>
        /// Max amount of time we want to stimulate if a lag spike occcurs
        /// between ticks.
        /// </summary>
        private TimeSpan MaxElapsedTime;

        /// <summary>
        /// Tracks how much time has passed.
        /// </summary>
        private Stopwatch stopwatch;

        /// <summary>
        /// When the last tick occured.
        /// </summary>
        private TimeSpan lastTickTime;

        /// <summary>
        /// How much time has passed.
        /// </summary>
        private TimeSpan accumulatedTime;
        #endregion

        #region Events
        /// <summary>
        /// When the application is first started.
        /// </summary>
        public event EventHandler OnInitialize;

        /// <summary>
        /// Fired when the engine is just being started
        /// to run.
        /// </summary>
        public event EventHandler OnStart;

        /// <summary>
        /// When the application is shut down
        /// </summary>
        public event EventHandler OnStop;

        /// <summary>
        /// When the application is closed down.
        /// </summary>
        public event EventHandler OnExit;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new instance of the voxelated
        /// engine. This is a headless server instance.
        /// </summary>
        protected VoxelatedEngine() {
            Engine = this;

            //Set up components
            World = new World();
            Console = new CommandConsole();
            Time = new NetTime();

            //Set up the game loop
            Initialize();
        }
        #endregion

        #region Life Cycle Events
        /// <summary>
        /// Sets up the gameloop and
        /// prepares it to run.
        /// </summary>
        public void Initialize() {
            stopwatch = Stopwatch.StartNew();
            TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / Settings.UpdateTicksPerSecond);
            MaxElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 10);

            //Fire off the event.
            OnInitialize?.Invoke(this, null);
        }

        /// <summary>
        /// Starts up the game engine.
        /// </summary>
        public void Start() {
            if (IsRunning) {
                return;
            }

            LoggerUtils.Log("VoxelatedEngine: Starting...");
            IsRunning = true;

            TaskScheduler.Start();

            ThreadPool.QueueUserWorkItem(delegate { MainLoop(); });

            //Fire off the event.
            OnStart?.Invoke(this, null);
        }

        /// <summary>
        /// Runs an update on the engine. 
        /// </summary>
        /// <param name="deltaTime">Time since the last time.</param>
        /// <param name="netTick">If network message should be sent
        /// out on this update.</param>
        private void Update(float deltaTime, bool netTick) {
            //Update net time
            Time.Update(deltaTime);

            //See if anything came in from the network
            if (NetManager != null) {
                NetManager.CheckForMessages();
            }

            //Update the game state
            World.Update();

            if (NetManager != null) {
                NetManager.SendOutMessages();
            }
        }

        /// <summary>
        /// Stops the game engine.
        /// </summary>
        public void Stop() {
            if (!IsRunning) {
                return;
            }

            TaskScheduler.Stop();

            LoggerUtils.Log("VoxelatedEngine: Stopping...");
            IsRunning = false;

            //Fire off the event.
            OnStop?.Invoke(this, null);
        }

        /// <summary>
        /// Start running the engine. This runs on a background thread
        /// to prevent lag. Runs a fixed time step update
        /// </summary>
        private void MainLoop() {
            IsRunning = true;
            int tickCount = 1;
            int netTickEveryNTicks = Settings.UpdateTicksPerSecond / Settings.NetworkTicksPerSecond;
            bool netTick;

            while (IsRunning) {
                TimeSpan currTickTime = stopwatch.Elapsed;
                TimeSpan elapsedTime = currTickTime - lastTickTime;
                lastTickTime = currTickTime;

                //Prevent massive time step when resuming from pause
                accumulatedTime += elapsedTime > MaxElapsedTime ? MaxElapsedTime : elapsedTime;

                //If still waiting, sleep a bit.
                if(accumulatedTime < TargetElapsedTime) {
                    Thread.Sleep(TargetElapsedTime - accumulatedTime);
                }

                //When the time limit has been reached. Fire update
                while (accumulatedTime >= TargetElapsedTime) {
                    netTick = tickCount == netTickEveryNTicks;
                    tickCount = netTick ? 1 : tickCount + 1;

                    float deltaTime = (float)0.01 * accumulatedTime.Milliseconds;

                    Update(deltaTime, netTick);
                    accumulatedTime -= TargetElapsedTime;
                }
            }
        }
        #endregion
    }
}
