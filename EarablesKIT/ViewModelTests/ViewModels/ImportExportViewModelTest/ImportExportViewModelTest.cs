using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using EarablesKIT.Models;
using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities.RunningActivity;
using EarablesKIT.Models.Library;
using EarablesKIT.ViewModels;
using MediaManager;
using Moq;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xunit;


namespace ViewModelTests
{
    public class ImportExportViewModelTest
    {
        [Fact]
        public void Import()
        {
            IServiceProvider unused = ServiceManager.ServiceProvider;

            //Feld Infos holen
            FieldInfo rootServiceProvider = typeof(ServiceManager).GetField("_serviceProvider", BindingFlags.Static | BindingFlags.NonPublic);
            
            //Mocksaufsetzen 
            //ServiceProvider
            Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();
            
            //Service der gemockt werden soll
            Mock<IDataBaseConnection> mockDatabaseConnection = new Mock<IDataBaseConnection>();
            

            //Verhalten für die Mocks festlegen (Bei Aufruf was zurückgegeben werden soll)

            // Nachverfolgen, ob Sampling aktiviert wurde
            FileData importetFile = null;

            mockDatabaseConnection.Setup(x => x.ImportTrainingsData(It.IsAny<FileData>())).Callback((FileData f) =>
            {
                importetFile = f;
            });

            //ServiceProvider anlegen
            Assert.NotNull(rootServiceProvider);
            rootServiceProvider.SetValue(null, mockSingleton.Object);

            mockSingleton.Setup(x => x.GetService(typeof(IDataBaseConnection))).Returns(mockDatabaseConnection.Object);



            // Override CrossPermissions.Current.CheckPermissionStatusAsync
            var permissionGranted = PermissionStatus.Granted;

            var CrossPermissionMock = new Mock<IPermissions>();
            CrossPermissionMock.Setup(x => x.CheckPermissionStatusAsync(It.IsAny<Permission>())).Returns(
                async () => permissionGranted);

            var currentPermissions = typeof(ImportExportViewModel).GetField("_crossPermissions", BindingFlags.Static | BindingFlags.NonPublic);
            Assert.NotNull(currentPermissions);
            currentPermissions.SetValue(null, CrossPermissionMock.Object);

            var vm = new ImportExportViewModel();

            var file = new FileData("", "TestData.txt", () => new FileStream("TestData.txt", FileMode.Open), null);
            
            vm.ImportCommand.Execute(file);

            Assert.Equal(file, importetFile);
        }

        [Fact]
        public void Export()
        {
            IServiceProvider unused = ServiceManager.ServiceProvider;

            //Feld Infos holen
            FieldInfo rootServiceProvider = typeof(ServiceManager).GetField("_serviceProvider", BindingFlags.Static | BindingFlags.NonPublic);

            //Mocksaufsetzen 
            //ServiceProvider
            Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();

            //Service der gemockt werden soll
            Mock<IDataBaseConnection> mockDatabaseConnection = new Mock<IDataBaseConnection>();


            //Verhalten für die Mocks festlegen (Bei Aufruf was zurückgegeben werden soll)

            // Nachverfolgen, ob Sampling aktiviert wurde
            var exportedData = "testcontent";

            mockDatabaseConnection.Setup(x => x.ExportTrainingsData()).Returns(exportedData);

            //ServiceProvider anlegen
            Assert.NotNull(rootServiceProvider);
            rootServiceProvider.SetValue(null, mockSingleton.Object);

            mockSingleton.Setup(x => x.GetService(typeof(IDataBaseConnection))).Returns(mockDatabaseConnection.Object);

            // Mock crosspermission
            var CrossPermissionMock = new Mock<IPermissions>();
            var currentPermissions = typeof(ImportExportViewModel).GetField("_crossPermissions", BindingFlags.Static | BindingFlags.NonPublic);
            Assert.NotNull(currentPermissions);
            currentPermissions.SetValue(null, CrossPermissionMock.Object);


            var vm = new ImportExportViewModel();

            
            vm.ExportCommand.Execute(null);

            //Assert.Equal(file, importetFile);
        }

    }
}
