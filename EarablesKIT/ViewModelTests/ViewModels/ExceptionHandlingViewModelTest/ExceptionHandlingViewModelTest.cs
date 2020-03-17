using EarablesKIT.Models;
using EarablesKIT.Models.PopUpService;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using EarablesKIT.ViewModels;
using Xunit;

namespace ViewModelTests.ViewModels.ExceptionHandlingViewModelTest
{
    [ExcludeFromCodeCoverage]
    public class ExceptionHandlingViewModelTest
    {
        [Fact]
        public void testHandleExceptionWithParam()
        {
            IServiceProvider unused = ServiceManager.ServiceProvider;


            //Setup
            FieldInfo serviceProviderFieldInfo =
                typeof(ServiceManager).GetField("_serviceProvider", BindingFlags.Static | BindingFlags.NonPublic);

            Mock<IServiceProvider> providerMock = new Mock<IServiceProvider>();
            Mock<IPopUpService> popupServiceMock = new Mock<IPopUpService>();
            Exception exception = new Exception("This is a crucial exception");

            providerMock.Setup(provider => provider.GetService(typeof(IPopUpService)))
                .Returns(popupServiceMock.Object);
            popupServiceMock
                .Setup(service => service.DisplayAlert(It.IsAny<string>(), exception.Message, It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            serviceProviderFieldInfo.SetValue(null, providerMock.Object);


            //Act
            ExceptionHandlingViewModel viewModel = new ExceptionHandlingViewModel();
            viewModel.HandleException(exception);

            //Verify
            providerMock.VerifyAll();
            popupServiceMock.VerifyAll();
        }
        [Fact]
        public void testHandleExceptionWithoutParam()
        {
            IServiceProvider unused = ServiceManager.ServiceProvider;

            //Setup
            FieldInfo serviceProviderFieldInfo =
                typeof(ServiceManager).GetField("_serviceProvider", BindingFlags.Static | BindingFlags.NonPublic);

            Mock<IServiceProvider> providerMock = new Mock<IServiceProvider>();
            Mock<IPopUpService> popupServiceMock = new Mock<IPopUpService>();
            Exception exception = new Exception("");

            providerMock.Setup(provider => provider.GetService(typeof(IPopUpService)))
                .Returns(popupServiceMock.Object);
            popupServiceMock
                .Setup(service => service.DisplayAlert(It.IsAny<string>(), It.IsRegex("\\w+"), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            serviceProviderFieldInfo.SetValue(null, providerMock.Object);

            //Act
            ExceptionHandlingViewModel viewModel = new ExceptionHandlingViewModel();
            viewModel.HandleException(exception);

            //Verify
            providerMock.VerifyAll();
            popupServiceMock.VerifyAll();
        }
    }
}