using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarablesKIT.Models.DatabaseService
{
    interface IDataBaseConnection
    {

        void SaveDBEntry(DBEntry entry);

        Task<List<DBEntry>> GetAllEntriesAsync();

        Task<List<DBEntry>> GetMostRecentEntriesAsync(int amount);

        //void ImportTrainingsData(FileData file);

        void ExportTrainingsData(string path);

        void DeleteAllEntries();

    }
}
