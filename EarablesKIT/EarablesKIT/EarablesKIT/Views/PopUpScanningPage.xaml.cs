using EarablesKIT.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Views
{
    /// <summary>
    /// Class PopUpScanningPage is a Pop-up which handles the scanning and connection of bluetooth devices
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpScanningPage : PopupPage
    {
        private ScanningPopUpViewModel _viewModel;

        /// <summary>
        /// Constructor PopUpScanningPage creates a new PopUpScanningPage and it's viewmodel
        /// </summary>
        public PopUpScanningPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ScanningPopUpViewModel();
            ConnectButton.IsEnabled = false;
        }

        private void OnScanningButtonPressed(object sender, EventArgs e)
        {
            _viewModel.ScanDevicesCommand.Execute(this);
            if (_viewModel.DevicesList.Count != 0)
            {
                ConnectButton.IsEnabled = true;
                DevicesListView.SelectedItem = _viewModel.DevicesList[0];
            }
            else
            {
                ConnectButton.IsEnabled = false;
            }
        }
    }
}