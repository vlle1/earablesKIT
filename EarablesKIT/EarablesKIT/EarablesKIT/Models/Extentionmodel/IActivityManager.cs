using EarablesKIT.Models.Library;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace EarablesKIT.Models.Extentionmodel
{
    interface IActivityManager :  IManager
    {
        ServiceProvider ActitvityProvider { get; set; }

        void OnIMUDataReceived(object sender, DataEventArgs args);
    }
}
