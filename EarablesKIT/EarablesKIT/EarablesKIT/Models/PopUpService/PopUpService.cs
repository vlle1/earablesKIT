using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EarablesKIT.Models.PopUpService
{
	class PopUpService : IPopUpService
	{
		public Task DisplayAlert(string title, string message, string cancel)
		{
			return Application.Current.MainPage.DisplayAlert(title, message, cancel);
		}
		public Task<string> ActionSheet(string title, string cancel, string destruction, string choice1, string choice2, string choice3)
		{
			return Application.Current.MainPage.DisplayActionSheet(title, cancel, destruction, choice1, choice2, choice3);
		}

		public Task<string> DisplayPrompt(string title, string message, string ok, string cancel, string placeholder, int length, Keyboard keyboard)
		{
			return Application.Current.MainPage.DisplayPromptAsync(title, message, ok, cancel, placeholder, length, keyboard);
		}
	}
}
