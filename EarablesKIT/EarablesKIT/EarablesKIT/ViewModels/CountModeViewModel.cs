using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
	public class CountModeViewModel : BaseModeViewModel, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private Stopwatch _timer;
		private ActivityWrapper _pushUpActivity { get; set; }
		private ActivityWrapper _sitUpActivity { get; set; }
		private ActivityWrapper _comingSoon { get; set; }
		private IActivityManager _activityManager { get; set; }
		private IDataBaseConnection _dataBaseConnection { get; set; }

		private string _minutes, _seconds, _milliseconds;

		public ObservableCollection<ActivityWrapper> PossibleActivities { get; set; }
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
		

		public ActivityWrapper SelectedActivity { get; set; }

		public CountModeViewModel()
		{
			_pushUpActivity = new ActivityWrapper("Push-ups");
			_sitUpActivity = new ActivityWrapper("Sit-ups");
			_comingSoon = new ActivityWrapper("Coming soon");
			//_activityManager = (IActivityManager)ServiceManager.ServiceProvider.GetService(typeof(IActivityManager));
			//_pushUpActivity._activity = (AbstractPushUpActivity)_activityManager.ActitvityProvider.GetService(typeof(AbstractPushUpActivity));
			//_sitUpActivity._activity = (AbstractSitUpActivity)_activityManager.ActitvityProvider.GetService(typeof(AbstractSitUpActivity));
			//_dataBaseConnection = (IDataBaseConnection)_activityManager.ActitvityProvider.GetService(typeof(IDataBaseConnection));
			PossibleActivities = new ObservableCollection<ActivityWrapper>
			{
				_pushUpActivity,
				_sitUpActivity,
				_comingSoon
			};
			SelectedActivity = _pushUpActivity;

		}

		private bool RegisterActivity()
		{
			//if (SelectedActivity._activity != null)
			//{
				//SelectedActivity._activity.ActivityDone += OnActivityDone;
				return true;
			//}
			//return false;
		}


		protected void OnPropertyChanged([CallerMemberName] string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}


		public override void OnActivityDone(object sender, ActivityArgs args)
		{
			SelectedActivity.Counter++;
		}

		public override bool StartActivity()
		{
			if (CheckConnection())
			{
				if (RegisterActivity())
				{
					_pushUpActivity.Counter = 0;
					_sitUpActivity.Counter = 0;
					return true;
				}
				return false;

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
			//SelectedActivity._activity.ActivityDone -= OnActivityDone;
			SaveData();
			ShowPopUp();
		}

		private void StopTimer()
		{
			_timer.Reset();
		}

		private void SaveData()
		{
			//DateTime _dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			//DBEntry _entryNew = new DBEntry(_dt, 0, _pushUpActivity.Counter, _sitUpActivity.Counter);
			//_dataBaseConnection.SaveDBEntry(_entryNew);
		}

		private void ShowPopUp()
		{
			App.Current.MainPage.DisplayAlert("Result", "You have done " + SelectedActivity.Counter + " " + SelectedActivity._name + "!", "Cool");
		}
	}
}
