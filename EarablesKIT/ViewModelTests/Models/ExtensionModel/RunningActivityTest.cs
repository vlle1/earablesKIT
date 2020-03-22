using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using EarablesKIT.Models;
using EarablesKIT.Models.Extentionmodel;
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
            
            
            //first we have to mock StepActivity (it needs no functionality, but needs to be instantiated)

            //Für den ServiceProviderMock
            //Muss enthalten sein, damit der Mock nicht überschrieben wird
            IServiceProvider unused = ServiceManager.ServiceProvider;

            //Feld Infos holen
            System.Reflection.FieldInfo instance = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            //Mocks aufsetzen 
            //ServiceProvider
            Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();
            Mock<IActivityManager> activityManagerMock = new Mock<IActivityManager>();
            Mock<IServiceProvider> activityProviderMock = new Mock<IServiceProvider>();
            Mock<AbstractStepActivity> stepActivityMock = new Mock<AbstractStepActivity>();

            mockSingleton.Setup(provider => provider.GetService(typeof(IActivityManager)))
                .Returns(activityManagerMock.Object);
            //ActivityManager
            activityManagerMock.Setup(x => x.ActitvityProvider).Returns(activityProviderMock.Object);
            activityProviderMock.Setup(x => x.GetService(typeof(AbstractStepActivity))).Returns(stepActivityMock.Object);

            Assert.NotNull(instance);
            instance.SetValue(null, mockSingleton.Object);

            //now we can instantiate a runningActivity
            RunningActivityThreshold toTest = new RunningActivityThreshold();

            bool detectedStatus = false;
            int changeDetectedCount = 0;
            toTest.ActivityDone +=
                (object sender, ActivityArgs a) =>
                {
                    detectedStatus = ((RunningEventArgs)a).Running;
                    changeDetectedCount++;
                };


            //read all the input from csv file and simulate a step after some tick
            int stepTick = 10;

            string line;
            System.IO.StreamReader file = new System.IO.StreamReader("../../../../ViewModelTests/Models/ExtensionModel/testData0steps.csv");

            //ignore first line

            file.ReadLine();
            int tick = 0;
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
                //push data to algorithm
                toTest.DataUpdate(data);
                tick++;
                if (tick == stepTick)
                {
                    //simulate detected step from step algorithm
                    toTest.OnStepRecognized(null, null);
                    Assert.Equal(1, changeDetectedCount);
                }
            }


            Assert.Equal(2, changeDetectedCount);
            Assert.False(detectedStatus);

            mockSingleton.VerifyAll();
            activityManagerMock.VerifyAll();
            activityProviderMock.VerifyAll();
            stepActivityMock.VerifyAll();
        }

        private void onChangeRecognizedTest(object sender, ActivityArgs args)
        {
        }

        [Fact]
        public void TestNoListener()
        {
            //here we will simulate what happens when no one is listening at some point. by Debugging it can be verified, that then when calling analyze, no more code is executed!
            //first register and analyze some data:

            IServiceProvider unused = ServiceManager.ServiceProvider;
            //feld infos holen
            System.Reflection.FieldInfo instance = typeof(ServiceManager).GetField("_serviceProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            //Mocks aufsetzen 
            //ServiceProvider
            Mock<IServiceProvider> mockSingleton = new Mock<IServiceProvider>();
            Mock<IActivityManager> activityManagerMock = new Mock<IActivityManager>();
            Mock<IServiceProvider> activityProviderMock = new Mock<IServiceProvider>();
            Mock<AbstractStepActivity> stepActivityMock = new Mock<AbstractStepActivity>();

            mockSingleton.Setup(provider => provider.GetService(typeof(IActivityManager)))
                .Returns(activityManagerMock.Object);
            //ActivityManager
            activityManagerMock.Setup(x => x.ActitvityProvider).Returns(activityProviderMock.Object);
            activityProviderMock.Setup(x => x.GetService(typeof(AbstractStepActivity))).Returns(stepActivityMock.Object);

            Assert.NotNull(instance);
            instance.SetValue(null, mockSingleton.Object);

            //now we can instantiate a runningActivity
            RunningActivityThreshold toTest = new RunningActivityThreshold();
            toTest.ActivityDone += onChangeRecognizedTest;

            //read all the input from csv file and simulate unregistering after some tick
            int stepTick = 10;

            string line;
            System.IO.StreamReader file = new System.IO.StreamReader("../../../../ViewModelTests/Models/ExtensionModel/testData0steps.csv");

            //ignore first line

            file.ReadLine();
            int tick = 0;
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
                //push data to algorithm
                toTest.DataUpdate(data);
                tick++;
                if (tick == stepTick)
                {
                    //simulate detected step from step algorithm
                    toTest.ActivityDone -= onChangeRecognizedTest;
                    //only using debugging you can verify now that the algorithm isn't executed anymore!
                }
            }

            mockSingleton.VerifyAll();
            activityManagerMock.VerifyAll();
            activityProviderMock.VerifyAll();
            stepActivityMock.VerifyAll();
        }
    }
}
