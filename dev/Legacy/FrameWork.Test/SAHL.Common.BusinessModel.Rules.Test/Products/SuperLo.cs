using System;
using System.Data;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Rules.Products;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.Test.Products
{
    [TestFixture]
    public class SuperLo : RuleBase
    {
        public interface IApplicationProductSupportsVariableLoanApplicationInformation : IApplicationProduct, ISupportsVariableLoanApplicationInformation
        {
        }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        #region ProductSuperLoMinimum

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        //[NUnit.Framework.Test]
        //public void ProductSuperLoLTVSuccess()
        //{
        //    SuperLoLTVRequirement rule = new SuperLoLTVRequirement();
        //    IDomainMessageCollection Messages = new DomainMessageCollection();
        //    IApplication app = _mockery.StrictMock<IApplication>();
        //    IApplicationProductSupportsVariableLoanApplicationInformation appProd = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
        //    ISupportsVariableLoanApplicationInformation vli = _mockery.StrictMock<ISupportsVariableLoanApplicationInformation>();
        //    IApplicationInformationVariableLoan aivl = _mockery.StrictMock<IApplicationInformationVariableLoan>();
        //    SetupResult.For(aivl.LTV).Return(84);
        //    SetupResult.For(appProd.VariableLoanInformation).Return(aivl);
        //    SetupResult.For(app.CurrentProduct).Return(appProd);
        //    ExecuteRule(rule, 0, app);
        //}

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        //[NUnit.Framework.Test]
        //public void ProductSuperLoLTVFail()
        //{
        //    SuperLoLTVRequirement rule = new SuperLoLTVRequirement();
        //    IDomainMessageCollection Messages = new DomainMessageCollection();
        //    IApplication app = _mockery.StrictMock<IApplication>();
        //    IApplicationProductSupportsVariableLoanApplicationInformation appProd = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
        //    ISupportsVariableLoanApplicationInformation vli = _mockery.StrictMock<ISupportsVariableLoanApplicationInformation>();
        //    IApplicationInformationVariableLoan aivl = _mockery.StrictMock<IApplicationInformationVariableLoan>();
        //    SetupResult.For(aivl.LTV).Return(95);
        //    SetupResult.For(appProd.VariableLoanInformation).Return(aivl);
        //    SetupResult.For(app.CurrentProduct).Return(appProd);
        //    ExecuteRule(rule, 1, app);
        //}

        [NUnit.Framework.Test]
        public void ProductSuperLoMinimumSuccess()
        {
            ProductSuperLoMinimum rule = new ProductSuperLoMinimum();

            // Setup the correct object to pass along
            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductSuperLoLoan applicationProductSuperLoLoan = _mockery.StrictMock<IApplicationProductSuperLoLoan>();
            SetupResult.For(app.CurrentProduct).Return(applicationProductSuperLoLoan);

            // Setup applicationProductSuperLoLoan.LoanAgreementAmount
            SetupResult.For(applicationProductSuperLoLoan.LoanAgreementAmount).Return(300000.0);

            ExecuteRule(rule, 0, app);
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductSuperLoMinimumFail()
        {
            ProductSuperLoMinimum rule = new ProductSuperLoMinimum();

            // Setup the correct object to pass along
            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductSuperLoLoan applicationProductSuperLoLoan = _mockery.StrictMock<IApplicationProductSuperLoLoan>();
            SetupResult.For(app.CurrentProduct).Return(applicationProductSuperLoLoan);

            // Setup applicationProductSuperLoLoan.LoanAgreementAmount
            SetupResult.For(applicationProductSuperLoLoan.LoanAgreementAmount).Return(200000.0);

            ExecuteRule(rule, 1, app);
        }

        #endregion ProductSuperLoMinimum

        #region ProductSuperLoNewCat1

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductSuperLoNewCat1Success()
        {
            ProductSuperLoNewCat1 rule = new ProductSuperLoNewCat1();

            // Setup the correct object to pass along
            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductSuperLoLoan applicationProductSuperLoLoan = _mockery.StrictMock<IApplicationProductSuperLoLoan>();
            SetupResult.For(app.CurrentProduct).Return(applicationProductSuperLoLoan);

            // Setup applicationProductSuperLoLoan.VariableLoanInformation
            IApplicationInformationVariableLoan applicationInformationVariableLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(applicationProductSuperLoLoan.VariableLoanInformation).Return(applicationInformationVariableLoan);

            // Setup applicationInformationVariableLoan.Category
            ICategory category = _mockery.StrictMock<ICategory>();
            SetupResult.For(applicationInformationVariableLoan.Category).Return(category);
            SetupResult.For(category.Key).Return(1);

            ExecuteRule(rule, 0, app);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductSuperLoNewCat1Fail()
        {
            ProductSuperLoNewCat1 rule = new ProductSuperLoNewCat1();

            // Setup the correct object to pass along
            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductSuperLoLoan applicationProductSuperLoLoan = _mockery.StrictMock<IApplicationProductSuperLoLoan>();
            SetupResult.For(app.CurrentProduct).Return(applicationProductSuperLoLoan);

            // Setup applicationProductSuperLoLoan.VariableLoanInformation
            IApplicationInformationVariableLoan applicationInformationVariableLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(applicationProductSuperLoLoan.VariableLoanInformation).Return(applicationInformationVariableLoan);

            // Setup applicationInformationVariableLoan.Category
            ICategory category = _mockery.StrictMock<ICategory>();
            SetupResult.For(applicationInformationVariableLoan.Category).Return(category);
            SetupResult.For(category.Key).Return(2);

            ExecuteRule(rule, 1, app);
        }

        #endregion ProductSuperLoNewCat1

        #region ProductSuperLoNewSPV

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductSuperLoNewSPVSuccess()
        {
            ProductSuperLoNewSPV rule = new ProductSuperLoNewSPV();

            // Setup the correct object to pass along
            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductSuperLoLoan applicationProductSuperLoLoan = _mockery.StrictMock<IApplicationProductSuperLoLoan>();
            SetupResult.For(app.CurrentProduct).Return(applicationProductSuperLoLoan);

            // Setup applicationProductSuperLoLoan.VariableLoanInformation
            IApplicationInformationVariableLoan applicationInformationVariableLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(applicationProductSuperLoLoan.VariableLoanInformation).Return(applicationInformationVariableLoan);

            // Setup applicationInformationVariableLoan.SPV
            ISPV spv = _mockery.StrictMock<ISPV>();
            SetupResult.For(applicationInformationVariableLoan.SPV).Return(spv);
            SetupResult.For(spv.Key).Return((int)SPVs.MainStreet65PtyLtd);

            ExecuteRule(rule, 0, app);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductSuperLoNewSPVFail()
        {
            ProductSuperLoNewSPV rule = new ProductSuperLoNewSPV();

            // Setup the correct object to pass along
            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductSuperLoLoan applicationProductSuperLoLoan = _mockery.StrictMock<IApplicationProductSuperLoLoan>();
            SetupResult.For(app.CurrentProduct).Return(applicationProductSuperLoLoan);

            // Setup applicationProductSuperLoLoan.VariableLoanInformation
            IApplicationInformationVariableLoan applicationInformationVariableLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(applicationProductSuperLoLoan.VariableLoanInformation).Return(applicationInformationVariableLoan);

            // Setup applicationInformationVariableLoan.SPV
            ISPV spv = _mockery.StrictMock<ISPV>();
            SetupResult.For(applicationInformationVariableLoan.SPV).Return(spv);
            SetupResult.For(spv.Key).Return(21);

            ExecuteRule(rule, 1, app);
        }

        #endregion ProductSuperLoNewSPV

        #region ProductSuperLoResetDate

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductSuperLoResetDateSuccess()
        {
            ProductSuperLoResetDate rule = new ProductSuperLoResetDate();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.FinancialServices
            IEventList<IFinancialService> financialServices = new EventList<IFinancialService>();
            IMortgageLoan financialService = _mockery.StrictMock<IMortgageLoan>();
            financialServices.Add(Messages, financialService);
            SetupResult.For(mortgageLoanAccount.FinancialServices).Return(financialServices);

            // Setup mortgageLoan.ResetConfiguration
            IResetConfiguration resetConfiguration = _mockery.StrictMock<IResetConfiguration>();
            SetupResult.For(financialService.ResetConfiguration).Return(resetConfiguration);
            SetupResult.For(resetConfiguration.Key).Return((int)ResetConfigurations.Eighteenth);

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductSuperLoResetDateFail()
        {
            ProductSuperLoResetDate rule = new ProductSuperLoResetDate();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.FinancialServices
            IEventList<IFinancialService> financialServices = new EventList<IFinancialService>();
            IMortgageLoan financialService = _mockery.StrictMock<IMortgageLoan>();
            financialServices.Add(Messages, financialService);
            SetupResult.For(mortgageLoanAccount.FinancialServices).Return(financialServices);

            // Setup mortgageLoan.ResetConfiguration
            IResetConfiguration resetConfiguration = _mockery.StrictMock<IResetConfiguration>();
            SetupResult.For(financialService.ResetConfiguration).Return(resetConfiguration);
            SetupResult.For(resetConfiguration.Key).Return((int)ResetConfigurations.TwentyFirst);

            ExecuteRule(rule, 1, mortgageLoanAccount);
        }

        #endregion ProductSuperLoResetDate

        #region ProductSuperLoOptInLoanTransaction

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductSuperLoOptInLoanTransactionSuccess()
        {
            ProductSuperLoOptInLoanTransaction rule = new ProductSuperLoOptInLoanTransaction(RepositoryFactory.GetRepository<ICastleTransactionsService>());

            // Setup the correct object to pass along
            IApplication app = _mockery.StrictMock<IApplication>();

            ParameterCollection parameters = new ParameterCollection();
            IDbConnection con = Helper.GetSQLDBConnection("EWorkConnectionString");
            DataTable DT = new DataTable();
            string query = "select st.sLoanNumber from [e-work]..SuperTracker st where st.bActiveFolder != -1 and st.sLoanNumber != 0";
            Helper.FillFromQuery(DT, query, con, parameters);

            int accountKey = Convert.ToInt32(DT.Rows[0][0]);

            IAccount acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(acc.Key).Return((int)accountKey);
            SetupResult.For(app.Account).Return(acc);

            ExecuteRule(rule, 0, app);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductSuperLoOptInLoanTransactionFail()
        {
            ProductSuperLoOptInLoanTransaction rule = new ProductSuperLoOptInLoanTransaction(RepositoryFactory.GetRepository<ICastleTransactionsService>());

            // Setup the correct object to pass along
            IApplication app = _mockery.StrictMock<IApplication>();

            ParameterCollection parameters = new ParameterCollection();
            IDbConnection con = Helper.GetSQLDBConnection("EWorkConnectionString");
            DataTable DT = new DataTable();
            string query = "select st.sLoanNumber from [e-work]..SuperTracker st where st.bActiveFolder = -1";
            Helper.FillFromQuery(DT, query, con, parameters);

            int accountKey = Convert.ToInt32(DT.Rows[0][0]);

            IAccount acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(acc.Key).Return((int)accountKey);
            SetupResult.For(app.Account).Return(acc);

            ExecuteRule(rule, 1, app);
        }

        #endregion ProductSuperLoOptInLoanTransaction

        #region ProductSuperLoFLSPVChange

        /// <summary>
        /// Success, testing if the application has not changed products
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductSuperLoFLSPVChangeSuccess()
        {
            ProductSuperLoFLSPVChange rule = new ProductSuperLoFLSPVChange(RepositoryFactory.GetRepository<ICastleTransactionsService>());

            IApplication app = _mockery.StrictMock<IApplication>();

            ParameterCollection parameters = new ParameterCollection();
            IDbConnection con = Helper.GetSQLDBConnection("DBConnectionString");
            DataTable DT = new DataTable();
            string query = @"select top 1 o.OfferKey
							from Offer o (nolock)
							join (
								select
									moi.offerkey,
									max(offerinformationkey) as offerinformationkey,
									max(ProductKey) as ProductKey
								from
									offerinformation moi (nolock)
								join Offer on
									moi.offerkey = offer.offerkey
								group by
									moi.offerkey)
								maxoi on
									o.offerkey = maxoi.offerkey join
								OfferInformationVariableLoan vl (nolock) on
									vl.OfferInformationKey = maxoi.OfferInformationKey
								join spv.SPV s (nolock) on 	vl.SPVKey = s.SPVKey
								and	o.OfferTypeKey in (3, 4)
								and	maxoi.ProductKey = 5
								and	s.SPVCompanyKey = 2

								join FinancialService fs on	fs.AccountKey = o.AccountKey
								JOIN Account act on act.AccountKey = fs.AccountKey
								and	fs.FinancialServiceTypeKey = 1
								and fs.AccountStatusKey = 1

								join spv.SPV spv on	act.SPVKey = spv.SPVKey
								and	spv.SPVCompanyKey = s.SPVCompanyKey";
            Helper.FillFromQuery(DT, query, con, parameters);

            int applicationKey = Convert.ToInt32(DT.Rows[0][0]);

            SetupResult.For(app.Key).Return(applicationKey);

            ExecuteRule(rule, 0, app);
        }

        /// <summary>
        /// Fail, testing if the application has not changed products
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductSuperLoFLSPVChangeFail()
        {
            ProductSuperLoFLSPVChange rule = new ProductSuperLoFLSPVChange(RepositoryFactory.GetRepository<ICastleTransactionsService>());

            IApplication app = _mockery.StrictMock<IApplication>();

            ParameterCollection parameters = new ParameterCollection();
            IDbConnection con = Helper.GetSQLDBConnection("DBConnectionString");
            DataTable DT = new DataTable();
            string query = @"select top 1 o.OfferKey
                            from [2am].[dbo].Offer o (nolock)
                            join (
	                            select
		                            moi.offerkey,
		                            max(offerinformationkey) as offerinformationkey,
		                            max(ProductKey) as ProductKey
	                            from
		                            [2am].[dbo].offerinformation moi (nolock)
	                            join  [2am].[dbo].Offer on
		                            moi.offerkey = offer.offerkey
	                            group by
		                            moi.offerkey
	                            )
	                            maxoi on
		                            o.offerkey = maxoi.offerkey
	                            join
		                            [2am].[dbo].OfferInformationVariableLoan vl (nolock) on vl.OfferInformationKey = maxoi.OfferInformationKey
	                            join [2am].spv.SPV s (nolock) on vl.SPVKey = s.SPVKey
							                                 and o.OfferTypeKey in (3, 4)
								                             and maxoi.ProductKey = 5
								                             and s.SPVCompanyKey = 2

	                            join [2am].[dbo].FinancialService fs on	fs.AccountKey = o.AccountKey
	                            JOIN [2am].[dbo].Account act on act.AccountKey = fs.AccountKey
									                            and	fs.FinancialServiceTypeKey = 1
									                            and fs.AccountStatusKey = 1

	                            join [2am].spv.SPV spv on act.SPVKey = spv.SPVKey
							                              and spv.SPVCompanyKey != s.SPVCompanyKey
                                order by 1";
            Helper.FillFromQuery(DT, query, con, parameters);

            int applicationKey = Convert.ToInt32(DT.Rows[0][0]);

            SetupResult.For(app.Key).Return(applicationKey);

            ExecuteRule(rule, 1, app);
        }

        #endregion ProductSuperLoFLSPVChange

        #region SuperLoOptOutCheck

        [Test]
        public void SuperLoOptCheckFail()
        {
            var rule = new SuperLoOptOutCheck();
            IApplication app = _mockery.StrictMock<IApplication>();
            IApplicationProduct appProd = _mockery.StrictMock<IApplicationProduct>();

            SetupResult.For(appProd.ProductType).Return(Globals.Products.NewVariableLoan);
            SetupResult.For(app.CurrentProduct).Return(appProd);

            ExecuteRule(rule, 1, app);
        }

        [Test]
        public void SuperLoOptCheckPass()
        {
            var rule = new SuperLoOptOutCheck();

            IApplicationProduct appProd = _mockery.StrictMock<IApplicationProduct>();
            SetupResult.For(appProd.ProductType).Return(Globals.Products.SuperLo);

            IApplication app = _mockery.StrictMock<IApplication>();
            SetupResult.For(app.CurrentProduct).Return(appProd);

            ExecuteRule(rule, 0, app);
        }

        #endregion SuperLoOptOutCheck
    }
}