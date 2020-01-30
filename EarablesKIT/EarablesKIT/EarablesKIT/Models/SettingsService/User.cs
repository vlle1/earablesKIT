using EarablesKIT.Resources;
using System;
using System.Text.RegularExpressions;

namespace EarablesKIT.Models.SettingsService
{
    /// <summary>
    /// Class containing the User information step length and username. Used in <see cref="SettingsService"/>
    /// </summary>
    public class User
    {
        private const string USER_PATTERN = @"^username=\w+,steplength=\d+$";

        private string _username = AppResources.SettingsServiceDefaultUser;

        /// <summary>
        /// Username of the User. Can only contain chars from type \w (word)
        /// Throws an <exception cref="ArgumentException">ArgumentException: Given username is not in the correct format</exception>
        /// </summary>
        public string Username
        {
            get => _username;
            private set => _username = Regex.Match(value, @"\w+").Success ? value : _username;
        }

        private int _steplength = 70;

        /// <summary>
        /// Step length property which saves the step length of the user in cm. If given step length is smaller than 0, 0 is saved.
        /// </summary>
        public int Steplength
        {
            get => _steplength;

            private set => _steplength = value > 0 ? value : 0;
        }

        /// <summary>
        /// Constructor for class User
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <param name="steplength">the steplength of the user</param>
        public User(string username, int steplength)
        {
            Username = username;
            Steplength = steplength;
        }

        /// <summary>
        /// Converts the User instance in a string format (username=____,steplength=____)
        /// </summary>
        /// <returns>The User as a string</returns>
        override
        public string ToString()
        {
            return "username=" + Username + ",steplength=" + Steplength;
        }

        /// <summary>
        /// Method ParseUser parses a User from the given string.
        /// String gets checked by the Regex: ^username=\w+,steplength=\d+$
        /// </summary>
        /// <param name="User">The string which contains a user instance</param>
        /// <returns>Returns a User instance, based on the given string.
        /// or 'null' if the string is not parse-able.</returns>
        public static User ParseUser(string User)
        {
            Match match = Regex.Match(User, USER_PATTERN);
            if (!match.Success)
            {
                return null;
            }

            string[] properties = User.Split(',');
            string username = properties[0].Substring(properties[0].IndexOf('=') + 1);
            int steplength = int.Parse(properties[1].Substring(properties[1].IndexOf('=') + 1));

            return new User(username, steplength);
        }
    }
}