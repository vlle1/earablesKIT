using Xamarin.Forms;
using EarablesKIT.Views;
using MediaManager;

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
            PlayAudio();
        }

        private async void PlayAudio()
        {
            // TODO: move this to an appropriate location
            await CrossMediaManager.Current.Play("https://ia800806.us.archive.org/15/items/Mp3Playlist_555/AaronNeville-CrazyLove.mp3");
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
