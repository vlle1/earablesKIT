using System;
using EarablesKIT.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CountModeActivePage : ContentPage
	{
		private CountModeViewModel ViewModel { get; set; }
		public CountModeActivePage(CountModeViewModel cmvm)
		{
			InitializeComponent();
			ViewModel = cmvm;
		}

		public void OnStopButtonClicked(object sender, EventArgs args)
		{
			ViewModel.StopActivity();
			ChangeView();
		}

		public async void ChangeView()
		{
			await Navigation.PopModalAsync();
		}

	}
}