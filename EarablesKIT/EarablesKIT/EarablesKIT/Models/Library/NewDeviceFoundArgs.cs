using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    class NewDeviceFoundArgs
    {
        public IDevice device;

        public NewDeviceFoundArgs(IDevice device)
        {
            this.device = device;    
        }
    }
}
