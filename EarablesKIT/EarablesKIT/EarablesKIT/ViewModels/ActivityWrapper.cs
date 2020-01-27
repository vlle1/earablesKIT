﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using EarablesKIT.Models.Extentionmodel.Activities;

namespace EarablesKIT.ViewModels
{
	public class ActivityWrapper : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public Activity _activity { get; set; }
		public string _name { get; set; }

		private int _counter;

		public int Counter
		{
			get { return _counter; }
			set
			{
				_counter = value;
				OnPropertyChanged();
			}
		}

		public ActivityWrapper()
		{
			Counter = 0;
		}

		protected void OnPropertyChanged([CallerMemberName] string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		

	}
}
