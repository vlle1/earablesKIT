using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EarablesKIT.Models.PopUpService
{
	interface IPopUpService
	{
		Task DisplayAlert(string title, string message, string cancel);

		Task<string> ActionSheet(string title, string cancel, string destruction, string choice1, string choice2, string choice3);

		Task<string> DisplayPrompt(string title, string message, string ok, string cancel, string placeholder, int length, Keyboard keyboard);
	}
}
