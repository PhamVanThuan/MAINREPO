using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRoleCommandHandler : IHandlesDomainServiceCommand<GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRoleCommand>
    {
        IWorkflowAssignmentRepository workflowAssignmentRepository;

        public GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRoleCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRoleCommand command)
        {
            command.ADUserNameResult = workflowAssignmentRepository.GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRole(command.ApplicationRoleTypeKey, command.ApplicationKey, command.InstanceID);
        }
    }
}