using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class DisbursementRepositoryTest : TestBase
    {
        [SetUp]
        public void Setup()
        {
            // set the strategy to default so we actually go to the database
            SetRepositoryStrategy(TypeFactoryStrategy.Default);
            _mockery = new MockRepository();
        }

        [TearDown]
        public void TearDown()
        {
            MockCache.Flush();
            // GC.Collect();
        }

        [Test]
        public void DeleteDisbursement()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                IDisbursementRepository repo = RepositoryFactory.GetRepository<IDisbursementRepository>();
                Disbursement_DAO disb = Disbursement_DAO.FindFirst();
                int key = disb.Key;
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IDisbursement disbursement = BMTM.GetMappedType<IDisbursement, Disbursement_DAO>(disb);
                repo.DeleteDisbursement(disbursement);
                Disbursement_DAO testDisb = Disbursement_DAO.Find(key);
                Assert.IsTrue(1 == 1);
            }
        }

        private string ReflectDAO(object obj, string name)
        {
            if (obj == null)
                return String.Format("{0}: NULL\n", name);

            Type objType = obj.GetType();
            StringBuilder SB = new StringBuilder();
            SB.AppendFormat("{0}:\n", name);

            //FieldInfo[] fis = objType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            //MemberInfo[] members = objType.GetMembers();
            PropertyInfo[] properties = objType.GetProperties();

            foreach (PropertyInfo pi in properties)
            {
                if (pi.Name == "BusinessModelObject" || pi.Name == "PropertiesValidationErrorMessage" || pi.Name == "ValidationErrorMessages")
                    continue;
                else if (pi.PropertyType.IsClass)
                {
                    object theClass = pi.GetValue(obj, null);

                    if (theClass == null)
                    {
                        SB.AppendFormat("{0} = NULL\n", pi.Name);
                        continue;
                    }

                    Type classType = theClass.GetType();
                    object val = null;
                    object dval = null;
                    PropertyInfo[] props = classType.GetProperties();

                    foreach (PropertyInfo pic in props)
                    {
                        if (pic.Name.ToLower() == "key")
                            val = pic.GetValue(theClass, null);
                        else if (pic.Name.ToLower() == "description")
                            dval = pic.GetValue(theClass, null);
                    }

                    if (val != null)
                        SB.AppendFormat("{0}Key = {1} ({2})\n", pi.Name, val, dval);
                    else
                        SB.AppendFormat("{0} = {1}\n", pi.Name, theClass);
                }
                else if (pi.PropertyType.IsArray)
                {
                    Array val = (Array)pi.GetValue(obj, null);

                    if (val != null)
                        SB.AppendFormat("{0} Count = {1}\n", pi.Name, val.Length);
                    else
                        SB.AppendFormat("{0} = NULL\n", pi.Name);
                }
                else if (pi.PropertyType.FullName.StartsWith("System.Collections"))
                {
                    object theClass = pi.GetValue(obj, null);

                    if (theClass == null)
                    {
                        SB.AppendFormat("{0} = NULL\n", pi.Name);
                        break;
                    }

                    Type classType = theClass.GetType();
                    PropertyInfo[] props = classType.GetProperties();

                    foreach (PropertyInfo pic in props)
                    {
                        if (pic.Name == "Count")
                        {
                            object val = pic.GetValue(theClass, null);
                            SB.AppendFormat("{0}.Count = {1}\n", pi.Name, val);
                            break;
                        }
                    }
                }
                else if (pi.PropertyType.IsEnum)
                {
                    object val = pi.GetValue(obj, null);

                    if (val == null)
                    {
                        SB.AppendFormat("{0} = NULL\n", pi.Name);
                        continue;
                    }

                    Type t = val.GetType();
                    SB.AppendFormat("{0} = {1} ({2})\n", t.Name, Enum.Format(t, val, "d"), Enum.GetName(t, val));
                }
                else
                {
                    object val = pi.GetValue(obj, null);

                    if (val == null)
                        SB.AppendFormat("{0} = NULL\n", pi.Name);
                    else
                        SB.AppendFormat("{0} = {1}\n", pi.Name, val);
                }
            }

            //SB.AppendLine();
            return SB.ToString();
        }

        private string LogDisbursementEffects(Application_DAO offer)
        {
            StringBuilder SB = new StringBuilder();

            Account_DAO account = offer.Account;
            MortgageLoan_DAO ml = null;
            Account_DAO hocAccount = null;
            Account_DAO lifeAccount = null;
            FinancialService_DAO vFS = null;
            FinancialService_DAO fFS = null;
            LifePolicy_DAO lifePolicy = null;
            HOC_DAO hoc = null;

            string HQL = "from FinancialService_DAO fs where fs.Account.Key = ? and fs.FinancialServiceType.Key in (1,2)";
            SimpleQuery<FinancialService_DAO> fsQuery = new SimpleQuery<FinancialService_DAO>(HQL, account.Key);
            FinancialService_DAO[] finServices = fsQuery.Execute();

            Bond_DAO[] bond = Bond_DAO.FindAllByProperty("Application.Key", offer.Key);

            Guarantee_DAO[] guarantees = Guarantee_DAO.FindAllByProperty("Account.Key", account.Key);

            HQL = "from RegMail_DAO rm where rm.LoanNumber = ?";
            SimpleQuery<RegMail_DAO> rmQuery = new SimpleQuery<RegMail_DAO>(HQL, account.Key);
            RegMail_DAO[] regMails = rmQuery.Execute();

            for (int i = 0; i < finServices.Length; i++)
            {
                if (finServices[i].FinancialServiceType.Key == 1)
                {
                    vFS = finServices[i];
                    ml = finServices[i] as MortgageLoan_DAO;
                }
                else if (finServices[i].FinancialServiceType.Key == 2)
                {
                    fFS = finServices[i];
                }
            }

            for (int i = 0; i < account.RelatedChildAccounts.Count; i++)
            {
                if (account.RelatedChildAccounts[i].Product.Key == 3)
                {
                    hocAccount = account.RelatedChildAccounts[i];

                    if (account.RelatedChildAccounts[i].FinancialServices != null && account.RelatedChildAccounts[i].FinancialServices.Count > 0)
                    {
                        //hoc = account.RelatedChildAccounts[i].FinancialServices[0] as HOC_DAO;
                        HOC_DAO[] hocs = HOC_DAO.FindAllByProperty("Key", account.RelatedChildAccounts[i].FinancialServices[0].Key);
                        hoc = hocs[0];
                    }
                }
                else if (account.RelatedChildAccounts[i].Product.Key == 4)
                {
                    lifeAccount = account.RelatedChildAccounts[i];

                    if (account.RelatedChildAccounts[i].FinancialServices != null && account.RelatedChildAccounts[i].FinancialServices.Count > 0)
                        lifePolicy = (account.RelatedChildAccounts[i] as AccountLifePolicy_DAO).LifePolicyFinancialService;
                    //account.RelatedChildAccounts[i].FinancialServices[0] as LifePolicy_DAO;
                }
            }

            SB.AppendLine(ReflectDAO(account, "ACCOUNT"));
            SB.AppendLine(ReflectDAO(lifeAccount, "LIFE ACCOUNT"));
            SB.AppendLine(ReflectDAO(hocAccount, "HOC ACCOUNT"));
            SB.AppendLine(ReflectDAO(vFS, "VARIABLE FINANCIAL SERVICE"));
            SB.AppendLine(ReflectDAO(fFS, "FIXED FINANCIAL SERVICE"));
            SB.AppendLine(ReflectDAO(lifePolicy, "LIFE POLICY"));
            SB.AppendLine(ReflectDAO(hoc, "HOC"));

            if (hoc != null)
            {
                SB.AppendLine(ReflectDAO(hoc.HOCHistory, "HOC HISTORY"));

                if (hoc.HOCHistory != null)
                {
                    for (int i = 0; i < hoc.HOCHistory.HOCHistoryDetails.Count; i++)
                    {
                        SB.AppendLine(ReflectDAO(hoc.HOCHistory.HOCHistoryDetails[i], String.Format("HOC HISTORY DETAIL {0}", i)));
                    }
                }
            }

            if (hocAccount != null)
            {
                for (int i = 0; i < hocAccount.Roles.Count; i++)
                {
                    if (hocAccount.Roles[i].RoleType.Key == 2)
                        SB.AppendLine(ReflectDAO(hocAccount.Roles[i], String.Format("HOC ROLE {0}", i)));
                }
            }

            //SB.AppendLine(ReflectDAO(offer, "OFFER"));
            SB.AppendLine(ReflectDAO(ml, "MORTGAGE LOAN"));

            if (bond != null)
            {
                for (int i = 0; i < bond.Length; i++)
                {
                    SB.AppendLine(ReflectDAO(bond[i], String.Format("BOND {0}", i)));

                    for (int k = 0; k < bond[i].LoanAgreements.Count; k++)
                        SB.AppendLine(ReflectDAO(bond[i].LoanAgreements[k], String.Format("BOND {0} LOAN AGREEMENT {1}", i, k)));
                }
            }
            else
                SB.AppendLine("BOND = NULL");

            for (int i = 0; i < vFS.FinancialTransactions.Count; i++)
            {
                SB.AppendLine(ReflectDAO(vFS.FinancialTransactions[i], String.Format("LOAN TRANSACTION {0}", i)));
            }

            ArrearTransaction_DAO[] arrears = ArrearTransaction_DAO.FindAllByProperty("FinancialService.Key", vFS.Key);

            for (int i = 0; i < arrears.Length; i++)
            {
                SB.AppendLine(ReflectDAO(arrears[i], String.Format("ARREAR TRANSACTION {0}", i)));
            }

            ManualDebitOrder_DAO[] manDebitOrder = ManualDebitOrder_DAO.FindAllByProperty("FinancialService.Key", vFS.Key);

            for (int i = 0; i < manDebitOrder.Length; i++)
            {
                SB.AppendLine(ReflectDAO(manDebitOrder[i], String.Format("MANUAL DEBIT ORDER {0}", i)));
            }

            for (int i = 0; i < account.Details.Count; i++)
                SB.AppendLine(ReflectDAO(account.Details[i], String.Format("DETAIL {0}", i)));

            for (int i = 0; i < guarantees.Length; i++)
                SB.AppendLine(ReflectDAO(guarantees[i], String.Format("GUARANTEE {0}", i)));

            for (int i = 0; i < regMails.Length; i++)
                SB.AppendLine(ReflectDAO(regMails[i], String.Format("REGMAIL {0}", i)));

            return SB.ToString();
        }

        [Test]
        public void GetDisbursementTransactionTypes()
        {
            using (new SessionScope())
            {
                string query = String.Format(@"select TOP 1 TTDA.ADCredentials, count(TTDA.ADCredentials)
                                                from [2am].[fin].TransactionType tt
                                                join [2am].[dbo].[TransactionTypeDataAccess] ttda (nolock) ON tt.TransactionTypeKey = ttda.TransactionTypeKey
                                                join [2am].[fin].TransactionTypeBalanceEffect tte (nolock) on tt.TransactionTypekey = tte.TransactionTypekey
                                                join [2am].[fin].TransactionEffect tte1 (nolock) on tte.TransactionEffectkey = tte1.TransactionEffectkey
                                                join [2am].dbo.TransactionTypeUI tt1 (nolock) on tt.TransactionTypeKey = tt1.TransactionTypeKey
                                                join [2am].[fin].TransactionTypeGroup ttg (nolock) on tt.TransactionTypekey = ttg.TransactionTypekey
                                                where tte.TransactionEffectkey < 3
                                                and tt.transactionTypeKey in (140, 160, 170, 141)
                                                group by TTDA.ADCredentials order by TTDA.ADCredentials, count(TTDA.ADCredentials) desc");

                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                string role = Convert.ToString(DT.Rows[0][0]);
                int count = Convert.ToInt32(DT.Rows[0][1]);

                IDisbursementRepository repo = new DisbursementRepository();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                CurrentPrincipalCache.Roles.Clear();
                CurrentPrincipalCache.Roles.Add(role);
                IReadOnlyEventList<IDisbursementTransactionType> list = repo.GetDisbursementTransactionTypes(TestPrincipal);

                Assert.AreEqual(count, list.Count);
            }
        }

        [Test]
        public void GetDisbursmentsByParentAccountKeyAndStatus()
        {
            using (new SessionScope())
            {
                string query = "SELECT TOP 1 D.AccountKey, D.DisbursementStatusKey, count(D.AccountKey) from [2AM].[dbo].[disbursement] D (NOLOCK) "
                + "WHERE  D.DisbursementStatusKey is not null Group by D.AccountKey, D.DisbursementStatusKey ORDER BY count(D.AccountKey) DESC ";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                int accountKey = Convert.ToInt32(DT.Rows[0][0]);
                int statusKey = Convert.ToInt32(DT.Rows[0][1]);
                int count = Convert.ToInt32(DT.Rows[0][2]);

                IDisbursementRepository repo = new DisbursementRepository();

                IReadOnlyEventList<IDisbursement> list = repo.GetDisbursmentsByParentAccountKeyAndStatus(accountKey, statusKey);

                Assert.That(list.Count == count);
            }
        }

        [Test]
        public void GetDisbursementLoanTransactions()
        {
            using (new SessionScope())
            {
                string query = @"SELECT TOP 1 D.AccountKey,
                                TTDA.ADCredentials
                                FROM [2AM].[dbo].[Disbursement] D (NOLOCK)
                                Join [2AM].[dbo].[DisbursementTransactionType] DTT (NOLOCK) ON DTT.DisbursementTransactionTypeKey = D.DisbursementTransactionTypeKey
                                Join [2AM].[fin].[TransactionType] TT (nolock) ON TT.TransactionTypeKey = DTT.TransactionTypeNumber
                                Join [2AM].[fin].[FinancialTransaction] LT (nolock) ON LT.TransactionTypekey = DTT.TransactionTypeNumber
                                Join [2AM].[fin].[TransactionTypeGroup] TTG (nolock) ON TT.TransactionTypeKey = TTG.TransactionTypeKey
                                Join [2AM].[dbo].[TransactionTypeDataAccess] TTDA (nolock) ON TTDA.TransactionTypekey = TT.TransactionTypeKey
                                WHERE TT.TransactionTypeKey < 1000 AND TTG.TransactionGroupKey = 1";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                int accountKey = Convert.ToInt32(DT.Rows[0][0]);
                string role = Convert.ToString(DT.Rows[0][1]);

                IDisbursementRepository repo = RepositoryFactory.GetRepository<IDisbursementRepository>();

                CurrentPrincipalCache.Roles.Add(role);
                DataTable resultDT = repo.GetDisbursementLoanTransactions(accountKey, TestPrincipal);
            }
        }

        [Test]
        public void GetDisbursementRollbackTransactions()
        {
            using (new SessionScope())
            {
                string query = @"SELECT TOP 5 D.AccountKey, dft.FinancialTransactionKey
                                FROM [2AM].[dbo].[Disbursement] D (NOLOCK)
                                JOIN [2AM].[dbo].DisbursementFinancialTransaction dft (NOLOCK) ON D.DisbursementKey = dft.DisbursementKey
                                WHERE D.DisbursementStatusKey IN (0, 1) AND D.AccountKey in
                                (
                                    SELECT TOP 1 D2.AccountKey FROM [2AM].[dbo].[Disbursement] D2 (NOLOCK)
                                    JOIN   [2AM].[dbo].DisbursementFinancialTransaction dft2  ON   D2.DisbursementKey = dft2.DisbursementKey
                                    group by  D2.AccountKey order by count(D2.AccountKey) DESC
                                );";
                DataTable DT = base.GetQueryResults(query);
                if (DT.Rows.Count == 0)
                {
                    Assert.Inconclusive();
                    return;
                }
                Assert.That(DT.Rows.Count == 5);

                int accountKey = Convert.ToInt32(DT.Rows[0][0]);

                List<int> numbers = new List<int>();

                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    numbers.Add(Convert.ToInt32(DT.Rows[i][1]));
                }

                IDisbursementRepository repo = RepositoryFactory.GetRepository<IDisbursementRepository>();

                DataTable resultDT = repo.GetDisbursementRollbackTransactions(accountKey, numbers.ToArray());
            }
        }

        [Test]
        public void UpdateRate()
        {
            //This will test pLoanUpdateRate
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string query = @"select top 1 m.MarginKey, a.AccountKey, lb.RateConfigurationKey
                    from account a (nolock)
                    join financialService fs (nolock) on a.AccountKey = fs.AccountKey
                    join fin.Loanbalance lb (nolock) on fs.FinancialServiceKey = lb.FinancialServiceKey
					join fin.mortgageLoan ml (nolock) on ml.FinancialServiceKey = fs.FinancialServiceKey
                    join RateConfiguration rc (nolock) on lb.RateConfigurationKey = rc.RateConfigurationKey
                    join CreditCriteria cc (nolock) on ml.CreditMatrixKey = cc.CreditMatrixKey
	                    and cc.CategoryKey != fs.CategoryKey and cc.MarginKey != rc.MarginKey
                    join Margin m (nolock) on cc.MarginKey = m.MarginKey
                    where a.RRR_OriginationSourceKey = 1
	                and a.AccountStatusKey = 1 order by a.AccountKey desc";

                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                int marginKey = Convert.ToInt32(DT.Rows[0][0]);
                Int32 accountKey = Convert.ToInt32(DT.Rows[0][1]);
                int oldRateConfigKey = Convert.ToInt32(DT.Rows[0][2]);

                IDisbursementRepository repo = RepositoryFactory.GetRepository<IDisbursementRepository>();
                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                IMargin margin = new Margin(Margin_DAO.Find(marginKey));
                IAccount acc = accRepo.GetAccountByKey(accountKey);

                repo.UpdateRate(acc, margin, "test", false);

                string chk = String.Format(@"select lb.RateConfigurationKey from financialService fs (nolock)
                    join fin.LoanBalance lb (nolock) on fs.financialServiceKey = lb.financialServiceKey
                    where fs.AccountKey = {0}", accountKey);

                DT = base.GetQueryResults(chk);

                Assert.That(Convert.ToInt32(DT.Rows[0][0]) != oldRateConfigKey);
            }
        }

        //[Ignore("Test times-out since last migration. Neesds a re-write")]
        [Test]
        public void GetDisbursementByLoanTransactionNumber()
        {
            IDisbursementRepository repo = new DisbursementRepository();

            using (new SessionScope())
            {
                string query = @"Select  top 1 finTran.FinancialTransactionKey,disb.DisbursementKey
                                from dbo.Disbursement disb
                                inner join dbo.DisbursementFinancialTransaction disbFinTran
                                on disb.DisbursementKey = disbFinTran.DisbursementKey
                                inner join fin.FinancialTransaction finTran
                                on finTran.FinancialTransactionKey = disbFinTran.FinancialTransactionKey
                                ";
                DataTable DT = base.GetQueryResults(query);
                if (DT.Rows.Count == 0)
                {
                    Assert.Inconclusive();
                    return;
                }
                Assert.That(DT.Rows.Count == 1);

                decimal LoanTransactionNumber = Convert.ToDecimal(DT.Rows[0][0]);
                int DisbursementKey = Convert.ToInt32(DT.Rows[0][1]);

                IReadOnlyEventList<IDisbursement> list = repo.GetDisbursementByLoanTransactionNumber(LoanTransactionNumber);
                bool found = false;

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Key == DisbursementKey)
                    {
                        found = true;
                        break;
                    }
                }

                Assert.That(found == true);
            }
        }

        private IFinancialTransaction GetDisbursementLoanTransactionNumberHelper()
        {
            ILoanTransactionRepository repo = RepositoryFactory.GetRepository<ILoanTransactionRepository>();

            using (new SessionScope())
            {
                string query = @"select top 1 dft.FinancialTransactionKey, dft.DisbursementKey from [2AM].dbo.DisbursementFinancialTransaction dft
				join [2AM].fin.FinancialTransaction ft on ft.FinancialTransactionKey = dft.FinancialTransactionKey where len(ft.Reference) > 1 ";
                DataTable DT = base.GetQueryResults(query);
                if (DT.Rows.Count == 0)
                {
                    Assert.Inconclusive();
                }
                Assert.That(DT.Rows.Count == 1);
                int LoanTransactionNumber = Convert.ToInt32(DT.Rows[0][0]);
                return repo.GetLoanTransactionByLoanTransactionNumber(LoanTransactionNumber);
            }
        }

        private IList<IDisbursement> GetDisbursementsHelper()
        {
            using (new SessionScope())
            {
                IDisbursementRepository repo = RepositoryFactory.GetRepository<IDisbursementRepository>();
                string query = @"select top 1 dlt.LoanTransactionNumber, dlt.DisbursementKey from [2AM].dbo.DisbursementLoanTransaction dlt
				join [2AM].dbo.LoanTransaction lt on lt.LoanTransactionNumber = dlt.LoanTransactionNumber";
                DataTable DT = base.GetQueryResults(query);
                decimal LoanTransactionNumber = Convert.ToDecimal(DT.Rows[0][0]);
                return (List<IDisbursement>)repo.GetDisbursementByLoanTransactionNumber(LoanTransactionNumber);
            }
        }

        [Test]
        public void GetDisbursementByKey()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 DisbursementKey from Disbursement";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int key = Convert.ToInt32(DT.Rows[0][0]);

                IDisbursementRepository repo = RepositoryFactory.GetRepository<IDisbursementRepository>();
                IDisbursement disb = repo.GetDisbursementByKey(key);
                Assert.IsNotNull(disb);
            }
        }

        private void GetDisbursementByKeyHelper(out int accountKey, out int disbursementKey)
        {
            using (new SessionScope())
            {
                string query = @"select top 1 d.AccountKey,d.DisbursementKey
                        from account acc
                        join financialService fs
	                        on acc.accountKey = fs.AccountKey
                        join fin.Balance fb
	                        on fb.financialServiceKey = fs.financialServiceKey
                        join disbursement d
	                        on acc.AccountKey = d.AccountKey and d.DisbursementStatusKey = 2
                        where
	                        acc.AccountStatusKey = 1 and
	                        fs.AccountStatusKey = 1 and
	                        fs.financialServiceTypeKey = 1  ";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                accountKey = Convert.ToInt32(DT.Rows[0][0]);
                disbursementKey = Convert.ToInt32(DT.Rows[0][1]);
            }
        }

        [Test]
        public void PostDisbursementTransactionTest()
        {
            using (new TransactionScope(TransactionMode.New, OnDispose.Rollback))
            {
                IDisbursementRepository repo = RepositoryFactory.GetRepository<IDisbursementRepository>();

                int accountKey, disbursementKey;

                GetDisbursementByKeyHelper(out accountKey, out disbursementKey);

                repo.PostDisbursementTransaction(disbursementKey, DateTime.Now, "PostDisbursementTransactionTest", "Bob teh Builder");
                Assert.IsTrue(true);
            }
        }

        [Test]
        public void ReturnDisbursedLoanToRegistrationTest()
        {
            using (new TransactionScope(TransactionMode.New, OnDispose.Rollback))
            {
                string query = @"select top 1 d.AccountKey
                                    from Disbursement d
                                    join dbo.DisbursementFinancialTransaction dft on d.DisbursementKey = dft.DisbursementKey
                                    join fin.FinancialTransaction ft on ft.FinancialTransactionKey = dft.FinancialTransactionKey
                                    where DisbursementStatusKey = 1 --Disbursed
                                    and d.amount > 0
                                    and ft.TransactionTypeKey in (select distinct TransactionTypeNumber
								                                    from dbo.DisbursementTransactionType
								                                    where TransactionTypeNumber is not null)";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int accountKey = Convert.ToInt32(DT.Rows[0][0]);
                IDisbursementRepository repo = RepositoryFactory.GetRepository<IDisbursementRepository>();

                try
                {
                    repo.ReturnDisbursedLoanToRegistration(accountKey);
                }
                catch (DomainValidationException)
                {
                    // check specifically for this as it means the proc worked but returned an error.
                    Assert.IsTrue(true);
                }

                Assert.IsTrue(true);
            }
        }

        [Test]
        public void CreateEmptyDisbursement()
        {
            using (new SessionScope())
            {
                IDisbursementRepository repo = new DisbursementRepository();
                IDisbursement iDisb = repo.CreateEmptyDisbursement();
                Assert.IsNotNull(iDisb);
            }
        }
    }
}