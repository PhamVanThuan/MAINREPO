using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;
using SAHL.Services.WorkflowAssignmentDomain.CommandHandlers;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using SAHL.Services.WorkflowAssignmentDomain.Rules.Models;
using System;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.CommandHandlers
{
    public class when_assigning_a_workflow_case_when_rule_manager_returns_errors : WithFakes
    {
        private Establish that = () =>
        {
            dataManager = An<IWorkflowCaseDataManager>();
            eventRaiser = An<IEventRaiser>();
            domainRuleManager = An<IDomainRuleManager<UserHasCapabilityRuleModel>>();
            domainRuleManager
                .When(a => a.ExecuteRules(Arg.Any<ISystemMessageCollection>(), Arg.Any<UserHasCapabilityRuleModel>()))
                .Do(a => a.Arg<ISystemMessageCollection>().AddMessage(new SystemMessage("Some error", SystemMessageSeverityEnum.Error)));

            handler = new AssignWorkflowCaseCommandHandler(dataManager, eventRaiser, domainRuleManager);

            command = new AssignWorkflowCaseCommand(GenericKeyType.ThirdPartyInvoice, 2, 3L, 4, Capability.InvoiceApprover);
            metadata = An<IServiceRequestMetadata>();
        };

        private Because of = () =>
        {
            result = handler.HandleCommand(command, metadata);
        };

        private It should_have_executed_the_domain_rules = () =>
        {
            domainRuleManager.WasToldTo(a => a.ExecuteRules(Arg.Any<ISystemMessageCollection>(),
                Param<UserHasCapabilityRuleModel>.Matches(c => c.UserOrganisationStructureKey == command.UserOrganisationStructureKey
                    && c.CapabilityKey == (int)command.Capability)
                ));
        };

        private It should_have_error_messages = () =>
        {
            result.AllMessages.ShouldNotBeEmpty();
        };

        private It should_not_have_performed_the_command = () =>
        {
            dataManager.WasNotToldTo(a => a.AssignWorkflowCase(command));
        };

        private It should_not_have_raised_a_workflow_case_assigned_event = () =>
        {
            eventRaiser.WasNotToldTo(a => a.RaiseEvent(Arg.Any<DateTime>(), Arg.Any<IEvent>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<IServiceRequestMetadata>()));
        };

        private static IWorkflowCaseDataManager dataManager;
        private static IEventRaiser eventRaiser;
        private static AssignWorkflowCaseCommandHandler handler;
        private static AssignWorkflowCaseCommand command;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection result;
        private static IDomainRuleManager<UserHasCapabilityRuleModel> domainRuleManager;
    }
}