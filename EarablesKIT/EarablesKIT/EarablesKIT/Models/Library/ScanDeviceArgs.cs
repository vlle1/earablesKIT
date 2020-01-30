using Plugin.BLE.Abstractions.Contracts;

namespace EarablesKIT.Models.Library
{
    class ScanDeviceArgs
    {
        public IDevice Device { get; }

        public ScanDeviceArgs(IDevice device)
        {
            this.Device = device;
        }
    }
}
