using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.StepActivity;
using EarablesKIT.Models.Library;
using System.Diagnostics;
using System.Globalization;
using Xunit;

namespace ViewModelTests.Models.ExtensionModel
{
    public class StepActivityTest
    {
        private const double ALLOWED_RELATIVE_ERROR = 0.1;
        [Fact]
        public void Test30Steps()
        {
            StepActivityThreshold toTest = new StepActivityThreshold();
            int lineNr = 0;
            int count = 0;
            toTest.ActivityDone +=
                (object sender, ActivityArgs a) =>
                {
                    count++;
                };
            //for read all the input from csv file

            string line;
            System.IO.StreamReader file = new System.IO.StreamReader("../../../../ViewModelTests/Models/ExtensionModel/testData30steps.csv");

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
                ConfigContainer c = new ConfigContainer();
                c.Samplerate = freq;
                DataEventArgs data = new DataEventArgs(new IMUDataEntry(new Accelerometer(accX, accY, accZ, 0, 0, 0), new Gyroscope(gyroX, gyroY, gyroZ)), c);
                lineNr++;
                toTest.DataUpdate(data);
            }

            //ok, if in 10 percent range
            Assert.True(count > 30 * (1 - ALLOWED_RELATIVE_ERROR), "too less steps recognized!");
            Assert.True(count < 30 * (1 + ALLOWED_RELATIVE_ERROR), "too many steps recognized!");
        }

        [Fact]
        public void Test50Steps()
        {
            StepActivityThreshold toTest = new StepActivityThreshold();
            int lineNr = 0;
            int count = 0;
            toTest.ActivityDone +=
                (object sender, ActivityArgs a) =>
                {
                    count++;
                };
            //for read all the input from csv file

            string line;
            System.IO.StreamReader file = new System.IO.StreamReader("../../../../ViewModelTests/Models/ExtensionModel/testData50steps.csv");

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
                ConfigContainer c = new ConfigContainer();
                c.Samplerate = freq;
                DataEventArgs data = new DataEventArgs(new IMUDataEntry(new Accelerometer(accX, accY, accZ, 0, 0, 0), new Gyroscope(gyroX, gyroY, gyroZ)), c);
                lineNr++;
                toTest.DataUpdate(data);
            }

            //ok, if in 10 percent range
            Assert.InRange(count, 50 * (1 - ALLOWED_RELATIVE_ERROR), 50 * (1 + ALLOWED_RELATIVE_ERROR));
        }

        //test what happens when nothing is registered at an activity (it shouldn't analyze and throw events!!)
        [Fact]
        public void Test50StepsWithoutListener()
        {
            StepActivityThreshold toTest = new StepActivityThreshold();
            int lineNr = 0;
            int count = 0;

            //read all the input from csv file 

            string line;
            System.IO.StreamReader file = new System.IO.StreamReader("../../../../ViewModelTests/Models/ExtensionModel/testData50steps.csv");

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
                ConfigContainer c = new ConfigContainer();
                c.Samplerate = freq;
                DataEventArgs data = new DataEventArgs(new IMUDataEntry(new Accelerometer(accX, accY, accZ, 0, 0, 0), new Gyroscope(gyroX, gyroY, gyroZ)), c);
                lineNr++;
                toTest.DataUpdate(data);
                //verifying that no more code is executed can be done via debugging.
            }

            Assert.Equal(count, 0);
        }

        [Fact]
        public void Test0Steps()
        {
            StepActivityThreshold toTest = new StepActivityThreshold();
            int lineNr = 0;
            int count = 0;

            //read all the input from csv file 

            string line;
            System.IO.StreamReader file = new System.IO.StreamReader("../../../../ViewModelTests/Models/ExtensionModel/testData0steps.csv");

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
                ConfigContainer c = new ConfigContainer();
                c.Samplerate = freq;
                DataEventArgs data = new DataEventArgs(new IMUDataEntry(new Accelerometer(accX, accY, accZ, 0, 0, 0), new Gyroscope(gyroX, gyroY, gyroZ)), c);
                lineNr++;
                toTest.DataUpdate(data);
                //verifying that no more code is executed can be done via debugging.
            }

            Assert.Equal(count, 0);
        }
    }
}