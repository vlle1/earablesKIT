using System;

namespace EarablesKIT.Models.Library
{
    public class Gyroscope
    {
        private float degPerSec_X;
        public float DegsPerSec_X { get => degPerSec_X; set => degPerSec_X = value; }
        private float degPerSec_Y;
        public float DegsPerSec_Y { get => degPerSec_Y; set => degPerSec_Y = value; }
        private float degPerSec_Z;
        public float DegsPerSec_Z { get => degPerSec_Z; set => degPerSec_Z = value; }
    }
}