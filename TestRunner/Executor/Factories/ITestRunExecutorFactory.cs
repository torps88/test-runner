namespace TestRunner.Executor.Factories
{
    public interface ITestRunExecutorFactory
    {
        ITestRunExecutor GetNewTestRunExecutor(string testSuiteName);
    }
}