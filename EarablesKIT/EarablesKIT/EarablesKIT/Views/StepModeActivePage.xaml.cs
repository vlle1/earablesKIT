using System;
using EarablesKIT.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Views
{
	/// <summary>
	/// Codebehind class of the StepModeActivePage.
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StepModeActivePage : ContentPage
	{
		/// <summary>
		/// ViewModel that will be the Binding Context of this class.
		/// </summary>
		private StepModeViewModel ViewModel { get; set; }

		/// <summary>
		/// Initializes the ViewModel.
		/// </summary>
		/// <param name="smvm">Binding Context of the passive page</param>
		public StepModeActivePage(StepModeViewModel smvm)
		{
			InitializeComponent();
			ViewModel = smvm;
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