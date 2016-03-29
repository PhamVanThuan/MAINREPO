using System;
using System.Linq;
using SAHL.Config.Services;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;
using StructureMap;

namespace SAHL.Services.Interfaces.WorkflowAssignmentDomain.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var bootstrapper = new ServiceBootstrapper();
            var container = bootstrapper.Initialise();

            ObjectFactory.Configure(a =>
            {
                a.For<IRawLogger>().Use<NullRawLogger>();

                a.For<IRawMetricsLogger>().Use<NullMetricsRawLogger>();

                a.For<ILoggerSource>().Use(new LoggerSource("SAHL.Services.Interfaces.WorkflowAssignmentDomainService.Console", LogLevel.Error, true));
            });

            var instance = container.GetInstance<IWorkflowAssignmentDomainServiceClient>();

            System.Console.WriteLine("Press key to perform an assignment");

            while (true)
            {
                System.Console.ReadKey();

                var rand = new Random();

                var assignCommand = new AssignWorkflowCaseCommand(GenericKeyType.Account, 2, 5397430, 3, Capability.InvoiceProcessor);

                System.Console.WriteLine("Attempting to assign InstanceId {0} to (u{1}; c{2}; with gktk{3} and gk{4})",
                    assignCommand.InstanceId,
                    assignCommand.UserOrganisationStructureKey,
                    assignCommand.Capability,
                    assignCommand.GenericKeyTypeKey,
                    assignCommand.GenericKey);

                var messages = instance.PerformCommand(assignCommand, new ServiceRequestMetadata());

                foreach (var item in messages.AllMessages)
                {
                    System.Console.WriteLine("{0}: {1}", item.Severity, item.Message);
                }

                if (!messages.AllMessages.Any())
                {
                    System.Console.WriteLine("Success");
                }

                System.Console.WriteLine("Done");
            }
        }
    }
}
