using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;
using SAHL.Services.WorkflowTask.Shared.Tests;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;

namespace SAHL.Services.Interfaces.WorkflowAssignmentDomain.Specs.Commands
{
    public class when_performing_a_command : WithFakes
    {
        Establish that = () =>
        {
            command = An<IWorkflowAssignmentDomainCommand>();

            client = ServiceClientHelper.CreateServiceClient();
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => client.PerformCommand(command, null));
        };

        private It should_have_performed_the_command = () =>
        {
            exception.Message.ShouldEqual("Completed successfully");
        };

        private static IWorkflowAssignmentDomainCommand command;
        private static WorkflowAssignmentDomainServiceClient client;
        private static Exception exception;
    }
}
