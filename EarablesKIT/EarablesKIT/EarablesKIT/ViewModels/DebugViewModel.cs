using EarablesKIT.Models.DatabaseService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using EarablesKIT.Models.Library;
using Xamarin.Forms;
using System.ComponentModel;

namespace EarablesKIT.ViewModels
{
    public class DebugViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<IMUDataEntry> TrainingsData { get; private set; }

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
                    // RecordData(this.Recording);
                });
            }
        }

        public bool Recording { get; private set; } = false;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
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

            //Dummy Event
            Device.StartTimer(TimeSpan.FromSeconds(2), () =>
            {
                RecordData(this.Recording);
                return true; // True = Repeat again, False = Stop the timer
            });
            ////

            TrainingsData = new ObservableCollection<IMUDataEntry>();
        }

        public void RecordData(bool recording)
        {
            if (recording)
            {
                InsertData(null, null);
            }
        }

        public void InsertData(DataEventArgs args, IMUDataEntry data)
        {
            TrainingsData.Add(
                new IMUDataEntry
                {
                    Acc = new Accelerometer
                    {
                        G_X = 1.2f,
                        G_Y = 1.4f,
                        G_Z = 1.6f,
                        MperS_X = 8,
                        MperS_Y = 43.4f,
                        MperS_Z = 3.4f
                    },
                    Gyro = new Gyroscope
                    {
                        DegsPerSec_X = 32.4f,
                        DegsPerSec_Y = 32.4f,
                        DegsPerSec_Z = 23.32f
                    }
                });
        }
    }
}