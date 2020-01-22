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
	public partial class CountModeActivePage : ContentPage
	{
		private CountModeViewModel ViewModel { get; set; }
		public CountModeActivePage(CountModePage cmp)
		{
			InitializeComponent();
			ViewModel = (CountModeViewModel) cmp.ThrowingViewModel();
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