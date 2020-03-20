using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.RunningActivity;
using EarablesKIT.Models.Extentionmodel.Activities.StepActivity;
using EarablesKIT.Models.Library;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace ViewModelTests.Models.ExtensionModel
{
    public class RunningActivityTest
    {
        [Fact]
        public void TestProcessingStep()
        {
            
            RunningActivityThreshold toTest = new RunningActivityThreshold();
            
            bool detectedStatus = false;
            int changeDetectedCount = 0;
            toTest.ActivityDone +=
                (object sender, ActivityArgs a) =>
                {
                    detectedStatus = ((RunningEventArgs) a).Running;
                    changeDetectedCount++;
                };
            
            //simulate detected step from step algorithm
            toTest.OnStepRecognized(null, null);


            Assert.Equal(1, changeDetectedCount);
            Assert.True(detectedStatus);
        }

        [Fact]
        public void TestProcessingTimeout()
        {
            //here we will simulate one step and then a lot of data that does not contain a step.
            //after that, there should have been two events and the current status should be standing.
            
            /*
            //first we have to mock StepActivity (it needs no functionality, but needs to be instantiated)

            //Für den ServiceProviderMock
            //Muss enthalten sein, damit der Mock nicht überschrieben wird
            IServiceProvider unused = ServiceManager.ServiceProvider;

            //Feld Infos holen
            System.Reflection.FieldInfo instance = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            //Mocksaufsetzen 
            //ServiceProvider
            Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();
            Mock<IActivityManager> activityManagerMock = new Mock<IActivityManager>();
            Mock<IServiceProvider> activityProviderMock = new Mock<IServiceProvider>();
            Mock<AbstractStepActivity> stepActivityMock = new Mock<AbstractStepActivity>();
            Mock<AbstractRunningActivity> runningActivityMock = new Mock<AbstractRunningActivity>();
            Mock<IPopUpService> popUpMock = new Mock<IPopUpService>();

            //ActivityManager
            activityManagerMock.Setup(x => x.ActitvityProvider).Returns(activityProviderMock.Object);
            activityProviderMock.Setup(x => x.GetService(typeof(AbstractRunningActivity))).Returns(runningActivityMock.Object);
            activityProviderMock.Setup(x => x.GetService(typeof(AbstractStepActivity))).Returns(stepActivityMock.Object);

            */
            RunningActivityThreshold toTest = new RunningActivityThreshold();

            bool detectedStatus = false;
            int changeDetectedCount = 0;
            toTest.ActivityDone +=
                (object sender, ActivityArgs a) =>
                {
                    detectedStatus = ((RunningEventArgs)a).Running;
                    changeDetectedCount++;
                };

            //simulate detected step from step algorithm
            toTest.OnStepRecognized(null, null);


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
                toTest.DataUpdate(data);
                //verifying that no more code is executed can be done via debugging.
            }


            Assert.Equal(2, changeDetectedCount);
            Assert.False(detectedStatus);


        }

    }
}
