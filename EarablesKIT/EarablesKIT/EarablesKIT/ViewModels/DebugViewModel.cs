using EarablesKIT.Models.DatabaseService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using EarablesKIT.Models.Library;
using Xamarin.Forms;
using System.ComponentModel;
using EarablesKIT.Models;
using Microsoft.Extensions.DependencyInjection;
using EarablesKIT.Annotations;
using System.Runtime.CompilerServices;

namespace EarablesKIT.ViewModels
{
    public class DebugViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<IMUDataEntry> TrainingsData { get ; private set; }

        private double _absAcc;
        public double AbsGAcc
        {
            get
            {
                return _absAcc;
            }
            set
            {
                _absAcc = value; 
                OnPropertyChanged("AbsGAcc");
            }
        }
        public Command ToggleRecordingCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (!this.Recording)
                    {
                        TrainingsData.Clear();
                    }

                    this.Recording = !this.Recording;
                    this.RecordingLabelText = this.Recording ? "Stop Recording" : "Start Recording";
                });
            }
        }

        public bool Recording { get; private set; } = false;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _recordingLabelText = "Start Recording";

        public string RecordingLabelText
        {
            get => _recordingLabelText;
            set
            {
                if (_recordingLabelText != value)
                {
                    _recordingLabelText = value;

                    OnPropertyChanged("RecordingLabelText"); // Notify that there was a change on this property
                }
            }
        }

        public DebugViewModel()
        {
            // TODO register eventHandler

            var earablesService = (EarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection));
            
            // Bennis popup
            earablesService.StartSampling();

            earablesService.IMUDataReceived += (object sender, DataEventArgs args) =>
            {
                if (this.Recording)
                {
                    TrainingsData.Clear();
                    TrainingsData.Add(args.Data);
                    AbsGAcc = Math.Pow(args.Data.Acc.G_X, 2) + Math.Pow(args.Data.Acc.G_Y, 2) + Math.Pow(args.Data.Acc.G_Z, 2);
                }
            };

            TrainingsData = new ObservableCollection<IMUDataEntry>();
        }
    }
}