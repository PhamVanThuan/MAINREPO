using System;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class AssignDebtCounsellingCaseForGroupOrLoadBalanceCommandHandler : IHandlesDomainServiceCommand<AssignDebtCounsellingCaseForGroupOrLoadBalanceCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public AssignDebtCounsellingCaseForGroupOrLoadBalanceCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, AssignDebtCounsellingCaseForGroupOrLoadBalanceCommand command)
        {
            command.Result = workflowAssignmentRepository.AssignDebtCounsellingCaseForGroupOrLoadBalance(command.InstanceID, command.DebtCounsellingKey, command.WorkflowRoleType, command.State, command.States, command.IncludeStates, command.CourtCase);
            if (command.Result == false)
            {
                messages.Add(new Error(String.Format("Could not find a user to assign to the case: InstanceID : {0}", command.InstanceID), ""));
            }
        }
    }
}