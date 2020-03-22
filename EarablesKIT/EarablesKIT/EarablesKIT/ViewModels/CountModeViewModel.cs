using EarablesKIT.Models;
using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.PushUpActivity;
using EarablesKIT.Models.Extentionmodel.Activities.SitUpActivity;
using EarablesKIT.Models.Library;
using EarablesKIT.Models.PopUpService;
using EarablesKIT.Resources;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    /// <summary>
    /// Class that represents the CountMode. Manages the progress of the CountMode, implements the BaseModeViewModel.
    /// </summary>
    public class CountModeViewModel : BaseModeViewModel, INotifyPropertyChanged
    {
        /// <summary>
        /// Timer that starts running on activation of the mode.
        /// </summary>
        private Stopwatch _timer;

        /// <summary>
        /// Property which wraps the pushUpActivity.
        /// </summary>
        private ActivityWrapper _pushUpActivityWrapper { get; set; }

        /// <summary>
        /// Property which wraps the sitUpActivity.
        /// </summary>
        private ActivityWrapper _sitUpActivityWrapper { get; set; }

        /// <summary>
        /// Placeholder for other activities.
        /// </summary>
        private ActivityWrapper _comingSoon { get; set; }

        /// <summary>
        /// The pushUpActivity from the ActivityProvider.
        /// </summary>
        private AbstractPushUpActivity _pushUpActivity { get; set; }

        /// <summary>
        /// The sitUpActivity from the ActivityProvider.
        /// </summary>
        private AbstractSitUpActivity _sitUpActivity { get; set; }

        /// <summary>
        /// Property which holds the instance of the ActivityManager.
        /// </summary>
        private IActivityManager _activityManager { get; set; }

        /// <summary>
        /// Property which hold the instance of the DataBaseConnection.
        /// </summary>
        private IDataBaseConnection _dataBaseConnection { get; set; }

        private IPopUpService _popUpService { get; set; }

        /// <summary>
        /// List of all possible activities that the user can do.
        /// </summary>
        public ObservableCollection<ActivityWrapper> PossibleActivities { get; set; }

        /// <summary>
        /// The currently selected activity by the user, bound to the view.
        /// </summary>
        public ActivityWrapper SelectedActivity { get; set; }

        /// <summary>
        /// Properties to show the elapsed time since the start of the mode, bound to the view.
        /// </summary>
        private string _minutes, _seconds, _milliseconds;

        public string Minutes
        {
            get { return _minutes; }
            set
            {
                _minutes = value;
                OnPropertyChanged();
            }
        }
        public string Seconds
        {
            get { return _seconds; }
            set
            {
                _seconds = value;
                OnPropertyChanged();
            }
        }
        public string Milliseconds
        {
            get { return _milliseconds; }
            set
            {
                _milliseconds = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Requests different services from the ServiceProvider and initializes the list of possible activities.
        /// </summary>
        public CountModeViewModel()
        {
            _activityManager = (IActivityManager)ServiceManager.ServiceProvider.GetService(typeof(IActivityManager));
            _pushUpActivity = (AbstractPushUpActivity)_activityManager.ActitvityProvider.GetService(typeof(AbstractPushUpActivity));
            _sitUpActivity = (AbstractSitUpActivity)_activityManager.ActitvityProvider.GetService(typeof(AbstractSitUpActivity));
            _pushUpActivityWrapper = new ActivityWrapper(AppResources.Push_ups, _pushUpActivity);
            _sitUpActivityWrapper = new ActivityWrapper(AppResources.Sit_ups, _sitUpActivity);
            _comingSoon = new ActivityWrapper(AppResources.Coming_soon, null);
            _dataBaseConnection = (IDataBaseConnection)ServiceManager.ServiceProvider.GetService(typeof(IDataBaseConnection));
            _popUpService = (IPopUpService)ServiceManager.ServiceProvider.GetService(typeof(IPopUpService));
            _timer = new Stopwatch();
            PossibleActivities = new ObservableCollection<ActivityWrapper>
            {
                _pushUpActivityWrapper,
                _sitUpActivityWrapper,
                _comingSoon
            };
            SelectedActivity = _pushUpActivityWrapper;
        }

        /// <summary>
        /// Method which handles the start of the mode. Checks for a connection to the Earables, 
        /// registers the event method and starts the sampling of the Earables.
        /// </summary>
        /// <returns>Bool if everything was successfull</returns>
        public override bool StartActivity()
        {
            if (CheckConnection())
            {
                if (RegisterActivity())
                {
                    ((IEarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection))).StartSampling();
                    _pushUpActivityWrapper.Counter = 0;
                    _sitUpActivityWrapper.Counter = 0;
                    return true;
                }
                return false;

            }
            return false;
        }

        /// <summary>
        /// Methods that registers the OnActivityDone event method with the event handler.
        /// </summary>
        /// <returns>Bool if the registration was successfull</returns>
        private bool RegisterActivity()
        {
            if (SelectedActivity != null && SelectedActivity._activity != null)
            {
                SelectedActivity._activity.ActivityDone += OnActivityDone;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Increases the active activity's counter by 1 whenever an event is thrown.
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="args">Ignored</param>
        public override void OnActivityDone(object sender, ActivityArgs args)
        {
            SelectedActivity.Counter++;
        }

        /// <summary>
        /// Methods that starts the timer and updated the time properties every 0.1 seconds 
        /// by calculating the elapsed time since the start of the mode.
        /// </summary>
        public void StartTimer()
        {
            Minutes = "00"; Seconds = "00"; Milliseconds = "000";
            _timer = new Stopwatch();
            _timer.Start();
            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                if (_timer.Elapsed.Minutes.ToString().Length == 1)
                {
                    Minutes = "0" + _timer.Elapsed.Minutes.ToString();
                }
                else
                {
                    Minutes = _timer.Elapsed.Minutes.ToString();
                }

                if (_timer.Elapsed.Seconds.ToString().Length == 1)
                {
                    Seconds = "0" + _timer.Elapsed.Seconds.ToString();
                }
                else
                {
                    Seconds = _timer.Elapsed.Seconds.ToString();
                }

                Milliseconds = _timer.Elapsed.Milliseconds.ToString();
                while (Milliseconds.Length < 3) Milliseconds = "0" + Milliseconds;
                return true;
            });
        }

        /// <summary>
        /// Method which handles the Stopping of the mode. Stops the timer, unregisters the event method, 
        /// saves data and shows a Pop-up.
        /// </summary>
        public override void StopActivity()
        {
            StopTimer();
            SelectedActivity._activity.ActivityDone -= OnActivityDone;
            SaveData();
            ShowPopUp();
        }

        /// <summary>
        /// Method which resets the timer.
        /// </summary>
        private void StopTimer()
        {
            _timer.Reset();
        }

        /// <summary>
        /// Method which saves the amount of repetitions done by the user via the DataBaseConnection.
        /// </summary>
        private void SaveData()
        {
            DateTime _dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DBEntry _entryNew = new DBEntry(_dt, 0, _pushUpActivityWrapper.Counter, _sitUpActivityWrapper.Counter);
            _dataBaseConnection.SaveDBEntry(_entryNew);
        }

        /// <summary>
        /// Methods which shows the amount of repetitions done by the user via a Pop-up.
        /// </summary>
        private void ShowPopUp()
        {
            _popUpService.DisplayAlert(AppResources.Result, AppResources.YouHaveDone + " " + SelectedActivity.Counter + " " + SelectedActivity.Name + AppResources.alternativeGrammarDone + "!", AppResources.Cool);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
