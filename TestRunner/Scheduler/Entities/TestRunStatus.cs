using System;
using System.Text;

namespace TestRunner.Scheduler.Entities
{
    public class TestRunStatus
    {
        public readonly RunStatus Status;
        public readonly TimeSpan CurrentRunTime;

        public TestRunStatus(RunStatus status, TimeSpan currentRunTime)
        {
            Status = status;
            CurrentRunTime = currentRunTime;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Status: {Status}");
            builder.AppendLine($"CurrentRunTime: {CurrentRunTime}");

            return builder.ToString();
        }
    }
}