using System;
using System.Diagnostics;

using TestRunner.Executor.Entities;

namespace TestRunner.Executor
{
    public class TestRunExecutor : ITestRunExecutor
    {
        private string _testSuiteNameToExecute;
        private Process _process;
        private Stopwatch _stopWatch;
        private ExecutorStatus _executorStatus;
        private string _standardOutput;
        private string _errorOutput;

        public ExecutorStatus Status
        {
            get => _executorStatus;
            private set => _executorStatus = value;
        }

        public string TestSuiteName
        {
            get => _testSuiteNameToExecute;
        }

        public TimeSpan ElapsedTime => _stopWatch.Elapsed;
        public string StandardOutput => _standardOutput;
        public string ErrorOutput => _errorOutput;

        public TestRunExecutor(string testSuiteNameToExecute)
        {
            _testSuiteNameToExecute = testSuiteNameToExecute;
            _stopWatch = new Stopwatch();
            _executorStatus = ExecutorStatus.NotStarted;
        }

        public void BeginTestRunExecution()
        {
            _process = new Process();
            _process.EnableRaisingEvents = true;
            _process.Exited += TestRunProcessExitedHandler;
            _process.StartInfo = new ProcessStartInfo
            {
                FileName = "node",
                Arguments = @"Executor\simpletestrunner.js " + _testSuiteNameToExecute,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            _process.Start();
            _stopWatch.Start();
            _executorStatus = ExecutorStatus.Running;
        }

        public void CancelTestRun()
        {
            if (!_process.HasExited)
            {
                _executorStatus = ExecutorStatus.Canceled;

                // Note: Killing the process will also raise the Exited event on the Process.
                _process.Kill();
            }
        }

        private void TestRunProcessExitedHandler(object sender, EventArgs e)
        {
            _stopWatch.Stop();

            if (_executorStatus == ExecutorStatus.Canceled)
            {
                // The process has already been killed via the CancelTestRun method, so nothing further to do here.
                return;
            }

            _executorStatus = ExecutorStatus.Completed;

            if (_process.StandardOutput != null)
            {
                using (var outputStream = _process.StandardOutput)
                {
                    _standardOutput = outputStream.ReadToEnd();
                }
            }

            if (_process.StandardError != null)
            {
                using (var errorStream = _process.StandardError)
                {
                    _errorOutput = errorStream.ReadToEnd();
                }
            }            
        }
    }
}