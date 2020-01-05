using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    class ConfigContainer
    {
        public int Samplerate { get => throw new NotImplementedException();}

        public LPF_Gyroscope GyroscopeLPF { get => throw new NotImplementedException(); }

        public LPF_Gyroscope AccelerometerLPF { get => throw new NotImplementedException(); }



    }


    public enum LPF_Gyroscope
    {

    }
    public enum LPF_Accelerometer
    {

    }
}
