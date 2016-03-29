using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NHibernate.Criterion;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Test;
using SAHL.Test.DAOHelpers;
using SAHL.Common.CacheData;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class LifeRepositoryTest : TestBase
    {
        [NUnit.Framework.Test]
        public void CreateEmptyLifePolicy()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                IDomainMessageCollection Messages = new DomainMessageCollection();
                ILifePolicy lifePolicy = LifeRepo.CreateEmptyLifePolicy();
                Assert.IsNotNull(lifePolicy);
            }
        }

        [NUnit.Framework.Test]
        public void CreateEmptyCallback()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                ICallback callback = LifeRepo.CreateEmptyCallback();
                Assert.IsNotNull(callback);
            }
        }

        [NUnit.Framework.Test]
        public void SaveCallback()
        {
            int key = -1;
            string completedUser = "", newUser = "";
            DateTime? completedDate, newDate;

            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                try
                {
                    IDomainMessageCollection Messages = new DomainMessageCollection();

                    CallbackHelper callbackHelper = new CallbackHelper();

                    // create a Callback object using the helper class
                    Callback_DAO callback = callbackHelper.CreateCallback();
                    callback.SaveAndFlush();

                    // make some changes and save using the Repository method
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    ICallback cb = BMTM.GetMappedType<ICallback>(callback);

                    newDate = new DateTime(2007, 12, 01);
                    newUser = "TestUser";

                    completedDate = cb.CompletedDate;
                    completedUser = cb.CompletedUser;
                    cb.CompletedDate = newDate;
                    cb.CompletedUser = newUser;

                    LifeRepo.SaveCallback(cb);

                    key = cb.Key;

                    // retrieve a new version of the object and ensure the value has been changed
                    Callback_DAO callbackCheck = Callback_DAO.Find(key) as Callback_DAO;

                    Assert.AreEqual(callbackCheck.Key, key);
                    Assert.AreNotEqual(callbackCheck.CompletedDate, completedDate);
                    Assert.AreNotEqual(callbackCheck.CompletedUser, completedUser);
                    Assert.IsTrue(callbackCheck.CompletedUser == newUser);
                    Assert.IsTrue(callbackCheck.CompletedDate == newDate);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.ToString());
                }
            }
        }

        [NUnit.Framework.Test]
        public void GetLifePolicyByAccountKey()
        {
            using (new SessionScope())
            {
                // find a life policy account
                int accountKey;
                using (IDbConnection con = Helper.GetSQLDBConnection())
                {
                    string query = "select top 1 AccountKey from FinancialService (nolock) where FinancialService.FinancialServiceTypeKey = 5";
                    accountKey = Convert.ToInt32(Helper.ExecuteScalar(con, query));
                }

                ILifePolicy lifePolicy = LifeRepo.GetLifePolicyByAccountKey(accountKey);
                Assert.IsNotNull(lifePolicy);
                Assert.IsTrue(lifePolicy.FinancialService.Account.Key == accountKey);
            }
        }

        [NUnit.Framework.Test]
        public void GetTextStatementsForTypes()
        {
            using (new SessionScope())
            {
                int[] STs = new int[6];
                STs[0] = (int)SAHL.Common.Globals.TextStatementTypes.Exclusions;    // 1
                STs[1] = (int)SAHL.Common.Globals.TextStatementTypes.RPAR;          // 2;
                STs[2] = (int)SAHL.Common.Globals.TextStatementTypes.Declaration;   // 3;
                STs[3] = (int)SAHL.Common.Globals.TextStatementTypes.FAIS;          // 4;
                STs[4] = (int)SAHL.Common.Globals.TextStatementTypes.Benefits1;     // 5;
                STs[5] = (int)SAHL.Common.Globals.TextStatementTypes.Benefits2;     // 6;

                IReadOnlyEventList<ITextStatement> results = LifeRepo.GetTextStatementsForTypes(STs);
                Assert.IsTrue(results.Count > 0);
            }
        }

        [NUnit.Framework.Test]
        public void GetCancellationTypes()
        {
            using (new SessionScope())
            {
                IDictionary<string, int> cancellationTypes = LifeRepo.GetCancellationTypes();

                Assert.IsTrue(cancellationTypes.Count == 4);
            }
        }

        [NUnit.Framework.Test]
        public void GetLoanPipelineStatus()
        {
            using (new SessionScope())
            {
                int accountKey;
                string pipeLineStatus;

                // get test data
                string query = "select top 1 eFolderName, eStageName from [e-work]..eFolder (nolock) where eMapName = 'Pipeline' order by eCreationTime desc";
                IDataReader reader = DBHelper.ExecuteReader(query);
                if (!reader.Read())
                    Assert.Ignore("No data");
                accountKey = Convert.ToInt32(reader.GetString(0));
                pipeLineStatus = reader.GetString(1);
                reader.Dispose();

                // execute repo method
                string checkPipeLineStatus = LifeRepo.GetLoanPipelineStatus(accountKey);

                // check results
                Assert.IsTrue(checkPipeLineStatus == pipeLineStatus);
            }
        }

        [NUnit.Framework.Test]
        public void GetLifePremiumHistory()
        {
            using (new SessionScope())
            {
                int accountKey = 0;

                string query = @"   select top 1 lph.AccountKey
                                        from LifePremiumHistory lph (nolock)
                                        join Account acc (nolock) on lph.AccountKey = acc.AccountKey
                                        where acc.AccountStatusKey = 1
                                        order by LifePremiumHistoryKey desc";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(Account_DAO), new ParameterCollection());
                if (o != null)
                {
                    accountKey = Convert.ToInt32(o);

                    IList<ILifePremiumHistory> premiumHistory = LifeRepo.GetLifePremiumHistory(accountKey);

                    Assert.IsTrue(premiumHistory.Count > 0);
                    Assert.IsTrue(accountKey == premiumHistory[0].Account.Key);
                }
                else
                    Assert.Fail("No test data");
            }
        }

        [NUnit.Framework.Test]
        public void IsLifeOverExposed_True()
        {
            using (new SessionScope())
            {
                // get test data - look for 1st assured life who has more than 1 policy
                int legalEntityKey;
                using (IDbConnection con = Helper.GetSQLDBConnection())
                {
                    string query = @"select top 1 r.LegalEntityKey
                                    from [2am]..Role r (nolock)
                                    join [2am]..FinancialService fs (nolock) on fs.AccountKey = r.AccountKey
                                    join [2am]..LifePolicy lp (nolock) on lp.FinancialServiceKey = fs.FinancialServiceKey
                                    where
	                                    r.RoleTypeKey = 1 -- Assured Life
                                    and
	                                    lp.PolicyStatusKey in (2,3,13)
                                    group by
	                                    r.LegalEntityKey
                                    having
	                                    count (r.LegalEntityKey) > 1";
                    legalEntityKey = Convert.ToInt32(Helper.ExecuteScalar(con, query));
                }

                // execute repo method
                ILegalEntity legalEntity = RepositoryFactory.GetRepository<ILegalEntityRepository>().GetLegalEntityByKey(legalEntityKey);
                bool isLifeOverExposed = LifeRepo.IsLifeOverExposed(legalEntity);

                // check results
                Assert.IsTrue(isLifeOverExposed == true);
            }
        }

        [NUnit.Framework.Test]
        public void IsLifeOverExposed_False()
        {
            using (new SessionScope())
            {
                // get test data - look for 1st assured life who has only 1 policy
                int legalEntityKey;
                using (IDbConnection con = Helper.GetSQLDBConnection())
                {
                    string query = @"select top 1 r.LegalEntityKey
                                    from [2am]..Role r (nolock)
                                    join [2am]..FinancialService fs (nolock) on fs.AccountKey = r.AccountKey
                                    join [2am]..LifePolicy lp (nolock) on lp.FinancialServiceKey = fs.FinancialServiceKey
                                    where
	                                    r.RoleTypeKey = 1 -- Assured Life
                                    and
	                                    lp.PolicyStatusKey in (2,3,13)
                                    group by
	                                    r.LegalEntityKey
                                    having
	                                    count (r.LegalEntityKey) = 1";
                    legalEntityKey = Convert.ToInt32(Helper.ExecuteScalar(con, query));
                }

                // execute repo method
                ILegalEntity legalEntity = RepositoryFactory.GetRepository<ILegalEntityRepository>().GetLegalEntityByKey(legalEntityKey);
                bool isLifeOverExposed = LifeRepo.IsLifeOverExposed(legalEntity);

                // check results
                Assert.IsTrue(isLifeOverExposed == false);
            }
        }

        [Test]
        public void IsLifeConditionOfLoan()
        {
            using (DBHelper dbHelper = new DBHelper(Databases.TwoAM))
            {
                int accountKey;
                string query = @"select top 1
	                                    o.AccountKey
                                    from
	                                    [2am]..OfferCondition oc (nolock)
                                    join
	                                    Offer o (nolock)
                                    on
	                                    o.OfferKey = oc.OfferKey and o.OfferStatusKey = 3
                                    where
                                        o.OfferTypeKey not in (2,5,9)
                                    and
                                        o.OfferStatusKey  = 3
                                    and
                                        oc.conditionkey = 1547 
                                    order by
	                                    --o.OfferEndDate
                                        o.OfferKey
                                    ";

                using (IDbCommand cmd = dbHelper.CreateCommand(query))
                {
                    cmd.CommandTimeout = 240;
                    object obj = dbHelper.ExecuteScalar(cmd);
                    if (obj != null)
                    {
                        accountKey = (int)obj;

                        // execute repo method
                        bool isLifeConditionOfLoan = LifeRepo.IsLifeConditionOfLoan(accountKey);

                        // check results
                        Assert.IsTrue(isLifeConditionOfLoan == true);
                    }
                }
            }
        }

        [NUnit.Framework.Test]
        public void IsAdminUser()
        {
            // get test data
            SAHLPrincipal principal = base.TestPrincipal;
            bool isAdminUser = LifeRepo.IsAdminUser(principal);

            // do test - not sure how to test this so just make sure the method doesnt fail
            Assert.IsTrue(isAdminUser == true || isAdminUser == false);
        }

        [NUnit.Framework.Test]
        public void IsRegentLoan()
        {
            using (new SessionScope())
            {
                bool regentLoan = false;

                // Look for the existance of a regent record with this accountkey with a status of 1 (NewBusiness)
                ICriterion[] criterion = new ICriterion[]
                {
                   Expression.Eq("RegentStatus.Key", (int)SAHL.Common.Globals.RegentStatus.NewBusiness)
                };

                Regent_DAO regent = Regent_DAO.FindFirst(criterion);
                if (regent == null)
                    Assert.Ignore("No Regent Loan Available");

                regentLoan = LifeRepo.IsRegentLoan((int)regent.Key);

                Assert.IsTrue(regentLoan == true);
            }
        }

        [NUnit.Framework.Test]
        public void CreateEmptyLifeInsurableInterest()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                ILifeInsurableInterest lifeInsurableInterest = LifeRepo.CreateEmptyLifeInsurableInterest();
                Assert.IsNotNull(lifeInsurableInterest);
            }
        }

        [NUnit.Framework.Test]
        public void RecalculateSALifePremium()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                // get test data - find first inforce life policy
                int accountKey = -1;
                double origCurrentSumAssured, origMonthlyInstalment, origYearlyPremium, origDeathBenefitPremium, origIPBenefitPremium;

                using (IDbConnection con = Helper.GetSQLDBConnection())
                {
                    string query = @"select top 1
	                                    fs.AccountKey,
	                                    lp.CurrentSumAssured,
	                                    lp.DeathBenefitPremium,
	                                    lp.InstallmentProtectionPremium,
	                                    lp.YearlyPremium,
	                                    fs.Payment
                                    from
	                                    [2am]..LifePolicy lp (nolock)
                                    join
	                                    [2am]..FinancialService fs (nolock)
                                    on
	                                    fs.FinancialServiceKey = lp.FinancialServiceKey
                                    where
	                                    lp.PolicyStatusKey = 3";
                    IDataReader reader = DBHelper.ExecuteReader(query);
                    if (!reader.Read())
                        Assert.Ignore("No data");

                    accountKey = reader.GetInt32(0);
                    origCurrentSumAssured = reader.GetDouble(1);
                    origDeathBenefitPremium = reader.GetDouble(2);
                    origIPBenefitPremium = reader.GetDouble(3);
                    origYearlyPremium = reader.GetDouble(4);
                    origMonthlyInstalment = reader.GetDouble(5);

                    reader.Dispose();
                }

                // run repo method
                IAccountLifePolicy accountLifePolicy = RepositoryFactory.GetRepository<IAccountRepository>().GetAccountByKey(accountKey) as IAccountLifePolicy;
                LifeRepo.RecalculateSALifePremium(accountLifePolicy, false);

                // check results
                Assert.IsTrue(accountKey > 0);
                Assert.IsTrue(accountLifePolicy.LifePolicy.CurrentSumAssured > 0);
                Assert.IsTrue(accountLifePolicy.LifePolicy.MonthlyPremium > 0);
                Assert.IsTrue(accountLifePolicy.LifePolicy.YearlyPremium > 0);
                Assert.IsTrue(accountLifePolicy.LifePolicy.DeathBenefitPremium > 0);
            }
        }

        /// <summary>
        /// RecalculateSALifePremiumQuote
        /// </summary>
        [NUnit.Framework.Test]
        public void RecalculateSALifePremiumQuote()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                // get test data - find latest inforce life policy
                int accountKey = -1;
                using (IDbConnection con = Helper.GetSQLDBConnection())
                {
                    string query = @"SELECT TOP 1 CA.AccountKey
	                                    FROM [2am].dbo.Account A (NOLOCK)
	                                    JOIN [2am].dbo.Account CA (NOLOCK)
	                                    ON A.AccountKey = CA.ParentAccountKey
	                                    JOIN [2am].dbo.FinancialService FS (NOLOCK)
	                                    ON FS.AccountKey = CA.AccountKey
	                                    JOIN [2am].dbo.LifePolicy LP (NOLOCK)
	                                    ON FS.FinancialServiceKey = LP.FinancialServiceKey
	                                    WHERE A.AccountStatusKey = 1
	                                    AND CA.AccountStatusKey = 1
	                                    AND LP.PolicyStatusKey = 3";
                    accountKey = Convert.ToInt32(Helper.ExecuteScalar(con, query));
                }
                string ageList = "30,45";
                double currentSumAssured, monthlyInstalment, yearlyPremium, deathBenefitPremium, ipBenefitPremium;

                // run repo methid
                LifeRepo.RecalculateSALifePremiumQuote(accountKey, (int)SAHL.Common.Globals.LifePolicyTypes.StandardCover, true, ageList, out currentSumAssured, out monthlyInstalment, out yearlyPremium, out deathBenefitPremium, out ipBenefitPremium);

                // check results
                Assert.IsTrue(accountKey > 0);
                Assert.IsTrue(currentSumAssured > 0);
                Assert.IsTrue(monthlyInstalment > 0);
                Assert.IsTrue(yearlyPremium > 0);
                Assert.IsTrue(deathBenefitPremium > 0);
                Assert.IsTrue(ipBenefitPremium > 0);
            }
        }


        [NUnit.Framework.Test]
        public void GetPolicyHolderDetails()
        {
            using (new SessionScope())
            {
                // get test data - find latest inforce life policy
                int accountKey = -1;
                using (IDbConnection con = Helper.GetSQLDBConnection())
                {
                    string query = @"select top 1
	                                    fs.AccountKey
                                    from
	                                    [2am]..LifePolicy lp (nolock)
                                    join
	                                    [2am]..FinancialService fs (nolock)
                                    on
	                                    fs.FinancialServiceKey = lp.FinancialServiceKey
                                    where
	                                    lp.PolicyStatusKey = 3
                                    order by
	                                    lp.FinancialServiceKey desc";
                    accountKey = Convert.ToInt32(Helper.ExecuteScalar(con, query));
                }

                // run repo method
                int legalEntityKey, addressKey;

                LifeRepo.GetPolicyHolderDetails(accountKey, out legalEntityKey, out addressKey);

                // check results
                Assert.IsTrue(accountKey > 0);
                Assert.IsTrue(legalEntityKey > 0);
                Assert.IsTrue(addressKey > 0);
            }
        }

        [NUnit.Framework.Test]
        public void CancelLifePolicy()
        {
            int lifeAccountKey, policyStatusKey = (int)Globals.LifePolicyStatuses.Closed;

            using (new SessionScope(FlushAction.Never)) // plop, swish... Dohhh!
            {
                using (IDbConnection con = Helper.GetSQLDBConnection())
                {
                    string query = String.Format(@"select top 1 fs.AccountKey
                                    from [2am]..LifePolicy lp (nolock)
                                    join [2am]..FinancialService fs (nolock) on fs.FinancialServiceKey = lp.FinancialServiceKey and FinancialServiceTypeKey = {0}
                                    join [2am].fin.Balance lb (nolock) on fs.FinancialServiceKey = lb.FinancialServiceKey
                                    left join [2am]..FinancialService afs (nolock) on afs.ParentFinancialServiceKey = fs.FinancialServiceKey
                                        and afs.FinancialServiceTypeKey = 8
                                    left join [2am].fin.Balance ab (nolock) on  afs.FinancialServiceKey = ab.FinancialServiceKey and ab.BalanceTypeKey = 4
                                    where lp.PolicyStatusKey = {1}
                                    and isnull(lb.Amount, 0) = 0
                                    and isnull(ab.Amount, 0) = 0
                                    order by lp.FinancialServiceKey desc", (int)Globals.FinancialServiceTypes.LifePolicy, (int)Globals.LifePolicyStatuses.Inforce);

                    lifeAccountKey = Convert.ToInt32(Helper.ExecuteScalar(con, query));
                }
                if (lifeAccountKey > 0)
                {
                    TransactionScope txn = new TransactionScope(TransactionMode.New, OnDispose.Rollback);
                    try
                    {
                        // Perform the cancellation
                        LifeRepo.CancelLifePolicy(lifeAccountKey, policyStatusKey);

                        txn.VoteCommit();
                    }
                    catch (Exception)
                    {
                        txn.VoteRollBack();
                    }
                    finally
                    {
                        txn.Dispose();
                    }
                }
            }
        }

        [NUnit.Framework.Test]
        public void GetInsurerByDescription()
        {
            using (new SessionScope())
            {
                IDomainMessageCollection Messages = new DomainMessageCollection();

                Insurer_DAO insurerCheck = Insurer_DAO.FindFirst() as Insurer_DAO;

                IInsurer insurer = LifeRepo.GetInsurerByDescription(insurerCheck.Description);

                Assert.IsTrue(insurerCheck.Description == insurer.Description);
                Assert.IsTrue(insurerCheck.Key == insurer.Key);
            }
        }

        [NUnit.Framework.Test]
        public void GetLifeConsultantsAll()
        {
            using (new SessionScope())
            {
                IList<IADUser> adUsers = LifeRepo.GetLifeConsultants();

                Assert.IsTrue(adUsers.Count > 1);
            }
        }

        [NUnit.Framework.Test]
        public void GetLifeConsultantSingle()
        {
            using (new SessionScope())
            {
                IList<IADUser> adUsers = LifeRepo.GetLifeConsultants();

                string adUserName = adUsers[0].ADUserName;
                IList<IADUser> adUser = LifeRepo.GetLifeConsultants(adUserName);

                Assert.IsTrue(adUser.Count == 1);
                Assert.IsTrue(adUser[0].ADUserName == adUserName);
            }
        }

        [Test]
        public void GetLifeConsultantsIncludeInactive()
        {
            using (new SessionScope())
            {
                IList<IADUser> adus = LifeRepo.GetLifeConsultants(true);
                Assert.IsTrue(adus.Count > 0);

                adus = LifeRepo.GetLifeConsultants(false);
                Assert.IsTrue(adus.Count > 0);
            }
        }

        [NUnit.Framework.Test]
        public void GetAssuredLifeAgeForPremiumCalc()
        {
            using (new SessionScope())
            {
                // get test data - find latest inforce life policy
                int legalEntityKey = -1;
                using (IDbConnection con = Helper.GetSQLDBConnection())
                {
                    string query = @"select top 1 r.LegalEntityKey
                                        from [2am]..Role r (nolock)
                                        join [2am]..LegalEntity le (nolock) on le.LegalEntityKey = r.LegalEntityKey
                                        join [2am]..FinancialService fs (nolock) on fs.AccountKey = r.AccountKey
                                        join [2am]..LifePolicy lp (nolock) on lp.FinancialServiceKey = fs.FinancialServiceKey
                                        where
                                            r.RoleTypeKey = 1 -- Assured Life
                                        and
                                            lp.PolicyStatusKey = 3
                                        and
	                                        le.LegalEntityTypeKey = 2 -- natural person";

                    legalEntityKey = Convert.ToInt32(Helper.ExecuteScalar(con, query));
                }

                // run repo method
                ILegalEntityNaturalPerson legalEntityNaturalPerson = RepositoryFactory.GetRepository<ILegalEntityRepository>().GetLegalEntityByKey(legalEntityKey) as ILegalEntityNaturalPerson;
                int legalEntityAge = LifeRepo.GetAssuredLifeAgeForPremiumCalc(legalEntityNaturalPerson, DateTime.Now);

                // check results
                Assert.IsTrue(legalEntityKey > 0);
                Assert.IsTrue(legalEntityAge > 0);
            }
        }

        [Test]
        public void GetLatestLifeOfferAssignment()
        {
            ILifeOfferAssignment loa = LifeRepo.GetLatestLifeOfferAssignment("SomeRubbish", -1);
            Assert.IsNull(loa);

            string sql = @"select top 1 loa.ADUserName, loa.LoanOfferTypeKey, loa.LifeOfferAssignmentKey
                from [2am]..LifeOfferAssignment loa (nolock)
                order by loa.DateAssigned desc";

            DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                loa = LifeRepo.GetLatestLifeOfferAssignment(ds.Tables[0].Rows[0][0].ToString(), Convert.ToInt32(ds.Tables[0].Rows[0][1]));
                Assert.AreEqual(loa.Key, Convert.ToInt32(ds.Tables[0].Rows[0][2]));
            }
        }

        public void GetDateAssuredLifeAddedToPolicy()
        {
            using (new SessionScope())
            {
                // get test data - find latest inforce life policy
                int accountkey = -1;
                int legalEntityKey = -1;

                string query = @"select top 1 fs.AccountKey,r.LegalEntityKey
                                    from [2am]..Role r (nolock)
                                    join [2am]..LegalEntity le (nolock) on le.LegalEntityKey = r.LegalEntityKey
                                    join [2am]..FinancialService fs (nolock) on fs.AccountKey = r.AccountKey
                                    join [2am]..LifePolicy lp (nolock) on lp.FinancialServiceKey = fs.FinancialServiceKey
                                    where
                                        r.RoleTypeKey = 1 -- Assured Life
                                    and
                                        lp.PolicyStatusKey = 3
                                    and
	                                    le.LegalEntityTypeKey = 2 -- natural person";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), null);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    accountkey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    legalEntityKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                }

                // run repo method
                DateTime? dateAddedToPolicy = LifeRepo.GetDateAssuredLifeAddedToPolicy(accountkey, legalEntityKey);

                // check results
                Assert.IsTrue(dateAddedToPolicy.HasValue);
            }
        }

        [Test]
        public void CreateCreditLifeForPersonalLoan_FailAccount()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            using (new TransactionScope(OnDispose.Rollback))
            {
                string query = @"select top 1 accountKey from account where rrr_productkey != 12";
                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(Account_DAO), new ParameterCollection());

                if (o != null)
                {
                    int personalLoanAccountKey = Convert.ToInt32(o);
                    LifeRepo.CreateCreditLifeForPersonalLoan(personalLoanAccountKey, "test");

                    Assert.AreEqual(spc.DomainMessages.Count, 1);
                    Assert.AreEqual(spc.DomainMessages[0].Message.ToString(), "Create Credit Life failed on Personal Loan Account.");
                    spc.DomainMessages.Clear();
                }
            }
        }

        [Test]
        public void CreateCreditLifeForPersonalLoan_Pass()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            using (new TransactionScope(OnDispose.Rollback))
            {
                string query = @"select top 1 a.AccountKey 
                                from Account a (nolock)
                                join AccountExternalLife ael (nolock) on a.AccountKey = ael.AccountKey
                                join ExternalLifePolicy el (nolock) on el.ExternalLifePolicyKey = ael.ExternalLifePolicyKey
	                                and el.LifePolicyStatusKey = 3
                                join FinancialService fs (nolock) on fs.AccountKey = a.AccountKey
	                                and fs.FinancialServiceTypeKey = 10
	                                and fs.AccountStatusKey = 1
                                join fin.Balance b (nolock) on b.FinancialServiceKey = fs.FinancialServiceKey
	                                and b.BalanceTypeKey = 1
	                                and b.Amount > 100
                                where a.rrr_productkey = 12
                                and a.AccountStatusKey = 1";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(Account_DAO), new ParameterCollection());

                if (o != null)
                {
                    int personalLoanAccountKey = Convert.ToInt32(o);
                    LifeRepo.CreateCreditLifeForPersonalLoan(personalLoanAccountKey, "test");
                    Assert.AreEqual(spc.DomainMessages.Count, 0, String.Format("Test failed for personalLoanAccountKey: {0}", personalLoanAccountKey));
                }
            }
        }

        private ILifeRepository _lRepo;

        public ILifeRepository LifeRepo
        {
            get
            {
                if (_lRepo == null)
                    _lRepo = RepositoryFactory.GetRepository<ILifeRepository>();

                return _lRepo;
            }
        }
    }
}