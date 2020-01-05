using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EarablesKIT.Models.SettingsService
{
    interface ISettingsService
    {
        CultureInfo ActiveLanguage { get; set; }
        SamplingRate SamplingRate { get; set; }
        User MyUser { get; set; }

    }

    public enum SamplingRate
    {
        Hz_1,
        Hz_20,
        Hz_50,
        Hz_80,
        Hz_100

    }
}
