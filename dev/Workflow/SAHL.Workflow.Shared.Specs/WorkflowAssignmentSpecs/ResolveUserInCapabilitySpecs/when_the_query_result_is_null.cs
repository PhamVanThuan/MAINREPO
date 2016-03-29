using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Queries;

namespace SAHL.Workflow.Shared.Specs.WorkflowAssignmentSpecs.ResolveUserInCapabilitySpecs
{
    public class when_the_query_result_is_null : WithFakes
    {
        private static IWorkflowAssignmentDomainServiceClient workflowDomainServiceClient;
        private static GetUserCurrentlyAssignedInCapabilityQuery query;
        private static long instanceId;
        private static Capability capability;
        private static WorkflowAssignment workflowAssignment;
        private static ISystemMessageCollection systemMessageCollection;
        private static string assignedUser;
        private static string expectedUser;

        private Establish context = () =>
        {
            expectedUser = string.Empty;
            systemMessageCollection = SystemMessageCollection.Empty();
            instanceId = 1234567L;
            capability = Capability.InvoiceProcessor;
            query = new GetUserCurrentlyAssignedInCapabilityQuery(instanceId, (int)capability);
            workflowDomainServiceClient = An<IWorkflowAssignmentDomainServiceClient>();
            workflowDomainServiceClient.WhenToldTo(c => c.PerformQuery(Param.IsAny<GetUserCurrentlyAssignedInCapabilityQuery>()))
            .Return<GetUserCurrentlyAssignedInCapabilityQuery>(y => { y.Result = null; return SystemMessageCollection.Empty(); });
            workflowAssignment = new WorkflowAssignment(workflowDomainServiceClient);
        };

        private Because of = () =>
        {
            assignedUser = workflowAssignment.ResolveUserInCapability(systemMessageCollection, instanceId, Capability.InvoiceProcessor);
        };

        private It should_query_to_find_the_user_role = () =>
        {
            workflowDomainServiceClient.WasToldTo(x => x.PerformQuery(Arg.Is<GetUserCurrentlyAssignedInCapabilityQuery>(y => y.InstanceId == instanceId
              && y.Capability == (int)Capability.InvoiceProcessor)));
        };

        private It should_return_an_empty_string = () =>
        {
            assignedUser.ShouldEqual(expectedUser);
        };
    }
}