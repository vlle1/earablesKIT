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
		private StepModePage _StepModePage { get; set; }
		private StepModeViewModel ViewModel { get; set; }
		public StepModeActivePage(StepModePage smp)
		{
			InitializeComponent();
			_StepModePage = smp;
			ViewModel = (StepModeViewModel) smp.ThrowingViewModel();
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