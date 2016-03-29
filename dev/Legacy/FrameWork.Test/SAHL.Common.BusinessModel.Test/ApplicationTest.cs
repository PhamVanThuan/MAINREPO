using Castle.ActiveRecord;
using NHibernate.Criterion;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Test;
using System;
using System.Data;
using System.Linq;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class ApplicationTest : TestBase
    {
        [Test]
        public void ConvertTo30YearTermTest()
        {
            string query = @"select 
                                top 1 o.offerKey from offer o
                            join (
	                            select max(offerinformationkey) offerinformationkey, offerkey 
	                            from offerinformation oi  (nolock)
	                            group by offerkey
	                            ) maxoi on o.offerkey = maxoi.OfferKey
                            join 
	                            OfferInformation oi (nolock)
                            on 
	                            maxoi.offerinformationkey = oi.OfferInformationKey
	                            and oi.OfferInformationTypeKey = 3
                            join 
	                            OfferInformationVariableLoan oivl  (nolock)
                            on
	                            oi.OfferInformationKey = oivl.OfferInformationKey
                            where 
	                            o.OfferTypeKey = 7 and oivl.Term = 240 -- New Purchase Loan
                            order by 
	                            o.OfferKey desc";

            DataTable DT = base.GetQueryResults(query);
            Assert.That(DT.Rows.Count == 1);
            var applicationkey = Convert.ToInt32(DT.Rows[0][0]);

            var currentRevisionKey = -1;

            using (new TransactionScope(OnDispose.Rollback))
            {
                int newTerm = 360;
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                var application = appRepo.GetApplicationByKey(applicationkey);
                currentRevisionKey = application.GetLatestApplicationInformation().Key;
                appRepo.ConvertAcceptedApplicationToExtendedTerm(application, newTerm, true, 0.003);
                application = appRepo.GetApplicationByKey(applicationkey);
                ISupportsVariableLoanApplicationInformation vlInfo = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                var variableLoanInformation = vlInfo.VariableLoanInformation;

                //added app attribute
                var applicationAttribute = application.ApplicationAttributes.Where(x => x.ApplicationAttributeType.Key == (int)OfferAttributeTypes.Loanwith30YearTerm).FirstOrDefault();
                Assert.IsNotNull(applicationAttribute);

                //application has a term of 360 
                Assert.IsTrue(newTerm == variableLoanInformation.Term);

                //check if new revision has been created
                var newRevisionKey = application.GetLatestApplicationInformation().Key;
                Assert.IsTrue(newRevisionKey > currentRevisionKey);

                //check if fadj has been applied
                IApplicationInformation ai = application.GetLatestApplicationInformation();
                var fadj = ai.ApplicationInformationFinancialAdjustments.Where(x => x.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.Loanwith30YearTerm).FirstOrDefault();
                Assert.IsNotNull(fadj);

                //latest offer information has been set as accepted
                Assert.AreEqual(application.GetLatestApplicationInformation().ApplicationInformationType.Key, (int)OfferInformationTypes.AcceptedOffer);

            }
        }


        [Test]
        public void RevertToPreviousTermTest()
        {
            string query = @"select 
                    top 1 o.offerKey from offer o
                join (
	                select max(offerinformationkey) offerinformationkey, offerkey 
	                from offerinformation oi  (nolock)
	                group by offerkey
	                ) maxoi on o.offerkey = maxoi.OfferKey
                join 
	                OfferInformation oi (nolock)
                on 
	                maxoi.offerinformationkey = oi.OfferInformationKey
	                and oi.OfferInformationTypeKey = 3
                join 
	                OfferInformationVariableLoan oivl  (nolock)
                on
	                oi.OfferInformationKey = oivl.OfferInformationKey
                where 
	                o.OfferTypeKey = 7 and oivl.Term = 240 -- New Purchase Loan
                order by 
	                o.OfferKey desc";

            DataTable DT = base.GetQueryResults(query);
            Assert.That(DT.Rows.Count == 1);
            var applicationkey = Convert.ToInt32(DT.Rows[0][0]);
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplication application = null;
            var currentRevisionKey = -1;
            int newTerm = 360;

            using (new TransactionScope(OnDispose.Commit))
            {
                application = appRepo.GetApplicationByKey(applicationkey);
                appRepo.ConvertAcceptedApplicationToExtendedTerm(application, newTerm, true, 0.003);
            }

            using (new TransactionScope(OnDispose.Commit))
            {
                application = appRepo.GetApplicationByKey(applicationkey);
                currentRevisionKey = application.GetLatestApplicationInformation().Key;

                //make sure this is a 30 year term app
                var applicationAttribute = application.ApplicationAttributes.Where(x => x.ApplicationAttributeType.Key == (int)OfferAttributeTypes.Loanwith30YearTerm).FirstOrDefault();
                Assert.IsNotNull(applicationAttribute);

                ISupportsVariableLoanApplicationInformation vlInfo = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                var variableLoanInformation = vlInfo.VariableLoanInformation;
                Assert.IsTrue(newTerm == variableLoanInformation.Term);    

                // revert to 20 year term
                appRepo.RevertToPreviousTermAsAcceptedApplication(application);

                //refresh the application
                application = appRepo.GetApplicationByKey(applicationkey);

                //removed the app attribute
                var applicationAttributeRemoved = application.ApplicationAttributes.Where(x => x.ApplicationAttributeType.Key == (int)OfferAttributeTypes.Loanwith30YearTerm).FirstOrDefault();
                Assert.IsNull(applicationAttributeRemoved);

                //application has a term >= 240
                vlInfo = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                variableLoanInformation = vlInfo.VariableLoanInformation;
                Assert.IsTrue(240 >= variableLoanInformation.Term);

                //check if new revision has been created
                var newRevisionKey = application.GetLatestApplicationInformation().Key;
                Assert.IsTrue(newRevisionKey > currentRevisionKey);

                //check if fadj has been applied
                IApplicationInformation ai = application.GetLatestApplicationInformation();
                var fadj = ai.ApplicationInformationFinancialAdjustments.Where(x => x.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.Loanwith30YearTerm).FirstOrDefault();
                Assert.IsNull(fadj);

                //latest offer information has been set as accepted
                Assert.AreEqual(application.GetLatestApplicationInformation().ApplicationInformationType.Key, (int)OfferInformationTypes.AcceptedOffer);
            }

        }

        [Test]
        public void GetLatestApplicationInformation()
        {
            using (new SessionScope())
            {
                string query = "select top 1 o.OfferKey, count(oi.OfferInformationKey) "
                    + "from [2AM].[dbo].[Offer] o (nolock) "
                    + "join [2AM].[dbo].[OfferInformation] oi (nolock) on oi.OfferKey = o.OfferKey "
                    + "where o.AccountKey is not null "
                    + "group by o.OfferKey, oi.OfferInformationKey order by count(oi.OfferInformationKey) desc";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IApplication app = BMTM.GetMappedType<IApplication>(Application_DAO.Find(DT.Rows[0][0]));
                IApplicationInformation ai = app.GetLatestApplicationInformation();
            }
        }

        [Test]
        public void GetLifeApplication()
        {
            using (new SessionScope())
            {
                object key = base.GetPrimaryKey("Offer", "OfferKey", "OfferTypeKey = " + (int)SAHL.Common.Globals.OfferTypes.Life);
                ApplicationLife_DAO LPA = (ApplicationLife_DAO)ApplicationLife_DAO.Find(key);
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IApplicationLife ILPA = BMTM.GetMappedType<IApplicationLife>(LPA);
                Assert.IsNotNull(LPA);
            }
        }

        [Test]
        public void GetApplicationRolesByType()
        {
            using (new SessionScope())
            {
                string query = "select top 1 o.OfferKey from [2AM].[dbo].[Offer] o (nolock) "
                + "join [2AM].[dbo].[OfferRole] ofr (nolock) on ofr.OfferKey = o.OfferKey and ofr.OfferRoleTypeKey = " + (int)SAHL.Common.Globals.OfferRoleTypes.Consultant;
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IApplication application = BMTM.GetMappedType<IApplication>(Application_DAO.Find(DT.Rows[0][0]));

                // use the repository to get the roles
                IReadOnlyEventList<IApplicationRole> appRoles = application.GetApplicationRolesByType(OfferRoleTypes.Consultant);

                Assert.IsTrue(appRoles.Count > 0);
            }
        }

        [Test]
        public void GetApplicationRolesByGroup()
        {
            using (new SessionScope())
            {
                // get an offer that has a 'client' role on it
                string query = "select top 1 o.OfferKey from [2AM].[dbo].[Offer] o (nolock) "
                + "join [2AM].[dbo].[OfferRole] ofr (nolock) on ofr.OfferKey = o.OfferKey"; // and ofr.OfferRoleTypeKey = " + (int)SAHL.Common.Globals.OfferRoleTypes.MainApplicant;
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IApplication application = BMTM.GetMappedType<IApplication>(Application_DAO.Find(DT.Rows[0][0]));

                int iOfferRoleTypeGroupKey = application.ApplicationRoles[0].ApplicationRoleType.ApplicationRoleTypeGroup.Key;

                IReadOnlyEventList<IApplicationRole> clientRoles = application.GetApplicationRolesByGroup((OfferRoleTypeGroups)iOfferRoleTypeGroupKey);

                Assert.IsTrue(clientRoles.Count > 0);
                Assert.IsTrue(clientRoles[0].ApplicationRoleType.ApplicationRoleTypeGroup.Key == iOfferRoleTypeGroupKey);
            }
        }

        [Test]
        public void GetApplicationProductHistory()
        {
            using (new SessionScope())
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IApplication App = BMTM.GetMappedType<IApplication>(Application_DAO.Find(33239));
                IApplicationProduct[] Products = App.ProductHistory;
                for (int i = 0; i < Products.Length; i++)
                {
                    string mieeu = Products[i].ProductType.ToString();
                }
            }
        }

        [Test]
        public void GetAppInfoProduct()
        {
            using (new SessionScope())
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IApplication App = BMTM.GetMappedType<IApplication>(Application_DAO.FindFirst(new ICriterion[] { Expression.Eq("ApplicationType.Key", 6) }));
                if (App != null)
                {
                    for (int i = 0; i < App.ApplicationInformations.Count; i++)
                    {
                        Assert.IsNotNull(App.ApplicationInformations[i].ApplicationProduct);
                    }
                }
            }
        }

        [Ignore("test implemented to check specific data on UAT2, cannot be run normally.")]
        [Test]
        public void CheckApproleType()
        {
            TransactionScope TS = new TransactionScope();

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            try
            {
                int ApplicationKey = 691654;
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                IApplicationMortgageLoan app = appRepo.GetApplicationByKey(ApplicationKey) as IApplicationMortgageLoan;
                if (null == app)
                {
                    spc.DomainMessages.Add(new SAHL.Common.DomainMessages.Error(string.Format("Application with Key:{0} is not an IApplicationMortgageLoan"), ""));
                }

                // get the application roles and migrate leads to normal LE's
                foreach (IApplicationRole role in app.ApplicationRoles)
                {
                    int RoleKey = role.ApplicationRoleType.Key;
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }

        [Test]
        public void GetLegalName()
        {
            using (new SessionScope())
            {
                //int applicationKey = 835582;
                //Application_DAO LPA = (Application_DAO)Application_DAO.Find(applicationKey);

                Application_DAO LPA = (Application_DAO)Application_DAO.FindFirst();

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IApplication ILPA = BMTM.GetMappedType<IApplication>(LPA);
                Assert.IsNotNull(LPA);

                string legalName = ILPA.GetLegalName(LegalNameFormat.Full);

                Assert.IsNotNull(legalName);
            }
        }

        [Test]
        public void PricingForRisk()
        {
            using (new SessionScope(FlushAction.Never))
            {
                string sql = @"select top 5 * from offer
                        where offerstatuskey = 1 --open
                        and offertypekey in (6, 7, 8) --Switch Loan, New Purchase Loan, Refinance Loan
                        and originationSourceKey != 4
                        and offerstartdate > '2010/01/01'
                        ";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        IApplicationMortgageLoan appML = appRepo.GetApplicationByKey(Convert.ToInt32(dr["OfferKey"].ToString())) as IApplicationMortgageLoan;

                        if (appML != null)
                            appML.PricingForRisk();
                    }
                }
            }
        }

        [Test]
        public void Test()
        {
            try
            {
                using (new SessionScope(FlushAction.Never))
                {
                    IDomainMessageCollection messages = new DomainMessageCollection();
                    IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                    IApplication app = appRepo.GetApplicationByKey(1303008);

                    IRuleService ruleService = ServiceFactory.GetService<IRuleService>();
                    ruleService.Enabled = true;
                    int result = ruleService.ExecuteRule(messages, "EnsureAllLegalEntitiesOnApplicationHasDomicilium", app);
                    ruleService.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Test]
        public void GetComcorpVendor_Success_Test()
        {
            using (new SessionScope(FlushAction.Never))
            {
                // offer with active External Vendor OfferRole
                string sql = @"select top 1 o.OfferKey
                                from [dbo].[Offer] o
                                join [dbo].[OfferRole] ofr on ofr.OfferKey = o.OfferKey
	                                and ofr.GeneralStatusKey = 1
	                                and ofr.OfferRoleTypeKey = 941
                                where o.OfferTypeKey in (6, 7)
                                and o.OfferStatusKey = 1
                                and o.OriginationSourceKey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
                int offerKey;

                if (ds.Tables.Count > 0 &&
                    ds.Tables[0].Rows.Count > 0 &&
                    int.TryParse(ds.Tables[0].Rows[0]["OfferKey"].ToString(), out offerKey))
                {
                    IApplicationReadOnlyRepository applicationReadOnlyRepository = RepositoryFactory.GetRepository<IApplicationReadOnlyRepository>();
                    IApplication application = applicationReadOnlyRepository.GetApplicationByKey(offerKey);
                    Assert.IsNotNull(application.GetComcorpVendor());
                }
                else
                    Assert.Ignore("no data");
            }
        }

        [Test]
        public void GetComcorpVendor_Fail_Test()
        {
            using (new SessionScope(FlushAction.Never))
            {
                // offer with no External Vendor OfferRole
                string sql = @"select top 1 o.OfferKey
                                from [dbo].[Offer] o
                                left join [dbo].[OfferRole] ofr on ofr.OfferKey = o.OfferKey
	                                and ofr.OfferRoleTypeKey = 941
                                where o.OfferTypeKey in (6, 7)
                                and o.OfferStatusKey = 1
                                and o.OriginationSourceKey = 1
                                and ofr.OfferKey is null";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
                int offerKey;

                if (ds.Tables.Count > 0 &&
                    ds.Tables[0].Rows.Count > 0 &&
                    int.TryParse(ds.Tables[0].Rows[0]["OfferKey"].ToString(), out offerKey))
                {
                    IApplicationReadOnlyRepository applicationReadOnlyRepository = RepositoryFactory.GetRepository<IApplicationReadOnlyRepository>();
                    IApplication application = applicationReadOnlyRepository.GetApplicationByKey(offerKey);
                    Assert.IsNull(application.GetComcorpVendor());
                }
                else
                    Assert.Ignore("no data");
            }
        }

        [Test]
        public void HasCondition_Test_True()
        {
            using (new SessionScope(FlushAction.Never))
            {
                IApplicationReadOnlyRepository applicationReadOnlyRepository = RepositoryFactory.GetRepository<IApplicationReadOnlyRepository>();

                string sql = @"select top 1 o.OfferKey, c.[ConditionName]
                                from [2AM].[dbo].[Offer] o (nolock)
                                join [2AM].[dbo].[OfferCondition] oc (nolock) on oc.OfferKey = o.OfferKey
                                join [2AM].[dbo].[Condition] c (nolock) on c.ConditionKey = oc.ConditionKey
                                where o.OfferStatusKey = 3
                                order by o.OfferKey desc";

                DataTable dt = base.GetQueryResults(sql);

                if (dt.Rows.Count == 1)
                {
                    int offerKey = 0;
                    if (int.TryParse(dt.Rows[0][0].ToString(), out offerKey))
                    {
                        IApplication application = applicationReadOnlyRepository.GetApplicationByKey(offerKey);
                        Assert.IsTrue(application.HasCondition(dt.Rows[0][1].ToString()));
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
        public void HasCondition_Test_False()
        {
            using (new SessionScope(FlushAction.Never))
            {
                IApplicationReadOnlyRepository applicationReadOnlyRepository = RepositoryFactory.GetRepository<IApplicationReadOnlyRepository>();

                string sql = @"select top 1 o.OfferKey
                                from [2AM].[dbo].[Offer] o (nolock)
                                join [2AM].[dbo].[OfferCondition] oc (nolock) on oc.OfferKey = o.OfferKey
                                left join [2AM].[dbo].[Condition] c (nolock) on c.ConditionKey = oc.ConditionKey
	                                and c.[ConditionName] = '222'
                                where o.OfferStatusKey = 3
                                and c.ConditionKey is null
                                order by o.OfferKey desc";

                DataTable dt = base.GetQueryResults(sql);

                if (dt.Rows.Count == 1)
                {
                    int offerKey = 0;
                    if (int.TryParse(dt.Rows[0][0].ToString(), out offerKey))
                    {
                        IApplication application = applicationReadOnlyRepository.GetApplicationByKey(offerKey);
                        Assert.IsFalse(application.HasCondition("222"));
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