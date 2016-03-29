using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReassignToPreviousValuationsUserIfExistsElseRoundRobinCommandHandler : IHandlesDomainServiceCommand<ReassignToPreviousValuationsUserIfExistsElseRoundRobinCommand>
    {
        IWorkflowAssignmentRepository workflowAssignmentRepository;

        public ReassignToPreviousValuationsUserIfExistsElseRoundRobinCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, ReassignToPreviousValuationsUserIfExistsElseRoundRobinCommand command)
        {
            workflowAssignmentRepository.ReassignToPreviousValuationsUserIfExistsElseRoundRobin(command.DynamicRole,
                                                                                                command.OrganisationStructureKey,
                                                                                                command.ApplicationKey,
                                                                                                command.Map,
                                                                                                command.InstanceID,
                                                                                                command.State,
                                                                                                command.RoundRobinPointerKey);
        }
    }
}