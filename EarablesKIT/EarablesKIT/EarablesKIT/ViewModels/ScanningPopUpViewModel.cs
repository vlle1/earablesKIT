using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using EarablesKIT.Models.Library;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;
using Rg.Plugins.Popup;

namespace EarablesKIT.ViewModels
{
    class ScanningPopUpViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public static bool IsConnected { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Command ScanDevicesCommand { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Command ConnectDeviceCommand { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Command CancelCommand { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ObservableCollection<IDevice> DevicesList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public ScanningPopUpViewModel()
        {
            throw new NotImplementedException();
        }

        //TODO change EventArgs to DeviceEventArgs
        public void OnDeviceConnectionStateChanged(object sender, DeviceEventArgs args)
        {
            throw new NotImplementedException();
        }

        public static void ShowPopUp()
        {
            throw new NotImplementedException();
        }
        public static void HidePopUp()
        {
            throw new NotImplementedException();
        }
    }
}
