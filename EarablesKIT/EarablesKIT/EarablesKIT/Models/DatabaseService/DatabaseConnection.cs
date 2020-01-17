using SQLite;
using System;
using System.Collections.Generic;
using System.IO;

namespace EarablesKIT.Models.DatabaseService
{
    public  class DatabaseConnection : IDataBaseConnection
    {

        private readonly SQLiteConnection _database;

        private readonly string _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "EarablesKIT_TrainingsData.db3");

        
        public DatabaseConnection()
        {
            _database = new SQLiteConnection(_path);
            _database.CreateTable<DBEntryToSave>();
        }



        /// <inheritdoc />
        public void DeleteAllEntries()
        {
            _database.DropTable<DBEntryToSave>();
            _database.CreateTable<DBEntryToSave>();
        }


        /// <inheritdoc />
        public List<DBEntry> GetAllEntries()
        {
            List<DBEntry> dBEntries = new List<DBEntry>();
            List<DBEntryToSave> dbEntriesString = _database.Table<DBEntryToSave>().ToList();
            foreach (var entry in dbEntriesString)
            {
                dBEntries.Add(DBEntry.ParseDbEntry(entry));
            }
            return dBEntries;
        }


        /// <inheritdoc />
        public List<DBEntry> GetMostRecentEntries(int amount)
        {
            if (amount <= 0)
            {
                return new List<DBEntry>();
            }

            List<DBEntry> dbEntries = this.GetAllEntries();
            if (amount >= dbEntries.Count)
            {
                return dbEntries;
            }

            return dbEntries.GetRange(dbEntries.Count-amount, amount);
        }

        /// <inheritdoc />
        public int SaveDBEntry(DBEntry entryArg)
        {
            int primaryResult;
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