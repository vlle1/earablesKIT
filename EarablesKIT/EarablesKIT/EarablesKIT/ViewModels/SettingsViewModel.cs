using EarablesKIT.Models.SettingsService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private User _user;

        public string Username { get => _user.Username;}
        public int Steplength { get => _user.Steplength;}
        public SamplingRate SamplingRate { get; set; }
        public CultureInfo Language { get; set; }
        public ICommand SaveClickedCommand => new Command(SaveClicked);

        private void SaveClicked()
        {
            _user = new User("Bob", 20);
            SamplingRate = SamplingRate.Hz_50;
            Language = new CultureInfo("de-De");
        }
    }
}
