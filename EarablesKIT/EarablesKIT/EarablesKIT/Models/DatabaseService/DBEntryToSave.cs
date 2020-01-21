using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.DatabaseService
{
    /// <summary>
    /// Primitive class which will get saved in the Database (<see cref="DatabaseConnection"/>)
    /// Simple representation of class <see cref="DBEntry"/>
    /// </summary>
    public class DBEntryToSave
    {

        /// <summary>
        /// DateTime which is the date of the trainingsentry. It is the primarykey in the database
        /// </summary>
        [PrimaryKey]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Primitive type of the trainingsdata Dictionary in class <see cref="DBEntry"/>
        /// </summary>
        public string TrainingsDataAsString { get; set; }

        /// <summary>
        /// Empty Constructor for creating instances
        /// </summary>
        public DBEntryToSave()
        {
        }
    }
}
