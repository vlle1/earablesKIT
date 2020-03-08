using EarablesKIT.Models.DatabaseService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using EarablesKIT.Models.Library;
using Xamarin.Forms;
using System.ComponentModel;
using EarablesKIT.Models;
using EarablesKIT.Annotations;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using EarablesKIT.Resources;
using System.IO;

namespace EarablesKIT.ViewModels
{
    public class DebugViewModel : INotifyPropertyChanged
    {
        private const string CSV_FORMAT_STRING = "samplerate,acc_gx,acc_gy,acc_gz,gyro_pdsx,gyro_dpsy,gyro_dpsz";
        private Queue<String> insaneQueue = new Queue<String>();
        private IMUDataEntry _oneValue = new IMUDataEntry(new Models.Library.Accelerometer(0, 0, 0, 0, 0, 0), new Models.Library.Gyroscope(0, 0, 0));
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

        EarablesConnection _earablesService;
        /**
         * Updates Recording button label, toggles Sampling, clears queue and calls share method.
         */
        public Command ToggleRecordingCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (!this.Recording)
                    {
                        insaneQueue.Clear();
                        insaneQueue.Enqueue(CSV_FORMAT_STRING);
                        _earablesService.StartSampling();
                    }
                    else
                    {
                        _earablesService.StopSampling();
                        ShareAsMockDataCsv();
                    }
                    this.Recording = !this.Recording;
                    this.RecordingLabelText = this.Recording ? "Aufzeichnung Stoppen" : "Aufzeichnung Starten";
                });
            }
        }

        public bool Recording { get; private set; } = false;
        public event PropertyChangedEventHandler PropertyChanged;

        
        private string _recordingLabelText = "Aufzeichnung Starten";

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

            insaneQueue.Enqueue(CSV_FORMAT_STRING);

            _earablesService.IMUDataReceived += (object sender, DataEventArgs args) =>
                {
                    Analyze(args);
                };

        }
        /**
         * Set OneValue to the current value to show it on the Page. Also add the Value to the Queue.
         * Note that the current implementation of the Motion Detection Algorithms only use the following values, so only those are saved due to a lack of a string representation of DataEventArgs:
         *          data.Configs.Samplerate; 
         *          data.Data.Acc.G_X,
         *          data.Data.Acc.G_Y,
         *          data.Data.Acc.G_Z,
         *          data.Data.Gyro.DegsPerSec_X,
         *          data.Data.Gyro.DegsPerSec_Y,
         *          data.Data.Gyro.DegsPerSec_Z
         */
        private void Analyze(DataEventArgs data)
        {
            OneValue = data.Data;
            //buffering of data
            //create csv-like format
            insaneQueue.Enqueue(
                
                data.Configs.Samplerate
                + "," + data.Data.Acc.G_X
                + "," + data.Data.Acc.G_Y
                + "," + data.Data.Acc.G_Z
                + "," + data.Data.Gyro.DegsPerSec_X
                + "," + data.Data.Gyro.DegsPerSec_Y
                + "," + data.Data.Gyro.DegsPerSec_Z
                );

        }

        async private void ShareAsMockDataCsv()
        {
            //generate output file
            var fn = "recMockData" + DateTime.Now.ToString().Replace('/','-').Replace(':','-') + ".csv";
            var file = Path.Combine(FileSystem.CacheDirectory, fn);
            File.WriteAllLines(file, insaneQueue.ToArray());
            insaneQueue.Clear();
            //call share dialog
            await Share.RequestAsync(new ShareFileRequest
            { 
                Title = AppResources.ImportExportSaveDisplayTitle,
                File = new ShareFile(file)
            });
            

            //todo give success message
            string reminderString = "Jetzt schreib mir noch eine kleine Nachricht dazu: \n\nUm welche Aktivität hat es sich gehandelt? \n\nWie viele Wiederholungen waren es genau (wurde eine zu viel gemacht oder so..)? \n\nGibt es weitere Anmerkungen bzgl der Auswertung, die mir bei der Auswertung helfen könnten?";
            Application.Current.MainPage.DisplayAlert("Yeah!", "Du hast es geschafft. "+reminderString,"Okay");
        }




        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}