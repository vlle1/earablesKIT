using System;

namespace EarablesKIT.Models.Library
{
    class ConnectionFailedException : Exception
    {
        public ConnectionFailedException(string message) : base(message)
        {

        }
    }
}
