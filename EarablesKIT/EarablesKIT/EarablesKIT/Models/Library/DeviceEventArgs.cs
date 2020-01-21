using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    class DeviceEventArgs
    {
        // Brauch man hier getter und setter? Man kann die doch einfach weglassenoder zummindest die setter weil sie nur im Constrictor gesetzt werden
        public bool Connected { get; set; }
        public string DeviceName { get; set; }

        public DeviceEventArgs(bool connected, string deviceName)
        {
            Connected = connected;
            DeviceName = deviceName;
        }
    }
}
