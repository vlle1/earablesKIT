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
        private const int STATE_COUNT = 4;
        //in the following the conditions for a state increment are specified (from state index to state index + 1):
        //the cooldown after the last state-change has to be over and some value of the IMU Data has to be higher/equal or lower as some threshold
        //true iff a threshold has to be underrun instead of exceeded.
        private readonly bool[] LOWER = { true, false, false, true };
        //represents the value of the IMU Data that is used for the comparison.
        //0,1,2 ar Accelerometer X,Y,Z (in G) and 3,4,5 are Gyroscope X,Y,Z
        private readonly int[] VALUE_INDEX = { 1, 5, 1, 5 }; 

        //the threshold that needs to be passed
        private readonly double[] THRESHOLD = { -1, 100, 0, -100 };

        //_state represents a state machine with four states:
        //0 represents starting position, 
        //1 represents going up,
        //2 represents being upright
        //3 represents going down,
        private int _state;
        private double _accRef = 1;//not needed
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
        EarablesConnection _earablesService;
        public Command ToggleRecordingCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (!this.Recording)
                    {
                        _earablesService.StartSampling();
                    }
                    else _earablesService.StopSampling();

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
            _earablesService = (EarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection));
            
            

            _earablesService.IMUDataReceived += (object sender, DataEventArgs args) =>
                {
                    OneValue = args.Data;
                    Analyze(args);
                };

        }
        private void Analyze(DataEventArgs data)
        {

                Accelerometer newAccValue = data.Data.Acc;
                double[] dataAsArray = { 
                    data.Data.Acc.G_X, 
                    data.Data.Acc.G_Y, 
                    data.Data.Acc.G_Z, 
                    data.Data.Gyro.DegsPerSec_X, 
                    data.Data.Gyro.DegsPerSec_Y, 
                    data.Data.Gyro.DegsPerSec_Z 
                };
                //check if condition is fulfilled
                if (LOWER[_state] == ( dataAsArray[VALUE_INDEX[_state]]< THRESHOLD[_state]))
                {

                if (_state == 1) InfoString = "";
                //++
                InfoString += "state " + _state + " mit " + dataAsArray[1] + " in  YAcc\n";
                _state++;
                    //check if all states have been passed and activity therefore is detected
                if (_state % STATE_COUNT == 0)
                {
                    _state = 0;
                    //-- ActivityDone.Invoke(this, new PushUpEventArgs());

                        
                }
                Counter = _state;
                //++ 
                
            }
        }
    }
}