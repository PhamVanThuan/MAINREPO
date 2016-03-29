using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NHibernate.Criterion;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class AccountRepositoryTest : TestBase
    {
        private IAccountRepository _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

        [Test]
        public void RemoveNonNCACompliantAccountInformation()
        {
            string query = null;
            DataTable DT = null;
            int accountKey = -1;

            using (new SessionScope())
            {
                //get an account with NO accountinformations
                query = @"select top 1 accountkey from [2am].dbo.Account where accountkey not in
                                (select distinct accountkey from [2am].dbo.accountinformation where AccountInformationTypeKey <> 12)";
                DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);
                accountKey = Convert.ToInt32(DT.Rows[0][0]);

                //create an accountinformation
                query = String.Format("INSERT into [2AM].dbo.AccountInformation select 12, {0}, null, null, null; select SCOPE_IDENTITY()", accountKey);
                DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                //remove it
                _accRepo.RemoveNonNCACompliantAccountInformation(accountKey);
            }

            //and get the count
            query = String.Format("select * from [2AM].dbo.AccountInformation where accountkey = {0}", accountKey);
            DT = base.GetQueryResults(query);
            Assert.That(DT.Rows.Count == 0);
        }

        [Test]
        public void TitleDeedsOnFile()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 Loannumber from sahldb.dbo.vw_TitleDeedCheck";
                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(TitleDeedCheck_DAO), null);
                bool res = _accRepo.TitleDeedsOnFile(Convert.ToInt32(obj));
                Assert.That(res == true);

                sql = @"select max(AccountKey) as AccountKey from AccountSequence";
                obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
                res = _accRepo.TitleDeedsOnFile(Convert.ToInt32(obj));
                Assert.That(res == false);
            }
        }

        [Test, TestCaseSource(typeof(AccountRepositoryTest), "GetProducts")]
        public void CreateDiscriminatedAccount(Products product)
        {
            using (new SessionScope(FlushAction.Never))
            {
                IAccount acc = _accRepo.CreateDiscriminatedAccountByProduct((int)product);
                Assert.IsNotNull(acc);
            }
        }

        public IEnumerable<Products> GetProducts()
        {
            return new[] {  Products.VariableLoan, Products.VariFixLoan, Products.HomeOwnersCover,
                            Products.LifePolicy, Products.SuperLo, Products.DefendingDiscountRate,
                            Products.NewVariableLoan, Products.Edge };
        }

        [Test]
        public void CreateEmptyLifePolicyAccountTest()
        {
            using (new SessionScope(FlushAction.Never))
            {
                IAccountLifePolicy lifePolicy = _accRepo.CreateEmptyLifePolicyAccount();
                Assert.IsNotNull(lifePolicy);
            }
        }

        [Test]
        public void CreateEmptyDetailTest()
        {
            using (new SessionScope(FlushAction.Never))
            {
                IDetail detail = _accRepo.CreateEmptyDetail();
                Assert.IsNotNull(detail);
            }
        }

        [Test]
        public void CreateEmptyRoleTest()
        {
            using (new SessionScope(FlushAction.Never))
            {
                IRole role = _accRepo.CreateEmptyRole();
                Assert.IsNotNull(role);
            }
        }

        [Test]
        public void GetRoleByKeyTest()
        {
            int roleKey = 0;

            using (new SessionScope(FlushAction.Never))
            {
                Role_DAO r = Role_DAO.FindFirst();
                roleKey = r.Key;
            }

            using (new SessionScope(FlushAction.Never))
            {
                IRole r = _accRepo.GetRoleByKey(roleKey);
                Assert.IsNotNull(r);
            }
        }

        [Test]
        public void GetAccountSequenceByKeyTest()
        {
            int accSeqKey = 0;

            using (new SessionScope(FlushAction.Never))
            {
                AccountSequence_DAO a = AccountSequence_DAO.FindFirst();
                accSeqKey = a.Key;
            }

            using (new SessionScope(FlushAction.Never))
            {
                IAccountSequence accSeq = _accRepo.GetAccountSequenceByKey(accSeqKey);
                Assert.IsNotNull(accSeq);
            }
        }

        [Test]
        public void GetAccountStatusByKeyTest()
        {
            int accStatusKey = 0;

            using (new SessionScope(FlushAction.Never))
            {
                AccountStatus_DAO a = AccountStatus_DAO.FindFirst();
                accStatusKey = a.Key;
            }

            using (new SessionScope(FlushAction.Never))
            {
                IAccountStatus accStatus = _accRepo.GetAccountStatusByKey((AccountStatuses)accStatusKey);
                Assert.IsNotNull(accStatus);
            }
        }

        [Test]
        public void GetExpenseTypeByKeyTest()
        {
            int expTypeKey = 0;

            using (new SessionScope(FlushAction.Never))
            {
                ExpenseType_DAO et = ExpenseType_DAO.FindFirst();
                expTypeKey = et.Key;
            }

            using (new SessionScope(FlushAction.Never))
            {
                IExpenseType et = _accRepo.GetExpenseTypeByKey(expTypeKey);
                Assert.IsNotNull(et);
            }
        }

        [Test]
        public void DeleteDetail()
        {
            DataTable DT = null;
            string query = "";
            int key = 0;

            using (new SessionScope())
            {
                query = "declare @AccountKey int "
                    + "set @AccountKey = (select top 1 accountkey from [2AM].[dbo].[Account] WHERE RRR_ProductKey IS NOT NULL) "
                    + "INSERT INTO [2AM].[dbo].[Detail] "
                    + "VALUES (1, @AccountKey, getdate(), 0, 'test', null, 'test', getdate()) "
                    + "set @AccountKey = scope_identity() "
                    + "select @AccountKey";
                DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                key = Convert.ToInt32(DT.Rows[0][0]);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IDetail detail = BMTM.GetMappedType<IDetail>(Detail_DAO.Find(key));

                IAccountRepository repo = RepositoryFactory.GetRepository<IAccountRepository>();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                repo.DeleteDetail(detail);
            }

            query = "Select * from [2AM].[dbo].[Detail] where DetailKey = " + key.ToString();
            DT = base.GetQueryResults(query);

            Assert.That(DT.Rows.Count == 0);
        }

        [Test]
        public void GetDetailByAccountKey()
        {
            using (new SessionScope())
            {
                string query = @"SELECT top 1 D.AccountKey, COUNT(d.AccountKey)
                FROM   [2AM].[dbo].[Detail] D (NOLOCK)
                JOIN   [2AM].[dbo].[DetailType] DT (NOLOCK)  ON   D.[DetailTypeKey] = DT.[DetailTypeKey]
                JOIN   [2AM].[dbo].[DetailClass] DC (NOLOCK)  ON   DT.[DetailClassKey] = DC.[DetailClassKey]
                LEFT OUTER JOIN  [2AM].[dbo].[CancellationType] CT (NOLOCK)  ON   D.[LinkID] = CT.[CancellationTypeKey]
                LEFT OUTER JOIN  [SAHLDB].[dbo].[RegMail] R WITH (NOLOCK)  ON     D.AccountKey = R.LoanNumber  AND     (D.DetailTypeKey < 10      OR      D.DetailTypeKey IN (213, 214))
                LEFT OUTER JOIN  [2AM].[dbo].[Attorney] ATT WITH (NOLOCK)  ON     R.AttorneyNumber = ATT.AttorneyKey
                LEFT OUTER JOIN  [2AM].[dbo].[LegalEntity] le WITH (NOLOCK) ON	att.LegalEntityKey = le.LegalEntityKey
                WHERE   D.[DetailTypeKey] <> 10
                AND D.AccountKey NOT IN
                (SELECT distinct D.AccountKey
                FROM   [2AM].[dbo].[Detail] D (NOLOCK)
                JOIN   [2AM].[dbo].[Account] A (NOLOCK)  ON     D.AccountKey = A.AccountKey
                JOIN   [2AM].[dbo].[DetailType] DT (NOLOCK)  ON   D.[DetailTypeKey] = DT.[DetailTypeKey]
                JOIN   [2AM].[dbo].[DetailClass] DC (NOLOCK)  ON   DT.[DetailClassKey] = DC.[DetailClassKey]
                LEFT OUTER JOIN   [2AM].[dbo].[CancellationType] CT (NOLOCK)  ON   D.[LinkID] = CT.[CancellationTypeKey]
                WHERE   D.[DetailTypeKey] = 10   AND      A.[AccountStatusKey] = 2
                AND      CONVERT(CHAR(10), D.[DetailDate], 121) = CONVERT(CHAR(10), A.[CloseDate], 121))
                GROUP BY D.AccountKey";

                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                int AccountKey = Convert.ToInt32(DT.Rows[0][0]);
                int count = Convert.ToInt32(DT.Rows[0][1]);

                IAccountRepository repo = RepositoryFactory.GetRepository<IAccountRepository>();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                IReadOnlyEventList<IDetail> list = repo.GetDetailByAccountKey(AccountKey);

                Assert.That(list.Count == count);
            }
        }

        [Test]
        public void GetFurtherLoanDetails()
        {
            using (new SessionScope())
            {
                string controlTblQuery = "select top 1 * from [2AM].[dbo].control where ControlDescription like '%FurtherLendingDetailTypes%'";

                DataTable controlDT = base.GetQueryResults(controlTblQuery);
                Assert.That(controlDT.Rows.Count == 1);

                string query = "select top 1 d.DetailTypeKey,d.AccountKey "
                    + "from [2am].[dbo].[Detail] d "
                    + "where d.DetailTypeKey = " + controlDT.Rows[0].ItemArray[3].ToString().Split(',')[0].ToString();
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count > 0);

                int AccountKey = Convert.ToInt32(DT.Rows[0][1]);

                IAccountRepository repo = RepositoryFactory.GetRepository<IAccountRepository>();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                IReadOnlyEventList<IDetail> list = repo.GetAccountDetailForFurtherLending(AccountKey);

                Assert.That(list.Count > 0);
            }
        }

        [Test]
        public void GetDetailByAccountKeyAndDetailType()
        {
            using (new SessionScope())
            {
                string query = "select top 1 d.AccountKey, d.DetailTypeKey, count(d.AccountKey) "
                    + "from [2am].[dbo].[Detail] d group by d.AccountKey, d.DetailTypeKey "
                    + "order by count(d.AccountKey) desc";
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                int AccountKey = Convert.ToInt32(DT.Rows[0][0]);
                int DetailTypeKey = Convert.ToInt32(DT.Rows[0][1]);
                int count = Convert.ToInt32(DT.Rows[0][2]);

                IAccountRepository repo = RepositoryFactory.GetRepository<IAccountRepository>();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                IReadOnlyEventList<IDetail> list = repo.GetDetailByAccountKeyAndDetailType(AccountKey, DetailTypeKey);

                Assert.That(list.Count == count);
            }
        }

        [Test]
        public void GetLatestDetailByAccountKeyAndDetailType()
        {
            using (new SessionScope())
            {
                string query = "select top 1 * from [2am].[dbo].[Detail] d (nolock) order by DetailKey desc";
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                int detailKey = Convert.ToInt32(DT.Rows[0][0]);
                int detailTypeKey = Convert.ToInt32(DT.Rows[0][1]);
                int accountKey = Convert.ToInt32(DT.Rows[0][2]);

                IAccountRepository repo = RepositoryFactory.GetRepository<IAccountRepository>();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                IDetail detail = repo.GetLatestDetailByAccountKeyAndDetailType(accountKey, detailTypeKey);

                Assert.That(detail.Key == detailKey);
            }
        }

        [Test]
        public void GetNaturalPersonLegalEntitiesByRoleType()
        {
            using (new SessionScope())
            {
                IAccountRepository AR = RepositoryFactory.GetRepository<IAccountRepository>();
                IDomainMessageCollection Messages = new DomainMessageCollection();
                IAccount Account = AR.GetAccountByKey(1235891);
                IReadOnlyEventList<ILegalEntityNaturalPerson> People = Account.GetNaturalPersonLegalEntitiesByRoleType(null, new int[] { 1, 2, 3 });
            }
        }

        [Test]
        public void GetAccountByApplicationKey()
        {
            IAccountRepository AR = RepositoryFactory.GetRepository<IAccountRepository>();
            IDomainMessageCollection Messages = new DomainMessageCollection();
            Application_DAO application = Application_DAO.FindFirst(new ICriterion[] { NHibernate.Criterion.Restrictions.IsNotNull("Account") });
            IAccount Account = AR.GetAccountByApplicationKey(application.Key);
            Assert.IsNotNull(Account);
        }

        [Test]
        public void GetAccountByKey()
        {
            using (new SessionScope())
            {
                int accountKey = Convert.ToInt32(base.GetPrimaryKey("Account", "AccountKey", "RRR_ProductKey = 1"));
                IAccount acc1 = _accRepo.GetAccountByKey(accountKey);
                Assert.IsNotNull(acc1);

                IAccount acc2 = _accRepo.GetAccountByKey(-1);
                Assert.IsNull(acc2);
            }
        }

        [Test]
        public void GetLatestValuation()
        {
            using (new SessionScope())
            {
                StringBuilder SB = new StringBuilder();
                SB.Append("WITH temp AS (");
                SB.Append("SELECT top 1 A.AccountKey as AccountKey, max(V.ValuationKey) as ValuationKey from [2AM].[dbo].[Account] A (nolock) ");
                SB.Append("JOIN [2AM].[dbo].[FinancialService] FS (nolock) ON A.AccountKey = FS.AccountKey and FS.FinancialServiceTypeKey = 1 ");
                SB.Append("JOIN [2AM].[fin].[MortgageLoan] ML (nolock) ON ML.FinancialServiceKey = FS.FinancialServiceKey ");
                SB.Append("JOIN [2AM].[dbo].[Property] P (nolock) ON P.PropertyKey = ML.PropertyKey ");
                SB.Append("JOIN [2AM].[dbo].[Valuation] V (nolock) ON P.PropertyKey = V.PropertyKey ");
                SB.Append("WHERE A.AccountStatusKey in (1,5) ");
                SB.Append("AND FS.AccountStatusKey in (1,5) ");
                SB.Append("GROUP BY V.PropertyKey, A.AccountKey ORDER BY count(V.ValuationKey) desc ) ");
                SB.Append("SELECT t.AccountKey, V.ValuationAmount as LastValuationAmount, V.ValuationDate as LastValuationDate ");
                SB.Append("FROM temp t JOIN [2AM].[dbo].[Valuation] V (nolock) ON t.ValuationKey = V.ValuationKey");
                DataTable DT = base.GetQueryResults(SB.ToString());

                Assert.That(DT.Rows.Count == 1);
            }
        }

        [Test]
        public void SearchAccountsByKey()
        {
            IAccountRepository accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            using (new SessionScope())
            {
                IAccountSearchCriteria searchCriteria = new AccountSearchCriteria();
                searchCriteria.AccountKey = Account_DAO.FindFirst().Key;

                // perform the search using the repository method
                IEventList<SAHL.Common.BusinessModel.Interfaces.IAccount> accounts = accountRepo.SearchAccounts(searchCriteria, 50);

                Assert.IsTrue(accounts.Count == 1);
                Assert.IsTrue(accounts[0].Key == searchCriteria.AccountKey);
            }
        }

        [Test]
        public void GetBSscoreByAccountKey()
        {
            using (new SessionScope())
            {
                string query = "Select top 1 Accountkey from AccountBaselII";
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                int AccountKey = Convert.ToInt32(DT.Rows[0][0]);

                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                DataSet ds = accRepo.GetBehaviouralScore(AccountKey);

                Assert.IsNotNull(ds);
            }
        }

        [Test]
        public void SearchAccountsByName()
        {
            IAccountRepository accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            using (new SessionScope())
            {
                // find the first account with a role of naturalperson
                string HQL = "from Role_DAO R where R.LegalEntity.class = ?";
                SimpleQuery query = new SimpleQuery(typeof(SAHL.Common.BusinessModel.DAO.Role_DAO), HQL, (int)SAHL.Common.Globals.LegalEntityTypes.NaturalPerson);
                query.SetQueryRange(1); // select one record
                Role_DAO[] roles = Role_DAO.ExecuteQuery(query) as Role_DAO[];
                LegalEntityNaturalPerson_DAO person = roles[0].LegalEntity.As<LegalEntityNaturalPerson_DAO>();

                // find all the roles matching the name of the naturalperson found above
                HQL = "select distinct R from Role_DAO R where R.LegalEntity.FirstNames = ?";
                query = new SimpleQuery(typeof(SAHL.Common.BusinessModel.DAO.Role_DAO), HQL, person.FirstNames);

                roles = Role_DAO.ExecuteQuery(query) as Role_DAO[];
                int recsFound = roles.Length;

                // set the search criteria
                IAccountSearchCriteria searchCriteria = new AccountSearchCriteria();
                searchCriteria.Distinct = false;
                searchCriteria.ExactMatch = true;

                //searchCriteria.Surname = person.Surname;
                searchCriteria.FirstNames = person.FirstNames;

                //searchCriteria.Products.Add(Products.LifePolicy);
                //searchCriteria.Products.Add(Products.HomeOwnersCover);

                // perform the search using the repository method
                IEventList<SAHL.Common.BusinessModel.Interfaces.IAccount> accounts = accountRepo.SearchAccounts(searchCriteria, 50);

                // check the results
                Assert.IsTrue(accounts.Count == recsFound);
            }
        }

        [Test]
        public void ConvertStaffLoanTest()
        {
            string userID = "System";
            IAccountRepository accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            TransactionScope tx = new TransactionScope();
            string sql = string.Empty;
            sql = @"select top 1 a.*
                    from [2am].dbo.Account a
                    inner join [2am].dbo.detail d
                     on a.accountKey = d.AccountKey
                    where a.accountstatuskey in (1,5) and d.detailtypekey = 237 and d.AccountKey not in
                    (SELECT fs.AccountKey
					FROM [2am].fin.FinancialAdjustment fa with (nolock)
					INNER JOIN [2am].dbo.financialservice fs (nolock)
					ON fs.financialservicekey = fa.financialservicekey
					WHERE fs.accountstatuskey in (1,5) and fa.FinancialAdjustmentStatusKey in (1, 2) and fa.FinancialAdjustmentTypeKey = 2)
					order by a.AccountKey desc";
            try
            {
                SimpleQuery<Account_DAO> accQ = new SimpleQuery<Account_DAO>(QueryLanguage.Sql, sql);
                accQ.AddSqlReturnDefinition(typeof(Account_DAO), "a");
                Account_DAO[] res = accQ.Execute();

                if (res != null && res.Length > 0)
                {
                    accountRepo.ConvertStaffLoan(res[0].Key, userID);
                    tx.VoteRollBack();
                }
            }
            catch (Exception ex)
            {
                tx.VoteRollBack();
                Assert.Fail(ex.Message);
            }
            finally
            {
                tx.Dispose();
            }
        }

        [Test]
        public void UnConvertStaffLoanTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                int accKey = 0;

                string sql = @"SELECT top 1 a.AccountKey
							FROM [2am].fin.FinancialAdjustment fa with (nolock)
							inner join [2am].dbo.financialservice fs (nolock)
							on fs.financialservicekey = fa.financialservicekey
							inner join [2am].dbo.Account a
							on fs.accountKey = a.accountKey
							inner join [2am].dbo.Detail d
							on a.accountkey = d.accountkey and d.detailtypekey = 237
							WHERE a.accountstatuskey in (1,5)
							and fa.FinancialAdjustmentStatusKey = 1
							and fa.FinancialAdjustmentSourceKey = 5";
                try
                {
                    object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
                    accKey = (int)obj;

                    if (accKey > 0)
                    {
                        _accRepo.UnConvertStaffLoan(accKey, "System", CancellationReasons.Staffoptoutnolongeramemberofstaff);
                    }
                }
                catch (DomainValidationException domainException)
                {
                    Assert.Fail(String.Format(@"AccountKey was: {0}, DM was: {1}", accKey, domainException.Message));
                }
                catch (Exception ex)
                {
                    Assert.Fail(String.Format(@"AccountKey was: {0}, Exception was: {1}", accKey, ex.Message));
                }
            }
        }

        [Test]
        public void GetCurrentMonthsInArrearsTest()
        {
            try
            {
                string sql = @"select top 1 acc.* from [2am].[dbo].[Account] acc
                            where acc.AccountStatusKey = 1 order by AccountKey";
                SimpleQuery<Account_DAO> q = new SimpleQuery<Account_DAO>(QueryLanguage.Sql, sql);
                q.AddSqlReturnDefinition(typeof(Account_DAO), "acc");
                Account_DAO[] res = q.Execute();

                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                accRepo.GetCurrentMonthsInArrears(res[0].Key);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [Test]
        public void GetAccountByCap2InstanceID_NoCrash()
        {
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            long instanceID = 1;
            IAccount account = accRepo.GetAccountByCap2InstanceID(instanceID);

            Assert.IsTrue(1 == 1); //dummy assert here - we are just making sure the repo method doesnt crash
        }

        [Test]
        public void RemoveDetailByAccountKeyAndDetailTypeKey()
        {
            DataTable DT = null;
            string query = "";
            int accountKey;

            using (new SessionScope())
            {
                query = "declare @AccountKey int "
                    + "set @AccountKey = (select top 1 accountkey from [2AM].[dbo].[Account] where AccountStatusKey in (1,2) order by accountkey desc) "
                    + "INSERT INTO [2AM].[dbo].[Detail] "
                    + "VALUES (1, @AccountKey, getdate(), 0, 'test', null, 'test', getdate()) "
                    + "select @AccountKey";
                DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                accountKey = Convert.ToInt32(DT.Rows[0][0]);
                Assert.That(accountKey > 0);
            }

            IAccountRepository repo = RepositoryFactory.GetRepository<IAccountRepository>();

            using (new SessionScope())
            {
                TransactionScope tx = new TransactionScope();
                try
                {
                    repo.RemoveDetailByAccountKeyAndDetailTypeKey(accountKey, 1);
                    tx.VoteCommit();
                }
                catch (Exception)
                {
                    tx.VoteRollBack();
                    Assert.Fail();
                }
                finally
                {
                    tx.Dispose();
                }
            }

            query = "Select * from [2AM].[dbo].[Detail] where DetailTypeKey = 1 and AccountKey = " + accountKey.ToString();
            DT = base.GetQueryResults(query);

            Assert.That(DT.Rows.Count == 0);
        }

        [Test]
        public void GetRegentTest()
        {
            using (new SessionScope())
            {
                string testQuery = string.Format(@"select top 1 r.LoanNumber
                from [dbo].[Regent] (nolock) r
                where
                    r.RegentPolicyStatus = {0}", (int)SAHL.Common.Globals.RegentStatus.NewBusiness);

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(testQuery, typeof(Regent_DAO), new ParameterCollection());
                if (o != null)
                {
                    int AccountKey = Convert.ToInt32(o);
                    IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                    IRegent regent = accRepo.GetRegent(AccountKey, (int)SAHL.Common.Globals.RegentStatus.NewBusiness);
                    Assert.IsNotNull(regent);
                }
                else
                    Assert.Fail("No test data");
            }
        }

        [Test]
        public void GetAccountDetailForFurtherLending()
        {
            using (new SessionScope())
            {
                string testQuery = string.Format(@"select top 1 AccountKey from [2am].dbo.Detail");

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(testQuery, typeof(Account_DAO), new ParameterCollection());
                if (o != null)
                {
                    int AccountKey = Convert.ToInt32(o);
                    IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                    IReadOnlyEventList<IDetail> detail = accRepo.GetAccountDetailForFurtherLending(AccountKey);
                    Assert.IsNotNull(detail[0]);
                }
                else
                    Assert.Fail("No test data");
            }
        }

        [Test]
        public void CreateOfferRolesNotInAccountTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                string sql = @"select top 1 ofr.offerKey
                from [2am].[dbo].offer o (nolock)
                inner join [2am].[dbo].Account acc (nolock)
                    on o.accountKey = acc.accountKey
                inner join [2am].[dbo].offerRole ofr (nolock)
                    on ofr.offerKey = o.offerKey
                inner join [2am].[dbo].offerRoleType ort (nolock)
                    on ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey
                inner join [2am].[dbo].RoleType rt (nolock)
                    on ort.description = rt.description
                left join [2am].[dbo].role r (nolock)
                    on ofr.legalEntityKey = r.legalEntityKey and o.accountKey = r.accountKey and rt.RoleTypeKey = r.RoleTypeKey
                where ort.OfferRoleTypeGroupKey = 3 and r.LegalEntityKey is null";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                if (o != null)
                {
                    int beforeAddCount;
                    int afterAddCount;
                    DataTable dt = null;
                    IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                    IApplication application = appRepo.GetApplicationByKey(Convert.ToInt32(o));
                    beforeAddCount = appRepo.GetOfferRolesNotInAccount(application).Rows.Count;
                    _accRepo.CreateOfferRolesNotInAccount(application);
                    afterAddCount = appRepo.GetOfferRolesNotInAccount(application).Rows.Count;
                    Assert.IsTrue(beforeAddCount > 0);
                    Assert.IsTrue(afterAddCount == 0);
                }
            }
        }

        [Test]
        public void RemoveDetailByKey()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

                int msgCount = spc.DomainMessages.Count;

                string sql = @"select DetailKey from Detail (nolock) where DetailTypeKey = 596";
                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                int detKey = (int)obj;
                if (detKey > 0)
                {
                    _accRepo.RemoveDetailByKey(detKey);
                }

                //we need an assert of some description
                //this detail type should not load and domain messages
                Assert.AreEqual(msgCount, spc.DomainMessages.Count);
            }
        }

        [Test]
        public void SaveDetail()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                int msgCount = spc.DomainMessages.Count;

                string sql = @"select DetailKey, AccountKey from Detail (nolock) where DetailTypeKey = 596";
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int detKey = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                    int accKey = Convert.ToInt32(ds.Tables[0].Rows[0][1].ToString());

                    if (detKey > 0 && accKey > 0)
                    {
                        IReadOnlyEventList<IDetail> dtList = _accRepo.GetDetailByAccountKeyAndDetailType(accKey, 596);
                        if (dtList.Count > 0)
                        {
                            _accRepo.SaveDetail(dtList[0]);
                        }
                    }
                }

                //we need an assert of some description
                //this detail type should not load and domain messages
                Assert.AreEqual(msgCount, spc.DomainMessages.Count);
            }
        }

        [Test]
        public void AccountAttorneyInvoiceTests()
        {
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            int key = 0;
            int accKey = 1509295;

            using (new TransactionScope())
            {
                IAccountAttorneyInvoice aAI = accRepo.CreateEmptyAccountAttorneyInvoice();

                aAI.AccountKey = accKey;
                aAI.Amount = 50;
                aAI.VatAmount = 10;
                aAI.TotalAmount = aAI.Amount + aAI.VatAmount;
                aAI.AttorneyKey = 1;
                aAI.Comment = "some rubbish";
                aAI.InvoiceNumber = "INV:5089";
                aAI.InvoiceDate = DateTime.Now;
                aAI.ChangeDate = DateTime.Now;

                accRepo.SaveAccountAttorneyInvoice(aAI);

                key = aAI.Key;
                Assert.Greater(key, 0);
            }

            using (new TransactionScope())
            {
                //GetAccountAttorneyInvoiceByKey
                IAccountAttorneyInvoice aAI = accRepo.GetAccountAttorneyInvoiceByKey(key);
                Assert.IsNotNull(aAI);
                //GetAccountAttorneyInvoiceListByAccountKey
                IEventList<IAccountAttorneyInvoice> ListAAI = accRepo.GetAccountAttorneyInvoiceListByAccountKey(accKey);
                Assert.Greater(ListAAI.Count, 0);

                accRepo.DeleteAccountAttorneyInvoice(aAI);
            }
        }

        [Test]
        public void SaveAccountDebtSettlement()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                IAccountDebtSettlement busObj = new AccountDebtSettlement(AccountDebtSettlement_DAO.FindFirst());
                _accRepo.SaveAccountDebtSettlement(busObj);
            }
        }

        [Test]
        public void UpdateAccountNoChange()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                // get latest open account
                string query = "select top 1 AccountKey, AccountStatusKey, FixedPayment, UserID from [2am].[dbo].[Account] (nolock) where AccountStatusKey = 3 order by 1 desc";
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                // get origional values from account
                int accountKey = Convert.ToInt32(DT.Rows[0][0]);
                int origAccountStatusKey = Convert.ToInt32(DT.Rows[0][1]);
                float origFixedPayment = float.Parse(DT.Rows[0][2].ToString());
                string origUserID = Convert.ToString(DT.Rows[0][3]);

                // call the update method with null values to make sure values dont change
                accRepo.UpdateAccount(accountKey, origAccountStatusKey, origFixedPayment, origUserID);

                // get the account and check result
                query = string.Format(@"select top 1 AccountStatusKey, FixedPayment, UserID from [2am].[dbo].[Account] (nolock) where AccountKey = {0}", accountKey);
                DT = base.GetQueryResults(query);

                // get updated values from account
                int accountStatusKey = Convert.ToInt32(DT.Rows[0][0]);
                float fixedPayment = float.Parse(DT.Rows[0][1].ToString());
                string userID = Convert.ToString(DT.Rows[0][2]);

                // check the values havent changed
                Assert.AreEqual(origAccountStatusKey, accountStatusKey);
                Assert.AreEqual(origFixedPayment, fixedPayment);
                Assert.AreEqual(origUserID, userID);
            }
        }

        [Test]
        public void UpdateAccountWithChanges()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                // get latest open account
                string query = "select top 1 AccountKey, AccountStatusKey, FixedPayment, UserID from [2am].[dbo].[Account] (nolock) where AccountStatusKey = 3 order by 1 desc";
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                // get origional values from account
                int accountKey = Convert.ToInt32(DT.Rows[0][0]);
                int origAccountStatusKey = Convert.ToInt32(DT.Rows[0][1]);
                float origFixedPayment = float.Parse(DT.Rows[0][2].ToString());
                string origUserID = Convert.ToString(DT.Rows[0][3]);

                // call the update method with new values to make sure values change
                int newAccountStatusKey = (int)SAHL.Common.Globals.AccountStatuses.Closed;
                float newFixedPayment = origFixedPayment + 1; //Increment the fixed payment
                string newUserID = "System";

                accRepo.UpdateAccount(accountKey, newAccountStatusKey, newFixedPayment, newUserID);

                // get the account and check result
                query = string.Format(@"select top 1 AccountStatusKey, FixedPayment, UserID from [2am].[dbo].[Account] (nolock) where AccountKey = {0}", accountKey);
                DT = base.GetQueryResults(query);

                // get updated values from account
                int accountStatusKey = Convert.ToInt32(DT.Rows[0][0]);
                float fixedPayment = float.Parse(DT.Rows[0][1].ToString());
                string userID = Convert.ToString(DT.Rows[0][2]);

                // check the values have been updated
                Assert.AreEqual(accountStatusKey, newAccountStatusKey);
                Assert.AreEqual(fixedPayment, newFixedPayment);
                Assert.AreEqual(userID, newUserID);
            }
        }

        [Test]
        public void ReOpenAccountTest()
        {
            string query = @"select top 1 a.AccountKey from Account a
                                join FinancialService fs ON fs.AccountKey = a.AccountKey
                                where     a.AccountStatusKey = 2
                                     and fs.AccountStatusKey = 2
                                and ParentAccountKey is NULL
                                order by a.InsertedDate Desc";

            DataTable dt = base.GetQueryResults(query);

            Assert.That(dt.Rows.Count == 1);

            int closedAccountKey = Convert.ToInt32(dt.Rows[0][0]);
            string userID = "System";

            using (new TransactionScope(OnDispose.Rollback))
            {
                IAccountRepository accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                accountRepo.ReOpenAccount(closedAccountKey, userID);

                IAccount reopenedAccount = accountRepo.GetAccountByKey(closedAccountKey);

                Assert.IsTrue(reopenedAccount.AccountStatus.Key == (int)AccountStatuses.Open);
                Assert.IsNull(reopenedAccount.CloseDate);
            }
        }

        [Test]
        public void AccountPersonalLoan()
        {
            using (new SessionScope())
            {
                SetRepositoryStrategy(TypeFactoryStrategy.Default);
                var accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
                var accountPersonalLoan = accountRepository.GetAccountByKey(3125787);
            }
        }

        [Test]
        public void TestGetDetailByAccountKeyAndDetailClassKey()
        {
            string query = @"select top 1 d.AccountKey, d.DetailTypeKey, dt.DetailClassKey
                             from Detail d
                             join DetailType dt on d.DetailtypeKey = dt.DetailTypeKey";

            using (new SessionScope())
            {
                DataTable dt = base.GetQueryResults(query);

                int accountKey = Convert.ToInt32(dt.Rows[0][0]);
                int detailTypeKey = Convert.ToInt32(dt.Rows[0][1]);
                int detailClassKey = Convert.ToInt32(dt.Rows[0][2]);

                var accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();

                IReadOnlyEventList<IDetail> detailList = accountRepository.GetDetailByAccountKeyAndDetailClassKey(accountKey, detailClassKey);

                Assert.Greater(detailList.Count, 0);

                IDetail detail = detailList[0];

                Assert.AreEqual(detail.DetailType.Key, detailTypeKey);
            }
        }

        [Test]
        public void TestGetDetailByAccountKeyAndDetailClassKey_NoResults_ReturnsEmptyList()
        {
            using (new SessionScope())
            {
                int accountKey = 999999;
                int detailClassKey = 14; // Alpha Housing

                var accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();

                IReadOnlyEventList<IDetail> detailList = accountRepository.GetDetailByAccountKeyAndDetailClassKey(accountKey, detailClassKey);

                Assert.IsEmpty(detailList);
            }
        }

        [Test]
        public void TestHasApplicationBeenInCompany2_True()
        {
            string query = @"select top 1 fa.AccountKey
                             from dw.dwwarehousepre.securitisation.factaccountattribute  fa (nolock)
                             where isnull(fa.HasEverMovedFromCompany1,0) = 1";

            using (new SessionScope())
            {
                DataTable dt = base.GetQueryResults(query);

                if (dt.Rows.Count == 0)
                {
                    Assert.Ignore("No Data to perform this test");
                }
                else
                {
                    int accountKey = Convert.ToInt32(dt.Rows[0][0]);

                    var accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();

                    bool hasBeenIncompany2 = accountRepository.HasApplicationBeenInCompany2(accountKey);

                    Assert.True(hasBeenIncompany2);
                }
            }
        }

        [Test]
        public void TestHasApplicationBeenInCompany2_False()
        {
            string query = @"select top 1 fa.AccountKey
                             from dw.dwwarehousepre.securitisation.factaccountattribute  fa (nolock)
                             where isnull(fa.HasEverMovedFromCompany1,0) = 0";

            using (new SessionScope())
            {
                DataTable dt = base.GetQueryResults(query);

                if (dt.Rows.Count == 0)
                {
                    Assert.Ignore("No Data to perform this test");
                }
                else
                {
                    int accountKey = Convert.ToInt32(dt.Rows[0][0]);

                    var accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();

                    bool hasBeenIncompany2 = accountRepository.HasApplicationBeenInCompany2(accountKey);

                    Assert.False(hasBeenIncompany2);
                }
            }
        }
    }
}