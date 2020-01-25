using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
	public class ListenAndPerformViewModel : BaseModeViewModel, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public ICommand AddActivityCommand;

		public ICommand RemoveActivityCommand;

		public ICommand EditActivityCommand;

		private string _minutes, _seconds, _milliseconds;

		private Stopwatch _timer;
		private ActivityWrapper _pushUpActivity { get; set; }
		private ActivityWrapper _sitUpActivity { get; set; }
		private ActivityWrapper _pause { get; set; }

		private IActivityManager _activityManager { get; set; }
		private IDataBaseConnection _dataBaseConnection { get; set; }
		public ActivityWrapper ActiveActivity { get; set; }
		public ActivityWrapper SelectedActivity { get; set; }
		public ObservableCollection<ActivityWrapper> ActivityList { get; set; }
		public ObservableCollection<int> ActivityAmounts { get; set; }

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

		public ListenAndPerformViewModel()
		{
			_pushUpActivity = new ActivityWrapper("Push-ups");
			_sitUpActivity = new ActivityWrapper("Sit-ups");
			_pause = new ActivityWrapper("Pause");
			AddActivityCommand = new Command(() => AddActivity());
			RemoveActivityCommand = new Command(() => RemoveActivity());
			EditActivityCommand = new Command(() => EditActivity());
			//_activityManager = (IActivityManager)ServiceManager.ServiceProvider.GetService(typeof(IActivityManager));
			//_pushUpActivity._activity = (AbstractPushUpActivity)_activityManager.ActitvityProvider.GetService(typeof(AbstractPushUpActivity));
			//_sitUpActivity._activity = (AbstractSitUpActivity)_activityManager.ActitvityProvider.GetService(typeof(AbstractSitUpActivity));
			//_dataBaseConnection = (IDataBaseConnection)_activityManager.ActitvityProvider.GetService(typeof(IDataBaseConnection));
			ActivityList = new ObservableCollection<ActivityWrapper>
			{
				_pushUpActivity,
				_pause,
				_sitUpActivity

			};
			ActivityAmounts = new ObservableCollection<int>{
				10,
				10,
				10
			};
		}

		protected void OnPropertyChanged([CallerMemberName] string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		public override void OnActivityDone(object sender, ActivityArgs args)
		{
			ActiveActivity.Counter++;
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

		public override bool StartActivity()
		{
			if (CheckConnection())
			{
				return true;
			}

			return false;
		}

		public override void StopActivity()
		{
			StopTimer();
		}

		public void DoActivities()
		{
			for (int i = 0; i < ActivityList.Count; i++)
			{
				ActiveActivity = ActivityList.ElementAt(i);
				ActiveActivity.Counter = 0;
				ActiveActivity._activity.ActivityDone += OnActivityDone;
				while (ActiveActivity.Counter < ActivityAmounts.ElementAt(i))
				{

				}
				ActivityList.ElementAt(i)._activity.ActivityDone -= OnActivityDone;

			}
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

		private void StopTimer()
		{
			_timer.Reset();
			if (ActiveActivity._activity.ActivityDone != null)
			{
				ActiveActivity._activity.ActivityDone -= OnActivityDone;
			}
			SaveData();
			ShowPopUp();
		}

		private void SaveData()
		{
			//DateTime _dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			//DBEntry _entryNew = new DBEntry(_dt, 0, _pushUpActivity.ResultCounter, _sitUpActivity.ResultCounter);
			//_dataBaseConnection.SaveDBEntry(_entryNew);
		}

		private void ShowPopUp()
		{
			App.Current.MainPage.DisplayAlert("Result", "You have done " + _pushUpActivity.ResultCounter + " " 
				+ _pushUpActivity._name + " and " + _sitUpActivity.ResultCounter + " " + _sitUpActivity._name + "!", "Cool");
		}

		private async void AddActivity()
		{
			string result = await App.Current.MainPage.DisplayActionSheet("Select an Activity:", "Cancel", null, "Push-ups", "Sit-ups", "Pause");
			if (result.Equals("Push-ups"))
			{
				ActivityList.Add(_pushUpActivity);
			}
			if (result.Equals("Sit-ups"))
			{
				ActivityList.Add(_sitUpActivity);
			}
			if (result.Equals("Pause"))
			{
				ActivityList.Add(_pause);
			}
			string res = await App.Current.MainPage.DisplayPromptAsync("Adding Activity", 
				"Enter the amount of repetitions", "OK", "Cancel", "10", 3, Keyboard.Numeric);
			int amount = int.Parse(res);
			ActivityAmounts.Add(amount);
		}

		private void RemoveActivity()
		{
			int Index = ActivityList.IndexOf(SelectedActivity);
			ActivityList.Remove(SelectedActivity);
			ActivityAmounts.RemoveAt(Index);
		}

		private void EditActivity()
		{
			int Index = ActivityList.IndexOf(SelectedActivity);
			ActivityList.Remove(SelectedActivity);
			ActivityAmounts.RemoveAt(Index);
			AddActivity();
			ActivityList.Move(ActivityList.Count - 1, Index);
			ActivityAmounts.Move(ActivityAmounts.Count - 1, Index);

		}
	}
}
