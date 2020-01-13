using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace EarablesKIT.Models.SettingsService
{
    /// <summary>
    /// Class containing the User information step length and username. Used in <see cref="SettingsService"/>
    /// </summary>
    public class User
    {

        private const string USER_PATTERN = @"^username=\w+,steplength=\d+$";

        private string _username;

        /// <summary>
        /// Username of the User. Can only contain chars from type \w (word)
        /// Throws an <exception cref="ArgumentException">ArgumentException: Given username is not in the correct format</exception>
        /// </summary>
        public string Username
        {
            get => _username;
            set
            {
                if (Regex.Match(value, @"\w+").Success)
                {
                    _username = value;
                }
                else
                {
                    throw new ArgumentException("Given username is not in the correct format");
                }
            }
        }

        private int _steplength;
        /// <summary>
        /// Step length property which saves the step length of the user in cm. If given step length is smaller than 0, 0 is saved.
        /// </summary>
        public int Steplength { get => _steplength; set => _steplength = value < 0 ? 0 : value ; }

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
            return "username="+Username+",steplength="+Steplength;
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
             if (match.Success)
             {
                 return null;
             }

             string[] properties = User.Split(',');
             string username = properties[0].Substring(properties[0].IndexOf('='));
             int steplength = int.Parse(properties[1].Substring(properties[1].IndexOf('=')));

             return new User(username, steplength);


        }
    }
}
