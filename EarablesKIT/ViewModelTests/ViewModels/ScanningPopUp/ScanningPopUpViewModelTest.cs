using EarablesKIT.Models;
using EarablesKIT.Models.Library;
using EarablesKIT.Models.PopUpService;
using EarablesKIT.ViewModels;
using Moq;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;


namespace ViewModelTests.ViewModels.ScanningPopUp
{
    [ExcludeFromCodeCoverage]
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
            if (instance == null)
            {
                Assert.True(false);
            }
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


        [Fact]

        public void testNewDeviceFound()
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

        [Fact]
        public void testConnectCommand()
        {
            IServiceProvider unused = ServiceManager.ServiceProvider;

            FieldInfo instanceToMock = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            Mock<IServiceProvider> providerMock = new Mock<IServiceProvider>();
            Mock<IEarablesConnection> earablesConnectionMock = new Mock<IEarablesConnection>();
            Mock<IDevice> deviceMock = new Mock<IDevice>();
            Mock<IExceptionHandler> exceptionHandlerMock = new Mock<IExceptionHandler>();

            earablesConnectionMock.Setup(x => x.ConnectToDevice(deviceMock.Object));
            earablesConnectionMock.SetupAdd(x => x.NewDeviceFound += ((sender, args) => { })).Verifiable();
            exceptionHandlerMock.Setup(x => x.HandleException(It.IsAny<Exception>())).Verifiable();

            providerMock.Setup(x => x.GetService(typeof(IEarablesConnection))).Returns(earablesConnectionMock.Object).Verifiable();
            providerMock.Setup(x => x.GetService(typeof(IExceptionHandler))).Returns(exceptionHandlerMock.Object).Verifiable();

            instanceToMock.SetValue(null, providerMock.Object);


            //Testing
            ScanningPopUpViewModel viewModelToTest = new ScanningPopUpViewModel();
            viewModelToTest.ConnectDeviceCommand.Execute(deviceMock.Object);

            earablesConnectionMock.Verify(x => x.ConnectToDevice(deviceMock.Object), Times.Once());
            exceptionHandlerMock.Verify(x => x.HandleException(It.IsAny<Exception>()), Times.Never);

            providerMock.VerifyAll();

        }

        [Fact]
        public void testConnectCommandException()
        {
            IServiceProvider unused = ServiceManager.ServiceProvider;

            FieldInfo instanceToMock = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            Mock<IServiceProvider> providerMock = new Mock<IServiceProvider>();
            Mock<IEarablesConnection> earablesConnectionMock = new Mock<IEarablesConnection>();
            Mock<IDevice> deviceMock = new Mock<IDevice>();
            Mock<IExceptionHandler> exceptionHandlerMock = new Mock<IExceptionHandler>();

            earablesConnectionMock.Setup(x => x.ConnectToDevice(deviceMock.Object)).Throws(new DeviceConnectionException(new Guid(), null, null));

            exceptionHandlerMock.Setup(x => x.HandleException(It.IsAny<DeviceConnectionException>()));

            providerMock.Setup(x => x.GetService(typeof(IEarablesConnection))).Returns(earablesConnectionMock.Object).Verifiable();
            providerMock.Setup(x => x.GetService(typeof(IExceptionHandler))).Returns(exceptionHandlerMock.Object).Verifiable();

            instanceToMock.SetValue(null, providerMock.Object);


            //Testing
            ScanningPopUpViewModel viewModelToTest = new ScanningPopUpViewModel();
            viewModelToTest.ConnectDeviceCommand.Execute(deviceMock.Object);

            //Asserts
            Assert.Empty(viewModelToTest.DevicesList);
            earablesConnectionMock.Verify(x => x.ConnectToDevice(deviceMock.Object), Times.Once());
            exceptionHandlerMock.Verify(x => x.HandleException(It.IsAny<Exception>()), Times.Once);
            providerMock.VerifyAll();


        }

