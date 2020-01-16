using SQLite;
using System;
using System.Collections.Generic;
using System.IO;

namespace EarablesKIT.Models.DatabaseService
{
    internal class DatabaseConnection : IDataBaseConnection
    {

        private readonly SQLiteConnection _database;

        private readonly string _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "EarablesKIT_TrainingsData.db3");

        public DatabaseConnection()
        {
            _database = new SQLiteConnection(_path);
            _database.CreateTable<DBEntry>();
        }

        public void DeleteAllEntries()
        {
            _database.DropTable<DBEntry>();
        }
        
        public void ExportTrainingsData(string path)
        {
            throw new NotImplementedException();
        }

        public List<DBEntry> GetAllEntriesAsync()
        {
            return _database.Table<DBEntry>().ToList();
        }


        /// <summary>
        /// Method <c>GetMostRecentEntriesAsync</c> connects to the SQLite Database and gets the amount' recent entries.
        /// If the param amount is greater than the amount of DBEntries which are saved, all saved Entries will get returned. Careful, There are not amount' many DBEntries in this returning List!
        /// </summary>
        /// <param name="amount">The 'amount' lastest database entries</param>
        /// <returns>A List of database entries wrapped in class <see cref="DBEntry"/>
        /// If the Database doesn't have the amount DBEntries saved, returns all saved entries instead!</returns>
        public List<DBEntry> GetMostRecentEntriesAsync(int amount)
        {
            List<DBEntry> dbEntries = this.GetAllEntriesAsync();
            if (amount >= dbEntries.Count)
            {
                return dbEntries;
            }

            return dbEntries.GetRange(0, amount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public int SaveDBEntry(DBEntry entry)
        {
            int primaryResult = -1;
            TableQuery<DBEntry> resultQuery = _database.Table<DBEntry>();
            DBEntry result = null;

            foreach (var s in resultQuery)
            {
                if (s.Date.Equals(entry.Date))
                {
                    result = s;
                    break;
                }
            }
            // DBEntry result = resultQuery.First();
            if (result == null)
            {
                primaryResult = _database.Insert(entry);
            }
            else
            {
                DBEntry toUpdate = result;
                toUpdate.TrainingsData[DBEntry.StepAmountIdentifier] +=
                    entry.TrainingsData[DBEntry.StepAmountIdentifier];

                toUpdate.TrainingsData[DBEntry.PushUpAmountIdentifier] +=
                    entry.TrainingsData[DBEntry.PushUpAmountIdentifier];

                toUpdate.TrainingsData[DBEntry.SitUpAmountIdentifier] +=
                    entry.TrainingsData[DBEntry.SitUpAmountIdentifier];

                primaryResult = _database.Update(toUpdate);
            }
            return primaryResult;
        }
    }
}