using System;
using System.Text;

using TestRunner.Scheduler;
using TestRunner.Scheduler.Entities;
using TestRunner.Scheduler.Exceptions;

namespace TestRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PrintUsage();

            TestRunScheduler scheduler = new TestRunScheduler();

            bool exit = false;
            while (!exit)
            {
                string userInput = Console.ReadLine();

                string invokedVerb = null;
                object invokedVerbInstance = null;

                var options = new Options();
                if (!CommandLine.Parser.Default.ParseArguments(userInput.Split(' '), options,
                  (verb, subOptions) =>
                  {
                      invokedVerb = verb;
                      invokedVerbInstance = subOptions;
                  }))
                {
                    Console.WriteLine($"Unrecognized command: {userInput}.");
                    PrintUsage();
                    continue;
                }

                switch (invokedVerb)
                {
                    case "exit":

                        exit = true;
                        Console.WriteLine("Exiting test runner.");
                        scheduler.CancelAllTestRuns();

                        continue;

                    case "start":

                        var startRunOptions = (StartNewTestRunOptions)invokedVerbInstance;
                        Console.WriteLine($"Starting test run of {startRunOptions.TestSuiteName}.");

                        Guid testRunId = scheduler.StartNewTestRun(startRunOptions.TestSuiteName);

                        Console.WriteLine($"Started test run with id {testRunId}.");
                        break;

                    case "status":

                        var statusOptions = (CommonOptions)invokedVerbInstance;

                        try
                        {
                            Console.WriteLine($"Retrieving status of test run with id {statusOptions.TestRunGuid}.");
                            TestRunStatus status = scheduler.GetTestRunStatus(Guid.Parse(statusOptions.TestRunGuid));
                            Console.WriteLine(status);
                        }
                        catch (TestRunNotFoundException ex)
                        {
                            Console.WriteLine("Failed to retrieve status of test run:");
                            Console.WriteLine(ex.Message);
                        }

                        break;

                    case "results":

                        var resultsOptions = (CommonOptions)invokedVerbInstance;

                        try
                        {
                            Console.WriteLine($"Retrieving results of test run with id {resultsOptions.TestRunGuid}.");
                            TestRunResults results = scheduler.GetTestRunResults(Guid.Parse(resultsOptions.TestRunGuid));
                            Console.WriteLine(results);
                        }
                        catch (TestRunNotFoundException ex)
                        {
                            Console.WriteLine("Failed to retrieve results of test run:");
                            Console.WriteLine(ex.Message);
                        }

                        break;

                    case "cancel":

                        var cancelRunOptions = (CommonOptions)invokedVerbInstance;

                        try
                        {
                            Console.WriteLine($"Canceling test run with id {cancelRunOptions.TestRunGuid}.");
                            scheduler.CancelTestRun(Guid.Parse(cancelRunOptions.TestRunGuid));
                            Console.WriteLine("Test run canceled.");
                        }
                        catch (TestRunNotFoundException ex)
                        {
                            Console.WriteLine("Failed to cancel test run:");
                            Console.WriteLine(ex.Message);
                        }

                        break;
                }
            }
        }

        private static void PrintUsage()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Test runner version 1.0.0");
            builder.AppendLine("Available commands:");

            builder.AppendLine("Exit the test runner:");
            builder.AppendLine("exit");
            builder.AppendLine();

            builder.AppendLine("Start a new test run:");
            builder.AppendLine("start -n testSuite1");
            builder.AppendLine();

            builder.AppendLine("Get the status of a test run:");
            builder.AppendLine("status -i [testRunId]");
            builder.AppendLine();

            builder.AppendLine("Get the results of a test run:");
            builder.AppendLine("results -i [testRunId]");
            builder.AppendLine();

            builder.AppendLine("Cancel a test run:");
            builder.AppendLine("cancel -i [testRunId]");
            builder.AppendLine();

            Console.WriteLine(builder.ToString());
        }
    }
}