using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Test;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Rules.Products;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Rules.Test.Products
{
	[TestFixture]
	public class NewVariable : RuleBase
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

		/// <summary>
		/// Ensure that the Minimum Loan Amount > Current Loan Amount
		/// </summary>
		[NUnit.Framework.Test]
		public void NewVariableMinimumLoanAmountMinLoanAmountMoreThanCurrentLoanAmount()
		{
			//Pass
			NewVariableMinimumLoanHelper(140000, 100000, 0);

			//Fail
			NewVariableMinimumLoanHelper(100000, 140000, 1);
		}

		/// <summary>
		/// Ensure that the Application Product is of ProductNewVariableLoan
		/// </summary>
		[NUnit.Framework.Test]
		public void NewVariableMinimumLoanAmountEnsureProductNewVariableLoanSuccess()
		{
			double loanAgreementAmount = 1000000;

			//Setup the rule
			NewVariableMinimumLoanAmount rule = new NewVariableMinimumLoanAmount();

			//Setup the Application Scenario
			IApplication application = _mockery.StrictMock<IApplication>();
			IApplicationProductNewVariableLoan newVariableLoan = _mockery.StrictMock<IApplicationProductNewVariableLoan>();
			SetupResult.For(application.CurrentProduct).Return(newVariableLoan);

			//1 Million for Loan Agreement Amount
			SetupResult.For(newVariableLoan.LoanAgreementAmount).Return(loanAgreementAmount);

			ExecuteRule(rule, 0, application);
		}

		/// <summary>
		/// New Variable Minimum Loan Helper
		/// </summary>
		/// <param name="loanAgreementAmount"></param>
		/// <param name="minimumLoanAmount"></param>
		/// <param name="expectedErrorCount">
		public void NewVariableMinimumLoanHelper(double loanAmount, double minimumLoanAmount, int expectedErrorCount)
		{
			//Setup the rule
			NewVariableMinimumLoanAmount rule = new NewVariableMinimumLoanAmount();

			//Setup the Application Scenario
			IApplication application = _mockery.StrictMock<IApplication>();
			IApplicationProductNewVariableLoan newVariableLoan = _mockery.StrictMock<IApplicationProductNewVariableLoan>();
			SetupResult.For(application.CurrentProduct).Return(newVariableLoan);

			//1 Million for Loan Agreement Amount
			SetupResult.For(newVariableLoan.LoanAgreementAmount).Return(loanAmount);

			IRuleRepository RuleRepo = _mockery.StrictMock<IRuleRepository>();

			MockCache.Add(typeof(IRuleRepository).ToString(), RuleRepo);
			IRuleItem ruleItem = _mockery.StrictMock<IRuleItem>();

			SetupResult.For(RuleRepo.FindRuleItemByTypeName("")).IgnoreArguments().Return(ruleItem);

			// Setup RuleItem.Parameters
			IEventList<IRuleParameter> ruleParameters = new EventList<IRuleParameter>();
			IRuleParameter ruleParameter = _mockery.StrictMock<IRuleParameter>();

			//The Min Loan Amount > Loan Agreement Amount
			SetupResult.For(ruleParameter.Name).Return("@MinimumLoanAmount");
			SetupResult.For(ruleParameter.Value).Return(minimumLoanAmount.ToString());
			ruleParameters.Add(Messages, ruleParameter);
			SetupResult.For(ruleItem.RuleParameters).Return(ruleParameters);

			ExecuteRule(rule, expectedErrorCount, application);
		}

		/// <summary>
		/// Ensure that the Current Loan Amount > Maximum Loan Amount
		/// </summary>
		[NUnit.Framework.Test]
		public void NewVariableMaximumLoanAmountMinLoanAmountMoreThanCurrentLoanAmount()
		{
			//Pass
			NewVariableMaximumLoanHelper(5000000, 7000000, 0);

			//Fail
			NewVariableMaximumLoanHelper(7000000, 5000000, 1);
		}

		/// <summary>
		/// Ensure that the Application Product is of ProductNewVariableLoan
		/// </summary>
		[NUnit.Framework.Test]
		public void NewVariableMaximumLoanAmountEnsureProductNewVariableLoanSuccess()
		{
			double loanAgreementAmount = 1000000;

			//Setup the rule
			NewVariableMaximumLoanAmount rule = new NewVariableMaximumLoanAmount();

			//Setup the Application Scenario
			IApplication application = _mockery.StrictMock<IApplication>();
			IApplicationProductNewVariableLoan newVariableLoan = _mockery.StrictMock<IApplicationProductNewVariableLoan>();
			SetupResult.For(application.CurrentProduct).Return(newVariableLoan);

			//1 Million for Loan Agreement Amount
			SetupResult.For(newVariableLoan.LoanAgreementAmount).Return(loanAgreementAmount);

			ExecuteRule(rule, 0, application);
		}

		/// <summary>
		/// New Variable Maximum Loan Helper
		/// </summary>
		/// <param name="loanAgreementAmount"></param>
		/// <param name="maximumLoanAmount"></param>
		/// <param name="expectedErrorCount">
		public void NewVariableMaximumLoanHelper(double loanAmount, double maximumLoanAmount, int expectedErrorCount)
		{
			//Setup the rule
			NewVariableMaximumLoanAmount rule = new NewVariableMaximumLoanAmount();

			//Setup the Application Scenario
			IApplication application = _mockery.StrictMock<IApplication>();
			IApplicationProductNewVariableLoan newVariableLoan = _mockery.StrictMock<IApplicationProductNewVariableLoan>();
			SetupResult.For(application.CurrentProduct).Return(newVariableLoan);

			//1 Million for Loan Agreement Amount
			SetupResult.For(newVariableLoan.LoanAgreementAmount).Return(loanAmount);

			IRuleRepository RuleRepo = _mockery.StrictMock<IRuleRepository>();

			MockCache.Add(typeof(IRuleRepository).ToString(), RuleRepo);
			IRuleItem ruleItem = _mockery.StrictMock<IRuleItem>();

			SetupResult.For(RuleRepo.FindRuleItemByTypeName("")).IgnoreArguments().Return(ruleItem);

			// Setup RuleItem.Parameters
			IEventList<IRuleParameter> ruleParameters = new EventList<IRuleParameter>();
			IRuleParameter ruleParameter = _mockery.StrictMock<IRuleParameter>();

			//The Min Loan Amount > Loan Agreement Amount
			SetupResult.For(ruleParameter.Name).Return("@MaximumLoanAmount");
			SetupResult.For(ruleParameter.Value).Return(maximumLoanAmount.ToString());
			ruleParameters.Add(Messages, ruleParameter);
			SetupResult.For(ruleItem.RuleParameters).Return(ruleParameters);

			ExecuteRule(rule, expectedErrorCount, application);
		}
	}
}
