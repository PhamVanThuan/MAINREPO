using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class CheckUserInMandateCommandHandler : IHandlesDomainServiceCommand<CheckUserInMandateCommand>
    {
        IWorkflowAssignmentRepository workflowAssignmentRepository;

        public CheckUserInMandateCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, CheckUserInMandateCommand command)
        {
            command.Result = workflowAssignmentRepository.CheckUserInMandate(command.ApplicationKey, command.ADuserName, command.OrgStructureName);
        }
    }
}