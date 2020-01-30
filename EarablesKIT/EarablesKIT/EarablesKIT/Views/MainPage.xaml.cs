using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using EarablesKIT.Models;

namespace EarablesKIT.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.About, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.CountMode:
                        MenuPages.Add(id, new NavigationPage(new CountModePage()));
                        break;
                    case (int)MenuItemType.DataOverview:
                        MenuPages.Add(id, new NavigationPage(new DataOverviewPage()));
                        break;
                    case (int)MenuItemType.ImportExport:
                        MenuPages.Add(id, new NavigationPage(new ImportExportPage()));
                        break;
                    case (int)MenuItemType.ListenAndPerform:
                        MenuPages.Add(id, new NavigationPage(new ListenAndPerformPage()));
                        break;
                    case (int)MenuItemType.MusicMode:
                        MenuPages.Add(id, new NavigationPage(new MusicModePage()));
                        break;
                    case (int)MenuItemType.Settings:
                        MenuPages.Add(id, new NavigationPage(new SettingsPage()));
                        break;
                    case (int)MenuItemType.StepMode:
                        MenuPages.Add(id, new NavigationPage(new StepModePage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}