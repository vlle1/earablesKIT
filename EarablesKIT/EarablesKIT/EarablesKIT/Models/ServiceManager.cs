using System;
using System.Collections.Generic;
using System.Text;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities.PushUpActivity;
using EarablesKIT.Models.Extentionmodel.Activities.RunningActivity;
using EarablesKIT.Models.Extentionmodel.Activities.SitUpActivity;
using EarablesKIT.Models.Extentionmodel.Activities.StepActivity;
using Microsoft.Extensions.DependencyInjection;

namespace EarablesKIT.Models
{
    class ServiceManager : IManager
    {
        private static IServiceProvider _serviceProvider;

        public static IServiceProvider ServiceProvider => _serviceProvider ?? (_serviceProvider = ServiceRegistration().BuildServiceProvider());

        private static IServiceCollection ServiceRegistration()
        {
            IServiceCollection collection = new ServiceCollection();

            collection.AddSingleton<AbstractStepActivity, StepActivityThreshold>();
            collection.AddSingleton<AbstractRunningActivity, RunningActivityThreshold>();

            collection.AddSingleton<AbstractPushUpActivity, PushUpActivityThreshold>();
            collection.AddSingleton<AbstractSitUpActivity, SitUpActivityThreshold>();

            return collection;
        }
    }
}
