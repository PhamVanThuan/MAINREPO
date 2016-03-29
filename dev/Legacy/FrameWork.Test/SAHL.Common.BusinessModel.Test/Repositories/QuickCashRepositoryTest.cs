using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using Castle.ActiveRecord;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;


namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class QuickCashRepositoryTest : TestBase
    {
        private IQuickCashRepository _quickCashRepo;

        [Test]
        public void CreateEmptyApplicationInformationQuickCashDetailTest()
        {
            using (new SessionScope())
            {
                _quickCashRepo = RepositoryFactory.GetRepository<IQuickCashRepository>();
                IApplicationInformationQuickCashDetail applicationInformationQuickCashDetail = _quickCashRepo.CreateEmptyApplicationInformationQuickCashDetail();
                Assert.IsNotNull(applicationInformationQuickCashDetail);
            }
        }

        [Test]
        public void SaveApplicationInformationQuickCashTest()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                ApplicationInformationQuickCash_DAO appInf = ApplicationInformationQuickCash_DAO.FindFirst();
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IApplicationInformationQuickCash app = BMTM.GetMappedType<IApplicationInformationQuickCash, ApplicationInformationQuickCash_DAO>(ApplicationInformationQuickCash_DAO.Find(appInf.Key) as ApplicationInformationQuickCash_DAO);

                app.Term = 1;

                _quickCashRepo = RepositoryFactory.GetRepository<IQuickCashRepository>();
                _quickCashRepo.SaveApplicationInformationQuickCash(app);

                ApplicationInformationQuickCash_DAO appInf1 = ApplicationInformationQuickCash_DAO.Find(app.Key);

                Assert.AreEqual(appInf1.Term, app.Term);

            }
        }

        [Test]
        public void SaveApplicationInformationQuickCashDetailTest()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                ApplicationInformationQuickCashDetail_DAO appInf = ApplicationInformationQuickCashDetail_DAO.FindFirst();
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IApplicationInformationQuickCashDetail app = BMTM.GetMappedType<IApplicationInformationQuickCashDetail, ApplicationInformationQuickCashDetail_DAO>(ApplicationInformationQuickCashDetail_DAO.Find(appInf.Key) as ApplicationInformationQuickCashDetail_DAO);

                app.RequestedAmount = 1.00;

                _quickCashRepo = RepositoryFactory.GetRepository<IQuickCashRepository>();
                _quickCashRepo.SaveApplicationInformationQuickCashDetail(app);

                ApplicationInformationQuickCashDetail_DAO appInf1 = ApplicationInformationQuickCashDetail_DAO.Find(app.Key);

                Assert.AreEqual(appInf1.RequestedAmount, app.RequestedAmount);

            }
        }

        [Test]
        public void GetApplicationInformationQuickCashDetailByKeyTest()
        {
            using (new SessionScope())
            {
                string query = "SELECT TOP 1 * FROM [2am].[dbo].[OfferInformationQuickCashDetail] order by OfferInformationQuickCashDetailKey desc";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count > 0);
                //
                int OfferInformationQuickCashDetailKey = Convert.ToInt32(DT.Rows[0]["OfferInformationQuickCashDetailKey"].ToString());
                //
                _quickCashRepo = RepositoryFactory.GetRepository<IQuickCashRepository>();
               IApplicationInformationQuickCashDetail applicationInformationQuickCashDetail =  _quickCashRepo.GetApplicationInformationQuickCashDetailByKey(OfferInformationQuickCashDetailKey);
                //
               Assert.IsNotNull(applicationInformationQuickCashDetail);
            }
        }

        [Test]
        public void DeleteApplicationExpenseTest()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                ApplicationExpense_DAO appExp = ApplicationExpense_DAO.FindFirst();
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IApplicationExpense app = BMTM.GetMappedType<IApplicationExpense, ApplicationExpense_DAO>(ApplicationExpense_DAO.Find(appExp.Key) as ApplicationExpense_DAO);

                _quickCashRepo = RepositoryFactory.GetRepository<IQuickCashRepository>();
                _quickCashRepo.DeleteApplicationExpense(app);

                ApplicationExpense_DAO appExp1 = ApplicationExpense_DAO.Find(app.Key);                
            }
        }

        [Test]
        public void DeleteApplicationDebtSettlementTest()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                ApplicationDebtSettlement_DAO appExp = ApplicationDebtSettlement_DAO.FindFirst();
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IApplicationDebtSettlement app = BMTM.GetMappedType<IApplicationDebtSettlement, ApplicationDebtSettlement_DAO>(ApplicationDebtSettlement_DAO.Find(appExp.Key) as ApplicationDebtSettlement_DAO);

                _quickCashRepo = RepositoryFactory.GetRepository<IQuickCashRepository>();
                _quickCashRepo.DeleteApplicationDebtSettlement(app);

                ApplicationDebtSettlement_DAO appExp1 = ApplicationDebtSettlement_DAO.Find(app.Key);
            }
        }
    }
}
