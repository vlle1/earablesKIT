namespace EarablesKIT.Models.Extentionmodel.Activities.RunningActivity
{
    class RunningEventArgs : ActivityArgs
    {
        public bool Running;
        public RunningEventArgs(bool isRunning)
        {
            this.Running = isRunning;
        }
    }
}
