using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class HandleApplicationManagementRolesOnReturnFromNTUtoPreviousStateCommandHandler : IHandlesDomainServiceCommand<HandleApplicationManagementRolesOnReturnFromNTUtoPreviousStateCommand>
    {
        IWorkflowAssignmentRepository workflowAssignmentRepository;

        public HandleApplicationManagementRolesOnReturnFromNTUtoPreviousStateCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, HandleApplicationManagementRolesOnReturnFromNTUtoPreviousStateCommand command)
        {
            string assignedUser = workflowAssignmentRepository.HandleApplicationManagamentRolesOnReturnFromNTUtoPreviousState(command.InstanceID,
                                                                                                                              command.ApplicationKey,
                                                                                                                              command.PreNTUState,
                                                                                                                              command.IsFL,
                                                                                                                              command.ApplicationCaptureInstanceID,
                                                                                                                              command.ProcessName);

            command.AssignedUserResult = assignedUser;
        }
    }
}