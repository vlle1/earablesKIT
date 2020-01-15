using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SQLite;

namespace EarablesKIT.Models.DatabaseService
{
    internal class DatabaseConnection : IDataBaseConnection
    {

        private readonly SQLiteAsyncConnection _database;

        private readonly string _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "EarablesKIT_TrainingsData.db3");

        public DatabaseConnection()
        {
            _database = new SQLiteAsyncConnection(_path);
            _database.CreateTableAsync<DBEntry>().Wait();
        }

        public void DeleteAllEntries()
        {
            throw new NotImplementedException();
        }
        
        public void ExportTrainingsData(string path)
        {
            throw new NotImplementedException();
        }

        public Task<List<DBEntry>> GetAllEntriesAsync()
        {
            return _database.Table<DBEntry>().ToListAsync();
        }

        //TODO Example Documentation
        /// <summary>
        /// Method <c>GetMostRecentEntriesAsync</c> connectes to the SQLite Database and gets the amount' recent entries. 
        /// </summary>
        /// <param name="amount">The 'amount' lastest database entries</param>
        /// <returns>A Task countaining a List of database entries wrapped in class <see cref="DBEntry"/></returns>
        public Task<List<DBEntry>> GetMostRecentEntriesAsync(int amount)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public int SaveDBEntry(DBEntry entry)
        {
            int primaryResult = -1;
            Task<DBEntry> result = _database.Table<DBEntry>().Where(i => i.Date.Equals(entry.Date)).FirstOrDefaultAsync();

            if (result.Result == null)
            {
                primaryResult = _database.InsertAsync(entry).Result;
            }
            else
            {
                DBEntry toUpdate = result.Result;
                toUpdate.TrainingsData[DBEntry.StepAmountIdentifier] +=
                    entry.TrainingsData[DBEntry.StepAmountIdentifier];

                toUpdate.TrainingsData[DBEntry.PushUpAmountIdentifier] +=
                    entry.TrainingsData[DBEntry.PushUpAmountIdentifier];

                toUpdate.TrainingsData[DBEntry.SitUpAmountIdentifier] +=
                    entry.TrainingsData[DBEntry.SitUpAmountIdentifier];

                primaryResult = _database.UpdateAsync(toUpdate).Result;
            }
            return primaryResult;
        }
    }
}