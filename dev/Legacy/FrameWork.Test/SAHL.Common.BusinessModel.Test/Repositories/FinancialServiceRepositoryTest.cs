using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class FinancialServiceRepositoryTest : TestBase
    {
        private IFinancialServiceRepository _fsRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
        private IManualDebitOrderRepository _manDebitOrderRepo = RepositoryFactory.GetRepository<IManualDebitOrderRepository>();

        [Test]
        public void GetFinancialServiceByKey()
        {
            using (new SessionScope())
            {
                string query = "SELECT TOP 1 * FROM [2am].[dbo].[FinancialService]";
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                IFinancialService fs = _fsRepo.GetFinancialServiceByKey(Convert.ToInt32(DT.Rows[0]["FinancialServiceKey"].ToString()));

                Assert.That(fs.Key.ToString() == DT.Rows[0]["FinancialServiceKey"].ToString());
            }
        }

        [Test]
        public void GetFinancialServiceBankAccountByKey()
        {
            using (new SessionScope())
            {
                int key = Convert.ToInt32(base.GetPrimaryKey("FinancialServiceBankAccount", "FinancialServiceBankAccountKey"));
                IFinancialServiceBankAccount fsBankAccount1 = _fsRepo.GetFinancialServiceBankAccountByKey(key);
                Assert.IsNotNull(fsBankAccount1);

                IFinancialServiceBankAccount fsBankAccount2 = _fsRepo.GetFinancialServiceBankAccountByKey(-1);
                Assert.IsNull(fsBankAccount2);
            }
        }

        [Test]
        public void GetEmptyFinancialServiceBankAccount()
        {
            using (new SessionScope())
            {
                IFinancialServiceBankAccount fsba = _fsRepo.GetEmptyFinancialServiceBankAccount();
                Assert.IsNotNull(fsba);
            }
        }

        [Test]
        public void GetSuspendedInterest()
        {
            using (new SessionScope())
            {
                string query = string.Format(@"
                               select top 1
                                fs.AccountKey
                                from dbo.FinancialService fs
                                join fin.FinancialAdjustment fa
                                    on fs.FinancialServiceKey = fa.FinancialServiceKey
                                join fin.FinancialAdjustmentTypeSource fats
	                                on fats.FinancialAdjustmentSourceKey = fa.FinancialAdjustmentSourceKey
	                                and fats.FinancialAdjustmentTypeKey = fa.FinancialAdjustmentTypeKey
                                where
	                                fs.FinancialServiceTypeKey = 1 and
	                                fs.AccountStatusKey = 1 and
                                    fa.FinancialAdjustmentStatusKey = 1 and
                                    DateDiff(mm, fa.FromDate, GetDate()) > 1 and 
	                                fats.FinancialAdjustmentTypeSourceKey = {0}
                                order by fs.AccountKey desc",
                                (int)SAHL.Common.Globals.FinancialAdjustmentTypeSources.NonPerforming); ;

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), new ParameterCollection());
                Assert.IsNotNull(obj, "Cannot find data");

                int accKey = Convert.ToInt32(obj);
                DateTime? dt;
                Decimal amount = _fsRepo.GetSuspendedInterest(accKey, out dt);
                Assert.Greater(amount, 0, String.Format("Account: {0} suspended interest amount is invalid", accKey));
                Assert.IsTrue(dt.HasValue, String.Format("Account: {0} suspended interest start date is invalid", accKey));
            }
        }

        [Test]
        public void IsLoanNonPerforming()
        {
            using (new SessionScope())
            {
                string query = string.Format(@"
                               select top 1
                                fs.AccountKey
                                from dbo.FinancialService fs
                                join fin.FinancialAdjustment fa
                                    on fs.FinancialServiceKey = fa.FinancialServiceKey
                                join fin.FinancialAdjustmentTypeSource fats
	                                on fats.FinancialAdjustmentSourceKey = fa.FinancialAdjustmentSourceKey
	                                and fats.FinancialAdjustmentTypeKey = fa.FinancialAdjustmentTypeKey
                                where
	                                fs.FinancialServiceTypeKey = 1 and
	                                fs.AccountStatusKey = 1 and
	                                fats.FinancialAdjustmentTypeSourceKey = {0} and
                                    fa.FinancialAdjustmentStatusKey = 1",
                                (int)SAHL.Common.Globals.FinancialAdjustmentTypeSources.NonPerforming); ;

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), new ParameterCollection());
                Assert.IsNotNull(obj, "Cannot find data");

                if (obj != null)
                {
                    bool res = _fsRepo.IsLoanNonPerforming(Convert.ToInt32(obj));
                    Assert.IsTrue(res);
                }
            }
        }

        [Test]
        public void GetFinancialServiceGroupTest()
        {
            using (new SessionScope())
            {
                FinancialServiceGroup_DAO fsg = FinancialServiceGroup_DAO.FindFirst();
                IFinancialServiceGroup fsGroup = _fsRepo.GetFinancialServiceGroup(fsg.Key);
                Assert.AreEqual(fsGroup.Key, fsg.Key);
            }
        }
    }
}