using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class IsUserActiveByADUserKeyCommandHandler : IHandlesDomainServiceCommand<IsUserActiveByADUserKeyCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public IsUserActiveByADUserKeyCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, IsUserActiveByADUserKeyCommand command)
        {
            command.Result = workflowAssignmentRepository.IsUserActive(command.ADUserKey);
        }
    }
}