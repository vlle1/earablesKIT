using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using static EarablesKIT.Models.Library.Constants;
using static EarablesKIT.Models.Library.IMUDataExtractor;



namespace EarablesKIT.Models.Library
{
      class EarablesConnection : IEarablesConnection
    {
        private IBluetoothLE ble = CrossBluetoothLE.Current;
        private IAdapter adapter = CrossBluetoothLE.Current.Adapter;
        private IDevice device;
        private ConfigContainer config = new ConfigContainer();
        private Characteristics characters = new Characteristics();
        private int accEnumValue;
        private int gyroEnumValue;
        private bool connected = true;
        public bool Connected { get => connected;} 
        public LPF_Accelerometer AccLPF { get => GetAccelerometerLPF(); set => SetAccelerometerLPF(value); }
        public LPF_Gyroscope GyroLPF { get => GetGyroscopeLPF(); set => SetGyroscopeLPF(value); }
        private float batteryVoltage;
        public float BatteryVoltage { get => batteryVoltage; }

        public EventHandler<DataEventArgs> IMUDataReceived;
        public EventHandler<ButtonEventArgs> ButtonPressed;
        public EventHandler<DeviceEventArgs> DeviceConnectionStateChanged;
        public EventHandler<NewDeviceFoundArgs> NewDeviceFound;

        public EarablesConnection()
        {
            deviceList = new List<IDevice>();
            config = new ConfigContainer();
            characters = new Characteristics();
        }

        public void ConnectToDevice(IDevice device)
        {
            if (connected)
            {
                throw new AllreadyConnectedException("Error, allready connected");
            }

            Device.BeginInvokeOnMainThread(new Action(async () =>
            {
                // Set the requiered Connection Parameters
                var connectParams = new ConnectParameters(true, true);

                   // adapter.DeviceConnected += OnDeviceConnected;
                    adapter.DeviceDisconnected += OnDeviceDisconnected;
                    adapter.DeviceConnectionLost += OnDeviceConnectionLost;

                    // Stop scanning for devices to be sure that nothing goes wrong
                    await adapter.StopScanningForDevicesAsync();
                    // Connect to the device
                    await adapter.ConnectToDeviceAsync(device, connectParams);
                    this.device = device;

                    // Load all required characteristics
                    IService Service;
                    Service = await device.GetServiceAsync(Guid.Parse(ACCES_SERVICE));
                    characters.StartStopIMUSamplingChar = await Service.GetCharacteristicAsync(Guid.Parse(START_STOP_IMU_SAMPLING_CHAR));
                    characters.SensordataChar = await Service.GetCharacteristicAsync(Guid.Parse(SENSORDATA_CHAR));
                    characters.PushbuttonChar = await Service.GetCharacteristicAsync(Guid.Parse(PUSHBUTTON_CHAR));
                    characters.BatteryChar = await Service.GetCharacteristicAsync(Guid.Parse(BATTERY_CHAR));
                    characters.OffsetChar = await Service.GetCharacteristicAsync(Guid.Parse(OFFSET_CHAR));
                    characters.AccelerometerGyroscopeLPFChar = await Service.GetCharacteristicAsync(Guid.Parse(ACC_GYRO_LPF_CHAR));

                    // Register on the events from the Earables (sollte am besten in eine art Constructor)
                    characters.SensordataChar.ValueUpdated += OnValueUpdatedIMU;
                    characters.PushbuttonChar.ValueUpdated += OnPushButtonPressed;
                    await characters.PushbuttonChar.StartUpdatesAsync();
                    characters.BatteryChar.ValueUpdated += GetBatteryVoltageFromDevice;
                    await characters.BatteryChar.StartUpdatesAsync();
                    connected = true;
                    DeviceEventArgs e = new DeviceEventArgs(connected, device.Name);
                    DeviceConnectionStateChanged?.Invoke(this, e);

                    // Initialise the BatteryVoltage the first time after connection in case it will be used befor the Batteryvalue updates the first time
                    await initBatteryVoltage();


            }));
        }

        public async void DisconnectFromDevice()
        {
            CheckConnection();
            await adapter.DisconnectDeviceAsync(device);
        }

        public bool IsBluetoothActive()
        {
            if (ble.State == BluetoothState.On)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Kann das private sein so we die anderen zwei und eine Methode für die zwei zeilen unter connected schreiben???
      //  public void OnDeviceConnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs args)
      //  {
      //      connected = true;
      //      Device.BeginInvokeOnMainThread(new Action(() =>
      //      {
      //          DeviceEventArgs e = new DeviceEventArgs(connected, args.Device.Name);
      //          DeviceConnectionStateChanged?.Invoke(this, e);
      //      }));
      //  }
        public void OnDeviceDisconnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs args)
        {
            connected = false;
            Device.BeginInvokeOnMainThread(new Action(() =>
            {
                DeviceEventArgs e = new DeviceEventArgs(connected, args.Device.Name);
                DeviceConnectionStateChanged?.Invoke(this, e);
            }));
        }

        // Brauchen wir die methode wenn sie keinen unterschied macht zu der oben drüber? man kann ja was ergänzen um zu signalisieren, dass die verbindung abgebrochen wurde
        public void OnDeviceConnectionLost(object sender, DeviceErrorEventArgs args)
        {
            connected = false;
            Device.BeginInvokeOnMainThread(new Action(() =>
            {
                DeviceConnectionStateChanged?.Invoke(this, new DeviceEventArgs(connected, args.Device.Name));
            }));
        }

        public void OnPushButtonPressed(object sender, CharacteristicUpdatedEventArgs args)
        {
            byte[] bytes = args.Characteristic.Value;
            // Get the LSB from Data0
            int bit = bytes[3] & 0x01;
            // Check if the PushButton was pressed and not released
            if (bit == 1)
            {
                ButtonPressed?.Invoke(this, new ButtonEventArgs());
            }
        }

