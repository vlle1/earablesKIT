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
	public partial class ListenAndPerformActivePage : ContentPage
	{
		private ListenAndPerformViewModel ViewModel { get; set; }
		public ListenAndPerformActivePage(ListenAndPerformViewModel lapvm)
		{
			ViewModel = lapvm;
			InitializeComponent();
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