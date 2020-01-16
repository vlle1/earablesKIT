using EarablesKIT.Models;
using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.RunningActivity;
using EarablesKIT.Models.Extentionmodel.Activities.StepActivity;
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

		private Stopwatch timer;
		private AbstractStepActivity stepActivity { get; set; }
		private AbstractRunningActivity runningActivity { get; set; }
		private IActivityManager activityManager { get; set; }
		public string StepsDoneLastTime
		{
			get { return StepsDoneLastTime; }
			set
			{
				StepsDoneLastTime = value;
				OnPropertyChanged();
			}
		}
		public string DistanceWalkedLastTime
		{
			get { return DistanceWalkedLastTime; }
			set
			{
				DistanceWalkedLastTime = value;
				OnPropertyChanged();
			}
		}
		public string LastDataTime
		{
			get { return LastDataTime; }
			set
			{
				LastDataTime = value;
				OnPropertyChanged();
			}
		}
		public int StepCounter
		{
			get { return StepCounter; }
			set
			{
				StepCounter = value;
				OnPropertyChanged();
			}
		}
		public int DistanceWalked
		{
			get { return DistanceWalked; }
			set
			{
				DistanceWalked = value;
				OnPropertyChanged();
			}
		}
		public bool IsRunning
		{
			get { return IsRunning; }
			set
			{
				IsRunning = value;
				OnPropertyChanged();
			}
		}
		public double StepFrequency
		{
			get { return StepFrequency; }
			set
			{
				StepFrequency = value;
				OnPropertyChanged();
			}
		}


		public StepModeViewModel()
		{
			StartActivityCommand = new Command(() => StartActivity());
			StopActivityCommand = new Command(() => StopActivity());
			activityManager = (IActivityManager) ServiceManager.ServiceProvider.GetService(typeof(IActivityManager));
			stepActivity = (AbstractStepActivity) activityManager.ActitvityProvider.GetService(typeof(AbstractStepActivity));
			runningActivity = (AbstractRunningActivity) ServiceManager.ServiceProvider.GetService(typeof(AbstractRunningActivity));
			UpdateLastData();
			IsRunning = false;
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

		protected override void StartActivity()
		{
			CheckConnection();
			StepCounter = 0;
			stepActivity.ActivityDone += OnActivityDone;
			runningActivity.ActivityDone += OnRunningDone;
			timer.Start();
			//Navigation.PushAsync(new StopModeActiveView());
			Device.StartTimer(TimeSpan.FromSeconds(3.0), () =>
			{
				UpdateFrequency();
				return true;
			});
		}

		protected override void StopActivity()
		{
			timer.Stop();
			stepActivity.ActivityDone -= OnActivityDone;
			runningActivity.ActivityDone -= OnRunningDone;
			IsRunning = false;
			ShowPopUp();
			UpdateLastData();
		}

		private void UpdateLastData()
		{
			//IDataBaseConnection DatabaseService = ServiceManager.ServiceProvider.GetService<IDataBaseConnection>();
			//DBEntry entry = DatabaseService.getMostRecentEntriesAsync(1)[0];
			//LastDataTime = entry.Date().ToString();
			//StepsDoneLastTime = entry.TrainingsData["steps"]
			//DistanceWalkedLastTime = StepsDoneLastTime * Steplength
		}


		private void UpdateFrequency()
		{
			double totalTime = timer.Elapsed.Seconds;
			StepFrequency = 60 * StepCounter / totalTime; //Schritte pro Minute
		}

		private void ShowPopUp()
		{
			App.Current.MainPage.DisplayAlert("Result", "You have done " + StepCounter + " Steps!.", "Cool");
		}



	}
}