        public void OnValueUpdatedIMU(object sender, CharacteristicUpdatedEventArgs args)
        {
            byte[] bytesIMUValue = args.Characteristic.Value;
            IMUDataEntry imuDataEntry = ExtractIMUDataString(bytesIMUValue, config.AccScaleFactor, config.GyroScaleFactor, config.ByteOffset);
            IMUDataReceived?.Invoke(this, new DataEventArgs(imuDataEntry, config));
        }

        public void SetSamplingRate(int rate)
        {
            if (rate < 1 || rate > 100)
            {
                throw new InvalideSamplerateException("The Samplerate has to be between 1 and 100");
            }
            config.Samplerate = rate;
        }

        public void StartSampling()
        {
            CheckConnection();
            Device.BeginInvokeOnMainThread(new Action(async () =>
            {
                byte[] bytesScaleFactor = await characters.AccelerometerGyroscopeLPFChar.ReadAsync();
                config.GyroScaleFactor = IMUDataExtractor.ExctractIMUScaleFactorGyroscope(bytesScaleFactor);
                config.AccScaleFactor = IMUDataExtractor.ExtractIMUScaleFactorAccelerometer(bytesScaleFactor);
                config.ByteOffset = await characters.OffsetChar.ReadAsync();
                await characters.SensordataChar.StartUpdatesAsync();
                byte[] bytes = { 0x53, Convert.ToByte(config.Samplerate + 3), 0x02, 0x01, Convert.ToByte(config.Samplerate) };
                await characters.StartStopIMUSamplingChar.WriteAsync(bytes);
            }));
        }


        public async void StartScanning()
        {
               // deviceList.Clear();
                adapter.DeviceDiscovered += (s, a) =>
                {
                    NewDeviceFound?.Invoke(this, new NewDeviceFoundArgs(a.Device) );
                   // deviceList.Add(a.Device);
                };

                if (!ble.Adapter.IsScanning)
                {
                    await adapter.StartScanningForDevicesAsync();
                    //ScannedDeviceHandler?.Invoke(this, new ScanDeviceArgs(deviceList));
                }
        }

        public async void StopSampling()
        {
            CheckConnection();
            await characters.SensordataChar.StopUpdatesAsync();
            byte[] bytes = { 0x53, 0x02, 0x02, 0x00, 0x00 };
            await characters.StartStopIMUSamplingChar.WriteAsync(bytes);
        }

        private async void SetAccelerometerLPF(LPF_Accelerometer accelerometerLPF)
        {
            CheckConnection();
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
            // wird hier schon gesetzt und nicht erst wenn neue daten ankommen, da
            config.AccelerometerLPF = accelerometerLPF;
        }

        private LPF_Accelerometer GetAccelerometerLPF()
        {
            GetAccelerometerLPFFromDevice();
            LPF_Accelerometer acc = (LPF_Accelerometer)Enum.ToObject(typeof(LPF_Accelerometer), accEnumValue);
            return acc;
        }

        private async void GetAccelerometerLPFFromDevice()
        {
            CheckConnection();
            byte[] bytes = await characters.AccelerometerGyroscopeLPFChar.ReadAsync();
            // read only the 4 LSBs
            accEnumValue = (int)(bytes[6] & 0x0F);
        }

        private async void SetGyroscopeLPF(LPF_Gyroscope gyroscopeLPF)
        {
            CheckConnection();
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
            config.GyroscopeLPF = gyroscopeLPF;
        }

        private LPF_Gyroscope GetGyroscopeLPF()
        {
            GetGyroscopeLPFFromDevice();
            LPF_Gyroscope gyro = (LPF_Gyroscope)Enum.ToObject(typeof(LPF_Gyroscope), gyroEnumValue);
            return gyro;
        }

        private async void GetGyroscopeLPFFromDevice()
        {
            CheckConnection();
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

        private async System.Threading.Tasks.Task initBatteryVoltage()
        {
            CheckConnection();
            byte[] bytes = await characters.BatteryChar.ReadAsync();
            batteryVoltage = (bytes[3] * 256 + bytes[4]) / 1000f;
        }

        private void GetBatteryVoltageFromDevice(object sender, CharacteristicUpdatedEventArgs args)
        {
            CheckConnection();
            byte[] bytes = args.Characteristic.Value;
            batteryVoltage = (bytes[3] * 256 + bytes[4]) / 1000f;
        }

        private async void setAccelerometerRange()
        {
            CheckConnection();
            // Range kann sein 0x00 = 2g, 0x08 = 4g, 0x10 = 8g, 0x18 = 16g
            int range = 0x00;

            byte[] bytesRead = await characters.AccelerometerGyroscopeLPFChar.ReadAsync();
            //clear Bit 4 and 3 from Data2
            int data2 = bytesRead[5] & 0xE7;
            // Set Data2 to the right value
            data2 = data2 | range;
            // Calculate chechsum
            int checksum = bytesRead[2] + bytesRead[3] + bytesRead[4] + data2 + bytesRead[6];
            // Write the new Accelerometerrange on the Earables
            byte[] bytesWrite = { 0x59, Convert.ToByte(checksum), bytesRead[2], bytesRead[3], bytesRead[4], Convert.ToByte(data2), bytesRead[6] };
            await characters.AccelerometerGyroscopeLPFChar.WriteAsync(bytesWrite);
        }

        private async void setGyroscopeRange()
        {
            CheckConnection();
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

        private void CheckConnection()
        {
            if (!connected)
            {
                throw new NoConnectionException("Error, no device connected");
            }
        }

        public bool GetConnection()
        {
            return connected;
        }
    }
}