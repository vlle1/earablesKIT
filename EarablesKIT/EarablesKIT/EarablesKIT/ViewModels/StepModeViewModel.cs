using EarablesKIT.Models.Extentionmodel.Activities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EarablesKIT.ViewModels
{
    class StepModeViewModel : BaseModeViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string StepsDoneLastTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string DistanceWalkedLastTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string LastDataTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int StepCounter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int DistanceWalked { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int StepFrequency { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsRunning { get =>  throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public StepModeViewModel()
        {
            throw new NotImplementedException();
        }

        public override void OnActivityDone(object sender, ActivityArgs args)
        {
            throw new NotImplementedException();
        }

        protected override void StartActivity()
        {
            throw new NotImplementedException();
        }

        protected override void StopActivity()
        {
            throw new NotImplementedException();
        }

    }
}
