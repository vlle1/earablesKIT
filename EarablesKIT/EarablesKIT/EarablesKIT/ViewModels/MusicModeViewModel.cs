using EarablesKIT.Models;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.RunningActivity;
using EarablesKIT.Models.Library;
using EarablesKIT.Resources;
using MediaManager;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    public class MusicModeViewModel : BaseModeViewModel, INotifyPropertyChanged
    {
        private bool _running = false;
        private bool _musicModeActive = false;
        private static IMediaManager _mediaManager;
        private static IActivityManager _activityManager;
        private IExceptionHandler _exceptionHandler;

        public event PropertyChangedEventHandler PropertyChanged;


        public bool IsRunning
        {
            set
            {
                _running = value;
                if (_running)
                {
                    _mediaManager.Play();
                }
                else
                {
                    _mediaManager.Pause();
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentStatusLabel));
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
                OnPropertyChanged(nameof(CurrentStatusLabel));
            });
        }


        public string StartStopLabel
        {
            get => _musicModeActive ? "Stop" : "Start";
        }

        public string CurrentStatusLabel
        {

            get
            {
                return _musicModeActive
                        ? (IsRunning
                            ? AppResources.MusicModeCurrentStatusLabelWalking
                            : AppResources.MusicModeCurrentStatusLabelStanding)
                        : AppResources.MusicModeExplanation;
            }
        }


        /// <summary>
        /// Loading the music file and pausing the player.
        /// </summary>
        private async void RestartMusic()
        {
            try
            {
                await _mediaManager.Play(_path);
                await _mediaManager.Pause();
            } catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        private AbstractRunningActivity runningActivity { get; set; }
        private string _path { get; set; }

        public MusicModeViewModel()
        {
            _activityManager = (IActivityManager)ServiceManager.ServiceProvider.GetService(typeof(IActivityManager));
            runningActivity = (AbstractRunningActivity)_activityManager.ActitvityProvider.GetService(typeof(AbstractRunningActivity));

            if (_mediaManager is null)
            {
                _mediaManager = CrossMediaManager.Current;
            }

            _exceptionHandler =
                (IExceptionHandler) ServiceManager.ServiceProvider.GetService(typeof(IExceptionHandler));

            _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "music/ukulele.mp3");
            Directory.CreateDirectory(Path.GetDirectoryName(_path));

            try
            {
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
            }
            catch (Exception e)
            {
                _exceptionHandler.HandleException(e);
            }
        }

        public override void OnActivityDone(object sender, ActivityArgs args)
        {
            IsRunning = ((RunningEventArgs)args).Running;
        }

        public override bool StartActivity()
        {
            if (CheckConnection())
            {
                RestartMusic();
                runningActivity.ActivityDone += OnActivityDone;
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
            try
            {
                ((IEarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection))).StopSampling();
                runningActivity.ActivityDone -= OnActivityDone;
            } catch
            {}
            _musicModeActive = false;
            IsRunning = false;
        }

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
