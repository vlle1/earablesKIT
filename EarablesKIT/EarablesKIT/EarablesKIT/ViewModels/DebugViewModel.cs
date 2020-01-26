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

        private int cooldown = 0;
        private double _accRef = 1;
        private double _accRefX = -1;
        private double _accRefY = 0;
        private double _accRefZ = 0;
        public double AbsRefGAcc
        {
            get
            {
                return _accRef;
            }
            set
            {
                _accRef= value;
                OnPropertyChanged("ReferenceAcc");
            }
        }
        private IMUDataEntry _oneValue = new IMUDataEntry(new Accelerometer(0, 0, 0, 0, 0, 0), new Gyroscope(0, 0, 0));
        public IMUDataEntry OneValue
        {
            get
            {
                return _oneValue;
            }
            set
            {
                _oneValue = value;
                OnPropertyChanged("OneValue");
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
                
                //Accelerometer av = args.Data.Acc;

                if (this.Recording)
                {
                    OneValue = args.Data;
                    Accelerometer av = OneValue.Acc;
                
                    AbsGAcc = Math.Sqrt(Math.Pow(av.G_X, 2) + Math.Pow(av.G_Y, 2) + Math.Pow(av.G_Z, 2));
                    AbsRefGAcc = (AbsGAcc + 100 * AbsRefGAcc)/101;
                    _accRefX = (av.G_X + 100 * _accRefX) / 101;
                    _accRefY = (av.G_Y + 100 * _accRefY) / 101;
                    _accRefZ = (av.G_Z + 100 * _accRefZ) / 101;
                    if (AbsGAcc > 1.15 * AbsRefGAcc)
                    {
                        if (cooldown == 0)
                        {
                            cooldown = 25;
                            //winkelcheck
                            if ((_accRefX * args.Data.Acc.G_X + _accRefY * args.Data.Acc.G_Y + _accRefZ * args.Data.Acc.G_Z)
                                / AbsGAcc / AbsRefGAcc > 0.89)
                            {
                                Counter++;
                                
                                InfoString = "new tick is" + "[coming soon]";

                            } else
                            {
                                InfoString = "falscher Winkel"+ Math.Round((_accRefX * args.Data.Acc.G_X + _accRefY * args.Data.Acc.G_Y + _accRefZ * args.Data.Acc.G_Z)
                                / AbsGAcc / AbsRefGAcc,2) +" erkannt. Aktuell "+Math.Round(av.G_X,2)+" "+ Math.Round(av.G_Y, 2) + " "+ Math.Round(av.G_Z, 2) + ", Ref" + Math.Round(_accRefX,2) + " " + Math.Round(_accRefY, 2) + " "+ Math.Round(_accRefZ, 2) + ". ";
                            }

                        }
                        else
                        {
                            InfoString += " c";
                        }
                    }

                    if (cooldown > 0) cooldown--;
                }
                
            };

        }
    }
}