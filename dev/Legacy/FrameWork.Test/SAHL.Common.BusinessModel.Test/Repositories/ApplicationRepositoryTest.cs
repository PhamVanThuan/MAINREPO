using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Queries;
using NHibernate;
using NHibernate.Criterion;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.X2.BusinessModel;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Services.Capitec.Models.Shared;
using SAHL.Test;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class ApplicationRepositoryTest : TestBase
    {
        private static bool _hasSetupHelpers = false;
        private IApplicationRepository applicationRepository;
        private IAddressRepository addressRepository;
        private IITCRepository itcRepository;
        private ICommonRepository commonRepository;
        private ILegalEntityRepository legalEntityRepository;
        private IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();

        public ApplicationRepositoryTest()
        {
            applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
            addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();
            itcRepository = RepositoryFactory.GetRepository<IITCRepository>();
            commonRepository = RepositoryFactory.GetRepository<ICommonRepository>();
            legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
        }

        [SetUp()]
        public void Setup()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Clear();
        }

        [Test]
        public void Webservice_Varifix_SelfEmployed()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                ILeadInputInformation leadInput = new LeadInputInformation
                {
                    EmploymentTypeKey = (int)EmploymentTypes.SelfEmployed,
                    ProductKey = (int)Products.VariFixLoan,
                    OfferSourceKey = (int)OfferSources.InternetApplication,
                    PropertyValue = 950000,
                    MortgageLoanPurposeKey = (int)MortgageLoanPurposes.Newpurchase,
                    Deposit = 0,
                    CashOut = 230000,
                    HouseholdIncome = 68000,
                    Term = 240,
                    ////N = PropertyValue (purchase price) - deposit
                    //double LoanAmountRequired { get; } //R
                    FixPercent = 30,
                    InterestOnly = false,
                    //  LegalEntity
                    FirstNames = "Some",
                    Surname = "Dude",
                    //EmailAddress = "sd@ano.com",
                    HomePhoneCode = "031",
                    HomePhoneNumber = "5555555",
                    NumberOfApplicants = 1,
                    //Referer
                    AdvertisingCampaignID = "AdvertisingCampaignID",
                    ReferringServerURL = "ReferringServerURL",
                    UserURL = "UserURL",
                    UserAddress = "UserAddress"
                };

                using (new TransactionScope(OnDispose.Rollback))
                {
                    string query = @"SELECT TOP 1 o.OfferKey
                                    FROM Offer o
                                    left join offermortgageloan ml on o.offerkey = ml.offerkey
                                    left join offerinformation oi on o.offerkey = oi.offerkey
                                    WHERE o.OfferTypeKey = 9 AND o.OfferStatusKey = 1
                                    and oi.Offerkey is null and ml.OfferKey is null";
                    DataTable DT = base.GetQueryResults(query);
                    int applicationKey = Convert.ToInt32(DT.Rows[0][0]);
                    bool success = applicationRepository.GenerateApplicationFromWeb(applicationKey, leadInput);
                    IApplication application = applicationRepository.GetApplicationByKey(applicationKey);
                    Assert.AreEqual(application.ApplicationSource.Key, leadInput.OfferSourceKey, "Offer Source has not been set correctly");
                }
            }
        }

        [Test]
        public void CalculateOriginationFees()
        {
            //we just want to test that this method can work from the app repo
            //The repo method uses a helper class, testing for different
            //input options should be done for the helper, and not duplicated here
            using (new SessionScope())
            {
                double initFee;
                double RegFee;
                double cancelFee;
                double interimInterest;
                double bondToRegister;
                double? InitiationFeeDiscount;

                applicationRepository.CalculateOriginationFees(200000, 300000, OfferTypes.NewPurchaseLoan, 100000, 0, false, true, false, false, out InitiationFeeDiscount, out initFee, out RegFee, out cancelFee, out interimInterest, out bondToRegister, false, 15500, 1, 270000, 0, false, DateTime.Now, false, false);
            }
        }

        [Test]
        public void CreditDisqualifications()
        {
            //we are testing the code paths in the repo method
            //more compelte tests for each rule should be done by rules tests
            //avoiding duplication here
            using (new SessionScope())
            {
                CreditDisqualificationsHelper(false, 0.1, 0.1, 50000, 500000, 750000, (int)EmploymentTypes.Salaried, false, 240, false, 0);
                CreditDisqualificationsHelper(false, 0.1, 0.1, 50000, 500000, 750000, (int)EmploymentTypes.Salaried, true, 220, false, 0);
                CreditDisqualificationsHelper(true, 0.1, 0.1, 50000, 500000, 750000, (int)EmploymentTypes.Salaried, false, 240, false, 0);
                CreditDisqualificationsHelper(false, 0.1, 0.1, 50000, 500000, 750000, (int)EmploymentTypes.Unemployed, false, 240, false, 1);
            }
        }

        private void CreditDisqualificationsHelper(bool calculationDone, double ltv, double pti, double income, double loanAmount, double valuationAmount, int employmentTypeKey, bool isFurtherLending, int term, bool readvanceOnly, int messageCount)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            int msgs = spc.DomainMessages.Count;

            applicationRepository.CreditDisqualifications(calculationDone, ltv, pti, income, loanAmount, valuationAmount, employmentTypeKey, isFurtherLending, term, readvanceOnly);

            Assert.That((msgs + messageCount) == spc.DomainMessages.Count);
        }

        [Test]
        public void ApplicationProductRecalcTest()
        {
            string query = null;
            DataTable DT = null;
            int offerkey = -1;

            using (new TransactionScope(OnDispose.Rollback))
            {
                //get an open application
                query = @"select min(o.offerkey), offertypekey from [2am].dbo.Offer o (nolock)
                        join (
                            select max(offerinformationkey) as offerinformationkey, offerkey
                            from [2am].dbo.offerinformation (nolock)
                            group by offerkey
                        ) maxoi on o.offerkey = maxoi.offerkey
                        join offerinformation oi (nolock) on maxoi.offerinformationkey = oi.offerinformationkey
                        where offerstatuskey = 1 and originationsourcekey != 4 and offerinformationtypekey != 3
                        and offertypekey in (6, 7, 8) and offerstartdate > GetDate()-150
                        group by offertypekey";
                DT = base.GetQueryResults(query);

                //Assert.That(DT.Rows.Count == 1);

                foreach (DataRow dr in DT.Rows)
                {
                    offerkey = Convert.ToInt32(dr[0]);

                    //get the application
                    IApplicationMortgageLoan appML = applicationRepository.GetApplicationByKey(offerkey) as IApplicationMortgageLoan;

                    switch ((OfferTypes)appML.ApplicationType.Key)
                    {
                        case OfferTypes.NewPurchaseLoan:
                            IApplicationMortgageLoanNewPurchase appNP = appML as IApplicationMortgageLoanNewPurchase;
                            foreach (ProductsNewPurchase p in Enum.GetValues(typeof(ProductsNewPurchase)))
                            {
                                if (p == ProductsNewPurchase.DefendingDiscountRate || p == ProductsNewPurchase.VariFixLoan || p == ProductsNewPurchase.SuperLo)
                                    continue;

                                //change product
                                appNP.SetProduct(p);

                                //recalc
                                appNP.CalculateApplicationDetail(false, false);
                            }
                            applicationRepository.SaveApplication(appML);

                            break;

                        case OfferTypes.RefinanceLoan:
                            IApplicationMortgageLoanRefinance appRF = appML as IApplicationMortgageLoanRefinance;
                            foreach (ProductsRefinance p in Enum.GetValues(typeof(ProductsRefinance)))
                            {
                                if (p == ProductsRefinance.DefendingDiscountRate || p == ProductsRefinance.VariFixLoan || p == ProductsRefinance.SuperLo)
                                    continue;

                                //change product
                                appRF.SetProduct(p);

                                //recalc
                                appRF.CalculateApplicationDetail(false, false);
                            }
                            applicationRepository.SaveApplication(appML);

                            break;

                        case OfferTypes.SwitchLoan:
                            IApplicationMortgageLoanSwitch appSW = appML as IApplicationMortgageLoanSwitch;
                            foreach (ProductsSwitchLoan p in Enum.GetValues(typeof(ProductsSwitchLoan)))
                            {
                                if (p == ProductsSwitchLoan.DefendingDiscountRate || p == ProductsSwitchLoan.VariFixLoan || p == ProductsSwitchLoan.SuperLo)
                                    continue;

                                //change product
                                appSW.SetProduct(p);

                                //recalc
                                appSW.CalculateApplicationDetail(false, false);
                            }
                            applicationRepository.SaveApplication(appML);

                            break;

                        default:
                            break;
                    }
                }
            }
        }

        [Test]
        public void ApplicationEdgeProductRecalcTest()
        {
            string query = null;
            DataTable DT = null;
            int offerkey = -1;

            using (new TransactionScope(OnDispose.Rollback))
            {
                //get an open application
                query = @"select min(o.offerkey), offertypekey from [2am].dbo.Offer o (nolock)
                        join (
                            select max(offerinformationkey) as offerinformationkey, offerkey
                            from [2am].dbo.offerinformation (nolock)
                            group by offerkey
                        ) maxoi on o.offerkey = maxoi.offerkey
                        join offerinformation oi (nolock) on maxoi.offerinformationkey = oi.offerinformationkey
                        where offerstatuskey = 1 and originationsourcekey != 4 and offerinformationtypekey != 3
                        and offertypekey in (6, 7, 8) and offerstartdate > GetDate()-150
                        group by offertypekey";
                DT = base.GetQueryResults(query);

                //Assert.That(DT.Rows.Count == 1);

                foreach (DataRow dr in DT.Rows)
                {
                    offerkey = Convert.ToInt32(dr[0]);

                    //get the application
                    IApplicationMortgageLoan appML = applicationRepository.GetApplicationByKey(offerkey) as IApplicationMortgageLoan;

                    switch ((OfferTypes)appML.ApplicationType.Key)
                    {
                        case OfferTypes.NewPurchaseLoan:
                            IApplicationMortgageLoanNewPurchase appNP = appML as IApplicationMortgageLoanNewPurchase;

                            //change product
                            appNP.SetProduct(ProductsNewPurchase.Edge);

                            //recalc
                            appNP.CalculateApplicationDetail(false, false);
                            applicationRepository.SaveApplication(appML);
                            break;

                        case OfferTypes.RefinanceLoan:
                            IApplicationMortgageLoanRefinance appRF = appML as IApplicationMortgageLoanRefinance;

                            //change product
                            appRF.SetProduct(ProductsRefinance.Edge);

                            //recalc
                            appRF.CalculateApplicationDetail(false, false);
                            applicationRepository.SaveApplication(appML);
                            break;

                        case OfferTypes.SwitchLoan:
                            IApplicationMortgageLoanSwitch appSW = appML as IApplicationMortgageLoanSwitch;

                            //change product
                            appSW.SetProduct(ProductsSwitchLoan.Edge);

                            //recalc
                            appSW.CalculateApplicationDetail(false, false);
                            applicationRepository.SaveApplication(appML);

                            break;

                        default:
                            break;
                    }
                }
            }
        }

        [Test]
        public void GetFurtherLendingX2Message()
        {
            using (new SessionScope(FlushAction.Never))
            {
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                string sql = @"select top 1 instancex2.name from
                                x2.x2data.application_management (nolock) as d
                                join x2.x2.instance (nolock) as instancex2 on instancex2.id=d.instanceid
                                join [x2].[x2].Worklist (nolock) as worklistx2 on worklistx2.instanceid=instancex2.id
                                where isFL=1";
                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
                string str;

                if (obj != null)
                {
                    str = appRepo.GetFurtherLendingX2Message(Convert.ToInt32(obj));
                    Console.WriteLine("App Key: " + obj);
                    Console.WriteLine("Message : " + str);
                    Assert.IsNotEmpty(str);
                    Assert.IsFalse(str.StartsWith("No X2 instance found"));
                }

                str = appRepo.GetFurtherLendingX2Message(-100);
                Assert.AreEqual(str, String.Format("No X2 instance found. Data is invalid for Application Number: {0}", -100));
            }
        }

        [Test]
        public void GetCallbackByOfferKeyForOfferWithCallback()
        {
            Callback_DAO calback_DAO = Callback_DAO.FindFirst();

            // string sql = base.GetSQLResource(this.GetType(), "GetOfferkeyForOpenCallback.sql");
            string sql = @"select top 1 o.offerkey from
	                offer o  (nolock)
                inner join
	                callback cb  (nolock)
                on
	                cb.GenericKey=o.offerKey
                where cb.CompletedDate is null";

            object o = DBHelper.ExecuteScalar(sql);
            if (null == o)
            {
                Assert.Ignore("No open callback available.");
            }
            int OfferKey = (int)o;
            using (new SessionScope(FlushAction.Never))
            {
                IDomainMessageCollection Messages = new DomainMessageCollection();
                ILifeRepository LR = RepositoryFactory.GetRepository<ILifeRepository>();

                IEventList<ICallback> cb = applicationRepository.GetCallBacksByApplicationKey(OfferKey, true);
                Assert.IsNotNull(cb, string.Format("No Callback found for offerkey {0}", OfferKey));
            }
        }

        [Test]
        public void GetApplicationByKey()
        {
            using (new SessionScope())
            {
                int appKeyKey = Convert.ToInt32(base.GetPrimaryKey("Offer", "OfferKey"));
                IApplication app1 = applicationRepository.GetApplicationByKey(appKeyKey);
                Assert.IsNotNull(app1);

                IApplication app2 = applicationRepository.GetApplicationByKey(-1);
                Assert.IsNull(app2);
            }
        }

        [Test]
        public void GetLatestOpenCallBackByApplicationKey()
        {
            using (new SessionScope(FlushAction.Never))
            {
                IDomainMessageCollection Messages = new DomainMessageCollection();

                // find the first callback with an offerkey to use in our test
                ICriterion[] criterion = new ICriterion[]
                    {
                        Expression.Eq("GenericKeyType.Key", Convert.ToInt32(SAHL.Common.Globals.GenericKeyTypes.Offer))
                        , Expression.IsNull("CompletedDate")
                    };
                Callback_DAO cb = Callback_DAO.FindFirst(new Order("CallbackDate", false), criterion);
                if (cb == null)
                    Assert.Ignore("No callbacks available.");

                int applicationKey = cb.GenericKey;

                // use the repository method to retreive the data and compare to the result obtained above
                ICallback callback = applicationRepository.GetLatestCallBackByApplicationKey(applicationKey, true);
                Assert.IsNotNull(callback, string.Format("No Callback found for offerkey {0}", applicationKey));
                Assert.AreEqual(applicationKey, callback.GenericKey);
            }
        }

        [Test]
        public void GetAllCallBacksByApplicationKey()
        {
            using (new SessionScope(FlushAction.Never))
            {
                IDomainMessageCollection Messages = new DomainMessageCollection();

                // find the first callback with an offerkey to use in our test
                ICriterion[] criterion = new ICriterion[]
                    {
                        Expression.Eq("GenericKeyType.Key",Convert.ToInt32(SAHL.Common.Globals.GenericKeyTypes.Offer))
                    };
                Callback_DAO cb = Callback_DAO.FindFirst(criterion);
                if (cb == null)
                    Assert.Ignore("No callbacks available.");

                int applicationKey = cb.GenericKey;

                // use the repository method to retreive the data and compare to the result obtained above
                IEventList<ICallback> callbacks = applicationRepository.GetCallBacksByApplicationKey(applicationKey, false);
                Assert.IsNotNull(callbacks, string.Format("No Callback found for offerkey {0}", applicationKey));

                // make sure all the callbacks that were retreived were for this offer
                foreach (ICallback cbs in callbacks)
                {
                    Assert.AreEqual(applicationKey, cbs.GenericKey);
                }
            }
        }

        [Test]
        public void GetAllOpenCallBacksByApplicationKey()
        {
            using (new SessionScope(FlushAction.Never))
            {
                IDomainMessageCollection Messages = new DomainMessageCollection();

                // find the first open callback with an offerkey to use in our test
                ICriterion[] criterion = new ICriterion[]
                    {
                        Expression.Eq("GenericKeyType.Key", Convert.ToInt32(SAHL.Common.Globals.GenericKeyTypes.Offer))
                        , Expression.IsNull("CompletedDate")
                    };
                Callback_DAO cb = Callback_DAO.FindFirst(criterion);
                if (cb == null)
                    Assert.Ignore("No callbacks available.");

                int applicationKey = cb.GenericKey;

                // use the repository method to retreive the data and compare to the result obtained above
                IEventList<ICallback> callbacks = applicationRepository.GetCallBacksByApplicationKey(applicationKey, true);
                Assert.IsNotNull(callbacks, string.Format("No Open Callbacks found for offerkey {0}", applicationKey));

                // make sure all the callbacks that were retreived were for this offer and are 'open'
                foreach (ICallback cbs in callbacks)
                {
                    Assert.AreEqual(applicationKey, cbs.GenericKey);
                    Assert.IsFalse(cbs.CompletedDate.HasValue);
                }
            }
        }

        [Test]
        public void GetMortageLoanApplicationInformation()
        {
            IApplicationMortgageLoan IFlApp;
            IApplication IApp;
            DomainMessageCollection msgs = new DomainMessageCollection();
            ApplicationRepository Rep;
            int key;

            using (new SessionScope())
            {
                ApplicationInformationType_DAO aitype = ApplicationInformationType_DAO.FindFirst();
                Rep = new ApplicationRepository();

                key = (int)base.GetPrimaryKey("Offer", "OfferKey", "OfferTypeKey = 6");

                IApp = Rep.GetApplicationByKey(key);
                Assert.IsTrue(IApp is IApplicationMortgageLoan);

                IFlApp = IApp as IApplicationMortgageLoan;

                IEventList<IApplicationInformation> ais = IFlApp.ApplicationInformations;

                foreach (var info in ais)
                {
                    if ((info.Application as ApplicationInformationVarifixLoan) != null)
                    {
                        int ki = info.Application.Key; //  TODO: Not sure what this test is testing??
                    }
                }

                //int cnt = ais.Count;
                //for (int i = 0; i < cnt; i++)
                //{
                //    if (ais[i].Application as ApplicationInformationVarifixLoan != null)
                //    {
                //        int ki = ais[i].ApplicationInformationVarifixLoan.Key;
                //    }
                //}
            }
        }

        [Test]
        public void GetOriginationProducts()
        {
            //this should be mocked, not sql
            using (new SessionScope())
            {
                SetupApplicationHelpers();

                DataSet ds = new DataSet();
                ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran("Select * from Product; Select Count(*) as OProducts from Product (nolock) where originateYN = 'Y'", typeof(Product_DAO), null);

                DataTable dtProds;
                int origProdCount = 0;
                int origProdKey = 0;
                int notorigProdKey = 0;

                if (ds != null && ds.Tables.Count > 0)
                {
                    dtProds = ds.Tables[0];

                    foreach (DataRow dr in dtProds.Rows)
                    {
                        if (dr[2].ToString() == "Y") //get an originatable product key
                            origProdKey = (int)dr[0];

                        if (dr[2].ToString() == "N") //get an non originatable product key
                            notorigProdKey = (int)dr[0];
                    }

                    if (ds.Tables.Count > 1)
                        origProdCount = (int)ds.Tables[1].Rows[0][0];

                    ReadOnlyEventList<IProduct> prodList = applicationRepository.GetOriginationProducts();
                    Assert.That(prodList.Count == origProdCount);

                    prodList = null;
                    prodList = applicationRepository.GetOriginationProducts(origProdKey);
                    Assert.That(prodList.Count == origProdCount);

                    prodList = null;
                    prodList = applicationRepository.GetOriginationProducts(notorigProdKey);
                    Assert.That(prodList.Count == (origProdCount + 1));
                }
            }
        }

        [Test]
        public void CreateLifeApplication()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                spc.DomainMessages.Clear();
                string sql = @"SELECT TOP 1 O.AccountKey , O.Offerkey
                                FROM Offer O (NOLOCK)
                                JOIN dbo.FinancialService FS (NOLOCK)
	                                ON FS.AccountKey = O.AccountKey
                                JOIN dbo.Account acc (nolock)
	                                ON acc.AccountKey = fs.AccountKey and acc.RRR_OriginationSourceKey = 1
                                JOIN dbo.FinancialService FSC (NOLOCK)
	                                ON FS.FinancialServiceKey = FSC.ParentFinancialServiceKey
                                where
	                                O.accountkey is not null
		                                AND
	                                OfferTypeKey = 7
		                                AND
	                                FS.FinancialServiceTypeKey <> 5";
                DataTable dt = base.GetQueryResults(sql);
                if (dt == null || dt.Rows.Count == 0)
                    Assert.Ignore("No Offer Life data found");
                int key = Convert.ToInt32(dt.Rows[0][0]);
                int appkey = Convert.ToInt32(dt.Rows[0][1]);

                IApplicationLife applicationLife = applicationRepository.CreateLifeApplication(key, appkey, "SAHL\\CraigF");
                Assert.IsNotNull(applicationLife);
            }
        }

        [Test]
        public void GetEmptyAccountSequence()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                DomainMessageCollection msgs = new DomainMessageCollection();

                IAccountSequence applicationSeq = null;

                try
                {
                    applicationSeq = applicationRepository.GetEmptyAccountSequence(false);
                    Assert.IsNotNull(applicationSeq);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    throw;
                }
            }
        }

        [Test]
        public void GetEmptyUnknownApplicationType()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Clear();
            using (new TransactionScope(OnDispose.Rollback))
            {
                IApplicationUnknown applicationUK = applicationRepository.GetEmptyUnknownApplicationType(1);
                Assert.IsNotNull(applicationUK);
            }
        }

        [Test]
        public void GetEmptyApplicationAttribute()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                DomainMessageCollection msgs = new DomainMessageCollection();

                IApplicationAttribute applicationAtt = null;

                try
                {
                    applicationAtt = applicationRepository.GetEmptyApplicationAttribute();
                    Assert.IsNotNull(applicationAtt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    throw;
                }
            }
        }

        [Test]
        public void GetEmptyApplicationRoleAttribute()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                DomainMessageCollection msgs = new DomainMessageCollection();

                IApplicationRoleAttribute applicationRoleAtt = null;

                try
                {
                    applicationRoleAtt = applicationRepository.GetEmptyApplicationRoleAttribute();
                    Assert.IsNotNull(applicationRoleAtt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    throw;
                }
            }
        }

        [Test]
        public void GetEmptyApplicationRole()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                DomainMessageCollection msgs = new DomainMessageCollection();

                IApplicationRole applicationRole = null;

                try
                {
                    applicationRole = applicationRepository.GetEmptyApplicationRole();
                    Assert.IsNotNull(applicationRole);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    throw;
                }
            }
        }

        [Test]
        public void GetEmptyApplicationExpense()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                DomainMessageCollection msgs = new DomainMessageCollection();

                IApplicationExpense applicationExpense = null;

                try
                {
                    applicationExpense = applicationRepository.GetEmptyApplicationExpense();
                    Assert.IsNotNull(applicationExpense);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    throw;
                }
            }
        }

        [Test]
        public void GetEmptyApplicationDeclaration()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                DomainMessageCollection msgs = new DomainMessageCollection();

                IApplicationDeclaration applicationDeclaration = null;

                try
                {
                    applicationDeclaration = applicationRepository.GetEmptyApplicationDeclaration();
                    Assert.IsNotNull(applicationDeclaration);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    throw;
                }
            }
        }

        [Test]
        public void GetEmptyApplicationDebitOrder()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                DomainMessageCollection msgs = new DomainMessageCollection();

                IApplicationDebitOrder applicationDebitOrder = null;

                try
                {
                    applicationDebitOrder = applicationRepository.GetEmptyApplicationDebitOrder();
                    Assert.IsNotNull(applicationDebitOrder);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    throw;
                }
            }
        }

        [Test]
        public void GetEmptyApplicationDebtSettlement()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                DomainMessageCollection msgs = new DomainMessageCollection();

                IApplicationDebtSettlement applicationDebtSettlement = null;

                try
                {
                    applicationDebtSettlement = applicationRepository.GetEmptyApplicationDebtSettlement();
                    Assert.IsNotNull(applicationDebtSettlement);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    throw;
                }
            }
        }

        [Test]
        public void GetEmptyApplicationInformation()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                DomainMessageCollection msgs = new DomainMessageCollection();

                IApplicationInformation applicationInformation = null;

                try
                {
                    applicationInformation = applicationRepository.GetEmptyApplicationInformation();
                    Assert.IsNotNull(applicationInformation);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    throw;
                }
            }
        }

        [Test]
        public void GetEmptyApplicationInformationAppliedRateAdjustment()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                DomainMessageCollection msgs = new DomainMessageCollection();

                IApplicationInformationAppliedRateAdjustment applicationInformationRA = null;

                try
                {
                    applicationInformationRA = applicationRepository.GetEmptyApplicationInformationAppliedRateAdjustment();
                    Assert.IsNotNull(applicationInformationRA);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    throw;
                }
            }
        }

        [Test]
        public void GetEmptyApplicationInformationRateOverride()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                var msgs = new DomainMessageCollection();

                IApplicationInformationFinancialAdjustment applicationInformationRO = null;

                try
                {
                    applicationInformationRO = applicationRepository.GetEmptyApplicationInformationFinancialAdjustment();
                    Assert.IsNotNull(applicationInformationRO);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    throw;
                }
            }
        }

        [Test]
        public void GetEmptyApplicationInternetReferrer()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                DomainMessageCollection msgs = new DomainMessageCollection();

                IApplicationInternetReferrer applicationIntRef = null;

                try
                {
                    applicationIntRef = applicationRepository.GetEmptyApplicationInternetReferrer();
                    Assert.IsNotNull(applicationIntRef);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    throw;
                }
            }
        }

        [Test]
        public void GetEmptyApplicationLife()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                DomainMessageCollection msgs = new DomainMessageCollection();

                IApplicationLife applicationLife = null;

                try
                {
                    applicationLife = applicationRepository.GetEmptyApplicationLife();
                    Assert.IsNotNull(applicationLife);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    throw;
                }
            }
        }

        [Test]
        public void GetFurtherLendingX2NTU()
        {
            using (new SessionScope(FlushAction.Never))
            {
                string result;
                result = applicationRepository.GetFurtherLendingX2NTU(-1);

                Assert.IsEmpty(result);

                string sql = @"select top 1 o.AccountKey
                    from x2.x2.instance i (nolock)
                    join x2.x2.state s (nolock) on i.stateid=s.id
                    join x2.x2data.Application_Management data (nolock) on i.id=data.instanceid
                    join [2am]..offer o (nolock) on data.applicationkey = o.offerkey
	                    and o.OfferTypeKey in (2, 3, 4)
	                    --and o.AccountKey = @AccountKey
                    where (s.name='NTU' or s.name='Decline' or s.name='Declined by Credit')
                    and s.workflowID in (
                          select max(ID) from x2.x2.workflow (nolock) where name='Application Management'
                    )
                    union
                    select top 1 o.AccountKey
                    from x2.x2.instance i (nolock)
                    join x2.x2.state s (nolock) on i.stateid=s.id
                    join x2.x2data.Readvance_Payments data (nolock) on i.id=data.instanceid
                    join [2am]..offer o (nolock) on data.applicationkey = o.offerkey
	                    and o.OfferTypeKey in (2, 3, 4)
	                    --and o.AccountKey = @AccountKey
                    where (s.name='NTU' or s.name='Decline') and s.workflowID in (
                          select max(ID) from x2.x2.workflow (nolock) where name='Readvance Payments'
                    )";

                DataTable dt = base.GetQueryResults(sql);

                if (dt == null || dt.Rows.Count == 0)
                    Assert.Ignore("No open account data found");

                int accKey = Convert.ToInt32(dt.Rows[0][0]);

                try
                {
                    result = applicationRepository.GetFurtherLendingX2NTU(accKey);
                    Assert.IsNotNull(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    throw;
                }
            }
        }

        [Test]
        public void GetOpenApplicationsForPropertyKey()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                string sql = @"SELECT     TOP (1) OfferMortgageLoan.PropertyKey
                            FROM OfferMortgageLoan (nolock) INNER JOIN
                            Offer (nolock) ON OfferMortgageLoan.OfferKey = Offer.OfferKey
                            WHERE (Offer.OfferStatusKey = 1 and PropertyKey is not null)
                            ORDER BY Offer.OfferKey DESC";

                DataTable dt = base.GetQueryResults(sql);

                if (dt == null || dt.Rows.Count == 0)
                    Assert.Ignore("No property data found");

                int propKey = Convert.ToInt32(dt.Rows[0][0]);

                try
                {
                    IEventList<IApplication> applicationList = applicationRepository.GetOpenApplicationsForPropertyKey(propKey);
                    Assert.IsNotNull(applicationList);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    throw;
                }
            }
        }

        [Test]
        public void GetRateAdjustmentGroupByKeyTest()
        {
            using (new SessionScope())
            {
                int key = RateAdjustmentGroup_DAO.FindFirst().Key;
                IRateAdjustmentGroup rateAdjustmentGroup = applicationRepository.GetRateAdjustmentGroupByKey(key);
                Assert.AreEqual(key, rateAdjustmentGroup.Key);
            }
        }

        [Test]
        public void GetRoleCurrentConsultantLife()
        {
            IDomainMessageCollection Messages = new DomainMessageCollection();
            IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

            try
            {
                object key = base.GetPrimaryKey("Offer", "OfferKey", "OfferTypeKey = 5");

                if (key == null)
                    Assert.Ignore("No life applications available.");

                IApplicationRoleType art = osRepo.GetApplicationRoleTypeByName("Consultant");
                IApplicationRole appRole = osRepo.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey((int)key, art.Key, (int)SAHL.Common.Globals.GeneralStatuses.Active);

                IADUser aduser = osRepo.GetAdUserByLegalEntityKey(appRole.LegalEntity.Key);

                Assert.IsTrue(aduser != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                throw;
            }
        }

        [Test]
        public void GetApplicationRoleTypeByName()
        {
            string Name = "Consultant";

            string HQL = "from ApplicationRoleType_DAO o where o.Description=?";
            SimpleQuery<ApplicationRoleType_DAO> q = new SimpleQuery<ApplicationRoleType_DAO>(HQL, Name);
            ApplicationRoleType_DAO[] res = q.Execute();
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            IApplicationRoleType rt = BMTM.GetMappedType<IApplicationRoleType>(res[0]);

            Assert.IsTrue(rt.Description == Name);
        }

        [Test]
        public void GetApplicationRoleAttributeTypeByKey()
        {
            using (new SessionScope())
            {
                //get first ApplicationRoleAttributeType_DAO
                ApplicationRoleAttributeType_DAO applicationRoleAttributeType_DAO = ApplicationRoleAttributeType_DAO.FindFirst();

                IApplicationRoleAttributeType applicationRoleAttributeType = applicationRepository.GetApplicationRoleAttributeTypeByKey(applicationRoleAttributeType_DAO.Key);

                Assert.IsTrue(applicationRoleAttributeType_DAO.Key == applicationRoleAttributeType.Key);
                Assert.IsTrue(applicationRoleAttributeType_DAO.Description == applicationRoleAttributeType.Description);
            }
        }

        [Test]
        public void CreateNewPurchaseApplicationNewVaribale()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                SetupApplicationHelpers();
                app = applicationRepository.CreateNewPurchaseApplication(os, ProductsNewPurchaseAtCreation.NewVariableLoan, null);
                app.ApplicationStatus = status;
                IApplicationProduct prod = app.CurrentProduct;
                IApplicationProductNewVariableLoan nvl = SetupNVL(prod);
                applicationRepository.SaveApplication(app);
                Console.WriteLine("New Purchase New Variable: {0}", app.Key);

                Assert.AreNotEqual(nvl.Application.Key, 0);
                Assert.AreEqual(SAHL.Common.Globals.Products.NewVariableLoan, nvl.Application.CurrentProduct.ProductType);
                Assert.AreEqual(nvl.VariableLoanInformation.PropertyValuation, 250000);
                Assert.AreEqual(nvl.InterestOnlyInformation.Installment, 12345);
            }
        }

        [Test]
        public void CreateNewPurchaseApplicationSuperLo()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                SetupApplicationHelpers();
                app = applicationRepository.CreateNewPurchaseApplication(os, ProductsNewPurchaseAtCreation.SuperLo, null);
                app.ApplicationStatus = status;
                IApplicationProduct prod = app.CurrentProduct;
                IApplicationProductSuperLoLoan nvl = SetupSuperLo(prod);
                applicationRepository.SaveApplication(app);
                Console.WriteLine("New Purchase SuperLo: {0}", app.Key);

                Assert.AreNotEqual(nvl.Application.Key, 0);
                Assert.AreEqual(SAHL.Common.Globals.Products.SuperLo, nvl.Application.CurrentProduct.ProductType);
                Assert.AreEqual(nvl.VariableLoanInformation.PropertyValuation, 250000);
                Assert.AreEqual(nvl.SuperLoInformation.PPThresholdYr1, 1500);
                Assert.AreEqual(nvl.InterestOnlyInformation.Installment, 12345);
            }
        }

        [Test]
        public void CreateNewPurchaseApplicationVariFix()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                SetupApplicationHelpers();
                app = applicationRepository.CreateNewPurchaseApplication(os, ProductsNewPurchaseAtCreation.VariFixLoan, null);
                app.ApplicationStatus = status;
                IApplicationProduct prod = app.CurrentProduct;
                IApplicationProductVariFixLoan nvl = SetupVariFix(prod);
                applicationRepository.SaveApplication(app);
                Console.WriteLine("New Purchase VariFix: {0}", app.Key);

                Assert.AreNotEqual(nvl.Application.Key, 0);
                Assert.AreEqual(SAHL.Common.Globals.Products.VariFixLoan, nvl.Application.CurrentProduct.ProductType);
                Assert.AreEqual(nvl.VariableLoanInformation.PropertyValuation, 250000);
                Assert.AreEqual(nvl.VariFixInformation.FixedInstallment, 1500);
            }
        }

        [Test]
        public void CreateSwitchLoanApplicationNewPurchase()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                SetupApplicationHelpers();
                IApplicationUnknown AppUnk = applicationRepository.GetEmptyUnknownApplicationType((int)SAHL.Common.Globals.OriginationSources.SAHomeLoans);
                app = applicationRepository.CreateSwitchLoanApplication(os, ProductsSwitchLoanAtCreation.NewVariableLoan, AppUnk);
                app.ApplicationStatus = status;
                IApplicationProduct prod = app.CurrentProduct;
                IApplicationProductNewVariableLoan nvl = SetupNVL(prod);

                applicationRepository.SaveApplication(app);
                Console.WriteLine("Switch New purchase: {0}", app.Key);

                Assert.AreNotEqual(nvl.Application.Key, 0);
                Assert.AreEqual(SAHL.Common.Globals.Products.NewVariableLoan, nvl.Application.CurrentProduct.ProductType);
                Assert.AreEqual(nvl.VariableLoanInformation.PropertyValuation, 250000);
                Assert.AreEqual(nvl.InterestOnlyInformation.Installment, 12345);
            }
        }

        [Test]
        public void CreateSwitchLoanApplicationVariFix()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                SetupApplicationHelpers();
                IApplicationUnknown AppUnk = applicationRepository.GetEmptyUnknownApplicationType((int)SAHL.Common.Globals.OriginationSources.SAHomeLoans);
                app = applicationRepository.CreateSwitchLoanApplication(os, ProductsSwitchLoanAtCreation.VariFixLoan, AppUnk);
                app.ApplicationStatus = status;
                IApplicationProduct prod = app.CurrentProduct;
                IApplicationProductVariFixLoan nvl = SetupVariFix(prod);

                applicationRepository.SaveApplication(app);
                Console.WriteLine("Switch VariFix: {0}", app.Key);

                Assert.AreNotEqual(nvl.Application.Key, 0);
                Assert.AreEqual(SAHL.Common.Globals.Products.VariFixLoan, nvl.Application.CurrentProduct.ProductType);
                Assert.AreEqual(nvl.VariableLoanInformation.PropertyValuation, 250000);
                Assert.AreEqual(nvl.VariFixInformation.FixedInstallment, 1500);
            }
        }

        [Test]
        public void CreateSwitchLoanApplicationSuperLo()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                SetupApplicationHelpers();
                app = applicationRepository.CreateSwitchLoanApplication(os, ProductsSwitchLoanAtCreation.SuperLo, null);
                app.ApplicationStatus = status;
                IApplicationProduct prod = app.CurrentProduct;

                IApplicationProductSuperLoLoan nvl = SetupSuperLo(prod);

                applicationRepository.SaveApplication(app);
                Console.WriteLine("Switch Superlo: {0}", app.Key);

                Assert.AreNotEqual(nvl.Application.Key, 0);
                Assert.AreEqual(SAHL.Common.Globals.Products.SuperLo, nvl.Application.CurrentProduct.ProductType);
                Assert.AreEqual(nvl.VariableLoanInformation.PropertyValuation, 250000);
                Assert.AreEqual(nvl.SuperLoInformation.PPThresholdYr1, 1500);
                Assert.AreEqual(nvl.InterestOnlyInformation.Installment, 12345);
            }
        }

        [Test]
        public void CreateRefinanceLoanApplicationNewPurchase()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                SetupApplicationHelpers();
                IApplicationUnknown AppUnk = applicationRepository.GetEmptyUnknownApplicationType((int)SAHL.Common.Globals.OriginationSources.SAHomeLoans);
                app = applicationRepository.CreateRefinanceApplication(os, ProductsRefinanceAtCreation.NewVariableLoan, AppUnk);
                app.ApplicationStatus = status;
                IApplicationProduct prod = app.CurrentProduct;
                IApplicationProductNewVariableLoan nvl = SetupNVL(prod);

                applicationRepository.SaveApplication(app);
                Console.WriteLine("Refin New purchase: {0}", app.Key);

                Assert.AreNotEqual(nvl.Application.Key, 0);
                Assert.AreEqual(SAHL.Common.Globals.Products.NewVariableLoan, nvl.Application.CurrentProduct.ProductType);
                Assert.AreEqual(nvl.VariableLoanInformation.PropertyValuation, 250000);
                Assert.AreEqual(nvl.InterestOnlyInformation.Installment, 12345);
            }
        }

        [Test]
        public void CreateRefinanceLoanApplicationVariFix()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                SetupApplicationHelpers();
                IApplicationUnknown AppUnk = applicationRepository.GetEmptyUnknownApplicationType((int)SAHL.Common.Globals.OriginationSources.SAHomeLoans);
                app = applicationRepository.CreateRefinanceApplication(os, ProductsRefinanceAtCreation.VariFixLoan, AppUnk);
                app.ApplicationStatus = status;
                IApplicationProduct prod = app.CurrentProduct;
                IApplicationProductVariFixLoan nvl = SetupVariFix(prod);

                applicationRepository.SaveApplication(app);
                Console.WriteLine("Refin VariFix: {0}", app.Key);

                Assert.AreNotEqual(nvl.Application.Key, 0);
                Assert.AreEqual(SAHL.Common.Globals.Products.VariFixLoan, nvl.Application.CurrentProduct.ProductType);
                Assert.AreEqual(nvl.VariableLoanInformation.PropertyValuation, 250000);
                Assert.AreEqual(nvl.VariFixInformation.FixedInstallment, 1500);
            }
        }

        [Test]
        public void CreateRefinanceLoanApplicationSuperLo()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                SetupApplicationHelpers();
                app = applicationRepository.CreateRefinanceApplication(os, ProductsRefinanceAtCreation.SuperLo, null);
                app.ApplicationStatus = status;
                IApplicationProduct prod = app.CurrentProduct;

                IApplicationProductSuperLoLoan nvl = SetupSuperLo(prod);

                applicationRepository.SaveApplication(app);
                Console.WriteLine("Switch Superlo: {0}", app.Key);

                Assert.AreNotEqual(nvl.Application.Key, 0);
                Assert.AreEqual(SAHL.Common.Globals.Products.SuperLo, nvl.Application.CurrentProduct.ProductType);
                Assert.AreEqual(nvl.VariableLoanInformation.PropertyValuation, 250000);
                Assert.AreEqual(nvl.SuperLoInformation.PPThresholdYr1, 1500);
                Assert.AreEqual(nvl.InterestOnlyInformation.Installment, 12345);
            }
        }

        [Test]
        public void CreateFurtherLoanAppliation()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                SetupApplicationHelpers();
                int AccountKey = -1;
                string query = @"select p.description ,max(a.accountkey)
                                from [2am].[fin].mortgageloan ml (nolock)
                                join [2am].dbo.financialservice fs (nolock) on ml.FinancialServiceKey = fs.FinancialServiceKey
                                JOIN [2am].fin.FinancialAdjustment FA (NOLOCK) ON fs.FinancialServiceKey = fa.FinancialServiceKey
                                join [2am].fin.FixedRateAdjustment fra (NOLOCK) ON fra.FinancialAdjustmentKey = fa.FinancialAdjustmentKey
                                join [2am].dbo.account a (nolock) on fs.accountkey=a.accountkey
                                join [2am].dbo.product p (nolock) on a.rrr_productkey = p.productkey
                                where a.accountstatuskey=1  and p.productkey = 1
                                group by p.description";
                IDbConnection con = null;
                try
                {
                    con = Helper.GetSQLDBConnection();
                    DataTable dt = new DataTable();
                    Helper.FillFromQuery(dt, query, con, null);
                    AccountKey = Convert.ToInt32(dt.Rows[0][1]);
                    Console.WriteLine("FurtherLoan using AccountKey:{0}", AccountKey);
                }
                catch (Exception ex)
                {
                    throw new Exception("Unable to get MLA key");
                }
                finally
                {
                    con.Dispose();
                }
                using (new SessionScope())
                {
                    IAccount acc = accRepo.GetAccountByKey(AccountKey);
                    IMortgageLoanAccount mla = acc as IMortgageLoanAccount;
                    if (null != mla)
                    {
                        app = applicationRepository.CreateFurtherLoanApplication(mla, true);
                        IApplicationFurtherLoan fl = app as IApplicationFurtherLoan;

                        applicationRepository.SaveApplication(fl);
                        Console.WriteLine("FurtherLoan Key:{0}", fl.Key);
                    }
                }
            }
        }

        [Test]
        public void CreateEmptyUnknownApplicationType()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                int appkey = 0;
                SetupApplicationHelpers();
                IApplicationUnknown applicationUnknown = SetupEmptyUnknownApplicationType((int)OriginationSources.SAHomeLoans);
                appkey = applicationUnknown.Key;
                Console.WriteLine("Empty Unknown Application Key:{0}", applicationUnknown.ReservedAccount.Key);
                Assert.AreNotEqual(applicationUnknown.ReservedAccount.Key, 0);
            }
        }

        [Test]
        public void CreateReAdvanceApplication()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                SetupApplicationHelpers();
                int AccountKey = -1;
                string query = @"select p.description ,max(a.accountkey)
                                from [2am].fin.mortgageloan ml (nolock)
                                join financialservice fs (nolock) on ml.FinancialServiceKey = fs.FinancialServiceKey
                                JOIN [2am].fin.FinancialAdjustment FA (NOLOCK) ON fs.FinancialServiceKey = fa.FinancialServiceKey
                                join [2am].fin.FixedRateAdjustment fra (NOLOCK) ON fra.FinancialAdjustmentKey = fa.FinancialAdjustmentKey
                                join account a (nolock) on fs.accountkey=a.accountkey
                                join product p (nolock) on a.rrr_productkey = p.productkey
                                where a.accountstatuskey=1 and p.productkey = 1
                                group by p.description";
                IDbConnection con = null;
                try
                {
                    con = Helper.GetSQLDBConnection();
                    DataTable dt = new DataTable();
                    Helper.FillFromQuery(dt, query, con, null);
                    AccountKey = Convert.ToInt32(dt.Rows[0][1]);
                    Console.WriteLine("ReadvanceAdvance using AccountKey:{0}", AccountKey);
                }
                catch (Exception ex)
                {
                    throw new Exception("Unable to get MLA key");
                }
                finally
                {
                    con.Dispose();
                }
                using (new SessionScope())
                {
                    IAccount acc = accRepo.GetAccountByKey(AccountKey);
                    IMortgageLoanAccount mla = acc as IMortgageLoanAccount;
                    if (null != mla)
                    {
                        app = applicationRepository.CreateReAdvanceApplication(mla, true);
                        IApplicationReAdvance fl = app as IApplicationReAdvance;
                        applicationRepository.SaveApplication(fl);
                        Console.WriteLine("Readvance Key:{0}", fl.Key);
                    }
                }
            }
        }

        [Test]
        public void CreateFurtherAdvanceApplication()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                SetupApplicationHelpers();
                int AccountKey = -1;
                string query = @"select p.description ,max(a.accountkey)
                                from [2am].[fin].mortgageloan ml (nolock)
                                join [2am].dbo.financialservice fs (nolock) on ml.FinancialServiceKey = fs.FinancialServiceKey
                                JOIN [2am].fin.FinancialAdjustment FA (NOLOCK) ON fs.FinancialServiceKey = fa.FinancialServiceKey
                                join [2am].fin.FixedRateAdjustment fra (NOLOCK) ON fra.FinancialAdjustmentKey = fa.FinancialAdjustmentKey
                                join [2am].dbo.account a (nolock) on fs.accountkey=a.accountkey
                                join [2am].dbo.product p (nolock) on a.rrr_productkey = p.productkey
                                where a.accountstatuskey=1 AND p.productkey = 1
                                group by p.description";
                IDbConnection con = null;
                try
                {
                    con = Helper.GetSQLDBConnection();
                    DataTable dt = new DataTable();
                    Helper.FillFromQuery(dt, query, con, null);
                    AccountKey = Convert.ToInt32(dt.Rows[0][1]);
                    Console.WriteLine("FurtherAdvance using AccountKey:{0}", AccountKey);
                }
                catch (Exception ex)
                {
                    throw new Exception("Unable to get MLA key");
                }
                finally
                {
                    con.Dispose();
                }
                using (new SessionScope())
                {
                    IAccount acc = accRepo.GetAccountByKey(AccountKey);
                    IMortgageLoanAccount mla = acc as IMortgageLoanAccount;

                    if (null != mla)
                    {
                        app = applicationRepository.CreateFurtherAdvanceApplication(mla, true);

                        IApplicationFurtherAdvance fl = app as IApplicationFurtherAdvance;
                        applicationRepository.SaveApplication(fl);
                        Console.WriteLine("Further Advance Key:{0}", fl.Key);
                    }
                }
            }
        }

        [Test]
        public void SearchApplicationsByKey()
        {
            using (new SessionScope())
            {
                // find the first application  & set the search criteria
                IApplicationSearchCriteria searchCriteria = new ApplicationSearchCriteria();
                searchCriteria.AccountKey = Application_DAO.FindFirst().ReservedAccount.Key;

                // perform the search using the repository method
                IEventList<SAHL.Common.BusinessModel.Interfaces.IApplication> applications = applicationRepository.SearchApplications(searchCriteria, 50, false);

                Assert.IsTrue(applications.Count > 0);
                Assert.IsTrue(applications[0].ReservedAccount.Key == searchCriteria.AccountKey);
            }
        }

        [Test]
        public void SearchLifeApplicationsByName()
        {
            //701861
            //Application_DAO.FindAll(
            SessionScope sessionScope = new SessionScope(FlushAction.Never);
            using (sessionScope)
            {
                // find the first life application with an account and a role
                string HQL = "from Application_DAO A where A.ApplicationType.Key = ? and A.Account.Roles.size > 0";
                SimpleQuery query = new SimpleQuery(typeof(SAHL.Common.BusinessModel.DAO.Application_DAO), HQL, (int)SAHL.Common.Globals.OfferTypes.Life);
                query.SetQueryRange(1); // select one record
                object o = Application_DAO.ExecuteQuery(query);
                Application_DAO[] apps = o as Application_DAO[];
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IApplication application = BMTM.GetMappedType<IApplication>(apps[0]);

                // look for a natural person role
                IRole role = null;
                foreach (IRole r in application.Account.Roles)
                {
                    if (r.LegalEntity.LegalEntityType.Key == (int)SAHL.Common.Globals.LegalEntityTypes.NaturalPerson)
                    {
                        role = r;
                        break;
                    }
                }

                if (role == null)
                    Assert.Ignore("No application found with Natural Person Role");

                ILegalEntityNaturalPerson person = role.LegalEntity as ILegalEntityNaturalPerson;

                // find all the roles for the naturalperson found above
                HQL = "from Role_DAO R where R.RoleType.Key = ? and R.LegalEntity.FirstNames = ?";
                query = new SimpleQuery(typeof(SAHL.Common.BusinessModel.DAO.Role_DAO), HQL, (int)SAHL.Common.Globals.RoleTypes.AssuredLife, person.FirstNames);
                o = Role_DAO.ExecuteQuery(query);
                Role_DAO[] roles = o as Role_DAO[];
                int recsFound = roles.Length;

                // set the search criteria
                IApplicationSearchCriteria searchCriteria = new ApplicationSearchCriteria();
                searchCriteria.ClientName = person.FirstNames + " " + person.Surname;
                searchCriteria.ApplicationTypes.Add(OfferTypes.Life);
                if (application.Account == null)
                    searchCriteria.ApplicationHasAccount = false;
                else
                    searchCriteria.ApplicationHasAccount = true;

                // perform the search using the repository method
                IEventList<IApplication> applications = applicationRepository.SearchApplications(searchCriteria, 50, false);

                // check the results
                Assert.IsTrue(applications.Count >= recsFound);
            }
        }

        [Test]
        public void SearchApplicationsByDetails()
        {
            using (new SessionScope())
            {
                // find the first application  & set the search criteria
                IApplicationSearchCriteria searchCriteria = new ApplicationSearchCriteria();

                //searchCriteria.AccountKey = Application_DAO.FindFirst().ReservedAccount.Key;
                //searchCriteria.AccountKey = 1872683;
                searchCriteria.ClientName = "C Moodley";
                searchCriteria.ApplicationStatuses.Add(OfferStatuses.Open);

                //searchCriteria.ApplicationStatuses.Add(OfferStatuses.NTU);
                searchCriteria.ConsultantADUserName = @"SAHL\CraigF";
                searchCriteria.ApplicationTypes.Add(OfferTypes.Life);
                searchCriteria.ApplicationHasAccount = true;

                // perform the search using the repository method
                IEventList<SAHL.Common.BusinessModel.Interfaces.IApplication> applications = applicationRepository.SearchApplications(searchCriteria, 50, false);

                Assert.IsTrue(applications.Count >= 0);

                //Assert.IsTrue(applications[0].ReservedAccount.Key == searchCriteria.AccountKey);
            }
        }

        [Test]
        public void GetApplicationFromInstance()
        {
            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
            using (new SessionScope())
            {
                ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();

                // get first instance from application capture workflow that has an offerkey
                string sql = "SELECT i.* FROM [X2].[X2DATA].Application_Capture xd (nolock) JOIN x2.x2.Instance i (nolock) on i.id = xd.InstanceID JOIN [2am]..Offer o (nolock) on o.OfferKey = xd.ApplicationKey";
                ISession session = sessionHolder.CreateSession(typeof(Instance_DAO));
                IQuery sqlQuery = session.CreateSQLQuery(sql).AddEntity(typeof(Instance_DAO));
                sqlQuery.SetMaxResults(1);
                IList<Instance_DAO> instances_DAO = sqlQuery.List<Instance_DAO>();

                if (instances_DAO.Count == 0)
                    Assert.Ignore("No Instance Data");

                IEventList<IInstance> instances = new DAOEventList<Instance_DAO, IInstance, Instance>(instances_DAO);

                Assert.IsNotNull(instances);
                Assert.IsTrue(instances.Count > 0);

                IInstance instance = instances[0];

                // call the repo method to retreive tha application
                IApplication application = applicationRepository.GetApplicationFromInstance(instance);
                Assert.IsNotNull(application);
                Assert.IsTrue(application.Key > 0);
            }
        }

        [Test]
        public void GetApplicationFromInstanceAndAddCriteria()
        {
            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
            using (new SessionScope())
            {
                ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();

                string sql = @"select top 1 INST.ID
                                from (SELECT xd.InstanceID
		                                FROM [X2].[X2DATA].Application_Capture xd (nolock)
		                                INNER JOIN [2am].dbo.Offer AS o (nolock) ON o.OfferKey = xd.ApplicationKey
		                                INNER JOIN [2am].dbo.OfferRole AS ofr (nolock) ON o.OfferKey = ofr.OfferKey
			                                and ofr.OfferRoleTypeKey = 101
		                                INNER JOIN [2am].dbo.ADUser AS ad (nolock) ON ad.LegalEntityKey = ofr.LegalEntityKey
		                                INNER JOIN [2am].dbo.vUserOrganisationStructureHistory AS vsh (nolock) ON vsh.ADUserKey = ad.ADUserKey
			                                AND vsh.EndDate IS NOT NULL
                                ) cte
                                INNER JOIN X2.X2.Instance AS INST (nolock) ON INST.ID = cte.InstanceID
                                INNER JOIN X2.X2.StateWorkList AS STWL (nolock) ON INST.StateID = STWL.StateID
                                INNER JOIN X2.X2.SecurityGroup AS SG (nolock) ON STWL.SecurityGroupID = SG.ID
	                                AND SG.Name = 'Branch Consultant D'";

                long instanceID = 0;

                using (DBHelper dbHelper = new DBHelper(Databases.TwoAM))
                {
                    using (IDbCommand cmd = dbHelper.CreateCommand(sql))
                    {
                        cmd.CommandTimeout = 60;

                        using (IDataReader reader = dbHelper.ExecuteReader(cmd))
                        {
                            reader.Read();
                            instanceID = reader.GetInt64(0);
                        }
                    }
                }

                IInstance instance = _mockery.StrictMock<IInstance>();
                SetupResult.For(instance.ID).Return(instanceID);

                IWorkFlow workflow = _mockery.StrictMock<IWorkFlow>();
                SetupResult.For(workflow.StorageTable).Return("Application_Capture");
                SetupResult.For(workflow.StorageKey).Return("ApplicationKey");

                SetupResult.For(instance.WorkFlow).Return(workflow);

                _mockery.ReplayAll();

                // lets go find the branch admin role
                IApplication app = applicationRepository.GetApplicationFromInstance(instance);
                SAHL.Common.Globals.OfferRoleTypes[] offerRoleTypes = new SAHL.Common.Globals.OfferRoleTypes[1] { SAHL.Common.Globals.OfferRoleTypes.BranchConsultantD };
                IReadOnlyEventList<ILegalEntity> les = app.GetLegalEntitiesByRoleType(offerRoleTypes, GeneralStatusKey.Active);
                IADUser usr = RepositoryFactory.GetRepository<IOrganisationStructureRepository>().GetAdUserByLegalEntityKey(les[0].Key);

                Hashtable criteria = new Hashtable();

                criteria.Add("ADUserName", usr.ADUserName);
                criteria.Add("ApplicationRoleTypeKey", 101);

                // call the repo method to retreive tha application
                IApplication application = applicationRepository.GetApplicationByInstanceAndAddCriteria(instance, criteria);
                Assert.IsNotNull(application);
                Assert.IsTrue(application.Key > 0);
            }
        }

        [Test]
        public void GetApplicationInformationSuperLoLoan()
        {
            using (new SessionScope())
            {
                int key = Convert.ToInt32(base.GetPrimaryKey("OfferInformationSuperLoLoan", "OfferInformationKey"));
                IApplicationInformationSuperLoLoan appInfo1 = applicationRepository.GetApplicationInformationSuperLoLoan(key);
                Assert.IsNotNull(appInfo1);

                IApplicationInformationSuperLoLoan appInfo2 = applicationRepository.GetApplicationInformationSuperLoLoan(-1);
                Assert.IsNull(appInfo2);
            }
        }

        [Test]
        public void GetApplicationInformationVarifixLoan()
        {
            using (new SessionScope())
            {
                int key = Convert.ToInt32(base.GetPrimaryKey("OfferInformationVarifixLoan", "OfferInformationKey"));
                IApplicationInformationVarifixLoan appInfo1 = applicationRepository.GetApplicationInformationVarifixLoan(key);
                Assert.IsNotNull(appInfo1);

                IApplicationInformationVarifixLoan appInfo2 = applicationRepository.GetApplicationInformationVarifixLoan(-1);
                Assert.IsNull(appInfo2);
            }
        }

        [Test]
        public void GetApplicationInformationVariableLoan()
        {
            using (new SessionScope())
            {
                int key = Convert.ToInt32(base.GetPrimaryKey("OfferInformationVariableLoan", "OfferInformationKey"));
                IApplicationInformationVariableLoan appInfo1 = applicationRepository.GetApplicationInformationVariableLoan(key);
                Assert.IsNotNull(appInfo1);

                IApplicationInformationVariableLoan appInfo2 = applicationRepository.GetApplicationInformationVariableLoan(-1);
                Assert.IsNull(appInfo2);
            }
        }

        [Test]
        public void GetApplicationKeys()
        {
            IList<int> keys1 = applicationRepository.GetApplicationKeys(null, 10);
            Assert.AreEqual(keys1.Count, 0);

            IList<int> keys2 = applicationRepository.GetApplicationKeys("", 10);
            Assert.AreEqual(keys2.Count, 0);

            string sql = "select top 1 OfferKey from [2am]..Offer o (nolock) where LEN(o.OfferKey) > 4";
            DataTable dt = GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data to test");
            string prefix = dt.Rows[0]["OfferKey"].ToString().Substring(0, 4);
            dt.Dispose();

            IList<int> keys3 = applicationRepository.GetApplicationKeys(prefix, 10);
            foreach (int k in keys3)
            {
                if (!k.ToString().StartsWith(prefix))
                    Assert.Fail("{0} does not start with expected prefix {1}", k, prefix);
            }
        }

        [Test]
        public void GetLastDisbursedApplicationByAccountKey()
        {
            using (new SessionScope())
            {
                // get first instance from application capture workflow
                string sql = "select top 1 o.* from offer o (nolock) join Account a (nolock) on o.AccountKey = a.AccountKey "
                    + " where o.OfferStatusKey = 3 and o.OfferTypeKey not in (2, 5, 0) and o.OfferEndDate is not null "
                    + " order by o.OfferEndDate desc";

                DataTable dt = base.GetQueryResults(sql);

                if (dt == null || dt.Rows.Count == 0)
                    Assert.Ignore("No disbursed Application Data found");

                int appKey = Convert.ToInt32(dt.Rows[0][0]);

                //got an application to test with
                IApplication app = applicationRepository.GetApplicationByKey(appKey);

                IApplication appCompare = applicationRepository.GetLastDisbursedApplicationByAccountKey(app.Account.Key);

                Assert.IsTrue(app.Key == appCompare.Key);
            }
        }

        [Test]
        public void GetApplicationRoletypes_Operators()
        {
            string HQL = string.Format("from ApplicationRoleType_DAO art where art.ApplicationRoleTypeGroup.Key=? order by art.Description desc");
            SimpleQuery<ApplicationRoleType_DAO> q = new SimpleQuery<ApplicationRoleType_DAO>(HQL, (int)SAHL.Common.Globals.OfferRoleTypeGroups.Operator);
            ApplicationRoleType_DAO[] arts = q.Execute();

            IEventList<IApplicationRoleType> applicationRoleTypes = new DAOEventList<ApplicationRoleType_DAO, IApplicationRoleType, ApplicationRoleType>(arts);

            Assert.IsTrue(applicationRoleTypes.Count > 0);
        }

        [Test]
        public void GetApplicationRoletypes_NonOperators()
        {
            string HQL = string.Format("from ApplicationRoleType_DAO art where art.ApplicationRoleTypeGroup.Key=? order by art.Description desc");
            SimpleQuery<ApplicationRoleType_DAO> q = new SimpleQuery<ApplicationRoleType_DAO>(HQL, (int)SAHL.Common.Globals.OfferRoleTypeGroups.NonOperator);
            ApplicationRoleType_DAO[] arts = q.Execute();

            IEventList<IApplicationRoleType> applicationRoleTypes = new DAOEventList<ApplicationRoleType_DAO, IApplicationRoleType, ApplicationRoleType>(arts);

            Assert.IsTrue(applicationRoleTypes.Count > 0);
        }

        [Test]
        public void GetApplicationAuditData()
        {
            string sql = "select top 1 PrimaryKeyValue from Warehouse..Audits (nolock) where TableName = 'OfferMortgageLoan'";
            DataTable dt = GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data");
            int offerKey = Convert.ToInt32(dt.Rows[0]["PrimaryKeyValue"]);
            dt.Dispose();

            IEventList<IAudit> auditData = applicationRepository.GetApplicationAuditData(offerKey);
            Assert.Greater(auditData.Count, 0);
        }

        [Test]
        public void HasFurtherLendingInProgressTest()
        {
            using (new SessionScope())
            {
                string sql = "select top 1 o.accountkey from offer o (nolock) where o.OfferStatusKey = 1 and o.OfferTypeKey in (2, 3, 4) order by o.accountkey desc";

                DataTable dt = base.GetQueryResults(sql);

                if (dt == null || dt.Rows.Count == 0)
                    Assert.Ignore("No Data found");

                int appKey = Convert.ToInt32(dt.Rows[0][0]);

                bool hasFurtherLendingInProgress = applicationRepository.HasFurtherLendingInProgress(appKey);

                Assert.IsTrue(hasFurtherLendingInProgress);
            }
        }

        [Test]
        public void GetLatestAcceptedApplicationByAccountKey()
        {
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            using (new SessionScope())
            {
                // get latest open variable loan account
                string sql = "select top 1 a.AccountKey from Account a (nolock) "
                    + " where a.AccountStatusKey = 1 and RRR_OriginationSourceKey = 1 and RRR_ProductKey = 1 "
                    + " order by a.AccountKey desc";

                DataTable dt = base.GetQueryResults(sql);

                if (dt == null || dt.Rows.Count == 0)
                    Assert.Ignore("No open account data found");

                int accKey = Convert.ToInt32(dt.Rows[0][0]);

                // get the account object
                IAccount account = accRepo.GetAccountByKey(accKey);

                // Get latest 'Open/Accepted' loan Offer with OfferInformationtype of 'Accepted' for the above account
                sql = "select top 1 o.OfferKey from	[2am]..Offer o (nolock) "
                    + " join [2am]..OfferInformation oi (nolock) on o.OfferKey = oi.OfferKey "
                    + " where o.ReservedAccountKey = " + accKey
                    + " and o.OfferStatusKey in (1,3) " // open, accepted
                    + " and oi.OfferInformationTypeKey = 3 " // Accepted Offer
                    + " and oi.OfferInformationKey in (select max(offerinformationkey) from [2am]..OfferInformation (nolock) group by OfferKey) " // latest offerinformation
                    + " order by o.OfferKey desc";

                dt = base.GetQueryResults(sql);

                if (dt == null || dt.Rows.Count == 0)
                    Assert.Ignore("No open/accepted offers for account");

                int offerKey = Convert.ToInt32(dt.Rows[0][0]);

                // call the repo method to test
                IApplication appCompare = applicationRepository.GetLastestAcceptedApplication(account);

                // test the results
                Assert.IsTrue(offerKey == appCompare.Key);
            }
        }

        [Test]
        public void GetOfferRolesNotInAccountTest()
        {
            using (new SessionScope())
            {
                int applicationKey = -1;
                IApplication application = _mockery.StrictMock<IApplication>();

                string sql = @"select top 1 ofr.OfferKey
                from [2am].[dbo].offer o (nolock)
                inner join [2am].[dbo].offerRole ofr (nolock)
	                on ofr.offerKey = o.offerKey
                inner join [2am].[dbo].offerRoleType ort (nolock)
	                on ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey
                inner join [2am].[dbo].RoleType rt (nolock)
	                on ort.description = rt.description
                left join [2am].[dbo].role r (nolock)
	                on ofr.legalEntityKey = r.legalEntityKey
                    and o.accountKey = r.accountKey and rt.RoleTypeKey = r.RoleTypeKey
                where ort.OfferRoleTypeGroupKey = 3 and r.LegalEntityKey is null";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                if (o != null)
                    applicationKey = Convert.ToInt32(o);

                SetupResult.For(application.Key).Return(applicationKey);
                _mockery.ReplayAll();
                DataTable dt = applicationRepository.GetOfferRolesNotInAccount(application);
                if (o != null)
                    Assert.IsTrue(dt.Rows.Count > 0);
                else
                    Assert.IsNull(dt);
            }
        }

        [Test]
        public void RapidGoToCreditCheckLTVTestPass()
        {
            using (new SessionScope())
            {
                IApplicationFurtherLoan appFL = _mockery.StrictMock<IApplicationFurtherLoan>();
                IApplicationType appType = _mockery.StrictMock<IApplicationType>();

                double LoanAmount = 1400000D;
                double EstimatedDisbursedLTV = 0.5D;
                SetupResult.For(appFL.LoanAgreementAmount).Return(LoanAmount);
                SetupResult.For(appFL.EstimatedDisbursedLTV).Return(EstimatedDisbursedLTV);
                SetupResult.For(appType.Key).Return((int)OfferTypes.FurtherLoan);
                SetupResult.For(appFL.ApplicationType).Return(appType);
                _mockery.ReplayAll();
                bool result = applicationRepository.RapidGoToCreditCheckLTV(appFL);
                Assert.IsTrue(result);
            }
        }

        [Test]
        public void RapidGoToCreditCheckLTVTestFail()
        {
            using (new SessionScope())
            {
                IApplicationFurtherLoan appFL = _mockery.StrictMock<IApplicationFurtherLoan>();
                IApplicationType appType = _mockery.StrictMock<IApplicationType>();

                double LoanAmount = 1500000D;
                double EstimatedDisbursedLTV = 0.8D;
                SetupResult.For(appFL.LoanAgreementAmount).Return(LoanAmount);
                SetupResult.For(appFL.EstimatedDisbursedLTV).Return(EstimatedDisbursedLTV);
                SetupResult.For(appType.Key).Return((int)OfferTypes.FurtherLoan);
                SetupResult.For(appFL.ApplicationType).Return(appType);
                _mockery.ReplayAll();
                bool result = applicationRepository.RapidGoToCreditCheckLTV(appFL);
                Assert.IsTrue(!result);
            }
        }

        [Test]
        public void GetActiveApplicationRoleForTypeAndKey()
        {
            using (new SessionScope())
            {
                //one rubbish test
                IApplicationRole ar = applicationRepository.GetActiveApplicationRoleForTypeAndKey(-20, -20);
                Assert.IsNull(ar);

                string query = @"select top 5 OfferKey, OfferRoleTypeKey from OfferRole ofr (nolock)
                        where ofr.GeneralStatusKey = 1
                        and ofr.OfferRoleTypeKey > 12
                        order by ofr.OfferRoleKey desc";

                DataTable DT = base.GetQueryResults(query);
                foreach (DataRow dr in DT.Rows)
                {
                    ar = applicationRepository.GetActiveApplicationRoleForTypeAndKey(Convert.ToInt32(dr[0].ToString()), Convert.ToInt32(dr[1].ToString()));
                    Assert.AreEqual(ar.Application.Key, Convert.ToInt32(dr[0].ToString()));
                    Assert.AreEqual(ar.ApplicationRoleType.Key, Convert.ToInt32(dr[1].ToString()));
                }

                DT.Dispose();
            }
        }

        #region Application Helpers

        private static IApplicationStatus status;
        private static IApplication app;
        private static IAccountRepository accRepo;
        private static IOriginationSource os;
        private static IMortgageLoanRepository mlRepo;
        private static ILegalEntityRepository leRepo;

        private void SetupApplicationHelpers()
        {
            if (!_hasSetupHelpers)
            {
                //arRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                mlRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
                os = RepositoryFactory.GetRepository<IApplicationRepository>().GetOriginationSource(OriginationSources.SAHomeLoans);
                status = RepositoryFactory.GetRepository<ILookupRepository>().ApplicationStatuses.ObjectDictionary["1"];
                leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                _hasSetupHelpers = true;
            }
        }

        private IApplicationUnknown SetupEmptyUnknownApplicationType(int originationSourceKey)
        {
            IApplicationUnknown applicationUnknown = applicationRepository.GetEmptyUnknownApplicationType(originationSourceKey);

            if (null != applicationUnknown)
            {
                return applicationUnknown;
            }
            else
                throw new InvalidCastException("Not a IApplicationUnknown");
        }

        private static IApplicationProductNewVariableLoan SetupNVL(IApplicationProduct prod)
        {
            IApplicationProductNewVariableLoan nvl = prod as IApplicationProductNewVariableLoan;
            if (null != nvl)
            {
                //nvl.LoanAgreementAmount = (double)500000;

                nvl.VariableLoanInformation.PropertyValuation = 250000;

                nvl.InterestOnlyInformation.Installment = 12345;
                return nvl;
            }
            else
                throw new InvalidCastException("Not a IApplicationProductNewVariableLoan");
        }

        private static IApplicationProductSuperLoLoan SetupSuperLo(IApplicationProduct prod)
        {
            IApplicationProductSuperLoLoan sl = prod as IApplicationProductSuperLoLoan;
            if (null != sl)
            {
                //sl.LoanAgreementAmount = (double)500000;

                sl.VariableLoanInformation.PropertyValuation = 250000;

                sl.SuperLoInformation.PPThresholdYr1 = 1500;

                sl.InterestOnlyInformation.Installment = 12345;

                return sl;
            }
            else
                throw new InvalidCastException("Not a IApplicationProductSuperLoLoan");
        }

        private static IApplicationProductVariFixLoan SetupVariFix(IApplicationProduct prod)
        {
            IApplicationProductVariFixLoan vfl = prod as IApplicationProductVariFixLoan;
            if (null != vfl)
            {
                //vfl.LoanAgreementAmount = (double)500000;

                vfl.VariableLoanInformation.PropertyValuation = 250000;

                vfl.VariFixInformation.FixedInstallment = 1500;

                return vfl;
            }
            else
                throw new InvalidCastException("Not a IApplicationProductVariFixLoan");
        }

        private void CreateRevision()
        {
            app.CreateRevision();

            applicationRepository.SaveApplication(app);

            Console.WriteLine("Revision Created New OfferInformation: {0}", app.ApplicationInformations[0].Key);
        }

        private static void DeleteApplication()
        {
            using (new SessionScope())
            {
                Application_DAO dao = ((Application)app).GetDAOObject() as Application_DAO;
                dao.DeleteAndFlush();
            }
        }

        #endregion Application Helpers

        [Test]
        public void CompleteCallbackNoUpdate()
        {
            using (new SessionScope())
            {
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                SetRepositoryStrategy(TypeFactoryStrategy.Mock);
                int appKey = 1;

                IApplicationRepository appRMock = _mockery.StrictMock<IApplicationRepository>();
                MockCache.Add(typeof(IApplicationRepository).ToString(), appRMock);

                DateTime? dt = DateTime.Now;
                ICallback cb = _mockery.StrictMock<ICallback>();
                SetupResult.For(cb.Key).Return(1);
                SetupResult.For(cb.CompletedDate).Return(dt);

                IEventList<ICallback> cbList = new EventList<ICallback>();
                cbList.Add(null, cb);

                Expect.Call(appRMock.GetCallBacksByApplicationKey(appKey, true)).Return(cbList).IgnoreArguments();
                _mockery.ReplayAll();

                bool res = appRepo.CompleteCallback(appKey, DateTime.Now);

                Assert.IsTrue(!res);

                SetRepositoryStrategy(TypeFactoryStrategy.Default);
            }
        }

        [Test]
        public void CompleteCallback()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                int appKey = 1;

                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                ILifeRepository lRepo = RepositoryFactory.GetRepository<ILifeRepository>();
                ILookupRepository lkRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                ICallback cb = lRepo.CreateEmptyCallback();

                DateTime now = DateTime.Now;
                string user = "some guy";

                cb.GenericKey = appKey;
                cb.CallbackDate = now;
                cb.CallbackUser = user;
                cb.EntryDate = now;
                cb.EntryUser = user;
                cb.GenericKeyType = lkRepo.GenericKeyType.ObjectDictionary[((int)GenericKeyTypes.Offer).ToString()];
                cb.Reason = null;

                SetRepositoryStrategy(TypeFactoryStrategy.Mock);

                IApplicationRepository appRMock = _mockery.StrictMock<IApplicationRepository>();
                MockCache.Add(typeof(IApplicationRepository).ToString(), appRMock);

                IEventList<ICallback> cbList = new EventList<ICallback>();
                cbList.Add(null, cb);

                Expect.Call(appRMock.GetCallBacksByApplicationKey(appKey, true)).Return(cbList).IgnoreArguments();
                Expect.Call(delegate { appRMock.SaveCallback(cb); }).IgnoreArguments();
                _mockery.ReplayAll();

                bool res = appRepo.CompleteCallback(appKey, DateTime.Now);

                Assert.IsTrue(res);

                SetRepositoryStrategy(TypeFactoryStrategy.Default);
            }
        }

        [Test]
        public void DeleteMeTest()
        {
            ILookupRepository lkRepo = RepositoryFactory.GetRepository<ILookupRepository>();

            var key = lkRepo.GenericKeyType.ObjectDictionary[((int)GenericKeyTypes.Offer).ToString()];

            Assert.That(1 == (2 / 2));
        }

        [Test]
        public void GetApplicantRoleTypesForApplication()
        {
            GetApplicantRoleTypesForApplicationHelper(true);
            GetApplicantRoleTypesForApplicationHelper(false);
        }

        private void GetApplicantRoleTypesForApplicationHelper(bool useLead)
        {
            using (new SessionScope())
            {
                int appKey = 1;

                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                SetRepositoryStrategy(TypeFactoryStrategy.Mock);

                IApplication app = _mockery.StrictMock<IApplication>();
                SetupResult.For(app.Key).Return(appKey);

                IStageDefinitionRepository sdRepoM = _mockery.StrictMock<IStageDefinitionRepository>();
                MockCache.Add(typeof(IStageDefinitionRepository).ToString(), sdRepoM);

                IStageDefinitionStageDefinitionGroup sdsdg = _mockery.StrictMock<IStageDefinitionStageDefinitionGroup>();
                SetupResult.For(sdsdg.Key).Return(1);

                Expect.Call(sdRepoM.GetStageDefinitionStageDefinitionGroup((int)StageDefinitionGroups.ApplicationCapture, (int)StageDefinitions.ApplicationCaptureSubmitted)).Return(sdsdg).IgnoreArguments();
                if (useLead)
                    Expect.Call(sdRepoM.CountCompositeStageOccurance(appKey, 1)).Return(0).IgnoreArguments();
                else
                    Expect.Call(sdRepoM.CountCompositeStageOccurance(appKey, 1)).Return(1).IgnoreArguments();

                _mockery.ReplayAll();

                IDictionary<string, string> roles = appRepo.GetApplicantRoleTypesForApplication(app);

                Assert.That(roles.Count == 2);

                foreach (KeyValuePair<string, string> kvp in roles)
                {
                    if (useLead)
                        Assert.That(kvp.Key == ((int)OfferRoleTypes.LeadMainApplicant).ToString() || kvp.Key == ((int)OfferRoleTypes.LeadSuretor).ToString());
                    else
                        Assert.That(kvp.Key == ((int)OfferRoleTypes.MainApplicant).ToString() || kvp.Key == ((int)OfferRoleTypes.Suretor).ToString());
                }

                SetRepositoryStrategy(TypeFactoryStrategy.Default);
            }
        }

        [Test]
        public void GetApplicationRoleForTypeAndKey()
        {
            using (new SessionScope())
            {
                string query = @"Select top 1 OfferKey, OfferRoleTypeKey From OfferRole (nolock) ";

                DataTable DT = base.GetQueryResults(query);
                if (DT != null)
                {
                    if (DT.Rows.Count > 0)
                    {
                        int iApplicationKey = Convert.ToInt32(DT.Rows[0]["OfferKey"]);
                        int iApplicationRoleTypeKey = Convert.ToInt32(DT.Rows[0]["OfferRoleTypeKey"]);

                        IApplicationRole appRole = applicationRepository.GetApplicationRoleForTypeAndKey(iApplicationKey, iApplicationRoleTypeKey);
                        Assert.IsNotNull(appRole);
                    }
                }
                else
                {
                    Assert.Fail("No valid keys for this test.");
                }
            }
        }

        [Test]
        public void GetApplicationRolesForKey()
        {
            using (new SessionScope())
            {
                string query = @"Select top 1 OfferKey " +
                                "From OfferRole OR1 (nolock) " +
                                "Inner Join OfferRoleType ORT (nolock) On OR1.OfferRoleTypeKey = ORT.OfferRoleTypeKey " +
                                "Where ORT.OfferRoleTypeGroupKey = 1";

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), new ParameterCollection());
                if (obj != null)
                {
                    int iApplicationKey = Convert.ToInt32(obj);

                    IEventList<IApplicationRole> lstAppRoles = applicationRepository.GetApplicationRolesForKey(iApplicationKey);
                    Assert.IsNotNull(lstAppRoles);
                }
                else
                {
                    Assert.Fail("No valid keys for this test.");
                }
            }
        }

        [Test]
        public void GetApplicationRoleTypesForKeys()
        {
            using (new SessionScope())
            {
                string query = @"Select top 1 OfferKey, OfferRoleTypeKey From OfferRole (nolock) ";

                DataTable DT = base.GetQueryResults(query);
                List<int> Keys = new List<int>();
                if (DT != null)
                {
                    if (DT.Rows.Count > 0)
                    {
                        int iApplicationKey = Convert.ToInt32(DT.Rows[0]["OfferKey"]);
                        int iApplicationRoleTypeKey = Convert.ToInt32(DT.Rows[0]["OfferRoleTypeKey"]);
                        Keys.Add(iApplicationRoleTypeKey);

                        IEventList<IApplicationRole> lstappRoles = applicationRepository.GetApplicationRoleTypesForKeys(Keys, iApplicationKey);
                        Assert.IsNotNull(lstappRoles);
                    }
                }
                else
                {
                    Assert.Fail("No valid keys for this test.");
                }
            }
        }

        [Test]
        public void GetCurrentCreditDecision()
        {
            using (new SessionScope())
            {
                string query = @"Select top 1 OfferKey From Offer (nolock) ";

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), new ParameterCollection());
                if (obj != null)
                {
                    int iApplicationKey = Convert.ToInt32(obj);
                    string strCurrentCreditDecision;
                    int iADUserKey;
                    DateTime dtDecisionDate;
                    applicationRepository.GetCurrentCreditDecision(iApplicationKey, out strCurrentCreditDecision, out iADUserKey, out dtDecisionDate);
                    Assert.IsNotNull(strCurrentCreditDecision);
                    Assert.IsNotNull(iADUserKey);
                    Assert.IsNotNull(dtDecisionDate);
                }
                else
                {
                    Assert.Fail("No valid keys for this test.");
                }
            }
        }

        #region StevenWIP

        [Test]
        public void GetApplicationLifeByKey()
        {
            using (new SessionScope())
            {
                string sql = @"Select top 1 Offerkey from OfferLife (nolock) order by Offerkey desc";
                DataTable dt = base.GetQueryResults(sql);
                if (dt == null || dt.Rows.Count == 0)
                    Assert.Ignore("No Offer Life data found");

                int key = Convert.ToInt32(dt.Rows[0][0]);
                IApplicationLife app = applicationRepository.GetApplicationLifeByKey(key);
                Assert.IsNotNull(app);
            }
        }

        [Test]
        public void GetApplicationRevisionHistory()
        {
            using (new SessionScope())
            {
                string sql = @"SELECT top 1 * FROM  OfferInformation (nolock) order by 1 desc";
                DataTable dt = base.GetQueryResults(sql);
                if (dt.Rows.Count > 0)
                {
                    int key = Convert.ToInt32(dt.Rows[0][2].ToString());
                    IEventList<IApplicationInformation> aiList = applicationRepository.GetApplicationRevisionHistory(key);

                    //Check for pass
                    Assert.Greater(aiList.Count, 0);
                }

                ////Check for fail
                //IEventList<IApplicationInformation>  aNList = _appRepo.GetApplicationRevisionHistory(-1);
                //Assert.IsNull(aNList);
            }
        }

        [Test]
        public void GetApplicationInformationInterestOnly()
        {
            {
                using (new SessionScope())
                {
                    IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                    string query = @"select top 1 * from OfferInformationInterestOnly (nolock) order by 1 desc";

                    DataTable DT = base.GetQueryResults(query);

                    if (DT.Rows.Count > 0)
                    {
                        int key = Convert.ToInt32(DT.Rows[0][0].ToString());
                        IApplicationInformationInterestOnly aie = appRepo.GetApplicationInformationInterestOnly(key);

                        Assert.AreEqual(aie.Installment.ToString(), DT.Rows[0][1].ToString());
                        Assert.AreEqual(aie.MaturityDate.ToString(), DT.Rows[0][2].ToString());
                    }

                    IApplicationInformationInterestOnly aieN = appRepo.GetApplicationInformationInterestOnly(-1);

                    Assert.IsNull(aieN);
                }
            }
        }

        #endregion StevenWIP

        [Test]
        public void SaveApplication()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                int appKeyKey = Application_DAO.FindFirst().Key;
                IApplication app = applicationRepository.GetApplicationByKey(appKeyKey);
                app.ApplicationStartDate = DateTime.Now;
                applicationRepository.SaveApplication(app);
            }
        }

        [Test]
        public void SaveApplicationDebtSettlement()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                ApplicationDebtSettlement_DAO ads = ApplicationDebtSettlement_DAO.FindFirst();
                if (ads != null)
                {
                    IApplicationDebtSettlement applicationDebtSettlement = BMTM.GetMappedType<IApplicationDebtSettlement, ApplicationDebtSettlement_DAO>(ads);
                    applicationDebtSettlement.SettlementDate = DateTime.Now;
                    applicationRepository.SaveApplicationDebtSettlement(applicationDebtSettlement);
                }
            }
        }

        [Test]
        public void SaveApplicationDeclaration()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                ApplicationDeclaration_DAO ad = ApplicationDeclaration_DAO.FindFirst();
                if (ad != null)
                {
                    IApplicationDeclaration applicationDeclaration = BMTM.GetMappedType<IApplicationDeclaration, ApplicationDeclaration_DAO>(ad);
                    applicationDeclaration.ApplicationDeclarationDate = DateTime.Now;
                    applicationRepository.SaveApplicationDeclaration(applicationDeclaration);
                }
            }
        }

        [Test]
        public void SaveApplicationExpense()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                ApplicationExpense_DAO ae = ApplicationExpense_DAO.FindFirst();
                if (ae != null)
                {
                    IApplicationExpense applicationExpense = BMTM.GetMappedType<IApplicationExpense, ApplicationExpense_DAO>(ae);
                    applicationExpense.ExpenseAccountName = "test";
                    applicationRepository.SaveApplicationExpense(applicationExpense);
                }
            }
        }

        [Test]
        public void SaveApplicationInternetReferrer()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                ApplicationInternetReferrer_DAO air = ApplicationInternetReferrer_DAO.FindFirst();
                if (air != null)
                {
                    IApplicationInternetReferrer applicationInternetReferrer = BMTM.GetMappedType<IApplicationInternetReferrer, ApplicationInternetReferrer_DAO>(air);
                    applicationInternetReferrer.ReferringServerURL = "test";
                    applicationRepository.SaveApplicationInternetReferrer(applicationInternetReferrer);
                }
            }
        }

        [Test]
        public void SaveAllOpenCallBacks()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                IDomainMessageCollection Messages = new DomainMessageCollection();

                // find the first open callback with an offerkey to use in our test
                ICriterion[] criterion = new ICriterion[]
                    {
                        Expression.Eq("GenericKeyType.Key", Convert.ToInt32(SAHL.Common.Globals.GenericKeyTypes.Offer))
                        , Expression.IsNull("CompletedDate")
                    };
                Callback_DAO cb = Callback_DAO.FindFirst(criterion);
                if (cb == null)
                    Assert.Ignore("No callbacks available.");

                int applicationKey = cb.GenericKey;

                // use the repository method to retreive the data and compare to the result obtained above
                IEventList<ICallback> callbacks = applicationRepository.GetCallBacksByApplicationKey(applicationKey, true);
                Assert.IsNotNull(callbacks, string.Format("No Open Callbacks found for offerkey {0}", applicationKey));

                // make sure all the callbacks that were retreived were for this offer and are 'open'
                foreach (ICallback cbs in callbacks)
                {
                    applicationRepository.SaveCallback(cbs);
                    Assert.AreEqual(applicationKey, cbs.GenericKey);
                    Assert.IsFalse(cbs.CompletedDate.HasValue);
                }
            }
        }

        [Test]
        public void GetApplicationSourceByKey()
        {
            using (new SessionScope())
            {
                string query = @"Select top 1 OfferSourceKey From OfferSource (nolock) ";

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), new ParameterCollection());
                if (obj != null)
                {
                    int iApplicationSourceKey = Convert.ToInt32(obj);

                    IApplicationSource appSource = applicationRepository.GetApplicationSourceByKey(iApplicationSourceKey);
                    Assert.IsNotNull(iApplicationSourceKey);
                }
                else
                {
                    Assert.Fail("No valid keys for this test.");
                }
            }
        }

        [Test]
        public void GetApplicationAttributeTypeByIsGeneric()
        {
            using (new SessionScope())
            {
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                string query = @"select * from OfferAttributeType (nolock)";

                DataTable DT = base.GetQueryResults(query);

                DataColumn[] keys = new DataColumn[1];
                keys[0] = DT.Columns[0];
                DT.PrimaryKey = keys;

                IList<IApplicationAttributeType> genAttList = appRepo.GetApplicationAttributeTypeByIsGeneric(true);

                foreach (IApplicationAttributeType att in genAttList)
                {
                    DataRow dr = DT.Rows.Find(att.Key);

                    Assert.IsNotNull(dr);
                    Assert.AreEqual("True", dr[2].ToString());
                }

                IList<IApplicationAttributeType> notGenAttList = appRepo.GetApplicationAttributeTypeByIsGeneric(false);

                foreach (IApplicationAttributeType att in notGenAttList)
                {
                    DataRow dr = DT.Rows.Find(att.Key);

                    Assert.IsNotNull(dr);
                    Assert.AreEqual("False", dr[2].ToString());
                }
            }
        }

        [Test]
        public void GetApplicationByReservedAccountKey()
        {
            using (new SessionScope())
            {
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                string query = @"select Max(o.ReservedAccountKey) as OfferKey, OfferTypeKey
                    from Offer o
                    where offertypekey in (6, 7, 8) and offerstatuskey = 1 and OriginationSourceKey != 4
                    and datediff(dd, o.offerstartdate, getdate()) > 0
                    group by offertypekey";

                DataTable DT = base.GetQueryResults(query);

                foreach (DataRow dr in DT.Rows)
                {
                    IApplication app = appRepo.GetApplicationByReservedAccountKey(Convert.ToInt32(dr[0].ToString()));

                    Assert.IsNotNull(app, String.Format(@"AppKey: {0}; OfferTypeKey: {1} returned a null application", dr[0].ToString(), dr[1].ToString()));
                    Assert.AreEqual(app.ApplicationType.Key.ToString(), dr[1].ToString());
                }

                IApplication appN = appRepo.GetApplicationByReservedAccountKey(-1);
                Assert.IsNull(appN);
            }
        }

        [Test]
        public void GetApplicationDefaultResetConfiguration()
        {
            using (new SessionScope())
            {
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                IResetConfiguration rc = appRepo.GetApplicationDefaultResetConfiguration();

                Assert.AreEqual(rc.Key, 2);
            }
        }

        [Test]
        public void GetApplicationInformationEdge()
        {
            using (new SessionScope())
            {
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                string query = @"select top 1 * from OfferInformationEdge (nolock)
                    order by 1 desc";

                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count > 0)
                {
                    int key = Convert.ToInt32(DT.Rows[0][0].ToString());
                    IApplicationInformationEdge aie = appRepo.GetApplicationInformationEdge(key);

                    Assert.AreEqual(aie.FullTermInstalment.ToString(), DT.Rows[0][1].ToString());
                    Assert.AreEqual(aie.AmortisationTermInstalment.ToString(), DT.Rows[0][2].ToString());
                    Assert.AreEqual(aie.InterestOnlyInstalment.ToString(), DT.Rows[0][3].ToString());
                    Assert.AreEqual(aie.InterestOnlyTerm.ToString(), DT.Rows[0][4].ToString());
                }

                IApplicationInformationEdge aieN = appRepo.GetApplicationInformationEdge(-1);

                Assert.IsNull(aieN);
            }
        }

        [Test]
        public void GetApplicationDebitOrdersByApplicationKey()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                string query = @"select top 1 OfferKey, count(OfferKey) DOCount from OfferDebitOrder (nolock)
                    group by OfferKey
                    order by count(OfferKey) desc";

                DataTable DT = base.GetQueryResults(query);

                DataColumn[] keys = new DataColumn[1];
                keys[0] = DT.Columns[0];
                DT.PrimaryKey = keys;

                if (DT.Rows.Count > 0)
                {
                    int key = Convert.ToInt32(DT.Rows[0][0].ToString());
                    int count = Convert.ToInt32(DT.Rows[0][1].ToString());
                    IEventList<IApplicationDebitOrder> doList = appRepo.GetApplicationDebitOrdersByApplicationKey(key);

                    Assert.IsNotNull(doList);
                    Assert.AreEqual(doList.Count, count);
                }

                IEventList<IApplicationDebitOrder> doListF = appRepo.GetApplicationDebitOrdersByApplicationKey(-1);
                Assert.AreEqual(doListF.Count, 0);
            }
        }

        [Test]
        public void GetApplicationDeclarationsByapplicationRoleKey()
        {
            using (new SessionScope())
            {
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                string query = @"select * from OfferDeclaration (nolock)
                        where offerrolekey in
                            (select max(offerrolekey) from OfferDeclaration)";

                DataTable DT = base.GetQueryResults(query);

                DataColumn[] keys = new DataColumn[1];
                keys[0] = DT.Columns[2];
                DT.PrimaryKey = keys;

                if (DT.Rows.Count > 0)
                {
                    int key = Convert.ToInt32(DT.Rows[0][1].ToString());
                    int count = DT.Rows.Count;

                    IList<IApplicationDeclaration> adList = appRepo.GetApplicationDeclarationsByapplicationRoleKey(key);

                    foreach (IApplicationDeclaration ad in adList)
                    {
                        DataRow dr = DT.Rows.Find(ad.ApplicationDeclarationQuestion.Key);

                        Assert.IsNotNull(dr);
                        Assert.AreEqual(ad.ApplicationDeclarationAnswer.Key.ToString(), dr[3].ToString());
                    }
                }
            }
        }

        [Test]
        public void GetApplicationRoleTypeByKey()
        {
            using (new SessionScope())
            {
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                foreach (OfferRoleTypes ort in Enum.GetValues(typeof(OfferRoleTypes)))
                {
                    IApplicationRoleType rt = appRepo.GetApplicationRoleTypeByKey(ort);

                    Assert.AreEqual((int)ort, rt.Key);
                }
            }
        }

        [Test]
        public void GetApplicationDeclarationsByLETypeAndApplicationRoleAndOSP()
        {
            using (new SessionScope())
            {
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                //this is testing db config, select limits the check to:
                //osp = SAHL
                //LEType = NatPerson (only configuration captured when this test was written)
                //OfferRoleTypes 11	Main Applicant, 12	Suretor

                string query = @"select distinct cfg.LegalEntityTypeKey, cfg.GenericKey, cfg.OriginationSourceProductKey
                    from OfferDeclarationQuestionAnswerConfiguration cfg (nolock)
                    join OriginationSourceProduct osp (nolock) on cfg.OriginationSourceProductKey = osp.OriginationSourceProductKey
                    where osp.OriginationSourceKey = 1
                    and cfg.LegalEntityTypeKey = " + (int)SAHL.Common.Globals.LegalEntityTypes.NaturalPerson +
                    " and cfg.GenericKey in (11, 12) " +
                    "and cfg.GenericKeyTypeKey = " + (int)SAHL.Common.Globals.GenericKeyTypes.OfferRoleType;

                DataTable DT = base.GetQueryResults(query);

                foreach (DataRow dr in DT.Rows)
                {
                    int leTypeKey = Convert.ToInt32(dr[0].ToString());
                    int genericKey = Convert.ToInt32(dr[1].ToString());
                    int ospKey = Convert.ToInt32(dr[2].ToString());
                    IList<IApplicationDeclarationQuestionAnswerConfiguration> cfg = appRepo.GetApplicationDeclarationsByLETypeAndApplicationRoleAndOSP((int)SAHL.Common.Globals.LegalEntityTypes.NaturalPerson, genericKey, (int)SAHL.Common.Globals.GenericKeyTypes.OfferRoleType, ospKey);

                    Assert.Greater(cfg.Count, 0);
                }
            }
        }

        [Test]
        public void GetPrimaryAndSecondaryApplicants()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                string query = @"
                        --select offer with 1 natural person main applicant and 1 suretor
                    select top 1 offerkey from offer (nolock) where offerkey in
                    (select ofr.offerkey
                    from offerrole ofr (nolock)
                    join LegalEntity le (nolock) on le.LegalEntityKey = ofr.LegalEntityKey
                    join OfferRoleAttribute ora (nolock) on ora.OfferRoleKey = ofr.OfferRoleKey
                    join Employment e (nolock) on e.LegalEntityKey = le.LegalEntityKey
                    where ofr.OfferRoleTypeKey = 11
                    and ofr.GeneralStatusKey = 1
                    and le.LegalEntityTypeKey = 2
                    and e.EmploymentStatusKey = 1
                    and ora.OfferRoleAttributeTypeKey = 1
                    group by ofr.offerkey
                    having count(ofr.offerrolekey) = 1
                    )  ";

                //intersect
                //select ofr.offerkey
                //from offerrole ofr
                //join LegalEntity le on le.LegalEntityKey = ofr.LegalEntityKey
                //join OfferRoleAttribute ora on ora.OfferRoleKey = ofr.OfferRoleKey
                //join Employment e on e.LegalEntityKey = le.LegalEntityKey
                //where ofr.OfferRoleTypeKey = 12
                //and ofr.GeneralStatusKey = 1
                //and le.LegalEntityTypeKey = 2
                //and e.EmploymentStatusKey = 1
                //and ora.OfferRoleAttributeTypeKey = 1
                //group by ofr.offerkey
                //having count(ofr.offerrolekey) = 1

                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                int offerKey = Convert.ToInt32(DT.Rows[0][0]);
                int pLEKey = -1;
                int sLEKey = -1;
                int numApplicants = -1;

                applicationRepository.GetPrimaryAndSecondaryApplicants(offerKey, out pLEKey, out sLEKey, out numApplicants);

                Assert.That(pLEKey > -1);
                Assert.That(pLEKey != sLEKey);
            }
        }

        [Test]
        public void GetPrimaryAndSecondaryApplicantsWithSuretorsOnly()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                string query = @"
                    select top 1 ofr.offerkey
                    from offerrole ofr (nolock)
                    join LegalEntity le (nolock) on le.LegalEntityKey = ofr.LegalEntityKey
                    join OfferRoleAttribute ora (nolock) on ora.OfferRoleKey = ofr.OfferRoleKey
                    join Employment e (nolock) on e.LegalEntityKey = le.LegalEntityKey
                    where ofr.OfferRoleTypeKey = 12
                    and ofr.GeneralStatusKey = 1
                    and le.LegalEntityTypeKey = 2
                    and e.EmploymentStatusKey = 1
                    and ora.OfferRoleAttributeTypeKey = 1
                    and offerkey not in
                    (
					select ofr.offerkey
                    from offerrole ofr (nolock)
                    join LegalEntity le (nolock) on le.LegalEntityKey = ofr.LegalEntityKey
                    join OfferRoleAttribute ora (nolock) on ora.OfferRoleKey = ofr.OfferRoleKey
                    join Employment e (nolock) on e.LegalEntityKey = le.LegalEntityKey
                    where ofr.OfferRoleTypeKey = 11
                    and ofr.GeneralStatusKey = 1
                    and le.LegalEntityTypeKey = 2
                    and e.EmploymentStatusKey = 1
                    and ora.OfferRoleAttributeTypeKey = 1
                    )
                    group by ofr.offerkey
                    having count(ofr.offerrolekey) = 2";

                DataTable DT = base.GetQueryResults(query);

                //if no such data exists, then do not continue
                if (DT.Rows.Count == 0)
                    Assert.Ignore();

                int offerKey = Convert.ToInt32(DT.Rows[0][0]);
                int pLEKey = -1;
                int sLEKey = -1;
                int numApplicants = -1;

                applicationRepository.GetPrimaryAndSecondaryApplicants(offerKey, out pLEKey, out sLEKey, out numApplicants);

                Assert.That(pLEKey > -1);
                Assert.That(sLEKey > -1);
                Assert.That(numApplicants == 2);
                Assert.That(pLEKey != sLEKey);
            }
        }

        [Test]
        public void UpdateApplicationAttributeTypes_Should_AddAttributeIfItDoesntExist()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                IApplication application = applicationRepository.GetApplicationByKey(Application_DAO.FindFirst().Key);
                applicationRepository.UpdateApplicationAttributeTypes(new List<Interfaces.DataTransferObjects.ApplicationAttributeToApply>
				{
					new Interfaces.DataTransferObjects.ApplicationAttributeToApply{
						ApplicationAttributeTypeKey = (int)OfferAttributeTypes.BuildingLoan,
						ToBeRemoved = false
					}
				}, application);
                Assert.IsTrue(application.HasAttribute(OfferAttributeTypes.BuildingLoan));
            }
        }

        [Test]
        public void UpdateApplicationAttributeTypes_Should_RemoveAttributeIfItExist()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                IApplication application = applicationRepository.GetApplicationByKey(Application_DAO.FindFirst().Key);
                applicationRepository.UpdateApplicationAttributeTypes(new List<Interfaces.DataTransferObjects.ApplicationAttributeToApply>
				{
					new Interfaces.DataTransferObjects.ApplicationAttributeToApply{
						ApplicationAttributeTypeKey = (int)OfferAttributeTypes.BuildingLoan,
						ToBeRemoved = true
					}
				}, application);
                Assert.IsFalse(application.HasAttribute(OfferAttributeTypes.BuildingLoan));
            }
        }

        [Test]
        public void RefreshDAOObjectTest()
        {
            using (new SessionScope())
            {
                ICommonRepository commonRepository = RepositoryFactory.GetRepository<ICommonRepository>();
                Application_DAO application = Application_DAO.Find(1252794);
                application.ApplicationStartDate = DateTime.Now;
                //application.Account.OpenDate = DateTime.Now.AddDays(10);
                application.ApplicationConditions.Clear();
                commonRepository.RefreshDAOObject<IApplication>(1252794);
            }
        }

        [Test]
        public void CreateWebLeadTest()
        {
            ILeadInputInformation leadInput = new LeadInputInformation
            {
                OfferSourceKey = (int)OfferSources.InternetLead,
                HouseholdIncome = 50000,
                Term = 240,
                //Lead
                MonthlyInstalment = 50000,
                InterestRate = 0.08,
                TotalPrice = 1500000,

                //  LegalEntity
                FirstNames = "Some",
                Surname = "Dude",
                //EmailAddress = "sd@ano.com",
                HomePhoneCode = "031",
                HomePhoneNumber = "5555555",
                NumberOfApplicants = 1,
                //Referer
                AdvertisingCampaignID = "AdvertisingCampaignID",
                ReferringServerURL = "ReferringServerURL",
                UserURL = "UserURL",
                UserAddress = "UserAddress"
            };

            IApplicationRepository applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            using (var transactionScope = new TransactionScope(OnDispose.Rollback))
            {
                int offerkey = applicationRepo.CreateWebLead(leadInput);
                Assert.AreNotEqual(-1, offerkey);
            }
        }

        [Test]
        public void GenerateLeadFromWebTest()
        {
            ILeadInputInformation leadInput = new LeadInputInformation
            {
                OfferSourceKey = (int)OfferSources.InternetLead,
                HouseholdIncome = 50000,
                Term = 240,
                //Lead
                MonthlyInstalment = 50000,
                InterestRate = 0.08,
                TotalPrice = 1500000,
                // LegalEntity
                FirstNames = "Some",
                Surname = "Dude",
                HomePhoneCode = "031",
                HomePhoneNumber = "5555555",
                NumberOfApplicants = 1,
                //Referer
                AdvertisingCampaignID = "AdvertisingCampaignID",
                ReferringServerURL = "ReferringServerURL",
                UserURL = "UserURL",
                UserAddress = "UserAddress"
            };

            using (new TransactionScope(OnDispose.Rollback))
            {
                int offerKey = applicationRepository.CreateWebLead(leadInput);
                bool success = applicationRepository.GenerateLeadFromWeb(offerKey, leadInput);

                IApplication application = applicationRepository.GetApplicationByKey(offerKey);

                Assert.AreEqual((int)OfferSources.InternetLead, application.ApplicationSource.Key);

                Assert.Greater(application.ApplicationRoles.Count, 0);

                LegalEntityNaturalPerson legalEntity = (LegalEntityNaturalPerson)application.ApplicationRoles[0].LegalEntity;

                Assert.AreEqual(leadInput.FirstNames, legalEntity.FirstNames);
                Assert.AreEqual(leadInput.Surname, legalEntity.Surname);
                Assert.Greater(legalEntity.Employment.Count, 0);
                Assert.AreEqual(typeof(ApplicationUnknown), application.GetType());
            }
        }

        [Test]
        public void GenerateApplicationFromWeb_Switch_Test()
        {
            ILeadInputInformation leadInput = new LeadInputInformation
            {
                EmploymentTypeKey = (int)EmploymentTypes.Salaried,
                CapitaliseFees = true,
                ProductKey = (int)Products.NewVariableLoan,
                OfferSourceKey = (int)OfferSources.InternetApplication,
                PropertyValue = 1500000,
                MortgageLoanPurposeKey = (int)MortgageLoanPurposes.Switchloan,
                CashOut = 500000,
                CurrentLoan = 500000,
                HouseholdIncome = 50000,
                Term = 240,
                ////N = PropertyValue (purchase price) - deposit
                //double LoanAmountRequired { get; } //R
                FixPercent = 0,
                InterestOnly = false,

                //  LegalEntity
                FirstNames = "Some",
                Surname = "Dude",
                //EmailAddress = "sd@ano.com",
                HomePhoneCode = "031",
                HomePhoneNumber = "5555555",
                NumberOfApplicants = 1,
                //Referer
                AdvertisingCampaignID = "AdvertisingCampaignID",
                ReferringServerURL = "ReferringServerURL",
                UserURL = "UserURL",
                UserAddress = "UserAddress"
            };

            using (new TransactionScope(OnDispose.Rollback))
            {
                int offerKey = applicationRepository.CreateWebLead(leadInput);
                bool success = applicationRepository.GenerateApplicationFromWeb(offerKey, leadInput);

                Assert.True(success, "GenerateApplicationFromWeb() Failed.");

                IApplication application = applicationRepository.GetApplicationByKey(offerKey);

                Type expected = typeof(ApplicationMortgageLoanSwitch);
                Assert.AreEqual(expected, application.GetType());

                ApplicationMortgageLoanSwitch appSwitch = (ApplicationMortgageLoanSwitch)application;
                Assert.AreEqual(appSwitch.ApplicationSource.Key, leadInput.OfferSourceKey, "Offer Sources do not match between the Lead input and the application");

                Assert.AreEqual(true, appSwitch.CapitaliseFees);

                Assert.AreEqual(leadInput.PropertyValue, appSwitch.ClientEstimatePropertyValuation);

                IApplicationProductNewVariableLoan product = (IApplicationProductNewVariableLoan)appSwitch.CurrentProduct;

                Assert.AreEqual(typeof(ApplicationInformationVariableLoan), product.VariableLoanInformation.GetType());

                ApplicationInformationVariableLoan appInfo = (ApplicationInformationVariableLoan)product.VariableLoanInformation;

                Assert.AreEqual(appInfo.CashDeposit, leadInput.Deposit);
                Assert.AreEqual(appInfo.EmploymentType.Key, leadInput.EmploymentTypeKey);
                Assert.AreEqual(appInfo.ExistingLoan, leadInput.CurrentLoan);

                Assert.AreEqual(appInfo.HouseholdIncome, leadInput.HouseholdIncome);
                Assert.AreEqual(appInfo.PropertyValuation, leadInput.PropertyValue);
                Assert.AreEqual(appInfo.Term, leadInput.Term);

                IApplicationInformationVariableLoanForSwitchAndRefinance appSwitchOrRefinance = product.VariableLoanInformation as IApplicationInformationVariableLoanForSwitchAndRefinance;

                Assert.NotNull(appSwitchOrRefinance);

                Assert.AreEqual(appSwitchOrRefinance.RequestedCashAmount, leadInput.CashOut);
            }
        }

        [Test]
        public void GenerateApplicationFromWeb_Refinance_Test()
        {
            ILeadInputInformation leadInput = new LeadInputInformation
            {
                EmploymentTypeKey = (int)EmploymentTypes.Salaried,
                CapitaliseFees = true,
                ProductKey = (int)Products.NewVariableLoan,
                OfferSourceKey = (int)OfferSources.InternetApplication,
                PropertyValue = 1500000,
                MortgageLoanPurposeKey = (int)MortgageLoanPurposes.Refinance,
                CashOut = 500000,
                CurrentLoan = 500000,
                HouseholdIncome = 50000,
                Term = 240,
                ////N = PropertyValue (purchase price) - deposit
                //double LoanAmountRequired { get; } //R
                FixPercent = 0,
                InterestOnly = false,

                //  LegalEntity
                FirstNames = "Some",
                Surname = "Dude",
                //EmailAddress = "sd@ano.com",
                HomePhoneCode = "031",
                HomePhoneNumber = "5555555",
                NumberOfApplicants = 1,
                //Referer
                AdvertisingCampaignID = "AdvertisingCampaignID",
                ReferringServerURL = "ReferringServerURL",
                UserURL = "UserURL",
                UserAddress = "UserAddress"
            };

            using (new TransactionScope(OnDispose.Rollback))
            {
                int offerKey = applicationRepository.CreateWebLead(leadInput);
                bool success = applicationRepository.GenerateApplicationFromWeb(offerKey, leadInput);

                Assert.True(success, "GenerateApplicationFromWeb() Failed.");

                IApplication application = applicationRepository.GetApplicationByKey(offerKey);
                Assert.AreEqual(application.ApplicationSource.Key, leadInput.OfferSourceKey, "Offer Sources do not match between the Lead input and the application");

                Type expected = typeof(ApplicationMortgageLoanRefinance);
                Assert.AreEqual(expected, application.GetType());

                ApplicationMortgageLoanRefinance appRefinance = (ApplicationMortgageLoanRefinance)application;

                Assert.AreEqual(true, appRefinance.CapitaliseFees);

                Assert.AreEqual(leadInput.PropertyValue, appRefinance.ClientEstimatePropertyValuation);

                IApplicationProductNewVariableLoan product = (IApplicationProductNewVariableLoan)appRefinance.CurrentProduct;

                Assert.AreEqual(typeof(ApplicationInformationVariableLoan), product.VariableLoanInformation.GetType());

                ApplicationInformationVariableLoan appInfo = (ApplicationInformationVariableLoan)product.VariableLoanInformation;

                Assert.AreEqual(appInfo.CashDeposit, leadInput.Deposit);
                Assert.AreEqual(appInfo.EmploymentType.Key, leadInput.EmploymentTypeKey);
                Assert.AreEqual(appInfo.ExistingLoan, leadInput.CurrentLoan);

                Assert.AreEqual(appInfo.HouseholdIncome, leadInput.HouseholdIncome);
                Assert.AreEqual(appInfo.PropertyValuation, leadInput.PropertyValue);
                Assert.AreEqual(appInfo.Term, leadInput.Term);

                IApplicationInformationVariableLoanForSwitchAndRefinance appSwitchOrRefinance = product.VariableLoanInformation as IApplicationInformationVariableLoanForSwitchAndRefinance;

                Assert.NotNull(appSwitchOrRefinance);

                Assert.AreEqual(appSwitchOrRefinance.RequestedCashAmount, leadInput.CashOut);
            }
        }

        [Test]
        public void GenerateApplicationFromWeb_NewPurchase_Test()
        {
            ILeadInputInformation leadInput = new LeadInputInformation
            {
                EmploymentTypeKey = (int)EmploymentTypes.Salaried,
                ProductKey = (int)Products.NewVariableLoan,
                OfferSourceKey = (int)OfferSources.InternetApplication,
                PropertyValue = 1500000,
                MortgageLoanPurposeKey = (int)MortgageLoanPurposes.Newpurchase,
                Deposit = 500000,
                HouseholdIncome = 50000,
                Term = 240,
                ////N = PropertyValue (purchase price) - deposit
                //double LoanAmountRequired { get; } //R
                FixPercent = 0,
                InterestOnly = true,

                //  LegalEntity
                FirstNames = "Some",
                Surname = "Dude",
                //EmailAddress = "sd@ano.com",
                HomePhoneCode = "031",
                HomePhoneNumber = "5555555",
                NumberOfApplicants = 1,
                //Referer
                AdvertisingCampaignID = "tabid=41&error=The+operation+has+timed+out&content=0",
                ReferringServerURL = "ReferringServerURL",
                UserURL = "UserURL",
                UserAddress = "UserAddress"
            };

            using (new TransactionScope(OnDispose.Rollback))
            {
                int offerKey = applicationRepository.CreateWebLead(leadInput);
                bool success = applicationRepository.GenerateApplicationFromWeb(offerKey, leadInput);

                Assert.True(success, "GenerateApplicationFromWeb() Failed.");

                IApplication application = applicationRepository.GetApplicationByKey(offerKey);

                Type expected = typeof(ApplicationMortgageLoanNewPurchase);
                Assert.AreEqual(expected, application.GetType());

                ApplicationMortgageLoanNewPurchase appNewPurchase = (ApplicationMortgageLoanNewPurchase)application;

                Assert.AreEqual(leadInput.PropertyValue, appNewPurchase.ClientEstimatePropertyValuation);

                IApplicationProductNewVariableLoan product = (IApplicationProductNewVariableLoan)appNewPurchase.CurrentProduct;

                ApplicationInformationVariableLoan appInfo = (ApplicationInformationVariableLoan)product.VariableLoanInformation;

                Assert.AreEqual(appInfo.CashDeposit, leadInput.Deposit);
                Assert.AreEqual(appInfo.EmploymentType.Key, leadInput.EmploymentTypeKey);
                Assert.AreEqual(appInfo.ExistingLoan, leadInput.CurrentLoan);

                Assert.AreEqual(appInfo.HouseholdIncome, leadInput.HouseholdIncome);
                Assert.AreEqual(appInfo.PropertyValuation, leadInput.PropertyValue);
                Assert.AreEqual(appInfo.Term, leadInput.Term);

                Assert.AreEqual(typeof(ApplicationInformationVariableLoan), product.VariableLoanInformation.GetType());
            }
        }

        [Test]
        public void GenerateApplicationFromWeb_NewPurchase_InterestOnly_Test()
        {
            ILeadInputInformation leadInput = new LeadInputInformation
            {
                EmploymentTypeKey = (int)EmploymentTypes.Salaried,
                ProductKey = (int)Products.NewVariableLoan,
                OfferSourceKey = (int)OfferSources.InternetApplication,
                PropertyValue = 1500000,
                MortgageLoanPurposeKey = (int)MortgageLoanPurposes.Newpurchase,
                Deposit = 500000,
                HouseholdIncome = 50000,
                Term = 240,
                ////N = PropertyValue (purchase price) - deposit
                //double LoanAmountRequired { get; } //R
                FixPercent = 0,
                InterestOnly = true,

                //  LegalEntity
                FirstNames = "Some",
                Surname = "Dude",
                //EmailAddress = "sd@ano.com",
                HomePhoneCode = "031",
                HomePhoneNumber = "5555555",
                NumberOfApplicants = 1,
                //Referer
                AdvertisingCampaignID = "AdvertisingCampaignID",
                ReferringServerURL = "ReferringServerURL",
                UserURL = "UserURL",
                UserAddress = "UserAddress"
            };

            using (new TransactionScope(OnDispose.Rollback))
            {
                int offerKey = applicationRepository.CreateWebLead(leadInput);
                bool success = applicationRepository.GenerateApplicationFromWeb(offerKey, leadInput);

                Assert.True(success, "GenerateApplicationFromWeb() Failed.");

                IApplication application = applicationRepository.GetApplicationByKey(offerKey);

                Type expected = typeof(ApplicationMortgageLoanNewPurchase);
                Assert.AreEqual(expected, application.GetType());

                ApplicationMortgageLoanNewPurchase appNewPurchase = (ApplicationMortgageLoanNewPurchase)application;

                Assert.AreEqual(leadInput.PropertyValue, appNewPurchase.ClientEstimatePropertyValuation);

                IApplicationProductNewVariableLoan product = (IApplicationProductNewVariableLoan)appNewPurchase.CurrentProduct;

                Assert.AreEqual(typeof(ApplicationInformationVariableLoan), product.VariableLoanInformation.GetType());

                ApplicationInformationVariableLoan appInfo = (ApplicationInformationVariableLoan)product.VariableLoanInformation;

                Assert.AreEqual(appInfo.CashDeposit, leadInput.Deposit);
                Assert.AreEqual(appInfo.EmploymentType.Key, leadInput.EmploymentTypeKey);
                Assert.AreEqual(appInfo.ExistingLoan, leadInput.CurrentLoan);

                Assert.AreEqual(appInfo.HouseholdIncome, leadInput.HouseholdIncome);
                Assert.AreEqual(appInfo.PropertyValuation, leadInput.PropertyValue);
                Assert.AreEqual(appInfo.Term, leadInput.Term);

                IApplicationInformationInterestOnly applicationInterestOnly = (IApplicationInformationInterestOnly)product.InterestOnlyInformation;

                Assert.NotNull(applicationInterestOnly);

                Assert.Greater(applicationInterestOnly.ApplicationInformation.ApplicationInformationFinancialAdjustments.Count, 0);
            }
        }

        [Test]
        public void GetApplicationDeclarationAnswerToQuestionTest()
        {
            // MortgageLoan
            using (new SessionScope(FlushAction.Never))
            {
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                // Active Main Applicant, New Purchase Loan, Open
                string query = @"select top 1 orl.LegalEntityKey, o.OfferKey, od.OfferDeclarationQuestionKey, od.OfferDeclarationAnswerKey
                            from dbo.Offer o (nolock)
                            join dbo.OfferRole orl (nolock) on orl.OfferKey = o.OfferKey
                                and orl.GeneralStatusKey = 1
	                            and orl.OfferRoleTypeKey = 11
                            join dbo.OfferDeclaration od (nolock) on od.OfferRoleKey = orl.OfferRoleKey
                            where o.OfferTypeKey = 7 ";

                DataTable DT = base.GetQueryResults(query);

                //no data exists
                if (DT.Rows.Count == 0)
                    Assert.Ignore();

                int legalEntityKey = Convert.ToInt32(DT.Rows[0][0]),
                    offerKey = Convert.ToInt32(DT.Rows[0][1]),
                    offerDeclarationQuestionKey = Convert.ToInt32(DT.Rows[0][2]),
                    offerDeclarationAnswerKey = Convert.ToInt32(DT.Rows[0][3]);

                int result = appRepo.GetApplicationDeclarationAnswerToQuestion(legalEntityKey, offerKey, offerDeclarationQuestionKey);
                Assert.AreEqual(result.ToString(), offerDeclarationAnswerKey.ToString());
            }

            // UnsecuredLending
            using (new SessionScope(FlushAction.Never))
            {
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                // Active Client, Unsecured Lending, Open
                string query = @"select top 1 erl.LegalEntityKey, o.OfferKey, erd.OfferDeclarationQuestionKey, erd.OfferDeclarationAnswerKey
                            from dbo.Offer o (nolock)
                            join dbo.ExternalRole erl (nolock) on erl.GenericKey = o.OfferKey
	                            and erl.GenericKeyTypeKey = 2
	                            and erl.GeneralStatusKey = 1
	                            and erl.ExternalRoleTypeKey = 1
                            join dbo.ExternalRoleDeclaration erd (nolock) on erd.ExternalRoleKey = erl.ExternalRoleKey
                            where o.OfferTypeKey = 11 ";

                DataTable DT = base.GetQueryResults(query);

                //no data exists
                if (DT.Rows.Count == 0)
                    Assert.Ignore();

                int legalEntityKey = Convert.ToInt32(DT.Rows[0][0]),
                    offerKey = Convert.ToInt32(DT.Rows[0][1]),
                    offerDeclarationQuestionKey = Convert.ToInt32(DT.Rows[0][2]),
                    offerDeclarationAnswerKey = Convert.ToInt32(DT.Rows[0][3]);

                int result = appRepo.GetApplicationDeclarationAnswerToQuestion(legalEntityKey, offerKey, offerDeclarationQuestionKey);
                Assert.AreEqual(result.ToString(), offerDeclarationAnswerKey.ToString());
            }
        }

        #region IsApplicationInOrder_Pass

        [Test]
        public void IsApplicationInOrder_Pass()
        {
            //Rule Service gets loaded as a singleton, so setting enabled true here
            // ensures the rules we are integration testing do get executed
            IRuleService ruleService = ServiceFactory.GetService<IRuleService>();
            ruleService.Enabled = true;
            //set ignore warnings to true
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.IgnoreWarnings = true;

            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            try
            {
                //Find one of each application type in Credit, if it is in Credit these rules should all have passed previously
                foreach (DataRow dr in GetCreditOffers())
                {
                    using (new SessionScope(FlushAction.Never))
                    {
                        Assert.IsTrue(appRepo.IsApplicationInOrder((int)dr["OfferKey"]), String.Format(@"OfferKey {0} of OfferTypeKey {1} failed", dr["OfferKey"], dr["OfferTypeKey"]));
                    }
                }
            }
            finally
            {
                //RESET IgnoreWarnings BACK TO false as this will AFFECT the RULES
                spc.IgnoreWarnings = false;
                ruleService.Enabled = false;
            }
        }

        private const string GetCreditOffersSQL = @"select max(o.offerkey) as OfferKey, o.OfferTypeKey --, *
                                            from
                                            x2.x2data.credit c (nolock)
                                            join x2.x2.worklist w (nolock) on c.InstanceID = w.InstanceID
                                            join x2.x2.Instance i (nolock) on c.InstanceID = i.ID
                                            join x2.x2.State s (nolock) on i.StateID = s.ID
                                            join [2am].dbo.offer o (nolock) on c.ApplicationKey = o.OfferKey
                                            join [2am].dbo.offermortgageloan oml (nolock) on oml.OfferKey = o.OfferKey
                                            join [2am].dbo.Property p (nolock) on oml.PropertyKey = p.PropertyKey
	                                            and len(p.ErfSuburbDescription) > 0
                                            join [2AM].dbo.PropertyAccessDetails pad (nolock) on pad.PropertyKey = p.PropertyKey
                                            left join [2am].dbo.StageTransition stOut on o.offerkey = stOut.GenericKey
	                                            and stOut.StageDefinitionStageDefinitionGroupKey in (2174,2149,2057,1507)
                                            left join [2am].dbo.StageTransition stIn on o.offerkey = stIn.GenericKey
	                                            and stIn.StageDefinitionStageDefinitionGroupKey in (1889, 1891, 1895)
                                            where offerstatuskey = 1
                                            and s.Name != 'Valuation Approval Required'
                                            and o.offerkey not in (select offerkey from [2am].dbo.OfferDocument where DocumentReceivedDate is null)
                                            and o.OfferKey not in ( select st.GenericKey
						                                            from [2am].dbo.StageTransition st
						                                            join [2am].dbo.StageDefinitionStageDefinitionGroup stsdg on st.StageDefinitionStageDefinitionGroupKey = stsdg.StageDefinitionStageDefinitionGroupKey
						                                            join [2am].dbo.StageDefinition sd on stsdg.StageDefinitionKey = sd.StageDefinitionKey
						                                            where sd.Name = 'Override Check')
                                            group by offertypekey
                                            having count(stout.StageDefinitionStageDefinitionGroupKey) >= count(stin.StageDefinitionStageDefinitionGroupKey)
                                            order by offertypekey";

        private DataRowCollection GetCreditOffers()
        {
            using (new SessionScope())
            {
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(GetCreditOffersSQL, typeof(GeneralStatus_DAO), null);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0].Rows; //.AsEnumerable().ToDictionary(x => x.Field<int>("OfferKey"), x => x.Field<int>("OfferTypeKey"));
            }

            return null;
        }

        #endregion IsApplicationInOrder_Pass

        #region IsApplicationInOrder_Fail

        [Test]
        public void IsApplicationInOrder_Validate_Fail()
        {
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            string sql = @"select max(o.OfferKey) OfferKey, o.OfferTypeKey
                        from [2am].dbo.offer o
                        join [2am].dbo.StageTransitionComposite st on o.offerKey = st.GenericKey
	                        and st.StageDefinitionStageDefinitionGroupKey in (110,111)
                        where o.OfferStatusKey in (4,5)
                        and offerTypeKey in (2,3,4,6,7,8)
                        group by offertypekey
                        order by OfferTypeKey";
            DataRowCollection drc = null;
            using (new SessionScope())
            {
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    drc = ds.Tables[0].Rows;
                }
            }
            if (drc == null)
            {
                Assert.Ignore("no test data for IsApplicationInOrder_Validate_Fail");
                return;
            }
            //Rule Service gets loaded as a singleton, so setting enabled true here
            // ensures the rules we are integration testing do get executed
            IRuleService ruleService = ServiceFactory.GetService<IRuleService>();
            ruleService.Enabled = true;
            try
            {
                using (new SessionScope(FlushAction.Never))
                {
                    foreach (DataRow dr in drc)
                    {
                        int offerKey = (int)dr["OfferKey"];
                        int offerTypeKey = (int)dr["OfferTypeKey"];
                        Assert.IsFalse(appRepo.IsApplicationInOrder(offerKey),
                            String.Format(@"OfferKey {0} of OfferTypeKey {1} passed, but was expected to fail on Validate", offerKey, offerTypeKey));
                    }
                }
            }
            finally
            {
                ruleService.Enabled = false;
            }
        }

        [Test]
        public void IsApplicationInOrder_MortgageLoan_Fail()
        {
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            string sql = @"select max(o.OfferKey) OfferKey, o.OfferTypeKey --, *
                from [2am].dbo.offer o (NOLOCK)
                join [x2].x2Data.Application_Management ap (NOLOCK) on ap.ApplicationKey = o.OfferKey
                where o.OfferStatusKey = 1
                group by o.offertypekey
                order by o.OfferTypeKey";

            DataRowCollection drc = null;

            using (new SessionScope())
            {
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    drc = ds.Tables[0].Rows;
                }
            }

            if (drc == null)
            {
                Assert.Ignore("no test data for IsApplicationInOrder_Validate_Fail");
                return;
            }

            //Rule Service gets loaded as a singleton, so setting enabled true here
            // ensures the rules we are integration testing do get executed
            IRuleService ruleService = ServiceFactory.GetService<IRuleService>();
            ruleService.Enabled = true;
            try
            {
                using (new SessionScope(FlushAction.Never))
                {
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                    foreach (DataRow dr in drc)
                    {
                        Assert.IsFalse(appRepo.IsApplicationInOrder((int)dr["OfferKey"]), String.Format(@"OfferKey {0} of OfferTypeKey {1} was expected to fail but passed", dr["OfferKey"], dr["OfferTypeKey"]));

                        spc.DomainMessages.Clear();
                    }
                }
            }
            finally
            {
                ruleService.Enabled = false;
            }
        }

        [Test]
        public void IsApplicationInOrder_Non_MortgageLoan_Fail()
        {
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            string sql = @"select max(o.OfferKey) OfferKey, o.OfferTypeKey --, *
                from [2am].dbo.offer o (NOLOCK)
                where o.OfferStatusKey = 1
                and o.offertypekey not in (2, 3, 4, 6, 7, 8)
                group by o.offertypekey
                order by o.OfferTypeKey";

            DataRowCollection drc = null;

            using (new SessionScope())
            {
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    drc = ds.Tables[0].Rows;
                }
            }

            if (drc == null)
            {
                Assert.Ignore("no test data for IsApplicationInOrder_Validate_Fail");
                return;
            }

            //Rule Service gets loaded as a singleton, so setting enabled true here
            // ensures the rules we are integration testing do get executed
            IRuleService ruleService = ServiceFactory.GetService<IRuleService>();
            ruleService.Enabled = true;
            try
            {
                using (new SessionScope(FlushAction.Never))
                {
                    foreach (DataRow dr in drc)
                    {
                        Assert.IsFalse(appRepo.IsApplicationInOrder((int)dr["OfferKey"]), String.Format(@"OfferKey {0} of OfferTypeKey {1} was expected to fail but passed", dr["OfferKey"], dr["OfferTypeKey"]));
                    }
                }
            }
            finally
            {
                ruleService.Enabled = false;
            }
        }

        #endregion IsApplicationInOrder_Fail

        [Test]
        public void ActivatingPendingDomiciliumAddressPass()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                string query = @"SELECT top 1 o.OfferKey
                            FROM [2am].dbo.Offer o
                            JOIN [2am].dbo.[ExternalRole] er ON o.OfferKey = er.GenericKey and ExternalRoleTypeKey = 1
                            JOIN [2am].dbo.ExternalRoleDomicilium erd ON er.ExternalRoleKey = erd.ExternalRoleKey
                            JOIN [2am].dbo.LegalEntityDomicilium led ON erd.LegalEntityDomiciliumKey = led.LegalEntityDomiciliumKey
                            JOIN [2am].dbo.LegalEntityAddress lea ON led.legalEntityAddressKey = lea.legalEntityAddressKey
                            WHERE led.generalStatusKey = 3 and o.OfferTypeKey = 11";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore();

                int offerKey = Convert.ToInt32(DT.Rows[0][0]);

                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                appRepo.ActivatePendingDomiciliumAddress(offerKey);
                Assert.IsFalse(CurrentPrincipalCache.DomainMessages.HasErrorMessages);
            }
        }

        [Ignore("incomplete")]
        [Test]
        public void SaveApplicationWithReturningMainApplicantClientTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                //Get a valid offer for a returning client
                string query = @"
                    select top 1 o.OfferKey
                        from [2am].[dbo].[OfferRole] ofr (nolock)
	                    inner join [2am].[dbo].[OfferRoleType] ort (nolock)
                                                            on ort.[OfferRoleTypeKey] = ofr.[OfferRoleTypeKey]
													        and ort.[OfferRoleTypeGroupKey] = 3 -- Client

                        join [2am].dbo.Offer o (nolock)     on o.OfferKey = ofr.OfferKey
										                    and o.OfferTypeKey in (6, 7, 8) --Switch Loan, New Purchase Loan, Refinance Loan
										                    and o.OfferStatusKey = 1

                        inner join [2am].dbo.[Role] r (nolock)    on r.LegalEntityKey = ofr.LegalEntityKey
										                    and r.RoleTypeKey = 2 --Main Applicant
										                    and r.GeneralStatusKey = 1

                        join [2am].[dbo].Account a (nolock) on a.AccountKey = r.AccountKey
										                    and a.ParentAccountKey is null

                        join (select max([OfferInformationKey]) [OfferInformationKey], [OfferKey]
		                        from [2am].[dbo].[OfferInformation] (nolock)
		                        group by [OfferKey])latest_oi
										                    on latest_oi.OfferKey = o.OfferKey

                        join [2am].[dbo].[OfferInformation] oi (nolock)
                                                            on oi.OfferInformationKey = latest_oi.OfferInformationKey
										                    and oi.[OfferInformationTypeKey] <> 3

                        left join [2am].[dbo].[OfferRoleAttribute] ora (nolock)
                                                            on ora.OfferRoleKey = ofr.OfferRoleKey
										                    and ora.OfferRoleAttributeTypeKey = 2
                        where ofr.GeneralStatusKey = 1
                        and [OfferRoleAttributeKey] is null";

                DataTable DT = base.GetQueryResults(query);
                int offerKey = 0;
                if (DT.Rows.Count > 0)
                {
                    offerKey = Convert.ToInt32(DT.Rows[0][0]);
                }
                else
                {
                    Assert.Inconclusive("No Data to work with");
                    return;
                }
                DT.Dispose();

                IApplication application = applicationRepository.GetApplicationByKey(offerKey);
                applicationRepository.SaveApplication(application);

                bool attrFound = false;
                foreach (IApplicationRole applicationRole in application.ApplicationRoles.Where(x =>
                    x.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant ||
                    x.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant ||
                    x.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor ||
                    x.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor).ToList())
                {
                    if (applicationRole.HasAttribute(OfferRoleAttributeTypes.ReturningClient))
                    {
                        attrFound = true;
                        break;
                    }
                }

                Assert.AreEqual(attrFound, true);
            }
        }

        [Ignore("incomplete")]
        [Test]
        public void SaveApplicationWithoutReturningMainApplicantClientTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                //Get a valid offer for a non returning client
                string query = @"
                    select top 1 o.OfferKey
                        from [2am].[dbo].[OfferRole] ofr (nolock)
	                    inner join [2am].[dbo].[OfferRoleType] ort (nolock)
                                                            on ort.[OfferRoleTypeKey] = ofr.[OfferRoleTypeKey]
													        and ort.[OfferRoleTypeGroupKey] = 3 -- Client

                        join [2am].dbo.Offer o (nolock)     on o.OfferKey = ofr.OfferKey
										                    and o.OfferTypeKey in (6, 7, 8) --Switch Loan, New Purchase Loan, Refinance Loan
										                    and o.OfferStatusKey = 1

                        left join [2am].dbo.[Role] r (nolock)    on r.LegalEntityKey = ofr.LegalEntityKey
										                    and r.RoleTypeKey = 2 --Main Applicant
										                    and r.GeneralStatusKey = 1

                        join [2am].[dbo].Account a (nolock) on a.AccountKey = r.AccountKey
										                    and a.ParentAccountKey is null

                        join (select max([OfferInformationKey]) [OfferInformationKey], [OfferKey]
		                        from [2am].[dbo].[OfferInformation] (nolock)
		                        group by [OfferKey])latest_oi
										                    on latest_oi.OfferKey = o.OfferKey

                        join [2am].[dbo].[OfferInformation] oi (nolock)
                                                            on oi.OfferInformationKey = latest_oi.OfferInformationKey
										                    and oi.[OfferInformationTypeKey] <> 3

                        where ofr.GeneralStatusKey = 1
						and r.AccountRoleKey is null ";

                DataTable DT = base.GetQueryResults(query);
                int offerKey = 0;
                if (DT.Rows.Count > 0)
                {
                    offerKey = Convert.ToInt32(DT.Rows[0][0]);
                }
                else
                {
                    Assert.Inconclusive("No Data to work with");
                    return;
                }
                DT.Dispose();

                IApplication application = applicationRepository.GetApplicationByKey(offerKey);
                applicationRepository.SaveApplication(application);

                bool attrFound = false;
                foreach (IApplicationRole applicationRole in application.ApplicationRoles.Where(x =>
                    x.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant ||
                    x.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant ||
                    x.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor ||
                    x.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor
                    ).ToList())
                {
                    if (applicationRole.HasAttribute(OfferRoleAttributeTypes.ReturningClient))
                    {
                        attrFound = true;
                        break;
                    }
                }

                Assert.AreEqual(attrFound, false);
            }
        }

        [Ignore("incomplete")]
        [Test]
        public void CalculateApplicationDetailWithReturningMainApplicantInitiationFeeDiscountTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                //Get a valid offer for a returning client
                string query = @"";

                DataTable DT = base.GetQueryResults(query);
                int offerKey = 0;
                if (DT.Rows.Count > 0)
                {
                    offerKey = Convert.ToInt32(DT.Rows[0][0]);
                }
                else
                {
                    Assert.Inconclusive("No Data to work with");
                    return;
                }
                DT.Dispose();

                IApplicationMortgageLoan appML = applicationRepository.GetApplicationByKey(offerKey) as IApplicationMortgageLoan;

                appML.CalculateApplicationDetail(false, false);
            }
        }

        [Test]
        public void CreateCapitecApplicationNewPurchaseWithITCTest()
        {
            CreateCapitecApplicationTestHelper(OfferTypes.NewPurchaseLoan, OfferStatuses.Open, true, false, EmploymentTypes.Salaried, EmploymentTypes.Salaried, false, false, CapitecLegalEntityTestType.NewLegalEntity);
        }

        [Test]
        public void CreateCapitecApplicationSwitchWithITCTest()
        {
            CreateCapitecApplicationTestHelper(OfferTypes.SwitchLoan, OfferStatuses.Open, true, false, EmploymentTypes.Salaried, EmploymentTypes.Salaried, false, false, CapitecLegalEntityTestType.NewLegalEntity);
        }
        [Test]
        public void CreateCapitecApplicationRefinanceWithITCTest()
        {
            CreateCapitecApplicationTestHelper(OfferTypes.RefinanceLoan, OfferStatuses.Open, true, false, EmploymentTypes.Salaried, EmploymentTypes.Salaried, false, false, CapitecLegalEntityTestType.NewLegalEntity);
        }

        [Test]
        public void CreateCapitecApplicationNewPurchaseNoITCTest()
        {
            CreateCapitecApplicationTestHelper(OfferTypes.NewPurchaseLoan, OfferStatuses.Open, false, false, EmploymentTypes.Salaried, EmploymentTypes.Salaried, false, false, CapitecLegalEntityTestType.NewLegalEntity);
        }

        [Test]
        public void CreateCapitecApplicationSalariedEmployment()
        {
            CreateCapitecApplicationTestHelper(OfferTypes.NewPurchaseLoan, OfferStatuses.Open, false, false, EmploymentTypes.Salaried, EmploymentTypes.Salaried, false, false, CapitecLegalEntityTestType.NewLegalEntity);
        }

        [Test]
        public void CreateCapitecApplicationSelfEmployment()
        {
            CreateCapitecApplicationTestHelper(OfferTypes.NewPurchaseLoan, OfferStatuses.Open, false, false, EmploymentTypes.SelfEmployed, EmploymentTypes.SelfEmployed, false, false, CapitecLegalEntityTestType.NewLegalEntity);
        }

        [Test]
        public void CreateCapitecApplicationUnEmployed()
        {
            CreateCapitecApplicationTestHelper(OfferTypes.NewPurchaseLoan, OfferStatuses.Declined, false, false, EmploymentTypes.Salaried, EmploymentTypes.Unemployed, false, false, CapitecLegalEntityTestType.NewLegalEntity);
        }

        [Test]
        public void CreateCapitecApplicationOneSalariedOneUnEmployed()
        {
            CreateCapitecApplicationTestHelper(OfferTypes.NewPurchaseLoan, OfferStatuses.Declined, false, true, EmploymentTypes.Salaried, EmploymentTypes.Unemployed, false, false, CapitecLegalEntityTestType.NewLegalEntity, true);
        }

        [Test]
        public void CreateCapitecApplicationSwitchNoITCTest()
        {
            CreateCapitecApplicationTestHelper(OfferTypes.SwitchLoan, OfferStatuses.Open, false, false, EmploymentTypes.Salaried, EmploymentTypes.Salaried, false, false, CapitecLegalEntityTestType.NewLegalEntity);
        }

        [Test]
        public void CreateCapitecApplicationNewPurchaseWithITCMultipleApplicantsTest()
        {
            CreateCapitecApplicationTestHelper(OfferTypes.NewPurchaseLoan, OfferStatuses.Open, true, true, EmploymentTypes.Salaried, EmploymentTypes.Salaried, false, false, CapitecLegalEntityTestType.NewLegalEntity);
        }

        public void CreateCapitecApplicationNewPurchaseWithITCAndPricingForRiskTest()
        {
            CreateCapitecApplicationTestHelper(OfferTypes.NewPurchaseLoan, OfferStatuses.Open, true, false, EmploymentTypes.Salaried, EmploymentTypes.Salaried, true, false, CapitecLegalEntityTestType.NewLegalEntity);
        }

        [Test]
        public void CreateCapitecApplicationSwitchWithITCAndPricingForRiskTest()
        {
            CreateCapitecApplicationTestHelper(OfferTypes.SwitchLoan, OfferStatuses.Open, true, false, EmploymentTypes.Salaried, EmploymentTypes.Salaried, true, false, CapitecLegalEntityTestType.NewLegalEntity);
        }

        [Test]
        public void CreateCapitecApplicationNewPurchaseWithITCMultipleApplicantsAndPricingForRiskTest()
        {
            CreateCapitecApplicationTestHelper(OfferTypes.NewPurchaseLoan, OfferStatuses.Open, true, true, EmploymentTypes.Salaried, EmploymentTypes.Salaried, true, false, CapitecLegalEntityTestType.NewLegalEntity);
        }

        [Test]
        public void CreateCapitecApplicationNewPurchaseDeclineTest()
        {
            CreateCapitecApplicationTestHelper(OfferTypes.NewPurchaseLoan, OfferStatuses.Declined, true, false, EmploymentTypes.Salaried, EmploymentTypes.Salaried, false, false, CapitecLegalEntityTestType.NewLegalEntity);
        }

        [Test]
        public void CreateCapitecApplicationNewPurchaseMarriedCOPTest()
        {
            CreateCapitecApplicationTestHelper(OfferTypes.NewPurchaseLoan, OfferStatuses.Declined, true, false, EmploymentTypes.Salaried, EmploymentTypes.Salaried, false, true, CapitecLegalEntityTestType.NewLegalEntity);
        }

        [Test]
        public void CreateCapitecApplicationUseExistingLe()
        {
            CreateCapitecApplicationTestHelper(OfferTypes.NewPurchaseLoan, OfferStatuses.Declined, true, false, EmploymentTypes.Salaried, EmploymentTypes.Salaried, false, true, CapitecLegalEntityTestType.ExistingLegalEntity);
        }

        [Test]
        public void CreateCapitecApplicationUseExistingLeNoFirstNameOrSurname()
        {
            CreateCapitecApplicationTestHelper(OfferTypes.NewPurchaseLoan, OfferStatuses.Declined, true, false, EmploymentTypes.Salaried, EmploymentTypes.Salaried, false, true, CapitecLegalEntityTestType.ExistingLegalEntityNoFirstOrSurname, false);
        }

        [Test]
        public void CreateCapitecApplicationUseExistingLeNoFirstNameOrSurnameOnOpenAccount()
        {
            CreateCapitecApplicationTestHelper(OfferTypes.NewPurchaseLoan, OfferStatuses.Declined, true, false, EmploymentTypes.Salaried, EmploymentTypes.Salaried, false, true, CapitecLegalEntityTestType.ExistingLegalEntityNoFirstOrSurnameOpenAccount, false);
        }

        [Test]
        public void CreateCapitecApplicationUseExistingLeNoFirstNameOrSurnameOnClosednAccountNoOffer()
        {
            CreateCapitecApplicationTestHelper(OfferTypes.NewPurchaseLoan, OfferStatuses.Declined, true, false, EmploymentTypes.Salaried, EmploymentTypes.Salaried, false, true, CapitecLegalEntityTestType.ExistingLegalEntityNoFirstOrSurnameClosedAccountNoOffer, false);
        }

        private enum CapitecLegalEntityTestType
        {
            NewLegalEntity,
            ExistingLegalEntity,
            ExistingLegalEntityNoFirstOrSurname,
            ExistingLegalEntityNoFirstOrSurnameOpenAccount,
            ExistingLegalEntityNoFirstOrSurnameClosedAccountNoOffer
        }

        private void CreateCapitecApplicationTestHelper(SAHL.Common.Globals.OfferTypes offerTypeToTest, OfferStatuses offerStatus, bool withITC, bool testMutltipleApplicants, SAHL.Common.Globals.EmploymentTypes applicationEmploymentType, SAHL.Common.Globals.EmploymentTypes legalentityEmploymentType, bool pricingForRisk, bool marriedInCommunityOfProperty, CapitecLegalEntityTestType legalEntityTestType, bool oneUnemployedLegalEntityOnly = false)
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                var adUser = RepositoryFactory.GetRepository<IOrganisationStructureRepository>().GetAdUserForAdUserName(@"SAHL\WebLeads");

                // get an unused OfferKey
                int applicationKey = 0;
                string query = @"select max(OfferKey)+1 from [2AM].dbo.Offer (nolock)";
                DataTable DT = base.GetQueryResults(query);
                applicationKey = Convert.ToInt32(DT.Rows[0][0]);

                var applicationDate = DateTime.Now;
                NewPurchaseLoanDetails newPurchaseloanDetails = null;
                SwitchLoanDetails switchLoanDetails = null;

                if (offerTypeToTest == SAHL.Common.Globals.OfferTypes.NewPurchaseLoan)
                    newPurchaseloanDetails = new NewPurchaseLoanDetails((int)applicationEmploymentType, 25000, 500000, 50000, true, 240);
                else if (offerTypeToTest == SAHL.Common.Globals.OfferTypes.SwitchLoan)
                    switchLoanDetails = new SwitchLoanDetails((int)applicationEmploymentType, 25000, 500000, 50000, 10000, 100, true, 240);
                else if (offerTypeToTest == SAHL.Common.Globals.OfferTypes.RefinanceLoan)
                    switchLoanDetails = new SwitchLoanDetails((int)applicationEmploymentType, 25000, 500000, 50000, 0, 100, true, 240); // for a refinance, we set the current balance on a switch to 0

                ApplicantInformation applicantInformation1 = null, applicantInformation2 = null;
                string identityNo1, identityNo2;
                ILegalEntityNaturalPerson existingLegalEntity1, existingLegalEntity2;

                switch (legalEntityTestType)
                {
                    case CapitecLegalEntityTestType.NewLegalEntity:
                        applicantInformation1 = new ApplicantInformation("8001015000319", "Fuzzy1", "Duck", (int)SAHL.Common.Globals.SalutationTypes.Mr, "0315551234", "0315713022", "0845552242", "fuzzy@ap.com", new DateTime(1980, 1, 1), "", true);
                        applicantInformation2 = new ApplicantInformation("7801011000319", "Grumpy1", "Duck", (int)SAHL.Common.Globals.SalutationTypes.Mrs, "0315551233", "0315713022", "0845552242", "grumpy@ap.com", new DateTime(1978, 1, 1), "", false);

                        break;

                    case CapitecLegalEntityTestType.ExistingLegalEntity:
                        query = @"select top 1 le.IDNumber from [2am].dbo.LegalEntity le
                            left join [2am].dbo.[Role] r on r.LegalEntityKey = le.LegalEntityKey
                            where LegalEntityTypeKey = 2 -- Natural Person
                            and r.LegalEntityKey is null
                            and (le.IDNumber is not null and le.IDNumber <> '')";
                        DT = base.GetQueryResults(query);
                        identityNo1 = Convert.ToString(DT.Rows[0][0]);
                        existingLegalEntity1 = legalEntityRepository.GetNaturalPersonByIDNumber(identityNo1);
                        applicantInformation1 = new ApplicantInformation(identityNo1, existingLegalEntity1.FirstNames, existingLegalEntity1.Surname, existingLegalEntity1.Salutation.Key, "0315551234", "0315713022", "0845552242", "fuzzy@ap.com", existingLegalEntity1.DateOfBirth, existingLegalEntity1.Salutation.Description, true);

                        query = @"select top 1 le.IDNumber
                            from [2am].dbo.LegalEntity le
                            left join [2am].dbo.[Role] r on r.LegalEntityKey = le.LegalEntityKey
                            where LegalEntityTypeKey = 2 -- Natural Person
                            and r.LegalEntityKey is null
                            and (le.IDNumber is not null and le.IDNumber <> '')
                            and le.IDNumber <> '" + identityNo1 + "'";
                        DT = base.GetQueryResults(query);
                        identityNo2 = Convert.ToString(DT.Rows[0][0]);
                        existingLegalEntity2 = legalEntityRepository.GetNaturalPersonByIDNumber(identityNo2);
                        applicantInformation2 = new ApplicantInformation(identityNo2, existingLegalEntity2.FirstNames, existingLegalEntity2.Surname, existingLegalEntity2.Salutation.Key, "0315551233", "0315713022", "0845552242", "grumpy@ap.com", existingLegalEntity2.DateOfBirth, existingLegalEntity2.Salutation.Description, false);

                        break;

                    case CapitecLegalEntityTestType.ExistingLegalEntityNoFirstOrSurname:
                        query = @"select top 1 le.IDNumber from [2am].dbo.LegalEntity le
                            where LegalEntityTypeKey = 2 -- Natural Person
                            and (le.IDNumber is not null and le.IDNumber <> '')
                            and (le.FirstNames is null and le.Surname is null)";
                        DT = base.GetQueryResults(query);
                        identityNo1 = Convert.ToString(DT.Rows[0][0]);
                        existingLegalEntity1 = legalEntityRepository.GetNaturalPersonByIDNumber(identityNo1);
                        applicantInformation1 = new ApplicantInformation(identityNo1, "Freddy", "Kruger", 1, "0315551234", "0315713022", "0845552242", "fuzzy@ap.com", existingLegalEntity1.DateOfBirth, "Mr", true);

                        query = @"select top 1 le.IDNumber from [2am].dbo.LegalEntity le
                            where LegalEntityTypeKey = 2 -- Natural Person
                            and (le.IDNumber is not null and le.IDNumber <> '')
                            and (le.FirstNames is null and le.Surname is null)
                            and le.IDNumber <> '" + identityNo1 + "' and len(le.IDNumber) = 13";
                        DT = base.GetQueryResults(query);
                        identityNo2 = Convert.ToString(DT.Rows[0][0]);
                        existingLegalEntity2 = legalEntityRepository.GetNaturalPersonByIDNumber(identityNo2);
                        applicantInformation2 = new ApplicantInformation(identityNo2, "Kerry", "Kruger", 2, "0315551234", "0315713022", "0845552242", "fuzzy@ap.com", existingLegalEntity2.DateOfBirth, "Mrs", true);

                        break;

                    case CapitecLegalEntityTestType.ExistingLegalEntityNoFirstOrSurnameOpenAccount:
                        query = @"select top 1 le.IDNumber from [2am].dbo.LegalEntity le
                            join [2am]..Role r on r.LegalEntityKey = le.LegalEntityKey
                            join [2am]..Account a on a.AccountKey = r.AccountKey
                            where LegalEntityTypeKey = 2 -- Natural Person
                            and (le.IDNumber is not null and le.IDNumber <> '')
                            and (le.FirstNames is null or le.Surname is null)
                            and a.AccountStatusKey <> 2 -- closed
                            and len(le.IDNumber) = 13";
                        DT = base.GetQueryResults(query);
                        identityNo1 = Convert.ToString(DT.Rows[0][0]);
                        existingLegalEntity1 = legalEntityRepository.GetNaturalPersonByIDNumber(identityNo1);
                        applicantInformation1 = new ApplicantInformation(identityNo1, "Freddy", "Kruger", 1, "0315551234", "0315713022", "0845552242", "fuzzy@ap.com", existingLegalEntity1.DateOfBirth, "Mr", true);

                        query = @"select top 1 le.IDNumber from [2am].dbo.LegalEntity le
                            join [2am]..Role r on r.LegalEntityKey = le.LegalEntityKey
                            join [2am]..Account a on a.AccountKey = r.AccountKey
                            where LegalEntityTypeKey = 2 -- Natural Person
                            and (le.IDNumber is not null and le.IDNumber <> '')
                            and (le.FirstNames is null or le.Surname is null)
                            and a.AccountStatusKey <> 2 -- closed
                            and le.IDNumber <> '" + identityNo1 + "' and len(le.IDNumber) = 13";

                        DT = base.GetQueryResults(query);
                        identityNo2 = Convert.ToString(DT.Rows[0][0]);
                        existingLegalEntity2 = legalEntityRepository.GetNaturalPersonByIDNumber(identityNo2);
                        applicantInformation2 = new ApplicantInformation(identityNo2, "Kerry", "Kruger", 2, "0315551234", "0315713022", "0845552242", "fuzzy@ap.com", existingLegalEntity2.DateOfBirth, "Mrs", true);

                        break;

                    case CapitecLegalEntityTestType.ExistingLegalEntityNoFirstOrSurnameClosedAccountNoOffer:
                        query = @"select top 1 le.IDNumber
                                from [2am].dbo.LegalEntity le
                                join [2am]..[Role] r on r.LegalEntityKey = le.LegalEntityKey
                                join [2am]..Account a on a.AccountKey = r.AccountKey
	                                and a.AccountStatusKey = 2 -- closed
                                left join [2am]..[offerrole] ofr on ofr.LegalEntityKey = le.LegalEntityKey
                                where LegalEntityTypeKey = 2 -- Natural Person
                                and (le.IDNumber is not null and len(le.IDNumber) = 13)
                                and (le.FirstNames is null or le.Surname is null)
                                and ofr.LegalEntityKey is null";
                        DT = base.GetQueryResults(query);
                        identityNo1 = Convert.ToString(DT.Rows[0][0]);
                        existingLegalEntity1 = legalEntityRepository.GetNaturalPersonByIDNumber(identityNo1);
                        applicantInformation1 = new ApplicantInformation(identityNo1, "Freddy", "Kruger", 1, "0315551234", "0315713022", "0845552242", "fuzzy@ap.com", existingLegalEntity1.DateOfBirth, "Mr", true);

                        break;

                    default:
                        break;
                }

                int addressKey = 0;
                query = @"select top 1 a.AddressKey from [2am]..LegalEntityAddress lea join [2am]..Address a on a.AddressKey = lea.AddressKey where lea.AddressTypeKey = 1 and a.AddressFormatKey = 1";
                DT = base.GetQueryResults(query);
                addressKey = Convert.ToInt32(DT.Rows[0][0]);
                IAddressStreet sa = addressRepository.GetAddressByKey(addressKey) as IAddressStreet;
                var applicantResidentialAddress = new ApplicantResidentialAddress(sa.UnitNumber, sa.BuildingNumber, sa.BuildingName, sa.StreetNumber, sa.StreetName, sa.RRR_SuburbDescription, sa.RRR_ProvinceDescription, sa.RRR_CityDescription, sa.RRR_PostalCode, sa.Suburb.Key);

                var employmentDetails1 = default(EmploymentDetails);
                var employmentDetails2 = default(EmploymentDetails);
                var employmentRenumeration = default(RemunerationTypes);

                switch (legalentityEmploymentType)
                {
                    case EmploymentTypes.Salaried:
                        employmentDetails1 = new SalariedDetails(50000);
                        employmentDetails2 = new SalariedDetails(35000);
                        employmentRenumeration = RemunerationTypes.Salaried;
                        break;

                    case EmploymentTypes.SelfEmployed:
                        employmentDetails1 = new SelfEmployedDetails(50000);
                        employmentDetails2 = new SelfEmployedDetails(35000);
                        employmentRenumeration = RemunerationTypes.Drawings;
                        break;

                    case EmploymentTypes.Unemployed:
                        employmentDetails1 = null;
                        employmentDetails2 = null;
                        employmentRenumeration = RemunerationTypes.Unknown;
                        break;
                }

                var applicantEmploymentDetails1 = new ApplicantEmploymentDetails((int)legalentityEmploymentType, employmentDetails1);
                var applicantEmploymentDetails2 = new ApplicantEmploymentDetails((int)legalentityEmploymentType, employmentDetails2);

                if (oneUnemployedLegalEntityOnly && testMutltipleApplicants) //set
                {
                    employmentDetails1 = new SalariedDetails(50000);
                    applicantEmploymentDetails1 = new ApplicantEmploymentDetails((int)EmploymentTypes.Salaried, employmentDetails1);
                }

                var applicantDeclarations = new ApplicantDeclarations(true, true, true, marriedInCommunityOfProperty); // How are we gonna do this??

                string request = "itcrequest";
                string response = "itcrequest";
                if (withITC == false)
                    response = null;

                if (pricingForRisk) //withITC should be true
                {
                    response = Empirica_590;
                }

                ApplicantITC applicantITC1 = new ApplicantITC(DateTime.Now, request, response);
                ApplicantITC applicantITC2 = new ApplicantITC(DateTime.Now.AddHours(-1), request, response);

                var applicant1 = new Applicant(applicantInformation1, applicantResidentialAddress, applicantEmploymentDetails1, applicantDeclarations, applicantITC1);
                var applicant2 = new Applicant(applicantInformation2, applicantResidentialAddress, applicantEmploymentDetails2, applicantDeclarations, applicantITC2);

                var applicants = new List<Applicant>() { applicant1 };
                if (testMutltipleApplicants)
                    applicants.Add(applicant2);

                var consultantDetails = new ConsultantDetails("Joe Consultant", "Diepsloot");

                NewPurchaseApplication newPurchaseApplication = null;
                SwitchLoanApplication switchLoanApplication = null;

                var messages = new List<string>();
                messages.Add("Test Capitec Decline Message");
                messages.Add("Test Duplicate Capitec Decline Message");
                messages.Add("Test Duplicate Capitec Decline Message");
                messages.Add("Test another Capitec Decline Message");
                messages.Add("Test and anotherCapitec Decline Message");

                if (offerTypeToTest == SAHL.Common.Globals.OfferTypes.NewPurchaseLoan)
                {
                    newPurchaseApplication = new NewPurchaseApplication(applicationKey, (int)offerStatus, applicationDate, newPurchaseloanDetails, applicants, (int)applicationEmploymentType, consultantDetails, messages);
                    applicationRepository.CreateCapitecApplication(newPurchaseApplication);
                }
                else if (offerTypeToTest == SAHL.Common.Globals.OfferTypes.SwitchLoan ||
                    (offerTypeToTest == SAHL.Common.Globals.OfferTypes.RefinanceLoan))
                {
                    switchLoanApplication = new SwitchLoanApplication(applicationKey, (int)offerStatus, applicationDate, switchLoanDetails, applicants, (int)applicationEmploymentType, consultantDetails, messages);
                    applicationRepository.CreateCapitecApplication(switchLoanApplication);
                }

                // lets get he newly created offer
                IApplication capitecApplication = applicationRepository.GetApplicationByKey(applicationKey);

                // check the offer was created
                Assert.IsTrue(capitecApplication != null && capitecApplication.Key == applicationKey);

                // check the correct offer type has been created
                Assert.IsTrue(capitecApplication.ApplicationType.Key == (int)offerTypeToTest);

                // check correct offer source has been set
                Assert.IsTrue(capitecApplication.ApplicationSource != null && capitecApplication.ApplicationSource.Key == (int)SAHL.Common.Globals.OfferSources.Capitec);

                // check the application status is correct
                Assert.IsTrue(capitecApplication.ApplicationStatus.Key == (int)offerStatus);

                // Test for Decline Reasons
                var msgCount = RepositoryFactory.GetRepository<IReasonRepository>().GetReasonByGenericKeyAndReasonTypeKey(capitecApplication.GetLatestApplicationInformation().Key, (int)ReasonTypes.CapitecBranchDecline).Count();
                Assert.IsTrue(msgCount == messages.Distinct().Count());

                // check correct capitec loan attribute has been added
                Assert.IsTrue(capitecApplication.ApplicationAttributes != null && capitecApplication.ApplicationAttributes.Count > 0);
                var capitecLoanAttribute = capitecApplication.ApplicationAttributes.Where(x => x.ApplicationAttributeType.Key == (int)SAHL.Common.Globals.OfferAttributeTypes.CapitecLoan).FirstOrDefault();
                Assert.IsTrue(capitecLoanAttribute != null);

                // check capitec consultant details
                IApplicationCapitecDetail applicationCapitecDetail = applicationRepository.GetApplicationCapitecDetail(applicationKey);
                Assert.IsTrue(applicationCapitecDetail != null && applicationCapitecDetail.Consultant == consultantDetails.Name && applicationCapitecDetail.Branch == consultantDetails.Branch);

                // check applicants
                var mainApplicantRoles = capitecApplication.ApplicationRoles.Where(appRole => appRole.ApplicationRoleType.Key == (int)SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant).ToList();
                Assert.IsTrue(mainApplicantRoles != null);
                Assert.IsTrue(mainApplicantRoles.Count == applicants.Count);

                int i = 0;
                foreach (var mainApplicantRole in mainApplicantRoles)
                {
                    // check the offer role attribute
                    if (i == 0) // main contact
                    {
                        var mainContactOfferRoleAttributeTYpe = mainApplicantRole.ApplicationRoleAttributes.Where(x => x.OfferRoleAttributeType.Key == (int)SAHL.Common.Globals.OfferRoleAttributeTypes.MainContact).LastOrDefault();
                        Assert.IsTrue(mainContactOfferRoleAttributeTYpe != null);
                    }
                    else // income contributer
                    {
                        var incomeContributerOfferRoleAttributeTYpe = mainApplicantRole.ApplicationRoleAttributes.Where(x => x.OfferRoleAttributeType.Key == (int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor).LastOrDefault();
                        Assert.IsTrue(incomeContributerOfferRoleAttributeTYpe != null);
                    }

                    var mainApplicant = mainApplicantRole.LegalEntity as ILegalEntityNaturalPerson;

                    // check legal entity
                    Assert.IsTrue(mainApplicant.LegalEntityStatus == LookupRepository.LegalEntityStatuses.ObjectDictionary[Convert.ToString((int)LegalEntityStatuses.Alive)]);

                    Assert.IsTrue(mainApplicant.IDNumber == applicants[i].Information.IdentityNumber);

                    if (legalEntityTestType != CapitecLegalEntityTestType.ExistingLegalEntityNoFirstOrSurnameOpenAccount)
                    {
                        Assert.IsTrue(mainApplicant.Salutation.Key == applicants[i].Information.SalutationTypeKey);
                        Assert.IsTrue(mainApplicant.FirstNames == applicants[i].Information.FirstName);
                        Assert.IsTrue(mainApplicant.Surname == applicants[i].Information.Surname);

                        Assert.IsTrue(mainApplicant.WorkPhoneCode == applicants[i].Information.WorkPhoneNumber.Substring(0, 3));
                        Assert.IsTrue(mainApplicant.WorkPhoneNumber == applicants[i].Information.WorkPhoneNumber.Substring(3));
                        Assert.IsTrue(mainApplicant.HomePhoneCode == applicants[i].Information.HomePhoneNumber.Substring(0, 3));
                        Assert.IsTrue(mainApplicant.HomePhoneNumber == applicants[i].Information.HomePhoneNumber.Substring(3));
                        Assert.IsTrue(mainApplicant.EmailAddress == applicants[i].Information.EmailAddress);
                        Assert.IsTrue(mainApplicant.CellPhoneNumber == applicants[i].Information.CellPhoneNumber);

                        if (legalEntityTestType == CapitecLegalEntityTestType.NewLegalEntity)
                            Assert.IsTrue(mainApplicant.DocumentLanguage == commonRepository.GetLanguageByKey((int)Languages.English));
                    }

                    // check address
                    if (legalEntityTestType == CapitecLegalEntityTestType.NewLegalEntity)
                    {
                        Assert.IsTrue(mainApplicant.LegalEntityAddresses != null && mainApplicant.LegalEntityAddresses.Count == 1);
                        var legalEntityAddress = mainApplicant.LegalEntityAddresses[0];
                        Assert.IsTrue(legalEntityAddress.AddressType.Key == (int)SAHL.Common.Globals.AddressTypes.Residential);
                        Assert.IsTrue(legalEntityAddress.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active);
                        var streetAddress = legalEntityAddress.Address as IAddressStreet;
                        Assert.IsTrue(streetAddress != null);
                        Assert.IsTrue(streetAddress.BuildingName == applicantResidentialAddress.BuildingName);
                        Assert.IsTrue(streetAddress.BuildingNumber == applicantResidentialAddress.BuildingNumber);
                        Assert.IsTrue(streetAddress.StreetName == applicantResidentialAddress.StreetName);
                        Assert.IsTrue(streetAddress.StreetNumber == applicantResidentialAddress.StreetNumber);
                        Assert.IsTrue(streetAddress.Suburb.Description == applicantResidentialAddress.Suburb);
                        Assert.IsTrue(streetAddress.UnitNumber == applicantResidentialAddress.UnitNumber);
                    }

                    // check latest employment
                    Assert.IsTrue(mainApplicant.Employment != null && mainApplicant.Employment.Count > 0);
                    IEmployment latestEmployment = mainApplicant.Employment.Where(x => x.Key > 0).LastOrDefault();
                    Assert.IsTrue(latestEmployment.EmploymentStatus.Key == (int)SAHL.Common.Globals.EmploymentStatuses.Current);
                    if (i == 0 && oneUnemployedLegalEntityOnly && testMutltipleApplicants) // main contact
                    {
                        Assert.IsTrue(latestEmployment.RemunerationType.Key == (int)RemunerationTypes.Salaried);
                    }
                    else
                    {
                        Assert.IsTrue(latestEmployment.RemunerationType.Key == (int)employmentRenumeration);
                    }

                    if (applicants[i].EmploymentDetails.EmploymentTypeKey == (int)EmploymentTypes.Unemployed)
                    {
                        Assert.IsTrue(latestEmployment.BasicIncome.Value == 0D);
                    }
                    else
                    {
                        Assert.IsTrue(latestEmployment.BasicIncome.Value == (double)applicants[i].EmploymentDetails.Employment.BasicMonthlyIncome);
                    }

                    // check latest itc
                    if (withITC)
                    {
                        IList<IITC> itcs = itcRepository.GetITCByLEAndAccountKey(mainApplicant.Key, capitecApplication.ReservedAccount.Key);

                        Assert.IsTrue(itcs != null && itcs.Count > 0);
                        IITC latestITC = itcs.Where(x => x.Key > 0).LastOrDefault();
                        Assert.IsTrue(latestITC.ChangeDate == applicants[i].ITC.ITCDate);
                        Assert.IsTrue(latestITC.RequestXML == applicants[i].ITC.Request.ToString());
                        Assert.IsTrue(latestITC.ResponseXML == applicants[i].ITC.Response.ToString());
                        Assert.IsTrue(latestITC.ReservedAccount.Key == capitecApplication.ReservedAccount.Key);
                        Assert.IsTrue(latestITC.ResponseStatus == "Success");
                        Assert.IsTrue(latestITC.UserID == adUser.ADUserName);
                    }

                    i++;
                }

                if (pricingForRisk)
                {
                    var appML = capitecApplication as IApplicationMortgageLoan;
                    Assert.Greater(appML.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments.Where(x => x.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.RateAdjustment).Count(), 0);
                }
            }
        }

        private static string Empirica_590 = "<BureauResponse xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <EmpiricaEM07 xmlns=\"https://secure.transunion.co.za/TUBureau\">    <EmpiricaScore>00590</EmpiricaScore>  </EmpiricaEM07>  </BureauResponse>";

        [Test]
        public void GetApplicationCapitecDetailTest()
        {
            string sql = @"select top 1 o.offerkey from [2am].dbo.OfferCapitecDetail o  (nolock)";

            object o = DBHelper.ExecuteScalar(sql);
            if (null == o)
            {
                Assert.Ignore("No OfferCapitecDetail data available.");
            }
            int offerKey = (int)o;
            using (new SessionScope(FlushAction.Never))
            {
                IApplicationCapitecDetail applicationCapitecDetail = applicationRepository.GetApplicationCapitecDetail(offerKey);
                Assert.IsNotNull(applicationCapitecDetail);
                Assert.IsTrue(offerKey == applicationCapitecDetail.ApplicationKey);
            }
        }

        [Test]
        public void SaveApplicationCapitecDetailTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                string consultant = "Bob Jones";
                ApplicationCapitecDetail_DAO ac = ApplicationCapitecDetail_DAO.FindFirst();
                if (ac != null)
                {
                    IApplicationCapitecDetail applicationCapitecDetail = BMTM.GetMappedType<IApplicationCapitecDetail, ApplicationCapitecDetail_DAO>(ac);
                    applicationCapitecDetail.Consultant = consultant;
                    applicationRepository.SaveApplicationCapitecDetail(applicationCapitecDetail);
                    Assert.IsTrue(applicationCapitecDetail.Consultant == consultant);
                }
                else
                {
                    Assert.Ignore("No OfferCapitecDetail data available.");
                }
            }
        }

        [Test]
        public void CreateNewApplicationCapitecDetailTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                // find an offerkey
                Application_DAO ac = Application_DAO.FindFirst();
                int offerKey = ac.Key;
                string branch = "Test Branch";
                string consultant = "Bob Jones";

                IApplicationCapitecDetail applicationCapitecDetail = applicationRepository.CreateEmptyApplicationCapitecDetail();
                applicationCapitecDetail.ApplicationKey = offerKey;
                applicationCapitecDetail.Branch = branch;
                applicationCapitecDetail.Consultant = consultant;
                applicationRepository.SaveApplicationCapitecDetail(applicationCapitecDetail);
                Assert.IsTrue(applicationCapitecDetail.Key > 0);
                Assert.IsTrue(applicationCapitecDetail.ApplicationKey == offerKey);
                Assert.IsTrue(applicationCapitecDetail.Branch == branch);
                Assert.IsTrue(applicationCapitecDetail.Consultant == consultant);
            }
        }

        [Test]
        public void DeleteApplicationCapitecDetailTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                // find an offerkey
                Application_DAO ac = Application_DAO.FindFirst();
                int offerKey = ac.Key;
                string branch = "Test Branch";
                string consultant = "Bob Jones";

                // create a record
                IApplicationCapitecDetail applicationCapitecDetail = applicationRepository.CreateEmptyApplicationCapitecDetail();
                applicationCapitecDetail.ApplicationKey = offerKey;
                applicationCapitecDetail.Branch = branch;
                applicationCapitecDetail.Consultant = consultant;
                applicationRepository.SaveApplicationCapitecDetail(applicationCapitecDetail);

                // delete the record
                applicationRepository.DeleteApplicationCapitecDetail(offerKey);

                // try and get the deleted record
                IApplicationCapitecDetail acd = applicationRepository.GetApplicationCapitecDetail(offerKey);

                // shouldnt exist
                Assert.IsTrue(acd == null);
            }
        }

        [Test]
        public void CapitecCreateOrUpdateLegalEntityNoFirstNameOrSurname()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                // find a legal entity with an idnumber but no firstname and surname
                string query = @"select top 1 le.IDNumber from [2am].dbo.LegalEntity le
                            where LegalEntityTypeKey = 2 -- Natural Person
                            and (le.IDNumber is not null and le.IDNumber <> '')
                            and (le.FirstNames is null and le.Surname is null)
							and le.DateOfBirth is not null
							and le.Salutationkey is not null";
                DataTable DT = base.GetQueryResults(query);
                string identityNo = Convert.ToString(DT.Rows[0][0]);
                ILegalEntityNaturalPerson existingLegalEntity = legalEntityRepository.GetNaturalPersonByIDNumber(identityNo);
                ApplicantInformation applicantInformation = new ApplicantInformation(identityNo, existingLegalEntity.FirstNames, existingLegalEntity.Surname, existingLegalEntity.Salutation.Key, "0315551234", "0315713022", "0845552242", "fuzzy@ap.com", existingLegalEntity.DateOfBirth, existingLegalEntity.Salutation.Description, true);
            }
        }
    }
}