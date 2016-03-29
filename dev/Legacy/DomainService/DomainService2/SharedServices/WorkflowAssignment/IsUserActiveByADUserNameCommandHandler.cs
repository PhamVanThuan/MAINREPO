using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class IsUserActiveByADUserNameCommandHandler : IHandlesDomainServiceCommand<IsUserActiveByADUserNameCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public IsUserActiveByADUserNameCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, IsUserActiveByADUserNameCommand command)
        {
            command.Result = workflowAssignmentRepository.IsUserActive(command.ADUserName);
        }
    }
}