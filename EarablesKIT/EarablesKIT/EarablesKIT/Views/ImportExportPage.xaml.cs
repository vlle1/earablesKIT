using EarablesKIT.Resources;
using EarablesKIT.ViewModels;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Views
{
    /// <summary>
    /// Codebehind for class <see cref="ImportExportPage"/>
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImportExportPage : ContentPage
    {
        private ImportExportViewModel _viewModel;

        /// <summary>
        /// Command DeleteDataCommand is called when the DeleteData command is pressed. Calls method DeleteEntries
        /// </summary>
        public ICommand DeleteDataCommand => new Command(DeleteEntries);

        /// <summary>
        /// Constructor for class ImportExportPage
        /// </summary>
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
                if (filedata == null || string.IsNullOrEmpty(filedata.FilePath) || !filedata.FilePath.EndsWith(".txt"))
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

            _viewModel.ExportCommand.Execute(filedata);
        }

        private void DeleteEntries()
        {
            _viewModel.DeleteCommand.Execute(this);
        }
    }
}