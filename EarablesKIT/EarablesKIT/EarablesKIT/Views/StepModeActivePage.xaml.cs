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
		public StepModeActivePage()
		{
			InitializeComponent();
			BindingContext = new StepModeViewModel();
		}

		public async void ShowPopUp()
		{
			await DisplayAlert("Result", "You have done " + "" + " Steps!.", "Cool");
		}

		public void changeView()
		{
			StepModePage NeueView = new StepModePage();
			NeueView.BindingContext = this.BindingContext;
			Navigation.PushAsync(NeueView);
		}
	}
}