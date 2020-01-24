using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    /// <summary>
    /// This class contains all configurations
    /// </summary>
    public class ConfigContainer
    {
        private int samplerate = 50;
        public int Samplerate { get => samplerate; set => setSamplingRate(value); }
        private LPF_Gyroscope gyroscopeLPF = LPF_Gyroscope.Hz5;
        public LPF_Gyroscope GyroscopeLPF { get => gyroscopeLPF; set => gyroscopeLPF = value; }
        private LPF_Accelerometer accelerometerLPF = LPF_Accelerometer.Hz5;
        public LPF_Accelerometer AccelerometerLPF { get => accelerometerLPF; set => accelerometerLPF = value; }
        private int accScaleFactor;
        public int AccScaleFactor { get => accScaleFactor; set => accScaleFactor = value; }
        private double gyroScaleFactor;
        public double GyroScaleFactor { get => gyroScaleFactor; set => gyroScaleFactor = value; }

        /// <summary>
        /// Checks if the samplingrate is in the valid interval
        /// </summary>
        /// <param name="rate"></param>
        private void setSamplingRate(int rate)
        {
            if (rate < 1 || rate > 100)
            {
                throw new InvalideSamplerateException("The Samplerate has to be between 1 and 100");
            }
            samplerate = rate;
        }
    }
}