using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using EarablesKIT.Models.SettingsService;

namespace ViewModelTests.Models.SettingsService
{
    public class UserTest
    {
        [Fact]
        public void ParseUserTest()
        {
            User expctedUser = new User("Bob", 70);
            var UserString = "username=Bob,steplength=70";

            User actual = User.ParseUser(UserString);
            Assert.Equal(expctedUser.Username, actual.Username);
            Assert.Equal(expctedUser.Steplength,expctedUser.Steplength);
        }

        [Fact]
        public void ToStringUserTest()
        {
            User expecteduser = new User("Bob", 70);
            string ExpectedUserString = "username=Bob,steplength=70";

            Assert.Equal(ExpectedUserString, expecteduser.ToString());
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
    }
}
