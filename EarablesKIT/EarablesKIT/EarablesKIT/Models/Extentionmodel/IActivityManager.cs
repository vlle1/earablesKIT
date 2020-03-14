using EarablesKIT.Models.Library;
using System;


namespace EarablesKIT.Models.Extentionmodel
{
    /// <summary>
    /// ActivityManager is a ServiceManager that manages the Activities. It also pushes the sensor
    /// data from the library to the activities.
    /// </summary>
    public interface IActivityManager : IManager
    {
        /// <summary>
        /// This ActivityProvider knows all possible Activities.
        /// </summary>
        IServiceProvider ActitvityProvider { get; set; }

        /// <summary>
        /// This method is a EventMethod that passes data on from the library to the Activities.
        /// </summary>
        /// <param name="sender">The sender of the data</param>
        /// <param name="args">The sensor data</param>
        void OnIMUDataReceived(object sender, DataEventArgs args);
    }
}