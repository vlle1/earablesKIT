using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    /// <summary>
    /// This class contains all arguments which are necessary if the connection state changes
    /// </summary>
    public class DeviceEventArgs
    {
        private bool connected;
        public bool Connected  { get => connected; set => connected = value; }
        private string deviceName;
        public string DeviceName { get => deviceName; set => deviceName = value; }

        public DeviceEventArgs(bool connected, string deviceName)
        {
            Connected = connected;
            DeviceName = deviceName;
        }
    }
}
