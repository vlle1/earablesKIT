using EarablesKIT.Models;
using EarablesKIT.Models.PopUpService;
using EarablesKIT.Resources;
using System;

namespace EarablesKIT.ViewModels
{
    /// <summary>
    /// Class ExceptionHandlingViewModel notifies the user via a PopUp about an exception.
    /// </summary>
    public class ExceptionHandlingViewModel : IExceptionHandler
    {
        /// <summary>
        /// Method HandleException displays a PopUp with the given error message.
        /// </summary>
        /// <param name="Error">The thrown exception which gets displayed</param>
        public void HandleException(Exception Error)
        {

            if (Error?.Message != null && Error.Message.Length != 0)
            {
                ((IPopUpService)ServiceManager.ServiceProvider.GetService(typeof(IPopUpService))).DisplayAlert(
                    AppResources.ErrorAlert, Error.Message.Trim(), AppResources.Okay);
            }
            else
            {
                HandleException();
            }
        }

        /// <summary>
        /// Method HandleException displays a PopUp containing the default error message.
        /// </summary>
        public void HandleException()
        {
            ((IPopUpService)ServiceManager.ServiceProvider.GetService(typeof(IPopUpService))).DisplayAlert(
                AppResources.ErrorAlert, AppResources.DefaultError, AppResources.Okay);
        }
    }

    public interface IExceptionHandler
    {

        void HandleException(Exception Error);

        void HandleException();
    }
}