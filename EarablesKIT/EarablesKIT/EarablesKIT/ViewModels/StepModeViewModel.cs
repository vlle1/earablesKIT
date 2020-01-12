using EarablesKIT.Models;
using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.RunningActivity;
using EarablesKIT.Models.Extentionmodel.Activities.StepActivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
		public string StepsDoneLastTime { get; set; }
		public string DistanceWalkedLastTime { get; set; }
		public string LastDataTime { get; set; }
		public int StepCounter { get; set; }
		public int DistanceWalked { get; set; }
		public bool IsRunning { get; set; }
		public double StepFrequency { get; set; }
		

		public StepModeViewModel()
		{
			StartActivityCommand = new Command(() => StartActivity());
			StopActivityCommand = new Command(() => StopActivity());
			IsRunning = false;
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
			IsRunning = true;
		}

		protected void OnPropertyChanged()
		{
			throw new NotImplementedException();
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
			stepActivity.ActivityDone -= OnActivityDone;
			runningActivity.ActivityDone -= OnRunningDone;
			IsRunning = false;
			ShowPopUp();
			UpdateLastData();
		}

		private void UpdateLastData()
		{
			throw new NotImplementedException();
		}


		private void UpdateFrequency()
		{
			double totalTime = timer.Elapsed.Seconds;
			StepFrequency = 60 * StepCounter / totalTime; //Schritte pro Minute
		}

		private void ShowPopUp()
		{
			//await DisplayAlert ("Result", "You have done " + StepCounter " Steps!.", "Cool");
		}



	}
}
