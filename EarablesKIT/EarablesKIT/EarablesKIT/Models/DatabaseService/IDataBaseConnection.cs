using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarablesKIT.Models.DatabaseService
{
    interface IDataBaseConnection
    {

        int SaveDBEntry(DBEntry entry);

        List<DBEntry> GetAllEntriesAsync();

        List<DBEntry> GetMostRecentEntriesAsync(int amount);

        //void ImportTrainingsData(FileData file);

        void ExportTrainingsData(string path);

        void DeleteAllEntries();

    }
}
