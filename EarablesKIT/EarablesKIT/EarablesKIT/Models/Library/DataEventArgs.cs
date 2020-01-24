using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    public class DataEventArgs
    {
        // Brauch man hier getter und setter? Man kann die doch einfach weglassenoder zummindest die setter weil sie nur im Constrictor gesetzt werden

        public IMUDataEntry Data; //{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ConfigContainer Configs; // { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public DataEventArgs(IMUDataEntry data, ConfigContainer configs)
        {
            Data = data;
            Configs = configs;
        }
    }
}
