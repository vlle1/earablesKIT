﻿using EarablesKIT.Annotations;
using EarablesKIT.Models;
using EarablesKIT.Models.Library;
using EarablesKIT.Models.PopUpService;
using EarablesKIT.Resources;
using EarablesKIT.Views;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
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
    public class ScanningPopUpViewModel : INotifyPropertyChanged
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

        private IEarablesConnection _earablesConnectionService;
        private IExceptionHandler _exceptionHandler;
        private IPopUpService _popUpService;

        /// <summary>
        /// Constructor ScanningPopUpViewModel initializes the attributes and properties
        /// </summary>
        public ScanningPopUpViewModel()
        {
            DevicesList = new ObservableCollection<IDevice>();
            _earablesConnectionService = (IEarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection));
            _earablesConnectionService.NewDeviceFound += (sender, args) =>
            {
                if (args.Device.Name != null && !DevicesList.Contains(args.Device))
                {
                    if (args.Device.Name.StartsWith("eSense"))
                    {
                        DevicesList.Insert(0, args.Device);
                    }
                    else
                    {
                        DevicesList.Add(args.Device);
                    }
                    OnPropertyChanged(nameof(DevicesList));
                }
            };

            _popUpService = (IPopUpService)ServiceManager.ServiceProvider.GetService(typeof(IPopUpService));
            _exceptionHandler =
                (IExceptionHandler)ServiceManager.ServiceProvider.GetService(typeof(IExceptionHandler));
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
            PopupNavigation.Instance.PushAsync(new PopUpScanningPage());
        }

        /// <summary>
        /// Hides the pop-up.
        /// </summary>
        public static void HidePopUp()
        {
            PopupNavigation.Instance.PopAsync();
        }

        private async void ScanDevices()
        {
            DevicesList.Clear();
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (!_earablesConnectionService.IsBluetoothActive)

            {

                await _popUpService.DisplayAlert(AppResources.Error, AppResources.ScanningPopUpTurnBluetoothOn, AppResources.Accept);

                return;
            }
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Unknown))
                {
                    await _popUpService.DisplayAlert(AppResources.ScanningPopUpAlertLabel, AppResources.ScanningPopUpPermissionLocationNeeded, AppResources.Accept);
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                status = results[Permission.Location];
            }



            if (status != PermissionStatus.Granted)
            {
                await _popUpService.DisplayAlert(AppResources.ScanningPopUpAlertLabel, AppResources.ScanningPopUpLocationDenied, AppResources.Accept);
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
                DevicesList.Clear();
                _exceptionHandler.HandleException(e);
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