using EarablesKIT.Models.Extentionmodel.Activities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    class ListenAndPerformViewModel : BaseModeViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Minutes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Seconds { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Milliseconds { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ObservableCollection<string> ActivityList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ObservableCollection<int> ActivityAmounts { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string SelectedActivity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Command AddActivityCommand { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Command RemoveActivityCommand { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Command EditActivityCommand { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public ListenAndPerformViewModel()
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

    }
}
