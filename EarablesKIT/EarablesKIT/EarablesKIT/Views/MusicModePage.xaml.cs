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
        }
        /// <summary>
        /// This method is used to stop the Logic of the Music Mode if the User navigates to a different page.
        /// The Page should be set to its deafault state (not activated, music not running)
        /// </summary>
        public void forceStopOnPageChange()
        {

            //debug:
            Application.Current.MainPage.DisplayAlert("I did not hit her!", "It's not true! It's bullshit, I did not hit her! I did not!","Oh hi mark!");
            //todo: call music view model
        }
    }
}