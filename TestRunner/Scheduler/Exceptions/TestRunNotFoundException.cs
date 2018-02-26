using System;

namespace TestRunner.Scheduler.Exceptions
{
    public class TestRunNotFoundException : Exception
    {
        public TestRunNotFoundException(string message)
            : base(message)
        {
        }
    }
}