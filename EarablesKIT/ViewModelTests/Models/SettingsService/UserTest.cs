using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using EarablesKIT.Models.SettingsService;

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


        //TODO
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
    }
}
