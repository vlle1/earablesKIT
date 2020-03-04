using System;
using System.Diagnostics;
using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.RunningActivity;
using EarablesKIT.Models.Extentionmodel.Activities.StepActivity;
using EarablesKIT.Models.Library;
using EarablesKIT.ViewModels;
using Xamarin.Forms;
using Xunit;

namespace ViewModelTests
{
	public class StepModeViewModelTest
	{
		private StepModeViewModel TestVM;

		private void SetUp()
		{
			TestVM = new StepModeViewModel("mock");
			TestVM._runningActivity = new RunningActivityThreshold();
			TestVM._stepActivity = new StepActivityThreshold();
			TestVM._dataBaseConnection = new DatabaseConnection();
			ScanningPopUpViewModel.IsConnected = true;
			//TestVM._samplingStarter = new EarablesConnection();

		}
		[Fact]
		public void EventHandlerCheck()
		{
			SetUp();
			TestVM.StartActivity("Mock");
			Assert.Single(TestVM._runningActivity.ActivityDone.GetInvocationList());
			Assert.Single(TestVM._stepActivity.ActivityDone.GetInvocationList());
			TestVM.StopActivity("Mock");
			Assert.Null(TestVM._runningActivity.ActivityDone);
			Assert.Null(TestVM._stepActivity.ActivityDone);
		}

		[Fact]
		public void CounterCheck()
		{
			SetUp();
			TestVM.StartActivity("Mock");
			TestVM.OnActivityDone(this, new StepEventArgs());
			TestVM.OnActivityDone(this, new StepEventArgs());
			TestVM.OnActivityDone(this, new StepEventArgs());
			Assert.Equal(3, TestVM.StepCounter);
			TestVM.StepCounter = 0;
			TestVM.OnActivityDone(this, new StepEventArgs());
			TestVM.OnActivityDone(this, new StepEventArgs());
			Assert.Equal(2, TestVM.StepCounter);
		}

		[Fact]
		public void RunningStateCheck()
		{
			SetUp();
			TestVM.StartActivity("Mock");
			TestVM.OnRunningDone(this, new RunningEventArgs(true));
			Assert.True(TestVM.IsRunning);
			TestVM.OnRunningDone(this, new RunningEventArgs(false));
			Assert.False(TestVM.IsRunning);

			TestVM.IsRunning = false;
			TestVM.OnRunningDone(this, new RunningEventArgs(true));
			TestVM.OnRunningDone(this, new RunningEventArgs(true));
			Assert.True(TestVM.IsRunning);
		}

		[Fact(Skip = "No device")]
		public void FrequencyCheck()
		{
			SetUp();
			TestVM.UpdateFrequency();
			Assert.Equal(3, TestVM.StepCounter);
		}
	}
}
