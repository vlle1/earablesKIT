using EarablesKIT.Models.Library;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Extentionmodel
{
    class ActivityManager : IActivityManager
    {
        public ServiceProvider ActitvityProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void OnIMUDataReceived(object sender, DataEventArgs args)
        {
            throw new NotImplementedException();
        }

        public ServiceCollection ServiceRegistration()
        {
            throw new NotImplementedException();
        }

        public ActivityManager()
        {
             throw new NotImplementedException();
        }
    }
}
