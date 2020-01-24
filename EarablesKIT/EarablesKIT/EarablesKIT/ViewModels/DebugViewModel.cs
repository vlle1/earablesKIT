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
        private int cooldown = 0;
        private double _referenceAcc = 1;
        public double ReferenceAcc
        {
            get
            {
                return _referenceAcc;
            }
            set
            {
                _referenceAcc= value;
                OnPropertyChanged("ReferenceAcc");
            }
        }
        private string _infoString = "";
        public string InfoString
        {
            get
            {
                return _infoString;
            }
            set
            {
                _infoString = value;
                OnPropertyChanged("InfoString");
            }
        }

        private int _counter = 0;
        public int Counter
        {
            get
            {
                return _counter;
            }
            set
            {
                _counter = value;
                OnPropertyChanged("Counter");
            }
        }
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
                    ReferenceAcc = (AbsGAcc + 100 * ReferenceAcc)/101;
                    if (AbsGAcc > 1.2 * ReferenceAcc)
                    {
                        if (cooldown == 0)
                        {
                            Counter++;
                            cooldown = 15;
                            InfoString = "new tick is" + "[coming soon]";
                        }
                        else
                        {
                            InfoString += " c";
                        }
                    }

                    if (cooldown > 0) cooldown--;
                }
                
            };

            TrainingsData = new ObservableCollection<IMUDataEntry>();
        }
    }
}