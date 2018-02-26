using System;
using TestRunner.Executor.Entities;

namespace TestRunner.Executor
{
    public interface ITestRunExecutor
    {
        string TestSuiteName { get; }

        ExecutorStatus Status { get; }
        
        TimeSpan ElapsedTime { get; }

        string StandardOutput { get; }

        string ErrorOutput { get; }

        void BeginTestRunExecution();

        void CancelTestRun();
    }
}