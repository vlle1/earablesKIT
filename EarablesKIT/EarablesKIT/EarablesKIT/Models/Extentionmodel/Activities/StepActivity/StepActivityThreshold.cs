using EarablesKIT.Models.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Extentionmodel.Activities.StepActivity
{
    class StepActivityThreshold : AbstractStepActivity
    {
        //the weight of the old average acceleration value when calculating the new one (weight of single new value is always 1)
        private const int REF_WEIGHT = 100;
        //the threshold to the current acceleration relative to average acceleration to trigger step recognition
        private const double TRIGGER_THRESHOLD = 1.15;
        //the cosinus of the angle that the current acceleration direction is maximally allowed 
        //to differ from the average acceleration direction (about 27 degree)
        private const double ANGLE_TOLERANCE_COS = 0.89;
        //the remaining time in seconds that no step should be detected (e.g. after detected step)
        private double cooldown = 0;

        //average values for the acceleration. Initialisation estimates default (upright) position of sensor
        private double _avgAccAbsolute = 1;
        
        
        private double _avgAccX = -1;
        private double _avgAccY = 0;
        private double _avgAccZ = 0;

        public StepActivityThreshold()
        {
            
        }
        protected override void Analyse(DataEventArgs data)
        {
            IMUDataEntry _newValue = data.Data;
            //the accelerometer is the only relevant thing
            Accelerometer accV = _newValue.Acc;
            double accVAbs = Math.Sqrt(Math.Pow(accV.G_X, 2) + Math.Pow(accV.G_Y, 2) + Math.Pow(accV.G_Z, 2));
            //first update average values
            _avgAccAbsolute = (accVAbs + REF_WEIGHT * _avgAccAbsolute) / (REF_WEIGHT + 1);
            _avgAccX = (accV.G_X + REF_WEIGHT * _avgAccX) / (REF_WEIGHT + 1);
            _avgAccY = (accV.G_Y + REF_WEIGHT * _avgAccY) / (REF_WEIGHT + 1);
            _avgAccZ = (accV.G_Z + REF_WEIGHT * _avgAccZ) / (REF_WEIGHT + 1);

            if (accVAbs > TRIGGER_THRESHOLD * _avgAccAbsolute)
            {
                //threshold is passed
                if (cooldown <= 0)
                {
                    //set cooldown, no matter what motion has been registered (maybe change this to only when step recognized, but first check in debug branch)
                    cooldown = 0.3;
                    //check if direction of current value and average head in the same direction using simple formula for cosinus
                    if ((_avgAccX * accV.G_X + _avgAccY * accV.G_Y + _avgAccZ * accV.G_Z)
                        / accVAbs / _avgAccAbsolute > ANGLE_TOLERANCE_COS)
                    {
                        //step recognized
                        ActivityDone.Invoke(this, new StepEventArgs());

                    }

                }
                else
                {
                    //kniebeuge avoidance: after 0.2s of overload 
                    //maybe revert step (first test this feature in debug branch)
                }
            }

            if (cooldown > 0) cooldown--;
        }
        override protected void Activate()
        {
            base.Activate();

            cooldown = 0;
            //average values for the acceleration. Initialisation estimates default (upright) position of sensor
            _avgAccAbsolute = 1;
            _avgAccX = -1;
            _avgAccY = 0;
            _avgAccZ = 0;

        }
    }
}
