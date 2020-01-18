using EarablesKIT.Views;
using Xamarin.Forms;

namespace EarablesKIT
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            //Register eventmethode for ScanningPopUpViewModel
            //TODO uncomment
            //IEarablesConnection service = (IEarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection));
            //service.DeviceConnectionStateChanged += ScanningPopUpViewModel.OnDeviceConnectionStateChanged;
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}