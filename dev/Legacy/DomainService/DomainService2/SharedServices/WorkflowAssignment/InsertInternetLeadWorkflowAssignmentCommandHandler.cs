using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class InsertInternetLeadWorkflowAssignmentCommandHandler : IHandlesDomainServiceCommand<InsertInternetLeadWorkflowAssignmentCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public InsertInternetLeadWorkflowAssignmentCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, InsertInternetLeadWorkflowAssignmentCommand command)
        {
            command.AssignedToResult = workflowAssignmentRepository.InsertInternetLeadWorkflowAssignment(command.InstanceID, command.ApplicationKey, command.State);
        }
    }
}