using EarablesKIT.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Views
{
    /// <summary>
    /// Codebehind class of ListenAndPerformActivePage.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListenAndPerformActivePage : ContentPage
    {
        /// <summary>
        /// ViewModel that will be the Binding Context of this class.
        /// </summary>
        private ListenAndPerformViewModel ViewModel { get; set; }

        /// <summary>
        /// Initializes the ViewModel.
        /// </summary>
        /// <param name="lapvm">Binding Context of the passive page</param>
        public ListenAndPerformActivePage(ListenAndPerformViewModel lapvm)
        {
            ViewModel = lapvm;
            InitializeComponent();
        }

        /// <summary>
        /// Bound to the Clicked event of the Stop Button. Delegates to the ViewModel and changes the view to passive.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="args">Ignored</param>
        public void OnStopButtonClicked(object sender, EventArgs args)
        {
            ViewModel.StopActivity();
            ChangeView();
        }

        /// <summary>
        /// Changes the view to passive.
        /// </summary>
        public async void ChangeView()
        {
            await Navigation.PopModalAsync();
        }
    }
}