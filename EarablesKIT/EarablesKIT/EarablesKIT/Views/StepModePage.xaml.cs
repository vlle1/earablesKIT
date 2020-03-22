using System;
using EarablesKIT.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Views
{
	/// <summary>
	/// Codebehind class of the StepModePage. 
	/// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StepModePage : ContentPage
    {
		/// <summary>
		/// ViewModel which will be the Binding Context for this page and its equivalent active page.
		/// </summary>
		private StepModeViewModel ViewModel { get; set; }

		/// <summary>
		/// Sets the Binding Context.
		/// </summary>
        public StepModePage()
        {
            InitializeComponent();
			ViewModel = new StepModeViewModel();
			BindingContext = ViewModel; 
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
			}
		}

		/// <summary>
		/// Changes the view to active and sets the Binding Context of the new page.
		/// </summary>
		public async void ChangeView()
		{
			StepModeActivePage NewView = new StepModeActivePage(ViewModel);
			NewView.BindingContext = this.BindingContext;
			await Navigation.PushModalAsync(NewView);
		}
    }
}