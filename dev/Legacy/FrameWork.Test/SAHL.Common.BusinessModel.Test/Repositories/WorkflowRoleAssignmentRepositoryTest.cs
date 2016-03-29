using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using workflowRole = SAHL.Common.BusinessModel.Interfaces.Repositories.WorkflowRole;
using SAHL.Common.BusinessModel.Service;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    public class WorkflowRoleAssignmentRepositoryTest : TestBase
    {
        static readonly ICastleTransactionsService castleTransactionService = new SAHL.Common.BusinessModel.Service.CastleTransactionsService();
        static readonly IWorkflowSecurityRepository workflowSecurityRepository = new SAHL.Common.BusinessModel.Repositories.WorkflowSecurityRepository();
        static readonly IWorkflowRoleAssignmentRepository workflowRoleAssignmentRepo = new SAHL.Common.BusinessModel.Repositories.WorkflowRoleAssignmentRepository(workflowSecurityRepository, castleTransactionService);

        [SetUp]            
        public void setup()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Default);
        }

        /// <summary>
        /// Test if the SQL script executes and the cache does populate
        /// </summary>
        [Test]
        public void LoadOrganisationStructure()
        {
            using (new SessionScope())
            {
                workflowRole.WorkflowAssignment ds = workflowSecurityRepository.GetWorkflowRoleOrganisationStructure();
                Assert.IsTrue(ds.ADUser.Rows.Count > 0);
                Assert.IsTrue(ds.UserOrganisationStructure.Rows.Count > 0);
                Assert.IsTrue(ds.WorkflowRoleTypeOrganisationStructureMapping.Rows.Count > 0);
                Assert.IsTrue(ds.UserOrganisationStructureRoundRobinStatus.Rows.Count > 0);
                Assert.IsTrue(ds.RoundRobinPointerDefinition.Rows.Count > 0);
            }
        }

    }
}

