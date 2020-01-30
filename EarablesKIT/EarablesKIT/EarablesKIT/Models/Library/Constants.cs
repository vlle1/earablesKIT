namespace EarablesKIT.Models.Library
{
    /// <summary>
    /// This class contains the Guid parse strings for the characteristics
    /// </summary>
    class Constants
    {
        public static string START_STOP_IMU_SAMPLING_CHAR { get => "0000ff07-0000-1000-8000-00805f9b34fb"; } 
        public static string SENSORDATA_CHAR { get => "0000ff08-0000-1000-8000-00805f9b34fb"; }
        public static string PUSHBUTTON_CHAR { get => "0000ff09-0000-1000-8000-00805f9b34fb"; }
        public static string BATTERY_CHAR { get => "0000ff0a-0000-1000-8000-00805f9b34fb"; }
        public static string ACC_GYRO_LPF_CHAR { get => "0000ff0e-0000-1000-8000-00805f9b34fb"; }
        public static string OFFSET_CHAR { get => "0000ff0d-0000-1000-8000-00805f9b34fb"; }
        public static string ACCES_SERVICE { get => "0000ff06-0000-1000-8000-00805f9b34fb"; }
    }
}
