using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Plugin.BLE.Abstractions.EventArgs;

namespace EarablesKIT.Models.Library
{
    interface IEarablesConnection
    {
        EventHandler<DataEventArgs> IMUDataReceived { get; set; }

        EventHandler<ButtonEventArgs> ButtonPressed { get; set; }

        EventHandler<DeviceEventArgs> DeviceConnectionStateChanged { get; set; }

        
        List<IDevice> StartScanning();


        bool ConnectToDevice(IDevice device);

        void DisconnectFromDevice();

        void StartSampling();
        
        void StopSampling();

        void SetSamplingRate(int rate);

        //TODO weitere Getter Setter für die LowPassFilter hinzufügern ODER als Property (ist besser)



        bool IsBluetoothActive();

        bool IsConnected();


        //TODO wahrscheinlich hier die Args ändern, je nach dem, was die BLE Lib wirft
        void OnValueUpdatedIMU(object sender, CharacteristicUpdatedEventArgs args);

        void OnPushButtonPressed(object sender, EventArgs args);

        void OnDeviceConnected(object sender, EventArgs args);
    }
}
