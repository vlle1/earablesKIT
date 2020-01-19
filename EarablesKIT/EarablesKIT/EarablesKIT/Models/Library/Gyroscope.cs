using System;

namespace EarablesKIT.Models.Library
{
    public class Gyroscope
    {
        private float degPerSec_X;
        public float DegsPerSec_X { get => degPerSec_X; }
        private float degPerSec_Y;
        public float DegsPerSec_Y { get => degPerSec_Y; }
        private float degPerSec_Z;
        public float DegsPerSec_Z { get => degPerSec_Z; }

        public Gyroscope (float degPerSec_X, float degPerSec_Y, float degPerSec_Z )
        {
            this.degPerSec_X = degPerSec_X;
            this.degPerSec_Y = degPerSec_Y;
            this.degPerSec_Z = degPerSec_Z;
        }
    }
}