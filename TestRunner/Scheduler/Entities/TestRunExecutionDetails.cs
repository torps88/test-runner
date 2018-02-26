using System.Threading;
using TestRunner.Executor;

namespace TestRunner.Scheduler.Entities
{
    public class TestRunExecutionDetails
    {
        public ITestRunExecutor Executor { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
    }
}