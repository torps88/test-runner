using System;
using System.Text;

namespace TestRunner.Scheduler.Entities
{
    public class TestRunResults
    {
        public Guid TestRunId { get; set; }
        public string TestSuiteName { get; set; }
        public string TestOutput { get; set; }
        public string ErrorOutput { get; set; }
        public RunStatus RunStatus { get; set; }
        public TimeSpan TotalRuntime { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("---TestRunStatus---");
            builder.AppendLine($"TestRunId: {TestRunId}");
            builder.AppendLine($"TestSuiteName: {TestSuiteName}");
            builder.AppendLine($"TestRunStatus: {RunStatus}");
            builder.AppendLine($"TotalRuntime: {TotalRuntime}");

            if (RunStatus == RunStatus.Completed)
            {
                if (TestOutput != null)
                {
                    builder.AppendLine("Output:");
                    builder.AppendLine(TestOutput);
                }

                if (ErrorOutput != null)
                {
                    builder.AppendLine("Errors:");
                    builder.AppendLine(ErrorOutput);
                }
            }

            return builder.ToString();
        }
    }
}