using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

using TestRunner.Executor;
using TestRunner.Executor.Entities;
using TestRunner.Executor.Factories;
using TestRunner.Scheduler.Entities;
using TestRunner.Scheduler.Exceptions;

namespace TestRunner.Scheduler
{
    public class TestRunScheduler
    {
        private readonly ITestRunExecutorFactory _testRunExecutorFactory;
        private ConcurrentDictionary<Guid, TestRunExecutionDetails> _testRunExecutions = new ConcurrentDictionary<Guid, TestRunExecutionDetails>();
        
        public TestRunScheduler()
            : this(new TestRunExecutorFactory())
        {
        }

        public TestRunScheduler(ITestRunExecutorFactory testRunExecutorFactory)
        {
            _testRunExecutions = new ConcurrentDictionary<Guid, TestRunExecutionDetails>();
            _testRunExecutorFactory = testRunExecutorFactory;
        }

        public Guid StartNewTestRun(string testSuiteName)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            ITestRunExecutor executor = _testRunExecutorFactory.GetNewTestRunExecutor(testSuiteName);

            Guid testRunId = Guid.NewGuid();
            _testRunExecutions[testRunId] = new TestRunExecutionDetails
            {
                CancellationTokenSource = cancellationTokenSource,
                Executor = executor
            };

            Task.Factory.StartNew(
                () =>
                {
                    cancellationToken.Register(() => executor.CancelTestRun());
                    executor.BeginTestRunExecution();
                },
                cancellationToken);

            return testRunId;
        }

        public TestRunStatus GetTestRunStatus(Guid testRunId)
        {
            if (!_testRunExecutions.ContainsKey(testRunId))
            {
                throw new TestRunNotFoundException($"Test run with id {testRunId} was not found.");
            }

            TestRunExecutionDetails runDetails = _testRunExecutions[testRunId];
            ITestRunExecutor executor = runDetails.Executor;

            TestRunStatus status = new TestRunStatus(
                MapRunStatus(executor.Status),
                executor.ElapsedTime);

            return status;
        }

        public TestRunResults GetTestRunResults(Guid testRunId)
        {
            if (!_testRunExecutions.ContainsKey(testRunId))
            {
                throw new TestRunNotFoundException($"Test run with id {testRunId} was not found.");
            }

            TestRunExecutionDetails runDetails = _testRunExecutions[testRunId];
            ITestRunExecutor executor = runDetails.Executor;

            TestRunResults results = new TestRunResults
            {
                TestOutput = executor.StandardOutput,
                ErrorOutput = executor.ErrorOutput,
                RunStatus = MapRunStatus(executor.Status),
                TotalRuntime = executor.ElapsedTime,
                TestRunId = testRunId,
                TestSuiteName = executor.TestSuiteName
            };

            return results;
        }

        public void CancelTestRun(Guid testRunId)
        {
            if (!_testRunExecutions.ContainsKey(testRunId))
            {
                throw new TestRunNotFoundException($"Test run with id {testRunId} was not found.");
            }

            TestRunExecutionDetails runDetails = _testRunExecutions[testRunId];
            runDetails.CancellationTokenSource.Cancel();
        }

        public void CancelAllTestRuns()
        {
            foreach (var entry in _testRunExecutions)
            {
                var executionStatus = entry.Value.Executor.Status;

                if (!(executionStatus == ExecutorStatus.Canceled || executionStatus == ExecutorStatus.Completed))
                {
                    entry.Value.CancellationTokenSource.Cancel();
                }
            }
        }

        private RunStatus MapRunStatus(ExecutorStatus status)
        {
            switch (status)
            {
                case ExecutorStatus.NotStarted:
                    return RunStatus.NotStarted;

                case ExecutorStatus.Running:
                    return RunStatus.Running;

                case ExecutorStatus.Canceled:
                    return RunStatus.Canceled;

                case ExecutorStatus.Completed:
                    return RunStatus.Completed;

                default:
                    throw new NotSupportedException($"Unknown ExecutorStatus {status}");
            }
        }
    }
}