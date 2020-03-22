using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Moq;
using Xunit;
using EarablesKIT.Models;
using EarablesKIT.Models.DatabaseService;
using EarablesKIT.ViewModels;

namespace ViewModelTests.ViewModels.DataOverviewViewModelTest
{
    [ExcludeFromCodeCoverage]
    public class OverviewViewModelTest
    {
        [Fact]
        public void DataOverViewModelConstructorTest()
        {

            //Für den ServiceProviderMock
            //Muss enthalten sein, damit der Mock nicht überschrieben wird
            IServiceProvider unused = ServiceManager.ServiceProvider;

            //Feld Infos holen
            System.Reflection.FieldInfo instance = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            //Mocksaufsetzen 
            //ServiceProvider
            Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();
            
            //Service der gemockt werden soll
            Mock<IDataBaseConnection> mockDataBase = new Mock<IDataBaseConnection>();
            List<DBEntry> entries = new List<DBEntry>();
            DBEntry one = new DBEntry(
                new DateTime(2000, 4, 12), 100, 200, 30);
            DBEntry two = new DBEntry(
                new DateTime(2020, 3, 15), 102, 20, 30);

            entries.Add(one);
            entries.Add(two);

            //Verhalten für die Mocks festlegen (Bei Aufruf was zurückgegeben werden soll)
            mockDataBase.As<IDataBaseConnection>().Setup(x => x.GetMostRecentEntries(30)).Returns(entries);
            mockSingleton.Setup(x => x.GetService(typeof(IDataBaseConnection))).Returns(mockDataBase.Object);

            //ServiceProvider anlegen
            instance.SetValue(null, mockSingleton.Object);


            //Test ausführen
            DataOverviewViewModel dataOverview = new DataOverviewViewModel();

            //Verifizieren
            Assert.NotEmpty(dataOverview.TrainingsDataDbEntries);
            Assert.Equal(2, dataOverview.TrainingsDataDbEntries.Count);
            DBEntry firsEntry = dataOverview.TrainingsDataDbEntries[0];
            Assert.NotNull(firsEntry);

            Assert.Equal(two.ToString(), firsEntry.ToString()); 
            DBEntry secondEntry = dataOverview.TrainingsDataDbEntries[1];
            Assert.NotNull(firsEntry);

            Assert.Equal(one.ToString(), secondEntry.ToString());
        }

        [Fact]
        public void testOnAppearing()
        {
            //Muss enthalten sein, damit der Mock nicht überschrieben wird
            IServiceProvider unused = ServiceManager.ServiceProvider;

            //Feld Infos holen
            System.Reflection.FieldInfo instance = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            //Mocksaufsetzen 
            Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();
            Mock<IDataBaseConnection> mockDataBase = new Mock<IDataBaseConnection>();
            List<DBEntry> entries = new List<DBEntry>();
            DBEntry one = new DBEntry(
                new DateTime(2000, 4, 12), 100, 200, 30);
            DBEntry two = new DBEntry(
                new DateTime(2020, 3, 15), 102, 20, 30);

            entries.Add(one);
            entries.Add(two);
            mockDataBase.As<IDataBaseConnection>().SetupSequence(x => x.GetMostRecentEntries(30))
                .Returns(new List<DBEntry>())
                .Returns(entries);
            mockSingleton.Setup(x => x.GetService(typeof(IDataBaseConnection))).Returns(mockDataBase.Object);

            instance.SetValue(null, mockSingleton.Object);


            //TestObjekt erstellen
            DataOverviewViewModel dataOverview = new DataOverviewViewModel();

            Assert.Empty(dataOverview.TrainingsDataDbEntries);

            dataOverview.OnAppearing(null, null);

            Assert.NotEmpty(dataOverview.TrainingsDataDbEntries);
            Assert.Equal(2, dataOverview.TrainingsDataDbEntries.Count);
            DBEntry firsEntry = dataOverview.TrainingsDataDbEntries[0];
            Assert.NotNull(firsEntry);

            Assert.Equal(two.ToString(), firsEntry.ToString());
            DBEntry secondEntry = dataOverview.TrainingsDataDbEntries[1];
            Assert.NotNull(firsEntry);

            Assert.Equal(one.ToString(), secondEntry.ToString());

        }
    }
}
