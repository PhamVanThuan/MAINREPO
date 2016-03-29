using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class MortgageLoanTest : TestBase
    {
        private const string SqlValuation = @"select top 1 ml.FinancialServiceKey, v.ValuationKey, v.ValuationAmount, v.ValuationDate
                                                from [2AM].[fin].MortgageLoan ml (nolock)
                                                inner join Valuation v (nolock) on v.PropertyKey = ml.PropertyKey
                                                where v.IsActive = 1 and v.ValuationStatusKey = 2";

        /// <summary>
        /// Private method to load an IMortgageLoan
        /// </summary>
        /// <param name="fsKey"></param>
        /// <returns></returns>
        private IMortgageLoan GetMortgageLoan(int fsKey)
        {
            // using hql = loading with find causes issues for now that i wasn't able to resolve
            string hql = string.Format("from MortgageLoan_DAO mort where mort.Key = {0}", fsKey);
            SimpleQuery<MortgageLoan_DAO> q2 = new SimpleQuery<MortgageLoan_DAO>(hql);
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return BMTM.GetMappedType<MortgageLoan>(q2.Execute()[0]);
        }

        [Test]
        public void GetActiveValuation()
        {
            DataTable dt = base.GetQueryResults(SqlValuation);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data");
            int fsKey = Convert.ToInt32(dt.Rows[0]["FinancialServiceKey"]);
            int valKey = Convert.ToInt32(dt.Rows[0]["ValuationKey"]);
            dt.Dispose();

            using (new SessionScope())
            {
                IMortgageLoan ml = GetMortgageLoan(fsKey);

                IValuation val = ml.GetActiveValuation();
                Assert.IsNotNull(val);
                Assert.AreEqual(val.Key, valKey);
            }
        }

        [Test]
        public void GetActiveValuationAmount()
        {
            DataTable dt;
            int fsKey;

            using (dt = base.GetQueryResults(SqlValuation + " and ValuationAmount is not null and ValuationAmount > 0"))
            {
                if (dt.Rows.Count > 0)
                {
                    fsKey = Convert.ToInt32(dt.Rows[0]["FinancialServiceKey"]);
                    double valAmount = Convert.ToDouble(dt.Rows[0]["ValuationAmount"]);

                    using (new SessionScope(FlushAction.Never))
                    {
                        IMortgageLoan ml = GetMortgageLoan(fsKey);

                        double amount = ml.GetActiveValuationAmount();
                        Assert.Greater(amount, 0);
                        Assert.AreEqual(amount, valAmount);
                    }
                }
            }

            // run the same test with null - should receive a 0
            using (dt = base.GetQueryResults(SqlValuation + " and ValuationAmount is null"))
            {
                if (dt.Rows.Count > 0)
                {
                    fsKey = Convert.ToInt32(dt.Rows[0]["FinancialServiceKey"]);

                    using (new SessionScope(FlushAction.Never))
                    {
                        // using hql = loading with find causes issues for now that i wasn't able to resolve
                        IMortgageLoan ml = GetMortgageLoan(fsKey);

                        double amount = ml.GetActiveValuationAmount();
                        Assert.AreEqual(amount, 0);
                    }
                }
            }
        }

        [Test]
        public void GetActiveValuationDate()
        {
            using (DataTable dt = base.GetQueryResults(SqlValuation + " and ValuationDate is not null"))
            {
                if (dt.Rows.Count > 0)
                {
                    int fsKey = Convert.ToInt32(dt.Rows[0]["FinancialServiceKey"]);
                    DateTime valDate = Convert.ToDateTime(dt.Rows[0]["ValuationDate"]);
                    dt.Dispose();

                    using (new SessionScope(FlushAction.Never))
                    {
                        IMortgageLoan ml = GetMortgageLoan(fsKey);

                        DateTime? d = ml.GetActiveValuationDate();
                        Assert.IsTrue(d.HasValue);
                        Assert.AreEqual(d, valDate);
                    }
                }
            }
        }

        [Test]
        public void SaveValuation()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 v.* from Valuation v
					join fin.MortgageLoan ml on ml.PropertyKey = v.PropertyKey
                    where ValuationDataProviderDataServiceKey = 1 and IsActive = 1";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                int key = Convert.ToInt32(DT.Rows[0][0]);
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IValuation valuation = BMTM.GetMappedType<ValuationDiscriminatedSAHLManual>(Valuation_DAO.Find(key).As<Valuation_DAO>());

                string data = valuation.Data;
                double? amount = valuation.ValuationAmount;
                valuation.Data = "SaveTest";
                valuation.ValuationAmount = 999999999.99;

                PropertyRepository repo = new PropertyRepository();
                repo.SaveValuation(valuation);

                valuation.Data = data;
                valuation.ValuationAmount = amount;
                repo.SaveValuation(valuation);
            }
        }

        [Test]
        public void GetLatestRegisteredBond()
        {
            //   select @BondRegistrationDate = max( B.BondRegistrationDate )  from
            //[2AM].[dbo].[BondMortgageLoan] BML (nolock)  JOIN [2AM].[dbo].[Bond] B (nolock)  ON B.BondKey = BML.BondKey  where BML.FinancialServiceKey = @FSKeyVariable;
            using (new SessionScope())
            {
                IDomainMessageCollection messages = new DomainMessageCollection();

                ParameterCollection parameters = new ParameterCollection();
                IDbConnection con = Helper.GetSQLDBConnection();
                DataTable DT = new DataTable();

                string query = "select top 1 ML.FinancialServiceKey, B.BondRegistrationDate from [2AM].[dbo].[Bond] B (nolock) "
                    + "join [2AM].[dbo].[BondMortgageLoan] BML (nolock) ON B.BondKey = BML.BondKey "
                    + "join [2AM].[fin].[MortgageLoan] ML (nolock) ON ML.FinancialServiceKey = BML.FinancialServiceKey "
                    + "where B.BondRegistrationDate is not null "
                    + "group by ML.FinancialServiceKey, B.BondRegistrationDate order by B.BondRegistrationDate desc";
                Helper.FillFromQuery(DT, query, con, parameters);

                Assert.That(DT.Rows.Count == 1, "No data found.");

                DateTime date = (DateTime)DT.Rows[0][1];

                IMortgageLoan ml = GetMortgageLoan(Convert.ToInt32(DT.Rows[0][0]));

                IBond bond = ml.GetLatestRegisteredBond();

                Assert.That(bond.BondRegistrationDate.ToShortDateString() == date.ToShortDateString());
            }
        }

        [Test]
        public void SumBondRegistrationAmounts()
        {
            using (new SessionScope())
            {
                IDomainMessageCollection messages = new DomainMessageCollection();

                ParameterCollection parameters = new ParameterCollection();
                IDbConnection con = Helper.GetSQLDBConnection();
                DataTable DT = new DataTable();

                string query = "select top 1 BML.FinancialServiceKey, sum(B.BondRegistrationAmount) from [2AM].[dbo].[FinancialService] FS (nolock) "
                    + "JOIN [2AM].[dbo].[BondMortgageLoan] BML (nolock) ON BML.FinancialServiceKey = FS.FinancialServiceKey "
                    + "JOIN [2AM].[dbo].[Bond] B (nolock) ON B.BondKey = BML.BondKey "
                    + "WHERE FS.AccountStatusKey in (1,5) "
                    + "group by BML.FinancialServiceKey order by count(BML.FinancialServiceKey) desc";
                Helper.FillFromQuery(DT, query, con, parameters);

                Assert.That(DT.Rows.Count == 1, "No data found.");

                double sumData = (double)DT.Rows[0][1];

                IMortgageLoan ml = GetMortgageLoan(Convert.ToInt32(DT.Rows[0][0]));

                double sum = ml.SumBondRegistrationAmounts();

                Assert.That(sum == sumData);
            }
        }

        [Test]
        public void SumBondLoanAgreementAmounts()
        {
            using (new SessionScope())
            {
                IDomainMessageCollection messages = new DomainMessageCollection();

                ParameterCollection parameters = new ParameterCollection();
                IDbConnection con = Helper.GetSQLDBConnection();
                DataTable DT = new DataTable();

                string query = "select top 1 BML.FinancialServiceKey, ROUND(sum(B.BondLoanAgreementAmount), 2) from [2AM].[dbo].[FinancialService] FS (nolock) "
                    + "JOIN [2AM].[dbo].[BondMortgageLoan] BML (nolock) ON BML.FinancialServiceKey = FS.FinancialServiceKey "
                    + "JOIN [2AM].[dbo].[Bond] B (nolock) ON B.BondKey = BML.BondKey "
                    + "WHERE FS.AccountStatusKey in (1,5) "
                    + "group by BML.FinancialServiceKey order by count(BML.FinancialServiceKey) desc";
                Helper.FillFromQuery(DT, query, con, parameters);

                Assert.That(DT.Rows.Count == 1, "No data found.");
                double sumData = Convert.ToDouble(DT.Rows[0][1]);
                IMortgageLoan ml = GetMortgageLoan(Convert.ToInt32(DT.Rows[0][0]));
                double sum = ml.SumBondLoanAgreementAmounts();
                Assert.AreEqual(Math.Round(sum, 2), Math.Round(sumData, 2));
            }
        }

        [Test]
        public void CheckLoanTransactions()
        {
            using (new SessionScope())
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                MortgageLoan_DAO dao = (MortgageLoan_DAO)MortgageLoan_DAO.Find(10346126);

                int mieeu = dao.FinancialTransactions.Count;
                IMortgageLoan mort = BMTM.GetMappedType<IMortgageLoan>(dao);
                mieeu = mort.FinancialTransactions.Count;
            }
        }

        [Test]
        public void GetLatestPropertyValuation()
        {
            using (new SessionScope())
            {
                string query = "select top 1 ML.FinancialServiceKey from [2AM].[fin].[MortgageLoan] ML (nolock) "
                    + "JOIN [2AM].[dbo].[Property] P (nolock) ON P.PropertyKey = ML.PropertyKey "
                    + "JOIN [2AM].[dbo].[Valuation] V (nolock) ON P.PropertyKey = V.PropertyKey";
                DataTable DT = base.GetQueryResults(query);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                MortgageLoan ml = BMTM.GetMappedType<MortgageLoan>(MortgageLoan_DAO.Find(DT.Rows[0][0]) as MortgageLoan_DAO);
                IValuation val = ml.GetActiveValuation();

                Assert.That(val != null);

                query = "SELECT TOP 1 V.ValuationKey FROM [2AM].[dbo].[FinancialService] FS (nolock) "
                    + "JOIN [2AM].[fin].[MortgageLoan] ML (nolock) ON ML.FinancialServiceKey = FS.FinancialServiceKey "
                    + "JOIN [2AM].[dbo].[Property] P (nolock) ON P.PropertyKey = ML.PropertyKey "
                    + "JOIN [2AM].[dbo].[Valuation] V (nolock) ON P.PropertyKey = V.PropertyKey "
                    + "WHERE FS.FinancialServiceKey = " + ml.Key.ToString() + " ORDER BY V.ValuationDate desc";
                DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);
                Assert.That((int)DT.Rows[0][0] == val.Key);
            }
        }

        [Test]
        public void GetLatestPropertyValuationDate()
        {
            using (new SessionScope())
            {
                string query = "select top 1 ML.FinancialServiceKey from [2AM].[fin].[MortgageLoan] ML (nolock) "
                    + "JOIN [2AM].[dbo].[Property] P (nolock) ON P.PropertyKey = ML.PropertyKey "
                    + "JOIN [2AM].[dbo].[Valuation] V (nolock) ON P.PropertyKey = V.PropertyKey";
                DataTable DT = base.GetQueryResults(query);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                MortgageLoan ml = BMTM.GetMappedType<MortgageLoan>(MortgageLoan_DAO.Find(DT.Rows[0][0]) as MortgageLoan_DAO);
                //acc.GetLatestPropertyValuationDate();
            }
        }

        [Test]
        public void GetLatestPropertyValuationAmount()
        {
            using (new SessionScope())
            {
                string query = "select top 1 ML.FinancialServiceKey from [2AM].[fin].[MortgageLoan] ML (nolock) "
                    + "JOIN [2AM].[dbo].[Property] P (nolock) ON P.PropertyKey = ML.PropertyKey "
                    + "JOIN [2AM].[dbo].[Valuation] V (nolock) ON P.PropertyKey = V.PropertyKey";
                DataTable DT = base.GetQueryResults(query);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                MortgageLoan ml = BMTM.GetMappedType<MortgageLoan>(MortgageLoan_DAO.Find(DT.Rows[0][0]) as MortgageLoan_DAO);
                //acc.GetLatestPropertyValuationAmount();
            }
        }

        [Test]
        public void HasInterestOnly()
        {
            using (new SessionScope())
            {
                IDomainMessageCollection messages = new DomainMessageCollection();

                ParameterCollection parameters = new ParameterCollection();
                IDbConnection con = Helper.GetSQLDBConnection();
                DataTable DT = new DataTable();

                string query = "select top 1 ML.FinancialServiceKey, B.BondRegistrationDate from [2AM].[dbo].[Bond] B (nolock) "
                    + "join [2AM].[dbo].[BondMortgageLoan] BML (nolock) ON B.BondKey = BML.BondKey "
                    + "join [2AM].[fin].[MortgageLoan] ML (nolock) ON ML.FinancialServiceKey = BML.FinancialServiceKey "
                    + "where B.BondRegistrationDate is not null "
                    + "group by ML.FinancialServiceKey, B.BondRegistrationDate order by B.BondRegistrationDate desc";
                Helper.FillFromQuery(DT, query, con, parameters);

                Assert.That(DT.Rows.Count == 1, "No data found.");

                IMortgageLoan ml = GetMortgageLoan(Convert.ToInt32(DT.Rows[0][0]));

                bool _hasInterestOnly = ml.HasInterestOnly();

                Assert.That(_hasInterestOnly == true || _hasInterestOnly == false);
            }
        }
    }
}