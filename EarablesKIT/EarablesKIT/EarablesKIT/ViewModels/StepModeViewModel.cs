using EarablesKIT.Models;
using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.RunningActivity;
using EarablesKIT.Models.Extentionmodel.Activities.StepActivity;
using EarablesKIT.Models.SettingsService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
	class StepModeViewModel : BaseModeViewModel, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private Stopwatch _timer;
		private AbstractStepActivity _stepActivity { get; set; }
		private AbstractRunningActivity _runningActivity { get; set; }
		private IActivityManager _activityManager { get; set; }

		private String _lastDataTime, _currentDate;
		private int _stepCounter, _distanceWalked, _stepsDoneLastTime, _distanceWalkedLastTime;
		private bool _isRunning;
		private double _stepFrequency;

		public String CurrentDate
		{
			get { return _currentDate; }
			set
			{
				_currentDate = value;
				OnPropertyChanged();
			}
		}
		public int StepsDoneLastTime
		{
			get { return _stepsDoneLastTime; }
			set
			{
				_stepsDoneLastTime = value;
				OnPropertyChanged();
			}
		}
		public int DistanceWalkedLastTime
		{
			get { return _distanceWalkedLastTime; }
			set
			{
				_distanceWalkedLastTime = value;
				OnPropertyChanged();
			}
		}
		public string LastDataTime
		{
			get { return _lastDataTime; }
			set
			{
				_lastDataTime = value;
				OnPropertyChanged();
			}
		}
		public int StepCounter
		{
			get { return _stepCounter; }
			set
			{
				_stepCounter = value;
				OnPropertyChanged();
			}
		}
		public int DistanceWalked
		{
			get { return _distanceWalked; }
			set
			{
				_distanceWalked = value;
				OnPropertyChanged();
			}
		}
		public bool IsRunning
		{
			get { return _isRunning; }
			set
			{
				_isRunning = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(StatusDisplay));
			}
		}
		public double StepFrequency
		{
			get { return _stepFrequency; }
			set
			{
				_stepFrequency = value;
				OnPropertyChanged();
			}
		}

		public string StatusDisplay
		{
			get
			{
				return $"You are " +
					   $"{(IsRunning ? "Walking!!" : "Standing")} ";
			}
		}


		public StepModeViewModel()
		{
			//_activityManager = (IActivityManager)ServiceManager.ServiceProvider.GetService(typeof(IActivityManager));
			//_stepActivity = (AbstractStepActivity)_activityManager.ActitvityProvider.GetService(typeof(AbstractStepActivity));
			//_runningActivity = (AbstractRunningActivity)ServiceManager.ServiceProvider.GetService(typeof(AbstractRunningActivity));
			DistanceWalkedLastTime = 0;
			StepsDoneLastTime = 0;
			StepFrequency = 0.0;
			DistanceWalked = 0;
			LastDataTime = "DD.MM.YY";
			CurrentDate = DateTime.Now.ToString();
			UpdateLastData();
			IsRunning = false;
			_timer = new Stopwatch();
		}

		public override void OnActivityDone(object sender, ActivityArgs args)
		{
			StepCounter++;
		}

		public void OnRunningDone(object sender, ActivityArgs args)
		{
			RunningEventArgs RunningEvent = (RunningEventArgs)args;
			//IsRunning = RunningEvent.Running; 
		}

		protected void OnPropertyChanged([CallerMemberName] string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		public override bool StartActivity()
		{
			if (CheckConnection())
			{
				StepCounter = 0;
				StepFrequency = 0;
				//_stepActivity.ActivityDone += OnActivityDone;
				//_runningActivity.ActivityDone += OnRunningDone;
				CurrentDate = DateTime.Now.ToString();
				return true;
			}
			return false;
		}

		public void HandlingTimer()
		{
			_timer = new Stopwatch();
			_timer.Start();
			Device.StartTimer(TimeSpan.FromSeconds(3.0), () =>
			{
				UpdateFrequency();
				return true;
			});
		}

		public override void StopActivity()
		{
			_timer.Stop();
			//_stepActivity.ActivityDone -= OnActivityDone;
			//_runningActivity.ActivityDone -= OnRunningDone;
			IsRunning = false;
			ShowPopUp();
			UpdateLastData();
		}

		private void UpdateLastData()
		{
			//IDataBaseConnection DatabaseService = (IDataBaseConnection)ServiceManager.ServiceProvider.GetService(typeof(IDataBaseConnection));
			//List<DBEntry> Entries = (List<DBEntry>)DatabaseService.GetMostRecentEntriesAsync(1).Result;
			//DBEntry entry = Entries[0];
			//LastDataTime = entry.Date.ToString();
			//StepsDoneLastTime = entry.TrainingsData["steps"].ToString();
			//ISettingsService setser = (ISettingsService)ServiceManager.ServiceProvider.GetService(typeof(ISettingsService));
			//int dwlt = entry.TrainingsData["steps"] * setser.MyUser.Steplength;
			//DistanceWalkedLastTime = dwlt.ToString();
		}


		private void UpdateFrequency()
		{
			double totalTime = _timer.Elapsed.Seconds;
			StepFrequency = 60 * StepCounter / totalTime;
		}

		private void ShowPopUp()
		{
			App.Current.MainPage.DisplayAlert("Result", "You have taken " + StepCounter + " Steps!", "Cool");
		}



	}
}
