using EarablesKIT.Resources;
using EarablesKIT.ViewModels;
using Plugin.BLE.Abstractions.Contracts;
using Rg.Plugins.Popup.Pages;
using System;
using System.ComponentModel;
using Xamarin.Forms;
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
            _viewModel.PropertyChanged += UpdateList;
        }

        /// <summary>
        /// Method UpdateList updates the devices list in ScanningPopUpPage
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="eventArgs">Arguments of the event</param>
        public void UpdateList(object sender, PropertyChangedEventArgs eventArgs)
        {
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

        private void ConnectButton_Clicked(object sender, EventArgs e)
        {
            IDevice selectedDevice = (IDevice)DevicesListView.SelectedItem;
            ConnectButton.IsEnabled = false;
            PleaseWaitLabel.IsVisible = true;
            Device.StartTimer(new TimeSpan(0, 0, 5), () =>
            {
                return PleaseWaitLabel.IsVisible = false;
            });
            try
            {
                _viewModel.ConnectDeviceCommand.Execute(selectedDevice);
            }
            catch (Exception)
            {
                AlertLabel.Text = AppResources.Error + ": " + AppResources.ScanningPopUpAlertCouldntConnect;

                ConnectButton.IsEnabled = true;
                PleaseWaitLabel.IsVisible = false;
            }
        }
    }
}