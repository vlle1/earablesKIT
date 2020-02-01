using Xunit;

namespace ViewModelTests.Models.SettingsService

{
    public class SettingsServiceTest
    {

        //SettingsService got tested manually        

        [Fact]
        public void TestCreateSettingsService()
        {
            /* Manueller Test: Einfügen im App Konstruktor
            MainPage.DisplayAlert("Properties:", "App.Properties: " + JsonConvert.SerializeObject(App.Current.Properties), "Accept", "Cancel");

            SettingsService.ActiveLanguage = CultureInfo.GetCultureInfo("en-US");
            SettingsService.ActiveUser = new User("Bob", 85);

            MainPage.DisplayAlert("SettingsPropertyChange!",
                "Language: " + SettingsService.ActiveLanguage + "\nLanguageCurrentUICulture: " +
                System.Globalization.CultureInfo.CurrentUICulture + "\nUser: " + SettingsService.ActiveUser + "\n",
                "Accept", "Cancel");*/

        }


    }
}