        [Fact]
        public void testScanningCommand()
        {
            IServiceProvider unused = ServiceManager.ServiceProvider;

            FieldInfo instanceToMock = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            Mock<IServiceProvider> providerMock = new Mock<IServiceProvider>();
            Mock<IEarablesConnection> earablesConnectionMock = new Mock<IEarablesConnection>();
            Mock<IExceptionHandler> exceptionHandlerMock = new Mock<IExceptionHandler>();


            earablesConnectionMock.Setup(x => x.StartScanning());
            earablesConnectionMock.Setup(x => x.IsBluetoothActive).Returns(true);


            providerMock.Setup(x => x.GetService(typeof(IEarablesConnection))).Returns(earablesConnectionMock.Object);
            providerMock.Setup(x => x.GetService(typeof(IExceptionHandler))).Returns(exceptionHandlerMock.Object);

            instanceToMock.SetValue(null, providerMock.Object);

            FieldInfo currentCrossPermissionMock =
                typeof(CrossPermissions).GetField("implementation", BindingFlags.NonPublic | BindingFlags.Static);

            Mock<IPermissions> mockImplementation = new Mock<IPermissions>();
            Mock<IPermissions> valueMock = new Mock<IPermissions>();

            mockImplementation.Setup(x => x.CheckPermissionStatusAsync(Permission.Location)).Returns(Task.FromResult(PermissionStatus.Granted));

            currentCrossPermissionMock.SetValue(null, new Lazy<IPermissions>(() => mockImplementation.Object));


            //Testing
            ScanningPopUpViewModel viewModelToTest = new ScanningPopUpViewModel();
            viewModelToTest.ScanDevicesCommand.Execute(null);

            providerMock.VerifyAll();
            earablesConnectionMock.VerifyAll();
            mockImplementation.VerifyAll();
            valueMock.VerifyAll();
        }

        [Fact]
        public void testScanningCommandBluetoothNotActive()
        {
            IServiceProvider unused = ServiceManager.ServiceProvider;

            FieldInfo instanceToMock = typeof(ServiceManager).GetField("_serviceProvider",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);


            //Servii
            Mock<IServiceProvider> providerMock = new Mock<IServiceProvider>();
            Mock<IEarablesConnection> earablesConnectionMock = new Mock<IEarablesConnection>();
            Mock<IExceptionHandler> exceptionHandlerMock = new Mock<IExceptionHandler>();
            Mock<IPopUpService> popUpServiceMock = new Mock<IPopUpService>();

            //earablesConnectionMock.Setup(x => x.StartScanning());
            earablesConnectionMock.Setup(x => x.IsBluetoothActive).Returns(false);

            providerMock.Setup(x => x.GetService(typeof(IEarablesConnection))).Returns(earablesConnectionMock.Object);
            providerMock.Setup(x => x.GetService(typeof(IExceptionHandler))).Returns(exceptionHandlerMock.Object);
            providerMock.Setup(x => x.GetService(typeof(IPopUpService))).Returns(popUpServiceMock.Object);
            instanceToMock.SetValue(null, providerMock.Object);



            //Crosspermission
            FieldInfo currentCrossPermissionMock =
                typeof(CrossPermissions).GetField("implementation", BindingFlags.NonPublic | BindingFlags.Static);

            Mock<IPermissions> mockImplementation = new Mock<IPermissions>();
            Mock<IPermissions> valueMock = new Mock<IPermissions>();

            mockImplementation.Setup(x => x.CheckPermissionStatusAsync(Permission.Location)).Returns(Task.FromResult(PermissionStatus.Granted));
            currentCrossPermissionMock.SetValue(null, new Lazy<IPermissions>(() => mockImplementation.Object));


            //Displayalert
            popUpServiceMock.Setup(x => x.DisplayAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new object()));

            //Testing
            ScanningPopUpViewModel viewModelToTest = new ScanningPopUpViewModel();
            viewModelToTest.ScanDevicesCommand.Execute(null);

