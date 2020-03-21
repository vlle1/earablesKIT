using EarablesKIT.Models.Extentionmodel.Activities.StepActivity;
using EarablesKIT.Models.Library;

namespace EarablesKIT.Models.Extentionmodel.Activities.RunningActivity
{
    /// <summary>
    /// This Activity just checks if a step can be detected to tell the user is walking
    /// The user is considered not running after a timeout.
    /// </summary>
    public class RunningActivityThreshold : AbstractRunningActivity
    {
        //after this time has passed while no step was detected the user is considered standing.
        private const double TIMEOUT_LENGTH = 1.1;
        //this counter counts the seconds that no step is performed while in walking state.
        private double _timeout_counter;
        //this is the StepActivity used for detecting steps.
        private AbstractStepActivity _subDetection;


        public RunningActivityThreshold()
        {

        }
        ///<inheritdoc/>
        override protected void Activate()
        {
            //the subdetection algorithm can only be registered now, because IActivityManager has to already be initialized
            
            if (_subDetection == null)
                _subDetection = (AbstractStepActivity)
                            ((IActivityManager)ServiceManager.ServiceProvider.GetService(typeof(IActivityManager)))
                            .ActitvityProvider.GetService(typeof(AbstractStepActivity));

            base.Activate();
            _runningState = false;
            _subDetection.ActivityDone += OnStepRecognized;
        }
        //is called when a step is recognized and refreshes the timeout
        public void OnStepRecognized(object sender, ActivityArgs e)
        {
            _timeout_counter = TIMEOUT_LENGTH;
            //if not running (walking) so far, now 
            if (!_runningState) ChangeDetected();
        }


        ///<inheritdoc/>
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
                    this.ChangeDetected();
                }
                else _timeout_counter -= 1.0 / _frequency;
            }
        }
    }
}
