using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    class ConnectionFailedException : Exception
    {
        public ConnectionFailedException(string message) : base(message)
        {

        }
    }
}