            //Verify
            providerMock.VerifyAll();
            earablesConnectionMock.VerifyAll();
            mockImplementation.VerifyAll();
            valueMock.VerifyAll();
            popUpServiceMock.VerifyAll();
        }

        [Fact]
        public void testScanningCommandPermissionNotGranted()
        {
            //SetupProvider
            IServiceProvider unused = ServiceManager.ServiceProvider;
            FieldInfo instanceToMock = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            //Servii
            Mock<IServiceProvider> providerMock = new Mock<IServiceProvider>();
            Mock<IEarablesConnection> earablesConnectionMock = new Mock<IEarablesConnection>();
            Mock<IExceptionHandler> exceptionHandlerMock = new Mock<IExceptionHandler>();
            Mock<IPopUpService> popupServiceMock = new Mock<IPopUpService>();


            //EarablesConnectionMock
            earablesConnectionMock.Setup(x => x.IsBluetoothActive).Returns(true);

            //PopupService
            popupServiceMock
                .Setup(service => service.DisplayAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            //ServiceProvider
            providerMock.Setup(x => x.GetService(typeof(IEarablesConnection))).Returns(earablesConnectionMock.Object);
            providerMock.Setup(x => x.GetService(typeof(IExceptionHandler))).Returns(exceptionHandlerMock.Object);
            providerMock.Setup(x => x.GetService(typeof(IPopUpService))).Returns(popupServiceMock.Object);

            instanceToMock.SetValue(null, providerMock.Object);


            //CrossPermission
            FieldInfo currentCrossPermissionMock =
                typeof(CrossPermissions).GetField("implementation", BindingFlags.NonPublic | BindingFlags.Static);

            Mock<IPermissions> mockImplementation = new Mock<IPermissions>();
            Mock<IPermissions> valueMock = new Mock<IPermissions>();

            mockImplementation.Setup(x => x.CheckPermissionStatusAsync(Permission.Location)).Returns(Task.FromResult(PermissionStatus.Denied));
            mockImplementation
                .Setup(permissions => permissions.ShouldShowRequestPermissionRationaleAsync(Permission.Unknown))
                .Returns(Task.FromResult(true));

            Permission[] expected = new[] { Permission.Location };
            Dictionary<Permission, PermissionStatus> statusToGive = new Dictionary<Permission, PermissionStatus>();
            statusToGive.Add(Permission.Location, PermissionStatus.Denied);
            mockImplementation.Setup(x => x.RequestPermissionsAsync(expected)).Returns(Task.FromResult(statusToGive));


            currentCrossPermissionMock.SetValue(null, new Lazy<IPermissions>(() => mockImplementation.Object));


            //Testing
            ScanningPopUpViewModel viewModelToTest = new ScanningPopUpViewModel();
            viewModelToTest.ScanDevicesCommand.Execute(null);

            providerMock.VerifyAll();
            earablesConnectionMock.VerifyAll();
            mockImplementation.VerifyAll();
            valueMock.VerifyAll();
            popupServiceMock.Verify(service => service.DisplayAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }


        [Fact]
        public void testCancelCommand()
        {
            IServiceProvider unused = ServiceManager.ServiceProvider;

            FieldInfo instanceToMock = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            FieldInfo customField =
                typeof(PopupNavigation).GetField("_customNavigation", BindingFlags.Static | BindingFlags.NonPublic);

            Mock<IPopupNavigation> _customNavigationAsMock = new Mock<IPopupNavigation>();

            _customNavigationAsMock.Setup(x => x.PopAsync(true));
            customField.SetValue(null, _customNavigationAsMock.Object);

            Mock<IServiceProvider> providerMock = new Mock<IServiceProvider>();

            instanceToMock.SetValue(null, providerMock.Object);

            Mock<IEarablesConnection> earablesConnectionMock = new Mock<IEarablesConnection>();
            Mock<IExceptionHandler> exceptionHandlerMock = new Mock<IExceptionHandler>();

            providerMock.Setup(x => x.GetService(typeof(IEarablesConnection))).Returns(earablesConnectionMock.Object);
            providerMock.Setup(x => x.GetService(typeof(IExceptionHandler))).Returns(exceptionHandlerMock.Object);

            //Act 
            ScanningPopUpViewModel toTestModel = new ScanningPopUpViewModel();
            toTestModel.CancelCommand.Execute(null);

            //Verify
            providerMock.VerifyAll();
            _customNavigationAsMock.VerifyAll();
            earablesConnectionMock.VerifyAll();
        }

        [Fact]
        public void testOnDeviceConnectionStateChanged()
        {
            FieldInfo customField =
                typeof(PopupNavigation).GetField("_customNavigation", BindingFlags.Static | BindingFlags.NonPublic);

            Mock<IPopupNavigation> _customNavigationAsMock = new Mock<IPopupNavigation>();

            _customNavigationAsMock.Setup(x => x.PopAsync(true));
            customField.SetValue(null, _customNavigationAsMock.Object);

            //Act 
            ScanningPopUpViewModel.OnDeviceConnectionStateChanged(null, new DeviceEventArgs(true, ""));

            //Verify
            _customNavigationAsMock.VerifyAll();
        }

        [Fact]
        public void testConnectedProperty()
        {
            ScanningPopUpViewModel.IsConnected = true;
            Assert.True(ScanningPopUpViewModel.IsConnected);
            ScanningPopUpViewModel.IsConnected = false;
            Assert.False(ScanningPopUpViewModel.IsConnected);
        }


    }
}
