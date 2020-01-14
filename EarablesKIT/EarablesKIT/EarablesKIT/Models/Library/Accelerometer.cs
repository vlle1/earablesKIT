using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    class Accelerometer
    {
        private float mperS_X;
        public float MperS_X { get => mperS_X; set => mperS_X = value; }
        private float mperS_Y;
        public float MperS_Y { get => mperS_Y; set => mperS_Y = value; }
        private float mperS_Z;
        public float MperS_Z { get => mperS_Z; set => mperS_Z = value; }
        private float g_X;
        public float G_X { get => g_X; set => g_X = value; }
        private float g_Y;
        public float G_Y { get => g_Y; set => g_Y = value; }
        private float g_Z;
        public float G_Z { get => g_Z; set => g_Z = value; }
    }
}
