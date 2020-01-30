using EarablesKIT.Models.Library;
using Microsoft.Extensions.DependencyInjection;

namespace EarablesKIT.Models.Extentionmodel
{
    /// <summary>
    /// ActivityManager is a ServiceManager that manages the Activities.
    /// It also pushes the sensor data from the library to all activities in Activities.
    /// </summary>
    interface IActivityManager :  IManager
    {
        /// <summary>
        /// This ActivityProvider knows all possible Activities.
        /// </summary>
        ServiceProvider ActitvityProvider { get; set; }
        /// <summary>
        /// This method is a EventMethod that passes data on from the library to the Activities.
        /// </summary>
        /// <param name="sender">The sender of the data</param>
        /// <param name="args">The sensor data</param>
        void OnIMUDataReceived(object sender, DataEventArgs args);
    }
}
