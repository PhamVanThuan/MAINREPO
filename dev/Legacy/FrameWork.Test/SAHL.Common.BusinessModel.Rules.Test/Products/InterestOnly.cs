using System;
using System.Collections.Generic;
using System.Data;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
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
    public class InterestOnly : RuleBase
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

        #region AttributeInterestOnlyVarifix

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AttributeInterestOnlyVarifixSuccess()
        {
            AttributeInterestOnlyVarifix rule = new AttributeInterestOnlyVarifix();

            // Setup the correct object to pass along
            IApplicationMortgageLoan mortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductVariFixLoan applicationProductVariFixLoan = _mockery.StrictMock<IApplicationProductVariFixLoan>();
            SetupResult.For(mortgageLoan.CurrentProduct).Return(applicationProductVariFixLoan);

            // Setup applicationProductVariFixLoan.VariFixInformation
            IApplicationInformationVarifixLoan applicationInformationVarifixLoan = _mockery.StrictMock<IApplicationInformationVarifixLoan>();
            SetupResult.For(applicationProductVariFixLoan.VariFixInformation).Return(applicationInformationVarifixLoan);

            // Setup applicationInformationVarifixLoan.ApplicationInformation
            IApplicationInformation applicationInformation = _mockery.StrictMock<IApplicationInformation>();
            SetupResult.For(applicationInformationVarifixLoan.ApplicationInformation).Return(applicationInformation);

            // Setup ApplicationInformation.ApplicationInformationFinancialAdjustments
            IEventList<IApplicationInformationFinancialAdjustment> applicationInformationFinancialAdjustments = new EventList<IApplicationInformationFinancialAdjustment>();
            IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment = _mockery.StrictMock<IApplicationInformationFinancialAdjustment>();
            applicationInformationFinancialAdjustments.Add(Messages, applicationInformationFinancialAdjustment);
            SetupResult.For(applicationInformation.ApplicationInformationFinancialAdjustments).Return(applicationInformationFinancialAdjustments);

            // Setup financialAdjustmentTypeSource.Key
            IFinancialAdjustmentTypeSource financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();
            SetupResult.For(financialAdjustmentTypeSource.Key).Return((int)FinancialAdjustmentTypeSources.DiscountedLinkrate);  // Some other arb value

            // Setup applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource
            SetupResult.For(applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource).Return(financialAdjustmentTypeSource);

            ExecuteRule(rule, 0, mortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AttributeInterestOnlyVarifixFail()
        {
            AttributeInterestOnlyVarifix rule = new AttributeInterestOnlyVarifix();

            // Setup the correct object to pass along
            IApplicationMortgageLoan mortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductVariFixLoan applicationProductVariFixLoan = _mockery.StrictMock<IApplicationProductVariFixLoan>();
            SetupResult.For(mortgageLoan.CurrentProduct).Return(applicationProductVariFixLoan);

            // Setup applicationProductVariFixLoan.VariFixInformation
            IApplicationInformationVarifixLoan applicationInformationVarifixLoan = _mockery.StrictMock<IApplicationInformationVarifixLoan>();
            SetupResult.For(applicationProductVariFixLoan.VariFixInformation).Return(applicationInformationVarifixLoan);

            // Setup applicationInformationVarifixLoan.ApplicationInformation
            IApplicationInformation applicationInformation = _mockery.StrictMock<IApplicationInformation>();
            SetupResult.For(applicationInformationVarifixLoan.ApplicationInformation).Return(applicationInformation);

            // Setup ApplicationInformation.ApplicationInformationFinancialAdjustments
            IEventList<IApplicationInformationFinancialAdjustment> applicationInformationFinancialAdjustments = new EventList<IApplicationInformationFinancialAdjustment>();
            IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment = _mockery.StrictMock<IApplicationInformationFinancialAdjustment>();
            applicationInformationFinancialAdjustments.Add(Messages, applicationInformationFinancialAdjustment);
            SetupResult.For(applicationInformation.ApplicationInformationFinancialAdjustments).Return(applicationInformationFinancialAdjustments);

            // Setup financialAdjustmentTypeSource.Key
            IFinancialAdjustmentTypeSource financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();
            SetupResult.For(financialAdjustmentTypeSource.Key).Return((int)FinancialAdjustmentTypeSources.InterestOnly);

            // Setup applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource
            SetupResult.For(applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource).Return(financialAdjustmentTypeSource);

            ExecuteRule(rule, 1, mortgageLoan);
        }

        #endregion AttributeInterestOnlyVarifix

        #region AttributeInterestOnlyMinimumLoanAmount

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AttributeInterestOnlyMinimumLoanAmountSuccess()
        {
            AttributeInterestOnlyMinimumLoanAmount rule = new AttributeInterestOnlyMinimumLoanAmount();

            // Setup the correct object to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.CurrentProduct
            IApplicationProductMortgageLoan applicationProductMortgageLoan = _mockery.StrictMock<IApplicationProductMortgageLoan>();
            SetupResult.For(applicationMortgageLoan.CurrentProduct).Return(applicationProductMortgageLoan);

            // Setup applicationMortgageLoan.LoanAgreementAmount
            SetupResult.For(applicationProductMortgageLoan.LoanAgreementAmount).Return(250000.0);

            // Setup applicationMortgageLoan.GetLatestApplicationInformation()
            IApplicationInformation applicationInformation = _mockery.StrictMock<IApplicationInformation>();
            SetupResult.For(applicationMortgageLoan.GetLatestApplicationInformation()).Return(applicationInformation);

            // Setup applicationInformation.ApplicationInformationFinancialAdjustments
            IEventList<IApplicationInformationFinancialAdjustment> applicationInformationFinancialAdjustments = new EventList<IApplicationInformationFinancialAdjustment>();
            IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment = _mockery.StrictMock<IApplicationInformationFinancialAdjustment>();
            applicationInformationFinancialAdjustments.Add(Messages, applicationInformationFinancialAdjustment);
            SetupResult.For(applicationInformation.ApplicationInformationFinancialAdjustments).Return(applicationInformationFinancialAdjustments);

            // Setup applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource
            IFinancialAdjustmentTypeSource financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();
            SetupResult.For(financialAdjustmentTypeSource.Key).Return((int)FinancialAdjustmentTypeSources.InterestOnly);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AttributeInterestOnlyMinimumLoanAmountFail()
        {
            AttributeInterestOnlyMinimumLoanAmount rule = new AttributeInterestOnlyMinimumLoanAmount();

            // Setup the correct object to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.CurrentProduct
            IApplicationProductMortgageLoan applicationProductMortgageLoan = _mockery.StrictMock<IApplicationProductMortgageLoan>();
            SetupResult.For(applicationMortgageLoan.CurrentProduct).Return(applicationProductMortgageLoan);

            // Setup applicationMortgageLoan.LoanAgreementAmount
            SetupResult.For(applicationProductMortgageLoan.LoanAgreementAmount).Return(200000.0);

            // Setup applicationMortgageLoan.GetLatestApplicationInformation()
            IApplicationInformation applicationInformation = _mockery.StrictMock<IApplicationInformation>();
            SetupResult.For(applicationMortgageLoan.GetLatestApplicationInformation()).Return(applicationInformation);

            // Setup applicationInformation.ApplicationInformationFinancialAdjustments
            IEventList<IApplicationInformationFinancialAdjustment> applicationInformationFinancialAdjustments = new EventList<IApplicationInformationFinancialAdjustment>();
            IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment = _mockery.StrictMock<IApplicationInformationFinancialAdjustment>();
            applicationInformationFinancialAdjustments.Add(Messages, applicationInformationFinancialAdjustment);
            SetupResult.For(applicationInformation.ApplicationInformationFinancialAdjustments).Return(applicationInformationFinancialAdjustments);

            // Setup financialAdjustmentTypeSource.Key
            IFinancialAdjustmentTypeSource financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();
            SetupResult.For(financialAdjustmentTypeSource.Key).Return((int)FinancialAdjustmentTypeSources.InterestOnly);

            // Setup applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource
            SetupResult.For(applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource).Return(financialAdjustmentTypeSource);

            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        #endregion AttributeInterestOnlyMinimumLoanAmount

        #region AttributeInterestOnlyMaximumLoanAmount

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AttributeInterestOnlyMaximumLoanAmountSuccess()
        {
            AttributeInterestOnlyMaximumLoanAmount rule = new AttributeInterestOnlyMaximumLoanAmount();

            // Setup the correct object to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.CurrentProduct
            IApplicationProductMortgageLoan applicationProductMortgageLoan = _mockery.StrictMock<IApplicationProductMortgageLoan>();
            SetupResult.For(applicationMortgageLoan.CurrentProduct).Return(applicationProductMortgageLoan);

            // Setup applicationMortgageLoan.LoanAgreementAmount
            SetupResult.For(applicationProductMortgageLoan.LoanAgreementAmount).Return(3000000.0);

            // Setup applicationMortgageLoan.GetLatestApplicationInformation()
            IApplicationInformation applicationInformation = _mockery.StrictMock<IApplicationInformation>();
            SetupResult.For(applicationMortgageLoan.GetLatestApplicationInformation()).Return(applicationInformation);

            // Setup applicationInformation.ApplicationInformationFinancialAdjustments
            IEventList<IApplicationInformationFinancialAdjustment> applicationInformationFinancialAdjustments = new EventList<IApplicationInformationFinancialAdjustment>();
            IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment = _mockery.StrictMock<IApplicationInformationFinancialAdjustment>();
            applicationInformationFinancialAdjustments.Add(Messages, applicationInformationFinancialAdjustment);
            SetupResult.For(applicationInformation.ApplicationInformationFinancialAdjustments).Return(applicationInformationFinancialAdjustments);

            // Setup applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource
            IFinancialAdjustmentTypeSource financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();
            SetupResult.For(financialAdjustmentTypeSource.Key).Return((int)FinancialAdjustmentTypeSources.InterestOnly);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AttributeInterestOnlyMaximumLoanAmountFail()
        {
            AttributeInterestOnlyMaximumLoanAmount rule = new AttributeInterestOnlyMaximumLoanAmount();

            // Setup the correct object to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.CurrentProduct
            IApplicationProductMortgageLoan applicationProductMortgageLoan = _mockery.StrictMock<IApplicationProductMortgageLoan>();
            SetupResult.For(applicationMortgageLoan.CurrentProduct).Return(applicationProductMortgageLoan);

            // Setup applicationMortgageLoan.LoanAgreementAmount
            SetupResult.For(applicationProductMortgageLoan.LoanAgreementAmount).Return(4000000.0);

            // Setup applicationMortgageLoan.GetLatestApplicationInformation()
            IApplicationInformation applicationInformation = _mockery.StrictMock<IApplicationInformation>();
            SetupResult.For(applicationMortgageLoan.GetLatestApplicationInformation()).Return(applicationInformation);

            // Setup applicationInformation.ApplicationInformationFinancialAdjustments
            IEventList<IApplicationInformationFinancialAdjustment> applicationInformationFinancialAdjustments = new EventList<IApplicationInformationFinancialAdjustment>();
            IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment = _mockery.StrictMock<IApplicationInformationFinancialAdjustment>();
            applicationInformationFinancialAdjustments.Add(Messages, applicationInformationFinancialAdjustment);
            SetupResult.For(applicationInformation.ApplicationInformationFinancialAdjustments).Return(applicationInformationFinancialAdjustments);

            // Setup financialAdjustmentTypeSource.Key
            IFinancialAdjustmentTypeSource financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();
            SetupResult.For(financialAdjustmentTypeSource.Key).Return((int)FinancialAdjustmentTypeSources.InterestOnly);

            // Setup applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource
            SetupResult.For(applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource).Return(financialAdjustmentTypeSource);

            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        #endregion AttributeInterestOnlyMaximumLoanAmount

        #region AttributeInterestOnlyMaxLTV

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AttributeInterestOnlyMaxLTVSuccess()
        {
            AttributeInterestOnlyMaxLTV rule = new AttributeInterestOnlyMaxLTV();

            // Setup the correct object to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.GetLatestApplicationInformation()
            IApplicationInformation applicationInformation = _mockery.StrictMock<IApplicationInformation>();
            SetupResult.For(applicationMortgageLoan.GetLatestApplicationInformation()).Return(applicationInformation);

            // Setup applicationInformation.ApplicationInformationFinancialAdjustments
            IEventList<IApplicationInformationFinancialAdjustment> applicationInformationFinancialAdjustments = new EventList<IApplicationInformationFinancialAdjustment>();
            IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment = _mockery.StrictMock<IApplicationInformationFinancialAdjustment>();
            applicationInformationFinancialAdjustments.Add(Messages, applicationInformationFinancialAdjustment);
            SetupResult.For(applicationInformation.ApplicationInformationFinancialAdjustments).Return(applicationInformationFinancialAdjustments);

            // Setup financialAdjustmentTypeSource.Key
            IFinancialAdjustmentTypeSource financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();
            SetupResult.For(financialAdjustmentTypeSource.Key).Return((int)FinancialAdjustmentTypeSources.InterestOnly);

            // Setup applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource
            SetupResult.For(applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource).Return(financialAdjustmentTypeSource);

            // Setup applicationMortgageLoan.CurrentProduct
            IApplicationProductNewVariableLoan applicationProductNewVariableLoan = _mockery.StrictMock<IApplicationProductNewVariableLoan>();
            SetupResult.For(applicationMortgageLoan.CurrentProduct).Return(applicationProductNewVariableLoan);

            // Setup supportsVariableLoanApplicationInformation.VariableLoanInformation
            IApplicationInformationVariableLoan applicationInformationVariableLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(applicationProductNewVariableLoan.VariableLoanInformation).Return(applicationInformationVariableLoan);

            // Setup applicationInformationVariableLoan.LTV
            SetupResult.For(applicationInformationVariableLoan.LTV).Return(89.0);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AttributeInterestOnlyMaxLTVFail()
        {
            AttributeInterestOnlyMaxLTV rule = new AttributeInterestOnlyMaxLTV();

            // Setup the correct object to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.GetLatestApplicationInformation()
            IApplicationInformation applicationInformation = _mockery.StrictMock<IApplicationInformation>();
            SetupResult.For(applicationMortgageLoan.GetLatestApplicationInformation()).Return(applicationInformation);

            // Setup applicationInformation.ApplicationInformationFinancialAdjustments
            IEventList<IApplicationInformationFinancialAdjustment> applicationInformationFinancialAdjustments = new EventList<IApplicationInformationFinancialAdjustment>();
            IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment = _mockery.StrictMock<IApplicationInformationFinancialAdjustment>();
            applicationInformationFinancialAdjustments.Add(Messages, applicationInformationFinancialAdjustment);
            SetupResult.For(applicationInformation.ApplicationInformationFinancialAdjustments).Return(applicationInformationFinancialAdjustments);

            // Setup financialAdjustmentTypeSource.Key
            IFinancialAdjustmentTypeSource financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();
            SetupResult.For(financialAdjustmentTypeSource.Key).Return((int)FinancialAdjustmentTypeSources.InterestOnly);

            // Setup applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource
            SetupResult.For(applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource).Return(financialAdjustmentTypeSource);

            // Setup applicationMortgageLoan.CurrentProduct
            IApplicationProductNewVariableLoan applicationProductNewVariableLoan = _mockery.StrictMock<IApplicationProductNewVariableLoan>();
            SetupResult.For(applicationMortgageLoan.CurrentProduct).Return(applicationProductNewVariableLoan);

            // Setup supportsVariableLoanApplicationInformation.VariableLoanInformation
            IApplicationInformationVariableLoan applicationInformationVariableLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(applicationProductNewVariableLoan.VariableLoanInformation).Return(applicationInformationVariableLoan);

            // Setup applicationInformationVariableLoan.LTV
            SetupResult.For(applicationInformationVariableLoan.LTV).Return(90.1);

            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        #endregion AttributeInterestOnlyMaxLTV

        #region AttributeInterestOnlyMaxLoanAmountLTV80

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AttributeInterestOnlyMaxLoanAmountLTV80Success()
        {
            AttributeInterestOnlyMaxLoanAmountLTV80 rule = new AttributeInterestOnlyMaxLoanAmountLTV80();

            // Setup the correct object to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.GetLatestApplicationInformation()
            IApplicationInformation applicationInformation = _mockery.StrictMock<IApplicationInformation>();
            SetupResult.For(applicationMortgageLoan.GetLatestApplicationInformation()).Return(applicationInformation);

            // Setup applicationInformation.ApplicationInformationFinancialAdjustments
            IEventList<IApplicationInformationFinancialAdjustment> applicationInformationFinancialAdjustments = new EventList<IApplicationInformationFinancialAdjustment>();
            IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment = _mockery.StrictMock<IApplicationInformationFinancialAdjustment>();
            applicationInformationFinancialAdjustments.Add(Messages, applicationInformationFinancialAdjustment);
            SetupResult.For(applicationInformation.ApplicationInformationFinancialAdjustments).Return(applicationInformationFinancialAdjustments);

            // Setup financialAdjustmentTypeSource.Key
            IFinancialAdjustmentTypeSource financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();
            SetupResult.For(financialAdjustmentTypeSource.Key).Return((int)FinancialAdjustmentTypeSources.InterestOnly);

            // Setup applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource
            SetupResult.For(applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource).Return(financialAdjustmentTypeSource);

            // Setup applicationMortgageLoan.CurrentProduct
            IApplicationProductNewVariableLoan applicationProductNewVariableLoan = _mockery.StrictMock<IApplicationProductNewVariableLoan>();
            SetupResult.For(applicationMortgageLoan.CurrentProduct).Return(applicationProductNewVariableLoan);

            // Setup supportsVariableLoanApplicationInformation.VariableLoanInformation
            IApplicationInformationVariableLoan applicationInformationVariableLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(applicationProductNewVariableLoan.VariableLoanInformation).Return(applicationInformationVariableLoan);

            // Setup VariableLoanInformation.LoanAgreementAmount
            SetupResult.For(applicationInformationVariableLoan.LoanAgreementAmount).Return(1600000.0);

            // Setup applicationInformationVariableLoan.LTV
            SetupResult.For(applicationInformationVariableLoan.LTV).Return(80.0);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AttributeInterestOnlyMaxLoanAmountLTV80Fail()
        {
            AttributeInterestOnlyMaxLoanAmountLTV80 rule = new AttributeInterestOnlyMaxLoanAmountLTV80();

            // Setup the correct object to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.GetLatestApplicationInformation()
            IApplicationInformation applicationInformation = _mockery.StrictMock<IApplicationInformation>();
            SetupResult.For(applicationMortgageLoan.GetLatestApplicationInformation()).Return(applicationInformation);

            // Setup applicationInformation.ApplicationInformationFinancialAdjustments
            IEventList<IApplicationInformationFinancialAdjustment> applicationInformationFinancialAdjustments = new EventList<IApplicationInformationFinancialAdjustment>();
            IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment = _mockery.StrictMock<IApplicationInformationFinancialAdjustment>();
            applicationInformationFinancialAdjustments.Add(Messages, applicationInformationFinancialAdjustment);
            SetupResult.For(applicationInformation.ApplicationInformationFinancialAdjustments).Return(applicationInformationFinancialAdjustments);

            // Setup financialAdjustmentTypeSource.Key
            IFinancialAdjustmentTypeSource financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();
            SetupResult.For(financialAdjustmentTypeSource.Key).Return((int)FinancialAdjustmentTypeSources.InterestOnly);

            // Setup applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource
            SetupResult.For(applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource).Return(financialAdjustmentTypeSource);

            // Setup applicationMortgageLoan.CurrentProduct
            IApplicationProductNewVariableLoan applicationProductNewVariableLoan = _mockery.StrictMock<IApplicationProductNewVariableLoan>();
            SetupResult.For(applicationMortgageLoan.CurrentProduct).Return(applicationProductNewVariableLoan);

            // Setup supportsVariableLoanApplicationInformation.VariableLoanInformation
            IApplicationInformationVariableLoan applicationInformationVariableLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(applicationProductNewVariableLoan.VariableLoanInformation).Return(applicationInformationVariableLoan);

            // Setup VariableLoanInformation.LoanAgreementAmount
            SetupResult.For(applicationInformationVariableLoan.LoanAgreementAmount).Return(1600000.0);

            // Setup applicationInformationVariableLoan.LTV
            SetupResult.For(applicationInformationVariableLoan.LTV).Return(81.0);

            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        #endregion AttributeInterestOnlyMaxLoanAmountLTV80

        #region AttributeInterestOnlyMaxLoanAmountLTV90

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AttributeInterestOnlyMaxLoanAmountLTV90Success()
        {
            AttributeInterestOnlyMaxLoanAmountLTV90 rule = new AttributeInterestOnlyMaxLoanAmountLTV90();

            // Setup the correct object to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.GetLatestApplicationInformation()
            IApplicationInformation applicationInformation = _mockery.StrictMock<IApplicationInformation>();
            SetupResult.For(applicationMortgageLoan.GetLatestApplicationInformation()).Return(applicationInformation);

            // Setup applicationInformation.ApplicationInformationFinancialAdjustments
            IEventList<IApplicationInformationFinancialAdjustment> applicationInformationFinancialAdjustments = new EventList<IApplicationInformationFinancialAdjustment>();
            IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment = _mockery.StrictMock<IApplicationInformationFinancialAdjustment>();
            applicationInformationFinancialAdjustments.Add(Messages, applicationInformationFinancialAdjustment);
            SetupResult.For(applicationInformation.ApplicationInformationFinancialAdjustments).Return(applicationInformationFinancialAdjustments);

            // Setup financialAdjustmentTypeSource.Key
            IFinancialAdjustmentTypeSource financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();
            SetupResult.For(financialAdjustmentTypeSource.Key).Return((int)FinancialAdjustmentTypeSources.InterestOnly);

            // Setup applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource
            SetupResult.For(applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource).Return(financialAdjustmentTypeSource);

            // Setup applicationMortgageLoan.CurrentProduct
            IApplicationProductNewVariableLoan applicationProductNewVariableLoan = _mockery.StrictMock<IApplicationProductNewVariableLoan>();
            SetupResult.For(applicationMortgageLoan.CurrentProduct).Return(applicationProductNewVariableLoan);

            // Setup supportsVariableLoanApplicationInformation.VariableLoanInformation
            IApplicationInformationVariableLoan applicationInformationVariableLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(applicationProductNewVariableLoan.VariableLoanInformation).Return(applicationInformationVariableLoan);

            // Setup VariableLoanInformation.LoanAgreementAmount
            SetupResult.For(applicationInformationVariableLoan.LoanAgreementAmount).Return(1800000.0);

            // Setup applicationInformationVariableLoan.LTV
            SetupResult.For(applicationInformationVariableLoan.LTV).Return(85.0);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AttributeInterestOnlyMaxLoanAmountLTV90Fail()
        {
            AttributeInterestOnlyMaxLoanAmountLTV90 rule = new AttributeInterestOnlyMaxLoanAmountLTV90();

            // Setup the correct object to pass along
            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.GetLatestApplicationInformation()
            IApplicationInformation applicationInformation = _mockery.StrictMock<IApplicationInformation>();
            SetupResult.For(applicationMortgageLoan.GetLatestApplicationInformation()).Return(applicationInformation);

            // Setup applicationInformation.ApplicationInformationFinancialAdjustments
            IEventList<IApplicationInformationFinancialAdjustment> applicationInformationFinancialAdjustments = new EventList<IApplicationInformationFinancialAdjustment>();
            IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment = _mockery.StrictMock<IApplicationInformationFinancialAdjustment>();
            applicationInformationFinancialAdjustments.Add(Messages, applicationInformationFinancialAdjustment);
            SetupResult.For(applicationInformation.ApplicationInformationFinancialAdjustments).Return(applicationInformationFinancialAdjustments);

            // Setup financialAdjustmentTypeSource.Key
            IFinancialAdjustmentTypeSource financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();
            SetupResult.For(financialAdjustmentTypeSource.Key).Return((int)FinancialAdjustmentTypeSources.InterestOnly);

            // Setup applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource
            SetupResult.For(applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource).Return(financialAdjustmentTypeSource);

            // Setup applicationMortgageLoan.CurrentProduct
            IApplicationProductNewVariableLoan applicationProductNewVariableLoan = _mockery.StrictMock<IApplicationProductNewVariableLoan>();
            SetupResult.For(applicationMortgageLoan.CurrentProduct).Return(applicationProductNewVariableLoan);

            // Setup supportsVariableLoanApplicationInformation.VariableLoanInformation
            IApplicationInformationVariableLoan applicationInformationVariableLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(applicationProductNewVariableLoan.VariableLoanInformation).Return(applicationInformationVariableLoan);

            // Setup VariableLoanInformation.LoanAgreementAmount
            SetupResult.For(applicationInformationVariableLoan.LoanAgreementAmount).Return(500000.0);

            // Setup applicationInformationVariableLoan.LTV
            SetupResult.For(applicationInformationVariableLoan.LTV).Return(91.0);

            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        #endregion AttributeInterestOnlyMaxLoanAmountLTV90

        #region AttributeInterestOnlyArrears

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AttributeInterestOnlyArrearsSuccess()
        {
            AttributeInterestOnlyArrears rule = new AttributeInterestOnlyArrears();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.InstallmentSummary
            IAccountInstallmentSummary accountInstallmentSummary = _mockery.StrictMock<IAccountInstallmentSummary>();
            SetupResult.For(mortgageLoanAccount.InstallmentSummary).Return(accountInstallmentSummary);

            // Setup accountInstallmentSummary.MonthsInArrears
            SetupResult.For(accountInstallmentSummary.MonthsInArrears).Return(1.5);

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AttributeInterestOnlyArrearsFail()
        {
            AttributeInterestOnlyArrears rule = new AttributeInterestOnlyArrears();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.InstallmentSummary
            IAccountInstallmentSummary accountInstallmentSummary = _mockery.StrictMock<IAccountInstallmentSummary>();
            SetupResult.For(mortgageLoanAccount.InstallmentSummary).Return(accountInstallmentSummary);

            // Setup accountInstallmentSummary.MonthsInArrears
            SetupResult.For(accountInstallmentSummary.MonthsInArrears).Return(2.2);

            ExecuteRule(rule, 1, mortgageLoanAccount);
        }

        #endregion AttributeInterestOnlyArrears

        #region AttributeInterestOnlyRateReset

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AttributeInterestOnlyRateResetSuccess()
        {
            AttributeInterestOnlyRateReset rule = new AttributeInterestOnlyRateReset();

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
        public void AttributeInterestOnlyRateResetFail()
        {
            AttributeInterestOnlyRateReset rule = new AttributeInterestOnlyRateReset();

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

        #endregion AttributeInterestOnlyRateReset

        #region AttributeInterestOnlyUnderCancellation

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AttributeInterestOnlyUnderCancellationSuccess()
        {
            AttributeInterestOnlyUnderCancellation rule = new AttributeInterestOnlyUnderCancellation();

            using (new SessionScope())
            {
                // we need to get hold of an account that is under cancellation
                string hql = "SELECT a FROM AccountNewVariableLoan_DAO a WHERE a.Details.size = 0";
                SimpleQuery<AccountNewVariableLoan_DAO> q = new SimpleQuery<AccountNewVariableLoan_DAO>(hql);
                q.SetQueryRange(1);
                AccountNewVariableLoan_DAO[] accounts = q.Execute();

                if (accounts == null || accounts.Length == 0)
                {
                    Assert.Fail("Unable to find any loans without detail types");
                    return;
                }

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IMortgageLoanAccount mortgageLoanAccount = BMTM.GetMappedType<IAccountNewVariableLoan>(accounts[0]) as IMortgageLoanAccount;
                ExecuteRule(rule, 0, mortgageLoanAccount);
            }
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void AttributeInterestOnlyUnderCancellationFail()
        {
            AttributeInterestOnlyUnderCancellation rule = new AttributeInterestOnlyUnderCancellation();

            using (new SessionScope())
            {
                // we need to get hold of an account that is under cancellation
                string hql = "SELECT a FROM AccountNewVariableLoan_DAO a INNER JOIN a.Details AS d WHERE d.DetailType.Key = ?";
                SimpleQuery<AccountNewVariableLoan_DAO> q = new SimpleQuery<AccountNewVariableLoan_DAO>(hql, (int)DetailTypes.UnderCancellation);
                q.SetQueryRange(1);
                AccountNewVariableLoan_DAO[] accounts = q.Execute();

                if (accounts == null || accounts.Length == 0)
                {
                    Assert.Fail("Unable to find any loans under cancellation");
                    return;
                }
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IMortgageLoanAccount mortgageLoanAccount = BMTM.GetMappedType<IAccountNewVariableLoan>(accounts[0]) as IMortgageLoanAccount;
                ExecuteRule(rule, 1, mortgageLoanAccount);
            }
        }

        #endregion AttributeInterestOnlyUnderCancellation

        #region ProductInterestOnlyOptInLoanTransaction

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ProductInterestOnlyOptInLoanTransactionSuccess()
        {
            ProductInterestOnlyOptInLoanTransaction rule = new ProductInterestOnlyOptInLoanTransaction(RepositoryFactory.GetRepository<ICastleTransactionsService>());

            // Setup the correct object to pass along
            IApplication app = _mockery.StrictMock<IApplication>();

            ParameterCollection parameters = new ParameterCollection();
            IDbConnection con = Helper.GetSQLDBConnection("EWorkConnectionString");
            DataTable DT = new DataTable();
            string query = "select ino.sLoanNumber from [e-work]..InterestOnly ino where ino.bActiveFolder != -1 and ino.sLoanNumber != 0";
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
        public void ProductInterestOnlyOptInLoanTransactionFail()
        {
            ProductInterestOnlyOptInLoanTransaction rule = new ProductInterestOnlyOptInLoanTransaction(RepositoryFactory.GetRepository<ICastleTransactionsService>());

            // Setup the correct object to pass along
            IApplication app = _mockery.StrictMock<IApplication>();

            ParameterCollection parameters = new ParameterCollection();
            IDbConnection con = Helper.GetSQLDBConnection("EWorkConnectionString");
            DataTable DT = new DataTable();
            string query = "select ino.sLoanNumber from [e-work]..InterestOnly ino where ino.bActiveFolder = -1";
            Helper.FillFromQuery(DT, query, con, parameters);

            int accountKey = Convert.ToInt32(DT.Rows[0][0]);

            IAccount acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(acc.Key).Return((int)accountKey);
            SetupResult.For(app.Account).Return(acc);

            ExecuteRule(rule, 1, app);
        }

        #endregion ProductInterestOnlyOptInLoanTransaction
    }
}