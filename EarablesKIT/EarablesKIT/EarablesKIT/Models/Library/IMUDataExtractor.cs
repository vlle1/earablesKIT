using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    class IMUDataExtractor
    {
        public static IMUDataEntry ExtractIMUDataString(byte[] data, int accScaleFactor, int gyroScaleFactor)
        {

            throw new NotImplementedException();
        }
        public static int ExtractIMUScaleFactorAccelerometer(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static double ExctractIMUScaleFactorGyroscope(byte[] data)
        {
            // Get the 3th and the 4th Bit from Data1
            int byteValue = data[4] & 0x18;
            double ScaleFactor = 0;
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
