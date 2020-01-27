using EarablesKIT.Models;
using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Resources;
using EarablesKIT.ViewModels;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImportExportPage : ContentPage
    {
        private ImportExportViewModel _viewModel;

        public ICommand DeleteDataCommand => new Command(DeleteEntries);

        

        public ImportExportPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ImportExportViewModel();
        }


        private async void ImportButton_Clicked(object sender, EventArgs e)
        {
            FileData filedata;
            try
            {
                filedata = await CrossFilePicker.Current.PickFile();
            }
            catch (Exception)
            {
                ExceptionHandlingViewModel.HandleException(new Exception(AppResources.ImportExportFileError));
                return;
            }
            _viewModel.ImportCommand.Execute(filedata);
        }

        private async void ExportButton_Clicked(object sender, EventArgs e)
        {
            FileData filedata;
            try
            {
                filedata = await CrossFilePicker.Current.PickFile();
                if (filedata.FilePath.EndsWith(".txt"))
                {
                    ExceptionHandlingViewModel.HandleException(new Exception(AppResources.ImportExportFileError));
                    return;
                }
            }
            catch (Exception)
            {
                ExceptionHandlingViewModel.HandleException(new Exception(AppResources.ImportExportFileError));
                return;
            }

            _viewModel.ExportCommand.Execute(filedata.FilePath);
        }

        private void DeleteEntries()
        {
            _viewModel.DeleteCommand.Execute(this);
        }
    }
}