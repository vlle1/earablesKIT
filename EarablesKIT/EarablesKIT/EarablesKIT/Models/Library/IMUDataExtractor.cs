using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    /// <summary>
    /// This class exctract the informations from the bytearrays
    /// </summary>
    class IMUDataExtractor
    {
        /// <summary>
        /// Extracts the accelerometer and gyroscope information from the bytearray
        /// </summary>
        /// <param name="bytesIMUData"> The bytearray that contains the information</param>
        /// <param name="accScaleFactor"> The scalefactor for the accelerometer</param>
        /// <param name="gyroScaleFactor"> The scalefactor for the gyroscope</param>
        /// <param name="bytesOffset"> The bytearray that contaons the offset for the acceleration</param>
        /// <returns> Returns the accelerometer and gyroscope values as a IMUDataEntry</returns>
        public static IMUDataEntry ExtractIMUDataString(byte[] bytesIMUData, double accScaleFactor, double gyroScaleFactor, byte[] bytesOffset)
        {

            // Offest noch bearbeiten
            short AccOffsetX = (short)(bytesOffset[9] * 256 + bytesOffset[10]);
            short AccOffsetY = (short)(bytesOffset[11] * 256 + bytesOffset[12]);
            short AccOffsetZ = (short)(bytesOffset[13] * 256 + bytesOffset[14]);

            // Get the value from the accelerometer
            short AccXRaw = (short)(bytesIMUData[10] * 256 + bytesIMUData[11]);
            short AccYRaw = (short)(bytesIMUData[12] * 256 + bytesIMUData[13]);
            short AccZRaw = (short)(bytesIMUData[14] * 256 + bytesIMUData[14]);

            // Calculate the accelerometer in g
            float AccGX = (float)(AccXRaw / accScaleFactor);
            float AccGY = (float)(AccYRaw / accScaleFactor);
            float AccGZ = (float)(AccZRaw / accScaleFactor);

            // Calculate the acceleration in m/s^2
            float AccMSX = AccGX * 9.80665f;
            float AccMSY = AccGY * 9.80665f;
            float AccMSZ = AccGZ * 9.80665f;

            // Create the accelerometervalues
            Accelerometer accelerometer = new Accelerometer(AccGX, AccGY, AccGZ, AccMSX, AccMSY, AccMSZ);

            // Get the value from the gyroscope
            short GyroXRaw = (short)(bytesIMUData[4] * 256 + bytesIMUData[5]);
            short GyroYRaw = (short)(bytesIMUData[6] * 256 + bytesIMUData[7]);
            short GyroZRaw = (short)(bytesIMUData[8] * 256 + bytesIMUData[9]);

            // Scale the gyroscopevalues to deg/s
            float GyroX = (float)(GyroXRaw / gyroScaleFactor);
            float GyroY = (float)(GyroYRaw / gyroScaleFactor);
            float GyroZ = (float)(GyroZRaw / gyroScaleFactor);
            
            // Create the gyroscopevalues
            Gyroscope gyroscope = new Gyroscope(GyroX, GyroY, GyroZ);

            // Create the IMUDataEntry
            IMUDataEntry imuDataEntry = new IMUDataEntry(accelerometer, gyroscope);

            return imuDataEntry;
        }

        /// <summary>
        /// Extracts the range for the accelerometer from the bytearray
        /// </summary>
        /// <param name="bytes"> The bytearray that contains the information</param>
        /// <returns> Returns the range</returns>
        public static int ExtractIMURangeAccelerometer(byte[] bytes)
        {
            // Get the 3th and the 4th bit from Data2
            int byteValue = bytes[5] & 0x18;
            int Range = 0;
            // Select the right range
            switch (byteValue)
            {
                case (0x00):
                    Range = 2;
                    break;
                case (0x08):
                    Range = 4;
                    break;
                case (0x10):
                    Range = 8;
                    break;
                case (0x18):
                    Range = 16;
                    break;
            }
            return Range;
        }

        /// <summary>
        /// Extracts the scalefactor for the accelerometer from the bytearray
        /// </summary>
        /// <param name="bytes"> The bytearray that contains the information</param>
        /// <returns> Returns the scalefactor for the accelerometer</returns>
        public static int ExtractIMUScaleFactorAccelerometer(byte[] bytes)
        {
            // Get the 3th and the 4th bit from Data2
            int byteValue = bytes[5] & 0x18;
            int ScaleFactor = 0;
            // Select the right scaleFactor
            switch (byteValue)
            {
                case (0x00):
                    ScaleFactor = 16384;
                    break;
                case (0x08):
                    ScaleFactor = 8192;
                    break;
                case (0x10):
                    ScaleFactor = 4096;
                    break;
                case (0x18):
                    ScaleFactor = 2048;
                    break;
            }
            return ScaleFactor;
        }

        /// <summary>
        /// Extracts the range for the gyroscope from the bytearray
        /// </summary>
        /// <param name="bytes"> The bytearray that contains the information</param>
        /// <returns> Returns the range for the gyroscope</returns>
        public static double ExtractIMURangeGyroscope(byte[] bytes)
        {
            // Get the 3th and the 4th bit from Data2
            int byteValue = bytes[5] & 0x18;
            double Range = 0;
            // Select the right range
            switch (byteValue)
            {
                case (0x00):
                    Range = 250;
                    break;
                case (0x08):
                    Range = 500;
                    break;
                case (0x10):
                    Range = 1000;
                    break;
                case (0x18):
                    Range = 2000;
                    break;
            }
            return Range;
        }

        /// <summary>
        /// Extracts the scalefactor for the gyroscope from the bytearray
        /// </summary>
        /// <param name="bytes"> The bytearray that contains the information</param>
        /// <returns> Returns the scalefactor for the gyroscope</returns>
        public static double ExctractIMUScaleFactorGyroscope(byte[] bytes)
        {
            // Get the 3th and the 4th bit from Data1
            int byteValue = bytes[4] & 0x18;
            double ScaleFactor = 0;
            // Select the right scaleFactor
            switch (byteValue)
            {
                case (0x00):
                    ScaleFactor = 131;
                    break;
                case (0x08):
                    ScaleFactor = 65.5;
                    break;
                case (0x10):
                    ScaleFactor = 32.8;
                    break;
                case (0x18):
                    ScaleFactor = 16.4;
                    break;
            }
            return ScaleFactor;
        }

    }
}
