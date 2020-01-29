using EarablesKIT.Models;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.RunningActivity;
using EarablesKIT.Models.Extentionmodel.Activities.StepActivity;
using EarablesKIT.Models.Library;
using MediaManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                } else
                {
                    CrossMediaManager.Current.Pause();
                }
                OnPropertyChanged("StartStopLabel");
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
                    StartActivity();
                } else
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

        private async void InitMusic(string urlPath)
        {
            await CrossMediaManager.Current.Play(urlPath);
            await CrossMediaManager.Current.Pause();
        }

        private IActivityManager _activityManager;
        private AbstractRunningActivity _runningActivity { get; set; }

        public MusicModeViewModel()
        {
            InitMusic("https://sampleswap.org/samples-ghost/PUBLIC%20DOMAIN%20MUSIC/3337[kb]Extracts-from-the-Ballet-Suite-Scherazada.mp3.mp3");
            _activityManager = (IActivityManager)ServiceManager.ServiceProvider.GetService(typeof(IActivityManager));
            _runningActivity = (AbstractRunningActivity)_activityManager.ActitvityProvider.GetService(typeof(AbstractRunningActivity));
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
            _running = false;
            InitMusic("https://sampleswap.org/samples-ghost/PUBLIC%20DOMAIN%20MUSIC/3337[kb]Extracts-from-the-Ballet-Suite-Scherazada.mp3.mp3");
            ((IEarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection))).StopSampling();
            _runningActivity.ActivityDone -= OnActivityDone;
        }

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
