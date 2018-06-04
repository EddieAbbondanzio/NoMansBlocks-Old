using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated {
    /// <summary>
    /// Count down timer that starts at some
    /// fixed amount and counts down until it reaches 0.
    /// </summary>
    public interface ITimer {
        #region Events
        /// <summary>
        /// The event that goes off when the timer's 
        /// remaining time is 0.
        /// </summary>
        event EventHandler OnTimerFinished;
        #endregion

        #region Properties
        /// <summary>
        /// If the timer is currently running.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// The time that the timer began on.
        /// </summary>
        double StartTime { get; }

        /// <summary>
        /// How long the timer is running for.
        /// </summary>
        double Duration { get; }

        /// <summary>
        /// How much time is left until the timer goes off.
        /// </summary>
        double TimeRemaining { get; }
        #endregion

        #region Publics
        /// <summary>
        /// End the timer early, and don't set off the
        /// on timer finished event.
        /// </summary>
        void Stop();
        #endregion
    }
}
