using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using EarablesKIT.Models;
using EarablesKIT.Models.SettingsService;
using EarablesKIT.ViewModels;
using Moq;
using Xunit;

namespace ViewModelTests.ViewModels.SettingsViewModelTest
{
    [ExcludeFromCodeCoverage]
    public class SettingsViewModelTest
    {
        [Fact]
        public void SettingsViewModelConstructorTest()
        {

            //Für den ServiceProviderMock
            //Muss enthalten sein, damit der Mock nicht überschrieben wird
            IServiceProvider unused = ServiceManager.ServiceProvider;

            //Feld Infos holen
            System.Reflection.FieldInfo instance = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            //Mocksaufsetzen 
            //ServiceProvider
            Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();

            //Service der gemockt werden soll
            Mock<ISettingsService> mockSettingsService = new Mock<ISettingsService>();

            User initUser = new User("Alice", 70);
            CultureInfo initCultureInfo = CultureInfo.GetCultureInfo("en-US");

            mockSettingsService.SetupProperty(x => x.SamplingRate, SamplingRate.Hz_50);
            mockSettingsService.SetupProperty(x => x.ActiveUser, initUser);
            mockSettingsService.SetupProperty(x => x.ActiveLanguage, initCultureInfo);

            mockSingleton.Setup(x => x.GetService(typeof(ISettingsService))).Returns(mockSettingsService.Object);
            instance.SetValue(null, mockSingleton.Object);

            //Ausführung
            SettingsViewModel settingsViewModel = new SettingsViewModel();


            //Verify
            mockSingleton.Verify(x => x.GetService(typeof(ISettingsService)), Times.Once);
            mockSettingsService.Verify(x => x.ActiveLanguage, Times.Once);
            mockSettingsService.Verify(x => x.SamplingRate, Times.Once);
            mockSettingsService.Verify(x => x.ActiveUser, Times.Once);

            
            Assert.NotNull(settingsViewModel);
            Assert.Equal((int)SamplingRate.Hz_50, settingsViewModel.SamplingRate);
            Assert.Equal(initUser.Username, settingsViewModel.Username);
            Assert.Equal(initUser.Steplength, settingsViewModel.Steplength);
        }




        [Fact]
        public void SaveClickedAllChangedTest()
        {

            //Für den ServiceProviderMock
            //Muss enthalten sein, damit der Mock nicht überschrieben wird
            IServiceProvider unused = ServiceManager.ServiceProvider;

            //Feld Infos holen
            System.Reflection.FieldInfo instance = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            //Mocksaufsetzen 
            //ServiceProvider
            Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();

            //Service der gemockt werden soll
            Mock<ISettingsService> mockSettingsService = new Mock<ISettingsService>();



            User initUser = new User("Alice", 70);
            CultureInfo initCultureInfo = CultureInfo.GetCultureInfo("en-US");
            
            SamplingRate newSamplingRate = SamplingRate.Hz_100;
            User newUser = new User("Bob", 80);
            CultureInfo newCultureInfo = CultureInfo.GetCultureInfo("de-DE");


            mockSettingsService.SetupProperty(x => x.SamplingRate, SamplingRate.Hz_50);
            mockSettingsService.SetupProperty(x => x.ActiveUser, initUser);
            mockSettingsService.SetupProperty(x => x.ActiveLanguage, initCultureInfo);



            mockSingleton.Setup(x => x.GetService(typeof(ISettingsService))).Returns(mockSettingsService.Object);
            instance.SetValue(null, mockSingleton.Object);

            //Ausführung
            SettingsViewModel settingsViewModel = new SettingsViewModel();
            bool actual = settingsViewModel.SaveClicked(newUser.Username, newUser.Steplength, newSamplingRate, newCultureInfo);

            //Asserts

            Assert.True(actual);
            Assert.Equal((int)newSamplingRate, settingsViewModel.SamplingRate);
            Assert.Equal(newUser.Username, settingsViewModel.Username);
            Assert.Equal(newUser.Steplength, settingsViewModel.Steplength);
        }

        [Fact]
        public void SaveClickedNothingChangedTest()
        {

            //Für den ServiceProviderMock
            //Muss enthalten sein, damit der Mock nicht überschrieben wird
            IServiceProvider unused = ServiceManager.ServiceProvider;

            //Feld Infos holen
            System.Reflection.FieldInfo instance = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            //Mocksaufsetzen 
            //ServiceProvider
            Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();

            //Service der gemockt werden soll
            Mock<ISettingsService> mockSettingsService = new Mock<ISettingsService>();

            User initUser = new User("Alice", 70);
            CultureInfo initCultureInfo = CultureInfo.GetCultureInfo("en-US");

            mockSettingsService.SetupProperty(x => x.SamplingRate, SamplingRate.Hz_50);
            mockSettingsService.SetupProperty(x => x.ActiveUser, initUser);
            mockSettingsService.SetupProperty(x => x.ActiveLanguage, initCultureInfo);



            mockSingleton.Setup(x => x.GetService(typeof(ISettingsService))).Returns(mockSettingsService.Object);
            instance.SetValue(null, mockSingleton.Object);

            //Ausführung
            SettingsViewModel settingsViewModel = new SettingsViewModel();
            bool actual = settingsViewModel.SaveClicked(initUser.Username, initUser.Steplength, SamplingRate.Hz_50, initCultureInfo);

            //Asserts
            Assert.False(actual);
            Assert.Equal((int)SamplingRate.Hz_50, settingsViewModel.SamplingRate);
            Assert.Equal(initUser.Username, settingsViewModel.Username);
            Assert.Equal(initUser.Steplength, settingsViewModel.Steplength);
        }

        [Fact]
        public void TestOnAppearing()
        {
            //Für den ServiceProviderMock
            //Muss enthalten sein, damit der Mock nicht überschrieben wird
            IServiceProvider unused = ServiceManager.ServiceProvider;

            //Feld Infos holen
            System.Reflection.FieldInfo instance = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            //Mocksaufsetzen 
            //ServiceProvider
            Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();

            //Service der gemockt werden soll
            Mock<ISettingsService> mockSettingsService = new Mock<ISettingsService>();

            User initUser = new User("Alice", 70);
            CultureInfo initCultureInfo = CultureInfo.GetCultureInfo("en-US");

            mockSettingsService.SetupProperty(x => x.SamplingRate, SamplingRate.Hz_50);
            mockSettingsService.SetupProperty(x => x.ActiveUser, initUser);
            mockSettingsService.SetupProperty(x => x.ActiveLanguage, initCultureInfo);



            mockSingleton.Setup(x => x.GetService(typeof(ISettingsService))).Returns(mockSettingsService.Object);
            instance.SetValue(null, mockSingleton.Object);

            mockSingleton.Setup(x => x.GetService(typeof(ISettingsService))).Returns(mockSettingsService.Object);
            instance.SetValue(null, mockSingleton.Object);

            int count = 0;
            List<string> propertiesActual = new List<string>();


            //Ausführung
            SettingsViewModel settingsViewModel = new SettingsViewModel();
            settingsViewModel.PropertyChanged +=
                (object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
                {
                    count++;
                    propertiesActual.Add(e.PropertyName);

                };
            settingsViewModel.OnAppearing(null, null);

            Assert.Equal(4, count);
            Assert.Contains("Username",propertiesActual);
            Assert.Contains("Steplength", propertiesActual);
            Assert.Contains("_samplingrate", propertiesActual);
            Assert.Contains("Language", propertiesActual);

        }

        private void SettingsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
