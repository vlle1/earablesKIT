namespace EarablesKIT.Models.Library
{
    /// <summary>
    /// This class contains all information about the rotation of the earables
    /// </summary>
    public class Gyroscope
    {
        private float degPerSec_X;
        public float DegsPerSec_X { get => degPerSec_X; }
        private float degPerSec_Y;
        public float DegsPerSec_Y { get => degPerSec_Y; }
        private float degPerSec_Z;
        public float DegsPerSec_Z { get => degPerSec_Z; }

        /// <summary>
        /// Constructor to set all values at once
        /// </summary>
        /// <param name="degPerSec_X"> The rotation around the X axis in deg per s </param>
        /// <param name="degPerSec_Y"> The rotation around the Y axis in deg per s </param>
        /// <param name="degPerSec_Z"> The rotation around the Z axis in deg per s </param>
        public Gyroscope (float degPerSec_X, float degPerSec_Y, float degPerSec_Z )
        {
            this.degPerSec_X = degPerSec_X;
            this.degPerSec_Y = degPerSec_Y;
            this.degPerSec_Z = degPerSec_Z;
        }
    }
}