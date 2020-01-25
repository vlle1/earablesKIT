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

		public void OnStartButtonClicked(object sender, EventArgs args)
		{
			if (ViewModel.StartActivity())
			{
				ChangeView();
				ViewModel.StartTimer();
				ViewModel.DoActivities();
			}
		}

		public async void ChangeView()
		{
			ListenAndPerformActivePage NewView = new ListenAndPerformActivePage(ViewModel);
			NewView.BindingContext = this.BindingContext;
			await Navigation.PushModalAsync(NewView);
		}

	}
}