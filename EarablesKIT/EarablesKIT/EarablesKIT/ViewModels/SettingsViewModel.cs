using EarablesKIT.Models;
using EarablesKIT.Models.SettingsService;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using EarablesKIT.Annotations;

namespace EarablesKIT.ViewModels
{
    /// <summary>
    /// Class SettingsViewModel contains the logic behind the Settingspage. Builds the connection to the <see cref="SettingsService"/>
    /// </summary>
    public class SettingsViewModel : INotifyPropertyChanged
    {

        private User _user;

        private ISettingsService _settingsService;

        /// <summary>
        /// The active username
        /// </summary>
        public string Username { get => _user.Username; }
        /// <summary>
        /// The active steplength of the user
        /// </summary>
        public int Steplength { get => _user.Steplength; }

        private SamplingRate _samplingrate;
        /// <summary>
        /// The samplingrate of the earables
        /// </summary>
        public int SamplingRate => (int)_samplingrate;

        /// <summary>
        /// The active language
        /// </summary>
        public CultureInfo Language { get; private set; }

        /// <summary>
        /// Constructor for SettingsViewModel. Loads the Settings (username, steplength, samplingrate, language)
        /// </summary>
        public SettingsViewModel()
        {
            _settingsService = (ISettingsService)ServiceManager.ServiceProvider.GetService(typeof(ISettingsService));
            _user = _settingsService.ActiveUser;
            _samplingrate = _settingsService.SamplingRate;
            Language = _settingsService.ActiveLanguage;
        }

        /// <summary>
        /// Method SaveClicked gets called when the "Save button" is clicked. It tries to save the
        /// new settings given as the arguments. Returns true if the saving was completed without an exception; false otherwise
        /// </summary>
        /// <param name="chosenUsername">The new username</param>
        /// <param name="chosenSteplength">The new steplength</param>
        /// <param name="chosenSamplingRate">The new Samplingrate</param>
        /// <param name="chosenCultureInfo">The new Language</param>
        /// <returns>Bool, if the saving was completed without an exception</returns>
        public bool SaveClicked(string chosenUsername, int chosenSteplength, SamplingRate chosenSamplingRate, CultureInfo chosenCultureInfo)
        {
            bool needToSave = false;

            //Check if the username or steplength have changed
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


            //Check if the samplingrate has changed
            if (_samplingrate != chosenSamplingRate)
            {
                try
                {
                    _settingsService.SamplingRate = chosenSamplingRate;
                    _samplingrate = chosenSamplingRate;
                    needToSave = true;
                }
                catch (Exception e)
                {
                    ExceptionHandlingViewModel.HandleException(e);
                    needToSave = false;
                }
            }

            //Check if the language has changed
            if (!Equals(Language, chosenCultureInfo))
            {
                _settingsService.ActiveLanguage = chosenCultureInfo;
                Language = chosenCultureInfo;
                needToSave = true;
            }
            return needToSave;
        }

        public void OnAppearing(object sender, EventArgs e)
        {
            _user = _settingsService.ActiveUser;
            _samplingrate = _settingsService.SamplingRate;
            Language = _settingsService.ActiveLanguage;
            OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(_samplingrate));
            OnPropertyChanged(nameof(Language));
            OnPropertyChanged(nameof(Steplength));


        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}