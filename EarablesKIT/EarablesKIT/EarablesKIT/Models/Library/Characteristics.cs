using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    /// <summary>
    /// This class provides all characteristics
    /// </summary>
    class Characteristics
    {
        private ICharacteristic startStopIMUSamplingChar;
        public ICharacteristic StartStopIMUSamplingChar { get => startStopIMUSamplingChar; set => startStopIMUSamplingChar = value; }
        private ICharacteristic sensordataChar;
        public ICharacteristic SensordataChar { get => sensordataChar; set => sensordataChar = value; }
        private ICharacteristic pushbuttonChar;
        public ICharacteristic PushbuttonChar { get => pushbuttonChar; set => pushbuttonChar = value; }
        private ICharacteristic batteryChar;
        public ICharacteristic BatteryChar { get => batteryChar; set => batteryChar = value; }
        private ICharacteristic imuScaleRangeChar;
        public ICharacteristic IMUScaleRangeChar { get => imuScaleRangeChar; set => imuScaleRangeChar = value; }
        private ICharacteristic accelerometerGyroscopeLPFChar;
        public ICharacteristic AccelerometerGyroscopeLPFChar { get => accelerometerGyroscopeLPFChar; set => accelerometerGyroscopeLPFChar = value; }
        private ICharacteristic offsetChar;
        public ICharacteristic OffsetChar { get => offsetChar; set => offsetChar = value; }
    }
}
