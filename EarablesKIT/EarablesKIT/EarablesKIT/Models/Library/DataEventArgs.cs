using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    public class DataEventArgs
    {
        public IMUDataEntry Data { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ConfigContainer Configs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public DataEventArgs(IMUDataEntry data, ConfigContainer configs)
        {
            throw new NotImplementedException();
        }
    }
}
