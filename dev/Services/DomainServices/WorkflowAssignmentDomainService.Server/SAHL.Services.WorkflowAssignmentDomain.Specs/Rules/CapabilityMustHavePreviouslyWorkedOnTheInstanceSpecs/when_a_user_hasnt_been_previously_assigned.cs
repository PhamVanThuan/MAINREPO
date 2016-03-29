using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using SAHL.Services.WorkflowAssignmentDomain.Rules;
using System.Linq;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.Rules.CapabilityMustHavePreviouslyWorkedOnTheInstanceSpecs
{
    public class when_a_user_hasnt_been_previously_assigned : WithFakes
    {
        private static IWorkflowCaseDataManager dataManager;
        private static UserWithCapabilityMustHavePreviouslyBeenAssignedToTheInstanceRule rule;
        private static ISystemMessageCollection messages;
        private static int instanceId;
        private static Capability capability;
        private static ReturnInstanceToLastUserInCapabilityCommand ruleModel;
        private Establish that = () =>
        {
            instanceId = 1;
            capability = Capability.InvoiceProcessor;
            UserModel user = null;
            dataManager = An<IWorkflowCaseDataManager>();
            dataManager.WhenToldTo(x => x.GetLastUserAssignedInCapability((int)capability, instanceId)).Return(user);
            rule = new UserWithCapabilityMustHavePreviouslyBeenAssignedToTheInstanceRule(dataManager);
            messages = SystemMessageCollection.Empty();
            ruleModel = new ReturnInstanceToLastUserInCapabilityCommand(GenericKeyType.ThirdPartyInvoice, 1, capability, instanceId);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_try_to_user_with_provide_capability = () =>
        {
            dataManager.WasToldTo(x => x.GetLastUserAssignedInCapability((int)capability, instanceId));
        };

        private It should_have_an_error_message = () =>
        {
            messages.ErrorMessages().Any(x => x.Message.Equals(string.Format("The case could not be returned to role: {0}. A user with that role has not previously worked on this case.",
                    ruleModel.Capability.ToString()))).ShouldBeTrue();
        };
    }
}