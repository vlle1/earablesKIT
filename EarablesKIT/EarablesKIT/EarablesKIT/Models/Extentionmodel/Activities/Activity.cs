using EarablesKIT.Models.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Extentionmodel.Activities
{
    public abstract class Activity
    {
        public EventHandler<ActivityArgs> ActivityDone { get; set; }

        private Queue<DataEventArgs> buffer;


        public void DataUpdate(DataEventArgs Data)
        {
             throw new NotImplementedException();
        }

        protected abstract void Analyse();

    }
}
