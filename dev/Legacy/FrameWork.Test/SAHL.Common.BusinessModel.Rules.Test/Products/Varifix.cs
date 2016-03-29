using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.DataAccess;
using System.Data;
using SAHL.Common.BusinessModel.Rules.Products;

namespace SAHL.Common.BusinessModel.Rules.Test.Products
{
    [TestFixture]
    public class Varifix : RuleBase
    {
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

        #region ProductVarifixApplicationMinLoanAmount
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixApplicationMinLoanAmountSuccess()
        {
            ProductVarifixApplicationMinLoanAmount rule = new ProductVarifixApplicationMinLoanAmount();

            // Setup the correct object to pass along
            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductVariFixLoan applicationProductVariFixLoan = _mockery.StrictMock<IApplicationProductVariFixLoan>();
            SetupResult.For(app.CurrentProduct).Return(applicationProductVariFixLoan);

            // Setup applicationProductSuperLoLoan.LoanAgreementAmount
            SetupResult.For(applicationProductVariFixLoan.LoanAgreementAmount).Return(200000.0);

            ExecuteRule(rule, 0, app);
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixApplicationMinLoanAmountFail()
        {
            ProductVarifixApplicationMinLoanAmount rule = new ProductVarifixApplicationMinLoanAmount();

            // Setup the correct object to pass along
            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductVariFixLoan applicationProductVariFixLoan = _mockery.StrictMock<IApplicationProductVariFixLoan>();
            SetupResult.For(app.CurrentProduct).Return(applicationProductVariFixLoan);

            // Setup applicationProductSuperLoLoan.LoanAgreementAmount
            SetupResult.For(applicationProductVariFixLoan.LoanAgreementAmount).Return(150000.0);

            ExecuteRule(rule, 1, app);

        }
        #endregion

        #region ProductVarifixApplicationMaxLTV
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixApplicationMaxLTVSuccess()
        {
            ProductVarifixApplicationMaxLTV rule = new ProductVarifixApplicationMaxLTV();

            // Setup the correct object to pass along
            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductVariFixLoan applicationProductVariFixLoan = _mockery.StrictMock<IApplicationProductVariFixLoan>();
            SetupResult.For(app.CurrentProduct).Return(applicationProductVariFixLoan);

            // Setup applicationProductVariFixLoan.VariableLoanInformation
            IApplicationInformationVariableLoan applicationInformationVariableLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(applicationProductVariFixLoan.VariableLoanInformation).Return(applicationInformationVariableLoan);

            // Setup applicationInformationVariableLoan.LTV
            SetupResult.For(applicationInformationVariableLoan.LTV).Return(90.0);

            ExecuteRule(rule, 0, app);
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixApplicationMaxLTVFail()
        {
            ProductVarifixApplicationMaxLTV rule = new ProductVarifixApplicationMaxLTV();

            // Setup the correct object to pass along
            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductVariFixLoan applicationProductVariFixLoan = _mockery.StrictMock<IApplicationProductVariFixLoan>();
            SetupResult.For(app.CurrentProduct).Return(applicationProductVariFixLoan);

            // Setup applicationProductVariFixLoan.VariableLoanInformation
            IApplicationInformationVariableLoan applicationInformationVariableLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(applicationProductVariFixLoan.VariableLoanInformation).Return(applicationInformationVariableLoan);

            // Setup applicationInformationVariableLoan.LTV
            SetupResult.For(applicationInformationVariableLoan.LTV).Return(91.0);

            ExecuteRule(rule, 1, app);

        }
        #endregion

        #region ProductVarifixApplicationTerm
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixApplicationTermSuccess()
        {
            ProductVarifixApplicationTerm rule = new ProductVarifixApplicationTerm();

            // Setup the correct object to pass along
            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductVariFixLoan applicationProductVariFixLoan = _mockery.StrictMock<IApplicationProductVariFixLoan>();
            SetupResult.For(app.CurrentProduct).Return(applicationProductVariFixLoan);

            // Setup applicationProductSuperLoLoan.LoanAgreementAmount
            SetupResult.For(applicationProductVariFixLoan.Term).Return(20 * 12);

            ExecuteRule(rule, 0, app);
        }

        /// <summary>
        /// Expects Messages.Count = 1.0
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixApplicationTermFail()
        {
            ProductVarifixApplicationTerm rule = new ProductVarifixApplicationTerm();

            // Setup the correct object to pass along
            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductVariFixLoan applicationProductVariFixLoan = _mockery.StrictMock<IApplicationProductVariFixLoan>();
            SetupResult.For(app.CurrentProduct).Return(applicationProductVariFixLoan);

            // Setup applicationProductSuperLoLoan.LoanAgreementAmount
            SetupResult.For(applicationProductVariFixLoan.Term).Return(21 * 12);

            ExecuteRule(rule, 1, app);

        }
        #endregion

        #region ProductVarifixApplicationFixedMinimum
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixApplicationFixedMinimumSuccess()
        {
            ProductVarifixApplicationFixedMinimum rule = new ProductVarifixApplicationFixedMinimum();

            // Setup the correct object to pass along
            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductVariFixLoan applicationProductVariFixLoan = _mockery.StrictMock<IApplicationProductVariFixLoan>();
            SetupResult.For(app.CurrentProduct).Return(applicationProductVariFixLoan);

            // Setup applicationProductVariFixLoan.VariableLoanInformation.LoanAgreementAmount
            IApplicationInformationVariableLoan applicationInformationVariableLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(applicationProductVariFixLoan.VariableLoanInformation).Return(applicationInformationVariableLoan);
            SetupResult.For(applicationInformationVariableLoan.LoanAgreementAmount).Return(500000.0);
            SetupResult.For(applicationInformationVariableLoan.LoanAmountNoFees).Return(500000.0);

            // Setup applicationProductVariFixLoan.VariFixInformation.FixedInstallment
            IApplicationInformationVarifixLoan applicationInformationVarifixLoan = _mockery.StrictMock<IApplicationInformationVarifixLoan>();
            SetupResult.For(applicationProductVariFixLoan.VariFixInformation).Return(applicationInformationVarifixLoan);
            SetupResult.For(applicationInformationVarifixLoan.FixedPercent).Return(0.15);

            ExecuteRule(rule, 0, app);
        }

        /// <summary>
        /// Expects Messages.Count = 1
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixApplicationFixedMinimumFail()
        {
            ProductVarifixApplicationFixedMinimum rule = new ProductVarifixApplicationFixedMinimum();

            // Setup the correct object to pass along
            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductVariFixLoan applicationProductVariFixLoan = _mockery.StrictMock<IApplicationProductVariFixLoan>();
            SetupResult.For(app.CurrentProduct).Return(applicationProductVariFixLoan);

            // Setup applicationProductVariFixLoan.VariableLoanInformation.LoanAgreementAmount
            IApplicationInformationVariableLoan applicationInformationVariableLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(applicationProductVariFixLoan.VariableLoanInformation).Return(applicationInformationVariableLoan);
            SetupResult.For(applicationInformationVariableLoan.LoanAgreementAmount).Return(500000.0);
            SetupResult.For(applicationInformationVariableLoan.LoanAmountNoFees).Return(500000.0);

            // Setup applicationProductVariFixLoan.VariFixInformation.FixedInstallment
            IApplicationInformationVarifixLoan applicationInformationVarifixLoan = _mockery.StrictMock<IApplicationInformationVarifixLoan>();
            SetupResult.For(applicationProductVariFixLoan.VariFixInformation).Return(applicationInformationVarifixLoan);
            SetupResult.For(applicationInformationVarifixLoan.FixedPercent).Return(0.09);

            ExecuteRule(rule, 1, app);

        }
        #endregion

        #region ProductVarifixAccountOptInOpenAccount
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixAccountOptInOpenAccountSuccess()
        {
            ProductVarifixAccountOptInOpenAccount rule = new ProductVarifixAccountOptInOpenAccount();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.AccountStatus
            IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
            SetupResult.For(mortgageLoanAccount.AccountStatus).Return(accountStatus);
            SetupResult.For(accountStatus.Key).Return((int)AccountStatuses.Open);

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        /// <summary>
        /// Expects Messages.Count = 1.0
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixAccountOptInOpenAccountFail()
        {
            ProductVarifixAccountOptInOpenAccount rule = new ProductVarifixAccountOptInOpenAccount();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.AccountStatus
            IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
            SetupResult.For(mortgageLoanAccount.AccountStatus).Return(accountStatus);
            SetupResult.For(accountStatus.Key).Return((int)AccountStatuses.Closed);

            ExecuteRule(rule, 1, mortgageLoanAccount);

        }


        #endregion

        #region ProductVarifixAccountOptInFurtherLoan
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixAccountOptInFurtherLoanSuccess()
        {
            ProductVarifixAccountOptInFurtherLoan rule = new ProductVarifixAccountOptInFurtherLoan();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.Applications
            IEventList<IApplication> applications = new EventList<IApplication>();
            IApplicationFurtherLoan application = _mockery.StrictMock<IApplicationFurtherLoan>();
            applications.Add(Messages, application);
            SetupResult.For(mortgageLoanAccount.Applications).Return(applications);

            // Setup application.ApplicationStatus.Key
            IApplicationStatus applicationStatus = _mockery.StrictMock<IApplicationStatus>();
            SetupResult.For(application.ApplicationStatus).Return(applicationStatus);
            SetupResult.For(applicationStatus.Key).Return((int)OfferStatuses.Closed);

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        /// <summary>
        /// Expects Messages.Count = 1.0
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixAccountOptInFurtherLoanFail()
        {
            ProductVarifixAccountOptInFurtherLoan rule = new ProductVarifixAccountOptInFurtherLoan();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.Applications
            IEventList<IApplication> applications = new EventList<IApplication>();
            IApplicationFurtherLoan application = _mockery.StrictMock<IApplicationFurtherLoan>();
            applications.Add(Messages, application);
            SetupResult.For(mortgageLoanAccount.Applications).Return(applications);

            // Setup application.ApplicationStatus.Key
            IApplicationStatus applicationStatus = _mockery.StrictMock<IApplicationStatus>();
            SetupResult.For(application.ApplicationStatus).Return(applicationStatus);
            SetupResult.For(applicationStatus.Key).Return((int)OfferStatuses.Open);

            ExecuteRule(rule, 1, mortgageLoanAccount);

        }
        #endregion

        #region ProductVarifixAccountOptInReadvance
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixAccountOptInReadvanceSuccess()
        {
            ProductVarifixAccountOptInReadvance rule = new ProductVarifixAccountOptInReadvance();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.Applications
            IEventList<IApplication> applications = new EventList<IApplication>();
            IApplicationReAdvance application = _mockery.StrictMock<IApplicationReAdvance>();
            applications.Add(Messages, application);
            SetupResult.For(mortgageLoanAccount.Applications).Return(applications);

            // Setup application.ApplicationStatus.Key
            IApplicationStatus applicationStatus = _mockery.StrictMock<IApplicationStatus>();
            SetupResult.For(application.ApplicationStatus).Return(applicationStatus);
            SetupResult.For(applicationStatus.Key).Return((int)OfferStatuses.Closed);

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        /// <summary>
        /// Expects Messages.Count = 1.0
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixAccountOptInReadvanceFail()
        {
            ProductVarifixAccountOptInReadvance rule = new ProductVarifixAccountOptInReadvance();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.Applications
            IEventList<IApplication> applications = new EventList<IApplication>();
            IApplicationReAdvance application = _mockery.StrictMock<IApplicationReAdvance>();
            applications.Add(Messages, application);
            SetupResult.For(mortgageLoanAccount.Applications).Return(applications);

            // Setup application.ApplicationStatus.Key
            IApplicationStatus applicationStatus = _mockery.StrictMock<IApplicationStatus>();
            SetupResult.For(application.ApplicationStatus).Return(applicationStatus);
            SetupResult.For(applicationStatus.Key).Return((int)OfferStatuses.Open);

            ExecuteRule(rule, 1, mortgageLoanAccount);

        }
        #endregion

        #region ProductVarifixAccountOptInArrears
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixAccountOptInArrearsSuccess()
        {
            ProductVarifixAccountOptInArrears rule = new ProductVarifixAccountOptInArrears();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.InstallmentSummary
            IAccountInstallmentSummary accountInstallmentSummary = _mockery.StrictMock<IAccountInstallmentSummary>();
            SetupResult.For(mortgageLoanAccount.InstallmentSummary).Return(accountInstallmentSummary);

            // Setup accountInstallmentSummary.MonthsInArrears
            SetupResult.For(accountInstallmentSummary.MonthsInArrears).Return(1.0);

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        /// <summary>
        /// Expects Messages.Count = 1.0
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixAccountOptInArrearsFail()
        {
            ProductVarifixAccountOptInArrears rule = new ProductVarifixAccountOptInArrears();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.InstallmentSummary
            IAccountInstallmentSummary accountInstallmentSummary = _mockery.StrictMock<IAccountInstallmentSummary>();
            SetupResult.For(mortgageLoanAccount.InstallmentSummary).Return(accountInstallmentSummary);

            // Setup accountInstallmentSummary.MonthsInArrears
            SetupResult.For(accountInstallmentSummary.MonthsInArrears).Return(1.3);

            ExecuteRule(rule, 1, mortgageLoanAccount);
        }
        #endregion


        #region ProductVarifixAccountOptInDebtCounselling
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixAccountOptInDebtCounsellingSuccess()
        {
            ProductVarifixAccountOptInDebtCounselling rule = new ProductVarifixAccountOptInDebtCounselling();

            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup  mortgageLoanAccount.Key
            SetupResult.For(mortgageLoanAccount.Key).Return(-1);

            IStageDefinitionRepository stageDefinitionRepo = _mockery.StrictMock<IStageDefinitionRepository>();
            MockCache.Add((typeof(IStageDefinitionRepository)).ToString(), stageDefinitionRepo);

			IStageDefinitionStageDefinitionGroup sdsdg = _mockery.StrictMock<IStageDefinitionStageDefinitionGroup>();
			SetupResult.For(sdsdg.Key).Return(-1);

            // Setup StageDefinitionRepository.CheckCompositeStageDefinition()
			SetupResult.For(stageDefinitionRepo.GetStageDefinitionStageDefinitionGroup(-1, -1)).IgnoreArguments().Return(sdsdg);
            SetupResult.For(stageDefinitionRepo.CheckCompositeStageDefinition(-1, -1)).IgnoreArguments().Return(false);

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        /// <summary>
        /// Expects Messages.Count = 1.0
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixAccountOptInDebtCounsellingFail()
        {
			ProductVarifixAccountOptInDebtCounselling rule = new ProductVarifixAccountOptInDebtCounselling();

			SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);

			// Setup the correct object to pass along
			IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

			// Setup  mortgageLoanAccount.Key
			SetupResult.For(mortgageLoanAccount.Key).Return(-1);

			IStageDefinitionRepository stageDefinitionRepo = _mockery.StrictMock<IStageDefinitionRepository>();
			MockCache.Add((typeof(IStageDefinitionRepository)).ToString(), stageDefinitionRepo);

			IStageDefinitionStageDefinitionGroup sdsdg = _mockery.StrictMock<IStageDefinitionStageDefinitionGroup>();
			SetupResult.For(sdsdg.Key).Return(-1);

			// Setup StageDefinitionRepository.CheckCompositeStageDefinition()
			SetupResult.For(stageDefinitionRepo.GetStageDefinitionStageDefinitionGroup(-1, -1)).IgnoreArguments().Return(sdsdg);
			SetupResult.For(stageDefinitionRepo.CheckCompositeStageDefinition(-1, -1)).IgnoreArguments().Return(true);

			ExecuteRule(rule, 1, mortgageLoanAccount);

        }
        #endregion

        #region ProductVarifixAccountOptInCapApplication
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixAccountOptInCapApplicationSuccess()
        {
            ProductVarifixAccountOptInCapApplication rule = new ProductVarifixAccountOptInCapApplication();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.Applications
            IEventList<IApplication> applications = new EventList<IApplication>();
            IApplicationFurtherLoan application = _mockery.StrictMock<IApplicationFurtherLoan>();
            applications.Add(Messages, application);
            SetupResult.For(mortgageLoanAccount.Applications).Return(applications);

            // Setup application.ApplicationInformations
            IEventList<IApplicationInformation> applicationInformations = new EventList<IApplicationInformation>();
            IApplicationInformation  applicationInformation  = _mockery.StrictMock<IApplicationInformation>();
            applicationInformations.Add(Messages, applicationInformation);
            SetupResult.For(application.ApplicationInformations).Return(applicationInformations);

            // Setup ApplicationInformation.ApplicationInformationRateOverrides
            IEventList<IApplicationInformationFinancialAdjustment> applicationInformationFinancialAdjustments = new EventList<IApplicationInformationFinancialAdjustment>();
            var applicationInformationRateOverride = _mockery.StrictMock<IApplicationInformationFinancialAdjustment>();
            applicationInformationFinancialAdjustments.Add(Messages, applicationInformationRateOverride);
            SetupResult.For(applicationInformation.ApplicationInformationFinancialAdjustments).Return(applicationInformationFinancialAdjustments);

            // Setup financialAdjustmentTypeSource.Key
            IFinancialAdjustmentTypeSource financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();
            SetupResult.For(financialAdjustmentTypeSource.Key).Return(0);

            // Setup applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource
            SetupResult.For(applicationInformationRateOverride.FinancialAdjustmentTypeSource).Return(financialAdjustmentTypeSource);

            ExecuteRule(rule, 0, mortgageLoanAccount);

        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixAccountOptInCapApplicationFail()
        {
            ProductVarifixAccountOptInCapApplication rule = new ProductVarifixAccountOptInCapApplication();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.Applications
            IEventList<IApplication> applications = new EventList<IApplication>();
            IApplicationFurtherLoan application = _mockery.StrictMock<IApplicationFurtherLoan>();
            applications.Add(Messages, application);
            SetupResult.For(mortgageLoanAccount.Applications).Return(applications);

            // Setup application.ApplicationInformations
            IEventList<IApplicationInformation> applicationInformations = new EventList<IApplicationInformation>();
            var applicationInformation = _mockery.StrictMock<IApplicationInformation>();
            applicationInformations.Add(Messages, applicationInformation);
            SetupResult.For(application.ApplicationInformations).Return(applicationInformations);

            // Setup ApplicationInformation.ApplicationInformationRateOverrides
            IEventList<IApplicationInformationFinancialAdjustment> applicationInformationFinancialAdjustments = new EventList<IApplicationInformationFinancialAdjustment>();
            var applicationInformationRateOverride = _mockery.StrictMock<IApplicationInformationFinancialAdjustment>();
            applicationInformationFinancialAdjustments.Add(Messages, applicationInformationRateOverride);
            SetupResult.For(applicationInformation.ApplicationInformationFinancialAdjustments).Return(applicationInformationFinancialAdjustments);

            // Setup financialAdjustmentTypeSource.Key
            IFinancialAdjustmentTypeSource financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();
            SetupResult.For(financialAdjustmentTypeSource.Key).Return((int)FinancialAdjustmentTypeSources.CAP2);

            // Setup applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource
            SetupResult.For(applicationInformationRateOverride.FinancialAdjustmentTypeSource).Return(financialAdjustmentTypeSource);

            ExecuteRule(rule, 1, mortgageLoanAccount);

        }
        #endregion

        #region ProductVarifixOptInLoanTransaction
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixOptInLoanTransactionSuccess()
        {
            ProductVarifixOptInLoanTransaction rule = new ProductVarifixOptInLoanTransaction();

            // Setup the correct object to pass along
            IApplication app = _mockery.StrictMock<IApplication>();
            ParameterCollection parameters = new ParameterCollection();
            IDbConnection con = Helper.GetSQLDBConnection("EWorkConnectionString");
            DataTable DT = new DataTable();
            string query = "select hd.LoanNumber from [e-work]..HelpDesk hd where hd.ActiveFolder != -1 and hd.LoanNumber != 0";
            Helper.FillFromQuery(DT, query, con, parameters);
            int accountKey = Convert.ToInt32(DT.Rows[0][0]);
            IAccount acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(acc.Key).Return((int)accountKey);
            SetupResult.For(app.Account).Return(acc);
            IAccountStatus status = _mockery.StrictMock<IAccountStatus>();
            SetupResult.For(status.Key).Return((int)AccountStatuses.Open);
            SetupResult.For(acc.AccountStatus).Return(status);
            ExecuteRule(rule, 0, app);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixOptInLoanTransactionFail()
        {
            ProductVarifixOptInLoanTransaction rule = new ProductVarifixOptInLoanTransaction();

            // Setup the correct object to pass along
            IApplicationFurtherLoan app = _mockery.StrictMock<IApplicationFurtherLoan>();
            ParameterCollection parameters = new ParameterCollection();
            IDbConnection con = Helper.GetSQLDBConnection("EWorkConnectionString");
            DataTable DT = new DataTable();
            string query = "select hd.LoanNumber from [e-work]..HelpDesk hd where hd.ActiveFolder = -1 and hd.bClientOptOut = 0";
            Helper.FillFromQuery(DT, query, con, parameters);
            int accountKey = Convert.ToInt32(DT.Rows[0][0]);
            IAccount acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(acc.Key).Return((int)accountKey);
            SetupResult.For(app.Account).Return(acc);
            IAccountStatus status = _mockery.StrictMock<IAccountStatus>();
            SetupResult.For(status.Key).Return((int)AccountStatuses.Open);
            SetupResult.For(acc.AccountStatus).Return(status);
            ExecuteRule(rule, 1, app);
        }

        #endregion

        #region ProductVarifixOptInFlag
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixOptInFlagSuccess()
        {
            ProductVarifixOptInFlag rule = new ProductVarifixOptInFlag();

            // Setup the correct object to pass along
            IApplication app = _mockery.StrictMock<IApplication>();
            ParameterCollection parameters = new ParameterCollection();
            IDbConnection con = Helper.GetSQLDBConnection("EWorkConnectionString");
            DataTable DT = new DataTable();
            string query = "select hd.LoanNumber from [e-work]..HelpDesk hd where hd.ActiveFolder != -1 and hd.LoanNumber != 0";
            Helper.FillFromQuery(DT, query, con, parameters);
            int accountKey = Convert.ToInt32(DT.Rows[0][0]);
            IAccount acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(acc.Key).Return((int)accountKey);
            SetupResult.For(app.Account).Return(acc);
            IAccountStatus status = _mockery.StrictMock<IAccountStatus>();
            SetupResult.For(status.Key).Return((int)AccountStatuses.Open);
            SetupResult.For(acc.AccountStatus).Return(status);
            ExecuteRule(rule, 0, app);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductVarifixOptInFlagFail()
        {
            ProductVarifixOptInFlag rule = new ProductVarifixOptInFlag();

            // Setup the correct object to pass along
            IApplicationFurtherLoan app = _mockery.StrictMock<IApplicationFurtherLoan>();
            ParameterCollection parameters = new ParameterCollection();
            IDbConnection con = Helper.GetSQLDBConnection("EWorkConnectionString");
            DataTable DT = new DataTable();
            string query = "select hd.LoanNumber from [e-work]..HelpDesk hd where hd.ActiveFolder = -1 and hd.bClientOptOut = 0";
            Helper.FillFromQuery(DT, query, con, parameters);
            int accountKey = Convert.ToInt32(DT.Rows[0][0]);
            IAccount acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(acc.Key).Return((int)accountKey);
            SetupResult.For(app.Account).Return(acc);
            IAccountStatus status = _mockery.StrictMock<IAccountStatus>();
            SetupResult.For(status.Key).Return((int)AccountStatuses.Open);
            SetupResult.For(acc.AccountStatus).Return(status);
            ExecuteRule(rule, 1, app);
        }

        #endregion

    }
}
