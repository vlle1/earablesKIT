using EarablesKIT.Models.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Extentionmodel.Activities
{
    /// <summary>
    /// An activity provides an algorithm that throws certain events if a certain activity is detected.
    /// </summary>
    abstract class Activity
    {
        /// <summary>
        /// the frequency of the incoming data (how many values per second)
        /// </summary>
        protected int _frequency = 50;
        
        /// <summary>
        /// _isActive is set to false when the algorithm is not keeping the queue up to date. Like this,
        /// the Queue and other things can be reset when the algorithm is starting to work again.
        /// </summary>
        private bool _isActive = false;
        /// <summary>
       /// This EventHandler handles every ViewModel that wants to get notified when the activity is detected.
       /// </summary>
        public EventHandler<ActivityArgs> ActivityDone { get; set; }
        
        /// <summary>
        /// This method is used to process incoming data.
        /// By default, it pufferes the last 100 elements of dataEventArgs each time.
        /// By default, it triggers Analyse every 20th incoming element
        /// </summary>
        public void DataUpdate(DataEventArgs data)
        {
            if (ActivityDone == null)
            {
                _isActive = false;
            }
            if (ActivityDone != null)
            {
                if (!_isActive) Activate();
                _frequency = data.Configs.Samplerate;
                Analyse(data);
            }

        }

        protected abstract void Analyse(DataEventArgs data);
        /// <summary>
        /// Whenever the Algorithm is started, it needs to reset all its old values.
        /// </summary>
        protected virtual void Activate()
        {
            _isActive = true;
        }
    }
}
