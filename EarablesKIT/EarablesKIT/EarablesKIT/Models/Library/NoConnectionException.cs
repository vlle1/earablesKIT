using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    /// <summary>
    /// This exception is thrown if someone tries to comunicate with the eararbles, but is not connected
    /// </summary>
    public class NoConnectionException : Exception
    {
        public NoConnectionException(string message) : base(message)
        {

        }
    }
}
