using EarablesKIT.Models.SettingsService;
using Xunit;

namespace ViewModelTests.Models.SettingsService
{
    public class UserTest
    {


        [Fact]
        public void ToStringUserTest()
        {
            User expecteduser = new User("Bob", 70);
            string ExpectedUserString = "username=Bob,steplength=70";

            Assert.Equal(ExpectedUserString, expecteduser.ToString());
        }


        [Fact]
        public void ParseUserTest()
        {
            User expecteduser;
            expecteduser = new User("Bob", 70);

            User actual = User.ParseUser("username=Bob,steplength=70");
            Assert.Equal(expecteduser.Username, actual.Username);
            Assert.Equal(expecteduser.Steplength, actual.Steplength);
        }


        [Theory]
        [InlineData("Bob", "Bob", 70, 70)]
        [InlineData("Alice123", "Alice123", -100, 0)]
        public void SetPropertiesTest(string username, string expectedUsername, int steplength, int expectedSteplength)
        {

            User toTest = new User(username, steplength);

            Assert.Equal(expectedUsername, toTest.Username);
            Assert.Equal(expectedSteplength, toTest.Steplength);
        }

        [Theory]
        [InlineData("usernameBob,steplength=70")]
        [InlineData("username=Bob,Steplength=72220")]
        [InlineData("username=Bob,Steplength=70")]
        [InlineData("username=Bob,stePlength=70")]
        [InlineData("username=Bob;Steplength=70")]
        [InlineData("username=Bo+§b,steplength=72220")]
        [InlineData("username=Bob,steplength=12,steplength=70")]
        public void ParseUserTest_FalseInputs(string failingString)
        {
            User actualUser = User.ParseUser(failingString);
            Assert.Null(actualUser);
        }
    }
}
