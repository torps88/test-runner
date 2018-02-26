namespace TestRunner.Executor.Factories
{
    public class TestRunExecutorFactory : ITestRunExecutorFactory
    {
        ITestRunExecutor ITestRunExecutorFactory.GetNewTestRunExecutor(string testSuiteName)
        {
            return new TestRunExecutor(testSuiteName);
        }
    }
}