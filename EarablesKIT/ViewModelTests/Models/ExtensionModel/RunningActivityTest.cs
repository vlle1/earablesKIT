using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.RunningActivity;
using EarablesKIT.Models.Extentionmodel.Activities.StepActivity;
using EarablesKIT.Models.Library;
using Xunit;
namespace ViewModelTests.Models.ExtensionModel
{
    class RunningActivityTest
    {

        public void TestWithEmptyData()
        {
            
            //hier müsste man den Service-Manager mocken!! Und dann testen, ob am ende des vorgangs das richtige da steht (also laufen oder stehen).
            RunningActivityThreshold toTest = new RunningActivityThreshold();
            int lineNr = 0;
            int changeDetectedCount = 0;
            toTest.ActivityDone +=
                (object sender, ActivityArgs a) =>
                {
                    changeDetectedCount++;
                };
            //for read all the input from csv file

            string line;
            System.IO.StreamReader file = new System.IO.StreamReader("../../../../ViewModelTests/Models/ExtensionModel/randomTestMockData.csv");

            //ignore first line
            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                Debug.WriteLine(line);
                //parse data
                float gyroX, gyroY, gyroZ, accX, accY, accZ; int freq = 0;
                string[] values = line.Split(',');
                freq = int.Parse(values[0]);
                accX = float.Parse(values[1]);
                accY = float.Parse(values[2]);
                accZ = float.Parse(values[3]);
                gyroX = float.Parse(values[4]);
                gyroY = float.Parse(values[5]);
                gyroZ = float.Parse(values[6]);

                //parse data
                ConfigContainer c = new ConfigContainer();
                c.Samplerate = freq;
                DataEventArgs data = new DataEventArgs(new IMUDataEntry(new Accelerometer(accX, accY, accZ, 0, 0, 0), new Gyroscope(gyroX, gyroY, gyroZ)), c);
                lineNr++;
                toTest.DataUpdate(data);
            }


            Assert.Equal(0, changeDetectedCount);
        }
        
    }
}
