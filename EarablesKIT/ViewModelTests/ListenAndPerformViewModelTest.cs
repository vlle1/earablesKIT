using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EarablesKIT.Models;
using EarablesKIT.Models.AudioService;
using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities.PushUpActivity;
using EarablesKIT.Models.Extentionmodel.Activities.SitUpActivity;
using EarablesKIT.Models.Library;
using EarablesKIT.Models.PopUpService;
using EarablesKIT.ViewModels;
using Moq;
using Xamarin.Forms;
using Xunit;

namespace ViewModelTests
{
	public class ListenAndPerformViewModelTest
	{
		[Fact]
		public void ConstructorCheck()
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
			Mock<AbstractSitUpActivity> sitUpActivityMock = new Mock<AbstractSitUpActivity>();
			Mock<AbstractPushUpActivity> pushUpActivityMock = new Mock<AbstractPushUpActivity>();
			Mock<IEarablesConnection> connectionMock = new Mock<IEarablesConnection>();
			Mock<IPopUpService> popUpMock = new Mock<IPopUpService>();

			//ActivityManager
			activityManagerMock.Setup(x => x.ActitvityProvider).Returns(activityProviderMock.Object);
			activityProviderMock.Setup(x => x.GetService(typeof(AbstractSitUpActivity))).Returns(sitUpActivityMock.Object);
			activityProviderMock.Setup(x => x.GetService(typeof(AbstractPushUpActivity))).Returns(pushUpActivityMock.Object);

			//IDataBaseConnection
			Mock<IDataBaseConnection> mockDataBase = new Mock<IDataBaseConnection>();

			DateTime _dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			DBEntry _entryNew = new DBEntry(_dt, 10, 0, 0);
			mockDataBase.As<IDataBaseConnection>().Setup(x => x.SaveDBEntry(_entryNew)).Returns(1);

