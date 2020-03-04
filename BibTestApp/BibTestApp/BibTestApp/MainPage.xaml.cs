using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.BLE.Abstractions.Exceptions;
using Plugin.BLE.Abstractions;
using System.Threading;
using Plugin.BLE.Abstractions.EventArgs;
using EarablesKIT.Models.Library;
using System.Runtime.CompilerServices;

namespace BibTestApp
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage , INotifyPropertyChanged
    {
        EarablesConnection earables;
        ObservableCollection<IDevice> deviceList = new ObservableCollection<IDevice>();
        ObservableCollection<LPF_Accelerometer> accList = new ObservableCollection<LPF_Accelerometer>();
        public event PropertyChangedEventHandler PropertyChanged;

        private IMUDataEntry entry;
        public IMUDataEntry Entry { get => entry; set { entry = value; OnPropertyChanged("Entry"); }   }
        public MainPage()
        {
            InitializeComponent();
            lv.ItemsSource = deviceList;
            

            accList.Add(LPF_Accelerometer.Hz10);
            accList.Add(LPF_Accelerometer.Hz184);
            accList.Add(LPF_Accelerometer.Hz20);
            accList.Add(LPF_Accelerometer.Hz41);
            accList.Add(LPF_Accelerometer.Hz460);
            accList.Add(LPF_Accelerometer.Hz5);
            accList.Add(LPF_Accelerometer.Hz92);
            accList.Add(LPF_Accelerometer.OFF);


            earables = new EarablesConnection();
            earables.NewDeviceFound += neuesDevice;
            earables.ButtonPressed += buttonPressedEarables;
            earables.DeviceConnectionStateChanged += neuerVerbindungsstatus;
            earables.IMUDataReceived += neueIMUDatenearables;


        }

        private void neueIMUDatenearables(object sender, DataEventArgs e)
        {
            Entry = e.Data;

        }

        private void neuerVerbindungsstatus(object sender, EarablesKIT.Models.Library.DeviceEventArgs e)
        {
            string verbindungsstatus;
            if (e.Connected)
            {
                verbindungsstatus = "Verbunden";
            }
            else
            {
                verbindungsstatus = "Getrennt";
            }
            DisplayAlert("Verbindungsstatus", verbindungsstatus, "OK");
        }

        private void buttonPressedEarables(object sender, ButtonEventArgs e)
        {
            //DisplayAlert("PushButton", "Der PushButton wurde gedrückt", "OK");
            Console.WriteLine("Button gedrückt");
        }

        private void btnGetBluetooth_Cklicked(object sender, EventArgs e)
        {
            string active;
            if (earables.IsBluetoothActive) {
                active = "ON";
            }
            else{
                active = "OFF";
            }
            this.DisplayAlert("Bluetoothstatus", active, "OK");
        }


        private async void btnStartScanning_Cklicked(object sender, EventArgs e)
        {
            earables.StartScanning();   
        }


        private void lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            IDevice device;
            if (lv.SelectedItem == null)
            {
                return;
            }
            device = lv.SelectedItem as IDevice;
            earables.ConnectToDevice(device);
            
        }





        private void neuesDevice(object sender, NewDeviceFoundArgs e)
        {
            deviceList.Add(e.Device);
        }


        private async void btnDisconect_Clicked(object sender, EventArgs e)
        {
            earables.DisconnectFromDevice();
        }


        private async void btnsSetGyroLPF_Clicked(object sender, EventArgs e)
        {
            EarablesKIT.Models.Library.LPF_Gyroscope gyro = EarablesKIT.Models.Library.LPF_Gyroscope.Hz10;
            earables.GyroLPF = gyro;
            await DisplayAlert("Gyro", "Gyroscope wird auf " + gyro + " gesetzt", "OK");
        }

        private async void btnGetGyroLPF_Clicked(object sender, EventArgs e)
        {
            LPF_Gyroscope lpf = await earables.GetGyroscopeLPFFromDevice();
            await DisplayAlert("Gyro", "Gyroscope LPF ist bei " + lpf , "OK");
        }

        private async void btnSetAccLPF_Clicked(object sender, EventArgs e)
        {
            EarablesKIT.Models.Library.LPF_Accelerometer acc = EarablesKIT.Models.Library.LPF_Accelerometer.Hz10;
            earables.AccLPF = acc;
            await DisplayAlert("Acc", "Accelerometer wird auf " + acc + " gesetzt", "OK");
        }

        private async void btnGetAccLPF_Clicked(object sender, EventArgs e)
        {
            LPF_Accelerometer lpf = await earables.GetAccelerometerLPFFromDeviceAsync();
            await DisplayAlert("Acc", "Accelerometer LPF ist bei " + lpf, "OK");

        }


        private async void btnStartSampling_Clicked(object sender, EventArgs e)
        {
            earables.StartSampling();
            DisplayAlert("Sampling", "Sampling gestartet", "OK");
        }

        private async void btnStopSampling_Clicked(object sender, EventArgs e)
        {
            earables.StopSampling();
            DisplayAlert("Sampling", "Sampling gestopt", "OK");
        }


        private async void setName_klicked(object sender, EventArgs e)
        {
            byte[] bytes2 = { 0x45, 0x52, 0x57, 0x49, 0x4E, 0x41 };
           // await CharacteristicNameWrite.WriteAsync(bytes2);
        }

        private async void btnGetBatteryVoltage_Cklicked(object sender, EventArgs e)
        {
            await DisplayAlert("Battery", "BatteryVoltage ist bei " + earables.BatteryVoltage, "OK");
        }

        private async void btnGetSampleRate_Cklicked(object sender, EventArgs e)
        {
            await DisplayAlert("SampleRate", "SampleRate ist " + earables.SampleRate, "OK");
        }

        private async void btnSetSampleRate_Cklicked(object sender, EventArgs e)
        {
            earables.SampleRate = 70;
            await DisplayAlert("SampleRate", "SampleRate wurde auf " + earables.SampleRate + "gesetzt", "OK");
        }

        private async void btnGetConnection_Cklicked(object sender, EventArgs e)
        {
            await DisplayAlert("Verbindung", "verbindung ist " + earables.Connected, "OK");
        }
        
       // [NotifyPropertyChangedInvocator]
       // protected virtual void OnPropertyChanged(IMUDataEntry entry)
       // {
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(entry));
       // }
        
    }
}

