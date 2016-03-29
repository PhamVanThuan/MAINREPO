using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;
using SAHL.Services.WorkflowAssignmentDomain.CommandHandlers;
using SAHL.Services.WorkflowAssignmentDomain.Managers;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.CommandHandlers
{
    public class when_no_user_with_capability_acted_on_the_instance : WithFakes
    {
        private static IDomainRuleManager<ReturnInstanceToLastUserInCapabilityCommand> domainRuleManager;
        private static ReturnInstanceToLastUserInCapabilityCommandHandler handler;
        private static ReturnInstanceToLastUserInCapabilityCommand command;
        private static ISystemMessageCollection messages;
        private static ISystemMessageCollection systemMessages;
        private static IServiceCommandRouter serviceCommandRouter;
        private static IServiceQueryRouter serviceQueryRouter;
        private static IServiceRequestMetadata metadata;
        private static IWorkflowCaseDataManager dataManager;
        private static int instanceId;
        private static Capability capability;

        private Establish that = () =>
        {
            messages = SystemMessageCollection.Empty();
            systemMessages = SystemMessageCollection.Empty();
            dataManager = An<IWorkflowCaseDataManager>();
            serviceQueryRouter = An<IServiceQueryRouter>();
            instanceId = 1002;
            capability = Capability.InvoiceProcessor;
            metadata = new ServiceRequestMetadata();
            serviceCommandRouter = An<IServiceCommandRouter>();
            domainRuleManager = An<IDomainRuleManager<ReturnInstanceToLastUserInCapabilityCommand>>();

            handler = new ReturnInstanceToLastUserInCapabilityCommandHandler(domainRuleManager, dataManager, serviceCommandRouter, serviceQueryRouter);
            command = new ReturnInstanceToLastUserInCapabilityCommand(GenericKeyType.ThirdPartyInvoice, 2, capability, instanceId);

            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), command)).Callback<ISystemMessageCollection>
                (
                    y =>
                    {
                        systemMessages = y;
                        var errorMsg = "The provided capability never acted on the provided instance";
                        y.AddMessage(new SystemMessage(errorMsg, SystemMessageSeverityEnum.Error));
                    }
                );
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_execute_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<SystemMessageCollection>(), command));
        };

        private It should_not_return_messages = () =>
        {
            messages.ErrorMessages().Any(x => x.Message.Equals("The provided capability never acted on the provided instance")).ShouldBeTrue();
        };
    }
}