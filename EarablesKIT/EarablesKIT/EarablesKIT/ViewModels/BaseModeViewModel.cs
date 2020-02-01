using EarablesKIT.Models.Extentionmodel.Activities;

namespace EarablesKIT.ViewModels
{

	/// <summary>
	/// Class that provides general (abstract) methods for every ModeViewModel.
	/// </summary>
	public abstract class BaseModeViewModel
	{
		/// <summary>
		/// Method which is called when events are thrown by the analyse algorithms.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="args">Includes additional Data</param>
		public abstract void OnActivityDone(object sender, ActivityArgs args);

		/// <summary>
		/// Method for Starting an activity, will be bound to StartButtons when implemented in subclasses.
		/// </summary>
		/// <returns>Bool if the Starting of the activity was successfull</returns>
		public abstract bool StartActivity();

		/// <summary>
		/// Method for Stopping an activity, will be bound to StopButtons when implemented in subclasses.
		/// </summary>
		public abstract void StopActivity();

		/// <summary>
		/// Methods that is called when Starting an activity. Checks the connection status 
		/// with the Earables and invokes the ScanningPopUp when there is no connection present.
		/// </summary>
		/// <returns>Bool if a connection was present</returns>
		protected bool CheckConnection()
		{
			if (ScanningPopUpViewModel.IsConnected)
			{
				return true;
			}
			else
			{
				ScanningPopUpViewModel.ShowPopUp();
				return false;
			}
		}

	}
}
