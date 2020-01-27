using EarablesKIT.Models;
using EarablesKIT.Models.DatabaseService;
using Plugin.FilePicker.Abstractions;
using System;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    class ImportExportViewModel
    {

        public Command ExportCommand => new Command<string>(ExportData);

        public Command ImportCommand => new Command<FileData>(ImportData);

        public Command DeleteCommand => new Command(DeleteData);

        private void ExportData(string path)
        {

            IDataBaseConnection dataBaseConnection = (IDataBaseConnection)ServiceManager.ServiceProvider.GetService(typeof(IDataBaseConnection));
            dataBaseConnection.ExportTrainingsData(path);
        }

        private void ImportData(FileData filedata)
        {
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
