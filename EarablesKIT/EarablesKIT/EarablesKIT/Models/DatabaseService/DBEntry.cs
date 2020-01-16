using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace EarablesKIT.Models.DatabaseService
{
    public class DBEntry
    {

        public const string StepAmountIdentifier = "Steps";
        public const string PushUpAmountIdentifier = "PushUps";
        public const string SitUpAmountIdentifier = "SitUps";


        [PrimaryKey]
        public DateTime Date { get; set; }


        [TextBlob(nameof(TrainingsDataAsString))]
        public Dictionary<string, int> TrainingsData { get; set; }

        public string TrainingsDataAsString { get; set; }
        public DBEntry()
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