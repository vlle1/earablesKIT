using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarablesKIT.Models.DatabaseService
{
    /// <summary>
    /// Interface IDataBaseConnection defines an interface which handles the database and can save/delete/update entries in it
    /// </summary>
    interface IDataBaseConnection
    {

        /// <summary>
        /// Method SaveDBEntry saves the given DBEntry in the Database. Converts it into an DBEntryToSave, which is compatible with the DB.
        /// Updates the Entry, if there is already a database entry with this date (primary key)
        /// </summary>
        /// <param name="entryArg">The DBEntry which should get saved</param>
        /// <returns>An integer, which is the index of this </returns>
        int SaveDBEntry(DBEntry entry);

        /// <summary>
        /// Method GetAllEntries returns all entries, which are stored in the database
        /// </summary>
        /// <returns>Returns a List of DBEntries which are stored in the database</returns>
        List<DBEntry> GetAllEntries();

        /// <summary>
        /// Method <c>GetMostRecentEntries</c> connects to the SQLite Database and gets the amount' recent entries.
        /// If the param amount is greater than the amount of DBEntries which are saved, all saved Entries will get returned. Careful, There are not amount' many DBEntries in this returning List!
        /// </summary>
        /// <param name="amount">The 'amount' lastest database entries</param>
        /// <returns>A List of database entries wrapped in class <see cref="DBEntry"/>
        /// If the Database doesn't have the amount DBEntries saved, returns all saved entries instead!</returns>
        List<DBEntry> GetMostRecentEntries(int amount);

        void ImportTrainingsData(FileData file);

        void ExportTrainingsData(string path);

        /// <summary>
        /// Method DeleteAllEntries deletes all stored entries in the database. Simply drops the proper table.
        /// </summary>
        void DeleteAllEntries();



    }
}
