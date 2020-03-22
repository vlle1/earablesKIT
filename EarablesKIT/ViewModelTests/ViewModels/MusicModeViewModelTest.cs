using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using EarablesKIT.Models;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities.RunningActivity;
using EarablesKIT.Models.Library;
using EarablesKIT.ViewModels;
using MediaManager;
using Moq;
using Xunit;

namespace ViewModelTests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class MusicModeViewModelTest
    {
        [Fact]
        public void MusicPlayingCorrectly()
        {
            //Für den ServiceProviderMock
            //Muss enthalten sein, damit der Mock nicht überschrieben wird
            IServiceProvider unused = ServiceManager.ServiceProvider;

            //Feld Infos holen
            FieldInfo rootServiceProvider = typeof(ServiceManager).GetField("_serviceProvider", BindingFlags.Static | BindingFlags.NonPublic);
            FieldInfo currentMediaManager = typeof(MusicModeViewModel).GetField("_mediaManager", BindingFlags.Static | BindingFlags.NonPublic);

            // skip scanning popup
            ScanningPopUpViewModel.IsConnected = true;

            //Mocksaufsetzen 
            //ServiceProvider
            Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();
            Mock<IServiceProvider> mockActivityServiceProvider = new Mock<IServiceProvider>();

            //Service der gemockt werden soll
            Mock<IEarablesConnection> mockEarablesConnection = new Mock<IEarablesConnection>();
            Mock<AbstractRunningActivity> mockRunningActivity = new Mock<AbstractRunningActivity>();
            Mock<IActivityManager> mockActivityManager = new Mock<IActivityManager>();

            Mock<IMediaManager> mockCrossMediaManager = new Mock<IMediaManager>();

            //Verhalten für die Mocks festlegen (Bei Aufruf was zurückgegeben werden soll)

            // Nachverfolgen, ob Sampling aktiviert wurde
            bool samplingActive = false;

            mockEarablesConnection.As<IEarablesConnection>().Setup(x => x.StartSampling()).Callback(() =>
            {
                samplingActive = true;
            });

            mockEarablesConnection.As<IEarablesConnection>().Setup(x => x.StopSampling()).Callback(() =>
            {
                samplingActive = false;
            });

            // Service
            mockActivityManager.As<IActivityManager>().Setup(x => x.ActitvityProvider).Returns(mockActivityServiceProvider.Object);

            mockActivityServiceProvider.Setup(x => x.GetService(typeof(AbstractRunningActivity))).Returns(mockRunningActivity.Object);

            mockSingleton.Setup(x => x.GetService(typeof(IEarablesConnection))).Returns(mockEarablesConnection.Object);
            mockSingleton.Setup(x => x.GetService(typeof(IActivityManager))).Returns(mockActivityManager.Object);

            // Track if music was started
            bool musicPlaying = false;
            mockCrossMediaManager.Setup(x => x.Play()).Callback(() => { musicPlaying = true; });
            mockCrossMediaManager.Setup(x => x.Pause()).Callback(() => { musicPlaying = false; });

            //ServiceProvider anlegen
            rootServiceProvider.SetValue(null, mockSingleton.Object);

            currentMediaManager.SetValue(null, mockCrossMediaManager.Object);


            var vm = new MusicModeViewModel();

            //////////////////////////////////////////////////////////////////////////////////////////////
            // Everything Mocked, ready to start some tests

            Assert.False(musicPlaying);

            vm.ToggleMusicMode.Execute(null);

            Assert.False(musicPlaying);

            var args = new RunningEventArgs(false);
            vm.OnActivityDone(null, args);

            Assert.False(musicPlaying);

            // starting to walk
            args = new RunningEventArgs(true);
            vm.OnActivityDone(null, args);

            Assert.True(musicPlaying);

            vm.ToggleMusicMode.Execute(null);

            Assert.False(musicPlaying);

            vm.ToggleMusicMode.Execute(null);

            Assert.False(musicPlaying);

            args = new RunningEventArgs(true);
            vm.OnActivityDone(null, args);

            Assert.True(musicPlaying);

            vm.StopActivity();

            Assert.False(musicPlaying);
        }

        [Fact]
        public void SamplingAndIsRunning()
        {
            //Für den ServiceProviderMock
            //Muss enthalten sein, damit der Mock nicht überschrieben wird
            IServiceProvider unused = ServiceManager.ServiceProvider;

            //Feld Infos holen
            FieldInfo rootServiceProvider = typeof(ServiceManager).GetField("_serviceProvider", BindingFlags.Static | BindingFlags.NonPublic);
            FieldInfo currentMediaManager = typeof(MusicModeViewModel).GetField("_mediaManager", BindingFlags.Static | BindingFlags.NonPublic);

            // skip scanning popup
            ScanningPopUpViewModel.IsConnected = true;

            //Mocksaufsetzen 
            //ServiceProvider
            Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();
            Mock<IServiceProvider> mockActivityServiceProvider = new Mock<IServiceProvider>();

            //Service der gemockt werden soll
            Mock<IEarablesConnection> mockEarablesConnection = new Mock<IEarablesConnection>();
            Mock<AbstractRunningActivity> mockRunningActivity = new Mock<AbstractRunningActivity>();
            Mock<IActivityManager> mockActivityManager = new Mock<IActivityManager>();

            Mock<IMediaManager> mockCrossMediaManager = new Mock<IMediaManager>();

            //Verhalten für die Mocks festlegen (Bei Aufruf was zurückgegeben werden soll)

            // Nachverfolgen, ob Sampling aktiviert wurde
            bool samplingActive = false;

            mockEarablesConnection.As<IEarablesConnection>().Setup(x => x.StartSampling()).Callback(() => {
                samplingActive = true;
            });

            mockEarablesConnection.As<IEarablesConnection>().Setup(x => x.StopSampling()).Callback(() => {
                samplingActive = false;
            });

            // Service
            mockActivityManager.As<IActivityManager>().Setup(x => x.ActitvityProvider).Returns(mockActivityServiceProvider.Object);

            mockActivityServiceProvider.Setup(x => x.GetService(typeof(AbstractRunningActivity))).Returns(mockRunningActivity.Object);

            mockSingleton.Setup(x => x.GetService(typeof(IEarablesConnection))).Returns(mockEarablesConnection.Object);
            mockSingleton.Setup(x => x.GetService(typeof(IActivityManager))).Returns(mockActivityManager.Object);

            // Track if music was started
            bool musicPlaying = false;
            mockCrossMediaManager.Setup(x => x.Play()).Callback(() => { musicPlaying = true; });
            mockCrossMediaManager.Setup(x => x.Pause()).Callback(() => { musicPlaying = false; });

            //ServiceProvider anlegen
            rootServiceProvider.SetValue(null, mockSingleton.Object);

            currentMediaManager.SetValue(null, mockCrossMediaManager.Object);


            var vm = new MusicModeViewModel();

            //////////////////////////////////////////////////////////////////////////////////////////////
            // Everything Mocked, ready to start some tests

            Assert.False(vm.IsRunning);
            Assert.False(samplingActive);
            
            vm.ToggleMusicMode.Execute(null);

            Assert.False(vm.IsRunning);
            Assert.True(samplingActive);

            var args = new RunningEventArgs(false);
            vm.OnActivityDone(null, args);

            Assert.False(vm.IsRunning);
            Assert.True(samplingActive);
            
            // starting to walk
            args = new RunningEventArgs(true);
            vm.OnActivityDone(null, args);

            Assert.True(vm.IsRunning);
            Assert.True(samplingActive);

            vm.ToggleMusicMode.Execute(null);

            Assert.False(vm.IsRunning);
            Assert.False(samplingActive);

            vm.ToggleMusicMode.Execute(null);

            Assert.False(vm.IsRunning);
            Assert.True(samplingActive);
            
            args = new RunningEventArgs(true);
            vm.OnActivityDone(null, args);

            Assert.True(vm.IsRunning);
            Assert.True(samplingActive);
            
            vm.StopActivity();

            Assert.False(vm.IsRunning);
            Assert.False(samplingActive);
        }

        [Fact]
        public void TextOnLables()
        {
            //Für den ServiceProviderMock
            //Muss enthalten sein, damit der Mock nicht überschrieben wird
            IServiceProvider unused = ServiceManager.ServiceProvider;

            //Feld Infos holen
            FieldInfo rootServiceProvider = typeof(ServiceManager).GetField("_serviceProvider", BindingFlags.Static | BindingFlags.NonPublic);
            FieldInfo currentMediaManager = typeof(MusicModeViewModel).GetField("_mediaManager", BindingFlags.Static | BindingFlags.NonPublic);

            // skip scanning popup
            ScanningPopUpViewModel.IsConnected = true;

            //Mocksaufsetzen 
            //ServiceProvider
            Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();
            Mock<IServiceProvider> mockActivityServiceProvider = new Mock<IServiceProvider>();

            //Service der gemockt werden soll
            Mock<IEarablesConnection> mockEarablesConnection = new Mock<IEarablesConnection>();
            Mock<AbstractRunningActivity> mockRunningActivity = new Mock<AbstractRunningActivity>();
            Mock<IActivityManager> mockActivityManager = new Mock<IActivityManager>();

            Mock<IMediaManager> mockCrossMediaManager = new Mock<IMediaManager>();

            //Verhalten für die Mocks festlegen (Bei Aufruf was zurückgegeben werden soll)

            // Nachverfolgen, ob Sampling aktiviert wurde
            bool samplingActive = false;

            mockEarablesConnection.As<IEarablesConnection>().Setup(x => x.StartSampling()).Callback(() => {
                samplingActive = true;
            });

            mockEarablesConnection.As<IEarablesConnection>().Setup(x => x.StopSampling()).Callback(() => {
                samplingActive = false;
            });

            // Service
            mockActivityManager.As<IActivityManager>().Setup(x => x.ActitvityProvider).Returns(mockActivityServiceProvider.Object);

            mockActivityServiceProvider.Setup(x => x.GetService(typeof(AbstractRunningActivity))).Returns(mockRunningActivity.Object);

            mockSingleton.Setup(x => x.GetService(typeof(IEarablesConnection))).Returns(mockEarablesConnection.Object);
            mockSingleton.Setup(x => x.GetService(typeof(IActivityManager))).Returns(mockActivityManager.Object);

            // Track if music was started
            bool musicPlaying = false;
            mockCrossMediaManager.Setup(x => x.Play()).Callback(() => { musicPlaying = true; });
            mockCrossMediaManager.Setup(x => x.Pause()).Callback(() => { musicPlaying = false; });

            //ServiceProvider anlegen
            rootServiceProvider.SetValue(null, mockSingleton.Object);

            currentMediaManager.SetValue(null, mockCrossMediaManager.Object);


            var vm = new MusicModeViewModel();

            //////////////////////////////////////////////////////////////////////////////////////////////
            // Everything Mocked, ready to start some tests
            if (CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName == "eng")
            {
                Assert.Equal("Launch the music mode to enjoy a whole new listening experience! Music plays exactly when you walk!", vm.CurrentStatusLabel);
            }
            else if (CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName == "deu")
            {
                Assert.Equal("Starten Sie den Musikmodus, um eine ganz neue Hörerfahrung zu genießen! Die Musik spielt genau dann, wenn Sie gehen!", vm.CurrentStatusLabel);
            }

            Assert.Equal("Start", vm.StartStopLabel);
            
            vm.ToggleMusicMode.Execute(null);

            Assert.Equal("Stop", vm.StartStopLabel);
            
            var args = new RunningEventArgs(false);
            vm.OnActivityDone(null, args);

            Assert.Equal("Stop", vm.StartStopLabel);
            
            if (CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName == "eng")
            {
                Assert.Equal("You are standing", vm.CurrentStatusLabel);
            }
            else if (CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName == "deu")
            {
                Assert.Equal("Du stehst gerade", vm.CurrentStatusLabel);
            }

            // starting to walk
            args = new RunningEventArgs(true);
            vm.OnActivityDone(null, args);

            Assert.Equal("Stop", vm.StartStopLabel);

            if (CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName == "eng")
            {
                Assert.Equal("You are walking", vm.CurrentStatusLabel);
            }
            else if (CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName == "deu")
            {
                Assert.Equal("Du gehst gerade", vm.CurrentStatusLabel);
            }

            vm.ToggleMusicMode.Execute(null);
            Assert.Equal("Start", vm.StartStopLabel);

            if (CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName == "eng")
            {
                Assert.Equal("Launch the music mode to enjoy a whole new listening experience! Music plays exactly when you walk!", vm.CurrentStatusLabel);
            }
            else if (CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName == "deu")
            {
                Assert.Equal("Starten Sie den Musikmodus, um eine ganz neue Hörerfahrung zu genießen! Die Musik spielt genau dann, wenn Sie gehen!", vm.CurrentStatusLabel);
            }

            vm.ToggleMusicMode.Execute(null);

            Assert.Equal("Stop", vm.StartStopLabel);

            args = new RunningEventArgs(true);
            vm.OnActivityDone(null, args);

            Assert.Equal("Stop", vm.StartStopLabel);

            if (CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName == "eng")
            {
                Assert.Equal("You are walking", vm.CurrentStatusLabel);
            }
            else if (CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName == "deu")
            {
                Assert.Equal("Du gehst gerade", vm.CurrentStatusLabel);
            }

            vm.StopActivity();

            Assert.Equal("Start", vm.StartStopLabel);

            if (CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName == "eng")
            {
                Assert.Equal("Launch the music mode to enjoy a whole new listening experience! Music plays exactly when you walk!", vm.CurrentStatusLabel);
            }
            else if (CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName == "deu")
            {
                Assert.Equal("Starten Sie den Musikmodus, um eine ganz neue Hörerfahrung zu genießen! Die Musik spielt genau dann, wenn Sie gehen!", vm.CurrentStatusLabel);
            }
        }

        [Fact]
        public void FullTest()
        {
            //Für den ServiceProviderMock
            //Muss enthalten sein, damit der Mock nicht überschrieben wird
            var unused = ServiceManager.ServiceProvider;

            //Feld Infos holen
            var rootServiceProvider =
                typeof(ServiceManager).GetField("_serviceProvider", BindingFlags.Static | BindingFlags.NonPublic);
            var currentMediaManager =
                typeof(MusicModeViewModel).GetField("_mediaManager", BindingFlags.Static | BindingFlags.NonPublic);

            // skip scanning popup
            ScanningPopUpViewModel.IsConnected = true;

            //set up msocks 
            //ServiceProvider
            Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();
            Mock<IServiceProvider> mockActivityServiceProvider = new Mock<IServiceProvider>();

            //Service der gemockt werden soll
            Mock<IEarablesConnection> mockEarablesConnection = new Mock<IEarablesConnection>();
            Mock<AbstractRunningActivity> mockRunningActivity = new Mock<AbstractRunningActivity>();
            Mock<IActivityManager> mockActivityManager = new Mock<IActivityManager>();

            Mock<IMediaManager> mockCrossMediaManager = new Mock<IMediaManager>();

            //Verhalten für die Mocks festlegen (Bei Aufruf was zurückgegeben werden soll)

            // Nachverfolgen, ob Sampling aktiviert wurde
            bool samplingActive = false;

            mockEarablesConnection.As<IEarablesConnection>().Setup(x => x.StartSampling())
                .Callback(() => { samplingActive = true; });

            mockEarablesConnection.As<IEarablesConnection>().Setup(x => x.StopSampling())
                .Callback(() => { samplingActive = false; });

            // Service
            mockActivityManager.As<IActivityManager>().Setup(x => x.ActitvityProvider)
                .Returns(mockActivityServiceProvider.Object);

            mockActivityServiceProvider.Setup(x => x.GetService(typeof(AbstractRunningActivity)))
                .Returns(mockRunningActivity.Object);

            mockSingleton.Setup(x => x.GetService(typeof(IEarablesConnection))).Returns(mockEarablesConnection.Object);
            mockSingleton.Setup(x => x.GetService(typeof(IActivityManager))).Returns(mockActivityManager.Object);

            // Track if music was started
            bool musicPlaying = false;
            mockCrossMediaManager.Setup(x => x.Play()).Callback(() => { musicPlaying = true; });
            mockCrossMediaManager.Setup(x => x.Pause()).Callback(() => { musicPlaying = false; });

            //ServiceProvider anlegen
            rootServiceProvider.SetValue(null, mockSingleton.Object);

            currentMediaManager.SetValue(null, null);


            MusicModeViewModel vm = null;
            try
            {
                vm = new MusicModeViewModel();
            }
            catch (System.NotImplementedException)
            {
                return;
            }
            // FAIL
            Assert.False(true);
        }
    }
}
