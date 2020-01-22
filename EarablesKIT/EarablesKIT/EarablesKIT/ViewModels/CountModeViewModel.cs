using EarablesKIT.Models;
using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.PushUpActivity;
using EarablesKIT.Models.Extentionmodel.Activities.SitUpActivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
	class CountModeViewModel : BaseModeViewModel, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private Stopwatch _timer;
		private AbstractPushUpActivity _pushUpActivity { get; set; }
		private AbstractSitUpActivity _sitUpActivity { get; set; }
		private IActivityManager _activityManager { get; set; }
		private IDataBaseConnection _dataBaseConnection { get; set; }

		private string _selectedActivity, _minutes, _seconds, _milliseconds;
		public string SelectedActivity { get; set; }
		public string Minutes
		{
			get { return _minutes; }
			set
			{
				_minutes = value;
				OnPropertyChanged();
			}
		}
		public string Seconds
		{
			get { return _seconds; }
			set
			{
				_seconds = value;
				OnPropertyChanged();
			}
		}
		public string Milliseconds
		{
			get { return _milliseconds; }
			set
			{
				_milliseconds = value;
				OnPropertyChanged();
			}
		}
		public int Counter { get; set; }
		public ObservableCollection<string> PossibleActivities { get; set; }

		public CountModeViewModel()
		{
			//_activityManager = (IActivityManager)ServiceManager.ServiceProvider.GetService(typeof(IActivityManager));
			//_pushUpActivity = (AbstractPushUpActivity)_activityManager.ActitvityProvider.GetService(typeof(AbstractPushUpActivity));
			//_sitUpActivity = (AbstractSitUpActivity)_activityManager.ActitvityProvider.GetService(typeof(AbstractSitUpActivity));
			//_dataBaseConnection = (IDataBaseConnection)_activityManager.ActitvityProvider.GetService(typeof(IDataBaseConnection));
			_timer = new Stopwatch();
			PossibleActivities = new ObservableCollection<string>();
			Minutes = "00";
			Seconds = "00";
			Milliseconds = "000";


		}

		protected void OnPropertyChanged([CallerMemberName] string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}


		public override void OnActivityDone(object sender, ActivityArgs args)
		{
			Counter++;
		}

		public override bool StartActivity()
		{
			if (CheckConnection())
			{
				
				return true;
			}
			return false;
		}

		public void StartTimer()
		{
			Minutes = "00"; Seconds = "00"; Milliseconds = "000";
			_timer = new Stopwatch();
			_timer.Start();
			Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
			{
				if (_timer.Elapsed.Minutes.ToString().Length == 1)
				{
					Minutes = "0" + _timer.Elapsed.Minutes.ToString();
				}
				else
				{
					Minutes = _timer.Elapsed.Minutes.ToString();
				}

				if (_timer.Elapsed.Seconds.ToString().Length == 1)
				{
					Seconds = "0" + _timer.Elapsed.Seconds.ToString();
				}
				else
				{
					Seconds = _timer.Elapsed.Seconds.ToString();
				}

				Milliseconds = _timer.Elapsed.Milliseconds.ToString();
				return true;
			});
		}

		public override void StopActivity()
		{
			StopTimer();
		}

		public void StopTimer()
		{
			_timer.Reset();
			Milliseconds = _timer.Elapsed.Milliseconds.ToString();
			Minutes = _timer.Elapsed.Minutes.ToString();
			Seconds = _timer.Elapsed.Seconds.ToString();
		}
	}
}
