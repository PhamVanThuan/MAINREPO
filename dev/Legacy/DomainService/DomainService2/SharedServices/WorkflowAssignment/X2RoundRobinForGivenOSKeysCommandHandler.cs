using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class X2RoundRobinForGivenOSKeysCommandHandler : IHandlesDomainServiceCommand<X2RoundRobinForGivenOSKeysCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public X2RoundRobinForGivenOSKeysCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, X2RoundRobinForGivenOSKeysCommand command)
        {
        }
    }
}