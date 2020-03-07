using System;
using System.Collections.Generic;
using Xunit;
using EarablesKIT.Models.DatabaseService;
using Newtonsoft.Json;

namespace ViewModelTests.Models.DatabaseService
{
    public class DBEntryTest
    {
        [Fact]
        public void ToStringTest()
        {
            DateTime time = new DateTime(2000, 4,27);
            int stepAmount = 100;
            int pushUpAmount = 50;
            int sitUpAmount = 10;

            DBEntry dbEntry = new DBEntry(time, stepAmount, pushUpAmount, sitUpAmount);


            string expected = "27.04.2000,Steps=100,PushUps=50,SitUps=10";
            Assert.Equal(expected, dbEntry.ToString());
        }

        [Fact]
        public void ParseDBEntryTest()
        {
            string toParse = "27.04.2000,Steps=100,PushUps=50,SitUps=10";
            
            DBEntry expected = new DBEntry(new DateTime(2000,4,27), 100,50,10 );

            DBEntry actual = DBEntry.ParseDbEntry(toParse);

            Assert.NotNull(actual);
            Assert.Equal(expected.Date, actual.Date);
            foreach (KeyValuePair<string, int> keyValuePair in expected.TrainingsData)
            {
                Assert.True(actual.TrainingsData.ContainsKey(keyValuePair.Key));
                Assert.Equal(keyValuePair.Value,actual.TrainingsData[keyValuePair.Key]);
            }
        }

        [Theory]
        [InlineData("27.04.2000,Steps=100,PushUps=12")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("27.04.2000,steps=100,PushUps=12,SitUps=10")]
        [InlineData("20,steps=100,PushUps=12,SitUps=10")]
        [InlineData("27.04.2000,Steps=100,PushUps=12,SitUps=10,Burpees=120")]
        [InlineData("27.04.2000,Steps=10a0,PushUps=50,SitUps=10")]
        [InlineData("27.04.2000,Steps=100,PushUps=5s0,SitUps=10")]
        [InlineData("27.04.2000,Steps=100,PushUps=50,SitUps=1x0")]

        public void ParseDBEntryTestNullChecks(string toParse)
        {
            DBEntry actual = DBEntry.ParseDbEntry(toParse);
            Assert.Null(actual);
        }

        [Fact]
        public void ConvertToDBEntryToSave()
        {
            DBEntry entry = new DBEntry(DateTime.Parse("27.04.2000"), 100, 50, 20);

            DBEntryToSave expected = new DBEntryToSave();
            Dictionary<string, int> trainingsdata = new Dictionary<string, int>();
            expected.DateTime = DateTime.Parse("27.04.2000");
            trainingsdata.Add("Steps", 100);
            trainingsdata.Add("PushUps", 50);
            trainingsdata.Add("SitUps" ,20);
            expected.TrainingsDataAsString = JsonConvert.SerializeObject(trainingsdata);

            DBEntryToSave actual = entry.ConvertToDBEntryToSave();

            Assert.Equal(expected.DateTime, actual.DateTime);
            Assert.Equal(expected.TrainingsDataAsString, actual.TrainingsDataAsString);
        }

    }
}
