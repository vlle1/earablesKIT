using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Plugin.BLE.Abstractions.EventArgs;

namespace EarablesKIT.Models.Library
{
    public interface IEarablesConnection
    {
        bool Connected { get; }
        int SampleRate { get; set; }
        bool IsBluetoothActive { get; }
        LPF_Accelerometer AccLPF { get; set; }
        LPF_Gyroscope GyroLPF { get; set; }
        float BatteryVoltage { get;}

        /*
        event EventHandler<DataEventArgs> IMUDataReceived;
        event EventHandler<ButtonEventArgs> ButtonPressed;
        event EventHandler<DeviceEventArgs> DeviceConnectionStateChanged;
        event EventHandler<NewDeviceFoundArgs> NewDeviceFound;
        */

        void StartScanning();

        void ConnectToDevice(IDevice device);

        void DisconnectFromDevice();

        void StartSampling();
        
        void StopSampling();

    }
}
