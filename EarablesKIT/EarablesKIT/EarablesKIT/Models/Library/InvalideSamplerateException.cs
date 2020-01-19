using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    class InvalideSamplerateException : Exception
    {
        public InvalideSamplerateException(string message): base(message)
        {

        }
    }
}
