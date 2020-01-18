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
        public List<Activity> Activities = new List<Activity>();

        public ServiceProvider ActitvityProvider {
            get => _activityProvider; 
            set => _activityProvider = value; }

        public void OnIMUDataReceived(object sender, DataEventArgs args)
        {
            foreach (Activity activity in Activities) activity.DataUpdate(args);
        }

        public ActivityManager()
        {

           
            _activityProvider = ServiceRegistration().BuildServiceProvider();
          
            IEarablesConnection connection = (IEarablesConnection) ServiceManager.ServiceProvider.GetService(typeof (IEarablesConnection));
            //register at Library
            connection.IMUDataReceived += OnIMUDataReceived;
        }

        private IServiceCollection ServiceRegistration()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<AbstractStepActivity, StepActivityThreshold>();
            serviceCollection.AddSingleton<AbstractRunningActivity, RunningActivityThreshold>();
            serviceCollection.AddSingleton<AbstractPushUpActivity, PushUpActivityThreshold>();
            serviceCollection.AddSingleton<AbstractSitUpActivity, SitUpActivityThreshold>();
            return serviceCollection;
        }
    }
}
