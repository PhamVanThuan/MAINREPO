using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class InsertCommissionableConsultantCommandHandler : IHandlesDomainServiceCommand<InsertCommissionableConsultantCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public InsertCommissionableConsultantCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, InsertCommissionableConsultantCommand command)
        {
            command.Result = workflowAssignmentRepository.InsertCommissionableConsultant(command.InstanceID, command.AdUserName, command.GenericKey, command.StateName);
        }
    }
}