using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{    /// <summary>
     /// This exception is thrown if the checksum does not match with the values
     /// </summary>
    public class InvalidChecksumException : Exception
    {
        public InvalidChecksumException(string message) : base(message)
        {

        }
    }
}
