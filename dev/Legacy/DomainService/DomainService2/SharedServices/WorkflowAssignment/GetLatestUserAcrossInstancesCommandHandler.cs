using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class GetLatestUserAcrossInstancesCommandHandler : IHandlesDomainServiceCommand<GetLatestUserAcrossInstancesCommand>
    {
        IWorkflowAssignmentRepository workflowAssignmentRepository;

        public GetLatestUserAcrossInstancesCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, GetLatestUserAcrossInstancesCommand command)
        {
            string adusername = workflowAssignmentRepository.GetLatestUserAcrossInstances(command.InstanceID, command.ApplicationKey, command.OrganisationStructureKey, command.DynamicRole, command.State, command.Process);
            command.ADUserNameResult = adusername;
        }
    }
}