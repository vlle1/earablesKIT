using EarablesKIT.Models.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Extentionmodel
{
    interface IActivityManager : IManager
    {
        IServiceProvider ActitvityProvider { get; set; }

        void OnIMUDataReceived(object sender, DataEventArgs args);
    }
}
