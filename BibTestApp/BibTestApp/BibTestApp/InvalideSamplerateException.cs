using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    /// <summary>
    /// This exception is thrown if the samplerate is not in the valid interval from 1 to 100
    /// </summary>
    public class InvalideSamplerateException : Exception
    {
        public InvalideSamplerateException(string message): base(message)
        {

        }
    }
}
