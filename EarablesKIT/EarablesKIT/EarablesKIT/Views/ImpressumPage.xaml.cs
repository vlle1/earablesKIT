using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Views
{
    /// <summary>
    /// Impressum page containing the logic to open a link
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImpressumPage : ContentPage
    {
        /// <summary>
        /// Constructor of the Impressum page
        /// </summary>
        public ImpressumPage()
        {
            InitializeComponent();
        }


        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            Browser.OpenAsync(new Uri("https://esense.io/"), BrowserLaunchMode.External);
        }
    }
}