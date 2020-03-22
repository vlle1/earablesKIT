using EarablesKIT.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Views
{
    /// <summary>
    /// Codebehind class of ListenAndPerform.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListenAndPerformPage : ContentPage
    {
        /// <summary>
        /// ViewModel which will be the Binding Context for this page and its equivalent active page.
        /// </summary>
        private ListenAndPerformViewModel ViewModel { get; set; }

        /// <summary>
        /// Sets the Binding Context.
        /// </summary>
        public ListenAndPerformPage()
        {
            InitializeComponent();
            ViewModel = new ListenAndPerformViewModel();
            BindingContext = ViewModel;
            ActivityView.SelectedItem = ViewModel.ActivityList[0];
        }

        /// <summary>
        /// Bound to the Clicked event of the Start Button. Delegates to the ViewModel and changes the view to active.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="args">Ignored</param>
        public void OnStartButtonClicked(object sender, EventArgs args) //Async weggemacht, change View rückgabe auf void
        {
            if (ViewModel.StartActivity())
            {
                ViewModel.StartTimer();
                ChangeView();
            }
        }

        /// <summary>
        /// Changes the view to active and sets the Binding Context of the new page.
        /// </summary>
        public async void ChangeView()
        {
            ListenAndPerformActivePage NewView = new ListenAndPerformActivePage(ViewModel);
            NewView.BindingContext = this.BindingContext;
            await Navigation.PushModalAsync(NewView);
        }

    }
}