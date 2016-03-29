using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Castle.ActiveRecord;
using System.Data;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.DataAccess;
using SAHL.Common.BusinessModel.Service;
using System.Diagnostics;
using System.Threading;

namespace SAHL.Common.BusinessModel.Test.DebtCounsellingTest
{
    [TestFixture]
    public class DebtCounsellingTest : TestBase
    {        
        [Test]
        public void AcceptedProposal()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 * from debtcounselling.Proposal p (nolock)
                        where p.Accepted = 1
                        and p.ProposalTypeKey = 1
                        and p.ProposalStatusKey = 1 ";

                DataTable DT = base.GetQueryResults(sql);
               
                Assert.That(DT.Rows.Count == 1, "No data for test");
                if (DT == null || DT.Rows.Count == 0)
                    return;

                int proposalKey = (int)DT.Rows[0][0];
                int dcKey = (int)DT.Rows[0][3];

                IDebtCounsellingRepository dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                IDebtCounselling dc = dcRepo.GetDebtCounsellingByKey(dcKey);

                Assert.AreEqual(dc.AcceptedActiveProposal.Key, proposalKey);

                sql = @"select top 1 dc.DebtCounsellingKey  from debtcounselling.DebtCounselling dc (nolock)
                    where dc.DebtCounsellingKey not in 
                    (
                        select distinct p.DebtCounsellingKey from debtcounselling.Proposal p (nolock)
                        where p.Accepted = 1
                        and p.ProposalTypeKey = 1
                        and p.ProposalStatusKey = 1
                    )";

                DT = base.GetQueryResults(sql);
                
                Assert.That(DT.Rows.Count == 1, "No data for test");
                if (DT == null || DT.Rows.Count == 0)
                    return;

                dcKey = (int)DT.Rows[0][0];
                dc = dcRepo.GetDebtCounsellingByKey(dcKey);

                Assert.IsNull(dc.AcceptedActiveProposal);
            }
        }

        [Test]
        public void GetActiveHearingDetailsForDebtCounselling()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 dc.DebtCounsellingKey 
                            from debtcounselling.DebtCounselling dc (nolock)
                            join debtcounselling.HearingDetail hd (nolock) on dc.DebtCounsellingKey = hd.DebtCounsellingKey 
                            where hd.GeneralStatusKey = 1";

                DataTable DT = base.GetQueryResults(query);
                
                Assert.That(DT.Rows.Count == 1, "No data for test");
                if (DT == null || DT.Rows.Count == 0)
                    return;
                int DebtCounsellingKey = Convert.ToInt32(DT.Rows[0][0]);

                IDebtCounsellingRepository dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                IDebtCounselling dc = dcRepo.GetDebtCounsellingByKey(DebtCounsellingKey);

                Assert.IsNotNull(dc.GetActiveHearingDetails);
                Assert.Greater(dc.GetActiveHearingDetails.Count, 0);
            }
        }
    }
}
