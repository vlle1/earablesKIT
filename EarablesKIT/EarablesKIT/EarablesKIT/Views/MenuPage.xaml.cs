using EarablesKIT.Models;
using EarablesKIT.Resources;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace EarablesKIT.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        /// <summary>
        /// The MenuPage contains all Pages that the user can navigate to directly.
        /// The implementation follows the standard implementation of the Master-Detail App of Xamarin Forms.
        /// </summary>
        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem { Id = MenuItemType.StepMode, Title = AppResources.StepModeNameLabel},
                new HomeMenuItem { Id = MenuItemType.CountMode, Title = AppResources.CountModeNameLabel },
                new HomeMenuItem { Id = MenuItemType.ListenAndPerform, Title =  AppResources.ListenAndPerformNameLabel },
                new HomeMenuItem { Id = MenuItemType.MusicMode, Title = AppResources.MusicModeEntryLabel },
                new HomeMenuItem { Id = MenuItemType.DataOverview, Title = AppResources.DataOverviewTitle },
                new HomeMenuItem { Id = MenuItemType.Settings, Title = AppResources.SettingsTitle},
                new HomeMenuItem { Id = MenuItemType.ImportExport, Title = AppResources.ImportExportLabel },
            };
            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }
    }
}