using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EarablesKIT.Models.SettingsService
{
    /// <summary>
    /// Interface ISettingsService defines an interface which handles the App settings. The current settings are: Language, samplingrate, user
    /// </summary>
    interface ISettingsService
    {
        /// <summary>
        /// The language property, which is currently active
        /// </summary>
        CultureInfo ActiveLanguage { get; set; }
        /// <summary>
        /// The samplingrate property, which is currently set
        /// </summary>
        SamplingRate SamplingRate { get; set; }
        /// <summary>
        /// The user property, which is currently using the app
        /// </summary>
        User ActiveUser { get; set; }

    }

    /// <summary>
    /// Enum Samplingrate provides different predefined values for the samplingrate of the earables. 
    /// </summary>
    public enum SamplingRate
    {
        Hz_1 = 1,
        Hz_20 = 20,
        Hz_50 = 50,
        Hz_80 = 80,
        Hz_100 = 100

    }
}
