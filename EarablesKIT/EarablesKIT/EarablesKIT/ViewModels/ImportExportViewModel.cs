using EarablesKIT.Models;
using EarablesKIT.Models.DatabaseService;
using Plugin.FilePicker.Abstractions;
using System;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    class ImportExportViewModel
    {

        public Command ExportCommand => new Command<FileData>(ExportData);

        public Command ImportCommand => new Command<FileData>(ImportData);

        public Command DeleteCommand => new Command(DeleteData);

        private async void ExportData(FileData path)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                {
                    await App.Current.MainPage.DisplayAlert("Need storage", "Request storage permission", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                //Best practice to always check that the key exists
                if (results.ContainsKey(Permission.Storage))
                    status = results[Permission.Storage];
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
                    await App.Current.MainPage.DisplayAlert("Need storage", "Request storage permission", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                //Best practice to always check that the key exists
                if (results.ContainsKey(Permission.Storage))
                    status = results[Permission.Storage];
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
