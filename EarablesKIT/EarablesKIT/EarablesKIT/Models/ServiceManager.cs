using System;
using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Library;
using EarablesKIT.Models.PopUpService;
using EarablesKIT.Models.SettingsService;
using Microsoft.Extensions.DependencyInjection;

namespace EarablesKIT.Models
{
    /// <summary>
    /// Class ServiceManager contains the different Servii and provides them to other components
    /// like the viewmodel
    /// </summary>
    public class ServiceManager : IManager
    {
        private static IServiceProvider _serviceProvider;

        /// <summary>
        /// Property ServiceProvider is a static property which handles the singleton instance and
        /// initializes it.
        /// </summary>
        public static IServiceProvider ServiceProvider => _serviceProvider ?? (_serviceProvider = ServiceRegistration().BuildServiceProvider());

        private static IServiceCollection ServiceRegistration()
        {
            IServiceCollection collection = new ServiceCollection();

            collection.AddSingleton<IEarablesConnection, EarablesConnection>();
            collection.AddSingleton<IDataBaseConnection, DatabaseConnection>();

            collection.AddSingleton<ISettingsService, SettingsService.SettingsService>();
            collection.AddSingleton<IActivityManager, ActivityManager>();
			collection.AddSingleton<IPopUpService, PopUpService.PopUpService>();

            return collection;
        }
    }
}