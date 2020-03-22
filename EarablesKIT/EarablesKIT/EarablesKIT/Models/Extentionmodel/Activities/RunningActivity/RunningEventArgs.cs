namespace EarablesKIT.Models.Extentionmodel.Activities.RunningActivity
{
    /// <summary>
    /// Arguments of Running Event.
    /// </summary>
    public class RunningEventArgs : ActivityArgs
    {
        /// <summary>
        /// true, iff the user is determined to run.
        /// </summary>
        public bool Running;

        /// <summary>
        /// sets isRunning to the given value.
        /// </summary>
        /// <param name="isRunning"></param>
        public RunningEventArgs(bool isRunning)
        {
            this.Running = isRunning;
        }
    }
}