			//ServiceManager
			mockSingleton.Setup(x => x.GetService(typeof(IDataBaseConnection))).Returns(mockDataBase.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IActivityManager))).Returns(activityManagerMock.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IPopUpService))).Returns(popUpMock.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IEarablesConnection))).Returns(connectionMock.Object);

			//Connection
			ScanningPopUpViewModel.IsConnected = true;
			connectionMock.As<IEarablesConnection>().Setup(x => x.StartSampling());

			//ServiceProvider anlegen
			instance.SetValue(null, mockSingleton.Object);

			//Test
			ListenAndPerformViewModel viewModel = new ListenAndPerformViewModel();
			IEnumerator<ActivityWrapper> iterator = viewModel.ActivityList.GetEnumerator();
			Assert.Equal(3, viewModel.ActivityList.Count);
			iterator.MoveNext();
			Assert.Equal("Liegestütze", iterator.Current.Name);
			iterator.MoveNext();
			Assert.Equal("Pause", iterator.Current.Name);
			iterator.MoveNext();
			Assert.Equal("Sit-ups", iterator.Current.Name);
		}

		[Fact]
		public void ActivityListCheck()
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
			Mock<AbstractSitUpActivity> sitUpActivityMock = new Mock<AbstractSitUpActivity>();
			Mock<AbstractPushUpActivity> pushUpActivityMock = new Mock<AbstractPushUpActivity>();
			Mock<IEarablesConnection> connectionMock = new Mock<IEarablesConnection>();
			Mock<IPopUpService> popUpMock = new Mock<IPopUpService>();

			//ActivityManager
			activityManagerMock.Setup(x => x.ActitvityProvider).Returns(activityProviderMock.Object);
			activityProviderMock.Setup(x => x.GetService(typeof(AbstractSitUpActivity))).Returns(sitUpActivityMock.Object);
			activityProviderMock.Setup(x => x.GetService(typeof(AbstractPushUpActivity))).Returns(pushUpActivityMock.Object);

			//IDataBaseConnection
			Mock<IDataBaseConnection> mockDataBase = new Mock<IDataBaseConnection>();

			DateTime _dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			DBEntry _entryNew = new DBEntry(_dt, 10, 0, 0);
			mockDataBase.As<IDataBaseConnection>().Setup(x => x.SaveDBEntry(_entryNew)).Returns(1);

			//PopUpService
			popUpMock.SetupSequence(x => x.ActionSheet("Wähle eine Aktivität:", "Abbruch", null, "Liegestütze", "Sit-ups", "Pause"))
				.Returns(Task.Run(() => { return "Liegestütze"; }))
				.Returns(Task.Run(() => { return "Liegestütze"; }));
			popUpMock.SetupSequence(x => x.DisplayPrompt("Liegestütze", "Gebe die Anzahl Wiederholungen an:", "Okay", "Abbruch", "10", 3, Keyboard.Numeric))
				.Returns(Task.Run(() => { return "12"; }))
				.Returns(Task.Run(() => { return "12"; }));

			//ServiceManager
			mockSingleton.Setup(x => x.GetService(typeof(IDataBaseConnection))).Returns(mockDataBase.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IActivityManager))).Returns(activityManagerMock.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IPopUpService))).Returns(popUpMock.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IEarablesConnection))).Returns(connectionMock.Object);

			//Connection
			ScanningPopUpViewModel.IsConnected = true;
			connectionMock.As<IEarablesConnection>().Setup(x => x.StartSampling());

			//ServiceProvider anlegen
			instance.SetValue(null, mockSingleton.Object);

			//Test
			ListenAndPerformViewModel viewModel = new ListenAndPerformViewModel();
			_= viewModel.AddActivity(3); //4
			Assert.Equal(4, viewModel.ActivityList.Count);
			IEnumerator<ActivityWrapper> iterator = viewModel.ActivityList.GetEnumerator();
			iterator.MoveNext();
			iterator.MoveNext();
			iterator.MoveNext();
			iterator.MoveNext();
			Assert.Equal("Liegestütze", iterator.Current.Name);
			Assert.Equal(12, iterator.Current.Amount);

			viewModel.SelectedActivity = iterator.Current;
			viewModel.EditActivity();
			iterator = viewModel.ActivityList.GetEnumerator();
			iterator.MoveNext();
			iterator.MoveNext();
			iterator.MoveNext();
			iterator.MoveNext();
			viewModel.SelectedActivity = iterator.Current;
			viewModel.RemoveActivity(); 
			Assert.Equal(3, viewModel.ActivityList.Count);

		}

		[Fact]
		public void ProcedureCheck()
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
			Mock<AbstractSitUpActivity> sitUpActivityMock = new Mock<AbstractSitUpActivity>();
			Mock<AbstractPushUpActivity> pushUpActivityMock = new Mock<AbstractPushUpActivity>();
			Mock<IEarablesConnection> connectionMock = new Mock<IEarablesConnection>();
			Mock<IPopUpService> popUpMock = new Mock<IPopUpService>();
			Mock<IAudioService> audioMock = new Mock<IAudioService>();

			//ActivityManager
			activityManagerMock.Setup(x => x.ActitvityProvider).Returns(activityProviderMock.Object);
			activityProviderMock.Setup(x => x.GetService(typeof(AbstractSitUpActivity))).Returns(sitUpActivityMock.Object);
			activityProviderMock.Setup(x => x.GetService(typeof(AbstractPushUpActivity))).Returns(pushUpActivityMock.Object);

			//IDataBaseConnection
			Mock<IDataBaseConnection> mockDataBase = new Mock<IDataBaseConnection>();

			DateTime _dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			DBEntry _entryNew = new DBEntry(_dt, 10, 0, 0);
			mockDataBase.As<IDataBaseConnection>().Setup(x => x.SaveDBEntry(_entryNew)).Returns(1);

			//PopUpService
			popUpMock.SetupSequence(x => x.ActionSheet("Wähle eine Aktivität:", "Abbruch", null, "Liegestütze", "Sit-ups", "Pause"))
				.Returns(Task.Run(() => { return "Liegestütze"; }))
				.Returns(Task.Run(() => { return "Sit-ups"; }));
			popUpMock.SetupSequence(x => x.DisplayPrompt("Liegestütze", "Gebe die Anzahl Wiederholungen an:", "Okay", "Abbruch", "10", 3, Keyboard.Numeric))
				.Returns(Task.Run(() => { return "3"; }))
				.Returns(Task.Run(() => { return "4"; }));

			//ServiceManager
			mockSingleton.Setup(x => x.GetService(typeof(IDataBaseConnection))).Returns(mockDataBase.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IActivityManager))).Returns(activityManagerMock.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IPopUpService))).Returns(popUpMock.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IEarablesConnection))).Returns(connectionMock.Object);
			mockSingleton.Setup(x => x.GetService(typeof(IAudioService))).Returns(audioMock.Object);

			//Connection
			ScanningPopUpViewModel.IsConnected = true;
			connectionMock.As<IEarablesConnection>().Setup(x => x.StartSampling());

			//ServiceProvider anlegen
			instance.SetValue(null, mockSingleton.Object);

			//Test
			ListenAndPerformViewModel viewModel = new ListenAndPerformViewModel();
			_ = viewModel.AddActivity(3);
			Assert.Equal(4, viewModel.ActivityList.Count);

			IEnumerator<ActivityWrapper> iterator = viewModel.ActivityList.GetEnumerator();
			iterator.MoveNext();
			viewModel.SelectedActivity = iterator.Current;
			viewModel.RemoveActivity();

			iterator = viewModel.ActivityList.GetEnumerator();
			iterator.MoveNext();
			viewModel.SelectedActivity = iterator.Current;
			viewModel.RemoveActivity();

			iterator = viewModel.ActivityList.GetEnumerator();
			iterator.MoveNext();
			viewModel.SelectedActivity = iterator.Current;
			viewModel.RemoveActivity();

			Assert.Single(viewModel.ActivityList);

			viewModel.StartActivity();
			Assert.Equal("Liegestütze", viewModel.ActiveActivity.Name);
			viewModel.OnActivityDone(this, null);
			viewModel.OnActivityDone(this, null);
			Assert.Equal(2, viewModel.ActiveActivity.Counter);
			viewModel.OnActivityDone(this, null);
			viewModel.StopActivity();
			viewModel.StartActivity();
			Assert.Equal(0, viewModel.ActiveActivity.Counter);
		}
	}
}