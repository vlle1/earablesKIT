using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.PushUpActivity;
using EarablesKIT.Models.Extentionmodel.Activities.RunningActivity;
using EarablesKIT.Models.Extentionmodel.Activities.SitUpActivity;
using EarablesKIT.Models.Extentionmodel.Activities.StepActivity;
using EarablesKIT.Models.Library;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Extentionmodel
{
    class ActivityManager : IActivityManager
    {
        private ServiceProvider _activityProvider;
        public List<Activity> activities = new List<Activity>();

        public ServiceProvider ActitvityProvider { get => _activityProvider; set => _activityProvider = value; }

        public void OnIMUDataReceived(object sender, DataEventArgs args)
        {
            throw new NotImplementedException();
        }

        public IServiceCollection ServiceRegistration()
        {
            IServiceCollection collection = new ServiceCollection();
            collection.AddSingleton<AbstractStepActivity, StepActivityThreshold>();
            collection.AddSingleton<AbstractRunningActivity, RunningActivityThreshold>();

            collection.AddSingleton<AbstractPushUpActivity, PushUpActivityThreshold>();
            collection.AddSingleton<AbstractSitUpActivity, SitUpActivityThreshold>();
            
            return collection;
        }

        public ActivityManager()
        {
            IEarablesConnection connection= (IEarablesConnection) ServiceManager.ServiceProvider.GetService(typeof (IEarablesConnection));
        }
    }
}
