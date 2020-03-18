using System;
using System.Collections.Generic;
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

            var crossPermissionMock = new Mock<IPermissions>();
            crossPermissionMock.Setup(x => x.CheckPermissionStatusAsync(It.IsAny<Permission>())).Returns(
                async () => permissionGranted);

            var currentPermissions = typeof(ImportExportViewModel).GetField("_crossPermissions", BindingFlags.Static | BindingFlags.NonPublic);
            Assert.NotNull(currentPermissions);
            currentPermissions.SetValue(null, crossPermissionMock.Object);

            var vm = new ImportExportViewModel();

            var file = new FileData("", "TestData.txt", () => new FileStream("TestData.txt", FileMode.Open), null);
            
            vm.ImportCommand.Execute(file);

            Assert.Equal(file, importetFile);
        }


        [Fact]
        public void testDeleteCommand()
        {
            IServiceProvider unused = ServiceManager.ServiceProvider;

            //Feld Infos holen
            FieldInfo rootServiceProvider = typeof(ServiceManager).GetField("_serviceProvider", BindingFlags.Static | BindingFlags.NonPublic);

            //Mocksaufsetzen 
            //ServiceProvider
            Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();

            //Service der gemockt werden soll
            Mock<IDataBaseConnection> mockDatabaseConnection = new Mock<IDataBaseConnection>();
            mockDatabaseConnection.Setup(connection => connection.DeleteAllEntries());

            mockSingleton.Setup(provider => provider.GetService(typeof(IDataBaseConnection)))
                .Returns(mockDatabaseConnection.Object);
            rootServiceProvider.SetValue(null, mockSingleton.Object);

            //Setup Current
            var CrossPermissionMock = new Mock<IPermissions>();

            var currentPermissions = typeof(ImportExportViewModel).GetField("_crossPermissions", BindingFlags.Static | BindingFlags.NonPublic);
            Assert.NotNull(currentPermissions);
            currentPermissions.SetValue(null, CrossPermissionMock.Object);

            //Act
            ImportExportViewModel viewModel = new ImportExportViewModel();
            viewModel.DeleteCommand.Execute(null);

            //Verify
            mockDatabaseConnection.VerifyAll();
            mockSingleton.VerifyAll();
        }


        [Fact]
        public void testConstructor()
        {
            
            //CrossPermission
            FieldInfo currentCrossPermissionMock =
                typeof(CrossPermissions).GetField("implementation", BindingFlags.NonPublic | BindingFlags.Static);

            Mock<IPermissions> mockImplementation = new Mock<IPermissions>();
            Assert.NotNull(currentCrossPermissionMock);
            currentCrossPermissionMock.SetValue(null, new Lazy<IPermissions>(() => mockImplementation.Object));

            //Act
            ImportExportViewModel model = new ImportExportViewModel();

            //Verify
            FieldInfo _crossPermissionsField =
                typeof(ImportExportViewModel).GetField("_crossPermissions", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(_crossPermissionsField);
            IPermissions value = (IPermissions)_crossPermissionsField.GetValue(null);
            Assert.NotNull(value);
        }

    }
}
