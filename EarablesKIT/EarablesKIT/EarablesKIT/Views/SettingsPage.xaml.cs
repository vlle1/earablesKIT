using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using EarablesKIT.Models.SettingsService;
using EarablesKIT.Resources;
using EarablesKIT.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private SettingsViewModel _viewModel;

        public SettingsPage()
        {

            InitializeComponent();
            LanguagePicker.ItemsSource = new List<CultureInfo> {new CultureInfo("de-De"), new CultureInfo("en-US")};
            SamplingratePicker.ItemsSource = Enum.GetValues(typeof(SamplingRate)).Cast<int>().ToList();
            BindingContext = _viewModel = new SettingsViewModel();
            InformationLabel.IsVisible = false;
        }


        private void ButtonSavedClicked(object sender, EventArgs eventArgs)
        {
            if(!_viewModel.SaveClicked(UsernameEntry.Text, int.Parse(SteplengthEntry.Text), (SamplingRate) SamplingratePicker.SelectedItem, (CultureInfo)LanguagePicker.SelectedItem))
            {
                InformationLabel.Text = AppResources.ErrorAlert + " " + AppResources.SettingsPageWrongInput;
                InformationFrame.BackgroundColor = Color.LightCoral;
            }
            else
            {
                InformationLabel.Text = AppResources.SettingsPageSavingDone;
                InformationFrame.BackgroundColor = Color.LightGreen;
            }

            InformationLabel.IsVisible = true;
            Device.StartTimer(new TimeSpan(0,0,2), () =>
            {

                InformationFrame.BackgroundColor = Color.White;
                return InformationLabel.IsVisible = false;
            });
        }

        private void UsernameEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            
            if (UsernameEntry.Text != null && !Regex.IsMatch(UsernameEntry.Text, @"^\w+$"))
            {
                UsernameEntry.TextColor = Color.Red;
            }
            else
            {
                UsernameEntry.TextColor = Color.Black;
            }
        }

        private void SteplengthEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (SteplengthEntry.Text != null && !Regex.IsMatch(SteplengthEntry.Text, @"^\d+$"))
            {
                SteplengthEntry.TextColor = Color.Red;
                SaveButton.IsEnabled = false;
            }
            else
            {
                SteplengthEntry.Text = int.Parse(SteplengthEntry.Text) + "";
                SteplengthEntry.TextColor = Color.Black;
            }
        }
    }
}