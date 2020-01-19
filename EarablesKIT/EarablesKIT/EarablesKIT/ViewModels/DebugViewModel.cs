using EarablesKIT.Models.DatabaseService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using EarablesKIT.Models.Library;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    public class DebugViewModel
    {

        public ObservableCollection<IMUDataEntry> TrainingsData { get; private set; }
        public Command ToggleRecordingCommand
        {
            get
            {
                return new Command(() =>
                {
                    recordData(this.Recording ^= true);
                });
            }
        }
        public bool Recording { get; private set; } = false;
        public string RecordingLabelText { get { return Recording ? "Stop Recording" : "Start Recording"; } }

        public DebugViewModel()
        {
            // TODO register eventHandler

            TrainingsData = new ObservableCollection<IMUDataEntry>();
        }

        public void recordData(bool recording)
        {
            if (recording)
            {
                insertData(null, null);
            }
        }

        public void insertData(DataEventArgs args, IMUDataEntry data)
        {
            TrainingsData = new ObservableCollection<IMUDataEntry>(){
                new IMUDataEntry
                {
                    Acc = new Accelerometer
                    {
                        G_X = 1.2f, G_Y = 1.4f, G_Z = 1.6f, MperS_X = 8, MperS_Y = 43.4f, MperS_Z = 3.4f
                    },
                    Gyro = new Gyroscope
                    {
                        DegsPerSec_X = 32.4f, DegsPerSec_Y = 32.4f, DegsPerSec_Z = 23.32f
                    }
                },

            };
        }

    }
}
