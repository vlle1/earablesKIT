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
	public partial class ListenAndPerformPage : ContentPage
	{

		private ListenAndPerformViewModel ViewModel { get; set; }
		public ListenAndPerformPage()
		{
			InitializeComponent();
			ViewModel = new ListenAndPerformViewModel();
			BindingContext = ViewModel;

		}

		public async void OnStartButtonClicked(object sender, EventArgs args) //Async gemacht, change View rückgabe geändert
		{
			if (ViewModel.StartActivity())
			{
				ViewModel.StartTimer();
				await ChangeView();
				
				
			}
		}

		public async Task ChangeView()
		{
			ListenAndPerformActivePage NewView = new ListenAndPerformActivePage(ViewModel);
			NewView.BindingContext = this.BindingContext;
			await Navigation.PushModalAsync(NewView);
		}

	}
}