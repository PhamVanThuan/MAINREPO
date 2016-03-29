using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class RoundRobinAndAssignOtherFLCasesCommandHandler : IHandlesDomainServiceCommand<RoundRobinAndAssignOtherFLCasesCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public RoundRobinAndAssignOtherFLCasesCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, RoundRobinAndAssignOtherFLCasesCommand command)
        {
            command.AssignedUserResult = workflowAssignmentRepository.RoundRobinAndAssignOtherFLCases(command.ApplicationKey, command.DynamicRole, command.OrgStructureKey, command.InstanceID, command.State, command.RoundRobinPointerKey);
        }
    }
}