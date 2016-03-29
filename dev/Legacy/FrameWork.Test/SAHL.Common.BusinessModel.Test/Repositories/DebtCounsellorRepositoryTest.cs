using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.DataSets;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class DebtCounsellorRepositoryTest : TestBase
    {
        private IDebtCounsellingRepository _repo;

        [SetUp]
        public void Setup()
        {
            _repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

            // set the strategy to default so we actually go to the database
            SetRepositoryStrategy(TypeFactoryStrategy.Default);
            _mockery = new MockRepository();
        }

        [TearDown]
        public void TearDown()
        {
            MockCache.Flush();
        }

        [Test]
        public void CreateEmptyProposal()
        {
            using (new SessionScope())
            {
                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                IProposal proposal = repo.CreateEmptyProposal();
                Assert.IsNotNull(proposal);
            }
        }

        [Test]
        public void CreateEmptyProposalItem()
        {
            using (new SessionScope())
            {
                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                IProposalItem proposalItem = repo.CreateEmptyProposalItem();
                Assert.IsNotNull(proposalItem);
            }
        }

        [Test]
        public void GetProposalItemsByKey()
        {
            using (new SessionScope())
            {
                string query = "Select TOP 1 P.ProposalKey, COUNT(P.ProposalKey) " +
                                "From debtcounselling.Proposal P " +
                                "Inner Join debtcounselling.ProposalItem PITEM On P.ProposalKey = PITEM.ProposalKey " +
                                "Group By P.ProposalKey";
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1, "No data for test");
                if (DT == null || DT.Rows.Count == 0)
                    return;

                int proposalKey = Convert.ToInt32(DT.Rows[0][0]);
                int count = Convert.ToInt32(DT.Rows[0][1]);
                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                List<IProposalItem> proposalItemList = repo.GetProposalItemsByKey(proposalKey);

                Assert.That(proposalItemList.Count == count);
            }
        }

        public void TestDCSetSubject()
        {
            DataTable dt = null;
            string legalEntityName = "";
            using (new SessionScope())
            {
                try
                {
                    string sql = string.Format(@"SELECT   ISNULL(ST.Description + ' ', '') + COALESCE (LE.Initials + ' ', LEFT(LE.FirstNames, 1) + ' ', '')
												+ ISNULL(LE.Surname, '') AS Name
												FROM         [2am].dbo.ExternalRole (nolock) AS ER INNER JOIN
												[2am].dbo.LegalEntity (nolock) AS LE ON ER.LegalEntityKey = LE.LegalEntityKey INNER JOIN
												[2am].debtcounselling.DebtCounselling (nolock) AS DC ON ER.GenericKey = DC.DebtCounsellingKey LEFT OUTER JOIN
												[2am].dbo.SalutationType (nolock) AS ST ON LE.Salutationkey = ST.SalutationKey
												WHERE     (ER.ExternalRoleTypeKey = 1) AND (DC.DebtCounsellingKey = {0})
												GROUP BY LE.FirstNames, LE.Initials, LE.Surname, LE.PreferredName, ST.Description", 167);

                    DataTable DT = base.GetQueryResults(sql);
                    if (DT.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DT.Rows)
                        {
                            legalEntityName = legalEntityName + dr[0].ToString() + " & ";
                        }

                        legalEntityName = legalEntityName.Substring(0, legalEntityName.Length - 2);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        [Test]
        public void GetProposalItemByKey()
        {
            using (new SessionScope())
            {
                string query = "Select top 1 PITEM.ProposalItemKey " +
                                "From debtcounselling.ProposalItem PITEM";
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1, "No data for test");
                if (DT == null || DT.Rows.Count == 0)
                    return;

                int proposalItemKey = Convert.ToInt32(DT.Rows[0][0]);
                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                IProposalItem proposalItem = repo.GetProposalItemByKey(proposalItemKey);

                Assert.IsNotNull(proposalItem);
            }
        }

        [Test]
        public void CreateAccountSnapShot()
        {
            int accountKey = 0;
            int roCount = 0;
            int dcKey = 0;

            using (new SessionScope(FlushAction.Never))
            {
                string query = string.Format(@"with cteDC (AccountKey, DebtCounsellingKey) AS
                    (
	                    select AccountKey, max(DebtCounsellingKey)
	                    from DebtCounselling.DebtCounselling where DebtCounsellingStatusKey = 1
	                    GROUP BY AccountKey
	                    HAVING (COUNT(AccountKey) = 1)
                    )

                    SELECT   top 1   fs.AccountKey, count(fa.FinancialAdjustmentKey) rorCount, max(cteDC.DebtCounsellingKey) AS DebtCounsellingKey
                        FROM fin.FinancialAdjustment fa
                        INNER JOIN FinancialService fs ON fa.FinancialServiceKey = fs.FinancialServiceKey
                        JOIN cteDC on fs.AccountKey = cteDC.AccountKey
                        JOIN account act on act.accountkey = fs.AccountKey
                        left JOIN [debtcounselling].[SnapShotAccount] ssa on cteDC.AccountKey = ssa.AccountKey
                        WHERE fa.FinancialAdjustmentStatusKey  = 1 and ssa.AccountKey is null AND act.RRR_productkey = 1
                        Group by  fs.AccountKey
                        HAVING      (COUNT(fs.AccountKey) > 1)
                        Order by fs.AccountKey");

                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);
                accountKey = Convert.ToInt32(DT.Rows[0][0]);
                roCount = Convert.ToInt32(DT.Rows[0][1]);
                dcKey = Convert.ToInt32(DT.Rows[0][2]);
                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                repo.CreateAccountSnapShot(dcKey);
            }
            try
            {
                // Check for Account Insert
                string sqlact = string.Format(@"select * from [debtcounselling].[SnapShotAccount] (nolock) where  Accountkey = {0}", accountKey);
                DataTable DTResultact = base.GetQueryResults(sqlact);
                Assert.That(DTResultact.Rows.Count == 1);

                // Check for Financial Service
                string sqlfs = string.Format(@"select * from [debtcounselling].[SnapShotFinancialService](nolock) where  Accountkey = {0}", accountKey);
                DataTable DTResultfs = base.GetQueryResults(sqlfs);
                Assert.That(DTResultfs.Rows.Count == 1);

                // Check for RateOverRides
                string sql = string.Format(@"select * from [debtcounselling].[SnapShotFinancialAdjustment] (nolock) where  Accountkey = {0}", accountKey);
                DataTable DTResult = base.GetQueryResults(sql);
                Assert.That(DTResult.Rows.Count == roCount);
            }
            finally
            {
                base.GetQueryResults("Delete from [debtcounselling].SnapShotFinancialAdjustment where AccountKey =  " + accountKey.ToString());
                base.GetQueryResults("Delete from [debtcounselling].SnapShotFinancialService where AccountKey =  " + accountKey.ToString());
                base.GetQueryResults("Delete from [debtcounselling].SnapShotAccount where AccountKey =  " + accountKey.ToString());
            }
        }

        /// <summary>
        /// Attempt to Create a new Debt Counselling Group and Create a Debt Counselling Case and assign to group
        /// </summary>

        [Test]
        public void CreateNewDebtCounsellingGroup()
        {
            using (new SessionScope(FlushAction.Never))
            {
                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                IAccountRepository accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
                ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

                IDebtCounsellingStatus debtCounsellingStatus = lookupRepository.DebtCounsellingStatuses[DebtCounsellingStatuses.Open];

                ///Pass
                string sql = @"select top 1 * from [2am].dbo.Account";
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    return;

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    Assert.Fail("No data for UnderDebtCousellingTests : Pass");

                int accountKey = Convert.ToInt32(ds.Tables[0].Rows[0]["AccountKey"]);

                IAccount account = accountRepository.GetAccountByKey(accountKey);

                //Create empty group
                IDebtCounsellingGroup group = repo.CreateEmptyDebtCounsellingGroup();
                group.CreatedDate = DateTime.Now;

                //create new case
                IDebtCounselling debtCounselling = repo.CreateEmptyDebtCounselling();

                debtCounselling.Account = account;
                debtCounselling.DebtCounsellingStatus = debtCounsellingStatus;
                debtCounselling.DebtCounsellingGroup = group;

                group.DebtCounsellingCases.Add(null, debtCounselling);

                repo.SaveDebtCounsellingGroup(group);
            }
        }

        [Test]
        public void GetAmortisationScheduleForProposalByKey()
        {
            using (new SessionScope(FlushAction.Never))
            {
                int maxPeriods = 300;
                string query = @"select top 1 p.ProposalKey
                    from debtcounselling.ProposalItem ppi (nolock)
                    join debtcounselling.Proposal p (nolock) on ppi.ProposalKey = p.ProposalKey
                    where datediff(mm, ppi.StartDate, ppi.EndDate) > 1";
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1, "No data for test");
                if (DT == null || DT.Rows.Count == 0)
                    return;

                int ProposalKey = Convert.ToInt32(DT.Rows[0][0]);

                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                LoanCalculations.AmortisationScheduleDataTable lcAmortShed = repo.GetAmortisationScheduleForProposalByKey(ProposalKey, maxPeriods);
                Assert.IsNotNull(lcAmortShed);
            }
        }

        [Test]
        public void UnderDebtCousellingTests()
        {
            using (new SessionScope(FlushAction.Never))
            {
                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                IAccountRepository accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();

                //ILegalEntityRepository leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                ///Pass
                string sql = @"select top 1 r.*
                    from
                    [Role] r (nolock)
                    join debtcounselling.DebtCounselling dc (nolock) on r.AccountKey = dc.AccountKey
                    join ExternalRole er (nolock) on dc.DebtCounsellingKey = er.GenericKey
	                    and er.GenericKeyTypeKey = 27 --DebtCounselling
	                    and r.LegalEntityKey = er.LegalEntityKey
                    where
	                    r.GeneralStatusKey = 1
	                    and er.GeneralStatusKey = 1
	                    and dc.DebtCounsellingStatusKey = 1
	                    and er.ExternalRoleTypeKey = 1 --Client Role types only";
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    return;

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    Assert.Fail("No data for UnderDebtCousellingTests : Pass");

                int AccKey = Convert.ToInt32(ds.Tables[0].Rows[0]["AccountKey"]);
                int LEKey = Convert.ToInt32(ds.Tables[0].Rows[0]["LegalEntityKey"]);

                IAccount account = accountRepository.GetAccountByKey(AccKey);

                Assert.IsTrue(account.UnderDebtCounselling);

                bool letest = false;
                foreach (IRole r in account.Roles)
                {
                    if (r.LegalEntity.Key == LEKey)
                    {
                        Assert.IsTrue(r.UnderDebtCounselling(true));
                        Assert.IsTrue(r.LegalEntity.UnderDebtCounselling);
                        letest = true;
                    }
                }

                //Confirm we found and tested what we were after
                Assert.IsTrue(letest);

                //Fail
                sql = @"select top 1 * from [Role] r (nolock)
                    left join debtcounselling.DebtCounselling dc on r.AccountKey = dc.AccountKey
	                    and dc.DebtCounsellingStatusKey = 1
                    left join ExternalRole er (nolock) on r.LegalEntityKey = er.LegalEntityKey
	                    and er.GeneralStatusKey = 1
	                    and er.ExternalRoleTypeKey = 1
	                    and er.GenericKeyTypeKey = 27 --DebtCounselling
                    where
                        r.GeneralStatusKey = 1
	                    and dc.DebtCounsellingKey is null
	                    and er.ExternalRoleKey is null";
                ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    Assert.Fail("No data for UnderDebtCousellingTests : Fail");

                AccKey = Convert.ToInt32(ds.Tables[0].Rows[0]["AccountKey"]);
                LEKey = Convert.ToInt32(ds.Tables[0].Rows[0]["LegalEntityKey"]);

                account = accountRepository.GetAccountByKey(AccKey);

                Assert.IsFalse(account.UnderDebtCounselling);

                letest = false;
                foreach (IRole r in account.Roles)
                {
                    if (r.LegalEntity.Key == LEKey)
                    {
                        Assert.IsFalse(r.UnderDebtCounselling(true));
                        Assert.IsFalse(r.LegalEntity.UnderDebtCounselling);
                        letest = true;
                    }
                }

                //Confirm we found and tested what we were after
                Assert.IsTrue(letest);
            }
        }

        [Test]
        public void GetEworkDataForLossControlCaseTest()
        {
            string sql = @"select top 1 eFolderName " +
                        "from [e-work]..eFolder ef (nolock) " +
                        "join [e-work]..LossControl lc (nolock) on lc.eFolderID = ef.eFolderID " +
                        "   and lc.UserToDo is not null " +
                        "where ef.eMapName = 'LossControl' " +
                        "and eStageName in (select eStageName from [e-work]..eStage (nolock) " +
                        "where eStageType not in (4,6) and eStageName <> 'Collection Archive' " +
                        "and eLoadedTime = (select top 1 eLoadedTime from [e-work]..eProcedure (nolock) " +
                        "where eProcedureName = 'Loss Control' order by eVersion desc))";

            DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return;

            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                Assert.Fail("No data for GetEworkDataForLossControlCaseTest : Fail");

            string eFolderName = ds.Tables[0].Rows[0]["eFolderName"].ToString();

            IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
            string StageName = "";
            string FolderID = "";
            IADUser adUser = null;
            repo.GetEworkDataForLossControlCase(Convert.ToInt32(eFolderName), out StageName, out FolderID, out adUser);
            Assert.IsNotNull(StageName);
            Assert.IsNotNull(FolderID);
            Assert.IsNotNull(adUser);
        }

        [Test]
        public void GetParentCompanyDebtCounsellorNCR()
        {
            using (new SessionScope())
            {
                IControlRepository controlRepo = RepositoryFactory.GetRepository<IControlRepository>();
                IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IDebtCounsellingRepository debtcounsellingRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                // find latest debtcounselling record
                string sql = @"select top 1 DebtCounsellingKey from [2am].debtcounselling.DebtCounselling dc (nolock) order by 1 asc";
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    Assert.Fail("No data for GetParentCompanyDebtCounsellorNCR Test");

                int debtCounsellingKey = Convert.ToInt32(ds.Tables[0].Rows[0]["DebtCounsellingKey"]);

                // get top level company
                IDebtCounsellorOrganisationNode dcNode = debtcounsellingRepo.GetTopDebtCounsellorCompanyNodeForDebtCounselling(debtCounsellingKey);

                // get the NCR number of top level company
                string ncrNumber = "";
                if (dcNode.LegalEntities != null && dcNode.LegalEntities.Count == 1 && dcNode.LegalEntities[0].DebtCounsellorDetail != null)
                    ncrNumber = dcNode.LegalEntities[0].DebtCounsellorDetail.NCRDCRegistrationNumber;

                Assert.IsNotEmpty(ncrNumber);
            }
        }

        [Test]
        public void GetDebtCounsellorCompanyNodeForDebtCounselling()
        {
            using (new SessionScope())
            {
                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                // find latest debtcounselling record
                string sql = @"select top 1 DebtCounsellingKey from [2am].debtcounselling.DebtCounselling dc (nolock) order by 1 asc";
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    Assert.Fail("No data for GetDebtCounsellorCompanyNodeForDebtCounselling Test");

                int debtCounsellingKey = Convert.ToInt32(ds.Tables[0].Rows[0]["DebtCounsellingKey"]);

                // find the top level company node for the debt counsellor
                IDebtCounsellorOrganisationNode debtCounsellorOrganisationNode = repo.GetTopDebtCounsellorCompanyNodeForDebtCounselling(debtCounsellingKey);

                Assert.IsNotNull(debtCounsellorOrganisationNode);
            }
        }

        [Test]
        public void GetDebtCounsellorForDebtCounselling()
        {
            using (new SessionScope())
            {
                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                // find latest debtcounselling record with a Debt Counsellor external role
                string sql = @"select top 1 dc.DebtCounsellingKey from [2am].debtcounselling.DebtCounselling dc (nolock)
                                join [2am].dbo.ExternalRole er (nolock) on dc.DebtCounsellingKey = er.GenericKey
                                join [2am].dbo.LegalEntity le (nolock) ON er.LegalEntityKey = le.LegalEntityKey
                                join [2am].debtcounselling.DebtCounsellorDetail dcd (nolock) ON le.LegalEntityKey = dcd.LegalEntityKey
                                where er.GenericKeyTypeKey = " + (int)SAHL.Common.Globals.GenericKeyTypes.DebtCounselling2AM +
                                " and er.ExternalRoleTypeKey = " + (int)SAHL.Common.Globals.ExternalRoleTypes.DebtCounsellor +
                                " and er.GeneralStatusKey = " + (int)SAHL.Common.Globals.GeneralStatuses.Active +
                                " order by 1 desc";
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    Assert.Fail("No data for GetDebtCounsellorForDebtCounselling Test");

                int debtCounsellingKey = Convert.ToInt32(ds.Tables[0].Rows[0]["DebtCounsellingKey"]);

                // find the top level company node for the debt counsellor
                ILegalEntity debtCounsellor = repo.GetDebtCounsellorForDebtCounselling(debtCounsellingKey);

                Assert.IsNotNull(debtCounsellor);
            }
        }

        [Test]
        public void GetDebtCounsellorCompanyForDebtCounsellor()
        {
            using (new SessionScope())
            {
                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                // find latest debtcounselling record with a Natural Person, Debt Counsellor external role
                string sql = @"select top 1 le.LegalEntityKey from [2am].dbo.ExternalRole er (nolock)
                                join [2am].dbo.LegalEntity le (nolock) on er.LegalEntityKey = le.LegalEntityKey
                                where le.LegalEntityTypeKey = " + (int)SAHL.Common.Globals.LegalEntityTypes.NaturalPerson +
                                " and er.GenericKeyTypeKey = " + (int)SAHL.Common.Globals.GenericKeyTypes.DebtCounselling2AM +
                                " and er.ExternalRoleTypeKey = " + (int)SAHL.Common.Globals.ExternalRoleTypes.DebtCounsellor +
                                " order by 1 asc";
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    Assert.Fail("No data for GetDebtCounsellorCompanyForDebtCounsellor Test");

                int debtCounsellorKey = Convert.ToInt32(ds.Tables[0].Rows[0]["LegalEntityKey"]);

                // find the top level company node for the debt counsellor
                ILegalEntity debtCounsellor = repo.GetDebtCounsellorCompanyForDebtCounsellor(debtCounsellorKey);

                Assert.IsNotNull(debtCounsellor);
            }
        }

        [Test]
        public void GetDebtCounsellorOrganisationNodeForLegalEntity()
        {
            using (new SessionScope())
            {
                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                // find the debt counsellor on the latest debtcounselling record
                string sql = @"select top 1 er.LegalEntityKey from [2am].debtcounselling.DebtCounselling dc (nolock)
                                join [2am]..ExternalRole er (nolock) on er.GenericKey = dc.DebtCounsellingKey
                                where er.ExternalRoleTypeKey = 2
                                order by 1 desc";
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    Assert.Fail("No data for GetDebtCounsellorOrganisationNodeForLegalEntity Test");

                int legalEntityKey = Convert.ToInt32(ds.Tables[0].Rows[0]["LegalEntityKey"]);

                IDebtCounsellorOrganisationNode debtCounsellorOrganisationNode = repo.GetDebtCounsellorOrganisationNodeForLegalEntity(legalEntityKey);

                Assert.IsNotNull(debtCounsellorOrganisationNode);
            }
        }

        [Test]
        public void LoadBalanceAssign()
        {
            using (new SessionScope())
            {
                int workflowRoleTypeKey = 1;
                List<string> statesToDetermineLoad = new List<string>();
                statesToDetermineLoad.Add("Review Notification");
                statesToDetermineLoad.Add("Pend Proposal");

                IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();

                // get workflow
                IWorkFlow workFlow = x2Repo.GetWorkFlowByName("Debt Counselling", "Debt Counselling");

                // get a comma delimited list of state ids
                string stateIDs = "";
                foreach (string sName in statesToDetermineLoad)
                {
                    IState state = x2Repo.GetStateByName(workFlow.Name, workFlow.Process.Name, sName);
                    stateIDs += state.ID + ",";
                }

                // strip off last comma
                stateIDs = stateIDs.TrimEnd(',');

                string sql = UIStatementRepository.GetStatement("Client.Assignment.WorkflowRoleAssignment", "LoadBalanceAssign");
                ParameterCollection prms = new ParameterCollection();
                prms.Add(new SqlParameter("@CheckRoundRobinStatus", true));
                prms.Add(new SqlParameter("@IncludeStates", true));
                prms.Add(new SqlParameter("@WorkflowID", workFlow.ID));
                prms.Add(new SqlParameter("@GenericKeyTypeKey", (int)GenericKeyTypes.WorkflowRoleType));
                prms.Add(new SqlParameter("@WorkflowRoleTypeKey", workflowRoleTypeKey));
                prms.Add(new SqlParameter("@StateIDs", stateIDs));

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), prms);
                Assert.IsNotNull(ds);
            }
        }

        [Test]
        public void GetAllDebtCounsellorsForDebtCounselling()
        {
            using (new SessionScope())
            {
                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                // find latest debtcounselling record
                string sql = @"select top 1 DebtCounsellingKey
                                from [2am].debtcounselling.DebtCounselling dc (nolock)
                                join [2am].dbo.ExternalRole er (nolock) on dc.DebtCounsellingKey = er.GenericKey
                                join [2am].dbo.LegalEntity le (nolock) ON er.LegalEntityKey = le.LegalEntityKey
                                join [2am].debtcounselling.DebtCounsellorDetail dcd (nolock) ON le.LegalEntityKey = dcd.LegalEntityKey
                                where er.GenericKeyTypeKey = " + (int)SAHL.Common.Globals.GenericKeyTypes.DebtCounselling2AM +
                                " and er.ExternalRoleTypeKey = " + (int)SAHL.Common.Globals.ExternalRoleTypes.DebtCounsellor +
                                " order by 1 desc";
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    Assert.Fail("No data for GetAllDebtCounsellorsForDebtCounselling Test");

                int debtCounsellingKey = Convert.ToInt32(ds.Tables[0].Rows[0]["DebtCounsellingKey"]);

                IEventList<ILegalEntity> debtCounsellors = repo.GetAllDebtCounsellorsForDebtCounselling(debtCounsellingKey);

                Assert.IsNotNull(debtCounsellors);
                Assert.IsTrue(debtCounsellors.Count > 0);
            }
        }

        [Test]
        public void UpdateDebtCounsellingDebtReviewArrangementPassPost()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                string errorMessage = "";
                string user = @"SAHL\TestUser";

                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                // find some data
                string sql = @";
                                with cte as
                                (
                                select max(ArrearTransactionKey) as ArrearTransactionNumber, FinancialServiceKey
                                from [2am].fin.ArrearTransaction ar (nolock)
                                group by FinancialServiceKey
                                )

                                select top 10 fs.AccountKey, fs.FinancialServiceKey
                                from Account a (nolock)
                                join FinancialService fs (nolock) on a.AccountKey = fs.AccountKey
                                join cte on fs.FinancialServiceKey = cte.FinancialServiceKey
                                join [2am].fin.ArrearTransaction ar (nolock) on cte.ArrearTransactionNumber = ar.ArrearTransactionKey
                                where Balance > 10
                                and a.accountstatuskey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    Assert.Fail("No data for UpdateDebtCounsellingDebtReviewArangementTest Test");

                int accKey = Convert.ToInt32(ds.Tables[0].Rows[0]["AccountKey"]);
                int fsKey = Convert.ToInt32(ds.Tables[0].Rows[0]["FinancialServiceKey"]);

                bool res = repo.UpdateDebtCounsellingDebtReviewArrangement(accKey, user);

                Assert.AreEqual(res, true);
                Assert.AreEqual(errorMessage.Length, 0);
            }
        }

        [Test]
        public void UpdateDebtCounsellingDebtReviewArrangementPassNoPost()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                string errorMessage = "";
                string user = @"SAHL\TestUser";

                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                // find some data
                string sql = @";
                    with cte as
                    (
                    select max(ArrearTransactionKey) as ArrearTransactionkey, FinancialServiceKey as LoanNumber
                    from fin.ArrearTransaction ar (nolock)
                    group by FinancialServiceKey
                    )

                    select top 10 fs.AccountKey, fs.FinancialServiceKey--, *
                    from Account a (nolock)
                    join FinancialService fs (nolock) on a.AccountKey = fs.AccountKey
                    join cte on fs.FinancialServiceKey = cte.LoanNumber
                    join fin.ArrearTransaction ar (nolock) on cte.ArrearTransactionKey = ar.ArrearTransactionKey
                    where Balance < 0
                    and a.accountstatuskey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    Assert.Fail("No data for UpdateDebtCounsellingDebtReviewArangementTest Test");

                int accKey = Convert.ToInt32(ds.Tables[0].Rows[0]["AccountKey"]);
                int fsKey = Convert.ToInt32(ds.Tables[0].Rows[0]["FinancialServiceKey"]);

                bool res = repo.UpdateDebtCounsellingDebtReviewArrangement(accKey, user);

                Assert.AreEqual(res, true);
                Assert.AreEqual(errorMessage.Length, 0);
            }
        }

        [Test]
        public void UpdateDebtCounsellingDebtReviewArrangementClosedAccount()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                string errorMessage = "";
                string user = @"SAHL\TestUser";

                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                // find some data
                string sql = @";
                    select top 10 fs.AccountKey, fs.FinancialServiceKey--, *
                    from Account a (nolock)
                    join FinancialService fs (nolock) on a.AccountKey = fs.AccountKey
                    where a.accountstatuskey = 2";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    Assert.Fail("No data for UpdateDebtCounsellingDebtReviewArangementTest Test");

                int accKey = Convert.ToInt32(ds.Tables[0].Rows[0]["AccountKey"]);
                int fsKey = Convert.ToInt32(ds.Tables[0].Rows[0]["FinancialServiceKey"]);

                bool res = repo.UpdateDebtCounsellingDebtReviewArrangement(accKey, user);

                Assert.AreEqual(res, true);
                Assert.AreEqual(errorMessage.Length, 0);
            }
        }

        //[Ignore]
        //[Test]
        //public void CancelDebtCounselling()
        //{
        //    int debtCounsellingKey = 1102;
        //    string errorMessage  = null;

        //    IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
        //    ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

        //    using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
        //    {
        //        using (Castle.ActiveRecord.TransactionScope ts = new Castle.ActiveRecord.TransactionScope(TransactionMode.Inherits))
        //        {
        //            try
        //            {
        //                SAHL.Common.BusinessModel.Interfaces.IDebtCounselling debtCounselling = repo.GetDebtCounsellingByKey(debtCounsellingKey);

        //                // only call this sp if there is an accepted proposal
        //                List<SAHL.Common.BusinessModel.Interfaces.IProposal> activeProposals = repo.GetProposalsByTypeAndStatus(debtCounselling.Key, ProposalTypes.Proposal, ProposalStatuses.Active);
        //                bool acceptedPropsalExists = false;
        //                foreach (var proposal in activeProposals)
        //                {
        //                    if (proposal.Accepted == true)
        //                    {
        //                        acceptedPropsalExists = true;
        //                        break;
        //                    }
        //                }

        //                if (acceptedPropsalExists == true)
        //                    repo.CancelDebtCounselling(debtCounselling, out errorMessage);

        //                //ts.VoteCommit();

        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.ToString());
        //                //ts.VoteRollBack();
        //            }
        //        }
        //    //    //scope.Complete();
        //    }

        //    Assert.IsNotNull(errorMessage);
        //    Assert.IsTrue(errorMessage == "This Account is not in Debt Counselling");
        //}

        [Test]
        public void GetActiveDebtCounsellingUser()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 dc.DebtCounsellingKey, wrt.WorkflowRoleTypeKey, ad.ADUserKey
                                from debtcounselling.DebtCounselling dc
                                join dbo.WorkflowRole wr on wr.GenericKey = dc.DebtCounsellingKey
                                join dbo.WorkflowRoleType wrt on wrt.WorkflowRoleTypeKey = wr.WorkflowRoleTypeKey
                                join dbo.ADUser ad on ad.LegalEntityKey = wr.LegalEntityKey
                                where
                                wrt.description like '%Debt Counselling %'
                                and wr.GeneralStatusKey = 1";

                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1, "No data for test");
                if (DT == null || DT.Rows.Count == 0)
                    return;

                int DebtCounsellingKey = Convert.ToInt32(DT.Rows[0][0]);
                int WorkflowRoleTypeKey = Convert.ToInt32(DT.Rows[0][1]);
                int ADUserKey = Convert.ToInt32(DT.Rows[0][2]);

                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                IADUser adUser = repo.GetActiveDebtCounsellingUser(DebtCounsellingKey, (WorkflowRoleTypes)WorkflowRoleTypeKey);

                Assert.That(adUser.Key == ADUserKey);
            }
        }

        [Test]
        public void SaveDebtCounselling()
        {
            //this keeps failing becuase of dmsgs, only fails when something else fails.
            //just making sure it is not failing as a result of previous failures
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Clear();

            using (new TransactionScope(OnDispose.Rollback))
            {
                int key = DebtCounselling_DAO.FindFirst().Key;
                IDebtCounselling busObj = _repo.GetDebtCounsellingByKey(key);
                _repo.SaveDebtCounselling(busObj);
            }
        }

        [Test]
        public void CheckHearingDetailExistsForDebtCounsellingKey_Pass()
        {
            string sql = @"select top 1 DebtCounsellingKey, HearingTypeKey, HearingAppearanceTypeKey
                        from [2am].debtcounselling.HearingDetail (nolock)
                        where GeneralStatusKey = 1";

            DataTable DT = base.GetQueryResults(sql);

            Assert.That(DT.Rows.Count == 1, "No data for test");
            if (DT == null || DT.Rows.Count == 0)
                return;

            int dcKey = Convert.ToInt32(DT.Rows[0][0]);
            int hearingTypeKey = Convert.ToInt32(DT.Rows[0][1]);
            int hearingAppearanceTypeKey = Convert.ToInt32(DT.Rows[0][2]);
            bool exists = _repo.CheckHearingDetailExistsForDebtCounsellingKey(dcKey, hearingTypeKey, new List<int>() { hearingAppearanceTypeKey });

            Assert.That(exists);
        }

        [Test]
        public void CheckHearingDetailExistsForDebtCounsellingKey_Fail()
        {
            string sql = @"select top 1 DebtCounsellingKey
                        from [2am].debtcounselling.DebtCounselling (nolock)
                        where DebtCounsellingKey not in (
				            select DebtCounsellingKey
                            from [2am].debtcounselling.HearingDetail (nolock)
                            )";

            DataTable DT = base.GetQueryResults(sql);
            int dcKey = Convert.ToInt32(DT.Rows[0][0]);
            int hearingTypeKey = 1;
            int hearingAppearanceTypeKey = 1;
            bool exists = _repo.CheckHearingDetailExistsForDebtCounsellingKey(dcKey, hearingTypeKey, new List<int>() { hearingAppearanceTypeKey });

            Assert.That(exists == false);
        }

        [Test]
        public void GetRelatedDebtCounsellingGroupForLegalEntities_Pass()
        {
            string sql = @"select top 100 dcg.DebtCounsellingGroupKey, er.LegalEntityKey
                        from debtcounselling.DebtCounsellingGroup dcg  (nolock)
                        join debtcounselling.DebtCounselling dc (nolock)
	                        on dc.DebtCounsellingGroupKey = dcg.DebtCounsellingGroupKey
                        join ExternalRole er (nolock) on er.GenericKey = dc.DebtCounsellingKey
                        where er.GenericKeyTypeKey = 27
                        and er.ExternalRoleTypeKey = 1
                        and dcg.DebtCounsellingGroupKey > 1
                        order by dcg.DebtCounsellingGroupKey";
            DataTable DT = base.GetQueryResults(sql);

            Assert.That(DT.Rows.Count > 1, "Not enough data for test");
            if (DT.Rows.Count < 100)
                return;

            int groupKey1 = -1;
            int groupKey2 = -1;
            List<int> leKeys = new List<int>();

            foreach (DataRow row in DT.Rows)
            {
                int groupKey = Convert.ToInt32(row[0]);

                if (groupKey2 > -1 && groupKey != groupKey2)
                    break;

                if (groupKey1 == -1)
                    groupKey1 = groupKey;
                else if (groupKey != groupKey1 && groupKey2 == -1)
                    groupKey2 = groupKey;

                int leKey = Convert.ToInt32(row[1]);

                if (!leKeys.Contains(leKey))
                    leKeys.Add(leKey);
            }

            IList<IDebtCounsellingGroup> groups = _repo.GetRelatedDebtCounsellingGroupForLegalEntities(leKeys);

            Assert.That(groups.Count > 0);
            Assert.That(groups.Where(x => x.Key == groupKey1).Any());
            Assert.That(groups.Where(x => x.Key == groupKey2).Any());
        }

        [Test]
        public void GetRelatedDebtCounsellingGroupForLegalEntities_Fail()
        {
            string sql = @"select top 2 LegalEntityKey
                            from dbo.LegalEntity (nolock)
                            where LegalEntityKey not in (select LegalEntityKey from ExternalRole (nolock))";
            DataTable DT = base.GetQueryResults(sql);

            List<int> leKeys = new List<int>();

            foreach (DataRow row in DT.Rows)
            {
                int key = Convert.ToInt32(row[0]);
                leKeys.Add(key);
            }

            IList<IDebtCounsellingGroup> groups = _repo.GetRelatedDebtCounsellingGroupForLegalEntities(leKeys);

            Assert.That(groups == null || groups.Count == 0);
        }

        [Test]
        public void CancelDebtCounselling_Pass()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                string sql = @"select top 1 dc.DebtCounsellingKey
                            from debtcounselling.DebtCounselling dc (nolock)
                            join debtcounselling.Proposal p (nolock) on p.DebtCounsellingKey = dc.DebtCounsellingKey
                            join dbo.Account a (nolock) on a.AccountKey = dc.AccountKey
							join spv.SPV spv (nolock) on a.SPVKey = spv.SPVKey
                            join dbo.FinancialService fs (nolock) on fs.AccountKey = dc.AccountKey
							join fin.FinancialAdjustment fa (nolock) on fs.FinancialServiceKey = fa.FinancialServiceKey
							join fin.FinancialAdjustmentSource fas (nolock) on fa.FinancialAdjustmentSourcekey = fas.FinancialAdjustmentSourceKey
							join fin.FinancialAdjustmentTypeSource fats	(nolock) on fas.FinancialAdjustmentSourcekey = fats.FinancialAdjustmentSourcekey
							where dc.DebtCounsellingStatusKey = 1 -- open
							and spv.ParentSPVKey not in (38, 40, 42,44)
                            and p.ProposalStatusKey = 1
                            and p.ProposalTypeKey = 1
                            and p.Accepted = 1
                            and fs.FinancialServiceTypeKey = 1
                            and fa.FinancialAdjustmentStatusKey = 1
                            and fats.FinancialAdjustmentTypeSourceKey in (15, 16, 17, 18, 19, 20) order by fs.accountkey desc";

                //ParentSPVKey Not IN (38, 40, 42,44)

                DataTable DT = base.GetQueryResults(sql);

                if (DT != null && DT.Rows.Count > 0)
                {
                    int dcKey = Convert.ToInt32(DT.Rows[0][0]);
                    IDebtCounselling dc = new DebtCounselling(DebtCounselling_DAO.Find(dcKey));
                    try
                    {
                        _repo.CancelDebtCounselling(dc, "Test", SAHL.Common.Globals.DebtCounsellingStatuses.Cancelled);
                    }
                    catch (DomainValidationException ex)
                    {
                        Assert.Fail(String.Format("DCKey is {0}; Error was {1}", dcKey, ex.Message));
                    }
                }
            }
        }

        [Test]
        public void CancelDebtCounselling_Fail()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                string sql = @"select top 1 dc.DebtCounsellingKey
                            from debtcounselling.DebtCounselling dc (nolock)
                            join debtcounselling.Proposal p (nolock) on p.DebtCounsellingKey = dc.DebtCounsellingKey
                            join dbo.FinancialService fs (nolock) on fs.AccountKey = dc.AccountKey

							join fin.FinancialAdjustment fa (nolock) on fs.FinancialServiceKey = fa.FinancialServiceKey
							join fin.FinancialAdjustmentSource fas (nolock) on fa.FinancialAdjustmentSourcekey = fas.FinancialAdjustmentSourceKey
							join fin.FinancialAdjustmentTypeSource fats	(nolock) on fas.FinancialAdjustmentSourcekey = fats.FinancialAdjustmentSourcekey
							where dc.DebtCounsellingStatusKey = 1 -- open
                            and p.ProposalStatusKey = 1
                            and p.ProposalTypeKey = 1
                            and p.Accepted = 1
                            and fs.FinancialServiceTypeKey = 1
                            and fa.FinancialAdjustmentStatusKey = 1
                            and fats.FinancialAdjustmentTypeSourceKey in (15, 16, 17, 18, 19, 20)";
                DataTable DT = base.GetQueryResults(sql);

                if (DT != null && DT.Rows.Count > 0)
                {
                    int dcKey = Convert.ToInt32(DT.Rows[0][0]);
                    IDebtCounselling dc = new DebtCounselling(DebtCounselling_DAO.Find(dcKey));
                    try
                    {
                        _repo.CancelDebtCounselling(dc, "Test", SAHL.Common.Globals.DebtCounsellingStatuses.Cancelled);
                    }
                    catch (DomainValidationException ex)
                    {
                        Assert.Pass("The Test was succesfull");
                    }
                }
            }
        }

        [Test]
        public void GetPostDebtCounsellingMortgageLoanInstallment()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                IDebtCounsellingRepository debtCounsellingRepository = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                // find some data
                string sql = @";select top 1 DebtCounsellingKey from [2am].DebtCounselling.DebtCounselling where DebtCounsellingStatusKey = 1;";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    Assert.Fail("No data for UpdateDebtCounsellingDebtReviewArangementTest Test");

                int debtCounsellingKey = Convert.ToInt32(ds.Tables[0].Rows[0]["DebtCounsellingKey"]);
                double preDCInstalment, linkRate, marketRate, interestRate;
                int term;
                debtCounsellingRepository.GetPostDebtCounsellingMortgageLoanInstallment(debtCounsellingKey, out preDCInstalment, out linkRate, out marketRate, out interestRate, out term);

                Assert.That(preDCInstalment >= 0);
                Assert.That(linkRate >= 0);
                Assert.That(marketRate >= 0);
                Assert.That(interestRate >= 0);
                Assert.That(term >= 0);
            }
        }

        [Test]
        public void GetHearingAppearanceTypeByKeyTest()
        {
            using (new SessionScope())
            {
                string query = "Select top 1 h.hearingAppearanceTypeKey from [2am].debtcounselling.hearingAppearanceType h";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);
                int key = Convert.ToInt32(DT.Rows[0][0]);

                IHearingAppearanceType busObj = _repo.GetHearingAppearanceTypeByKey(key);
                Assert.IsNotNull(busObj);
                Assert.That(busObj.Key == key);
            }
        }

        [Test]
        public void GetHearingTypeByKeyTest()
        {
            using (new SessionScope())
            {
                string query = "Select top 1 h.hearingTypeKey from [2am].debtcounselling.hearingType h";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                int key = Convert.ToInt32(DT.Rows[0][0]);
                IHearingType busObj = _repo.GetHearingTypeByKey(key);
                Assert.IsNotNull(busObj);
                Assert.That(busObj.Key == key);
            }
        }

        [Test]
        public void GetCourtByKeyTest()
        {
            using (new SessionScope())
            {
                string query = "select top 1 courtKey from [2am].debtcounselling.court";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);
                int key = Convert.ToInt32(DT.Rows[0][0]);

                var court = _repo.GetCourtByKey(key);
                Assert.IsNotNull(court);
                Assert.That(court.Key == key);
            }
        }

        [Test]
        public void GetCourtTypeByKeyTest()
        {
            using (new SessionScope())
            {
                string query = "select top 1 courtTypeKey from [2am].debtcounselling.courtType";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);
                int key = Convert.ToInt32(DT.Rows[0][0]);

                var courtType = _repo.GetCourtTypeByKey(key);
                Assert.IsNotNull(courtType);
                Assert.That(courtType.Key == key);
            }
        }

        [Test]
        public void CreateEmptyHearingDetailTest()
        {
            using (new SessionScope())
            {
                IHearingDetail hearingDetail = _repo.CreateEmptyHearingDetail();
                Assert.IsNotNull(hearingDetail);
            }
        }

        [Test]
        public void CreateEmptyDebtCounsellorDetailTest()
        {
            using (new SessionScope())
            {
                IDebtCounsellorDetail debtCounsellorDetail = _repo.CreateEmptyDebtCounsellorDetail();
                Assert.IsNotNull(debtCounsellorDetail);
            }
        }

        [Test]
        public void GetDebtCounsellingGroupByKeyTest()
        {
            using (new SessionScope())
            {
                string query = "select top 1 debtCounsellingGroupKey from [2am].debtcounselling.debtCounsellingGroup";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);
                int key = Convert.ToInt32(DT.Rows[0][0]);

                var debtCounsellingGroup = _repo.GetDebtCounsellingGroupByKey(key);
                Assert.IsNotNull(debtCounsellingGroup);
                Assert.That(debtCounsellingGroup.Key == key);
            }
        }

        [Test]
        public void GetDebtCounsellingByAccountKeyTest()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 accountkey ,Count(*) as ListCount
                                from [2am].debtcounselling.debtcounselling (nolock)
                                group by accountkey";

                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count > 0);
                int key = Convert.ToInt32(DT.Rows[0][0]);
                int rowcount = Convert.ToInt32(DT.Rows[0][1]);
                List<IDebtCounselling> debtCounsellingList = _repo.GetDebtCounsellingByAccountKey(key);
                Assert.IsNotNull(debtCounsellingList, "List is null");
                Assert.That(debtCounsellingList.Count == rowcount, "List Item Count mismatch with query");
            }
        }

        private void GetDebtCounsellingByAccountKeyAndStatusHelper(DebtCounsellingStatuses dcstatus)
        {
            using (new SessionScope())
            {
                string query = string.Format(@"select top 1 accountkey ,Count(*) as ListCount
                                from [2am].debtcounselling.debtcounselling (nolock)
                                Where debtcounsellingStatusKey ={0}
                                group by accountkey,debtcounsellingStatusKey
                               ", (int)dcstatus);

                //Test will only run if there is data
                DataTable DT = base.GetQueryResults(query);
                if (DT.Rows.Count > 0)
                {
                    int key = Convert.ToInt32(DT.Rows[0][0]);
                    int rowcount = Convert.ToInt32(DT.Rows[0][1]);
                    List<IDebtCounselling> debtCounsellingList = _repo.GetDebtCounsellingByAccountKey(key, dcstatus);
                    Assert.IsNotNull(debtCounsellingList, "List is null for Status test" + dcstatus.ToString());
                    Assert.That(debtCounsellingList.Count == rowcount, "List Item Count mismatch with query using Status " + dcstatus.ToString());
                }
            }
        }

        [Test]
        public void GetDebtCounsellingByAccountKeyAndStatus()
        {
            GetDebtCounsellingByAccountKeyAndStatusHelper(DebtCounsellingStatuses.Open);

            GetDebtCounsellingByAccountKeyAndStatusHelper(DebtCounsellingStatuses.Closed);

            GetDebtCounsellingByAccountKeyAndStatusHelper(DebtCounsellingStatuses.Cancelled);

            GetDebtCounsellingByAccountKeyAndStatusHelper(DebtCounsellingStatuses.Terminated);
        }

        [Test]
        public void GetLitigationAttorneysTest()
        {
            //Not sure what value this test is. It provides coverage.
            IDictionary<int, string> attorneys = _repo.GetLitigationAttorneys();

            Assert.IsNotNull(attorneys, "Null object returned");
        }

        [Test]
        public void SearchDebtCounsellingCasesForAttorneyGivenNoCriteria()
        {
            var attorneyQuery = @" select top 1 ER.LegalEntityKey
                                    from [2AM].dbo.ExternalRole ER
                                    join [2AM].dbo.Attorney att on att.AttorneyKey = ER.GenericKey and ER.GenericKeyTypeKey = 35
                                    order by 1 asc";
            DataTable DT = base.GetQueryResults(attorneyQuery);

            Assert.That(DT.Rows.Count > 0, "No data for test");
            if (DT == null || DT.Rows.Count == 0)
                return;

            int attorneyLegalEntityKey = Convert.ToInt32(DT.Rows[0][0]);

            List<IDebtCounselling> debtCounsellingList = _repo.SearchDebtCounsellingCasesForAttorney(attorneyLegalEntityKey, "", 0, "");

            Assert.AreEqual(0, debtCounsellingList.Count, "Expected Search to return no results with no search criteria");
        }

        [Test]
        public void RaiseActiveExternalActivityForAddDetailTypeTest()
        {
            using (new SessionScope())
            {
                //IDebtCounsellingRepository dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                SetRepositoryStrategy(TypeFactoryStrategy.Mock);

                IX2Repository x2Repo = _mockery.StrictMock<IX2Repository>();
                MockCache.Add(typeof(IX2Repository).ToString(), x2Repo);

                IAccountRepository accRepo = _mockery.StrictMock<IAccountRepository>();
                MockCache.Add(typeof(IAccountRepository).ToString(), accRepo);

                IAccount account = _mockery.StrictMock<IAccount>();
                SetupResult.For(account.UnderDebtCounselling).Return(true);

                long instanceID = 1;
                IInstance instance = _mockery.StrictMock<IInstance>();
                SetupResult.For(instance.ID).Return(instanceID);

                int accountKey = 1;
                int detailTypeKey = (int)DetailTypes.UnderCancellation;

                string ExtActivityName = "test";
                string workflowName = "test";
                string processName = "test";
                string XMLFieldInputs = string.Empty;

                Expect.Call(delegate { x2Repo.CreateAndSaveActiveExternalActivity(ExtActivityName, instanceID, workflowName, processName, XMLFieldInputs); }).IgnoreArguments();
                Expect.Call(x2Repo.GetDebtCousellingInstanceByAccountKey(accountKey)).Return(instance).IgnoreArguments();
                Expect.Call(accRepo.GetAccountByKey(accountKey)).Return(account).IgnoreArguments();

                _mockery.ReplayAll();

                IDebtCounsellingRepository dcRepo = new DebtCounsellingRepository(new CastleTransactionsService(), accRepo);
                dcRepo.RaiseActiveExternalActivityForAddDetailType(accountKey, detailTypeKey);

                SetRepositoryStrategy(TypeFactoryStrategy.Default);
            }
        }

        [Test]
        public void RaiseActiveExternalActivityForDeleteDetailTypeTest()
        {
            using (new SessionScope())
            {
                SetRepositoryStrategy(TypeFactoryStrategy.Mock);

                IX2Repository x2Repo = _mockery.StrictMock<IX2Repository>();
                MockCache.Add(typeof(IX2Repository).ToString(), x2Repo);

                long instanceID = 1;

                IInstance instance = _mockery.StrictMock<IInstance>();
                SetupResult.For(instance.ID).Return(instanceID);

                IAccountRepository accRepo = _mockery.StrictMock<IAccountRepository>();
                MockCache.Add(typeof(IAccountRepository).ToString(), accRepo);

                IAccount account = _mockery.StrictMock<IAccount>();
                SetupResult.For(account.UnderDebtCounselling).Return(true);

                int accountKey = 1;
                int detailTypeKey = (int)DetailTypes.UnderCancellation;

                string ExtActivityName = "test";
                string workflowName = "test";
                string processName = "test";
                string XMLFieldInputs = string.Empty;

                Expect.Call(delegate { x2Repo.CreateAndSaveActiveExternalActivity(ExtActivityName, instanceID, workflowName, processName, XMLFieldInputs); }).IgnoreArguments();
                Expect.Call(x2Repo.GetDebtCousellingInstanceByAccountKey(accountKey)).Return(instance).IgnoreArguments();
                Expect.Call(accRepo.GetAccountByKey(accountKey)).Return(account).IgnoreArguments();
                _mockery.ReplayAll();

                IDebtCounsellingRepository dcRepo = new DebtCounsellingRepository(new CastleTransactionsService(), accRepo);
                dcRepo.RaiseActiveExternalActivityForDeleteDetailType(accountKey, detailTypeKey);

                SetRepositoryStrategy(TypeFactoryStrategy.Default);
            }
        }

        [Test]
        public void GetDebtCounsellingByLegalEntityKeyTest()
        {
            string sql = @"select top 1 er.LegalEntityKey
                        from [2am].DebtCounselling.DebtCounselling dc (nolock)
                        join [2am].dbo.ExternalRole er (nolock) on er.GenericKey = dc.DebtCounsellingKey
                                and er.GenericKeyTypeKey = 27
                                and er.ExternalRoleTypeKey in (1)
                                and er.GeneralStatusKey = 1
                        join [2am].dbo.[Role] r (nolock) on r.LegalEntityKey = er.LegalEntityKey
                                and r.AccountKey = dc.AccountKey
                                and r.RoleTypeKey in (2,3)
                                and r.GeneralStatusKey = 1
                        where dc.DebtCounsellingStatusKey = 1
                        order by DebtCounsellingKey desc";
            DataTable DT = base.GetQueryResults(sql);

            int legalEntityKey = 0;

            foreach (DataRow row in DT.Rows)
            {
                legalEntityKey = Convert.ToInt32(row[0]);
            }

            if (legalEntityKey > 0)
            {
                IList<IDebtCounselling> dc = _repo.GetDebtCounsellingByLegalEntityKey(
                    GenericKeyTypes.DebtCounselling2AM, new List<int> { (int)ExternalRoleTypes.Client },
                    GeneralStatuses.Active, new List<int> { (int)RoleTypes.MainApplicant, (int)RoleTypes.Suretor },
                    GeneralStatuses.Active, DebtCounsellingStatuses.Open, legalEntityKey);

                Assert.That(dc.Count > 0);
            }
        }

        [Test]
        public void GetRemainingTermPriorToProposalAcceptance_Pass()
        {
            using (new SessionScope())
            {
                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                string sql = @"select distinct top 1 dc.DebtCounsellingKey
                            from [2AM].debtcounselling.DebtCounselling dc (nolock)
                            join [2AM].dbo.StageTransition st (nolock) on st.GenericKey = dc.DebtCounsellingKey
                            join [2AM].dbo.Reason r (nolock) on r.StageTransitionKey = st.StageTransitionKey
                            join [2AM].debtcounselling.Proposal p (nolock) on p.ProposalKey = r.GenericKey
	                        join [2AM].debtcounselling.SnapShotAccount ssa (nolock) on dc.AccountKey = ssa.AccountKey
                            where
                            dc.DebtCounsellingStatusKey = 1
                            and st.StageDefinitionStageDefinitionGroupKey = 4405
                            and p.Accepted = 1
                            and p.ProposalStatusKey = 1
                            order by dc.DebtCounsellingKey desc";

                DataTable DT = base.GetQueryResults(sql);

                if (DT.Rows.Count == 0)
                    return;

                int dcKey = Convert.ToInt32(DT.Rows[0][0]);
                int term = repo.GetRemainingTermPriorToProposalAcceptance(dcKey);

                Assert.That(term > -1);
            }
        }

        public void GetRemainingTermPriorToProposalAcceptance_Fail()
        {
            using (new SessionScope())
            {
                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                string sql = @"select top 1 DebtCounsellingKey
                            from [2AM].debtcounselling.DebtCounselling (nolock)
                            where DebtCounsellingKey not in (
	                            select GenericKey from [2AM].dbo.StageTransition st (nolock)
	                            where st.StageDefinitionStageDefinitionGroupKey = 4405
                            )";

                DataTable DT = base.GetQueryResults(sql);

                if (DT.Rows.Count == 0)
                    return;

                int dcKey = Convert.ToInt32(DT.Rows[0][0]);
                int term = repo.GetRemainingTermPriorToProposalAcceptance(dcKey);

                Assert.That(term == -1);
            }
        }

        [Test]
        public void ProcessDebtCounsellingOptOutPass()
        {
            string user = "System";
            using (new TransactionScope(OnDispose.Rollback))
            {
                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                string sql = @"select
								dc.DebtCounsellingKey,
								dc.AccountKey
							from
										[2am].DebtCounselling.DebtCounselling dc
							join		[2am].dbo.Account acc on dc.AccountKey = acc.AccountKey and acc.SPVKey = 117
							join		[2am].dbo.FinancialService fs on fs.AccountKey = dc.AccountKey
							join		[2am].DebtCounselling.Proposal prop on prop.DebtCounsellingKey = dc.DebtCounsellingKey and prop.Accepted = 1
							left join	[2am].fin.financialadjustment fa on fa.FinancialServiceKey = fs.FinancialServiceKey and
										fs.FinancialServiceTypeKey in (1,2) and
										fa.FinancialAdjustmentSourceKey in (3) and
                                        dc.DebtCounsellingStatusKey = 1
							where fa.FinancialAdjustmentKey is not null";

                DataTable DT = base.GetQueryResults(sql);

                if (DT.Rows.Count == 0)
                    return;

                int debtCounsellingKey = Convert.ToInt32(DT.Rows[0][0]);
                int accountKey = Convert.ToInt32(DT.Rows[0][1]);
                try
                {
                    repo.ProcessDebtCounsellingOptOut(accountKey, user);
                }
                catch (Exception ex)
                {
                    Assert.Fail(String.Format("DCKey: {1}, AccKey: {2}, Message: {0}", ex.Message, debtCounsellingKey, accountKey));
                }
            }
        }

        [Test]
        public void RollbackTransactionTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                string sql = @"select top 1 dc.DebtCounsellingKey
                    from [2am].debtcounselling.DebtCounselling dc (nolock)
                    join [2am]..FinancialService fs (nolock) on fs.AccountKey = dc.AccountKey
	                    and fs.FinancialServiceTypeKey = 1
	                    and fs.AccountStatusKey = 1
                    join [2am]..FinancialService ar_fs (nolock) on ar_fs.ParentFinancialServiceKey = fs.FinancialServiceKey
	                    and ar_fs.FinancialServiceTypeKey = 8
	                    and ar_fs.AccountStatusKey = 1
                    where dc.DebtCounsellingStatusKey = 1
                    and dc.DebtCounsellingKey in (select DebtCounsellingKey
								                    from [2am].debtcounselling.Proposal (nolock)
								                    where ProposalStatusKey = 1
								                    and Accepted = 1)
                    and dc.DebtCounsellingKey in (select st.GenericKey
								                    from [2am].dbo.StageTransition st (nolock)
								                    join [2am].dbo.StageDefinitionStageDefinitionGroup sdsdg (nolock) on sdsdg.StageDefinitionStageDefinitionGroupKey = st.StageDefinitionStageDefinitionGroupKey
									                    and sdsdg.StageDefinitionStageDefinitionGroupKey = 4434 -- Payment in Order
								                    join [2am].dbo.StageDefinition sd (nolock) on sd.StageDefinitionKey = sdsdg.StageDefinitionKey
								                    join [2am].dbo.StageDefinitionGroup sdg (nolock) on sdg.StageDefinitionGroupKey = sdsdg.StageDefinitionGroupKey
									                    and sdg.GenericKeyTypeKey = 27
									                    and sdg.GeneralStatusKey = 1)
                    and ar_fs.FinancialServiceKey in (select FinancialServiceKey
									                    from [2am].fin.ArrearTransaction (nolock)
									                    where TransactionTypeKey = 972)
                    and ar_fs.FinancialServiceKey not in (select FinancialServiceKey
									                    from [2am].fin.ArrearTransaction (nolock)
									                    where TransactionTypeKey = 973);";

                DataTable DT = base.GetQueryResults(sql);

                if (DT != null && DT.Rows.Count > 0)
                {
                    int dcKey = Convert.ToInt32(DT.Rows[0][0]);

                    try
                    {
                        _repo.RollbackTransaction(dcKey);
                    }
                    catch
                    {
                        Assert.Fail("RollbackTransaction failed");
                    }
                }
            }
        }

        [Test]
        public void AmortisationScheduletest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                var amortisationSchedule = repo.GetAmortisationScheduleForProposalByKey(11593, 360);
            };
        }
    }
}