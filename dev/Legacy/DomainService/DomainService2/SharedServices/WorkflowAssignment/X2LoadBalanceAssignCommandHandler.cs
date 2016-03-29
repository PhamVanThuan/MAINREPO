using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class X2LoadBalanceAssignCommandHandler : IHandlesDomainServiceCommand<X2LoadBalanceAssignCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public X2LoadBalanceAssignCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, X2LoadBalanceAssignCommand command)
        {
            command.Result = workflowAssignmentRepository.X2LoadBalanceAssign(command.UserOrganisationStructureGenericKeyType, command.WorkflowRoleType, command.GenericKey, command.InstanceID, command.StatesToDetermineLoad, command.Process, command.Workflow, command.CheckRoundRobinStatus);
        }
    }
}