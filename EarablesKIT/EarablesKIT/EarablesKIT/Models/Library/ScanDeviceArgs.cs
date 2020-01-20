using System;
using System.Collections.Generic;
using System.Text;
using Plugin.BLE.Abstractions.Contracts;

namespace EarablesKIT.Models.Library
{
    class ScanDeviceArgs
    {
        public List<IDevice> Devices { get; }

        public ScanDeviceArgs(List<IDevice> devices)
        {
            this.Devices = devices;
        }
    }
}
