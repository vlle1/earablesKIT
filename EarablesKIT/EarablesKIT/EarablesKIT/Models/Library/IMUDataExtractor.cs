using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    class IMUDataExtractor
    {
        public static IMUDataEntry ExtractIMUDataString(byte[] bytes, double accScaleFactor, double gyroScaleFactor, short accoffset)
        {

            // Offest noch bearbeiten

            // Initialise all required Objects
            IMUDataEntry imuDataEntry = new IMUDataEntry();
            Accelerometer accelerometer = new Accelerometer();
            Gyroscope gyroscope = new Gyroscope();

            // Get the Value from the Accelerometer
            short AccXRaw = (short)(bytes[10] * 256 + bytes[11]);
            short AccYRaw = (short)(bytes[12] * 256 + bytes[13]);
            short AccZRaw = (short)(bytes[14] * 256 + bytes[14]);

            // Scale the Accelerometervalues to G
            float AccX = (float)(AccXRaw / accScaleFactor);
            float AccY = (float)(AccYRaw / accScaleFactor);
            float AccZ = (float)(AccZRaw / accScaleFactor);

            // Set the Acceleration in G
            accelerometer.G_X = AccX;
            accelerometer.G_Y = AccY;
            accelerometer.G_Z = AccZ;

            // Calculate the Acceleration in m/s^2
            AccX = AccX * 9.80665f;
            AccY = AccY * 9.80665f;
            AccZ = AccZ * 9.80665f;

            // Set the Acceleration in m/s^2
            accelerometer.MperS_X = AccX;
            accelerometer.MperS_Y = AccY;
            accelerometer.MperS_Z = AccZ;

            // Get the Value from the Gyroscope
            short GyroXRaw = (short)(bytes[4] * 256 + bytes[5]);
            short GyroYRaw = (short)(bytes[6] * 256 + bytes[7]);
            short GyroZRaw = (short)(bytes[8] * 256 + bytes[9]);

            // Scale the Gyroscopevalues to deg/s
            float GyroX = (float)(GyroXRaw / gyroScaleFactor);
            float GyroY = (float)(GyroYRaw / gyroScaleFactor);
            float GyroZ = (float)(GyroZRaw / gyroScaleFactor);

            // Set the Gyrochanges in deg/s
            gyroscope.DegsPerSec_X = GyroX;
            gyroscope.DegsPerSec_X = GyroY;
            gyroscope.DegsPerSec_X = GyroZ;

            // Set the Data in the IMUDataEntry
            imuDataEntry.Acc = accelerometer;
            imuDataEntry.Gyro = gyroscope;

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
