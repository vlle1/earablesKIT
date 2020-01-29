using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Extentionmodel.Activities.RunningActivity
{
    /// <summary>
    /// This activity detects changes of the running state of the user.
    /// </summary>
    public abstract class AbstractRunningActivity : Activity
    {
        /// <summary>
        /// the runningstate is false iff the user is not running.
        /// </summary>
        protected bool _runningState = false;
        /// <summary>
        /// this method is called when a change of the running state was detected. It automatically applies the change to _runningState and notifies the triggers the Event.
        /// </summary>
        protected void changeDetected()
        {
            _runningState = !_runningState;
            ActivityDone.Invoke(this, new RunningEventArgs(_runningState));
        }

    }
}
