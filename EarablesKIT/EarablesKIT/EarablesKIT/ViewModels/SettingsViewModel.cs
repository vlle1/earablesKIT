using EarablesKIT.Models.SettingsService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Username { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Steplength { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public SamplingRate SamplingRate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public CultureInfo Language { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Command ClickSaveCommand { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
