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
			IsRunning = false;
			//var activityManager = ServiceManager.ServiceProvider.GetService(IActivityManager);
			//IActivityManager activityManager = DependencyService.Get<IActivityManager>();
			//stepActivity = ServiceManager.ServiceProvider.GetService(AbstractStepActivity);
			//runningActivity = ServiceManager.ServiceProvider.GetService(AbstractRunningActivity);
			UpdateLastData();

		}

		public override void OnActivityDone(object sender, ActivityArgs args)
		{
			StepCounter++;
		}

		public void OnRunningDone(object sender, ActivityArgs args)
		{
			IsRunning = true; //??
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
			//IDataBaseConnection DatabaseService = ServiceManager.ServiceProvider.GetService<IDataBaseConnection();
			//DBEntry entry = DatabaseService.getMostRecentEntriesAsync(1)[0];
			//LastDataTime = entry.Date().ToString();
			//StepsDoneLastTime = entry.TrainingsData[steps]
			//DistanceWalkedLastTime = StepsDoneLastTime * Steplength
		}


		private void UpdateFrequency()
		{
			double totalTime = timer.Elapsed.Seconds;
			StepFrequency = 60 * StepCounter / totalTime; //Schritte pro Minute
		}

		private void ShowPopUp()
		{
			//await DisplayAlert("Result", "You have done " + StepCounter + " Steps!.", "Cool");
			//Liegt im CodeBehind
		}



	}
}
