using System;
using SAHL.Common.Collections.Interfaces;
using NUnit.Framework;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Rules.Products;

namespace SAHL.Common.BusinessModel.Rules.Test.Products
{
    [TestFixture]
    public class ThirtyYearTerm: RuleBase
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

        #region AttributeThirtyTermVarifixRule

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void AttributeThirtyTermVarifixRuleNoArgumentsPassed()
        {
            AttributeThirtyTermVarifixRule rule = new AttributeThirtyTermVarifixRule();
            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void AttributeThirtyTermVarifixRuleIncorrectArgumentsPassed()
        {
            AttributeThirtyTermVarifixRule rule = new AttributeThirtyTermVarifixRule();

            // Setup an incorrect Argumnt to pass along
            IApplication application = _mockery.StrictMock<IApplication>();
            ExecuteRule(rule, 0, application);

        }

        /// <summary>
        /// Test expects the rule to fail if a Varifix Account has both a 30 year term.
        /// </summary>
        [NUnit.Framework.Test]
        public void AttributeThirtyTermVarifixRuleFail30TermPlusVarifixOnIAccountVariFixLoan()
        {
            AttributeThirtyTermVarifixRule rule = new AttributeThirtyTermVarifixRule();

            // Setup an incorrect Argumnt to pass along
            IAccountVariFixLoan accountVariFixLoan = _mockery.StrictMock<IAccountVariFixLoan>();

            // Setup accountVariFixLoan.SecuredMortgageLoan (Assuming that the 30 year mortgage is indicated by the term of 30*12 months)
            IMortgageLoan mortgageLoan = _mockery.StrictMock<IMortgageLoan>();
            SetupResult.For(mortgageLoan.InitialInstallments).Return(Convert.ToInt16(360));

            // Setup accountVariFixLoan.SecuredMortgageLoan
            SetupResult.For(accountVariFixLoan.SecuredMortgageLoan).Return(mortgageLoan);

            ExecuteRule(rule, 1, accountVariFixLoan);

        }

        /// <summary>
        /// Test expects the rule to fail if a Varifix Application has both a 30 year term.
        /// </summary>
        [NUnit.Framework.Test]
        public void AttributeThirtyTermVarifixRuleFail30TermPlusVarifixOnIApplicationProductVariFixLoan()
        {
            AttributeThirtyTermVarifixRule rule = new AttributeThirtyTermVarifixRule();

            // Setup an incorrect Argumnt to pass along
            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();
            IApplicationProductVariFixLoan applicationProductVariFixLoan = _mockery.StrictMock<IApplicationProductVariFixLoan>();
            SetupResult.For(app.CurrentProduct).Return(applicationProductVariFixLoan);

            // Setup applicationInformationVariableLoan.Term
            IApplicationInformationVariableLoan applicationInformationVariableLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(applicationProductVariFixLoan.Term).Return(360);

            ExecuteRule(rule, 1, app);

        }

        #endregion

        #region Attribute30Term

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void Attribute30TermNoArgumentsPassed()
        {
            Attribute30Term rule = new Attribute30Term();
            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void Attribute30TermWrongArgumentsPassed()
        {
            Attribute30Term rule = new Attribute30Term();

            // Setup the wrong arguments.
            IAccountInformation arbAccount = _mockery.StrictMock<IAccountInformation>();
            ExecuteRule(rule, 0, arbAccount);
        }

        /// <summary>
        /// Test expects the rule to fail when an IMortgageLoan with InitialInstallments greater than 360 is passed.
        /// </summary>
        [NUnit.Framework.Test]
        public void Attribute30TermLoanTermTooHigh()
        {
            Attribute30Term rule = new Attribute30Term();

            // Setup the IMortgageLoan.InitialInstallments.
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.FinancialServices
            IMortgageLoan financialService = _mockery.StrictMock<IMortgageLoan>();
            IEventList<IFinancialService> financialServices = new EventList<IFinancialService>();
            financialServices.Add(Messages, financialService);
            SetupResult.For(mortgageLoanAccount.FinancialServices).Return(financialServices);

            // Setup financialService.InitialInstallments
            SetupResult.For(financialService.InitialInstallments).Return(Convert.ToInt16( 12 *31));

            ExecuteRule(rule, 1, mortgageLoanAccount);

        }

        /// <summary>
        /// Test expects the rule to fail when an IMortgageLoan with InitialInstallments greater than 360 is passed.
        /// </summary>
        [NUnit.Framework.Test]
        public void Attribute30TermLoanTermCorrect()
        {
            Attribute30Term rule = new Attribute30Term();

            // Setup the IMortgageLoan.InitialInstallments.
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.FinancialServices
            IMortgageLoan financialService = _mockery.StrictMock<IMortgageLoan>();
            IEventList<IFinancialService> financialServices = new EventList<IFinancialService>();
            financialServices.Add(Messages, financialService);
            SetupResult.For(mortgageLoanAccount.FinancialServices).Return(financialServices);

            // Setup financialService.InitialInstallments
            SetupResult.For(financialService.InitialInstallments).Return(Convert.ToInt16(12.0 * 30));

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        #endregion

    }
}
