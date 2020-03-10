using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    /// <summary>
    /// This exception is thrown if the checksum is incorrect
    /// </summary>
    class ChecksumException : Exception
    {
        public ChecksumException(string message): base(message)
        {

        }
    }
}
