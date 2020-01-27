using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Plugin.BLE.Abstractions.EventArgs;

namespace EarablesKIT.Models.Library
{
    /// <summary>
    /// Offers the interfaces to comunicate with the earables
    /// </summary>
    public interface IEarablesConnection
    {
        // Returns the connection status
        bool Connected { get; }
        // Set/Get the samplerate of the earables
        int SampleRate { get; set; }
        // Returns if the Bluetooth on your device is on
        bool IsBluetoothActive { get; }
        // Set/Get the LPF for the accelerometer
        LPF_Accelerometer AccLPF { get; set; }
        // SetGet the LPF for the gyroscope
        LPF_Gyroscope GyroLPF { get; set; }
        // Returns the batteryvoltage
        float BatteryVoltage { get;}

        
        event EventHandler<DataEventArgs> IMUDataReceived;
        event EventHandler<ButtonEventArgs> ButtonPressed;
        event EventHandler<DeviceEventArgs> DeviceConnectionStateChanged;
        event EventHandler<NewDeviceFoundArgs> NewDeviceFound;
        
        /// <summary>
        /// Starts the Scanning
        /// </summary>
        void StartScanning();

        /// <summary>
        ///  Connect to a device
        /// </summary>
        /// <param name="device"> Is the device which should connect</param>
        void ConnectToDevice(IDevice device);

        /// <summary>
        /// Disconnects from the device
        /// </summary>
        void DisconnectFromDevice();

        /// <summary>
        /// Starts the sampling
        /// </summary>
        void StartSampling();
        
        /// <summary>
        /// Stops the sampling
        /// </summary>
        void StopSampling();

    }
}
