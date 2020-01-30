namespace EarablesKIT.Models.Library
{
    /// <summary>
    /// This class contains all information about the acceleration of the earables
    /// </summary>
    public class Accelerometer
    {
        private float mperS_X;
        public float MperS_X { get => mperS_X; }
        private float mperS_Y;
        public float MperS_Y { get => mperS_Y; }
        private float mperS_Z;
        public float MperS_Z { get => mperS_Z; }
        private float g_X;
        public float G_X { get => g_X; }
        private float g_Y;
        public float G_Y { get => g_Y; }
        private float g_Z;
        public float G_Z { get => g_Z; }

        /// <summary>
        /// Constructor to set all values at once
        /// </summary>
        /// <param name="g_X"> The accleration in X directory in g </param>
        /// <param name="g_Y"> The accleration in Y directory in g </param>
        /// <param name="g_Z"> The accleration in Z directory in g </param>
        /// <param name="mperS_X"> The accleration in X directory in meter per sec^2 </param>
        /// <param name="mperS_Y"> The accleration in Y directory in meter per sec^2 </param>
        /// <param name="mperS_Z"> The accleration in Z directory in meter per sec^2 </param>
        public Accelerometer(float g_X, float g_Y, float g_Z, float mperS_X, float mperS_Y, float mperS_Z )
        {
            this.g_X = g_X;
            this.g_Y = g_Y;
            this.g_Z = g_Z;
            this.mperS_X = mperS_X;
            this.mperS_Y = mperS_Y;
            this.mperS_Z = mperS_Z;
        }
    }
}
