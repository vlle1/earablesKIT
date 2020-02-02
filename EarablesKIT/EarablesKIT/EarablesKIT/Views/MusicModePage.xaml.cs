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
            //HIER SOLLTE DAS VIEWMODEL GECALLED WERDEN UND DEN MUSIKMODUS STOPPEN
            //todo: call music view model
        }
    }
}