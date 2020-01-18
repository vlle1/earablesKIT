using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Extentionmodel.Activities.RunningActivity
{
    /// <summary>
    /// This activity detects changes of the running state of the user.
    /// </summary>
    abstract class AbstractRunningActivity : Activity
    {
        /// <summary>
        /// the runningstate is false iff the user is not running.
        /// </summary>
        protected bool _runningState = false;
    }
}
