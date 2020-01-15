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
        private List<IDevice> deviceList;
        private ConfigContainer config;
        private Characteristics characters;
        private int accEnumValue;
        private int gyroEnumValue;
        private bool connected;
        public bool Connected { get => connected; }
        public LPF_Accelerometer AccLPF { get => GetAccelerometerLPF(); set => SetAccelerometerLPF(value); }
        public LPF_Gyroscope GyroLPF { get => GetGyroscopeLPF(); set => SetGyroscopeLPF(value); }
        private float batteryVoltage;
        public float BatteryVoltage { get => batteryVoltage; }

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

                    // Initialise the BatteryVoltage the first time after connection in case it will be used befor the Batteryvalue updates the first time
                    initBatteryVoltage();

                    //register on the events from the Earables (sollte am besten in eine art Constructor)
                    characters.SensordataChar.ValueUpdated += OnValueUpdatedIMU;
                    characters.PushbuttonChar.ValueUpdated += OnPushButtonPressed;
                    await characters.PushbuttonChar.StartUpdatesAsync();
                    characters.BatteryChar.ValueUpdated += GetBatteryVoltageFromDevice;
                    await characters.BatteryChar.StartUpdatesAsync();
                    adapter.DeviceConnected += OnDeviceConnected;
                    adapter.DeviceDisconnected += OnDeviceDisconnected;
                    adapter.DeviceConnectionLost += OnDeviceConnectionLost;
                    



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



        // Kann das private sein so we die anderen zwei und eine Methode für die zwei zeilen unter connected schreiben???
        public void OnDeviceConnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs args)
        {
            Device.BeginInvokeOnMainThread(new Action(() =>
            {
                connected = true;
                DeviceEventArgs e = new DeviceEventArgs(connected, args.Device.Name);
                DeviceConnectionStateChanged?.Invoke(this, e);
            }));
        }
        private void OnDeviceDisconnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs args)
        {
            Device.BeginInvokeOnMainThread(new Action(() =>
            {
                connected = false;
                DeviceEventArgs e = new DeviceEventArgs(connected, args.Device.Name);
                DeviceConnectionStateChanged?.Invoke(this, e);
            }));
        }

        // Brauchen wir die methode wenn sie keinen unterschied macht zu der oben drüber? man kann ja was ergänzen um zu signalisieren, dass die verbindung abgebrochen wurde
        private void OnDeviceConnectionLost(object sender, DeviceErrorEventArgs args)
        {
            Device.BeginInvokeOnMainThread(new Action(() =>
            {
                connected = false;
                DeviceConnectionStateChanged?.Invoke(this, new DeviceEventArgs(connected, args.Device.Name));
            }));
        }

        public void OnPushButtonPressed(object sender, CharacteristicUpdatedEventArgs args)
        {
            byte[] bytes = args.Characteristic.Value;
            // Get the LSB from Data0
            int bit = bytes[3] & 0x01;
            if (bit == 1)
            {
                ButtonPressed?.Invoke(this, new ButtonEventArgs());
            }
        }

        public async void OnValueUpdatedIMU(object sender, CharacteristicUpdatedEventArgs args)
        {
            byte[] bytes1 = args.Characteristic.Value;
            byte[] bytes2 = await characters.AccelerometerGyroscopeLPFChar.ReadAsync();
            double gyroScaleFactor = IMUDataExtractor.ExctractIMUScaleFactorGyroscope(bytes2);
            int accScaleFactor = IMUDataExtractor.ExtractIMUScaleFactorAccelerometer(bytes2);
            IMUDataEntry imuDataEntry = ExtractIMUDataString(bytes1, accScaleFactor, gyroScaleFactor);
            IMUDataReceived?.Invoke(this, new DataEventArgs(imuDataEntry, config));
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
            // Read the characteristic to calculate the checksum and Data3
            byte[] bytesRead = await characters.AccelerometerGyroscopeLPFChar.ReadAsync();
            // clear the 4 LSBs from data3
            int data3 = bytesRead[6] & 0xF0;
            // set the 4 LSBs from data3 on the required value
            data3 = data3 | (int)accelerometerLPF;
            // calculate checksum
            int checksum = bytesRead[2] + bytesRead[3] + bytesRead[4] + bytesRead[5] + (int)data3;
            // Write the new accelerometerLPF in to the characteristic
            byte[] bytesWrite = { 0x59, Convert.ToByte(checksum), bytesRead[2], bytesRead[3], bytesRead[4], bytesRead[5], Convert.ToByte(data3) };
            await characters.AccelerometerGyroscopeLPFChar.WriteAsync(bytesWrite);
        }

        private LPF_Accelerometer GetAccelerometerLPF()
        {
            GetAccelerometerLPFFromDevice();
            LPF_Accelerometer acc = (LPF_Accelerometer)Enum.ToObject(typeof(LPF_Accelerometer), accEnumValue);
            return acc;
        }

        private async void GetAccelerometerLPFFromDevice()
        {
            byte[] bytes = await characters.AccelerometerGyroscopeLPFChar.ReadAsync();
            // read only the 4 LSBs 
            accEnumValue = (int)(bytes[6] & 0x0F);
        }

        private async void SetGyroscopeLPF(LPF_Gyroscope gyroscopeLPF)
        {
            // The Bytes that needed to be modified
            int data0 = 0;
            int data1 = 0;
            // Read the characteristic to calculate the checksum, Data0 and Data1
            byte[] bytesRead = await characters.AccelerometerGyroscopeLPFChar.ReadAsync();
            // clear the 2 LSBs from Data1
            data1 = bytesRead[4] & 0xFC;
            // If the LPF for the Gyroscope is anithing except OFF both Bytes need to be modified
            if ((int)gyroscopeLPF < 8)
            {
                // clear the 3 LSBs
                data0 = bytesRead[3] & 0xF8;
                // set the 3 LSBs on the required value
                data0 = data0 | (int)gyroscopeLPF;
            }
            //If the LPF for the Gyroscope is OFF only Data1 need to be modified
            else
            {
                // set the 2 LSBs on 1
                data1 = data1 | 0x01;
            }
            // Calculate the checksum
            int checksum = bytesRead[2] + (int)data0 + (int)data1 + bytesRead[5] + bytesRead[6];
            // Write the new GyroscopeLPF in to the characteristic
            byte[] bytesWrite = { 0x59, Convert.ToByte(checksum), bytesRead[2], Convert.ToByte(data0), Convert.ToByte(data1), bytesRead[5], bytesRead[6] };
            await characters.AccelerometerGyroscopeLPFChar.WriteAsync(bytesWrite);
        }

        private LPF_Gyroscope GetGyroscopeLPF()
        {
            GetGyroscopeLPFFromDevice();
            LPF_Gyroscope gyro = (LPF_Gyroscope)Enum.ToObject(typeof(LPF_Gyroscope), gyroEnumValue);
            return gyro;
        }

        private async void GetGyroscopeLPFFromDevice()
        {
            byte[] bytes = await characters.AccelerometerGyroscopeLPFChar.ReadAsync();
            // read only the 2 LSBs and checks if the Gyro LPF is bypassed
            int b = (int)(bytes[4] & 0x03);
            // If the Gyro LPF is bypassed it is representet as OFF in the LPF_Gyroscope Enum
            if (b == 0)
            {
                gyroEnumValue = (int)(bytes[3] & 0x07);
            }
            else
            {
                gyroEnumValue = 8;
            }
        }

        private async void initBatteryVoltage()
        {
            byte[] bytes = await characters.BatteryChar.ReadAsync();
            batteryVoltage = (bytes[3] * 256 + bytes[4]) / 1000f;
        }

        private void GetBatteryVoltageFromDevice(object sender, CharacteristicUpdatedEventArgs args)
        {
            byte[] bytes = args.Characteristic.Value;
            batteryVoltage = (bytes[3] * 256 + bytes[4]) / 1000f;
        }

        private async void setGyroscopeRange(object sender, EventArgs e)
        {
            // Range kann sein 0x00 = 250deg/s, 0x08 = 500deg/s, 0x10 = 1000deg/s, 0x18 = 2000deg/s
            int range = 0x18;

            byte[] bytesRead = await characters.AccelerometerGyroscopeLPFChar.ReadAsync();
            //clear Bit 4 and 3 from Data1
            int data1 = bytesRead[4] & 0xE7;
            // Set Data1 to the right value
            data1 = data1 | range;
            // Calculate chechsum
            int checksum = bytesRead[2] + bytesRead[3] + data1 + bytesRead[5] + bytesRead[6];
            // Write the new Gyroscoperange on the Earables
            byte[] bytesWrite = { 0x59, Convert.ToByte(checksum), bytesRead[2], bytesRead[3], Convert.ToByte(data1), bytesRead[5], bytesRead[6] };
            await characters.AccelerometerGyroscopeLPFChar.WriteAsync(bytesWrite);
        }
    }
}
