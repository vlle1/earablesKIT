using EarablesKIT.Models.DatabaseService;
using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using Xunit;

namespace ViewModelTests.Models.DatabaseService
{
    public class DatabaseConnectionTest
    {
        private DatabaseConnection _toTest;

        private void SetUp()
        {
            _toTest = new DatabaseConnection();
        }

        [Fact]
        public void TestDeleteAllEntries()
        {
            SetUp();
            _toTest.DeleteAllEntries();
            List<DBEntry> allEntries = _toTest.GetAllEntries();
            Assert.Empty(allEntries);

            DBEntry toSave = new DBEntry(DateTime.Parse("27.04.2000"), 100, 50, 20);
            _toTest.SaveDBEntry(toSave);
            allEntries = _toTest.GetAllEntries();
            Assert.Equal(1, allEntries.Count);

            _toTest.DeleteAllEntries();
            allEntries = _toTest.GetAllEntries();
            Assert.Empty(allEntries);
        }

        [Fact]
        public void TestGetAllEntries()
        {
            SetUp();

            DBEntry toSave1 = new DBEntry(DateTime.Parse("27.04.2000"), 100, 50, 20);
            DBEntry toSave2 = new DBEntry(DateTime.Parse("13.09.2000"), 500, 50, 0);
            DBEntry toSave3 = new DBEntry(DateTime.Parse("29.11.2000"), 120, 40, 10);
            
            _toTest.DeleteAllEntries();
            List<DBEntry> allEntries = _toTest.GetAllEntries();
            Assert.Empty(allEntries);

            _toTest.SaveDBEntry(toSave1);
            _toTest.SaveDBEntry(toSave2);
            _toTest.SaveDBEntry(toSave3);

            allEntries = _toTest.GetAllEntries();
            Assert.Equal(3, allEntries.Count);


            bool containingOne = false;
            bool containingTwo = false;
            bool containingThree = false;

            foreach (DBEntry dbEntry in allEntries)
            {
                containingOne = containingOne || (dbEntry.ToString().Equals(toSave1.ToString()));
                containingTwo = containingTwo || (dbEntry.ToString().Equals(toSave2.ToString()));
                containingThree = containingThree || (dbEntry.ToString().Equals(toSave3.ToString()));
            }
            Assert.True(containingOne);
            Assert.True(containingTwo);
            Assert.True(containingThree);

        }

        [Fact]
        public void TestGetMostRecentEntries()
        {
            SetUp();

            DBEntry toSave1 = new DBEntry(DateTime.Parse("27.04.2000"), 100, 50, 20);
            DBEntry toSave2 = new DBEntry(DateTime.Parse("13.09.2000"), 500, 50, 0);
            DBEntry toSave3 = new DBEntry(DateTime.Parse("29.11.2000"), 120, 40, 10);
            DBEntry toSave4 = new DBEntry(DateTime.Parse("02.05.2000"), 1230, 30, 211);
            DBEntry toSave5 = new DBEntry(DateTime.Parse("27.03.2000"), 10, 20, 50);

            _toTest.DeleteAllEntries();
            List<DBEntry> allEntries = _toTest.GetAllEntries();
            
            Assert.Empty(allEntries);

            _toTest.SaveDBEntry(toSave1);
            _toTest.SaveDBEntry(toSave2);
            _toTest.SaveDBEntry(toSave3);
            _toTest.SaveDBEntry(toSave4);
            _toTest.SaveDBEntry(toSave5);

            allEntries = _toTest.GetAllEntries();
            Assert.True(allEntries.Count == 5);

            List<DBEntry> mostRecentEntries = _toTest.GetMostRecentEntries(4);
            Assert.True(mostRecentEntries.Count == 4);

            bool contains = false;
            foreach (DBEntry dbEntry in mostRecentEntries)
            {
                contains = dbEntry.Date == toSave2.Date || dbEntry.Date ==
                    toSave3.Date || dbEntry.Date == toSave4.Date || dbEntry.Date == toSave5.Date;
                contains = contains && !(dbEntry.Date == toSave1.Date);
                Assert.True(contains);
                contains = false;
            }


            mostRecentEntries = _toTest.GetMostRecentEntries(-2);
            Assert.Empty(mostRecentEntries);
        }

        [Fact]
        public void TestAddEntry()
        {
            SetUp();
            _toTest.DeleteAllEntries();
            DBEntry toSave = new DBEntry(DateTime.Parse("27.04.2000"), 100, 50, 20);
            _toTest.SaveDBEntry(toSave);
            List<DBEntry> allEntries = _toTest.GetAllEntries();
            bool containing = false;
            DBEntry actual = null;
            foreach (DBEntry dbEntry in allEntries)
            {
                if (dbEntry.Date.Equals(toSave.Date))
                {
                    containing = !containing;
                    actual = dbEntry;
                }
            }
            Assert.True(containing);
            if (actual == null)
            {
                Assert.True(false);
            }

            containing = false;
            _toTest.SaveDBEntry(toSave);
            allEntries = _toTest.GetAllEntries();
            DBEntry actualDBEntry2 = null;
            foreach (DBEntry dbEntry in allEntries)
            {
                if (!dbEntry.Date.Equals(toSave.Date)) continue;

                containing = !containing;
                actualDBEntry2 = dbEntry;
            }
            Assert.True(containing);
            if (actualDBEntry2 == null) Assert.True(false);

            Assert.True(actual.Date.Equals(actualDBEntry2.Date));
            Assert.Equal(actual.TrainingsData[DBEntry.StepAmountIdentifier] + 100, actualDBEntry2.TrainingsData[DBEntry.StepAmountIdentifier]);
            Assert.Equal(actual.TrainingsData[DBEntry.PushUpAmountIdentifier] + 50, actualDBEntry2.TrainingsData[DBEntry.PushUpAmountIdentifier]);
            Assert.Equal(actual.TrainingsData[DBEntry.SitUpAmountIdentifier] + 20, actualDBEntry2.TrainingsData[DBEntry.SitUpAmountIdentifier]);
        }

        [Fact]
        public void TestExport()
        {
            DatabaseConnection dbToTest = new DatabaseConnection();
            dbToTest.SaveDBEntry(new DBEntry(DateTime.Parse("26.04.2000"), 100, 20, 10));
            dbToTest.SaveDBEntry(new DBEntry(DateTime.Parse("25.04.2000"), 10, 50, 15));
            dbToTest.SaveDBEntry(new DBEntry(DateTime.Parse("22.04.2000"), 170, 10, 230));
            dbToTest.SaveDBEntry(new DBEntry(DateTime.Parse("27.04.2000"), 130, 20, 10));
            


        }
    }
}