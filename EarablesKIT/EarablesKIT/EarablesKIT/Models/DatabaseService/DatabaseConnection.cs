using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarablesKIT.Models.DatabaseService
{
    class DatabaseConnection : IDataBaseConnection
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

        public Task<List<DBEntry>> GetMostRecentEntriesAsync(int amount)
        {
            throw new NotImplementedException();
        }

        public void SaveDBEntry(DBEntry entry)
        {
            throw new NotImplementedException();
        }


        public DatabaseConnection()
        {
             throw new NotImplementedException();
        }
    }
}
