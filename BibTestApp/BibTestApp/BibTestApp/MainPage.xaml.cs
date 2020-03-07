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
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        EarablesConnection earables;
        ObservableCollection<IDevice> deviceList = new ObservableCollection<IDevice>();
        public event PropertyChangedEventHandler PropertyChanged;
        private const string HZ5 = "5Hz";
        private const string HZ10 = "10Hz";
        private const string HZ20 = "20Hz";
        private const string HZ41 = "41Hz";
        private const string HZ92 = "92Hz";
        private const string HZ184 = "184Hz";
        private const string HZ460 = "460Hz";
        private const string OFF = "OFF";
        private const string ABBORT = "Abbruch";
        private const string OKAY = "Okay";
        private const string CHOSE = "Wähle einen LPF aus";
        private const string CHOSE_SAMPLERATE = "Gib eine samplerate ein (Hinweis: Samplerate muss zwischen 1 und 100 liegen)";
        private const string HZ250 = "250Hz";
        private const string HZ3600 = "3600Hz";
        private IMUDataEntry entry;
        public IMUDataEntry Entry { get => entry; set { entry = value; OnPropertyChanged(nameof(Entry)); } }
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            lv.ItemsSource = deviceList;

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
            DisplayAlert("PushButton", "Der PushButton wurde gedrückt", "OK");
            Console.WriteLine("Button gedrückt");
        }

        private void btnGetBluetooth_Cklicked(object sender, EventArgs e)
        {
            string active;
            if (earables.IsBluetoothActive) {
                active = "ON";
            }
            else {
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
            string lpf = await Application.Current.MainPage.DisplayActionSheet(CHOSE,
            ABBORT, null, HZ5, HZ10, HZ20, HZ41, HZ92, HZ184, HZ250, HZ3600, OFF);
            if (lpf != null && !lpf.Equals("") && !lpf.Equals(ABBORT))
            {
                EarablesKIT.Models.Library.LPF_Gyroscope gyro = EarablesKIT.Models.Library.LPF_Gyroscope.Hz5;
                switch (lpf)
                {
                    case HZ5:
                        gyro = EarablesKIT.Models.Library.LPF_Gyroscope.Hz5;
                        break;
                    case HZ10:
                        gyro = EarablesKIT.Models.Library.LPF_Gyroscope.Hz10;
                        break;
                    case HZ20:
                        gyro = EarablesKIT.Models.Library.LPF_Gyroscope.Hz20;
                        break;
                    case HZ41:
                        gyro = EarablesKIT.Models.Library.LPF_Gyroscope.Hz41;
                        break;
                    case HZ92:
                        gyro = EarablesKIT.Models.Library.LPF_Gyroscope.Hz92;
                        break;
                    case HZ184:
                        gyro = EarablesKIT.Models.Library.LPF_Gyroscope.Hz184;
                        break;
                    case HZ250:
                        gyro = EarablesKIT.Models.Library.LPF_Gyroscope.Hz250;
                        break;
                    case HZ3600:
                        gyro = EarablesKIT.Models.Library.LPF_Gyroscope.Hz3600;
                        break;
                    case OFF:
                        gyro = EarablesKIT.Models.Library.LPF_Gyroscope.OFF;
                        break;
                }

                earables.GyroLPF = gyro;
                await DisplayAlert("Gyro", "Gyroscope wird auf " + gyro + " gesetzt", "OK");
            }

        }

        private async void btnGetGyroLPF_Clicked(object sender, EventArgs e)
        {
            LPF_Gyroscope lpf = await earables.GetGyroscopeLPFFromDevice();
            await DisplayAlert("Gyro", "Gyroscope LPF ist bei " + lpf, "OK");
        }

        private async void btnSetAccLPF_Clicked(object sender, EventArgs e)
        {
            string lpf = await Application.Current.MainPage.DisplayActionSheet(CHOSE,
            ABBORT, null, HZ5, HZ10, HZ20, HZ41, HZ92, HZ184, HZ460, OFF);

            EarablesKIT.Models.Library.LPF_Accelerometer acc = EarablesKIT.Models.Library.LPF_Accelerometer.Hz10;
            if (lpf != null && !lpf.Equals("") && !lpf.Equals(ABBORT))
            {
                switch (lpf)
                {
                    case HZ5:
                        acc = EarablesKIT.Models.Library.LPF_Accelerometer.Hz5;
                        break;
                    case HZ10:
                        acc = EarablesKIT.Models.Library.LPF_Accelerometer.Hz10;
                        break;
                    case HZ20:
                        acc = EarablesKIT.Models.Library.LPF_Accelerometer.Hz20;
                        break;
                    case HZ41:
                        acc = EarablesKIT.Models.Library.LPF_Accelerometer.Hz41;
                        break;
                    case HZ92:
                        acc = EarablesKIT.Models.Library.LPF_Accelerometer.Hz92;
                        break;
                    case HZ184:
                        acc = EarablesKIT.Models.Library.LPF_Accelerometer.Hz184;
                        break;
                    case HZ460:
                        acc = EarablesKIT.Models.Library.LPF_Accelerometer.Hz460;
                        break;
                    case OFF:
                        acc = EarablesKIT.Models.Library.LPF_Accelerometer.OFF;
                        break;
                }

            }

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

            string newSampleRate = await Application.Current.MainPage.DisplayPromptAsync("Samplerate",
                        CHOSE_SAMPLERATE, initialValue: "50", maxLength: 3, keyboard: Keyboard.Numeric);
            if (newSampleRate != null && !newSampleRate.Equals("") && int.Parse(newSampleRate) > 0 && int.Parse(newSampleRate) < 101)
            {
                earables.SampleRate = int.Parse(newSampleRate);
                await DisplayAlert("SampleRate", "SampleRate wurde auf " + earables.SampleRate + " gesetzt", "OK");
            }
            if (!(int.Parse(newSampleRate) > 0) || !(int.Parse(newSampleRate) < 101))
            {
                await DisplayAlert("SampleRate", earables.SampleRate +
                    " ist ein ungültiger Wert. Die Samplerate muss zwischen 1 und 100 liegen", "OK");
            }
        }

        private async void btnGetConnection_Cklicked(object sender, EventArgs e)
        {
            await DisplayAlert("Verbindung", "verbindung ist " + earables.Connected, "OK");
        }

        //[NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void btnSetGyroRange_Clicked(object sender, EventArgs e)
        {
            string range = await Application.Current.MainPage.DisplayActionSheet(CHOSE,
            ABBORT, null, "250 deg/s", "500 deg/s", "1000 deg/s", "2000 deg/s");

            int rangeInt = 0;
            String s = "";
            if (range != null && !range.Equals("") && !range.Equals(ABBORT))
            {
                switch (range)
                {
                    case "250 deg/s":
                        rangeInt = 0x00;
                        s = "250 deg/s";
                        break;
                    case "500 deg/s":
                        rangeInt = 0x08;
                        s = "500 deg/s";
                        break;
                    case "1000 deg/s":
                        rangeInt = 0x10;
                        s = "1000 deg/s";
                        break;
                    case "2000 deg/s":
                        rangeInt = 0x18;
                        s = "2000 deg/s";
                        break;
                }

            }

            earables.SetGyroscopeRange(rangeInt);
            await DisplayAlert("Gyro Range", "Gyroscope Range wird auf " + s + " gesetzt", "OK");
        }

        private async void btnGetGyroScalefactor_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Gyroscope", "Gyroscope Scale Factor ist " + earables.config.GyroScaleFactor, "OK");
        }

    }
}

