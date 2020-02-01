using EarablesKIT.Models;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.RunningActivity;
using EarablesKIT.Models.Extentionmodel.Activities.StepActivity;
using EarablesKIT.Models.Library;
using EarablesKIT.Resources;
using MediaManager;
using MediaManager.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    class MusicModeViewModel : BaseModeViewModel, INotifyPropertyChanged
    {
        private bool _running = false;
        private bool _musicModeActive = false;

        public event PropertyChangedEventHandler PropertyChanged;


        public bool IsRunning
        {
            set
            {
                _running = value;
                if (_running)
                {
                    CrossMediaManager.Current.Play();
                }
                else
                {
                    CrossMediaManager.Current.Pause();
                }
                //OnPropertyChanged("StartStopLabel");
                OnPropertyChanged();
            }
            get => _running;
        }

        public Command ToggleMusicMode
        {
            get => new Command(() =>
            {
                _musicModeActive = !_musicModeActive;
                if (_musicModeActive)
                {
                    if (!StartActivity()) _musicModeActive = !_musicModeActive;
                }
                else
                {
                    StopActivity();
                }
                //CrossMediaManager.Current.PlayPause();
                OnPropertyChanged(nameof(StartStopLabel));
            });
        }


        public string StartStopLabel
        {
            get => _musicModeActive ? "Stop" : "Start";
        }

        public string CurrentStatusLabel
        {
            get => IsRunning ? AppResources.MusicModeCurrentStatusLabelWalking : AppResources.MusicModeCurrentStatusLabelStanding;
        }


        /// <summary>
        /// Loading the music file and pausing the player.
        /// </summary>
        private async void InitMusic()
        {
            try
            {
                await CrossMediaManager.Current.Play(_path);
                await CrossMediaManager.Current.Pause();
            } catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        private IActivityManager _activityManager;
        private AbstractRunningActivity _runningActivity { get; set; }
        private string _path { get; set; }

        public MusicModeViewModel()
        {
            _activityManager = (IActivityManager)ServiceManager.ServiceProvider.GetService(typeof(IActivityManager));
            _runningActivity = (AbstractRunningActivity)_activityManager.ActitvityProvider.GetService(typeof(AbstractRunningActivity));

            _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "music/ukulele.mp3");
            Directory.CreateDirectory(Path.GetDirectoryName(_path));

            // Copying the resource music file to the MyDocuments Path because the MediaPlayer can't play streams.
            using (BinaryWriter writer = new BinaryWriter(File.Open(_path, FileMode.Create)))
            {
                using (var input = new BinaryReader(AppResources.ukulele_low))
                {
                    while (true)
                    {
                        try
                        {
                            var b = input.ReadByte();
                            writer.Write(b);
                        }
                        catch
                        {
                            break;
                        }
                    }
                }
            }

            InitMusic();
        }

        public override void OnActivityDone(object sender, ActivityArgs args)
        {
            IsRunning = ((RunningEventArgs)args).Running;
        }

        public override bool StartActivity()
        {
            /* Debug events 
                         if (true)
            {
                Device.StartTimer(TimeSpan.FromSeconds(30), () =>
                {
                    // Do something
                    OnActivityDone(this, new RunningEventArgs(!IsRunning));
                    return true; // True = Repeat again, False = Stop the timer
                });
                //((IEarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection))).StartSampling();
                return true;
            }
            else
            {
                return false;
            }
             */
            if (CheckConnection())
            {
                InitMusic();
                _runningActivity.ActivityDone += OnActivityDone;
                ((IEarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection))).StartSampling();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void StopActivity()
        {
            IsRunning = false;
            ((IEarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection))).StopSampling();
            _runningActivity.ActivityDone -= OnActivityDone;
        }

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
