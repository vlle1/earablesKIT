using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.DatabaseService
{
    public class DBEntryToSave
    {
        [PrimaryKey]
        public DateTime DateTime { get; set; }

        public string TrainingsDataAsString { get; set; }

        public DBEntryToSave()
        {
        }
    }
}
