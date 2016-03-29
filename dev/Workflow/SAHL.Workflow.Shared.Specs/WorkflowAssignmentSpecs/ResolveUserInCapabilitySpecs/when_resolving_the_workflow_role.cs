using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Queries;

namespace SAHL.Workflow.Shared.Specs.WorkflowAssignmentSpecs.ResolveUserInCapabilitySpecs
{
    public class when_resolving_the_workflow_role : WithFakes
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
             expectedUser = @"SAHL\Bob";
             systemMessageCollection = SystemMessageCollection.Empty();
             instanceId = 1234567L;
             capability = Capability.InvoiceProcessor;
             query = new GetUserCurrentlyAssignedInCapabilityQuery(instanceId, (int)capability);
             workflowDomainServiceClient = An<IWorkflowAssignmentDomainServiceClient>();
             query.Result = new ServiceQueryResult<GetUserCurrentlyAssignedInCapabilityQueryResult>(
                         new GetUserCurrentlyAssignedInCapabilityQueryResult[] {
                            new GetUserCurrentlyAssignedInCapabilityQueryResult { UserName = expectedUser, AssignmentDate = DateTime.Now, UserOrganisationStructureKey = 1234 } }
                             );
             workflowDomainServiceClient.WhenToldTo(c => c.PerformQuery(Param.IsAny<GetUserCurrentlyAssignedInCapabilityQuery>())).Return<GetUserCurrentlyAssignedInCapabilityQuery>(y =>
             {
                 y.Result = query.Result;
                 return SystemMessageCollection.Empty();
             });
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

        private It should_return_the_user_from_the_query = () =>
        {
            assignedUser.ShouldEqual(expectedUser);
        };
    }
}