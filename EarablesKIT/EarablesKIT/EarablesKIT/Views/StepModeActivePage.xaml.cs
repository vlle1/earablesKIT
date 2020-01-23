using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EarablesKIT.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StepModeActivePage : ContentPage
	{
		private StepModeViewModel ViewModel { get; set; }
		public StepModeActivePage(StepModeViewModel smvm)
		{
			InitializeComponent();
			ViewModel = smvm;
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