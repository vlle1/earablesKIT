using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		}

		public async void ShowPopUp()
		{
			await DisplayAlert("Result", "You have done " + "" + " Steps!.", "Cool");
		}
	}
}