using System;
using EarablesKIT.Models;
using EarablesKIT.Models.Library;
using EarablesKIT.Resources;
using EarablesKIT.Views;
using Plugin.BLE.Abstractions.Contracts;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EarablesKIT.Annotations;
using Plugin.BLE.Abstractions.Exceptions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    /// <summary>
    /// Class ScanningPopUpViewModel handles the pop-up which gets shown, when
    /// <list type="bullet">
    /// <item>the connected disconnects</item>
    /// <item>an activity gets started without an active connection</item>
    /// </list>
    /// </summary>
    internal class ScanningPopUpViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Property IsConnected represents if a device is currently connected
        /// </summary>
        public static bool IsConnected { get; set; }

        /// <summary>
        /// Command ScanDevicesCommand calls method ScanDevices and handles the scanning process
        /// </summary>
        public ICommand ScanDevicesCommand => new Command(ScanDevices);

        /// <summary>
        /// Command ConnectDeviceCommand calls method ConnectDevice. Needs to get an instance of IDevice as a parameter.
        /// That is the device which should get connected to.
        /// </summary>
        public ICommand ConnectDeviceCommand => new Command<IDevice>(ConnectDevice);

        /// <summary>
        /// Command CancelCommand calls method HidePopUp. It hides the pop-up
        /// </summary>
        public ICommand CancelCommand => new Command(HidePopUp);

        /// <summary>
        /// The ObservableCollection DevicesList contains the current scanned devices
        /// </summary>
        public ObservableCollection<IDevice> DevicesList { get; set; }

        private EarablesConnection _earablesConnectionService;

        /// <summary>
        /// Constructor ScanningPopUpViewModel initializes the attributes and properties
        /// </summary>
        public ScanningPopUpViewModel()
        {
            DevicesList = new ObservableCollection<IDevice>();
            _earablesConnectionService = (EarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection));
            _earablesConnectionService.NewDeviceFound += (sender, args) =>
            {

                    if (args.Device.Name != null && args.Device.Name.StartsWith("eSense"))
                    {
                        DevicesList.Add(args.Device);
                        OnPropertyChanged(nameof(DevicesList));
                    }
                

            };
        }

        /// <summary>
        /// Method OnDeviceConnectionStateChanged is registered as an eventmethod in <see
        /// cref="IEarablesConnection"/>. Gets called, every time a Device connects or disconnects.
        /// It a device disconnects, this method shows the popup.
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="args">Arguments of the event, containing the connection status of the device</param>
        public static void OnDeviceConnectionStateChanged(object sender, DeviceEventArgs args)
        {
            IsConnected = args.Connected;
            if (!args.Connected)
            {
                ShowPopUp();
            }
            else
            {
                HidePopUp();
            }
        }

        /// <summary>
        /// Creates a PopUpScanningPage instance and shows it.
        /// </summary>
        public static void ShowPopUp()
        {

            PopupNavigation.Instance.PushAsync(new PopUpScanningPage(), true);
        }

        /// <summary>
        /// Hides the pop-up.
        /// </summary>
        public static void HidePopUp()
        {
            PopupNavigation.Instance.PopAsync(true);
        }

        private async void ScanDevices()
        {
            
            DevicesList.Clear();
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Unknown))
                {
                    await Application.Current.MainPage.DisplayAlert("Need location", "Gunna need that location", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                status = results[Permission.Location];
            }
            
            if (status != PermissionStatus.Granted)
            {
                await Application.Current.MainPage.DisplayAlert("Location Permission", "Denied!", "OK");
                return;
            }

            _earablesConnectionService.StartScanning();


        }

        private void ConnectDevice(IDevice selectedItem)
        {
            try
            {
                _earablesConnectionService.ConnectToDevice(selectedItem);
            }
            catch (DeviceConnectionException e)
            {
                Application.Current.MainPage.DisplayAlert(AppResources.Error,
                    AppResources.ScanningPopUpAlertCouldntConnect, AppResources.Accept);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}