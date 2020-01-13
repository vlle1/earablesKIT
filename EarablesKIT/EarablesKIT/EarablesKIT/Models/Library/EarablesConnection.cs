using System;
using System.Collections.Generic;
using static EarablesKIT.Models.Library.Constants;
using static EarablesKIT.Models.Library.IMUDataExtractor;
using System.Text;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE;
using Xamarin.Forms;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Exceptions;
using Plugin.BLE.Abstractions.EventArgs;

namespace EarablesKIT.Models.Library
{
    class EarablesConnection : IEarablesConnection
    {
        private IBluetoothLE ble = CrossBluetoothLE.Current;
        private IAdapter adapter = CrossBluetoothLE.Current.Adapter;
        private IDevice device;
        List<IDevice> deviceList;
        private ConfigContainer config;
        private Characteristics characters;

        public LPF_Accelerometer AccLPF { get => config.AccelerometerLPF; set => SetAccelerometerLPF(value); }
        public LPF_Gyroscope GyroLPF;
        public EventHandler<DataEventArgs> IMUDataReceived { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public EventHandler<ButtonEventArgs> ButtonPressed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public EventHandler<DeviceEventArgs> DeviceConnectionStateChanged { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool ConnectToDevice(IDevice device)
        {

            Device.BeginInvokeOnMainThread(new Action(async () =>
            {
                //set the requiered Connection Parameters
                var connectParams = new ConnectParameters(true, true);
                try
                {
                    //stop scanning for devices to be sure that nothing goes wrong
                    await adapter.StopScanningForDevicesAsync();
                    //connect to the device
                    await adapter.ConnectToDeviceAsync(device, connectParams);


                    //load all required characteristics
                    IService Service;
                    Service = await device.GetServiceAsync(Guid.Parse(ACCES_SERVICE));
                    characters.StartStopIMUSamplingChar = await Service.GetCharacteristicAsync(Guid.Parse(START_STOP_IMU_SAMPLING_CHAR));
                    characters.SensordataChar = await Service.GetCharacteristicAsync(Guid.Parse(SENSORDATA_CHAR));
                    characters.PushbuttonChar = await Service.GetCharacteristicAsync(Guid.Parse(PUSHBUTTON_CHAR));
                    characters.BatteryChar = await Service.GetCharacteristicAsync(Guid.Parse(BATTERY_CHAR));
                    //characters.IMUScaleRangeChar = await Service.GetCharacteristicAsync(Guid.Parse(IMU_FULL_SCALE_RANGE_CHAR));
                    characters.AccelerometerGyroscopeLPFChar = await Service.GetCharacteristicAsync(Guid.Parse(ACC_GYRO_LPF_CHAR));

                    //register on the events from the Earables
                    characters.SensordataChar.ValueUpdated += OnValueUpdatedIMU;
                    

                    
                }
                catch (DeviceConnectionException ex)
                {
                    //await DisplayAlert("Notice", "BLE connect DCE ex msg: " + ex.Message, "OK");
                    //await DisplayAlert("Name", "e.Device.Name = " + device.Name, "OK");

                }
                catch (Exception ex)
                {
                    //await DisplayAlert("Notice", "BLE connect basic ex msg: " + ex.Message, "OK");
                }
                
            }));
            return true;
        }
    

        public async void DisconnectFromDevice()
        {
            await adapter.DisconnectDeviceAsync(device);
        }

        public bool IsBluetoothActive()
        {
            if(ble.State == BluetoothState.On)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public void OnDeviceConnected(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }

        public void OnPushButtonPressed(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }

        public void OnValueUpdatedIMU(object sender, CharacteristicUpdatedEventArgs args)
        {
            byte[] bytes = args.Characteristic.Value;
            //int accScaleFactor = ExtractIMUScaleFactorAccelerometer(); //bytes als parameter übergeben
            //int gyroScaleFactor = ExtractIMUScaleFactorGyroscope(); //bytes als parameter übergeben
            //IMUDataEntry imuDataEntry = ExtractIMUDataString(bytes, accScaleFactor, gyroScaleFactor);
            //IMUDataReceived?.Invoke(this, argumente);
        }

        public void SetSamplingRate(int rate)
        {
            config.Samplerate = rate;
        }

        public async void StartSampling()
        {
            await characters.SensordataChar.StartUpdatesAsync();
            byte[] bytes = { 0x53, Convert.ToByte(config.Samplerate + 3), 0x02, 0x01, Convert.ToByte(config.Samplerate) };
            await characters.StartStopIMUSamplingChar.WriteAsync(bytes);
        }

        public List<IDevice> StartScanning()
        {
            FillDeviceList();
            return deviceList;
            
        }

        private async void FillDeviceList()
        {
            try
            {
                deviceList.Clear();
                adapter.DeviceDiscovered += (s, a) =>
                {
                    deviceList.Add(a.Device);
                };

                if (!ble.Adapter.IsScanning)
                {
                    await adapter.StartScanningForDevicesAsync();

                }
            }
            catch (Exception ex)
            {
                //Noch überlegen was im Fehlerfall passiert
            }
        }

        public async void StopSampling()
        {
            await characters.SensordataChar.StopUpdatesAsync();
            byte[] bytes = { 0x53, 0x02, 0x02, 0x00, 0x00 };
            await characters.StartStopIMUSamplingChar.WriteAsync(bytes);
        }

        private async void SetAccelerometerLPF(LPF_Accelerometer accelerometerLPF)
        {
            // 26 because it is the sum of the checksum except the byte Data 3
            int checksum = 26 + (int)accelerometerLPF;
            byte[] bytes = { 0x59, Convert.ToByte(checksum), 0x04, 0x06, 0x08, 0x08, Convert.ToByte((int)accelerometerLPF) };
            await characters.AccelerometerGyroscopeLPFChar.WriteAsync(bytes);
            // safe the AccelerometerLPF in the settings
            //config.AccelerometerLPF = accelerometerLPF; lieber accLPF holen wenn man es braucht anstatt redudant zu speichern
        }

        private async void GetAccelerometerFromDevice()
        {
            byte[] bytes = await characters.AccelerometerGyroscopeLPFChar.ReadAsync();
            //b ist der index des enums AccLPF
            int b = (int)(bytes[6] & 0x0F);
        }
    }
}
