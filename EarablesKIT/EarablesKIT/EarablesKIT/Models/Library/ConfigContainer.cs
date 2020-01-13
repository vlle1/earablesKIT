using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    class ConfigContainer
    {
        private int samplerate;
        public int Samplerate { get => samplerate; set => samplerate = value; }
        private LPF_Gyroscope gyroscope;
        public LPF_Gyroscope GyroscopeLPF { get => gyroscope; set => gyroscope = value; }
        private LPF_Accelerometer accelerometerLPF;
        public LPF_Accelerometer AccelerometerLPF { get => accelerometerLPF; set => accelerometerLPF = value; }



    }


    public enum LPF_Gyroscope
    {
        Hz250 = 0,
        Hz184 = 1,
        Hz92 = 2,
        Hz41 = 3,
        Hz20 = 4,
        Hz10 = 5,
        Hz5 = 6,
        Hz3600 = 7,
        OFF = 8,
    };
    public enum LPF_Accelerometer
    {
        Hz460 = 0,
        Hz184 = 1,
        Hz92 = 2,
        Hz41 = 3,
        Hz20 = 4,
        Hz10 = 5,
        Hz5 = 6,
        OFF = 8,
    };
}
