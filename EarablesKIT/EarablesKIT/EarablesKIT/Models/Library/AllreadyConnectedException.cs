using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    class AllreadyConnectedException : Exception
    {
            public AllreadyConnectedException(string message) : base(message)
            {

            }
    }
}
