using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    public class ConfigContainer
    {
        private int samplerate = 50;
        public int Samplerate { get => samplerate; set => samplerate = value; }
        private LPF_Gyroscope gyroscope;
        // ausprobieren ob man hier gleich die methoden in die getter und setter schreiben kann so wie in der Klasse EarablesConnection und fdafür die attr dort entfernen
        public LPF_Gyroscope GyroscopeLPF { set => gyroscope = value; }
        private LPF_Accelerometer accelerometerLPF;
        public LPF_Accelerometer AccelerometerLPF { set => accelerometerLPF = value; }
        private byte[] byteOffset;
        public byte[] ByteOffset { get => byteOffset; set => byteOffset = value; }
        private int accScaleFactor;
        public int AccScaleFactor { get => accScaleFactor; set => accScaleFactor = value; }
        private double gyroScaleFactor;
        public double GyroScaleFactor { get => gyroScaleFactor; set => gyroScaleFactor = value; }
    }
}
