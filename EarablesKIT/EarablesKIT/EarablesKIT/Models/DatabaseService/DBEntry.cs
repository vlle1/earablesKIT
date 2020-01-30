using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EarablesKIT.Models.DatabaseService
{
    /// <summary>
    /// 
    /// </summary>
    public class DBEntry
    {

        public const string StepAmountIdentifier = "Steps";
        public const string PushUpAmountIdentifier = "PushUps";
        public const string SitUpAmountIdentifier = "SitUps";


        /// <summary>
        /// Date of the Entry
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Dictionary containing the trainingsdata (steps, pushups, situps).
        /// The keys are the constants StepAmountIdentifier, PushUpAmountIdentifier,
        /// SitUpAmountIdentifier
        /// </summary>
        public Dictionary<string, int> TrainingsData { get; private set; }


        private DBEntry()
        {
            TrainingsData = new Dictionary<string, int>();
        }

        /// <summary>
        /// Constructor for DBEntry 
        /// </summary>
        /// <param name="date">The date of the entry</param>
        /// <param name="stepAmount">The amount of steps on this Date</param>
        /// <param name="pushUpAmount">The amount of pushUps done on this Date</param>
        /// <param name="sitUpAmount">THe amount of sitUps done on this Date</param>
        public DBEntry(DateTime date, int stepAmount, int pushUpAmount, int sitUpAmount)
        {
            TrainingsData = new Dictionary<string, int>
            {
                {StepAmountIdentifier, stepAmount},
                {PushUpAmountIdentifier, pushUpAmount},
                {SitUpAmountIdentifier, sitUpAmount}
            };
            Date = date;
        }

        public override string ToString()
        {
            string result = Date.ToString("dd.MM.yyyy")+ ",";
            foreach (var keyValuePair in TrainingsData)
            {
                result += keyValuePair.Key + "=" + keyValuePair.Value + ",";
            }

            result = result.Substring(0, result.Length - 1);
            return result;
        }

        /// <summary>
        /// Converts the current object to an instance of DBEntryToSave which
        /// is getting saved in the database.
        /// </summary>
        /// <returns>This object as a DBEntryToSave instance</returns>
        public DBEntryToSave ConvertToDBEntryToSave()
        {
            DBEntryToSave entryToSave = new DBEntryToSave
            {
                DateTime = this.Date, 
                TrainingsDataAsString = JsonConvert.SerializeObject(this.TrainingsData)
            };
            return entryToSave;
        }


        /// <summary>
        /// Parses the given instance of type DBEntryToSave to an instance to DBEntry.
        /// </summary>
        /// <param name="entry">The given entry, which needs to get parsed</param>
        /// <returns>A </returns>
        public static DBEntry ParseDbEntry(DBEntryToSave entry)
        {
            Dictionary<string, int> trainingsData = JsonConvert.DeserializeObject<Dictionary<string,int>>(entry.TrainingsDataAsString);
            DBEntry result = new DBEntry
            {
                Date = entry.DateTime, 
                TrainingsData = trainingsData
            };
            return result;
        }


        /// <summary>
        /// Parses a string to a DBEntry
        /// </summary>
        /// <param name="entry">The entry as a string, which should get parsed</param>
        /// <returns>The parsed instance of DBEntry</returns>
        public static DBEntry ParseDbEntry(string entry)
        {
            if (entry == null)
                return null;

            var parts = entry.Split(',');

            if (parts.Length != 4)
                return null;

            if (!DateTime.TryParse(parts[0], out DateTime date))
                return null;

            if (!parts[1].StartsWith(StepAmountIdentifier + "=") || !parts[2].StartsWith(PushUpAmountIdentifier + "=") || !parts[3].StartsWith(SitUpAmountIdentifier + "="))
                return null;

            if (!int.TryParse(parts[1].Substring(parts[1].IndexOf("=") + 1), out var stepAmount))
                return null;

            if (!int.TryParse(parts[2].Substring(parts[2].IndexOf("=") + 1), out var pushUpAmount))
                return null;
            
            if (!int.TryParse(parts[3].Substring(parts[3].IndexOf("=") + 1), out var sitUpAmount))
                return null;

            return new DBEntry(date, stepAmount, pushUpAmount, sitUpAmount);
        }
    }
}