using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    class DeviceEventArgs
    {
        // Brauch man hier getter und setter? Man kann die doch einfach weglassenoder zummindest die setter weil sie nur im Constrictor gesetzt werden
        public bool Connected { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string DeviceName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public DeviceEventArgs(bool connected, string deviceName)
        {
            Connected = connected;
            DeviceName = deviceName;
        }
    }
}
