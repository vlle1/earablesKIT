using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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

        /// <summary>
        /// Constructor, who initializes the Components and sets up the Picker values
        /// </summary>
        public SettingsPage()
        {
            InitializeComponent();
            LanguagePicker.ItemsSource = new List<CultureInfo> {new CultureInfo("de-De"), new CultureInfo("en-US")};
            SamplingratePicker.ItemsSource = Enum.GetValues(typeof(SamplingRate)).Cast<int>().ToList();
            BindingContext = _viewModel = new SettingsViewModel();
            InformationLabel.IsVisible = false;
            SaveButton.IsEnabled = false;
        }


        private void ButtonSavedClicked(object sender, EventArgs eventArgs)
        {
            bool savingCompleted = UsernameEntry.Text != null && Regex.IsMatch(UsernameEntry.Text, @"^\w+$");
            savingCompleted = savingCompleted && _viewModel.SaveClicked(UsernameEntry.Text,
                                  int.Parse(SteplengthEntry.Text), (SamplingRate) SamplingratePicker.SelectedItem,
                                  (CultureInfo) LanguagePicker.SelectedItem);

            //Check if saving was completed correctly
            if (!savingCompleted)
            {
                //An error appeared
                InformationLabel.Text = AppResources.ErrorAlert + " " + AppResources.SettingsPageWrongInput;
                InformationFrame.BackgroundColor = Color.LightCoral;
            }
            else
            {
                //Saving completed without an error
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

            //Check if the username only contains word characters
            if (UsernameEntry.Text != null && !Regex.IsMatch(UsernameEntry.Text, @"^\w+$"))
            {
                UsernameEntry.TextColor = Color.Red;
                SaveButton.IsEnabled = false;
            }
            else
            {
                SaveButton.IsEnabled = true;
                UsernameEntry.TextColor = Color.Black;
            }
        }

        private void SteplengthEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if(SteplengthEntry.Text == null) return;


            //Check if text contains only numbers and is not 0
            if (!Regex.IsMatch(SteplengthEntry.Text, @"^\d+$") || int.Parse(SteplengthEntry.Text) == 0)
            {
                SteplengthEntry.TextColor = Color.Red;
                SaveButton.IsEnabled = false;
            }
            else
            {
                SaveButton.IsEnabled = true;
                SteplengthEntry.Text = int.Parse(SteplengthEntry.Text) + "";
                SteplengthEntry.TextColor = Color.Black;
            }
        }
    }
}