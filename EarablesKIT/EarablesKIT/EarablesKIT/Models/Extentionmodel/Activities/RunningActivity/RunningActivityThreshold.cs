using EarablesKIT.Models.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Extentionmodel.Activities.RunningActivity
{
    /// <inheritdoc/>
    class RunningActivityThreshold : AbstractRunningActivity
    {
        private const float THRESHOLD_MOVING = 0.01F;
        /// <summary>
        /// This Activity calculates the difference of the most recent accelerometer values to determine if the user is moving
        /// </summary>
        protected override void Analyse()
        {
            Queue<DataEventArgs>.Enumerator enumerator = buffer.GetEnumerator();
            //user is running. now count check if user was completely inactive during last values
            //this implementation is only for testing purposes and doesn't make much sense...

            if (_runningState)
            {
                //change, if there is any movement during the last x values
                for (int i = 1; i<ANALYZE_TRIGGER_RATE; i++)
                {
                    if (enumerator.Current.Data.Acc.G_X >= THRESHOLD_MOVING)
                    {
                        changeDetected();
                        return;
                    }
                    if (!enumerator.MoveNext()) return ; //end of collection
                }
            }
            else
            {
                //change, if there is no movement during the last x values
                for (int i=1; i<ANALYZE_TRIGGER_RATE*2;i++ )
                {
                    if (enumerator.Current.Data.Acc.G_X >= THRESHOLD_MOVING)
                    {
                        return;
                    }
                    if (!enumerator.MoveNext()) break;
                }
                changeDetected();
            }

        }

        public RunningActivityThreshold()
        {
            //ActivityDone (EventHandler) muss nicht instanziiert werden, auch wenn das im Entwurf steht
        }

        override protected void Activate()
        {
            //apply all the resetting of Activity class
            base.Activate();
            _runningState = false;
        }
    }
}
