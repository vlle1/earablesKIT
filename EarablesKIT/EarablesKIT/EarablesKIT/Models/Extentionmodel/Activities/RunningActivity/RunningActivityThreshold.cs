using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities.StepActivity;
using EarablesKIT.Models.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Extentionmodel.Activities.RunningActivity
{
    /// <summary>
    /// This Activity just checks if a step can be detected to tell the user is walking
    /// The user is considered not running after a timeout
    /// </summary>
    public class RunningActivityThreshold : AbstractRunningActivity
    {
        //after this time has passed while no step was detected the user is considered standing.
        private const double TIMEOUT_LENGTH = 1.1;

        private double _timeout_counter;
        
        private AbstractStepActivity _subDetection = 
            ((AbstractStepActivity)
            ((ActivityManager) ServiceManager.ServiceProvider.GetService(typeof(IActivityManager)))
                .ActitvityProvider.GetService(typeof(AbstractStepActivity)));
        

        public RunningActivityThreshold()
        {

        }

        override protected void Activate()
        {
            base.Activate();
            _runningState = false;
            _subDetection.ActivityDone += OnStepRecognized;
        }
        //is called when a step is recognized and refreshes the timeout
        private void OnStepRecognized(object sender, ActivityArgs e)
        {
            _timeout_counter = TIMEOUT_LENGTH;
            //if not running (walking) so far, now 
            if (!_runningState) changeDetected();
        }

        /// <summary>
        /// See class description.
        /// </summary>
        protected override void Analyse(DataEventArgs data)
        {
            if (ActivityDone == null)
            {
                //unregister from stepActivity
                _subDetection.ActivityDone -= OnStepRecognized;
                return;
            }
            //if user is running we need to find out if he times out
            if (_runningState)
            {
                if (_timeout_counter <= 0)
                {
                    //no longer running.
                    this.changeDetected();
                }
                else _timeout_counter -= 1.0 / _frequency;
            }  
        }
    }
}
