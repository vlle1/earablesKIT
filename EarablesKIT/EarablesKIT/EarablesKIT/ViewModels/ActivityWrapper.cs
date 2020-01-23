using System;
using System.Collections.Generic;
using System.Text;
using EarablesKIT.Models.Extentionmodel.Activities;

namespace EarablesKIT.ViewModels
{
	public class ActivityWrapper
	{
		public Activity _activity { get; set; }

		public string _name { get; set; }

		public int StepCounter { get; set; }
		public int PushUpCounter { get; set; }
		public int SitUpCounter { get; set; }

		public ActivityWrapper()
		{
		}

	}
}
