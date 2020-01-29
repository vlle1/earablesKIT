using EarablesKIT.Models.DatabaseService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using EarablesKIT.Models;
using Xamarin.Forms;
using IDataBaseConnection = EarablesKIT.Models.DatabaseService.IDataBaseConnection;

namespace EarablesKIT.ViewModels
{
    /// <summary>
    /// Class DataOverviewViewModel contains the logic behind the page DataOverview/>
    /// </summary>
    class DataOverviewViewModel
    {

        /// <summary>
        /// Trainingsdata as observable collection containing the trainingsdata from the database
        /// </summary>
        public ObservableCollection<DBEntry> TrainingsDataDbEntries { get; set; }

        /// <summary>
        /// Constructor for class DataOverviewViewModel
        /// </summary>
        public DataOverviewViewModel()
        {
            var dataBaseConnection = (IDataBaseConnection) ServiceManager.ServiceProvider.GetService(typeof(IDataBaseConnection));

            List<DBEntry> entries = dataBaseConnection.GetMostRecentEntries(30);
            entries.Reverse();
            TrainingsDataDbEntries = new ObservableCollection<DBEntry>(entries);

        }

        /// <summary>
        /// Eventmethod OnAppearing is called when the page receives the focus of the app. TrainingsDataDbEntries gets updated.
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">EventArgs of the event</param>
        public void OnAppearing(object sender, EventArgs e)
        {
            var dataBaseConnection = (IDataBaseConnection)ServiceManager.ServiceProvider.GetService(typeof(IDataBaseConnection));
            List<DBEntry> entries = dataBaseConnection.GetMostRecentEntries(30);
            entries.Reverse();
            TrainingsDataDbEntries.Clear();
            foreach (DBEntry dbEntry in entries)
            {
                TrainingsDataDbEntries.Add(dbEntry);
            }
        }
    }
}
