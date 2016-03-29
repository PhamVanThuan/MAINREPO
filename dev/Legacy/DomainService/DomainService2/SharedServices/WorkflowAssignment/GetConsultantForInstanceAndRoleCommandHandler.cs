using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class GetConsultantForInstanceAndRoleCommandHandler : IHandlesDomainServiceCommand<GetConsultantForInstanceAndRoleCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public GetConsultantForInstanceAndRoleCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, GetConsultantForInstanceAndRoleCommand command)
        {
            command.Result = workflowAssignmentRepository.GetConsultantForInstanceAndRole(command.InstanceID, command.DynamicRole);
        }
    }
}