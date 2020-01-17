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

        public SQLiteConnection Database => _database;

        public DatabaseConnection()
        {
            _database = new SQLiteConnection(_path);
            _database.CreateTable<DBEntryToSave>();
        }

        public void DeleteAllEntries()
        {
            _database.DropTable<DBEntryToSave>();
            _database.CreateTable<DBEntryToSave>();
        }
        

        public List<DBEntry> GetAllEntriesAsync()
        {
            List<DBEntry> dBEntries = new List<DBEntry>();
            List<DBEntryToSave> dbEntriesString = _database.Table<DBEntryToSave>().ToList();
            foreach (var entry in dbEntriesString)
            {
                dBEntries.Add(DBEntry.ParseDbEntry(entry));
            }
            return dBEntries;
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
        /// <param name="entryArg"></param>
        /// <returns></returns>
        public int SaveDBEntry(DBEntry entryArg)
        {
            int primaryResult = -1;
            TableQuery<DBEntryToSave> resultQuery = _database.Table<DBEntryToSave>();
            DBEntry result = null;

            foreach (var dbEntry in resultQuery)
            {
                if (dbEntry.DateTime.Equals(entryArg.Date))
                {
                    result = DBEntry.ParseDbEntry(dbEntry);
                    break;
                }
            }
            // DBEntry result = resultQuery.First();
            if (result == null)
            {
                primaryResult = _database.Insert(entryArg.ConvertToDBEntryToSave());
            }
            else
            {
                DBEntry toUpdate = result;
                toUpdate.TrainingsData[DBEntry.StepAmountIdentifier] +=
                    entryArg.TrainingsData[DBEntry.StepAmountIdentifier];

                toUpdate.TrainingsData[DBEntry.PushUpAmountIdentifier] +=
                    entryArg.TrainingsData[DBEntry.PushUpAmountIdentifier];

                toUpdate.TrainingsData[DBEntry.SitUpAmountIdentifier] +=
                    entryArg.TrainingsData[DBEntry.SitUpAmountIdentifier];

                primaryResult = _database.Update(toUpdate.ConvertToDBEntryToSave());
            }
            return primaryResult;
        }


        public void ExportTrainingsData(string path)
        {
            throw new NotImplementedException();
        }
    }
}