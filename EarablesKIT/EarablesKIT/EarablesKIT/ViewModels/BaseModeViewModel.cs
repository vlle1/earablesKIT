using EarablesKIT.Models.Extentionmodel.Activities;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    public abstract class BaseModeViewModel
    {

        public abstract void OnActivityDone(object sender, ActivityArgs args);

        public abstract bool StartActivity(); 

        public abstract void StopActivity();

        protected bool CheckConnection()
        {
			//if (ScanningPopUpViewModel.IsConnected)
			//{
				return true;
			//}
			//else
			//{
			//	ScanningPopUpViewModel.ShowPopUp();
			//	return false;
			//}
		}

    }
}
