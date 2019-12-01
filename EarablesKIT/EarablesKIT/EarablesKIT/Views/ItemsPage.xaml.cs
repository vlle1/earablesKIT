using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EarablesKIT.ViewModels;
using System.Collections.Generic;
using Plugin.BLE;

namespace EarablesKIT.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {

        List<String> foundDevices = new List<String>();

        private async void scanBLEdevices()
        {
            var ble = CrossBluetoothLE.Current;
            var adapter = CrossBluetoothLE.Current.Adapter;

            var deviceList = new List<Plugin.BLE.Abstractions.Contracts.IDevice>();

            adapter.DeviceDiscovered += (s, a) => deviceList.Add(a.Device);
            await adapter.StartScanningForDevicesAsync();
            
            foundDevices.Clear();

            foreach (var d in deviceList)
            {
                
                foundDevices.Add(d.State + ": " + d.Name + "\t" + d.Id);
            }

            ItemsListView.ItemsSource = foundDevices;

        }

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = this;
        }

        private void Scan_Clicked(object sender, EventArgs e)
        {
            scanBLEdevices();
        }
    }
}