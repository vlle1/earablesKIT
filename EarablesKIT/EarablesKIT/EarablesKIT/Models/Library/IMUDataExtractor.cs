using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    class IMUDataExtractor
    {
        public static IMUDataEntry ExtractIMUDataString(byte[] bytesIMUData, double accScaleFactor, double gyroScaleFactor, byte[] bytesOffset)
        {

            // Offest noch bearbeiten
            short AccOffsetX = (short)(bytesOffset[9] * 256 + bytesOffset[10]);
            short AccOffsetY = (short)(bytesOffset[11] * 256 + bytesOffset[12]);
            short AccOffsetZ = (short)(bytesOffset[13] * 256 + bytesOffset[14]);

            // Get the Value from the Accelerometer
            short AccXRaw = (short)(bytesIMUData[10] * 256 + bytesIMUData[11]);
            short AccYRaw = (short)(bytesIMUData[12] * 256 + bytesIMUData[13]);
            short AccZRaw = (short)(bytesIMUData[14] * 256 + bytesIMUData[14]);

            // Calculate the Accelerometer in G
            float AccGX = (float)(AccXRaw / accScaleFactor);
            float AccGY = (float)(AccYRaw / accScaleFactor);
            float AccGZ = (float)(AccZRaw / accScaleFactor);

            // Calculate the Acceleration in m/s^2
            float AccMSX = AccGX * 9.80665f;
            float AccMSY = AccGY * 9.80665f;
            float AccMSZ = AccGZ * 9.80665f;

            // Create the Accelerometervalues
            Accelerometer accelerometer = new Accelerometer(AccGX, AccGY, AccGZ, AccMSX, AccMSY, AccMSZ);

            // Get the Value from the Gyroscope
            short GyroXRaw = (short)(bytesIMUData[4] * 256 + bytesIMUData[5]);
            short GyroYRaw = (short)(bytesIMUData[6] * 256 + bytesIMUData[7]);
            short GyroZRaw = (short)(bytesIMUData[8] * 256 + bytesIMUData[9]);

            // Scale the Gyroscopevalues to deg/s
            float GyroX = (float)(GyroXRaw / gyroScaleFactor);
            float GyroY = (float)(GyroYRaw / gyroScaleFactor);
            float GyroZ = (float)(GyroZRaw / gyroScaleFactor);
            
            // Create the Gyroscopevalues
            Gyroscope gyroscope = new Gyroscope(GyroX, GyroY, GyroZ);

            // Create the IMUDataEntry
            IMUDataEntry imuDataEntry = new IMUDataEntry(accelerometer, gyroscope);

            return imuDataEntry;
        }

        public static int ExtractIMURangeAccelerometer(byte[] bytes)
        {
            // Get the 3th and the 4th Bit from Data2
            int byteValue = bytes[5] & 0x18;
            int Range = 0;
            // Select the right Range
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
        public static int ExtractIMUScaleFactorAccelerometer(byte[] bytes)
        {
            // Get the 3th and the 4th Bit from Data2
            int byteValue = bytes[5] & 0x18;
            int ScaleFactor = 0;
            // Select the right ScaleFactor
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

        public static double ExtractIMURangeGyroscope(byte[] bytes)
        {
            // Get the 3th and the 4th Bit from Data2
            int byteValue = bytes[5] & 0x18;
            double Range = 0;
            // Select the right Range
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
        public static double ExctractIMUScaleFactorGyroscope(byte[] bytes)
        {
            // Get the 3th and the 4th Bit from Data1
            int byteValue = bytes[4] & 0x18;
            double ScaleFactor = 0;
            // Select the right ScaleFactor
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
