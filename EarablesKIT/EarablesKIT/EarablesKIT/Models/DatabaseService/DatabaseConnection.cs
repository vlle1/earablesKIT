using EarablesKIT.Resources;
using EarablesKIT.ViewModels;
using Plugin.FilePicker.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;

namespace EarablesKIT.Models.DatabaseService
{
    /// <summary>
    /// Class DatabaseConnection is the Connectionservice which handels the connection to the
    /// database. (Stores, updates, removes entries of primitive type <see cref="DBEntryToSave"/>
    /// and its Wrapper class <see cref="DBEntry"/>
    /// </summary>
    public class DatabaseConnection : IDataBaseConnection
    {
        private readonly SQLiteConnection _database;

        private readonly string _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "EarablesKIT_TrainingsData.db3");

        /// <summary>
        /// Constructor of the DatabaseConnection. Initializes the Database and creates the needed table
        /// </summary>
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

            return dbEntries.GetRange(dbEntries.Count - amount, amount);
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

        /// <inheritdoc />
        public void ImportTrainingsData(FileData file)
        {
            if (file == null || string.IsNullOrEmpty(file.FileName))
            {
                ExceptionHandlingViewModel.HandleException(new FileNotFoundException(AppResources.DataBaseFileDoesntExistError));
            }

            string content = System.Text.Encoding.Default.GetString(file.DataArray);
            string[] lines = content.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );

            List<DBEntry> parsedEntries = new List<DBEntry>();
            foreach (string entry in lines)
            {
                DBEntry dbEntry = DBEntry.ParseDbEntry(entry);
                if (dbEntry == null)
                {
                    ExceptionHandlingViewModel.HandleException(new ArgumentException(AppResources.DatabaseConnectionFileParseError));
                    return;
                }
                else
                {
                    parsedEntries.Add(dbEntry);
                }
            }
            foreach (DBEntry entry in parsedEntries)
            {
                this.SaveDBEntry(entry);
            }
        }

        /// <inheritdoc />
        public string ExportTrainingsData()
        {
            /*string fileName = "/storage/emulated/0/Android/data/count.txt";
            bool debugBool = string.IsNullOrEmpty(path.FilePath);
            Debug.WriteLine(fileName);
            Debug.WriteLine(path.FilePath);
            //debugBool = !File.Exists(fileToWrite);
            if (debugBool)
            {
                ExceptionHandlingViewModel.HandleException(new FileNotFoundException(AppResources.DataBaseFileDoesntExistError));
                return;
            }
            */
            List<DBEntry> entries = GetAllEntries();
            string toWrite = "";
            foreach(DBEntry entry in entries)
            {
                toWrite += entry.ToString() + "\n";
            }
            if (entries.Count == 0) return "";
            toWrite = toWrite.Remove(toWrite.Length - 1);

            return toWrite;
        }
    }
}