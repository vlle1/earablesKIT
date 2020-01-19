using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    class IMUDataEntry
    {
        private Accelerometer acc;
        public Accelerometer Acc { get => acc;}
        private Gyroscope gyro;
        public Gyroscope Gyro { get => gyro;}

        public IMUDataEntry(Accelerometer acc, Gyroscope gyro)
        {
            this.acc = acc;
            this.gyro = gyro;
        }
    }
}
