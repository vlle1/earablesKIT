using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ViewModelTests.ViewModels.ScanningPopUp
{
    internal class DeviceMock : IDevice
    {
        public DeviceMock(string name)
        {
            this.Name = name;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<IService>> GetServicesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<IService> GetServiceAsync(Guid id, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateRssiAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> RequestMtuAsync(int requestValue)
        {
            throw new NotImplementedException();
        }

        public bool UpdateConnectionInterval(ConnectionInterval interval)
        {
            throw new NotImplementedException();
        }

        public Guid Id { get; }
        public string Name { get; private set; }
        public int Rssi { get; }
        public object NativeDevice { get; }
        public DeviceState State { get; }
        public IList<AdvertisementRecord> AdvertisementRecords { get; }
    }
}