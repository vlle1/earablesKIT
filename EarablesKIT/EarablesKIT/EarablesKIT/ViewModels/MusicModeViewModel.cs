using EarablesKIT.Models;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.RunningActivity;
using EarablesKIT.Models.Library;
using EarablesKIT.Resources;
using MediaManager;
using System;
using System.ComponentModel;
using System.IO;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    public class MusicModeViewModel : BaseModeViewModel, INotifyPropertyChanged
    {
        private bool _running;
        private bool _musicModeActive;
        private static IMediaManager _mediaManager;

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

        public Command ToggleMusicMode =>
            new Command(() =>
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
            }
            catch (Exception e)
            {
                ExceptionHandlingViewModel.HandleException(e);
            }
        }

        private AbstractRunningActivity RunningActivity { get; }
        private string _path;

        public MusicModeViewModel()
        {
            var activityManager = (IActivityManager)ServiceManager.ServiceProvider.GetService(typeof(IActivityManager));
            RunningActivity = (AbstractRunningActivity)activityManager.ActitvityProvider.GetService(typeof(AbstractRunningActivity));

            if (_mediaManager is null)
            {
                _mediaManager = CrossMediaManager.Current;
            }

            _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "music/ukulele.mp3");
            Directory.CreateDirectory(path: Path.GetDirectoryName(_path) ?? throw new InvalidOperationException());

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
                ExceptionHandlingViewModel.HandleException(e);
            }
        }

        public override void OnActivityDone(object sender, ActivityArgs args)
        {
            IsRunning = ((RunningEventArgs)args).Running;
        }

        public override bool StartActivity()
        {
            if (!CheckConnection()) return false;
            RestartMusic();
            RunningActivity.ActivityDone += OnActivityDone;
            ((IEarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection))).StartSampling();
            return true;
        }

        public override void StopActivity()
        {
            try
            {
                ((IEarablesConnection) ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection)))
                    .StopSampling();

                if (RunningActivity != null) RunningActivity.ActivityDone -= OnActivityDone;
            }
            catch (Exception e)
            {
                ExceptionHandlingViewModel.HandleException(e);
            }
            _musicModeActive = false;
            IsRunning = false;
        }

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
