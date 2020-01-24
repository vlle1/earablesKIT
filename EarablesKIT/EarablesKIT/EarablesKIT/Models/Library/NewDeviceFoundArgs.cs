using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    class NewDeviceFoundArgs
    {
        public IDevice Device;

        public NewDeviceFoundArgs(IDevice device)
        {
            this.Device = device;    
        }
    }
}
