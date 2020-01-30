using EarablesKIT.Models;
using EarablesKIT.Models.DatabaseService;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.PushUpActivity;
using EarablesKIT.Models.Extentionmodel.Activities.SitUpActivity;
using EarablesKIT.Models.Library;
using EarablesKIT.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
	public class ListenAndPerformViewModel : BaseModeViewModel, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public ICommand AddActivityCommand { get; set; }

		public ICommand RemoveActivityCommand { get; set; }

		public ICommand EditActivityCommand { get; set; }

		private Stopwatch _timer;
		private Timer PauseTimer;
		private int _pushUpResult, _sitUpResult, Repetitions;
		private double _progress;

		private ActivityWrapper _activeActivity;

		private IEnumerator<ActivityWrapper> ActivityIterator;
		private AbstractPushUpActivity _pushUpActivity { get; set; }
		private AbstractSitUpActivity _sitUpActivity { get; set; }
		private IActivityManager _activityManager { get; set; }
		private IDataBaseConnection _dataBaseConnection { get; set; }
		public ActivityWrapper ActiveActivity
		{
			get { return _activeActivity; }
			set
			{
				_activeActivity = value;
				OnPropertyChanged();
			}
		}
		public ActivityWrapper SelectedActivity { get; set; }
		public ObservableCollection<ActivityWrapper> ActivityList { get; set; }

		private string _minutes, _seconds, _milliseconds;
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

		public double ProgressLive
		{
			get { return _progress; }
			set
			{
				_progress = value;
				OnPropertyChanged();
			}
		}

		public ListenAndPerformViewModel()
		{
			AddActivityCommand = new Command(() => AddActivity(ActivityList.Count));
			RemoveActivityCommand = new Command(() => RemoveActivity());
			EditActivityCommand = new Command(() => EditActivity());
			_activityManager = (IActivityManager)ServiceManager.ServiceProvider.GetService(typeof(IActivityManager));
			_pushUpActivity = (AbstractPushUpActivity)_activityManager.ActitvityProvider.GetService(typeof(AbstractPushUpActivity));
			_sitUpActivity = (AbstractSitUpActivity)_activityManager.ActitvityProvider.GetService(typeof(AbstractSitUpActivity));
			_dataBaseConnection = (IDataBaseConnection)ServiceManager.ServiceProvider.GetService(typeof(IDataBaseConnection));

			ActivityList = new ObservableCollection<ActivityWrapper>
			{
				new ActivityWrapper(AppResources.Push_ups, _pushUpActivity, 10),
				new ActivityWrapper(AppResources.Pause, null, 10),
				new ActivityWrapper(AppResources.Sit_ups, _sitUpActivity, 10)
			};
		}

		protected void OnPropertyChanged([CallerMemberName] string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		public override void OnActivityDone(object sender, ActivityArgs args)
		{
			ActiveActivity.Counter++;
			ProgressLive = Math.Round((double)ActiveActivity.Counter / Repetitions, 2);
			if (ActiveActivity.Counter >= Repetitions)
			{
				ActiveActivity._activity.ActivityDone -= OnActivityDone;
				IncreaseResultCounter();
				if (ActivityIterator.MoveNext())
				{
					CheckNextActivity();
				}
				else
				{
					_timer.Stop();

				}
			}
		}

		private void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			ActiveActivity.Counter--;
			ProgressLive = 1 - Math.Round((double)ActiveActivity.Counter / Repetitions, 2);
			if (ActiveActivity.Counter == 0)
			{
				PauseTimer.Stop();
				PauseTimer.Elapsed -= OnTimedEvent;
				if (ActivityIterator.MoveNext())
				{
					CheckNextActivity();
				}
				else
				{
					_timer.Stop();

				}
			}
		}

		private void CheckNextActivity()
		{
			ActiveActivity = ActivityIterator.Current;
			Repetitions = ActiveActivity.Amount;
			_ = SpeakActivity(Repetitions);
			if (ActiveActivity._activity != null)
			{
				ActiveActivity.Counter = 0;
				ActiveActivity._activity.ActivityDone += OnActivityDone;
				ProgressLive = 0;
			}
			else
			{
				ActiveActivity.Counter = Repetitions;
				ProgressLive = 0;
				PauseTimer = new Timer();
				PauseTimer.Interval = 1000;
				PauseTimer.Elapsed += OnTimedEvent;
				PauseTimer.AutoReset = true;
				PauseTimer.Enabled = true;
			}

		}

		public async Task SpeakActivity(int Amount)
		{
			if (!ActiveActivity.Name.Equals(AppResources.Pause))
			{
				await TextToSpeech.SpeakAsync(AppResources.NextActivity + Amount + "" + ActiveActivity.Name);
			}
			else
			{
				await TextToSpeech.SpeakAsync(Amount + AppResources.Seconds + ActiveActivity.Name);
			}
		}

		public override bool StartActivity()
		{
			if (CheckConnection() && ActivityList.Count > 0)
			{
				_pushUpResult = 0;
				_sitUpResult = 0;
				PauseTimer = new Timer();
				ActivityIterator = ActivityList.GetEnumerator();
				ActivityIterator.MoveNext();
				((IEarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection))).StartSampling();
				CheckNextActivity();
				return true;
			}
			return false;
		}

		public override void StopActivity()
		{
			StopTimer();
			if (ActiveActivity._activity != null && ActiveActivity._activity.ActivityDone != null)
			{
				ActiveActivity._activity.ActivityDone -= OnActivityDone;
			}
			SaveData();
			ShowPopUp();
		}

		public void IncreaseResultCounter()
		{
			if (ActiveActivity.Name.Equals(AppResources.Push_ups))
			{
				_pushUpResult += ActiveActivity.Counter;
			}
			if (ActiveActivity.Name.Equals(AppResources.Sit_ups))
			{
				_sitUpResult += ActiveActivity.Counter;
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
		}

		private void SaveData()
		{
			DateTime _dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			DBEntry _entryNew = new DBEntry(_dt, 0, _pushUpResult, _sitUpResult);
			_dataBaseConnection.SaveDBEntry(_entryNew);
		}

		private void ShowPopUp()
		{
			Application.Current.MainPage.DisplayAlert(AppResources.Result, AppResources.YouHaveDone + _pushUpResult + " "
				+ AppResources.Push_ups + " " +  AppResources.And + " " + _sitUpResult + " " + AppResources.Sit_ups + " " 
				+ AppResources.Done + "!", AppResources.Cool);
		}

		private async void AddActivity(int Index)
		{
			string newActivity = await Application.Current.MainPage.DisplayActionSheet(AppResources.SelectAnActivity, 
				AppResources.Cancel, null, AppResources.Push_ups, AppResources.Sit_ups, AppResources.Pause);
			if (newActivity != null && !newActivity.Equals("") && !newActivity.Equals(AppResources.Cancel))
			{
				string newAmount = await Application.Current.MainPage.DisplayPromptAsync(AppResources.AddingActivity, //Exception für Negatives vllt
						AppResources.EnterRepetitions, AppResources.Okay, AppResources.Cancel, "10", 2, Keyboard.Numeric);
				if (newAmount != null && !newAmount.Equals("") && int.Parse(newAmount) > 0) //TO-DO: Regex für Z-ahlinput
				{
					if (newActivity.Equals(AppResources.Push_ups))
					{
						ActivityList.Insert(Index, new ActivityWrapper(AppResources.Push_ups, _pushUpActivity, int.Parse(newAmount)));
					}
					if (newActivity.Equals(AppResources.Sit_ups))
					{
						ActivityList.Insert(Index, new ActivityWrapper(AppResources.Sit_ups, _sitUpActivity, int.Parse(newAmount)));
					}
					if (newActivity.Equals(AppResources.Pause))
					{
						ActivityList.Insert(Index, new ActivityWrapper(AppResources.Pause, null, int.Parse(newAmount)));
					}
				}
			}
		}

		private void RemoveActivity()
		{
			if (ActivityList.Count > 0 && ActivityList.Contains(SelectedActivity))
			{
				ActivityList.Remove(SelectedActivity);
			}
		}

		private void EditActivity()
		{
			if (SelectedActivity != null && ActivityList.Contains(SelectedActivity))
			{
				int Index = ActivityList.IndexOf(SelectedActivity);
				ActivityList.Remove(SelectedActivity);
				AddActivity(Index);
			}

		}
	}
}
