using EarablesKIT.Models.Extentionmodel.Activities;
using System;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    abstract class BaseModeViewModel
    {

        public Command StartActivityCommand { get; set; }
        public Command StopActivityCommand { get; set; }



        public abstract void OnActivityDone(object sender, ActivityArgs args);

        protected abstract void StartActivity();

        protected abstract void StopActivity();

        protected void CheckConnection()
        {
            throw new NotImplementedException();
        }

    }
}
