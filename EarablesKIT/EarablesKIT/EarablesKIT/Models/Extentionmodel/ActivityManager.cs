using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.PushUpActivity;
using EarablesKIT.Models.Extentionmodel.Activities.RunningActivity;
using EarablesKIT.Models.Extentionmodel.Activities.SitUpActivity;
using EarablesKIT.Models.Extentionmodel.Activities.StepActivity;
using EarablesKIT.Models.Library;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace EarablesKIT.Models.Extentionmodel
{
    /// <inheritdoc/>
    internal class ActivityManager : IActivityManager
    {
        private ServiceProvider _activityProvider;

        /// <summary>
        /// This list contains all Activities that should recieve new Data, e.g. all activities.
        /// </summary>
        public List<Activity> Activities = new List<Activity>();

        /// <summary>
        /// The activityProvider is the ServiceProvider for the Activities.
        /// </summary>
        public ServiceProvider ActitvityProvider
        {
            get => _activityProvider;
            set => _activityProvider = value;
        }

        /// <inheritdoc/>
        public void OnIMUDataReceived(object sender, DataEventArgs args)
        {
            foreach (Activity activity in Activities)
            {
                activity.DataUpdate(args);
            }
        }

        /// <summary>
        /// Constructor for class Activitymanager. Registers all EventHandlers.
        /// </summary>
        public ActivityManager()
        {
            _activityProvider = ServiceRegistration().BuildServiceProvider();

            IEarablesConnection connection = (IEarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection));
            //register at Library
            connection.IMUDataReceived += OnIMUDataReceived;
            Activities.Add(((Activity)_activityProvider.GetService(typeof(AbstractStepActivity))));
            Activities.Add(((Activity)_activityProvider.GetService(typeof(AbstractSitUpActivity))));
            Activities.Add(((Activity)_activityProvider.GetService(typeof(AbstractPushUpActivity))));
            Activities.Add(((Activity)_activityProvider.GetService(typeof(AbstractRunningActivity))));
        }

        /// <summary>
        /// Method initializing the serviceCollection elements used in constructor.
        /// </summary>
        /// <returns>all services that should be registered</returns>
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