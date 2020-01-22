using EarablesKIT.Models.SettingsService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using EarablesKIT.Resources;
using Xamarin.Forms;
using static EarablesKIT.Models.SettingsService.SamplingRate;

namespace EarablesKIT.ViewModels
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private User _user;

        private ISettingsService _settingsService;

        public string Username { get => _user.Username;}
        public int Steplength { get => _user.Steplength;}

        private SamplingRate _samplingrate;
        public int SamplingRate => (int) _samplingrate;

        public CultureInfo Language { get; private set; }

        public SettingsViewModel()
        {
            _user = new User("Bob", 20);
            _samplingrate = Hz_50;
            Language = new CultureInfo("de-De");
        }


        public bool SaveClicked(string chosenUsername, int chosenSteplength, SamplingRate chosenSamplingRate, CultureInfo chosenCultureInfo)
        {
            bool needToSave = false;
            if (!chosenUsername.Equals(Username) || chosenSteplength != Steplength)
            {
                try
                {
                    _settingsService.ActiveUser = new User(chosenUsername, chosenSteplength);
                    _user = _settingsService.ActiveUser;
                    needToSave = true;
                }
                catch (Exception e)
                {
                    ExceptionHandlingViewModel.HandleException(e);
                    needToSave = false;
                }
            }

            if (_samplingrate != chosenSamplingRate)
            {
                try
                {
                    //_settingsService.SamplingRate = _samplingrate;
                    _samplingrate = chosenSamplingRate;
                    needToSave = true;
                }
                catch (Exception e)
                {
                    ExceptionHandlingViewModel.HandleException(e);
                    needToSave = false;
                }
            }

            if (!Equals(Language, chosenCultureInfo))
            {
                //_settingsService.ActiveLanguage = chosenCultureInfo;
                Language = chosenCultureInfo;
                needToSave = true;
            }
            return needToSave;
        }
    }
}
