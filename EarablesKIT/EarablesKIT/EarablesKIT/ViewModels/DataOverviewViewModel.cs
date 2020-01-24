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
    class DataOverviewViewModel
    {

        public ObservableCollection<DBEntry> TrainingsDataDbEntries { get; set; }

        public DataOverviewViewModel()
        {
            var dataBaseConnection = (IDataBaseConnection) ServiceManager.ServiceProvider.GetService(typeof(IDataBaseConnection));

            for (int i = 1; i <= 31; i++)
            {
                dataBaseConnection.SaveDBEntry(new DBEntry(new DateTime(2000, 01, i), 1337, 69, 42));
            }

            List<DBEntry> entries = dataBaseConnection.GetMostRecentEntries(30);
            entries.Reverse();
            TrainingsDataDbEntries = new ObservableCollection<DBEntry>(entries);

        }

    }
}
