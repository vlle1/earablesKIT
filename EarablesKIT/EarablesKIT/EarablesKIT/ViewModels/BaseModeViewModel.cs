using EarablesKIT.Models.Extentionmodel.Activities;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    abstract class BaseModeViewModel
    {

        public ICommand StartActivityCommand { get; set; }
        public ICommand StopActivityCommand { get; set; }

        public abstract void OnActivityDone(object sender, ActivityArgs args);

        protected abstract void StartActivity();

        protected abstract void StopActivity();

        protected void CheckConnection()
        {
			if (!ScanningPopUpViewModel.IsConnected)
			{
				// ShowScanningPopUp
			}
        }

    }
}
