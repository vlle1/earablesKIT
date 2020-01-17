using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;
using Newtonsoft.Json;

namespace EarablesKIT.Models.DatabaseService
{
    public class DBEntry
    {

        public const string StepAmountIdentifier = "Steps";
        public const string PushUpAmountIdentifier = "PushUps";
        public const string SitUpAmountIdentifier = "SitUps";


        public DateTime Date { get; private set; }

        public Dictionary<string, int> TrainingsData { get; private set; }


        private DBEntry()
        {
            TrainingsData = new Dictionary<string, int>();
        }


        public DBEntry(DateTime date, int stepAmount, int pushUpAmount, int sitUpAmount)
        {
            TrainingsData = new Dictionary<string, int>();
            TrainingsData.Add(StepAmountIdentifier, stepAmount);
            TrainingsData.Add(PushUpAmountIdentifier, pushUpAmount);
            TrainingsData.Add(SitUpAmountIdentifier, sitUpAmount);
            Date = date;
        }

        public override string ToString()
        {
            string result = Date.ToString("d")+ ",";
            foreach (var keyValuePair in TrainingsData)
            {
                result += keyValuePair.Key + "=" + keyValuePair.Value + ",";
            }

            result = result.Substring(0, result.Length - 1);
            return result;
        }

        public DBEntryToSave ConvertToDBEntryToSave()
        {
            DBEntryToSave entryToSave = new DBEntryToSave();
            entryToSave.DateTime = this.Date;
            entryToSave.TrainingsDataAsString = JsonConvert.SerializeObject(this.TrainingsData);
            return entryToSave;
        }

        public static DBEntry ParseDbEntry(DBEntryToSave entry)
        {
            Dictionary<string, int> trainingsData = JsonConvert.DeserializeObject<Dictionary<string,int>>(entry.TrainingsDataAsString);
            DBEntry result = new DBEntry();
            result.Date = entry.DateTime;
            result.TrainingsData = trainingsData;
            return result;
        }

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