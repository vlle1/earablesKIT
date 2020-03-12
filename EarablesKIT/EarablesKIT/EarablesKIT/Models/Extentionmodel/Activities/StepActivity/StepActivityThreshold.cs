using EarablesKIT.Models.Library;
using System;

namespace EarablesKIT.Models.Extentionmodel.Activities.StepActivity
{
    /// <summary>
    /// StepActivityThreshold estimates where the ground is and determines a Accelerometer Value a step if:
    /// - the value is higher than a certain threshold
    /// - the value is occuring a significant time after the last step (after cooldown)
    /// - the angle between current acceleration and estimated ground is small
    /// </summary>
    public class StepActivityThreshold : AbstractStepActivity
    {
        //the weight of the old average acceleration value when calculating the new one (weight of single new value is always 1),
        //relative to the sampling rate (real weight will be sampling rate * REF_WEIGHT_REL)
        private const int REF_WEIGHT_REL = 2;
        //the threshold to the current acceleration relative to average acceleration to trigger step recognition
        private const double TRIGGER_THRESHOLD = 1.15;
        //the cosinus of the angle that the current acceleration direction is maximally allowed 
        //to differ from the average acceleration direction (about 27 degree)
        private const double ANGLE_TOLERANCE_COS = 0.89;

        //the duration of the cooldown after a step in seconds
        private const double COOLDOWN_DURATION = 0.3;
        //the remaining time in seconds that no step should be detected (e.g. after detected step)
        private double cooldown = 0;

        //average values for the acceleration. Initialisation estimates default (upright) position of sensor. Every attribute corresponds to a different axis.
        private double _avgAccX = -1;
        private double _avgAccY = 0;
        private double _avgAccZ = 0;

        //this value is the length of the vector of the estimated average acceleration
        private double _avgAccAbsolute = 1;
        
        

        public StepActivityThreshold()
        {
            
        }

        ///<inheritdoc/>
        protected override void Analyse(DataEventArgs data)
        {
            
            IMUDataEntry _newValue = data.Data;
            //the accelerometer is the only relevant thing
            Accelerometer accV = _newValue.Acc;
            double accVAbs = Math.Sqrt(Math.Pow(accV.G_X, 2) + Math.Pow(accV.G_Y, 2) + Math.Pow(accV.G_Z, 2));
            //first update average values
            //therefore calculate the weight of the old value
            int ref_weight = REF_WEIGHT_REL * _frequency;
            _avgAccAbsolute = (accVAbs + ref_weight * _avgAccAbsolute) / (ref_weight + 1);
            _avgAccX = (accV.G_X + ref_weight * _avgAccX) / (ref_weight + 1);
            _avgAccY = (accV.G_Y + ref_weight * _avgAccY) / (ref_weight + 1);
            _avgAccZ = (accV.G_Z + ref_weight * _avgAccZ) / (ref_weight + 1);

            if (accVAbs > TRIGGER_THRESHOLD * _avgAccAbsolute)
            {
                //threshold is passed
                if (cooldown <= 0)
                {
                    //set cooldown, no matter what motion has been registered
                    cooldown = COOLDOWN_DURATION;
                    //check if direction of current value and average head in the same direction using simple formula for cosinus
                    if ((_avgAccX * accV.G_X + _avgAccY * accV.G_Y + _avgAccZ * accV.G_Z)
                        / accVAbs / _avgAccAbsolute > ANGLE_TOLERANCE_COS)
                    {
                        //step recognized
                        ActivityDone.Invoke(this, new StepEventArgs());

                    }

                }
            }

            if (cooldown > 0) cooldown -= 1.0 / _frequency;
        }

        ///<inheritdoc/>
        override protected void Activate()
        {
            base.Activate();

            cooldown = 0;
            //Initialisation / reset of average acceleration.
            _avgAccAbsolute = 1;
            _avgAccX = -1;
            _avgAccY = 0;
            _avgAccZ = 0;

        }
    }
}
