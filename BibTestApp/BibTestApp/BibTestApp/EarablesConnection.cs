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
    /// <summary>
    /// This class is responsible for the connection with teh earables
    /// </summary>
    public class EarablesConnection : IEarablesConnection
    {
        //
        private IBluetoothLE ble = CrossBluetoothLE.Current;
        private IAdapter adapter = CrossBluetoothLE.Current.Adapter;
        // The connectet device
        private IDevice device;
        // Saves the configurations
        private ConfigContainer config = new ConfigContainer();
        // Caching the bytearray which contains the offset
        private byte[] byteOffset;
        // Holds all characteristics
        private Characteristics characters = new Characteristics();
        // The connection state
        private bool connected = false;
        public bool Connected { get => connected; }
        public bool IsBluetoothActive { get => CheckIsBluetoothActive(); }
        public int SampleRate { get => config.Samplerate; set => config.Samplerate = value; }
        public LPF_Accelerometer AccLPF { get => GetAccelerometerLPF(); set => SetAccelerometerLPF(value); }
        public LPF_Gyroscope GyroLPF { get => GetGyroscopeLPF(); set => SetGyroscopeLPF(value); }
        // The current batteryvoltage
        private float batteryVoltage;

        public event EventHandler<DataEventArgs> IMUDataReceived;
        public event EventHandler<ButtonEventArgs> ButtonPressed;
        public event EventHandler<DeviceEventArgs> DeviceConnectionStateChanged;
        public event EventHandler<NewDeviceFoundArgs> NewDeviceFound;

        public float BatteryVoltage { get => batteryVoltage; }


        /// <summary>
        ///  Connect to a device
        /// </summary>
        /// <param name="device"> Is the device which should connect</param>
        public void ConnectToDevice(IDevice device)
        {

            // Connectioncheck
            if (connected)
            {
                throw new AllreadyConnectedException("Error, allready connected");
            }
            this.device = device;

            Device.BeginInvokeOnMainThread(new Action(async () =>
            {
                // Set the requiered connection Parameters
                var connectParams = new ConnectParameters(true, true);

                // Register on the events from the earables
                adapter.DeviceDisconnected += OnDeviceDisconnected;
                adapter.DeviceConnectionLost += OnDeviceConnectionLost;
                // adapter.DeviceConnected += OnDeviceConnected;

                // Stop scanning for devices to be sure that nothing goes wrong
                await adapter.StopScanningForDevicesAsync();
                // Connect to the device
                await adapter.ConnectToDeviceAsync(device, connectParams);


                // Load all required characteristics
                IService Service;
                Service = await device.GetServiceAsync(Guid.Parse(ACCES_SERVICE));
                characters.StartStopIMUSamplingChar = await Service.GetCharacteristicAsync(Guid.Parse(START_STOP_IMU_SAMPLING_CHAR));
                characters.SensordataChar = await Service.GetCharacteristicAsync(Guid.Parse(SENSORDATA_CHAR));
                characters.PushbuttonChar = await Service.GetCharacteristicAsync(Guid.Parse(PUSHBUTTON_CHAR));
                characters.BatteryChar = await Service.GetCharacteristicAsync(Guid.Parse(BATTERY_CHAR));
                characters.OffsetChar = await Service.GetCharacteristicAsync(Guid.Parse(OFFSET_CHAR));
                characters.AccelerometerGyroscopeLPFChar = await Service.GetCharacteristicAsync(Guid.Parse(ACC_GYRO_LPF_CHAR));

                // Set scalefactors on norm
                byte[] bytes = { 0x59, 0x20, 0x04, 0x06, 0x08, 0x08, 0x06 };
                await characters.AccelerometerGyroscopeLPFChar.WriteAsync(bytes);

                // Starts the updating from the characteristics
                try
                {
                    await characters.PushbuttonChar.StartUpdatesAsync();
                    await characters.BatteryChar.StartUpdatesAsync();
                }
                catch (Exception exc)
                {
                    throw new ConnectionFailedException("Failed to connect. Please try again");
                }

                // Register the listeners to the characteristicevents
                characters.SensordataChar.ValueUpdated += OnValueUpdatedIMU;
                characters.PushbuttonChar.ValueUpdated += OnPushButtonPressed;
                characters.BatteryChar.ValueUpdated += GetBatteryVoltageFromDevice;

                // Throw event connected
                connected = true;
                DeviceEventArgs e = new DeviceEventArgs(connected, device.Name);
                DeviceConnectionStateChanged?.Invoke(this, e);

                // Initialise the BatteryVoltage the first time after connection in case it will be used befor the Batteryvalue updates the first time
                await initBatteryVoltage();


            }));
        }

        /// <summary>
        /// Disconnects from the device
        /// </summary>
        public async void DisconnectFromDevice()
        {
            CheckConnection();
            await adapter.DisconnectDeviceAsync(device);
        }

        /// <summary>
        /// Checks if Bluetooth on the smartphone is on
        /// </summary>
        /// <returns></returns>
        private bool CheckIsBluetoothActive()
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

        //  public void OnDeviceConnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs args)
        //  {
        //      connected = true;
        //      Device.BeginInvokeOnMainThread(new Action(() =>
        //      {
        //          DeviceEventArgs e = new DeviceEventArgs(connected, args.Device.Name);
        //          DeviceConnectionStateChanged?.Invoke(this, e);
        //      }));
        //  }


        /// <summary>
        /// Catches the event DeviceDisconnected from the earables and throws the event DeviceConnectionStateChanged
        /// </summary>
        /// <param name="sender">The Objekt which has thrown the event</param>
        /// <param name="args">The arguments from the exception</param>
        private void OnDeviceDisconnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs args)
        {
            connected = false;
            Device.BeginInvokeOnMainThread(new Action(() =>
            {
                DeviceEventArgs e = new DeviceEventArgs(connected, args.Device.Name);
                DeviceConnectionStateChanged?.Invoke(this, e);
            }));
        }

        // Brauchen wir die methode wenn sie keinen unterschied macht zu der oben drüber? man kann ja was ergänzen um zu signalisieren, dass die verbindung abgebrochen wurde
        private void OnDeviceConnectionLost(object sender, DeviceErrorEventArgs args)
        {
            connected = false;
            Device.BeginInvokeOnMainThread(new Action(() =>
            {
                DeviceConnectionStateChanged?.Invoke(this, new DeviceEventArgs(connected, args.Device.Name));
            }));
        }

        /// <summary>
        /// Notice if the Button on the earables is pressed and throws the event ButtonPressed
        /// </summary>
        /// <param name="sender">The Objekt which has thrown the event</param>
        /// <param name="args">The arguments from the exception</param>
        private void OnPushButtonPressed(object sender, CharacteristicUpdatedEventArgs args)
        {
            byte[] bytes = args.Characteristic.Value;
            //Check Checksum
            CheckChecksum(bytes);
            // Get the LSB from Data0
            int bit = bytes[3] & 0x01;
            // Check if the PushButton was pressed and not released
            if (bit == 1)
            {
                ButtonPressed?.Invoke(this, new ButtonEventArgs());
            }
        }

        /// <summary>
        /// Notice if new Data from the imu is recived and throws the event IMUDataRecived
        /// </summary>
        /// <param name="sender">The Objekt which has thrown the event</param>
        /// <param name="args">The arguments from the exception</param>
        private void OnValueUpdatedIMU(object sender, CharacteristicUpdatedEventArgs args)
        {
            byte[] bytesIMUValue = args.Characteristic.Value;
            //Check Checksum
            CheckIMUChecksum(bytesIMUValue);
            IMUDataEntry imuDataEntry = ExtractIMUDataString(bytesIMUValue, config.AccScaleFactor, config.GyroScaleFactor, byteOffset);
            IMUDataReceived?.Invoke(this, new DataEventArgs(imuDataEntry, config));
        }


        /// <summary>
        /// Starts the sampling
        /// </summary>
        public void StartSampling()
        {
            CheckConnection();
            Device.BeginInvokeOnMainThread(new Action(async () =>
            {
                // Read the current value from the characteristic
                byte[] bytesScaleFactor = await characters.AccelerometerGyroscopeLPFChar.ReadAsync();
                //Check Checksum
                CheckChecksum(bytesScaleFactor);
                // Extract the actual gyroscope scalefactor from the bytearray
                config.GyroScaleFactor = IMUDataExtractor.ExctractIMUScaleFactorGyroscope(bytesScaleFactor);
                // Extract the actual accelerometer scalefactor from the bytearray
                config.AccScaleFactor = IMUDataExtractor.ExtractIMUScaleFactorAccelerometer(bytesScaleFactor);
                // Read the bytearray for the offset
                byteOffset = await characters.OffsetChar.ReadAsync();
                //Check Checksum
                CheckChecksum(byteOffset);
                // Starts the updating for the characteristic
                await characters.SensordataChar.StartUpdatesAsync();
                // Starts the sampling
                byte[] bytes = { 0x53, Convert.ToByte(config.Samplerate + 3), 0x02, 0x01, Convert.ToByte(config.Samplerate) };
                await characters.StartStopIMUSamplingChar.WriteAsync(bytes);
            }));
        }

        /// <summary>
        /// Starts the Scanning
        /// </summary>
        public async void StartScanning()
        {
            adapter.DeviceDiscovered += (s, a) =>
            {
                // Throws an event if new devices are discoverd
                NewDeviceFound?.Invoke(this, new NewDeviceFoundArgs(a.Device));
            };

            if (!ble.Adapter.IsScanning)
            {
                await adapter.StartScanningForDevicesAsync();
            }
        }

        /// <summary>
        /// Stops the sampling
        /// </summary>
        public void StopSampling()
        {
            CheckConnection();
            Device.BeginInvokeOnMainThread(new Action(async () =>
            {
                // Stops updating the characteristic
                await characters.SensordataChar.StopUpdatesAsync();
                // Stops the sampling
                byte[] bytes = { 0x53, 0x02, 0x02, 0x00, 0x00 };
                await characters.StartStopIMUSamplingChar.WriteAsync(bytes);
            }));
        }

        /// <summary>
        /// Sets the aaccelerometerLPF
        /// </summary>
        /// <param name="accelerometerLPF">The value on which the accelerometerLPF should be set</param>
        private void SetAccelerometerLPF(LPF_Accelerometer accelerometerLPF)
        {
            CheckConnection();
            Device.BeginInvokeOnMainThread(new Action(async () =>
            {
                // Read the characteristic to calculate the checksum and Data3
                byte[] bytesRead = await characters.AccelerometerGyroscopeLPFChar.ReadAsync();
                //Check Checksum
                CheckChecksum(bytesRead);
                // clear the 4 LSBs from data3
                int data3 = bytesRead[6] & 0xF0;
                // set the 4 LSBs from data3 on the required value
                data3 = data3 | (int)accelerometerLPF;
                // calculate checksum
                int checksum = bytesRead[2] + bytesRead[3] + bytesRead[4] + bytesRead[5] + (int)data3;
                // Write the new accelerometerLPF in to the characteristic
                byte[] bytesWrite = { 0x59, Convert.ToByte(checksum), bytesRead[2], bytesRead[3], bytesRead[4], bytesRead[5], Convert.ToByte(data3) };
                await characters.AccelerometerGyroscopeLPFChar.WriteAsync(bytesWrite);
                config.AccelerometerLPF = accelerometerLPF;
            }));
        }

        /// <summary>
        /// Returns the current LPF for the accelerometer
        /// </summary>
        /// <returns>Returns the LPF for the accelerometer</returns>
        private LPF_Accelerometer GetAccelerometerLPF()
        {
            CheckConnection();
            return config.AccelerometerLPF;
        }

        /// <summary>
        /// Get the accelerometerLPF from the device
        /// Not needed but helpfull for testing
        /// </summary>
        public async System.Threading.Tasks.Task<LPF_Accelerometer> GetAccelerometerLPFFromDeviceAsync()
        {
            CheckConnection();
            byte[] bytes = await characters.AccelerometerGyroscopeLPFChar.ReadAsync();
            //Check Checksum
            CheckChecksum(bytes);
            // read only the 4 LSBs 
            int accEnumValue = (int)(bytes[6] & 0x0F);
            LPF_Accelerometer lpf = (LPF_Accelerometer) accEnumValue;
            return lpf;
        }

        /// <summary>
        /// Sets the gyroscope LPF
        /// </summary>
        /// <param name="gyroscopeLPF">The value on which the gyroscopeLPF should be set</param>
        private void SetGyroscopeLPF(LPF_Gyroscope gyroscopeLPF)
        {
            CheckConnection();
            Device.BeginInvokeOnMainThread(new Action(async () =>
            {
                // The bytes that needed to be modified
                int data0 = 0;
                int data1 = 0;
                // Read the characteristic to calculate the checksum, Data0 and Data1
                byte[] bytesRead = await characters.AccelerometerGyroscopeLPFChar.ReadAsync();
                //Check Checksum
                CheckChecksum(bytesRead);
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
                //If the LPF for the gyroscope is OFF only Data1 need to be modified
                else
                {
                    // set the 2 LSBs on 1
                    data1 = data1 | 0x01;
                }
                // Calculate the checksum
                int checksum = bytesRead[2] + (int)data0 + (int)data1 + bytesRead[5] + bytesRead[6];
                // Write the new gyroscopeLPF in to the characteristic
                byte[] bytesWrite = { 0x59, Convert.ToByte(checksum), bytesRead[2], Convert.ToByte(data0), Convert.ToByte(data1), bytesRead[5], bytesRead[6] };
                await characters.AccelerometerGyroscopeLPFChar.WriteAsync(bytesWrite);
                config.GyroscopeLPF = gyroscopeLPF;
            }));
        }

        /// <summary>
        /// Returns the current LPF for the gyroscope
        /// </summary>
        /// <returns>Returns the LPF for the gyroscope</returns>
        private LPF_Gyroscope GetGyroscopeLPF()
        {
            CheckConnection();
            return config.GyroscopeLPF;
        }

        /// <summary>
        /// Get the gyroscopeLPF from the device
        /// Not needed but helpfull for testing
        /// </summary>
        public async System.Threading.Tasks.Task<LPF_Gyroscope> GetGyroscopeLPFFromDevice()
        {
            CheckConnection();
            byte[] bytes = await characters.AccelerometerGyroscopeLPFChar.ReadAsync();
            //Check Checksum
            CheckChecksum(bytes);
            // read only the 2 LSBs and checks if the Gyro LPF is bypassed
            int b = (int)(bytes[4] & 0x03);
            // If the gyro LPF is bypassed it is representet as OFF in the LPF_Gyroscope Enum
            int gyroEnumValue;
            if (b == 0)
            {
                gyroEnumValue = (int)(bytes[3] & 0x07);
            }
            else
            {
                gyroEnumValue = 8;
            }
            LPF_Gyroscope lpf = (LPF_Gyroscope) gyroEnumValue;
            return lpf;
        }

        /// <summary>
        /// Sets the batteryVoltage
        /// </summary>
        /// <returns>Returns a Task cause it is a async method</returns>
        private async System.Threading.Tasks.Task initBatteryVoltage()
        {
            CheckConnection();
            byte[] bytes = await characters.BatteryChar.ReadAsync();
            //Check Checksum
            CheckChecksum(bytes);
            batteryVoltage = (bytes[3] * 256 + bytes[4]) / 1000f;
        }

        /// <summary>
        /// Notice if the batterychar changes and updates the batteryvalue
        /// </summary>
        /// <param name="sender">The Objekt which has thrown the event</param>
        /// <param name="args">The arguments from the exception</param>
        private void GetBatteryVoltageFromDevice(object sender, CharacteristicUpdatedEventArgs args)
        {
            CheckConnection();
            byte[] bytes = args.Characteristic.Value;
            //Check Checksum
            CheckChecksum(bytes);
            batteryVoltage = (bytes[3] * 256 + bytes[4]) / 1000f;
        }

        /// <summary>
        /// Set the accelerometer range
        /// Not needed but helpfull for testing
        /// </summary>
        private void SetAccelerometerRange(int range)
        {
            CheckConnection();
            Device.BeginInvokeOnMainThread(new Action(async () =>
            {
                // Range kann sein 0x00 = 2g, 0x08 = 4g, 0x10 = 8g, 0x18 = 16g
                //int range = 0x00;
                byte[] bytesRead = await characters.AccelerometerGyroscopeLPFChar.ReadAsync();
                //Check Checksum
                CheckChecksum(bytesRead);
                //clear Bit 4 and 3 from Data2
                int data2 = bytesRead[5] & 0xE7;
                // Set Data2 to the right value
                data2 = data2 | range;
                // Calculate chechsum
                int checksum = bytesRead[2] + bytesRead[3] + bytesRead[4] + data2 + bytesRead[6];
                // Write the new Accelerometerrange on the Earables
                byte[] bytesWrite = { 0x59, Convert.ToByte(checksum), bytesRead[2], bytesRead[3], bytesRead[4], Convert.ToByte(data2), bytesRead[6] };
                await characters.AccelerometerGyroscopeLPFChar.WriteAsync(bytesWrite);
            }));
        }

        /// <summary>
        /// Set the gyroscope range
        /// Not needed but helpfull for testing
        /// </summary>
        private void SetGyroscopeRange(int range)
        {
            CheckConnection();
            Device.BeginInvokeOnMainThread(new Action(async () =>
            {
                // Range kann sein 0x00 = 250deg/s, 0x08 = 500deg/s, 0x10 = 1000deg/s, 0x18 = 2000deg/s
                //int range = 0x18;
                byte[] bytesRead = await characters.AccelerometerGyroscopeLPFChar.ReadAsync();
                //Check Checksum
                CheckChecksum(bytesRead);
                //clear Bit 4 and 3 from Data1
                int data1 = bytesRead[4] & 0xE7;
                // Set Data1 to the right value
                data1 = data1 | range;
                // Calculate chechsum
                int checksum = bytesRead[2] + bytesRead[3] + data1 + bytesRead[5] + bytesRead[6];
                // Write the new Gyroscoperange on the Earables
                byte[] bytesWrite = { 0x59, Convert.ToByte(checksum), bytesRead[2], bytesRead[3], Convert.ToByte(data1), bytesRead[5], bytesRead[6] };
                await characters.AccelerometerGyroscopeLPFChar.WriteAsync(bytesWrite);
            }));
        }

        /// <summary>
        /// Checks if the earables are still connected
        /// </summary>
        private void CheckConnection()
        {
            if (!connected)
            {
                throw new NoConnectionException("Error, no device connected");
            }
        }

                /// <summary>
        /// Checks if the checksum from the received bytearray is correct
        /// </summary>
        /// <param name="bytes">The bytearray that holds the information about the checksum</param>
        private void CheckChecksum(byte[] bytes)
        {
            int checksum = 0;
            for (int i = 2; i < bytes.Length; i++ )
            {
                checksum += bytes[i];
            }
            checksum = checksum & 0b_1111_1111;
            if(checksum != bytes[1])
            {
                throw new ChecksumException("There was a problem with the checksum");
            }
        }

        /// <summary>
        /// Checks if the checksum from the received bytearray is correct
        /// </summary>
        /// <param name="bytes">The bytearray that holds the information about the checksum</param>
        private void CheckIMUChecksum(byte[] bytes)
        {
            int checksum = 0;
            for (int i = 3; i < bytes.Length; i++)
            {
                checksum += bytes[i];
            }
            checksum = checksum & 0b_1111_1111;
            if (checksum != bytes[2])
            {
                throw new ChecksumException("There was a problem with the checksum from the IMUData");
            }
        }
    }
}
