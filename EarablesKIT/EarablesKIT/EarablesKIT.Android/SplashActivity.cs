using Android.App;
using Android.OS;

namespace EarablesKIT.Droid
{
    [Activity(Label= "eSense Fitness", Theme = "@style/Theme.Splash", Icon = "@mipmap/icon", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.SplashLayout);
            System.Threading.ThreadPool.QueueUserWorkItem(o => LoadActivity());
        }

        private void LoadActivity()
        {
            System.Threading.Thread.Sleep(500); // Simulate a long pause    
            RunOnUiThread(() => StartActivity(typeof(MainActivity)));
        }

    }
}