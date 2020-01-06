using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    class ScanningPopUpViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public static bool IsConnected { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Command ScanDevicesCommand { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Command ConnectDeviceCommand { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Command CancelCommand { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //TODO change object in IDevice from BLE Lib
        public ObservableCollection<object> DevicesList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public ScanningPopUpViewModel()
        {
            throw new NotImplementedException();
        }

        //TODO change EventArgs to DeviceEventArgs
        public void OnDeviceConnectionStateChanged(object sender, EventArgs args)
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
