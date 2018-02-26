using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestRunner.Executor;
using TestRunner.Executor.Factories;
using TestRunner.Scheduler;
using System;
using TestRunner.Executor.Entities;
using TestRunner.Scheduler.Entities;

namespace TestRunner.UnitTests
{
    /*
     * In order to timebox my submission, I provide this class as a demonstration of how I would unit test the logic in TestRunner.
     * It is not meant to be an exhaustive list of unit tests.
     */
    [TestClass]
    public class SampleUnitTests
    {
        [TestMethod]
        public void TestRunScheduler_VerifyStartNewRun_ReturnsRunId()
        {
            Mock<ITestRunExecutor> mockExecutor = new Mock<ITestRunExecutor>();
            Mock<ITestRunExecutorFactory> mockFactory = new Mock<ITestRunExecutorFactory>();
            mockFactory
                .Setup(m => m.GetNewTestRunExecutor(It.IsAny<string>()))
                .Returns(mockExecutor.Object);

            var schedulerUnderTest = new TestRunScheduler(mockFactory.Object);

            Guid testRunId = schedulerUnderTest.StartNewTestRun("mockTestSuiteName");
            Assert.AreNotEqual(Guid.Empty, testRunId, "StartNewTestRun did not generate a test run id.");
        }

        [TestMethod]
        public void TestRunScheduler_VerifyStartNewRun_BeginsExecution()
        {
            Mock<ITestRunExecutor> mockExecutor = new Mock<ITestRunExecutor>();

            Mock<ITestRunExecutorFactory> mockFactory = new Mock<ITestRunExecutorFactory>();
            mockFactory.Setup(m => m.GetNewTestRunExecutor(It.IsAny<string>())).Returns(mockExecutor.Object);

            var schedulerUnderTest = new TestRunScheduler(mockFactory.Object);

            schedulerUnderTest.StartNewTestRun("mockTestSuiteName");

            mockExecutor.Verify(
                v => v.BeginTestRunExecution(),
                Times.Once);
        }

        [TestMethod]
        public void TestRunScheduler_VerifyGetTestRunStatus_ReturnsResults()
        {
            TimeSpan expectedRunTime = TimeSpan.FromSeconds(40);

            Mock<ITestRunExecutor> mockExecutor = new Mock<ITestRunExecutor>();
            mockExecutor
                .SetupGet(s => s.ElapsedTime)
                .Returns(expectedRunTime);

            mockExecutor
                .SetupGet(s => s.Status)
                .Returns(ExecutorStatus.Completed);

            Mock<ITestRunExecutorFactory> mockFactory = new Mock<ITestRunExecutorFactory>();
            mockFactory.Setup(m => m.GetNewTestRunExecutor(It.IsAny<string>())).Returns(mockExecutor.Object);

            var schedulerUnderTest = new TestRunScheduler(mockFactory.Object);

            Guid testRunId = schedulerUnderTest.StartNewTestRun("mockTestSuite");
            TestRunStatus actual = schedulerUnderTest.GetTestRunStatus(testRunId);

            Assert.AreEqual(expectedRunTime, actual.CurrentRunTime);
            Assert.AreEqual(RunStatus.Completed, actual.Status);
        }
    }
}