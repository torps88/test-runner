using CommandLine;

namespace TestRunner
{
    public class StartNewTestRunOptions
    {
        [Option('n', "name", Required = true)]
        public string TestSuiteName { get; set; }
    }

    public class CommonOptions
    {
        [Option('i', "id", Required = true)]
        public string TestRunGuid { get; set; }
    }

    public class Options
    {
        [VerbOption("exit")]
        public bool Exit { get; set; }

        [VerbOption("start")]
        public StartNewTestRunOptions StartNewTestRun { get; set; }

        [VerbOption("cancel")]
        public CommonOptions CancelTestRun { get; set; }

        [VerbOption("status")]
        public CommonOptions GetTestRunStatus { get; set; }

        [VerbOption("results")]
        public CommonOptions GetTestRunResults { get; set; }
    }
}