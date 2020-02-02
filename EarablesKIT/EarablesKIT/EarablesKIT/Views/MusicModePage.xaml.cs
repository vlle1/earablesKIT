using EarablesKIT.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MusicModePage : ContentPage
    {
        public MusicModePage()
        {
            InitializeComponent();
            this.BindingContext = new MusicModeViewModel();
        }
        /// <summary>
        /// This method is used to stop the Logic of the Music Mode if the User navigates to a different page.
        /// The Page should be set to its deafault state (not activated, music not running)
        /// </summary>
        public void forceStopOnPageChange()
        {

            //debug: Application.Current.MainPage.DisplayAlert("Info", "Stopping the MusicMode...", "OK");
            ((MusicModeViewModel)BindingContext).StopActivity();
        }
    }
}