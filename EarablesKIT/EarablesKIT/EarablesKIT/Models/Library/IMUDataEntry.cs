using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    class IMUDataEntry
    {
        private Accelerometer acc;
        public Accelerometer Acc { get => acc; set => acc = value; }
        private Gyroscope gyro;
        public Gyroscope Gyro { get => gyro; set => gyro = value; }
    }
}
