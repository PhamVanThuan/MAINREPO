using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Events;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using SAHL.Services.WorkflowAssignmentDomain.Rules;
using SAHL.Services.WorkflowAssignmentDomain.Rules.Models;

namespace SAHL.Services.WorkflowAssignmentDomain.CommandHandlers
{
    public class AssignWorkflowCaseCommandHandler : IDomainServiceCommandHandler<AssignWorkflowCaseCommand, WorkflowCaseAssignedEvent>
    {
        private readonly IWorkflowCaseDataManager dataManager;
        private readonly IEventRaiser eventRaiser;
        private readonly IDomainRuleManager<UserHasCapabilityRuleModel> domainRuleManager;

        public AssignWorkflowCaseCommandHandler(IWorkflowCaseDataManager dataManager, IEventRaiser eventRaiser, IDomainRuleManager<UserHasCapabilityRuleModel> domainRuleManager)
        {
            this.dataManager = dataManager;
            this.eventRaiser = eventRaiser;
            this.domainRuleManager = domainRuleManager;

            this.domainRuleManager.RegisterRule(new UserOrganisationStructureMustHaveCapabilityRule(this.dataManager));
            this.domainRuleManager.RegisterRule(new UserOrganisationStructureKeyShouldBelongToActiveADUserRule(this.dataManager));
        }

        public ISystemMessageCollection HandleCommand(AssignWorkflowCaseCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            domainRuleManager.ExecuteRules(messages, new UserHasCapabilityRuleModel(command.UserOrganisationStructureKey, (int)command.Capability));

            if (messages.HasErrors)
            {
                return messages;
            }

            dataManager.AssignWorkflowCase(command);

            var eventDate = DateTime.Now;
            var caseAssignedEvent = new WorkflowCaseAssignedEvent(eventDate,
                (int)command.GenericKeyTypeKey,
                command.GenericKey,
                command.InstanceId,
                command.UserOrganisationStructureKey,
                (int)command.Capability);

            eventRaiser.RaiseEvent(eventDate, caseAssignedEvent, command.GenericKey, (int)command.GenericKeyTypeKey, metadata);

            return messages;
        }
    }
}
