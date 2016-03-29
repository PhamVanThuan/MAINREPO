using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class ConditionsRepositoryTest : TestBase
    {
        private IConditionsRepository _conRepo = null;

        [Test]
        public void GetLoanConditions()
        {
            using (new SessionScope())
            {
                // get first instance from application capture workflow
                string sql = "select top 1 Offerkey from OfferCondition order by  offerkey desc";

                DataTable dt = base.GetQueryResults(sql);

                int appKey = Convert.ToInt32(dt.Rows[0][0]);

                try
                {
                    List<string> conditions = ConditionsRepository.GetLoanConditions(appKey);
                    Assert.IsTrue(conditions.Count > 0);
                }

                catch (Exception ex)
                {
                    Assert.Fail(ex.ToString());
                }
            }
        }

        [Test]
        public void NormaliseParenthesisTest()
        {
            using (new SessionScope())
            {
                try
                {
                    string conditions = ConditionsRepository.NormaliseParenthesis("Blah ' ++ Blah  Blah + char(13) + char(10) +");
                    Assert.IsTrue(conditions == "Blah \\' ++ Blah Blah \\r\\n");
                }

                catch (Exception ex)
                {
                    Assert.Fail(ex.ToString());
                }
            }
        }

        [Test]
        public void ParseArrayToStringTest()
        {
            using (new SessionScope())
            {
                try
                {
                    string[] InputArray = { "Zero", "One", "Two", "Three" };
                    string conditions = ConditionsRepository.ParseArrayToString(InputArray);
                    Assert.IsTrue(conditions == "'Zero','One','Two','Three',");
                }

                catch (Exception ex)
                {
                    Assert.Fail(ex.ToString());
                }
            }
        }

        [Test]
        public void StringHasChangedTest()
        {
            using (new SessionScope())
            {
                try
                {
                    string Input1 = "Zero One Two Three";
                    string Input2 = "One Two Three";

                    bool conditions = ConditionsRepository.StringHasChanged(Input1, Input2);
                    Assert.IsTrue(conditions);
                }

                catch (Exception ex)
                {
                    Assert.Fail(ex.ToString());
                }
            }
        }

        [Test]
        public void GetLastDisbursedApplicationConditions()
        {
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            using (new SessionScope())
            {
                // get first instance from application capture workflow
                string sql = "select top 1 o.* from offer o join Account a on o.AccountKey = a.AccountKey "
                    + " where o.OfferStatusKey = 3 and o.OfferTypeKey not in (2, 5, 0) and o.OfferEndDate is not null "
                    + " order by o.OfferEndDate desc";

                DataTable dt = base.GetQueryResults(sql);

                if (dt == null || dt.Rows.Count == 0)
                    Assert.Ignore("No disbursed Application Data found");

                int appKey = Convert.ToInt32(dt.Rows[0][0]);

                //got an application to test with
                IApplication app = appRepo.GetApplicationByKey(appKey);

                List<string> lastConditions = ConditionsRepository.GetLastDisbursedApplicationConditions(app.Account.Key);

                List<string> appConditions = ConditionsRepository.GetLoanConditions(app.Key);

                Assert.AreEqual(lastConditions.Count, appConditions.Count);
            }
        }

        [Test]
        public void GetExistingConditionSetForEditing()
        {
            using (new SessionScope())
            {
                // get first instance from application capture workflow
                string sql = "select top 1 Offerkey from OfferCondition order by  offerkey desc";

                DataTable dt = base.GetQueryResults(sql);

                int appKey = Convert.ToInt32(dt.Rows[0][0]);

                try
                {
                    ConditionsRepository.GetExistingConditionSetForEditing(appKey, 2);
                }

                catch (Exception ex)
                {
                    Assert.Fail(ex.ToString());
                }
            }
        }

        [Test]
        public void UpdateLoanConditionsTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                // GetNewConditionSet
                string sql = @"select top 1 ofr.OfferKey
                from [2am].[dbo].offer ofr
                join [2am].[dbo].offerInformation oi
	                on ofr.offerKey = oi.offerKey
                where ofr.offerStatusKey = 1
                order by ofr.offerKey desc";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                if (o != null)
                {
                    int offerKey = Convert.ToInt32(o);
                    int generickeytypekey = (int)GenericKeyTypes.Offer;

                    ConditionsRepository.UpdateLoanConditions(offerKey, generickeytypekey);
                    Assert.IsTrue(true);
                }
            }
        }

        [Test]
        public void GetNewConditionSetTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                // GetNewConditionSet
                string sql = @"select top 1 ofr.OfferKey
                from [2am].[dbo].offer ofr
                where ofr.offerStatusKey = 1
                and ofr.Offertypekey in (2, 3, 4, 6, 7, 8)
                order by ofr.offerKey desc";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                if (o != null)
                {
                    int offerKey = Convert.ToInt32(o);
                    int generickeytypekey = (int)GenericKeyTypes.Offer;

                    ConditionsRepository.GetNewConditionSet(offerKey, generickeytypekey);
                    Assert.IsTrue(true);
                }
            }
        }

        public IConditionsRepository ConditionsRepository
        {
            get
            {
                if (_conRepo == null)
                    _conRepo = RepositoryFactory.GetRepository<IConditionsRepository>();

                return _conRepo;
            }
        }

        [Test]
        public void GetNewConditionSet_Subsidy()
        {
            int offerKey = 0;
            int generickeytypekey = (int)GenericKeyTypes.Offer;

            using (new SessionScope())
            {
                string sql = @"select top 1 o.OfferKey
                    from [2AM].dbo.Offer o
                    join [2AM].dbo.OfferRole orl on orl.OfferKey = o.OfferKey
	                    and orl.OfferRoleTypeKey = 11
                    join [2AM].dbo.LegalEntity le on le.LegalEntityKey = orl.LegalEntityKey
                    join [2AM].dbo.Employment empl on empl.LegalEntityKey = le.LegalEntityKey
	                    and empl.EmploymentTypeKey = 3
	                    and empl.EmploymentStatusKey = 1
                    where o.OfferStatusKey = 1
                    and o.offertypekey in (6, 7, 8)
                    order by o.OfferKey desc ";

                DataTable DT = base.GetQueryResults(sql);

                if (DT.Rows != null && DT.Rows.Count > 0)
                    offerKey = Convert.ToInt32(DT.Rows[0][0]);
            }

            if (offerKey > 0)
            {
                using (new TransactionScope(OnDispose.Rollback))
                {
                    ConditionsRepository.GetNewConditionSet(offerKey, generickeytypekey);

                    Assert.NotNull(ConditionsRepository.ConditionsSet);
                    Assert.NotNull(ConditionsRepository.ConditionsSet.Tables);
                    Assert.AreEqual(2, ConditionsRepository.ConditionsSet.Tables.Count);

                    string condition222 = "222)";

                    bool contains = ConditionsRepository.ConditionsSet.Tables[0].AsEnumerable()
                                   .Any(row => row.Field<string>("Template").Contains(condition222));

                    Assert.IsTrue(contains);
                }
            }
        }
    }
}