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
    public partial class StepModePage : ContentPage
    {
        public StepModePage()
        {
            InitializeComponent();
			var ViewModel = new StepModeViewModel();
			BindingContext = ViewModel; //geändert
        }

		public void changeView()
		{
			StepModeActivePage NeueView = new StepModeActivePage();
			NeueView.BindingContext = this.BindingContext;
			Navigation.PushAsync(NeueView);
		}
    }
}