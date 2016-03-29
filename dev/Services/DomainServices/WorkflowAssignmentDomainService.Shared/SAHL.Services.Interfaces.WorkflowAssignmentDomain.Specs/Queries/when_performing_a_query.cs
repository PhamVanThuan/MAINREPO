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
    public class when_performing_a_query : WithFakes
    {
        Establish that = () =>
        {
            query = An<IWorkflowAssignmentDomainQuery>();

            client = ServiceClientHelper.CreateServiceClient();
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => client.PerformQuery(query));
        };

        private It should_have_performed_the_query = () =>
        {
            exception.Message.ShouldEqual("Completed successfully");
        };

        private static IWorkflowAssignmentDomainQuery query;
        private static WorkflowAssignmentDomainServiceClient client;
        private static Exception exception;
    }
}
