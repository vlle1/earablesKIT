using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Library;
using EarablesKIT.Models.SettingsService;
using Microsoft.Extensions.DependencyInjection;

namespace EarablesKIT.Models
{
    /// <summary>
    /// Class ServiceManager contains the different Servii and provides them to other components like the viewmodel
    /// </summary>
    class ServiceManager : IManager
    {
        private static ServiceProvider _serviceProvider;

        /// <summary>
        /// Property ServiceProvider is a static property which handles the singleton instance and initializes it.
        /// </summary>
        public static ServiceProvider ServiceProvider => _serviceProvider ?? (_serviceProvider = ServiceRegistration().BuildServiceProvider());

        private static IServiceCollection ServiceRegistration()
        {
            IServiceCollection collection = new ServiceCollection();

            collection.AddSingleton<IEarablesConnection, EarablesConnection>();
            collection.AddSingleton<IDataBaseConnection, DatabaseConnection>();

            collection.AddSingleton<ISettingsService, SettingsService.SettingsService>();
            collection.AddSingleton<IActivityManager, ActivityManager>();

            return collection;
        }
    }

}
