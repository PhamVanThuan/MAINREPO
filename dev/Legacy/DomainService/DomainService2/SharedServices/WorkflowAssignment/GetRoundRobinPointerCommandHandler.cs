using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class GetRoundRobinPointerCommandHandler : IHandlesDomainServiceCommand<GetRoundRobinPointerCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public GetRoundRobinPointerCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, GetRoundRobinPointerCommand command)
        {
            IRoundRobinPointer roundRobinPointer = this.workflowAssignmentRepository.DetermineRoundRobinPointerByOfferRoleTypeAndOrgStructure(command.OfferRoleType, command.OrganisationStructureKey);
            command.Result = (roundRobinPointer == null) ? -1 : roundRobinPointer.Key;
        }
    }
}
