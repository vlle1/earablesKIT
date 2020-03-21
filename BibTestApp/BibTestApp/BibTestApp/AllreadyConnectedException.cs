using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    /// <summary>
    /// This exception is thrown if someone tries to connect to a device while a connection allready exists
    /// </summary>
    public class AllreadyConnectedException : Exception
    {
        public AllreadyConnectedException(string message) : base(message)
        {

        }
    }
}
