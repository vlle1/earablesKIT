using EarablesKIT.ViewModels;
using Xamarin.Forms;
using EarablesKIT.Views;

namespace EarablesKIT
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
            System.Threading.Thread.Sleep(2000);
            ScanningPopUpViewModel.ShowPopUp();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
