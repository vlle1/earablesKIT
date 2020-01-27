using EarablesKIT.Models;
using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Models.SettingsService;
using EarablesKIT.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}