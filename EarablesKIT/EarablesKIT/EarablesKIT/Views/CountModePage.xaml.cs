using EarablesKIT.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Views
{
    /// <summary>
    /// Codebehind class of CountModePage.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CountModePage : ContentPage
    {
        /// <summary>
        /// ViewModel which will be the Binding Context for this page and its equivalent active page.
        /// </summary>
        CountModeViewModel ViewModel { get; set; }

        /// <summary>
        /// Sets the Binding Context.
        /// </summary>
        public CountModePage()
        {
            InitializeComponent();
            ViewModel = new CountModeViewModel();
            BindingContext = ViewModel;
            ActivityView.SelectedItem = ViewModel.PossibleActivities[0];
        }

        /// <summary>
        /// Bound to the Clicked event of the Start Button. Delegates to the ViewModel and changes the view to active.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="args">Ignored</param>
        public void OnStartButtonClicked(object sender, EventArgs args)
        {
            if (ViewModel.StartActivity())
            {
                ChangeView();
                ViewModel.StartTimer();
            }
        }

        /// <summary>
        /// Changes the view to active and sets the Binding Context of the new page.
        /// </summary>
        public async void ChangeView()
        {
            CountModeActivePage NewView = new CountModeActivePage(ViewModel);
            NewView.BindingContext = this.BindingContext;
            await Navigation.PushModalAsync(NewView);
        }
    }
}