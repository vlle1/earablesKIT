using EarablesKIT.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem { Id = MenuItemType.StepMode, Title = "Step Mode" },
                new HomeMenuItem { Id = MenuItemType.CountMode, Title = "Count Mode" },
                new HomeMenuItem { Id = MenuItemType.ListenAndPerform, Title = "Listen and Perform" },
                new HomeMenuItem { Id = MenuItemType.MusicMode, Title = "Music Mode" },
                new HomeMenuItem { Id = MenuItemType.DataOverview, Title = "30-Day-Overview" },
                new HomeMenuItem { Id = MenuItemType.Settings, Title = "Settings" },
                new HomeMenuItem { Id = MenuItemType.ImportExport, Title = "Manage your Data" },
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