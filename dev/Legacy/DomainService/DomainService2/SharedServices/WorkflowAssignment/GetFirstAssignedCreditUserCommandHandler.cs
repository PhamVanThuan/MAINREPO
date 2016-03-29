using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class GetFirstAssignedCreditUserCommandHandler : IHandlesDomainServiceCommand<GetFirstAssignedCreditUserCommand>
    {
        IWorkflowAssignmentRepository workflowAssignmentRepo;

        public GetFirstAssignedCreditUserCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepo)
        {
            this.workflowAssignmentRepo = workflowAssignmentRepo;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, GetFirstAssignedCreditUserCommand command)
        {
            string adusername = string.Empty;
            int offerRoleTypeKey = -1;
            int orgStructKey = -1;

            workflowAssignmentRepo.GetFirstAssignedCreditUser(command.SourceInstanceID, out adusername, out offerRoleTypeKey, out orgStructKey);

            command.ADUserName = adusername;
            command.OfferRoleTypeKey = offerRoleTypeKey;
            command.OrganisationStructureKey = orgStructKey;
        }
    }
}