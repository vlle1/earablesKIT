using EarablesKIT.Models;
using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Models.SettingsService;
using EarablesKIT.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using EarablesKIT.Models.Library;
using EarablesKIT.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace EarablesKIT
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();

            ISettingsService SettingsService =
                (ISettingsService)ServiceManager.ServiceProvider.GetService(typeof(ISettingsService));
            System.Globalization.CultureInfo.CurrentUICulture =
                (SettingsService).ActiveLanguage;

        }

        protected override void OnStart()
        {
            //Register eventmethode for ScanningPopUpViewModel
            //TODO uncomment
            EarablesConnection service = (EarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection));
            service.DeviceConnectionStateChanged += ScanningPopUpViewModel.OnDeviceConnectionStateChanged;

            if(!service.Connected)
            this.showPopUp();
        }

        private async void showPopUp()
        {
            ScanningPopUpViewModel.ShowPopUp();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}