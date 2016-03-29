using Castle.ActiveRecord;
using NUnit.Framework;
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
using SAHL.Test;
using System;
using System.Data;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class AccountTest : TestBase
    {
        [Test]
        public void AccountType()
        {
            using (new SessionScope())
            {
                string query = "select top 1 a.AccountKey from [2AM].[dbo].[Account] a (nolock) "
                    + "join [2AM].[dbo].[FinancialService] fs (nolock) on fs.AccountKey = a.AccountKey "
                    + "where a.RRR_ProductKey = 9 and a.accountstatuskey = 1";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccount acc = BMTM.GetMappedType<IAccount, Account_DAO>(Account_DAO.Find(DT.Rows[0][0]));
                AccountTypes at = acc.AccountType;
                Assert.That(at == AccountTypes.MortgageLoan);
            }
        }

        [Test]
        public void GetLatestOpenApplicationByType()
        {
            using (new SessionScope())
            {
                string query = "select top 1 a.AccountKey, count(o.OfferKey) "
                    + "from [2AM].[dbo].[Account] a (nolock) "
                    + "join [2AM].[dbo].[Offer] o (nolock) on o.AccountKey = a.AccountKey "
                    + "where o.OfferTypeKey in (6) "
                    + "group by a.AccountKey order by count(o.OfferKey) desc";
                DataTable DT = base.GetQueryResults(query);
                if (DT.Rows.Count != 1)
                    Assert.Ignore("No Data Found for test.");

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccount acc = BMTM.GetMappedType<IAccount, Account_DAO>(Account_DAO.Find(DT.Rows[0][0]));
                IApplication app = acc.GetLatestApplicationByType(OfferTypes.SwitchLoan);

                query = "select top 1 o.OfferKey from [2AM].[dbo].[Offer] o (nolock) "
                    + "where o.AccountKey = " + Convert.ToString(DT.Rows[0][0]) + " order by o.OfferKey desc";
                DT = base.GetQueryResults(query);

                if (app == null)
                    Assert.That(DT.Rows.Count == 0);
                else
                    Assert.That(app.Key == Convert.ToInt32(DT.Rows[0][0]));
            }
        }

        [Test]
        public void HasBeenInArrears()
        {
            int arrearMonths = 18, fsKey = 0;
            float minimum = 1000;
            bool arrears = false;

            using (DBHelper dbHelper = new DBHelper(Databases.TwoAM))
            {
                string query = String.Format(@" select top 1 at.FinancialServiceKey
                                                from [2AM].fin.ArrearTransaction at (nolock)
                                                join [2AM]..financialservice fs (nolock) on fs.FinancialServiceKey = at.FinancialServiceKey
                                                    and fs.FinancialServiceTypeKey = 8
                                                where at.TransactionTypekey = 910
                                                and at.balance > 1000
                                                and datediff(month, at.EffectiveDate, GETDATE()) < {0}
                                                and fs.AccountStatusKey = 1", arrearMonths.ToString()); // find account that went into arrears less than 18 months ago

                using (IDbCommand cmd = dbHelper.CreateCommand(query))
                {
                    cmd.CommandTimeout = 90;

                    var reader = dbHelper.ExecuteReader(cmd);

                    if (reader.Read())
                    {
                        fsKey = reader.GetInt32(0);
                    }
                }
            }

            if (fsKey > 0)
            {
                using (new SessionScope())
                {
                    FinancialService_DAO fs = FinancialService_DAO.Find(fsKey);

                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    IAccount acc = BMTM.GetMappedType<IAccount, Account_DAO>(fs.Account);
                    arrears = acc.HasBeenInArrears(arrearMonths, minimum);
                }
                Assert.That(arrears == true);
            }
            else
                Assert.Inconclusive("no data");
        }

        [Test]
        public void HasNotBeenInArrearsForPastNMonths()
        {
            int monthsSinceLastArrears = -6, fsKey = 0;

            using (DBHelper dbHelper = new DBHelper(Databases.TwoAM))
            {
                string query = String.Format(@"select top 1 at.FinancialServiceKey
                                        from [2AM].dbo.FinancialService fs (nolock)
                                        join (select max(ArrearTransactionKey) as ArrearTransactionKey, FinancialServiceKey
		                                        from [2am].fin.ArrearTransaction (nolock)
		                                        where TransactionTypeKey = 910
		                                        and Balance > 200
		                                        and EffectiveDate < dateadd(month, {0}, getdate())
		                                        group by FinancialServiceKey) at on at.FinancialServiceKey = fs.FinancialServiceKey
                                        join [2am].fin.ArrearTransaction at2 (nolock) on at2.ArrearTransactionKey = at.ArrearTransactionKey
                                        where fs.FinancialServiceTypeKey = 8
                                        and fs.AccountStatusKey = 1", monthsSinceLastArrears);

                using (IDbCommand cmd = dbHelper.CreateCommand(query))
                {
                    cmd.CommandTimeout = 90;

                    var reader = dbHelper.ExecuteReader(cmd);

                    if (reader.Read())
                    {
                        fsKey = reader.GetInt32(0);
                    }
                }
            }

            if (fsKey > 0)
            {
                using (new SessionScope())
                {
                    FinancialService_DAO fs = FinancialService_DAO.Find(fsKey);

                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    IAccount acc = BMTM.GetMappedType<IAccount, Account_DAO>(fs.Account);
                    bool arrears = acc.HasBeenInArrears(monthsSinceLastArrears - 1, 200.0f);

                    Assert.That(arrears == false);
                }
            }
            else
                Assert.Inconclusive("no data");
        }

        [Test]
        public void GetEmploymentType()
        {
            using (new SessionScope())
            {
                string query = "Select top 1 r.AccountKey From [2AM].[dbo].Employment e (nolock) "
                    + "join [2AM].[dbo].[Role] r (nolock) on r.LegalEntityKey = e.LegalEntityKey "
                    + "and r.GeneralStatusKey = 1 and r.RoleTypeKey in (2,3) "
                    + "and e.EmploymentStatusKey = 1 and e.ConfirmedIncome > 0 and e.EmploymentTypeKey is not null "
                    + "Group By r.AccountKey, e.EmploymentTypeKey Order by count(r.AccountKey) desc";
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccount acc = BMTM.GetMappedType<IAccount, Account_DAO>(Account_DAO.Find(DT.Rows[0][0]));

                DomainMessageCollection messages = new DomainMessageCollection();
                int et = acc.GetEmploymentTypeKey();

                query = "Select top 1 e.EmploymentTypeKey From [2AM].[dbo].Employment e (nolock) "
                    + "join [2AM].[dbo].[Role] r (nolock) on r.LegalEntityKey = e.LegalEntityKey "
                    + String.Format("where r.AccountKey = {0}", acc.Key.ToString())
                    + "and r.GeneralStatusKey = 1 and r.RoleTypeKey in (2,3) "
                    + "and e.EmploymentStatusKey = 1 and e.ConfirmedIncome > 0 and e.EmploymentTypeKey is not null "
                    + "Group By r.AccountKey, e.EmploymentTypeKey Order by sum(e.ConfirmedBasicIncome) desc";

                DT = base.GetQueryResults(query);

                Assert.That((int)DT.Rows[0][0] == et);
            }
        }

        [Test]
        public void GetHouseholdIncome()
        {
            using (new SessionScope())
            {
                string query = "SELECT top 1 R.AccountKey,  isnull(sum(isnull(e.ConfirmedIncome, 0)), 0)"
                    + "FROM [2AM].[dbo].[Employment] E (nolock) "
                    + "INNER JOIN [2AM].[dbo].[LegalEntity] LE (nolock) ON LE.LegalEntityKey = E.LegalEntityKey "
                    + "INNER JOIN [2AM].[dbo].[Role] R (nolock) ON R.LegalEntityKey = LE.LegalEntityKey "
                    + "INNER JOIN [2AM].[dbo].[Account] A (nolock) ON A.AccountKey = R.AccountKey "
                    + "WHERE R.RoleTypeKey IN (2, 3) AND R.GeneralStatusKey = 1 AND E.EmploymentStatusKey = 1 AND A.RRR_ProductKey = 1 AND E.EmploymentTypeKey is not null "
                    + "group by R.AccountKey order by count(R.LegalEntityKey) desc";
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);
                DataRow row = DT.Rows[0];

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                object key = DT.Rows[0][0];
                IAccountVariableLoan acc = BMTM.GetMappedType<IAccountVariableLoan, AccountVariableLoan_DAO>(AccountVariableLoan_DAO.Find(key));

                double d = acc.GetHouseholdIncome();

                Assert.That(d == (double)row[1]);
            }
        }

        [Test]
        public void GetNonProspectRelatedAccounts()
        {
            using (new SessionScope())
            {
                string query = "select top 1 ar.ParentAccountKey, count(ar.AccountKey) "
                    + "from [2AM].[dbo].[Account] ar (nolock) "
                    + "join [2AM].[dbo].[Account] acc (nolock) on acc.AccountKey = ar.ParentAccountKey "
                    + "where acc.AccountStatusKey <> 6 "
                    + "group by ar.ParentAccountKey order by count(ar.ParentAccountKey) desc";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccount acc = BMTM.GetMappedType<IAccount, Account_DAO>(Account_DAO.Find(DT.Rows[0][0]));
                IEventList<IAccount> list = acc.GetNonProspectRelatedAccounts();

                Assert.That(list.Count == (int)DT.Rows[0][1]);
            }
        }

        [Test]
        public void GetHOC()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 fs.AccountKey, hoc.FinancialServiceKey
                    from [2AM].[dbo].[HOC] hoc (nolock)
                    join [2AM].[dbo].[FinancialService] fs (nolock) on fs.FinancialServiceKey = hoc.FinancialServiceKey
                    where fs.FinancialServiceTypeKey = 4
                    and fs.AccountStatusKey in (1,5)";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);
                DataRow row = DT.Rows[0];
                int hocAccountKey = (int)row[0];
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccount hoc = BMTM.GetMappedType<IAccount, Account_DAO>(Account_DAO.Find(hocAccountKey));
                Assert.That(hoc != null);
                Assert.That(hoc.Key == hocAccountKey);
            }
        }

        [Test]
        public void GetFinancialServiceByType()
        {
            using (new SessionScope())
            {
                IDomainMessageCollection messages = new DomainMessageCollection();

                ParameterCollection parameters = new ParameterCollection();
                IDbConnection con = Helper.GetSQLDBConnection();
                DataTable DT = new DataTable();
                string query = "Select top 1 fs.AccountKey from [2AM].[dbo].[FinancialService] fs (nolock) "
                    + "where fs.AccountStatusKey = 1 and fs.FinancialServiceTypeKey = 5";
                Helper.FillFromQuery(DT, query, con, parameters);

                Assert.That(DT.Rows.Count == 1, "No Data found matching the query");
                DataRow row = DT.Rows[0];

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccount acc = BMTM.GetMappedType<IAccount, Account_DAO>(Account_DAO.Find(DT.Rows[0][0]));
                IFinancialService fs = acc.GetFinancialServiceByType(FinancialServiceTypes.LifePolicy);

                Assert.That(fs != null);
                Assert.That(fs.AccountStatus.Key == 1);
                Assert.That(fs.FinancialServiceType.Key == 5);
            }
        }

        [Test]
        public void GetFinancialServicesByType()
        {
            using (new SessionScope())
            {
                IDomainMessageCollection messages = new DomainMessageCollection();

                ParameterCollection parameters = new ParameterCollection();
                IDbConnection con = Helper.GetSQLDBConnection();
                DataTable DT = new DataTable();
                string query = "Select top 1 fs.* from [2AM].[dbo].[FinancialService] fs (nolock) "
                    + "where fs.AccountStatusKey = 1 and fs.FinancialServiceTypeKey = 5";
                Helper.FillFromQuery(DT, query, con, parameters);

                Assert.That(DT.Rows.Count == 1, "No Data found matching the query");
                DataRow row = DT.Rows[0];

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccount acc = BMTM.GetMappedType<IAccount, Account_DAO>(Account_DAO.Find(row[1]));
                IReadOnlyEventList<IFinancialService> list = acc.GetFinancialServicesByType(FinancialServiceTypes.LifePolicy, new AccountStatuses[] { AccountStatuses.Open });

                Assert.That(list.Count > 0);
            }
        }

        [Test]
        public void GetFinancialServicesByTypeAndAccountStatus()
        {
            using (new SessionScope())
            {
                IDomainMessageCollection messages = new DomainMessageCollection();

                ParameterCollection parameters = new ParameterCollection();
                IDbConnection con = Helper.GetSQLDBConnection();
                DataTable DT = new DataTable();
                string query = "Select top 1 fs.* from [2AM].[dbo].[FinancialService] fs (nolock) "
                    + "where fs.AccountStatusKey = 1 and fs.FinancialServiceTypeKey = 5";
                Helper.FillFromQuery(DT, query, con, parameters);

                Assert.That(DT.Rows.Count == 1, "No Data found matching the query");
                DataRow row = DT.Rows[0];

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccount acc = BMTM.GetMappedType<IAccount, Account_DAO>(Account_DAO.Find(row[1]));
                IReadOnlyEventList<IFinancialService> list = acc.GetFinancialServicesByType(FinancialServiceTypes.LifePolicy, new AccountStatuses[] { AccountStatuses.Open });

                Assert.That(list.Count > 0);
            }
        }

        [Test]
        public void GetFinancialServicesByTypeWithActiveStatus()
        {
            using (new SessionScope())
            {
                IDomainMessageCollection messages = new DomainMessageCollection();

                ParameterCollection parameters = new ParameterCollection();
                IDbConnection con = Helper.GetSQLDBConnection();
                DataTable DT = new DataTable();
                string query = "Select top 1 fs.* from [2AM].[dbo].[FinancialService] fs (nolock) "
                    + "where (fs.AccountStatusKey = 1 or fs.AccountStatusKey = 5) and fs.FinancialServiceTypeKey = 5";
                Helper.FillFromQuery(DT, query, con, parameters);

                Assert.That(DT.Rows.Count == 1, "No Data found matching the query");
                DataRow row = DT.Rows[0];

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccount acc = BMTM.GetMappedType<IAccount, Account_DAO>(Account_DAO.Find(row[1]));
                IReadOnlyEventList<IFinancialService> list = acc.GetFinancialServicesByType(FinancialServiceTypes.LifePolicy, new AccountStatuses[] { AccountStatuses.Open, AccountStatuses.Dormant });

                Assert.That(list.Count > 0);
            }
        }

        //[Ignore]
        [Test]
        public void AccountGetFSByType()
        {
            using (new SessionScope())
            {
                Account_DAO Acc = Account_DAO.Find(1308725);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccount A = BMTM.GetMappedType<IAccount, Account_DAO>(Acc);
                SAHL.Common.Globals.AccountStatuses[] aStatus = new SAHL.Common.Globals.AccountStatuses[] { AccountStatuses.Open };
                IReadOnlyEventList<IFinancialService> fs = A.GetFinancialServicesByType(SAHL.Common.Globals.FinancialServiceTypes.HomeOwnersCover, aStatus);
                for (int i = 0; i < fs.Count; i++)
                {
                    string Status = fs[i].AccountStatus.ToString();
                }
            }
        }

        /// <summary>
        /// Tests the specific Variable loan properties.
        /// </summary>
        [Test]
        public void AccountVariableLoanProperties()
        {
            using (new SessionScope())
            {
                int AccTypeKey = 1; // it's a variable loan account.

                string query = String.Format(@"SELECT   top 1  fs.AccountKey
                    FROM fin.MortgageLoan AS mort (nolock)
                    INNER JOIN FinancialService AS fs (nolock) ON mort.FinancialServiceKey = fs.FinancialServiceKey
                    INNER JOIN Account AS act (nolock) ON fs.AccountKey = act.AccountKey
                    WHERE     (act.RRR_ProductKey = {0})
                    order by fs.AccountKey desc ", AccTypeKey);
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                int key = Convert.ToInt32(DT.Rows[0][0]);

                AccountVariableLoan_DAO Acc = (AccountVariableLoan_DAO)AccountVariableLoan_DAO.Find(key);
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccountVariableLoan IVA = BMTM.GetMappedType<IAccountVariableLoan>(Acc);
                IMortgageLoan ML = IVA.SecuredMortgageLoan;

                IMortgageLoan IML = IVA.SecuredMortgageLoan;

                Assert.AreEqual(ML.Key, IML.Key);

                // Unsecured Loans.
                IReadOnlyEventList<IMortgageLoan> UnsecuredLoans = IVA.UnsecuredMortgageLoans;
                Assert.IsNotNull(UnsecuredLoans);

                // IMortgageLoanAccount
                Assert.IsTrue(IVA is IMortgageLoanAccount);
                IMortgageLoanAccount IMLA = IVA as IMortgageLoanAccount;
                IML = null;
                IML = IMLA.SecuredMortgageLoan;
                Assert.IsNotNull(IML);
                UnsecuredLoans = null;
                UnsecuredLoans = IMLA.UnsecuredMortgageLoans;
                Assert.IsNotNull(UnsecuredLoans);

                // Main Applicants and Suretors
                Assert.IsNotNull(IMLA.MainApplicants);
                Assert.IsNotNull(IMLA.Suretors);
                if (IVA.AccountStatus.Description == "Open")
                    Assert.IsTrue(IMLA.MainApplicants.Count > 0);
                else
                {
                    Assert.IsTrue(IMLA.MainApplicants.Count >= 0);
                }

                Assert.IsTrue(IMLA.Suretors.Count >= 0);
            }
        }

        /// <summary>
        /// tests specific properties for a varifix loan.
        /// </summary>
        [Test]
        public void VariFixAccountProperties()
        {
            using (new SessionScope())
            {
                int AccTypeKey = 2; // it's a varifix loan account.

                string query = String.Format(@"SELECT   top 1  fs.AccountKey
                    FROM fin.MortgageLoan AS mort (nolock)
                    INNER JOIN FinancialService AS fs (nolock) ON mort.FinancialServiceKey = fs.FinancialServiceKey
                    INNER JOIN Account AS act (nolock) ON fs.AccountKey = act.AccountKey
                    WHERE     (act.RRR_ProductKey = {0})
                    order by fs.AccountKey desc ", AccTypeKey);
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                int key = Convert.ToInt32(DT.Rows[0][0]);

                AccountVariFixLoan_DAO Acc = (AccountVariFixLoan_DAO)AccountVariFixLoan_DAO.Find(key);
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccountVariFixLoan IVA = BMTM.GetMappedType<IAccountVariFixLoan>(Acc);
                IMortgageLoan ML = IVA.SecuredMortgageLoan;

                IMortgageLoan IML = IVA.SecuredMortgageLoan;

                Assert.AreEqual(ML.Key, IML.Key);

                // Unsecured Loans.
                IReadOnlyEventList<IMortgageLoan> UnsecuredLoans = IVA.UnsecuredMortgageLoans;
                Assert.IsNotNull(UnsecuredLoans);

                // IMortgageLoanAccount
                Assert.IsTrue(IVA is IMortgageLoanAccount);
                IMortgageLoanAccount IMLA = IVA as IMortgageLoanAccount;
                IML = null;
                IML = IMLA.SecuredMortgageLoan;
                Assert.IsNotNull(IML);
                UnsecuredLoans = null;
                UnsecuredLoans = IMLA.UnsecuredMortgageLoans;
                Assert.IsNotNull(UnsecuredLoans);

                // Main Applicants and Suretors
                Assert.IsNotNull(IMLA.MainApplicants);
                Assert.IsNotNull(IMLA.Suretors);
                if (IVA.AccountStatus.Description == "Open")
                    Assert.IsTrue(IMLA.MainApplicants.Count > 0);
                else
                {
                    Assert.IsTrue(IMLA.MainApplicants.Count >= 0);
                }

                Assert.IsTrue(IMLA.Suretors.Count >= 0);
            }
        }

        /// <summary>
        /// Tests the specific Superlo loan properties.
        /// </summary>
        [Test]
        public void AccountSuperLoProperties()
        {
            using (new SessionScope())
            {
                int AccTypeKey = 5; // it's a superlo loan account.

                string query = String.Format(@"SELECT   top 1  fs.AccountKey
                    FROM fin.MortgageLoan AS mort (nolock)
                    INNER JOIN FinancialService AS fs (nolock) ON mort.FinancialServiceKey = fs.FinancialServiceKey
                    INNER JOIN Account AS act (nolock) ON fs.AccountKey = act.AccountKey
                    WHERE     (act.RRR_ProductKey = {0})
                    order by fs.AccountKey desc ", AccTypeKey);
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                int key = Convert.ToInt32(DT.Rows[0][0]);

                AccountSuperLo_DAO Acc = (AccountSuperLo_DAO)AccountSuperLo_DAO.Find(key);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccountSuperLo IVA = BMTM.GetMappedType<IAccountSuperLo>(Acc);
                IMortgageLoan ML = IVA.SecuredMortgageLoan;

                IMortgageLoan IML = IVA.SecuredMortgageLoan;

                Assert.AreEqual(ML.Key, IML.Key);

                // Unsecured Loans.
                IReadOnlyEventList<IMortgageLoan> UnsecuredLoans = IVA.UnsecuredMortgageLoans;
                Assert.IsNotNull(UnsecuredLoans);

                // IMortgageLoanAccount
                Assert.IsTrue(IVA is IMortgageLoanAccount);
                IMortgageLoanAccount IMLA = IVA as IMortgageLoanAccount;
                IML = null;
                IML = IMLA.SecuredMortgageLoan;
                Assert.IsNotNull(IML);
                UnsecuredLoans = null;
                UnsecuredLoans = IMLA.UnsecuredMortgageLoans;
                Assert.IsNotNull(UnsecuredLoans);

                // Main Applicants and Suretors
                Assert.IsNotNull(IMLA.MainApplicants);
                Assert.IsNotNull(IMLA.Suretors);
                if (IVA.AccountStatus.Description == "Open")
                    Assert.IsTrue(IMLA.MainApplicants.Count > 0);
                else
                {
                    Assert.IsTrue(IMLA.MainApplicants.Count >= 0);
                }

                Assert.IsTrue(IMLA.Suretors.Count >= 0);
            }
        }

        /// <summary>
        /// Tests the specific defending discount account properties.
        /// </summary>
        [Test]
        public void DefendingDiscountAccountProperties()
        {
            using (new SessionScope())
            {
                int AccTypeKey = 6; // it's a Defendign discount loan account.

                string query = String.Format(@"SELECT   top 1  fs.AccountKey
                    FROM fin.MortgageLoan AS mort (nolock)
                    INNER JOIN FinancialService AS fs (nolock) ON mort.FinancialServiceKey = fs.FinancialServiceKey
                    INNER JOIN Account AS act (nolock) ON fs.AccountKey = act.AccountKey
                    WHERE     (act.RRR_ProductKey = {0})
                    order by fs.AccountKey desc ", AccTypeKey);
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                int key = Convert.ToInt32(DT.Rows[0][0]);

                AccountDefendingDiscountRate_DAO Acc = (AccountDefendingDiscountRate_DAO)AccountDefendingDiscountRate_DAO.Find(key);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccountDefendingDiscountRate IVA = BMTM.GetMappedType<IAccountDefendingDiscountRate>(Acc);
                IMortgageLoan ML = IVA.SecuredMortgageLoan;

                IMortgageLoan IML = IVA.SecuredMortgageLoan;

                Assert.AreEqual(ML.Key, IML.Key);

                // Unsecured Loans.
                IReadOnlyEventList<IMortgageLoan> UnsecuredLoans = IVA.UnsecuredMortgageLoans;
                Assert.IsNotNull(UnsecuredLoans);

                // IMortgageLoanAccount
                Assert.IsTrue(IVA is IMortgageLoanAccount);
                IMortgageLoanAccount IMLA = IVA as IMortgageLoanAccount;
                IML = null;
                IML = IMLA.SecuredMortgageLoan;
                Assert.IsNotNull(IML);
                UnsecuredLoans = null;
                UnsecuredLoans = IMLA.UnsecuredMortgageLoans;
                Assert.IsNotNull(UnsecuredLoans);

                // Main Applicants and Suretors
                Assert.IsNotNull(IMLA.MainApplicants);
                Assert.IsNotNull(IMLA.Suretors);
                if (IVA.AccountStatus.Description == "Open")
                    Assert.IsTrue(IMLA.MainApplicants.Count > 0);
                else
                {
                    Assert.IsTrue(IMLA.MainApplicants.Count >= 0);
                }

                Assert.IsTrue(IMLA.Suretors.Count >= 0);
            }
        }

        /// <summary>
        /// Tests the specific new variable loan account properties.
        /// </summary>

        [Test]
        public void AccountNewVariableLoanProperties()
        {
            using (new SessionScope())
            {
                int AccTypeKey = 9; // it's a Defendign discount loan account.

                string query = String.Format(@"SELECT   top 1  fs.AccountKey
                    FROM fin.MortgageLoan AS mort (nolock)
                    INNER JOIN FinancialService AS fs (nolock) ON mort.FinancialServiceKey = fs.FinancialServiceKey
                    INNER JOIN Account AS act (nolock) ON fs.AccountKey = act.AccountKey
                    WHERE     (act.RRR_ProductKey = {0})
                    order by fs.AccountKey desc ", AccTypeKey);
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                int key = Convert.ToInt32(DT.Rows[0][0]);

                AccountNewVariableLoan_DAO Acc = (AccountNewVariableLoan_DAO)AccountNewVariableLoan_DAO.Find(key);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccountNewVariableLoan IVA = BMTM.GetMappedType<IAccountNewVariableLoan>(Acc);
                IMortgageLoan ML = IVA.SecuredMortgageLoan;

                IMortgageLoan IML = IVA.SecuredMortgageLoan;

                Assert.AreEqual(ML.Key, IML.Key);

                // Unsecured Loans.
                IReadOnlyEventList<IMortgageLoan> UnsecuredLoans = IVA.UnsecuredMortgageLoans;
                Assert.IsNotNull(UnsecuredLoans);

                // IMortgageLoanAccount
                Assert.IsTrue(IVA is IMortgageLoanAccount);
                IMortgageLoanAccount IMLA = IVA as IMortgageLoanAccount;
                IML = null;
                IML = IMLA.SecuredMortgageLoan;
                Assert.IsNotNull(IML);
                UnsecuredLoans = null;
                UnsecuredLoans = IMLA.UnsecuredMortgageLoans;
                Assert.IsNotNull(UnsecuredLoans);

                // Main Applicants and Suretors
                Assert.IsNotNull(IMLA.MainApplicants);
                Assert.IsNotNull(IMLA.Suretors);
                if (IVA.AccountStatus.Description == "Open")
                    Assert.IsTrue(IMLA.MainApplicants.Count > 0);
                else
                {
                    Assert.IsTrue(IMLA.MainApplicants.Count >= 0);
                }

                Assert.IsTrue(IMLA.Suretors.Count >= 0);
            }
        }

        /// <summary>
        /// Tests the specific HOC account properties.
        /// </summary>
        [Test]
        public void HOCAccountProperties()
        {
            using (new SessionScope())
            {
                int AccTypeKey = 3; // it's a HOC account.

                object Key = base.GetPrimaryKey("Account", "AccountKey", String.Format(" RRR_ProductKey = {0}", AccTypeKey));

                AccountHOC_DAO Acc = (AccountHOC_DAO)AccountHOC_DAO.Find(Key);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccountHOC IHOCAcc = BMTM.GetMappedType<IAccountHOC>(Acc);

                IHOC IHoc = IHOCAcc.HOC;
                Assert.IsTrue(IHoc.FinancialService.FinancialServiceType.Key == 4);
            }
        }

        /// <summary>
        /// Tests the specific Life account properties.
        /// </summary>
        [Test]
        public void LifeAccountProperties()
        {
            using (new SessionScope())
            {
                //int AccTypeKey = 4; // it's a HOC account.

                string sql = @"select top 1 acc.AccountKey
                    from Account acc (nolock)
                    join FinancialService (nolock) fs on fs.AccountKey = acc.AccountKey
                    where RRR_ProductKey = 4 and acc.AccountStatusKey = 1";

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                object Key = Convert.ToInt32(obj); //base.GetPrimaryKey("Account", "AccountKey", String.Format(" RRR_ProductKey = {0}", AccTypeKey));

                AccountLifePolicy_DAO Acc = AccountLifePolicy_DAO.Find(Key);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccountLifePolicy ILifePolicyAcc = BMTM.GetMappedType<IAccountLifePolicy>(Acc);

                ILifePolicy ILifePol = ILifePolicyAcc.LifePolicy;
                Assert.IsTrue(ILifePol.FinancialService.FinancialServiceType.Key == 5);
            }
        }

        /// <summary>
        /// Test to ensure that you cannot add a detail with detail types LoanClosed or PaidUpWithNoHOC to
        /// an account with a loan balance > 0
        /// </summary>
        [Test]
        public void DetailAddCurrentBalanceCheck()
        {
            CurrentPrincipalCache.DomainMessages.Clear();
            IDomainMessageCollection dmc = CurrentPrincipalCache.DomainMessages;

            string sql = String.Format(@"select top 1 * from [2AM].[dbo].Account a
                inner join [2AM].[dbo].FinancialService fs ON a.AccountKey = fs.AccountKey
                inner join [2AM].[fin].Balance bal ON bal.FinancialServiceKey = fs.FinancialServiceKey
                WHERE bal.Amount > 0
                AND a.RRR_ProductKey = {0}
                AND bal.BalanceTypeKey = 1
				AND fs.ParentFinancialServiceKey is null", (int)Products.VariableLoan);

            DataTable dt = base.GetQueryResults(sql);
            int accountKey = Convert.ToInt32(dt.Rows[0][0]);

            using (new SessionScope(FlushAction.Never))
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccountVariableLoan account = BMTM.GetMappedType<IAccountVariableLoan>(AccountVariableLoan_DAO.Find(accountKey));

                try
                {
                    // add LoanClosed detail to the account
                    IDetail detail = BMTM.GetMappedType<IDetail>(new Detail_DAO());
                    IDetailType detailType = BMTM.GetMappedType<IDetailType>(new DetailType_DAO());
                    detailType.Key = (int)DetailTypes.LoanClosed;
                    detail.DetailType = detailType;
                    account.Details.Add(dmc, detail);
                }
                catch (Exception)
                {
                    //do nothing, this should hit an error
                }

                try
                {
                    // add PaidUpWithNoHOC detail to the account
                    IDetail detail2 = BMTM.GetMappedType<IDetail>(new Detail_DAO());
                    IDetailType detailType2 = BMTM.GetMappedType<IDetailType>(new DetailType_DAO());
                    detailType2.Key = (int)DetailTypes.PaidUpWithNoHOC;
                    detail2.DetailType = detailType2;
                    account.Details.Add(dmc, detail2);
                }
                catch (Exception)
                {
                    //do nothing, this should hit an error
                }
            }
            Assert.AreEqual(dmc.ErrorMessages.Count, 2);
            dmc.Clear();
        }

        /// <summary>
        /// Test to ensure that you cannot add a detail with detail types BankDetailsIncorrect or DebitOrderSuspended to
        /// an account where the mortgage loan debit order date is today
        /// </summary>
        [Test]
        public void DetailAddDebitOrderDateCheck()
        {
            CurrentPrincipalCache.DomainMessages.Clear();
            IDomainMessageCollection dmc = CurrentPrincipalCache.DomainMessages;
            string query = String.Format(@"SELECT   top 1  fs.AccountKey
                    FROM fin.MortgageLoan AS mort (nolock)
                    INNER JOIN FinancialService AS fs (nolock) ON mort.FinancialServiceKey = fs.FinancialServiceKey
                    INNER JOIN Account AS act (nolock) ON fs.AccountKey = act.AccountKey
                    WHERE     (act.RRR_ProductKey = {0})
                    order by fs.AccountKey desc ", (int)Products.VariableLoan);
            DataTable DT = base.GetQueryResults(query);

            // Assert.That(DT.Rows.Count == 1);

            //  DataTable dt = base.GetQueryResults(sql);
            int accountKey = Convert.ToInt32(DT.Rows[0][0]);

            bool isDebitOrderDay = false;

            using (new SessionScope(FlushAction.Never))
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccountVariableLoan account = BMTM.GetMappedType<IAccountVariableLoan>(AccountVariableLoan_DAO.Find(accountKey));

                // check the bank accounts to see if there is a debit order date
                foreach (IFinancialServiceBankAccount bankAccount in account.SecuredMortgageLoan.FinancialServiceBankAccounts)
                {
                    if (bankAccount.DebitOrderDay == DateTime.Now.Day)
                    {
                        isDebitOrderDay = true;
                        break;
                    }
                }

                try
                {
                    // add LoanClosed detail to the account
                    IDetail detail = BMTM.GetMappedType<IDetail>(new Detail_DAO());
                    IDetailType detailType = BMTM.GetMappedType<IDetailType>(new DetailType_DAO());
                    detailType.Key = (int)DetailTypes.BankDetailsIncorrect;
                    detail.DetailType = detailType;
                    account.Details.Add(dmc, detail);
                }
                catch (Exception)
                {
                    //do nothing, this will throw an error
                }

                try
                {
                    // add PaidUpWithNoHOC detail to the account
                    IDetail detail2 = BMTM.GetMappedType<IDetail>(new Detail_DAO());
                    IDetailType detailType2 = BMTM.GetMappedType<IDetailType>(new DetailType_DAO());
                    detailType2.Key = (int)DetailTypes.DebitOrderSuspended;
                    detail2.DetailType = detailType2;
                    account.Details.Add(dmc, detail2);
                }
                catch (Exception)
                {
                    //do nothing, this will throw an error
                }
            }

            Assert.AreEqual(dmc.ErrorMessages.Count, (isDebitOrderDay ? 2 : 0));
            dmc.Clear();
        }

        /// <summary>
        /// Test to ensure that you cannot add a detail with detail type ReadvanceInProgress to an account
        /// with a Super Lo application in progress
        /// </summary>
        [Test]
        public void DetailAddReadvanceInProgressWithSuperLoApplication()
        {
            CurrentPrincipalCache.DomainMessages.Clear();
            IDomainMessageCollection dmc = CurrentPrincipalCache.DomainMessages;

            using (new SessionScope(FlushAction.Never))
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccountVariableLoan account = BMTM.GetMappedType<IAccountVariableLoan>(new AccountVariableLoan_DAO());

                IApplicationMortgageLoanSwitch application = BMTM.GetMappedType<IApplicationMortgageLoanSwitch>(new ApplicationMortgageLoanSwitch_DAO());
                IApplicationStatus applicationStatus = BMTM.GetMappedType<IApplicationStatus>(new ApplicationStatus_DAO());
                IApplicationInformation appInfo = BMTM.GetMappedType<IApplicationInformation>(new ApplicationInformation_DAO());
                appInfo.Product = LookupRepository.Products.ObjectDictionary[((int)Products.SuperLo).ToString()];

                application.ApplicationStatus = applicationStatus;
                application.ApplicationInformations.Add(dmc, appInfo);
                applicationStatus.Key = (int)OfferStatuses.Open;
                account.Applications.Add(dmc, application);

                try
                {
                    // add the detail to the account
                    IDetail detail = BMTM.GetMappedType<IDetail>(new Detail_DAO());
                    IDetailType detailType = BMTM.GetMappedType<IDetailType>(new DetailType_DAO());
                    detailType.Key = (int)DetailTypes.ReadvanceInProgress;
                    detail.DetailType = detailType;
                    account.Details.Add(dmc, detail);
                }
                catch (Exception)
                {
                    //do nothing, this will throw an error
                }
            }

            Assert.AreEqual(dmc.ErrorMessages.Count, 1);
            dmc.Clear();
        }

        /// <summary>
        /// Test to ensure that you cannot remove a detail with a detail type having AllowUpdate = 0
        /// </summary>
        [Test]
        public void DetailRemoveUpdateCheck()
        {
            CurrentPrincipalCache.DomainMessages.Clear();
            IDomainMessageCollection dmc = CurrentPrincipalCache.DomainMessages;

            string sql = String.Format(@"select top 1 a.AccountKey, d.DetailKey from Account a
                inner join Detail d on a.AccountKey = d.AccountKey
                inner join DetailType dt on d.DetailTypeKey = dt.DetailTypeKey
                where dt.AllowUpdate = 0
                and a.RRR_ProductKey = {0}", (int)Products.VariableLoan);

            DataTable dt = base.GetQueryResults(sql);
            int accountKey = Convert.ToInt32(dt.Rows[0][0]);
            int detailKey = Convert.ToInt32(dt.Rows[0][1]);

            using (new SessionScope(FlushAction.Never))
            {
                try
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    IAccountVariableLoan account = BMTM.GetMappedType<IAccountVariableLoan>(AccountVariableLoan_DAO.Find(accountKey));
                    IDetail detail = BMTM.GetMappedType<IDetail>(Detail_DAO.Find(detailKey));

                    account.Details.Remove(dmc, detail);
                }
                catch (Exception)
                {
                    //do nothing
                }
            }
            Assert.AreEqual(dmc.ErrorMessages.Count, 1);
            dmc.Clear();
        }

        /// <summary>
        /// Test to ensure that you cannot remove a detail with a detail type of LoanClosed
        /// </summary>
        [Test]
        public void DetailRemoveLoanClosed()
        {
            CurrentPrincipalCache.DomainMessages.Clear();
            IDomainMessageCollection dmc = CurrentPrincipalCache.DomainMessages;

            string sql = String.Format(@"select top 1 a.AccountKey, d.DetailKey
                from Account a
                inner join Detail d on a.AccountKey = d.AccountKey
                inner join DetailType dt on dt.DetailTypeKey = d.DetailTypeKey
                where dt.DetailTypeKey = {0}
                and a.RRR_ProductKey = {1}", (int)DetailTypes.LoanClosed, (int)Products.VariableLoan);

            DataTable dt = base.GetQueryResults(sql);
            int accountKey = Convert.ToInt32(dt.Rows[0][0]);
            int detailKey = Convert.ToInt32(dt.Rows[0][1]);

            using (new SessionScope(FlushAction.Never))
            {
                try
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    IAccountVariableLoan account = BMTM.GetMappedType<IAccountVariableLoan>(AccountVariableLoan_DAO.Find(accountKey));
                    IDetail detail = BMTM.GetMappedType<IDetail>(Detail_DAO.Find(detailKey));
                    account.Details.Remove(dmc, detail);
                }
                catch (Exception)
                {
                    //do nothing
                }
            }
            Assert.AreEqual(dmc.ErrorMessages.Count, 1);

            dmc.Clear();
        }

        /// <summary>
        /// Tests that you cannot remove details of type PaidUpWithNoHOC with accounts
        /// that are currently in the registration process.
        /// </summary>
        [Test]
        public void DetailRemoveFromLoanInRegistrationProcess()
        {
            CurrentPrincipalCache.DomainMessages.Clear();
            IDomainMessageCollection dmc = CurrentPrincipalCache.DomainMessages;

            string sql = String.Format(@"select top 1 a.AccountKey, count(a.AccountKey) as msgCount
                from Account a
                inner join Detail d on a.AccountKey = d.AccountKey
                inner join DetailType dt on dt.DetailTypeKey = d.DetailTypeKey
                inner join DetailClass dc on dc.DetailClassKey = dt.DetailClassKey
                where dc.DetailClassKey = {0}
                and a.RRR_ProductKey = {1}
				group by a.AccountKey ", (int)DetailClasses.RegistrationProcess, (int)Products.VariableLoan);

            DataTable dt = base.GetQueryResults(sql);

            int accountKey = Convert.ToInt32(dt.Rows[0][0]);
            int msgCount = Convert.ToInt32(dt.Rows[0][1]);

            using (new SessionScope(FlushAction.Never))
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccountVariableLoan account = BMTM.GetMappedType<IAccountVariableLoan>(AccountVariableLoan_DAO.Find(accountKey));

                try
                {
                    IDetail detail2 = account.Details[0];
                    detail2.DetailType.Key = (int)DetailTypes.PaidUpWithNoHOC;
                    detail2.DetailType.AllowUpdate = true;
                    detail2.DetailType.AllowUpdateDelete = true;
                    account.Details.Remove(dmc, detail2);
                }
                catch (Exception)
                {
                    //do nothing
                }
            }
            Assert.AreEqual(dmc.ErrorMessages.Count, msgCount);
            dmc.Clear();
        }

        [Test]
        public void GetAccountRoleNotificationByType()
        {
            using (new SessionScope(FlushAction.Never))
            {
                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                string sql = @"select distinct r.AccountKey, rd.ReasonTypeKey, rd.ReasonDescriptionKey, le.LegalEntityKey,
                    case
                                    when re.ReasonKey is null then 0
                                    else 1
                    end as NotificationExists
                    from [Role] r (nolock)
                    join [Account] a (nolock) on r.AccountKey=a.AccountKey and rrr_productkey not in (3,4)
                    join LegalEntity le (nolock) on r.LegalEntityKey = le.LegalEntityKey
                    join Reason re (nolock) on le.LegalEntityKey = re.GenericKey
                    join ReasonDefinition rd (nolock) on re.ReasonDefinitionKey = rd.ReasonDefinitionKey
                    join ReasonType rt (nolock) on rd.ReasonTypeKey = rt.ReasonTypeKey
                    where r.RoleTypeKey in (1, 2) and r.GeneralStatusKey = 1 --only legalentities on the account
                    and le.LegalEntityTypeKey = 2 and le.LegalEntityStatusKey != 3 --Natural Persons
                    and rt.GenericKeyTypeKey = 3";

                DataTable dt = base.GetQueryResults(sql);

                int count = dt.Rows.Count > 5 ? 5 : dt.Rows.Count;

                for (int i = 0; i < count; i++)
                {
                    IAccount acc = accRepo.GetAccountByKey((Int32)dt.Rows[i][0]);
                    DataTable adt = acc.GetAccountRoleNotificationByTypeAndDecription((ReasonTypes)(int)dt.Rows[i][1], (ReasonDescriptions)(int)dt.Rows[i][2]);

                    bool exists = false;

                    foreach (DataRow dr in adt.Rows)
                    {
                        if (Convert.ToBoolean(dr[1]))
                        {
                            exists = true;
                            break;
                        }
                    }

                    Assert.IsTrue(exists);
                    Assert.Greater(adt.Rows.Count, 0);
                }
            }
        }

        [Test]
        public void HasActiveSubsidy_Test_True()
        {
            using (new SessionScope(FlushAction.Never))
            {
                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                string sql = @"select top 1 accs.AccountKey 
                                from [2AM].dbo.AccountSubsidy accs
                                join [2AM].dbo.Subsidy s on accs.SubsidyKey = s.SubsidyKey
                                where s.GeneralStatusKey = 1
                                order by accs.AccountSubsidyKey";

                DataTable dt = base.GetQueryResults(sql);

                if (dt.Rows.Count == 1)
                {
                    int accountKey = 0;
                    if (int.TryParse(dt.Rows[0][0].ToString(), out accountKey))
                    {
                        IAccount account = accRepo.GetAccountByKey(accountKey);
                        Assert.IsTrue(account.HasActiveSubsidy);
                    }
                    else
                    {
                        Assert.Inconclusive("Bad data");
                    }
                }
                else
                {
                    Assert.Inconclusive("No data");
                }
            }
        }

        [Test]
        public void HasActiveSubsidy_Test_False()
        {
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

            using (new SessionScope(FlushAction.Never))
            {
                string sql = @"select top 1 r.AccountKey
                                from [2AM]..Role r (nolock)
                                join [2AM]..LegalEntity le (nolock) on le.LegalEntityKey = r.LegalEntityKey 
                                left join [2AM].[dbo].[Subsidy] s (nolock) on s.LegalEntityKey = le.LegalEntityKey
	                                and s.GeneralStatusKey = 1
                                where r.GeneralStatusKey = 1
                                and r.RoleTypeKey = 2
                                and s.LegalEntityKey is null
                                order by r.AccountKey desc";

                DataTable dt = base.GetQueryResults(sql);

                if (dt.Rows.Count == 1)
                {
                    int accountKey = 0;
                    if (int.TryParse(dt.Rows[0][0].ToString(), out accountKey))
                    {
                        IAccount account = accRepo.GetAccountByKey(accountKey);
                        Assert.IsFalse(account.HasActiveSubsidy);
                    }
                    else
                    {
                        Assert.Inconclusive("Bad data");
                    }
                }
                else
                {
                    Assert.Inconclusive("No data");
                }
            }
        }


        [Test]
        public void IsThirtyYearTerm_Test_True()
        {
            using (new SessionScope(FlushAction.Never))
            {
                string sql = @" select top 1 AccountKey 
                                from Offer o
                                join OfferAttribute oa on o.OfferKey = oa.OfferKey
                                where OfferStatusKey = 3 -- accepted
                                  and oa.OfferAttributeTypeKey = 33 -- ThirtyYearTerm
                                and OfferTypeKey in (6,7,8)";
                DataTable dt = base.GetQueryResults(sql);

                if (dt.Rows.Count == 1)
                {
                    int accountKey = 0;
                    if (int.TryParse(dt.Rows[0][0].ToString(), out accountKey))
                    {
                        IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                        IAccount account = accRepo.GetAccountByKey(accountKey);
                        Assert.IsTrue(account.IsThirtyYearTerm);
                    }
                    else
                    {
                        Assert.Inconclusive("Bad data");
                    }
                }
                else
                {
                    Assert.Inconclusive("No data");
                }
            }
        }


    }
}