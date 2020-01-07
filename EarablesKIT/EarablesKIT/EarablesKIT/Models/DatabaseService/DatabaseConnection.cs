using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarablesKIT.Models.DatabaseService
{
    internal class DatabaseConnection : IDataBaseConnection
    {
        
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
            throw new NotImplementedException();
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

        public void SaveDBEntry(DBEntry entry)
        {
            throw new NotImplementedException();
        }
    }
}