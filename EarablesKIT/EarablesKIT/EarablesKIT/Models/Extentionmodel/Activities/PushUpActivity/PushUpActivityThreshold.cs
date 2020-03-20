using EarablesKIT.Models.Library;
using System;

namespace EarablesKIT.Models.Extentionmodel.Activities.PushUpActivity
{
    /// <summary>
    /// This Algorithm only uses four states. The absolute acceleration is measured and certain threshold have to be passed with several cooldowns in between to detect a pushup.
    /// </summary>
    public class PushUpActivityThreshold : AbstractPushUpActivity
    {
        //the number of used states
        private const int STATE_COUNT = 4;
        //in the following the conditions for a state increment are specified (from state index to state index + 1):
        //the cooldown after the last state-change has to be over and the absolute Acceleration has to be higher/equal or lower as some threshold
        //true iff a threshold has to be underrun instead of exceeded.
        private readonly bool[] LOWER = { true, false, false, true };
        //the threshold that needs to be passed
        private readonly double[] ABS_ACC_THRESHOLD = { 0.8, 1.15, 1.15, 0.8 };
        //the duration that no state change can be performed after this state change in seconds 
        private readonly double[] COOLDOWN_DURATION = { 0, 0.3, 0.3, 0 };

        //_state represents a state machine with four states:
        //0 represents starting position, 
        //1 represents going down,
        //2 represents being down,
        //3 represents going up again.
        private int _state;
        //the current value of the cooldown (while cooldown is active state cannot change)
        private double _cooldown;

        ///<inheritdoc/>
        protected override void Analyse(DataEventArgs data)
        {

            if (_cooldown <= 0)
            {
                Accelerometer newAccValue = data.Data.Acc;

                double absAcc = Math.Sqrt(Math.Pow(newAccValue.G_Z, 2) + Math.Pow(newAccValue.G_Y, 2) + Math.Pow(newAccValue.G_X, 2));

                //check if condition is fulfilled
                if (LOWER[_state] == (absAcc < ABS_ACC_THRESHOLD[_state]))
                {
                    _cooldown = COOLDOWN_DURATION[_state];
                    _state++;
                    //check if all states have been passed and activity therefore is detected
                    if (_state % STATE_COUNT == 0)
                    {
                        _state = 0;
                        ActivityDone.Invoke(this, new PushUpEventArgs());
                    }
                }
            }
            if (_cooldown > 0)
            {
                _cooldown -= (1.0 / _frequency);
            }
        }

        ///<inheritdoc/>
        protected override void Activate()
        {
            base.Activate();
            _cooldown = 0;
            _state = 0;
        }
    }
}

