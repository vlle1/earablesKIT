using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    /// <summary>
    /// This class contains all arguments which are necessary if a new device is found
    /// </summary>
    public class NewDeviceFoundArgs
    {
        public IDevice Device;

        public NewDeviceFoundArgs(IDevice device)
        {
            this.Device = device;    
        }
    }
}
