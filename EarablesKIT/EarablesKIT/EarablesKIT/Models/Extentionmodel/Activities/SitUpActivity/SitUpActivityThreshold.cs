using EarablesKIT.Models.Extentionmodel.Activities.PushUpActivity;
using EarablesKIT.Models.Library;

namespace EarablesKIT.Models.Extentionmodel.Activities.SitUpActivity
{
    ///<inheritdoc/>
    public class SitUpActivityThreshold : AbstractSitUpActivity
    {
        //the number of states of the implemented state machine
        private const int STATE_COUNT = 4;
        //in the following the conditions for a state increment are specified (from state index to state index + 1):
        //the cooldown after the last state-change has to be over and some value of the IMU Data has to be higher/equal or lower as some threshold
        //true iff a threshold has to be underrun instead of exceeded.
        private readonly bool[] LOWER = { true, false, false, true };
        //represents the value of the IMU Data that is used for the comparison.
        //0,1,2 ar Accelerometer X,Y,Z (in G) and 3,4,5 are Gyroscope X,Y,Z
        private readonly int[] VALUE_INDEX = { 1, 5, 1, 5 };

        //the threshold that needs to be passed
        private readonly double[] THRESHOLD = { -1.3, 100, 0, -100 };

        //_state represents a state machine with four states:
        //0 represents starting position, 
        //1 represents going up,
        //2 represents being upright
        //3 represents going down,
        private int _state;

        public SitUpActivityThreshold()
        {

        }

        ///<inheritdoc/>
        override protected void Activate()
        {
            _state = 0;
            base.Activate();
        }


        ///<inheritdoc/>
        override protected void Analyse(DataEventArgs data)
        {
            //to avoid using reflections or other stuff to dynamically get right value from sensor data, copy data to an array structure.
            Accelerometer newAccValue = data.Data.Acc;
            double[] dataAsArray = {
                    data.Data.Acc.G_X,
                    data.Data.Acc.G_Y,
                    data.Data.Acc.G_Z,
                    data.Data.Gyro.DegsPerSec_X,
                    data.Data.Gyro.DegsPerSec_Y,
                    data.Data.Gyro.DegsPerSec_Z
                };
            //check if condition of current state is fulfilled
            if (LOWER[_state] == (dataAsArray[VALUE_INDEX[_state]] < THRESHOLD[_state]))
            {
                //progress to next state
                _state++;
                //check if all states have been passed and activity therefore is detected
                if (_state % STATE_COUNT == 0)
                {
                    _state = 0;
                    ActivityDone.Invoke(this, new PushUpEventArgs());
                }
            }
        }
    }
}
