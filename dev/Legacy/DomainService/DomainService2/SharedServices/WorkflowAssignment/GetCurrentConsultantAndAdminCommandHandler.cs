using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class GetCurrentConsultantAndAdminCommandHandler : IHandlesDomainServiceCommand<GetCurrentConsultantAndAdminCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public GetCurrentConsultantAndAdminCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, GetCurrentConsultantAndAdminCommand command)
        {
            command.Result = workflowAssignmentRepository.GetCurrentConsultantAndAdmin(command.InstanceID);
        }
    }
}