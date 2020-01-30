namespace EarablesKIT.Models.Library
{
    /// <summary>
    /// This class contains all arguments which are necessary if there are new IMUDates
    /// </summary>
    public class DataEventArgs
    {
        private IMUDataEntry data;
        public IMUDataEntry Data { get => data; set => data = value; }
        private ConfigContainer configs;

        public ConfigContainer Configs { get => configs; set => configs = value; }

        public DataEventArgs(IMUDataEntry data, ConfigContainer configs)
        {
            Data = data;
            Configs = configs;
        }
    }
}
