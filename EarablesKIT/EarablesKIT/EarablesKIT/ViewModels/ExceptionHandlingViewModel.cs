using EarablesKIT.Resources;
using System;

namespace EarablesKIT.ViewModels
{
    /// <summary>
    /// Class ExceptionHandlingViewModel notifies the user via a PopUp about an exception. 
    /// </summary>
    class ExceptionHandlingViewModel
    {
        /// <summary>
        /// Method HandleException displays a PopUp with the given error message. 
        /// </summary>
        /// <param name="Error">The thrown exception which gets displayed</param>
        public static void HandleException(Exception Error)
        {
            if(Error != null && Error.Message != null && Error.Message.Length != 0)
            {
                App.Current.MainPage.DisplayAlert(AppResources.ErrorAlert, Error.Message.Trim(), AppResources.Okay);
            }
            else
            {
                App.Current.MainPage.DisplayAlert(AppResources.ErrorAlert, AppResources.DefaultError, AppResources.Okay);
            }

        }
           
        /// <summary>
        /// Method HandleException displays a PopUp containing the default error message.
        /// </summary>
        public static void HandleException()
        {
            App.Current.MainPage.DisplayAlert(AppResources.ErrorAlert, AppResources.DefaultError, AppResources.Okay);
        }

    }
}
