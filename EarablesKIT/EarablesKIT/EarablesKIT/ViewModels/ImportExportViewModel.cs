using EarablesKIT.Models;
using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Views;
using Plugin.FilePicker.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    /// <summary>
    /// Class ImportExportViewModel contains the logic for <see cref="ImportExportPage"/>ImportExportPage
    /// </summary>
    internal class ImportExportViewModel
    {
        /// <summary>
        /// Command ExportCommand gets called when the Export button is clicked. Calls method ExportData
        /// </summary>
        public Command ExportCommand => new Command<FileData>(ExportData);

        /// <summary>
        /// Command ImportCommand gets called when the Import button is clicked. Calls method ImportData
        /// </summary>
        public Command ImportCommand => new Command<FileData>(ImportData);

        /// <summary>
        /// Command DeleteCommand gets called when the Delete button is clicked. Calls method DeleteData
        /// </summary>
        public Command DeleteCommand => new Command(DeleteData);

        private async void ExportData(FileData path)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
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

            IDataBaseConnection dataBaseConnection = (IDataBaseConnection)ServiceManager.ServiceProvider.GetService(typeof(IDataBaseConnection));
            dataBaseConnection.ExportTrainingsData(path.FilePath);
        }

        private async void ImportData(FileData filedata)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
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

            IDataBaseConnection dataBaseConnection = (IDataBaseConnection)ServiceManager.ServiceProvider.GetService(typeof(IDataBaseConnection));
            dataBaseConnection.ImportTrainingsData(filedata);
        }

        private void DeleteData()
        {
            IDataBaseConnection dataBaseConnection = (IDataBaseConnection)ServiceManager.ServiceProvider.GetService(typeof(IDataBaseConnection));
            dataBaseConnection.DeleteAllEntries();
        }
    }
}