using EarablesKIT.Models;
using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.RunningActivity;
using EarablesKIT.Models.Extentionmodel.Activities.StepActivity;
using EarablesKIT.Models.Library;

using EarablesKIT.Models.SettingsService;
using EarablesKIT.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
	/// <summary>
	/// Class that represents the StepMode. Manages the progress of the StepMode, implements the BaseModeViewModel.
	/// </summary>
	public class StepModeViewModel : BaseModeViewModel, INotifyPropertyChanged
	{
		/// <summary>
		/// Timer that is needed for calculating the step frequency, not binded to the view.
		/// </summary>
		private Stopwatch _timer;
		/// <summary>
		/// Saves relative timestamps of the last MAX_REGRESSION_LENGTH Steps, while walking. Unit: minutes
		/// </summary>
		private Queue<double> _stepMemory = new Queue<double>();
		private const int MAX_REGRESSION_LENGTH = 4;

		/// <summary>
		/// The stepActivity from the ActivityProvider.
		/// </summary>
		private AbstractStepActivity _stepActivity { get; set; }

		/// <summary>
		/// The runningActivity from the ActivityProvider.
		/// </summary>
		private AbstractRunningActivity _runningActivity { get; set; }

		/// <summary>
		/// Property which holds the instance of the ActivityManager.
		/// </summary>
		private IActivityManager _activityManager { get; set; }

		/// <summary>
		/// Property which hold the instance of the DataBaseConnection.
		/// </summary>
		private IDataBaseConnection _dataBaseConnection { get; set; }

		/// <summary>
		/// Property which holds the current date.
		/// </summary>
		private string _currentDate;
		public string CurrentDate
		{
			get { return _currentDate; }
			set
			{
				_currentDate = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Property which holds the amount of steps that the user has done on the last trainingsday, Bound to the view.
		/// </summary>
		private int _stepsDoneLastTime;
		public int StepsDoneLastTime
		{
			get { return _stepsDoneLastTime; }
			set
			{
				_stepsDoneLastTime = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Property which holds the distance that the user has walked the last time, bound to the view.
		/// </summary>
		private int _distanceWalkedLastTime;
		public int DistanceWalkedLastTime
		{
			get { return _distanceWalkedLastTime; }
			set
			{
				_distanceWalkedLastTime = value;
				OnPropertyChanged();
			}
		}


		/// <summary>
		/// Property which holds the date of the time last the user has started the mode, bound to the view.
		/// </summary>
		private string _lastDataTime;
		public string LastDataTime
		{
			get { return _lastDataTime; }
			set
			{
				_lastDataTime = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Property which holds the amount of steps that the user has done during the active mode, bound to the view.
		/// </summary>
		private int _stepCounter;
		public int StepCounter
		{
			get { return _stepCounter; }
			set
			{
				_stepCounter = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Property which holds the distance that the user has walked during the active mode.
		/// </summary>
		private int _distanceWalked;
		public int DistanceWalked
		{
			get { return _distanceWalked; }
			set
			{
				_distanceWalked = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Holds the current state of the user.
		/// </summary>
		private bool _isRunning;
		public bool IsRunning
		{
			get { return _isRunning; }
			set
			{
				_isRunning = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(StatusDisplay));
				OnPropertyChanged(nameof(StepFrequency));
            }
		}

		/// <summary>
		/// Holds the current step frequency. 
		/// </summary>
		public string StepFrequency
		{
			get 
			{
				if (!_isRunning) return "--:--";
				double[] timestamps = _stepMemory.ToArray();
				int n = timestamps.Length;
				if (n < 3)
				{
					String output = ".";
					for (int i = 0; i < n; i++)
					{
						output += " .";
					}
					return output;
				}
				//do linear regression: X-Axis: time, Y-Axis: steps 
				//steps are artificially set to values 0,1,2,...,n
				//return Math.Round(timestamps[n-1],2).ToString();
				//calculate arithmetic medium
				double arithMX = 0;
				foreach (float d in timestamps) arithMX += d;
				arithMX /= n;
				double arithMY = n / 2;
				//use simple formula to calculate gradient 
				double upperSum = 0;
				double lowerSum = 0;
				for (int i = 0; i < n; i++)
				{
					
					double x_diff = timestamps[i] - arithMX;
					double y_diff = i - arithMY;
					upperSum += x_diff * y_diff;
					lowerSum += x_diff * x_diff;
				}
				//this should never happen as the timestamps would need to be incredibly close together,
				//but avoid dividing by zero durch null teilen
				if (Math.Abs(lowerSum) < 0.000001f) return "N.A.";
				double result = upperSum / lowerSum;
				
				return Math.Round(result, 2).ToString();
			}
			
		}

		/// <summary>
		/// Display message for the current status of the user, updated when the status changes.
		/// </summary>
		public string StatusDisplay
		{
			get
			{
				return AppResources.YouAre + " " + (IsRunning ? AppResources.Walking : AppResources.Standing);
			}
		}

		/// <summary>
		/// Requests different services from the ServiceProvider and initializes the values which are displayed on the view. 
		/// </summary>
		public StepModeViewModel()
		{
			_activityManager = (IActivityManager)ServiceManager.ServiceProvider.GetService(typeof(IActivityManager));
			_stepActivity = (AbstractStepActivity)_activityManager.ActitvityProvider.GetService(typeof(AbstractStepActivity));
			_runningActivity = (AbstractRunningActivity)_activityManager.ActitvityProvider.GetService(typeof(AbstractRunningActivity));
			_dataBaseConnection = (IDataBaseConnection)ServiceManager.ServiceProvider.GetService(typeof(IDataBaseConnection));
			DistanceWalkedLastTime = 0;
			StepsDoneLastTime = 0;
			DistanceWalked = 0;
			LastDataTime = "01.01.2000"; 
			CurrentDate = DateTime.Now.ToString(); 
			UpdateLastData();
			IsRunning = false;
			_timer = new Stopwatch();
		}

		/// <summary>
		/// Method which handles the start of the mode. Checks for a connection to the Earables, 
		/// registers the event methods and starts the sampling of the Earables.
		/// </summary>
		/// <returns></returns>
		public override bool StartActivity()
		{
			if (CheckConnection())
			{
				StepCounter = 0;
				_stepActivity.ActivityDone += OnActivityDone;
				_runningActivity.ActivityDone += OnRunningDone;
				CurrentDate = DateTime.Now.ToString();
				((IEarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection))).StartSampling();
				_timer.Start();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Increases the step counter by 1 whenever an event is thrown.
		/// </summary>
		/// <param name="sender">The sender of the event</param>
		/// <param name="args">Ignored</param>
		public override void OnActivityDone(object sender, ActivityArgs args)
		{
			StepCounter++;
			//update step frequency!
			_stepMemory.Enqueue(_timer.ElapsedMilliseconds / 60000f);
			if (_stepMemory.Count > MAX_REGRESSION_LENGTH) _stepMemory.Dequeue();
			OnPropertyChanged(nameof(StepFrequency));
		}

		/// <summary>
		/// Method which will be invoked when the running state of the user changes.
		/// </summary>
		/// <param name="sender">The sender of the event</param>
		/// <param name="args">New state of the user</param>
		public void OnRunningDone(object sender, ActivityArgs args)
		{
			IsRunning = ((RunningEventArgs)args).Running;
			if (!IsRunning)
			{
				_stepMemory.Clear();
			}
			OnPropertyChanged(nameof(StepFrequency));
		}

		/// <summary>
		/// Method which handles the Stopping of the mode. Stops the timer, unregisters the event methods, 
		/// saves data, shows a Pop-up and updates the data on the overview page of the step mode.
		/// </summary>
		public override void StopActivity()
		{
			_timer.Stop();
			_stepActivity.ActivityDone -= OnActivityDone;
			_runningActivity.ActivityDone -= OnRunningDone;
			IsRunning = false;
			SaveData();
			ShowPopUp();
			UpdateLastData();
		}

		/// <summary>
		/// Method which saves the amount of steps taken by the user.
		/// </summary>
		private void SaveData()
		{
			DateTime _dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			DBEntry _entryNew = new DBEntry(_dt, _stepCounter, 0, 0);
			_dataBaseConnection.SaveDBEntry(_entryNew);
		}

		/// <summary>
		/// Method which shows a Pop-up with the amount of steps taken by the user.
		/// </summary>
		private void ShowPopUp()
		{
			Application.Current.MainPage.DisplayAlert(AppResources.Result, AppResources.YouHaveTaken + " " + StepCounter + " " + AppResources.Steps + AppResources.alternativeGrammarDone + ".", AppResources.Cool);
		}

		/// <summary>
		/// Method which updates the values that are displayed on the overview page of the stepmode.
		/// </summary>
		private void UpdateLastData()
		{
			List<DBEntry> Entries = _dataBaseConnection.GetMostRecentEntries(1);
			if (Entries.Count >= 1)
			{
				DBEntry entry = Entries[0];
				LastDataTime = entry.Date.ToString("dd.MM.yyyy");
				StepsDoneLastTime = entry.TrainingsData["Steps"];
				ISettingsService setser = (ISettingsService)ServiceManager.ServiceProvider.GetService(typeof(ISettingsService));
				int dwlt = StepsDoneLastTime * setser.ActiveUser.Steplength / 100;
				DistanceWalkedLastTime = dwlt;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}



	}
}
