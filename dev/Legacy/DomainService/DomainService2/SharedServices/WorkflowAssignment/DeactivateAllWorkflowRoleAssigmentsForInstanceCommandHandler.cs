namespace DomainService2.SharedServices.WorkflowAssignment
{
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;

    public class DeactivateAllWorkflowRoleAssigmentsForInstanceCommandHandler : IHandlesDomainServiceCommand<DeactivateAllWorkflowRoleAssigmentsForInstanceCommand>
    {
        private IWorkflowAssignmentRepository WorkflowAssignmentRepository;

        public DeactivateAllWorkflowRoleAssigmentsForInstanceCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.WorkflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, DeactivateAllWorkflowRoleAssigmentsForInstanceCommand command)
        {
            WorkflowAssignmentRepository.DeactivateAllWorkflowRoleAssigmentsForInstance(command.InstanceID);
        }
    }
}