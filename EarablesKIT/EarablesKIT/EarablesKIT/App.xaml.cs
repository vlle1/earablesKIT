using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Xamarin.Forms;
using EarablesKIT.Views;
using EarablesKIT.Models.DatabaseService;

namespace EarablesKIT
{                  
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            //Testing TODO
            DatabaseConnection dbConnection = new DatabaseConnection();
            DBEntry entryToSave = new DBEntry(DateTime.Today, 100,50,10);

            int saveDbEntry = dbConnection.SaveDBEntry(entryToSave);
            List<DBEntry> actualEntry = dbConnection.GetAllEntriesAsync();

            MainPage.DisplayAlert("SaveDBEntry",
                "saveDBEntryPrimaryKey: " + saveDbEntry + "\nDBEntry: " + entryToSave + "\nActual Entry: " +
                actualEntry[0]+ " \nEqual? "+ entryToSave.Equals(actualEntry[0]), "Accept", "Cancel");
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
