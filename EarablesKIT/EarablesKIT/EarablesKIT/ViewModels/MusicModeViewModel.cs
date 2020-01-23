using EarablesKIT.Models;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.RunningActivity;
using EarablesKIT.Models.SettingsService;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    class MusicModeViewModel : BaseModeViewModel
    {
        private bool _isActive;
        private bool _isRunning;
        private AbstractRunningActivity runningActivity;
        public Command TriggerActivity
        {
            get
            {
                return new Command(() =>
                { 
                    if (_isActive) StopActivity();
                    else StartActivity(); 
                });
            }
        }
        public MusicModeViewModel()
        {
            //runningActivity = (AbstractRunningActivity) 
            //    ((IActivityManager)ServiceManager.ServiceProvider.GetService(typeof(IActivityManager)))
            //    .ActitvityProvider.GetService(typeof(AbstractRunningActivity));
            _isActive = false;
            _isRunning = false;
        }

        public override void OnActivityDone(object sender, ActivityArgs args)
        {
            //toggle music
        }
        /// <summary>
        /// Toggles Activity of musicMode
        /// </summary>
        /// <returns></returns>
        public override bool StartActivity()
        {
            //register at eh
            return true;
        }

        public override void StopActivity()
        {
            //unregister at eh, stop music once
            
        }
    }
}
