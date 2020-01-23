using System;
using EarablesKIT.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CountModePage : ContentPage
    {
		CountModeViewModel ViewModel { get; set; }
        public CountModePage()
        {
            InitializeComponent();
			ViewModel = new CountModeViewModel();
			BindingContext = ViewModel;
        }

		public void OnStartButtonClicked(object sender, EventArgs args)
		{
			if (ViewModel.StartActivity())
			{
				ChangeView();
				ViewModel.StartTimer();
			}
		}

		public async void ChangeView()
		{
			CountModeActivePage NewView = new CountModeActivePage(ViewModel);
			NewView.BindingContext = this.BindingContext;
			await Navigation.PushModalAsync(NewView);
		}
	}
}