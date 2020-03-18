using System.IO;
using EarablesKIT.Models;
using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Resources;
using EarablesKIT.Views;
using Plugin.FilePicker.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    /// <summary>
    /// Class ImportExportViewModel contains the logic for <see cref="ImportExportPage"/>ImportExportPage
    /// </summary>
    public class ImportExportViewModel
    {
        private static IPermissions _crossPermissions = null;

        /// <summary>
        /// Command ExportCommand gets called when the Export button is clicked. Calls method ExportData
        /// </summary>
        public Command ExportCommand => new Command(ExportData);

        /// <summary>
        /// Command ImportCommand gets called when the Import button is clicked. Calls method ImportData
        /// </summary>
        public Command ImportCommand => new Command<FileData>(ImportData);

        /// <summary>
        /// Command DeleteCommand gets called when the Delete button is clicked. Calls method DeleteData
        /// </summary>
        public Command DeleteCommand => new Command(DeleteData);

        public ImportExportViewModel()
        {
            if (_crossPermissions is null)
            {
                _crossPermissions = CrossPermissions.Current;
            }
        }

        private async void ExportData()
        {
            var dataBaseConnection = (IDataBaseConnection)ServiceManager.ServiceProvider.GetService(typeof(IDataBaseConnection));

            string exportTrainingsData = dataBaseConnection.ExportTrainingsData();

            var fn = "Trainingsdata.txt";
            var file = Path.Combine(FileSystem.CacheDirectory, fn);
            File.WriteAllText(file, exportTrainingsData);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = AppResources.ImportExportSaveDisplayTitle,
                File = new ShareFile(file)
            });
        }

        private async void ImportData(FileData filedata)
        {
            var status = await _crossPermissions.CheckPermissionStatusAsync(Permission.Storage);
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                {
                    await Application.Current.MainPage.DisplayAlert("Need storage", "Request storage permission", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                //Best practice to always check that the key exists
                if (results.ContainsKey(Permission.Storage))
                    status = results[Permission.Storage];
            }
            if (status != PermissionStatus.Granted)
            {
                return;
            }

            var dataBaseConnection = (IDataBaseConnection)ServiceManager.ServiceProvider.GetService(typeof(IDataBaseConnection));
            dataBaseConnection.ImportTrainingsData(filedata);
        }

        private void DeleteData()
        {
            IDataBaseConnection dataBaseConnection = (IDataBaseConnection)ServiceManager.ServiceProvider.GetService(typeof(IDataBaseConnection));
            dataBaseConnection.DeleteAllEntries();
        }
    }
}