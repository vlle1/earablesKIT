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
        public void Test10Situps()
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
            System.IO.StreamReader file = new System.IO.StreamReader("../../../../ViewModelTests/Models/ExtensionModel/testData10Situps.csv");

            //ignore first line
            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                Debug.WriteLine(line);
                //parse data
                float gyroX, gyroY, gyroZ, accX, accY, accZ; int freq = 0;
                string[] values = line.Split(',');
                freq = int.Parse(values[0]);
                accX = float.Parse(values[1], CultureInfo.InvariantCulture.NumberFormat);
                accY = float.Parse(values[2], CultureInfo.InvariantCulture.NumberFormat);
                accZ = float.Parse(values[3], CultureInfo.InvariantCulture.NumberFormat);
                gyroX = float.Parse(values[4], CultureInfo.InvariantCulture.NumberFormat);
                gyroY = float.Parse(values[5], CultureInfo.InvariantCulture.NumberFormat);
                gyroZ = float.Parse(values[6], CultureInfo.InvariantCulture.NumberFormat);

                //parse data
                ConfigContainer c = new ConfigContainer {Samplerate = freq};
                DataEventArgs data = new DataEventArgs(new IMUDataEntry(new Accelerometer(accX, accY, accZ, 0, 0, 0), new Gyroscope(gyroX, gyroY, gyroZ)), c);
                lineNr++;
                toTest.DataUpdate(data);
            }

            int expected = 10;
            Assert.InRange(count, expected * (1 - ALLOWED_RELATIVE_ERROR), expected * (1 + ALLOWED_RELATIVE_ERROR));
        }

    }
}