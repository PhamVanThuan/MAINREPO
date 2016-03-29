using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class CheckIfReinstateAllowedByUserCommandHandler : IHandlesDomainServiceCommand<CheckIfReinstateAllowedByUserCommand>
    {
        private IWorkflowAssignmentRepository workflowAssRepository;

        public CheckIfReinstateAllowedByUserCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, CheckIfReinstateAllowedByUserCommand command)
        {
            bool isConsultantOrAdmin = false;

            // check if the user is a consultant or admin
            IList<OfferRoleTypes> offerRoleTypes = new List<OfferRoleTypes>();
            offerRoleTypes.Add(OfferRoleTypes.BranchConsultantD);
            offerRoleTypes.Add(OfferRoleTypes.BranchAdminD);

            isConsultantOrAdmin = workflowAssRepository.IsUserInOrganisationStructureRole(command.ADUserName, offerRoleTypes);

            // if user is a consultant then check the previous state the case was in
            // if state is in specified list then do not let them do reinstate.
            if (isConsultantOrAdmin)
            {
                switch (command.PreviousState)
                {
                    case "Registration Pipeline":
                    case "Disbursement":
                    case "Disbursement Review":
                    case "Application Check":
                    case "Resub Application Check":
                        messages.Add(new Error(string.Format("You cannot Reinstate this NTU (Previous State: {0}) - please refer to your Manager.", command.PreviousState), ""));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}