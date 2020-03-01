using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

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
            System.Threading.Thread.Sleep(2000); // Simulate a long pause    
            RunOnUiThread(() => StartActivity(typeof(MainActivity)));
        }
    }
}