using EarablesKIT.Models.Extentionmodel.Activities;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    abstract class BaseModeViewModel
    {

        public abstract void OnActivityDone(object sender, ActivityArgs args);

        public abstract void StartActivity(); 

        public abstract void StopActivity();

        protected void CheckConnection()
        {
			if (!ScanningPopUpViewModel.IsConnected)
			{
				// ShowScanningPopUp
			}
        }

    }
}
