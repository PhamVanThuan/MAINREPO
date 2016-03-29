using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using SAHL.Services.WorkflowAssignmentDomain.Rules;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.Rules.CapabilityMustHavePreviouslyWorkedOnTheInstanceSpecs
{
    public class when_a_user_has_been_previously_assigned : WithFakes
    {
        private static IWorkflowCaseDataManager dataManager;
        private static UserWithCapabilityMustHavePreviouslyBeenAssignedToTheInstanceRule rule;
        private static ISystemMessageCollection messages;
        private static int instanceId;
        private static Capability capability;

        private Establish that = () =>
        {
            instanceId = 1;
            capability = Capability.InvoiceProcessor;
            UserModel user = new UserModel(@"SAHL\TestUser", 123456, "Test User");
            dataManager = An<IWorkflowCaseDataManager>();
            dataManager.WhenToldTo(x => x.GetLastUserAssignedInCapability((int)capability, instanceId)).Return(user);
            rule = new UserWithCapabilityMustHavePreviouslyBeenAssignedToTheInstanceRule(dataManager);
            messages = SystemMessageCollection.Empty();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, new ReturnInstanceToLastUserInCapabilityCommand(GenericKeyType.ThirdPartyInvoice, 1, capability, instanceId));
        };

        private It should_try_to_user_with_provide_capability = () =>
        {
            dataManager.WasToldTo(x => x.GetLastUserAssignedInCapability((int)capability, instanceId));
        };

        private It should_not_return_any_errors = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}