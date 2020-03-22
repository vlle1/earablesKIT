using EarablesKIT.Models;
using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities.PushUpActivity;
using EarablesKIT.Models.Extentionmodel.Activities.SitUpActivity;
using EarablesKIT.Models.Library;
using EarablesKIT.Models.PopUpService;
using EarablesKIT.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace ViewModelTests
{

    public class CountModeViewModelTest
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
            CountModeViewModel viewModel = new CountModeViewModel();
            IEnumerator<ActivityWrapper> iterator = viewModel.PossibleActivities.GetEnumerator();
            Assert.Equal(3, viewModel.PossibleActivities.Count);
            iterator.MoveNext();
            Assert.Equal("Liegestütze", iterator.Current.Name);
            iterator.MoveNext();
            Assert.Equal("Sit-ups", iterator.Current.Name);
            iterator.MoveNext();
            Assert.Equal("Demnächst Verfügbar...", iterator.Current.Name);
            Assert.Equal("Liegestütze", viewModel.SelectedActivity.Name);

            viewModel.Milliseconds = "0";
            viewModel.Seconds = "0";
            viewModel.Minutes = "0";
            Assert.Equal("0", viewModel.Milliseconds);
            Assert.Equal("0", viewModel.Seconds);
            Assert.Equal("0", viewModel.Minutes);
        }

        [Fact]
        public void CounterCheck()
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
            CountModeViewModel viewModel = new CountModeViewModel();
            viewModel.StartActivity();
            viewModel.OnActivityDone(this, null);
            viewModel.OnActivityDone(this, null);
            viewModel.OnActivityDone(this, null);
            viewModel.OnActivityDone(this, null);
            viewModel.OnActivityDone(this, null);
            Assert.Equal(5, viewModel.SelectedActivity.Counter);

            viewModel.StopActivity();
            Assert.Equal(5, viewModel.SelectedActivity.Counter);

            viewModel.StartActivity();
            Assert.Equal(0, viewModel.SelectedActivity.Counter);

            viewModel.SelectedActivity = null;
            viewModel.StartActivity();
        }
    }
}
