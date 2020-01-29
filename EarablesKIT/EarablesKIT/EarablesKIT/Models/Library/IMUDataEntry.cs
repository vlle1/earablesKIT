using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    /// <summary>
    /// This class contains all information from the accelerometer and the gyroscope 
    /// </summary>
    public class IMUDataEntry
    {
        private Accelerometer acc;
        public Accelerometer Acc { get => acc;}
        private Gyroscope gyro;
        public Gyroscope Gyro { get => gyro;}
        /// <summary>
        ///  Constructor to set all values at once
        /// </summary>
        /// <param name="acc"> The acceleration values </param>
        /// <param name="gyro"> The gyroscope values</param>
        public IMUDataEntry(Accelerometer acc, Gyroscope gyro)
        {
            this.acc = acc;
            this.gyro = gyro;
        }
    }
}
