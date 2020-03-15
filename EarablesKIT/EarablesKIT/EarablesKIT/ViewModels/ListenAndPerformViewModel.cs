using EarablesKIT.Models;
using EarablesKIT.Models.AudioService;
using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.PushUpActivity;
using EarablesKIT.Models.Extentionmodel.Activities.SitUpActivity;
using EarablesKIT.Models.Library;
using EarablesKIT.Models.PopUpService;
using EarablesKIT.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
	public class ListenAndPerformViewModel : BaseModeViewModel, INotifyPropertyChanged
	{

		/// <summary>
		/// Command for Adding an activity to the list, bound to the add button.
		/// </summary>
		public ICommand AddActivityCommand { get; set; }

		/// <summary>
		/// Command for Removing an activity from the list, bound to the remove button.
		/// </summary>
		public ICommand RemoveActivityCommand { get; set; }

		/// <summary>
		/// Command for Editing an activity from the list, bound to the edit button.
		/// </summary>
		public ICommand EditActivityCommand { get; set; }

		/// <summary>
		/// Dit brauch ich.
		/// </summary>
		private bool _inserted = false;

		/// <summary>
		/// Timer that starts running on activation of the mode.
		/// </summary>
		private Stopwatch _timer;

		/// <summary>
		/// Timer that runs during pauses.
		/// </summary>
		private Timer PauseTimer;

		/// <summary>
		/// Counters for the total amount of repetitions of Sit-ups and Push-ups. 
		/// </summary>
		private int _pushUpResult, _sitUpResult;

		/// <summary>
		/// Holds the amount of the currently active activities' repetitions that the user has set.
		/// </summary>
		private int Repetitions;

		/// <summary>
		/// A list of ActivityWrappers, which saves the activity and the amount of repetitions the user wants to do, bound to view.
		/// </summary>
		public ObservableCollection<ActivityWrapper> ActivityList { get; set; }

		/// <summary>
		/// Iterator for the ActivityList
		/// </summary>
		private IEnumerator<ActivityWrapper> ActivityIterator;

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

		private IAudioService _audioService { get; set; }

		/// <summary>
		/// The currently selected activity by the user, bound to the view.
		/// </summary>
		public ActivityWrapper SelectedActivity { get; set; }

		/// <summary>
		/// The currently active Activity while the mode is running, set by the Iterator.
		/// </summary>
		private ActivityWrapper _activeActivity;
		public ActivityWrapper ActiveActivity
		{
			get { return _activeActivity; }
			set
			{
				_activeActivity = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Properties for displaying the elapsed time.
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
		/// Property which holds the progress of the user during an activity, displayed by a progressbar, 
		/// resets after each list iteration.
		/// </summary>
		private double _progress;
		public double ProgressLive
		{
			get { return _progress; }
			set
			{
				_progress = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Defines the Commands, requests different services from the ServiceProvider 
		/// and initializes the ActivityList with standard values.
		/// </summary>
		public ListenAndPerformViewModel()
		{
			AddActivityCommand = new Command(() => AddActivity(ActivityList.Count));
			RemoveActivityCommand = new Command(() => RemoveActivity());
			EditActivityCommand = new Command(() => EditActivity());
			_activityManager = (IActivityManager)ServiceManager.ServiceProvider.GetService(typeof(IActivityManager));
			_pushUpActivity = (AbstractPushUpActivity)_activityManager.ActitvityProvider.GetService(typeof(AbstractPushUpActivity));
			_sitUpActivity = (AbstractSitUpActivity)_activityManager.ActitvityProvider.GetService(typeof(AbstractSitUpActivity));
			_dataBaseConnection = (IDataBaseConnection)ServiceManager.ServiceProvider.GetService(typeof(IDataBaseConnection));
			_popUpService = (IPopUpService)ServiceManager.ServiceProvider.GetService(typeof(IPopUpService));
			_audioService = (IAudioService)ServiceManager.ServiceProvider.GetService(typeof(IAudioService));

			ActivityList = new ObservableCollection<ActivityWrapper>
			{
				new ActivityWrapper(AppResources.Push_ups, _pushUpActivity, 10),
				new ActivityWrapper(AppResources.Pause, null, 10),
				new ActivityWrapper(AppResources.Sit_ups, _sitUpActivity, 10)
			};
		}

		/// <summary>
		///Method which handles the start of the mode. Checks for a connection to the Earables and a valid ActivityList,
		///initializes the Interator and starts the sampling of the Earables.
		/// </summary>
		/// <returns>Bool if the start was successfull</returns>
		public override bool StartActivity()
		{
			if (//CheckConnection() && 
				ActivityList.Count > 0)
			{
				_pushUpResult = 0;
				_sitUpResult = 0;
				PauseTimer = new Timer();
				ActivityIterator = ActivityList.GetEnumerator();
				ActivityIterator.MoveNext();
				//((IEarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection))).StartSampling();
				CheckNextActivity();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Method which updates the properties whenever an entry of the ActivityList is done. Informs the user
		/// about the next activity via Text-to-Speech and registers the next activity with the correct event
		/// handler; if the next entry is a pause initalize a pause timer and its event.
		/// </summary>
		private void CheckNextActivity()
		{
			ActiveActivity = ActivityIterator.Current;
			Repetitions = ActiveActivity.Amount;
			_ = SpeakActivity(Repetitions);
			if (ActiveActivity._activity != null)
			{
				ActiveActivity.Counter = 0;
				ActiveActivity._activity.ActivityDone += OnActivityDone;
				ProgressLive = 0;
			}
			else
			{
				ActiveActivity.Counter = Repetitions;
				ProgressLive = 0;
				PauseTimer = new Timer();
				PauseTimer.Interval = 1000;
				PauseTimer.Elapsed += OnTimedEvent;
				PauseTimer.AutoReset = true;
				PauseTimer.Enabled = true;
			}

		}

		/// <summary>
		/// Increases the active activities' counter by 1 whenever an event is thrown. Updates the progress 
		/// and checks for the next activity in the ActivityList if the user has done the required amount
		/// of repetitions. Stops the timer when all activities are done and notifies the user via Text-to-Speech.
		/// </summary>
		/// <param name="sender">The sender of the event</param>
		/// <param name="args">Ignored</param>
		public async override void OnActivityDone(object sender, ActivityArgs args)
		{
			ActiveActivity.Counter++;
			ProgressLive = Math.Round((double)ActiveActivity.Counter / Repetitions, 2);
			if (ActiveActivity.Counter >= Repetitions)
			{
				ActiveActivity._activity.ActivityDone -= OnActivityDone;
				IncreaseResultCounter();
				if (ActivityIterator.MoveNext())
				{
					CheckNextActivity();
				}
				else
				{
					_timer.Stop();
					await _audioService.Speak(AppResources.TrainingDone);
				}
			}
		}

		/// <summary>
		/// Decreases the pause counter of the "pause activity" by 1 every time a time event is thrown. Updates
		/// the progress and checks for the next activity in the ActivityList if the pause timer equals 0. 
		/// Stops the timer when all activities are done and notifies the user via Text-to-Speech.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		private async void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			ActiveActivity.Counter--;
			ProgressLive = 1 - Math.Round((double)ActiveActivity.Counter / Repetitions, 2);
			if (ActiveActivity.Counter <= 0)
			{
				PauseTimer.Stop();
				PauseTimer.Elapsed -= OnTimedEvent;
				if (ActivityIterator.MoveNext())
				{
					CheckNextActivity();
				}
				else
				{
					_timer.Stop();
					await _audioService.Speak(AppResources.TrainingDone);
				}
			}
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
		/// Informs the user about the next activity via Text-to-Speech. 
		/// </summary>
		/// <param name="Amount">Amount of repetitions</param>
		/// <returns>Task</returns>
		public async Task SpeakActivity(int Amount)
		{
			if (!ActiveActivity.Name.Equals(AppResources.Pause))
			{
				await _audioService.Speak(AppResources.NextActivity + Amount + "" + ActiveActivity.Name);
			}
			else
			{
				await _audioService.Speak(Amount + AppResources.Seconds + ActiveActivity.Name);
			}
		}

		/// <summary>
		/// Increases the result counter of by the done repetitions of an activity after an entry is done.
		/// </summary>
		public void IncreaseResultCounter()
		{
			if (ActiveActivity.Name.Equals(AppResources.Push_ups) || ActiveActivity.Name.Equals("Push-ups"))
			{
				_pushUpResult += ActiveActivity.Counter;
			}
			if (ActiveActivity.Name.Equals(AppResources.Sit_ups))
			{
				_sitUpResult += ActiveActivity.Counter;
			}
		}

		/// <summary>
		/// Method which handles the Stopping of the mode. Stops the timer, unregisters the event method, 
		/// saves data and shows a Pop-up.
		/// </summary>
		public override void StopActivity()
		{
			StopTimer();
			if (ActiveActivity._activity != null && ActiveActivity._activity.ActivityDone != null)
			{
				ActiveActivity._activity.ActivityDone -= OnActivityDone;
			}
			SaveData();
			ShowPopUp();
		}

		/// <summary>
		/// Stops the timer.
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
			DBEntry _entryNew = new DBEntry(_dt, 0, _pushUpResult, _sitUpResult);
			_dataBaseConnection.SaveDBEntry(_entryNew);
		}

		/// <summary>
		/// Methods which shows the amount of repetitions of each activity done by the user via a Pop-up.
		/// </summary>
		private void ShowPopUp()
		{
			_popUpService.DisplayAlert(AppResources.Result, AppResources.YouHaveDone + " " + _pushUpResult
				+ " " + AppResources.Push_ups + " " + AppResources.And + " " + _sitUpResult + " " + AppResources.Sit_ups
				+ AppResources.alternativeGrammarDone + "!", AppResources.Cool);
		}

		/// <summary>
		/// Method that adds an activity to the ActivityList via Pop-ups, called by the equivalent command.
		/// </summary>
		/// <param name="Index">Index where the activity will be inserted</param>
		private async Task AddActivity(int Index)
		{
			string newActivity = await _popUpService.ActionSheet(AppResources.SelectAnActivity,
				AppResources.Cancel, null, AppResources.Push_ups, AppResources.Sit_ups, AppResources.Pause);
			if (newActivity != null && !newActivity.Equals("") && !newActivity.Equals(AppResources.Cancel))
			{
				string newAmount = await _popUpService.DisplayPrompt(newActivity,
						AppResources.EnterRepetitions, AppResources.Okay, AppResources.Cancel, "10", 3, Keyboard.Numeric);
				if (newAmount != null && Regex.IsMatch(newAmount, @"^[1-9]{1}\d{0,2}$"))
				{
					if (newActivity.Equals(AppResources.Push_ups))
					{
						ActivityList.Insert(Index, new ActivityWrapper(AppResources.Push_ups, _pushUpActivity, int.Parse(newAmount)));
					}
					if (newActivity.Equals(AppResources.Sit_ups))
					{
						ActivityList.Insert(Index, new ActivityWrapper(AppResources.Sit_ups, _sitUpActivity, int.Parse(newAmount)));
					}
					if (newActivity.Equals(AppResources.Pause))
					{
						ActivityList.Insert(Index, new ActivityWrapper(AppResources.Pause, null, int.Parse(newAmount)));
					}
					_inserted = true;
				}
			}
		}

		/// <summary>
		/// Method that removes the selected activity from the ActivityList, called by the equivalent command.
		/// </summary>
		private void RemoveActivity()
		{
			if (ActivityList.Count > 0 && ActivityList.Contains(SelectedActivity))
			{
				ActivityList.Remove(SelectedActivity);
			}
		}

		/// <summary>
		/// Method that edit the selected activity from the ActivityList, called by the equivalent command.
		/// </summary>
		private async void EditActivity()
		{
			if (SelectedActivity != null && ActivityList.Contains(SelectedActivity))
			{
				int Index = ActivityList.IndexOf(SelectedActivity);
				_inserted = false;
				await AddActivity(Index);
				if (_inserted)
				{
					ActivityList.Remove(SelectedActivity);
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}
