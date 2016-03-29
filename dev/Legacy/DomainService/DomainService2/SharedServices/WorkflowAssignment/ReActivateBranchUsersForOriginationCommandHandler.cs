using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReActivateBranchUsersForOriginationCommandHandler : IHandlesDomainServiceCommand<ReActivateBranchUsersForOriginationCommand>
    {
        IWorkflowAssignmentRepository workflowAssignmentRepository;

        public ReActivateBranchUsersForOriginationCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, ReActivateBranchUsersForOriginationCommand command)
        {
            string adUser = this.workflowAssignmentRepository.ReActivateBranchUsersForOrigination(command.ApplicationManagementInstanceID,
                                                                                                  command.ApplicationCaptureInstanceID,
                                                                                                  command.ApplicationKey,
                                                                                                  command.State,
                                                                                                  command.ProcessName);

            command.AssignedUsersResult = adUser;
        }
    }
}