using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SAHL.Config.Services;
using SAHL.Core.Data;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Dapper;
using SAHL.Core.Logging;
using SAHL.Services.WorkflowTask.Server;
using StructureMap;

namespace SAHL.Servcies.WorkflowTask.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var bootstrapper = new ServiceBootstrapper();
            bootstrapper.Initialise();

            ObjectFactory.Configure(a =>
            {
                a.For<IRawLogger>().Use<NullRawLogger>();

                a.For<IRawMetricsLogger>().Use<NullMetricsRawLogger>();

                a.For<ILoggerSource>().Use(new LoggerSource("SAHL.Services.WorkflowTask.Console", LogLevel.Error, true));
            });

            var roles = new List<string>
            {
                "Invoice Approver",
                "Invoice Processor",
            };
            var user = @"SAHL\jessicav";

            for(var i = 0; i < 10000; i ++)
            {
                var task = new Task(() => Run(user, roles));
                task.Start();
            }
            System.Console.WriteLine("Started");

            System.Console.ReadKey();
        }

        private static void Run(string user, List<string> roles)
        {
            var coordinator = ObjectFactory.Container.GetInstance<ITaskQueryCoordinator>();

            var watch = new Stopwatch();
            watch.Start();

            var tasks = coordinator.GetWorkflowTasks(user, roles);

            watch.Stop();

            var total = tasks.SelectMany(a => a.WorkFlows).SelectMany(a => a.States).SelectMany(a => a.Tasks).Count();

            System.Console.WriteLine("Retrieved " + total + " tasks in " + watch.ElapsedMilliseconds + " milliseconds; mem usage @ "
                + Math.Round(Process.GetCurrentProcess().WorkingSet64 / 1024m / 1024m, 0)
                + " MB");
        }
    }
}
