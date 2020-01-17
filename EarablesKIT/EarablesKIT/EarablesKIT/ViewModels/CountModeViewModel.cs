using EarablesKIT.Models.Extentionmodel.Activities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace EarablesKIT.ViewModels
{
    class CountModeViewModel : BaseModeViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string SelectedActivity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ObservableCollection<string> PossibleActivities { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Minutes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Seconds { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Milliseconds { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Counter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        
        public CountModeViewModel()
        {
            throw new NotImplementedException();
        }

        
        public override void OnActivityDone(object sender, ActivityArgs args)
        {
            throw new NotImplementedException();
        }

        public override void StartActivity()
        {
            throw new NotImplementedException();
        }

        public override void StopActivity()
        {
            throw new NotImplementedException();
        }

        private void StartTimer()
        {
            throw new NotImplementedException();
        }

        private void StopTimer()
        {
            throw new NotImplementedException();
        }

        private void ShowPopUp()
        {
            throw new NotImplementedException();
        }
    }
}
