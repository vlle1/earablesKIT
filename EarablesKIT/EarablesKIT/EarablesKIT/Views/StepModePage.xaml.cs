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
		StepModeViewModel ViewModel { get; set; }
        public StepModePage()
        {
            InitializeComponent();
			ViewModel = new StepModeViewModel();
			BindingContext = ViewModel; 
        }

		public void OnStartButtonClicked(object sender, EventArgs args)
		{
			ViewModel.StartActivity(); //bool
			ChangeView();
			ViewModel.HandlingTimer();
		}


		public async void ChangeView()
		{
			StepModeActivePage NewView = new StepModeActivePage(this);
			NewView.BindingContext = this.BindingContext;
			await Navigation.PushModalAsync(NewView);
		}

		public object ThrowingViewModel()
		{
			return ViewModel;
		}
    }
}