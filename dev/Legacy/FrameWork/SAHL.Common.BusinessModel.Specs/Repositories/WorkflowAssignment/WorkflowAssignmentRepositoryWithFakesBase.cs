using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using offerRole = SAHL.Common.BusinessModel.Interfaces.Repositories.OfferRole;

namespace SAHL.Common.BusinessModel.Specs.Repositories.WorkflowAssignment
{
    internal class WorkflowAssignmentRepositoryWithFakesBase : WithFakes
    {
        protected static IWorkflowSecurityRepository workflowSecurityRepo;
        protected static IWorkflowAssignmentRepository workflowAssignmentRepo;
        protected static ICastleTransactionsService castleTransactionService;
        protected static IApplicationRepository applicationRepository;
        protected static offerRole.WorkflowAssignment dataSet;

        public WorkflowAssignmentRepositoryWithFakesBase()
        {
            workflowSecurityRepo = An<IWorkflowSecurityRepository>();
            castleTransactionService = An<ICastleTransactionsService>();
            applicationRepository = An<IApplicationRepository>();
            //workflowAssignmentRepo = new WorkflowAssignmentRepository(castleTransactionService, workflowSecurityRepo, applicationRepository);
            workflowAssignmentRepo = An<IWorkflowAssignmentRepository>();
            dataSet = new offerRole.WorkflowAssignment();
        }
    }
}