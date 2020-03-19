using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.SitUpActivity;
using EarablesKIT.Models.Library;
using Xunit;


namespace ViewModelTests.Models.ExtensionModel
{
    public class SitUpsActivityTest
    {

        private const double ALLOWED_RELATIVE_ERROR = 0.1; //TODO new test data (only five situps!)
        [Fact]
        public void TestWithEmptyData()
        {
            SitUpActivityThreshold toTest = new SitUpActivityThreshold();
            int lineNr = 0;
            int count = 0;
            toTest.ActivityDone +=
                (object sender, ActivityArgs a) =>
                {
                    count++;
                };
            //for read all the input from csv file

            string line;
            System.IO.StreamReader file = new System.IO.StreamReader("../../../../ViewModelTests/Models/ExtensionModel/SitupsTestData30.csv");

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


            Assert.InRange(count, 5 * (1 - ALLOWED_RELATIVE_ERROR), 5 * (1 + ALLOWED_RELATIVE_ERROR));
        }

    }
}
