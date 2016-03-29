using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReturnFeedbackOnverrideUserCommandHandler : IHandlesDomainServiceCommand<ReturnFeedbackOnverrideUserCommand>
    {
        IWorkflowAssignmentRepository workflowAssignmentRepository;

        public ReturnFeedbackOnverrideUserCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, ReturnFeedbackOnverrideUserCommand command)
        {
            command.UserResult = workflowAssignmentRepository.ReturnFeedbackOnverrideUser(command.InstanceID);
        }
    }
}