using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Events;
using SAHL.Services.WorkflowAssignmentDomain.CommandHandlers;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using SAHL.Services.WorkflowAssignmentDomain.Rules.Models;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.CommandHandlers
{
    public class when_assigning_a_workflow_case : WithFakes
    {
        private static IWorkflowCaseDataManager dataManager;
        private static IEventRaiser eventRaiser;
        private static AssignWorkflowCaseCommandHandler handler;
        private static AssignWorkflowCaseCommand command;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection result;
        private static IDomainRuleManager<UserHasCapabilityRuleModel> domainRuleManager;

        private Establish that = () =>
        {
            dataManager = An<IWorkflowCaseDataManager>();
            eventRaiser = An<IEventRaiser>();
            domainRuleManager = An<IDomainRuleManager<UserHasCapabilityRuleModel>>();

            handler = new AssignWorkflowCaseCommandHandler(dataManager, eventRaiser, domainRuleManager);

            command = new AssignWorkflowCaseCommand(GenericKeyType.ThirdPartyInvoice, 2, 3L, 4, Capability.InvoicePaymentProcessor);
            metadata = An<IServiceRequestMetadata>();
        };

        private Because of = () =>
        {
            result = handler.HandleCommand(command, metadata);
        };

        private It should_have_executed_the_domain_rules = () =>
        {
            domainRuleManager.WasToldTo(a => a.ExecuteRules(Param<ISystemMessageCollection>.Matches(b =>
                !b.AllMessages.Any()),
                Param<UserHasCapabilityRuleModel>.Matches(c => c.UserOrganisationStructureKey == command.UserOrganisationStructureKey
                    && c.CapabilityKey == (int)command.Capability)
                ));
        };

        private It should_not_have_any_error_messages = () =>
        {
            result.AllMessages.ShouldBeEmpty();
        };

        private It should_have_performed_the_command = () =>
        {
            dataManager.WasToldTo(a => a.AssignWorkflowCase(command)).OnlyOnce();
        };

        private It should_have_raised_a_workflow_case_assigned_event = () =>
        {
            eventRaiser.WasToldTo(a => a.RaiseEvent(Arg.Any<DateTime>(),
                Param<IEvent>.Matches(b => b.GetType() == typeof(WorkflowCaseAssignedEvent)
                    && ((WorkflowCaseAssignedEvent)b).CapabilityKey.Equals((int)command.Capability)
                    && ((WorkflowCaseAssignedEvent)b).InstanceId.Equals(command.InstanceId)
                    && ((WorkflowCaseAssignedEvent)b).UserOrganisationStructureKey.Equals(command.UserOrganisationStructureKey)),
                command.GenericKey,
                (int)command.GenericKeyTypeKey,
                metadata))
                .OnlyOnce();
        };
    }
}