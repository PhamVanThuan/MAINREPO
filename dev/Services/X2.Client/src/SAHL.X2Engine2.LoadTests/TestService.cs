using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.LoadTests
{
    public class TestService
    {
        private ITestCase testCase;

        public TestService(ITestCase testCase)
        {
            this.testCase = testCase;
        }

        public void CreateWorkers()
        {
            var numberOfWorkers = 10;
            List<Task> tasks = new List<Task>();

            for (int i = 1; i <= numberOfWorkers; i++)
            {
                var hostName = Environment.MachineName;
                var task = Task.Factory.StartNew(() => { this.testCase.Test(hostName, i); });
                tasks.Add(task);
                Thread.Sleep(2000);
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}