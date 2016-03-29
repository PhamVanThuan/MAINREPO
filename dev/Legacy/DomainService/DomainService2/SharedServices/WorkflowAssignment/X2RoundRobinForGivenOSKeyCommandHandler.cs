using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class X2RoundRobinForGivenOSKeyCommandHandler : IHandlesDomainServiceCommand<X2RoundRobinForGivenOSKeyCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public X2RoundRobinForGivenOSKeyCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, X2RoundRobinForGivenOSKeyCommand command)
        {
        }
    }
}