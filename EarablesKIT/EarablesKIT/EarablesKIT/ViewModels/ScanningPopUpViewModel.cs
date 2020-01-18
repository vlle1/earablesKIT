using EarablesKIT.Models;
using EarablesKIT.Models.Library;
using Plugin.BLE.Abstractions.Contracts;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using EarablesKIT.Views;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    internal class ScanningPopUpViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static bool IsConnected { get; set; }

        public ICommand ScanDevicesCommand => new Command(ScanDevices);

        public ICommand ConnectDeviceCommand => new Command(ConnectDevice);

        public ICommand CancelCommand => new Command(HidePopUp);

        public ObservableCollection<string> DevicesList { get; set; }

        private readonly IEarablesConnection _earablesConnectionService;

        public ScanningPopUpViewModel()
        {
            DevicesList = new ObservableCollection<string>();
            //_earablesConnectionService= (IEarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection));
            //_earablesConnectionService.DeviceConnectionStateChanged += OnDeviceConnectionStateChanged;
        }

        public void OnDeviceConnectionStateChanged(object sender, DeviceEventArgs args)
        {
            IsConnected = args.Connected;
            if (!args.Connected)
            {
                ShowPopUp();
            }
        }

        public static void ShowPopUp()
        {
            PopupNavigation.Instance.PushAsync(new PopUpScanningPage(), true);
        }

        public static void HidePopUp()
        {
            PopupNavigation.Instance.PopAsync(true);
        }

        private void ScanDevices()
        {
            DevicesList.Clear();   
            //List<IDevice> scannedDevices = _earablesConnectionService.StartScanning();
            List<string> scannedDevices = new List<string>();
            scannedDevices.Add("Entry1");
            scannedDevices.Add("Entry2");
            scannedDevices.Add("Entry3");
            foreach (string device in scannedDevices)
            {
                DevicesList.Add(device);
            }
        }

        private void ConnectDevice(object selectedItem)
        {
            //bool connectToDevice = _earablesConnectionService.ConnectToDevice(selectedItem);
            Application.Current.MainPage.DisplayAlert("Connected!","You are " + selectedItem+"connected! ","Accept","Cancel");
        }
    }
}