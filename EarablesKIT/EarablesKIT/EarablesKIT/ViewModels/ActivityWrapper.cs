using System.ComponentModel;
using System.Runtime.CompilerServices;
using EarablesKIT.Models.Extentionmodel.Activities;

namespace EarablesKIT.ViewModels
{
	/// <summary>
	/// Class that Wraps an activity from the ActivityProvider with its name, 
	/// the amount of repetitions and a counter.
	/// </summary>
	public class ActivityWrapper : INotifyPropertyChanged
	{
		/// <summary>
		/// Property which holds an activity from the ActivityManager.
		/// </summary>
		public Activity _activity { get; set; }
		/// <summary>
		/// Property which holds the name of the Activity.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Property which holds the amount of Repetitions of an activity.
		/// </summary>
		public int Amount { get; set; }

		/// <summary>
		/// Property which holds the current amount of repetitions done. Bound to the View Classes.
		/// </summary>
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

		/// <summary>
		/// Constructor used in ListenAndPerform.
		/// </summary>
		/// <param name="name">Activity name</param>
		/// <param name="activity">Activity send from ActivityProvider</param>
		/// <param name="amount">Amount of repetitions</param>
		public ActivityWrapper(string name, Activity activity, int amount)
		{
			Counter = 0;
			Name = name;
			_activity = activity;
			Amount = amount;
		}
		/// <summary>
		/// Constructor used in CountMode, amount of repetitions to be done is not needed.
		/// </summary>
		/// <param name="name">Activity name</param>
		/// <param name="activity">Activity send from ActivityProvider</param>
		public ActivityWrapper(string name, Activity activity)
		{
			Counter = 0;
			Name = name;
			_activity = activity;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}



	}
}
