using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using workflowRole = SAHL.Common.BusinessModel.Interfaces.Repositories.WorkflowRole;

namespace SAHL.Common.BusinessModel.Specs.Repositories.WorkflowRoleAssignment
{
    public class WorkflowRoleAssignmentRepositoryWithFakesBase : WithFakes
    {
        protected static IWorkflowSecurityRepository workflowSecurityRepo;
        protected static IWorkflowRoleAssignmentRepository workflowRoleAssignmentRepo;
        protected static ICastleTransactionsService castleTransactionService;
        protected static IApplicationRepository applicationRepository;
        protected static workflowRole.WorkflowAssignment dataSet;

        public WorkflowRoleAssignmentRepositoryWithFakesBase()
        {
            workflowSecurityRepo = An<IWorkflowSecurityRepository>();
            castleTransactionService = An<ICastleTransactionsService>();
            applicationRepository = An<IApplicationRepository>();
            workflowRoleAssignmentRepo = new WorkflowRoleAssignmentRepository(workflowSecurityRepo, castleTransactionService);
            dataSet = new workflowRole.WorkflowAssignment();
        }
    }
}