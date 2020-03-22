using System;
using System.Collections.Generic;
using System.Diagnostics;
using EarablesKIT.Models;
using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.RunningActivity;
using EarablesKIT.Models.Extentionmodel.Activities.StepActivity;
using EarablesKIT.Models.Library;
using EarablesKIT.Models.PopUpService;
using EarablesKIT.Models.SettingsService;
using EarablesKIT.ViewModels;
using Moq;
using Xamarin.Forms;
using Xunit;

namespace ViewModelTests
{
	public class StepModeViewModelTest
	{
		[Fact]
		public void ConstructorCheckWithoutData()
		{
			//Für den ServiceProviderMock
			//Muss enthalten sein, damit der Mock nicht überschrieben wird
			IServiceProvider unused = ServiceManager.ServiceProvider;

			//Feld Infos holen
			System.Reflection.FieldInfo instance = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

			//Mocksaufsetzen 
			//ServiceProvider
			Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();
			Mock<IActivityManager> activityManagerMock = new Mock<IActivityManager>();
			Mock<IServiceProvider> activityProviderMock = new Mock<IServiceProvider>();
			Mock<AbstractStepActivity> stepActivityMock = new Mock<AbstractStepActivity>();
			Mock<AbstractRunningActivity> runningActivityMock = new Mock<AbstractRunningActivity>();
			Mock<IPopUpService> popUpMock = new Mock<IPopUpService>();

			//ActivityManager
			activityManagerMock.Setup(x => x.ActitvityProvider).Returns(activityProviderMock.Object);
			activityProviderMock.Setup(x => x.GetService(typeof(AbstractRunningActivity))).Returns(runningActivityMock.Object);
			activityProviderMock.Setup(x => x.GetService(typeof(AbstractStepActivity))).Returns(stepActivityMock.Object);

			//IDataBaseConnection
			Mock<IDataBaseConnection> mockDataBase = new Mock<IDataBaseConnection>();
			List<DBEntry> entries = new List<DBEntry>();
			mockDataBase.As<IDataBaseConnection>().Setup(x => x.GetMostRecentEntries(1)).Returns(entries);

			DateTime _dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			DBEntry _entryNew = new DBEntry(_dt, 10, 0, 0);
			mockDataBase.As<IDataBaseConnection>().Setup(x => x.SaveDBEntry(_entryNew)).Returns(1);

			//ServiceManager
			mockSingleton.Setup(x => x.GetService(typeof(IDataBaseConnection))).Returns(mockDataBase.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IActivityManager))).Returns(activityManagerMock.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IPopUpService))).Returns(popUpMock.Object);

			//ServiceProvider anlegen
			instance.SetValue(null, mockSingleton.Object);

			//Test
			StepModeViewModel viewModel = new StepModeViewModel();
			Assert.Equal(0, viewModel.DistanceWalked);
			Assert.Equal(0, viewModel.DistanceWalkedLastTime);
			Assert.Equal(0, viewModel.StepsDoneLastTime);
			Assert.Equal("01.01.2000", viewModel.LastDataTime);
			Assert.Equal(DateTime.Now.ToString(), viewModel.CurrentDate);
			Assert.False(viewModel.IsRunning);
		}

		[Fact]
		public void ConstructorCheckWithData()
		{
			//Für den ServiceProviderMock
			//Muss enthalten sein, damit der Mock nicht überschrieben wird
			IServiceProvider unused = ServiceManager.ServiceProvider;

			//Feld Infos holen
			System.Reflection.FieldInfo instance = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

			//Mocksaufsetzen 
			//ServiceProvider
			Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();
			Mock<IActivityManager> activityManagerMock = new Mock<IActivityManager>();
			Mock<IServiceProvider> activityProviderMock = new Mock<IServiceProvider>();
			Mock<AbstractStepActivity> stepActivityMock = new Mock<AbstractStepActivity>();
			Mock<AbstractRunningActivity> runningActivityMock = new Mock<AbstractRunningActivity>();
			Mock<ISettingsService> settingsMock = new Mock<ISettingsService>();
			Mock<IPopUpService> popUpMock = new Mock<IPopUpService>();

			//ActivityManager
			activityManagerMock.Setup(x => x.ActitvityProvider).Returns(activityProviderMock.Object);
			activityProviderMock.Setup(x => x.GetService(typeof(AbstractRunningActivity))).Returns(runningActivityMock.Object);
			activityProviderMock.Setup(x => x.GetService(typeof(AbstractStepActivity))).Returns(stepActivityMock.Object);

			//IDataBaseConnection
			Mock<IDataBaseConnection> mockDataBase = new Mock<IDataBaseConnection>();
			List<DBEntry> entries = new List<DBEntry>();
			DBEntry one = new DBEntry(
				new DateTime(2020, 3, 11), 100, 200, 30);
			DBEntry two = new DBEntry(
				new DateTime(2000, 4, 12), 100, 20, 30);
			entries.Add(one);
			entries.Add(two);

			mockDataBase.As<IDataBaseConnection>().Setup(x => x.GetMostRecentEntries(1)).Returns(entries);

			//SettingsService
			User user = new User("Thorsten", 70);
			settingsMock.As<ISettingsService>().SetupProperty(x => x.ActiveUser, user);

			//ServiceManager
			mockSingleton.Setup(x => x.GetService(typeof(IDataBaseConnection))).Returns(mockDataBase.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IActivityManager))).Returns(activityManagerMock.Object);
			mockSingleton.Setup(x => x.GetService(typeof(ISettingsService))).Returns(settingsMock.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IPopUpService))).Returns(popUpMock.Object);

			//ServiceProvider anlegen
			instance.SetValue(null, mockSingleton.Object);

			//Test
			StepModeViewModel viewModel = new StepModeViewModel();
			Assert.Equal(0, viewModel.DistanceWalked);
			Assert.Equal(70, viewModel.DistanceWalkedLastTime);
			Assert.Equal(100, viewModel.StepsDoneLastTime);
			Assert.Equal("11.03.2020", viewModel.LastDataTime);
			Assert.False(viewModel.IsRunning);
		}

		[Fact]
		public void StartActivityCheck()
		{
			//Für den ServiceProviderMock
			//Muss enthalten sein, damit der Mock nicht überschrieben wird
			IServiceProvider unused = ServiceManager.ServiceProvider;

			//Feld Infos holen
			System.Reflection.FieldInfo instance = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

			//Mocksaufsetzen 
			//ServiceProvider
			Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();
			Mock<IActivityManager> activityManagerMock = new Mock<IActivityManager>();
			Mock<IServiceProvider> activityProviderMock = new Mock<IServiceProvider>();
			Mock<AbstractStepActivity> stepActivityMock = new Mock<AbstractStepActivity>();
			Mock<AbstractRunningActivity> runningActivityMock = new Mock<AbstractRunningActivity>();
			Mock<IEarablesConnection> connectionMock = new Mock<IEarablesConnection>();
			Mock<IPopUpService> popUpMock = new Mock<IPopUpService>();

			//ActivityManager
			activityManagerMock.Setup(x => x.ActitvityProvider).Returns(activityProviderMock.Object);
			activityProviderMock.Setup(x => x.GetService(typeof(AbstractRunningActivity))).Returns(runningActivityMock.Object);
			activityProviderMock.Setup(x => x.GetService(typeof(AbstractStepActivity))).Returns(stepActivityMock.Object);

			//IDataBaseConnection
			Mock<IDataBaseConnection> mockDataBase = new Mock<IDataBaseConnection>();
			List<DBEntry> entries = new List<DBEntry>();
			mockDataBase.As<IDataBaseConnection>().Setup(x => x.GetMostRecentEntries(1)).Returns(entries);

			DateTime _dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			DBEntry _entryNew = new DBEntry(_dt, 10, 0, 0);
			mockDataBase.As<IDataBaseConnection>().Setup(x => x.SaveDBEntry(_entryNew)).Returns(1);

			//ServiceManager
			mockSingleton.Setup(x => x.GetService(typeof(IDataBaseConnection))).Returns(mockDataBase.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IActivityManager))).Returns(activityManagerMock.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IEarablesConnection))).Returns(connectionMock.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IPopUpService))).Returns(popUpMock.Object);

			//Connection
			connectionMock.As<IEarablesConnection>().Setup(x => x.StartSampling());


			//StepActivity
			//stepActivityMock.As<AbstractStepActivity>().SetupAdd(x => x.ActivityDone += (sender, args) => { });

			//ServiceProvider anlegen
			instance.SetValue(null, mockSingleton.Object);

			//Test
			StepModeViewModel viewModel = new StepModeViewModel();
			ScanningPopUpViewModel.IsConnected = true;
			viewModel.StartActivity();
			Assert.Equal(0, viewModel.StepCounter);
			Assert.Equal(DateTime.Now.ToString(), viewModel.CurrentDate);
			//stepActivityMock.VerifyAdd(m => m.ActivityDone += It.IsAny<EventHandler<ActivityArgs>>(), Times.Exactly(1));


		}

		[Fact]
		public void CounterStatusCheck()
		{
			//Für den ServiceProviderMock
			//Muss enthalten sein, damit der Mock nicht überschrieben wird
			IServiceProvider unused = ServiceManager.ServiceProvider;

			//Feld Infos holen
			System.Reflection.FieldInfo instance = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

			//Mocksaufsetzen 
			//ServiceProvider
			Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();
			Mock<IActivityManager> activityManagerMock = new Mock<IActivityManager>();
			Mock<IServiceProvider> activityProviderMock = new Mock<IServiceProvider>();
			Mock<AbstractStepActivity> stepActivityMock = new Mock<AbstractStepActivity>();
			Mock<AbstractRunningActivity> runningActivityMock = new Mock<AbstractRunningActivity>();
			Mock<IEarablesConnection> connectionMock = new Mock<IEarablesConnection>();
			Mock<IPopUpService> popUpMock = new Mock<IPopUpService>();

			//ActivityManager
			activityManagerMock.Setup(x => x.ActitvityProvider).Returns(activityProviderMock.Object);
			activityProviderMock.Setup(x => x.GetService(typeof(AbstractRunningActivity))).Returns(runningActivityMock.Object);
			activityProviderMock.Setup(x => x.GetService(typeof(AbstractStepActivity))).Returns(stepActivityMock.Object);

			//IDataBaseConnection
			Mock<IDataBaseConnection> mockDataBase = new Mock<IDataBaseConnection>();
			List<DBEntry> entries = new List<DBEntry>();
			mockDataBase.As<IDataBaseConnection>().Setup(x => x.GetMostRecentEntries(1)).Returns(entries);

			DateTime _dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			DBEntry _entryNew = new DBEntry(_dt, 10, 0, 0);
			mockDataBase.As<IDataBaseConnection>().Setup(x => x.SaveDBEntry(_entryNew)).Returns(1);

			//ServiceManager
			mockSingleton.Setup(x => x.GetService(typeof(IDataBaseConnection))).Returns(mockDataBase.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IActivityManager))).Returns(activityManagerMock.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IEarablesConnection))).Returns(connectionMock.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IPopUpService))).Returns(popUpMock.Object);

			//Connection
			ScanningPopUpViewModel.IsConnected = true;
			connectionMock.As<IEarablesConnection>().Setup(x => x.StartSampling());

			//ServiceProvider anlegen
			instance.SetValue(null, mockSingleton.Object);

			//Test
			StepModeViewModel viewModel = new StepModeViewModel();
			viewModel.StartActivity();
			Assert.Equal("--:--", viewModel.StepFrequency);
			viewModel.OnActivityDone(this, null);
			viewModel.OnActivityDone(this, null);
			viewModel.OnActivityDone(this, null);
			viewModel.OnRunningDone(this, new RunningEventArgs(true));
			Assert.Equal(3, viewModel.StepCounter);
			Assert.True(viewModel.IsRunning);
			Assert.NotNull(viewModel.StepFrequency);
			Assert.Equal("Du läufst!", viewModel.StatusDisplay);
			viewModel.OnRunningDone(this, new RunningEventArgs(false));
			Assert.False(viewModel.IsRunning);

			viewModel.StopActivity();
			Assert.Equal(3, viewModel.StepCounter);
			viewModel.StartActivity();
			Assert.Equal(0, viewModel.StepCounter);
			Assert.False(viewModel.IsRunning);
		}
	}
}
