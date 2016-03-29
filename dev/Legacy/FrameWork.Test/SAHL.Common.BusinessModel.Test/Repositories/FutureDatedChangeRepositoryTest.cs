using System;
using System.Collections;
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
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class FutureDatedChangeRepositoryTest : TestBase
    {
        private IFutureDatedChangeRepository futureDtChangeRepo;

        [Test]
        public void GetFutureDatedChangesByGenericKeyTest()
        {
            using (new SessionScope())
            {
                string query = "select top 1 * from [2am].[dbo].[FutureDatedChange] order by InsertDate desc";
                DataTable DT = base.GetQueryResults(query);
                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data.");

                int GenericKey = Convert.ToInt32(DT.Rows[0]["IdentifierReferenceKey"].ToString());
                int FutureDatedChangeTypeKey = Convert.ToInt32(DT.Rows[0]["FutureDatedChangeTypeKey"].ToString());

                //
                futureDtChangeRepo = RepositoryFactory.GetRepository<IFutureDatedChangeRepository>();
                IList<IFutureDatedChange> futureDTChangeList = futureDtChangeRepo.GetFutureDatedChangesByGenericKey(GenericKey, FutureDatedChangeTypeKey);

                //
                Assert.That(futureDTChangeList.Count > 0);
            }
        }

        [Test]
        public void GetFutureDatedChangeDetailByKeyTest()
        {
            using (new SessionScope())
            {
                string query = "select top 1 * from [2am].[dbo].[FutureDatedChangeDetail] order by FutureDatedChangeDetailKey desc";
                DataTable DT = base.GetQueryResults(query);
                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data");

                int FutureDatedChangeDetailKey = Convert.ToInt32(DT.Rows[0]["FutureDatedChangeDetailKey"].ToString());
                futureDtChangeRepo = RepositoryFactory.GetRepository<IFutureDatedChangeRepository>();

                //
                IFutureDatedChangeDetail futureDatedChangeDetail = futureDtChangeRepo.GetFutureDatedChangeDetailByKey(FutureDatedChangeDetailKey);

                //
                Assert.IsNotNull(futureDatedChangeDetail);
            }
        }

        [Test]
        public void CreateEmptyFutureDatedChangeTest()
        {
            using (new SessionScope())
            {
                futureDtChangeRepo = RepositoryFactory.GetRepository<IFutureDatedChangeRepository>();
                IFutureDatedChange futureDatedChange = futureDtChangeRepo.CreateEmptyFutureDatedChange();
                Assert.IsNotNull(futureDatedChange);
            }
        }

        [Test]
        public void CreateEmptyFutureDatedChangeDetailTest()
        {
            using (new SessionScope())
            {
                futureDtChangeRepo = RepositoryFactory.GetRepository<IFutureDatedChangeRepository>();
                IFutureDatedChangeDetail futureDatedChangeDetail = futureDtChangeRepo.CreateEmptyFutureDatedChangeDetail();
                Assert.IsNotNull(futureDatedChangeDetail);
            }
        }

        [Test]
        public void FutureDatedChangeMapTest()
        {
            futureDtChangeRepo = RepositoryFactory.GetRepository<IFutureDatedChangeRepository>();

            string testQuery = @"select top 1 fsV.FinancialServiceKey
            from futureDatedChange fdcV (nolock)
            inner join financialService fsV (nolock)
	            on fdcv.IdentifierReferenceKey = fsv.FinancialServiceKey
            inner join financialService fsF (nolock)
	            on fsv.AccountKey = fsf.AccountKey
            inner join futureDatedChange fdcF (nolock)
	            on (fdcf.IdentifierReferenceKey = fsF.FinancialServiceKey and
	            fdcf.FutureDatedChangeTypeKey = fdcV.FutureDatedChangeTypeKey and
	            fdcf.EffectiveDate = fdcV.EffectiveDate and
	            fdcf.ChangeDate = fdcv.ChangeDate)
            where
	            fsV.FinancialServiceTypeKey = 1
            and
	            fsf.FinancialServiceTypeKey = 2";

            object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(testQuery, typeof(FinancialService_DAO), new ParameterCollection());
            if (o != null)
            {
                int FinancialServiceKey = Convert.ToInt32(o);
                Hashtable ht = futureDtChangeRepo.FutureDatedChangeMap(FinancialServiceKey);
                if (ht.Count == 0)
                    Assert.Fail();
            }
        }

        [Test]
        public void FutureDatedChangeDetailMapTest()
        {
            futureDtChangeRepo = RepositoryFactory.GetRepository<IFutureDatedChangeRepository>();

            string testQuery = @"select top 1 fsV.FinancialServiceKey
            from futureDatedChange fdcV (nolock)
            inner join financialService fsV (nolock)
	            on fdcv.IdentifierReferenceKey = fsv.FinancialServiceKey
            inner join FutureDatedChangeDetail fdcdV (nolock)
	            on fdcdV.FutureDatedChangeKey = fdcV.FutureDatedChangeKey
            inner join financialService fsF (nolock)
	            on fsv.AccountKey = fsf.AccountKey
            inner join futureDatedChange fdcF (nolock)
	            on (fdcf.IdentifierReferenceKey = fsF.FinancialServiceKey and
	            fdcf.FutureDatedChangeTypeKey = fdcV.FutureDatedChangeTypeKey and
	            fdcf.EffectiveDate = fdcV.EffectiveDate and
	            fdcf.ChangeDate = fdcv.ChangeDate)
            inner join FutureDatedChangeDetail fdcdF (nolock)
	            on (fdcdF.FutureDatedChangeKey = fdcF.FutureDatedChangeKey and
		            fdcdF.Action = fdcdV.Action and
		            fdcdF.TableName = fdcdV.TableName and
		            fdcdF.ColumnName = fdcdV.ColumnName and
		            fdcdF.Value = fdcdV.Value)
            where
	            fsV.FinancialServiceTypeKey = 1
            and
	            fsf.FinancialServiceTypeKey = 2";

            object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(testQuery, typeof(FinancialService_DAO), new ParameterCollection());
            if (o != null)
            {
                int FinancialServiceKey = Convert.ToInt32(o);
                Hashtable ht = futureDtChangeRepo.FutureDatedChangeDetailMap(FinancialServiceKey);
                if (ht.Count == 0)
                    Assert.Fail();
            }
        }
    }
}