using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace EarablesKIT.Models.SettingsService
{
    public class User
    {

        private const string USER_PATTERN = @"^username=\w+,steplength=\d+$";

        private string _username;

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
                    throw new ArgumentException();
                }
            }
        }

        private int _steplength;
        public int Steplength { get => _steplength; set => _steplength = value < 0 ? 0 : value ; }

        public User(string username, int steplength)
        {
            Username = username;
            Steplength = steplength;
        }


        override
        public string ToString()
        {
            return "username="+Username+",steplength="+Steplength;
        }

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
