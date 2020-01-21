using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    class NoConnectionException : Exception
    {
        public NoConnectionException(string message) : base(message)
        {

        }
    }
}
