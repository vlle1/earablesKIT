using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using EarablesKIT.Models;
using EarablesKIT.Models.Library;
using EarablesKIT.ViewModels;
using Moq;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;
using Xunit;

namespace ViewModelTests.ViewModels.ScanningPopUp
{
    public class ScanningPopUpViewModelTest
    {
        [Fact]
        public void testScanningPopUpConstructor()
        {
            //Für den ServiceProviderMock
            //Muss enthalten sein, damit der Mock nicht überschrieben wird
            IServiceProvider unused = ServiceManager.ServiceProvider;

            //Feld Infos holen
            System.Reflection.FieldInfo instance = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            //Mocksaufsetzen 
            //ServiceProvider
            Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();
            Mock<IEarablesConnection> mockEarablesConnection = new Mock<IEarablesConnection>();
            Mock<IDevice> mockIDevice = new Mock<IDevice>();
            Mock<IDevice> mockSecondIDevice = new Mock<IDevice>();

            mockSingleton.As<IServiceProvider>().Setup(x => x.GetService(typeof(IEarablesConnection))).Returns(mockEarablesConnection.Object);

            mockIDevice.Setup(x => x.Name).Returns("DeviceOne");
            mockSecondIDevice.Setup(x => x.Name).Returns("eSense-120");

            mockEarablesConnection.SetupAdd(x => x.NewDeviceFound += ((sender, args) => { }));

            instance.SetValue(null, mockSingleton.Object);
            

            //Testing
            ScanningPopUpViewModel toTest = new ScanningPopUpViewModel();
            mockEarablesConnection.Raise(x=>x.NewDeviceFound += null, this, new NewDeviceFoundArgs(mockIDevice.Object));
            mockEarablesConnection.Raise(x=>x.NewDeviceFound += null, this, new NewDeviceFoundArgs(mockSecondIDevice.Object));
            
            //Verifizieren
            mockEarablesConnection.VerifyAdd(m => m.NewDeviceFound += It.IsAny<EventHandler<NewDeviceFoundArgs>>(), Times.Exactly(1));
            Assert.NotEmpty(toTest.DevicesList);
            Assert.Equal(2, toTest.DevicesList.Count);
            Assert.Contains(mockIDevice.Object, toTest.DevicesList);
            Assert.Contains(mockSecondIDevice.Object, toTest.DevicesList);
            Assert.Equal(0,toTest.DevicesList.IndexOf(mockSecondIDevice.Object));;
            Assert.Equal(1,toTest.DevicesList.IndexOf(mockIDevice.Object));;
        }


        [Fact]
        public void testOnDeviceConnectionStateChanged()
        {
            //Für den ServiceProviderMock
            //Muss enthalten sein, damit der Mock nicht überschrieben wird
            IServiceProvider unused = ServiceManager.ServiceProvider;

            //Feld Infos holen
            System.Reflection.FieldInfo instance = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            //Mocksaufsetzen 
            //ServiceProvider
            Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();
            Mock<IEarablesConnection> mockEarablesConnection = new Mock<IEarablesConnection>();
            Mock<IDevice> mockIDevice = new Mock<IDevice>();
            Mock<IDevice> mockSecondIDevice = new Mock<IDevice>();

            mockSingleton.As<IServiceProvider>().Setup(x => x.GetService(typeof(IEarablesConnection))).Returns(mockEarablesConnection.Object);

            mockIDevice.Setup(x => x.Name).Returns("DeviceOne");
            mockSecondIDevice.Setup(x => x.Name).Returns("eSense-120");

            mockEarablesConnection.SetupAdd(x => x.NewDeviceFound += ((sender, args) => { }));

            instance.SetValue(null, mockSingleton.Object);


            //Testing
            ScanningPopUpViewModel toTest = new ScanningPopUpViewModel();
            mockEarablesConnection.Raise(x => x.NewDeviceFound += null, this, new NewDeviceFoundArgs(mockIDevice.Object));
            mockEarablesConnection.Raise(x => x.NewDeviceFound += null, this, new NewDeviceFoundArgs(mockSecondIDevice.Object));

            //Verifizieren
            mockEarablesConnection.VerifyAdd(m => m.NewDeviceFound += It.IsAny<EventHandler<NewDeviceFoundArgs>>(), Times.Exactly(1));
            Assert.NotEmpty(toTest.DevicesList);
            Assert.Equal(2, toTest.DevicesList.Count);
            Assert.Contains(mockIDevice.Object, toTest.DevicesList);
            Assert.Contains(mockSecondIDevice.Object, toTest.DevicesList);
            Assert.Equal(0, toTest.DevicesList.IndexOf(mockSecondIDevice.Object)); ;
            Assert.Equal(1, toTest.DevicesList.IndexOf(mockIDevice.Object)); ;
        }
    }
}
