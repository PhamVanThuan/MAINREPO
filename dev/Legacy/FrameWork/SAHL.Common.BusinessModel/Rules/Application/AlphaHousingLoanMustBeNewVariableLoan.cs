using System;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;
using System.Linq;

namespace SAHL.Common.BusinessModel.Rules.Application
{
	[RuleDBTag("AlphaHousingLoanMustBeNewVariableLoan",
	"If a rule is alpha housing then it must be standard new variable",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Application.Application.AlphaHousingLoanMustBeNewVariableLoan")]
	public class AlphaHousingLoanMustBeNewVariableLoan : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
		{
			var application = parameters[0] as IApplication;
            if (application is IApplicationFurtherLending)
                return 1;
			if (application.IsAlphaHousing())
			{
				if (application.ProductHistory[application.ProductHistory.Length - 1].ProductType != Globals.Products.NewVariableLoan)
				{
					string errorMessage = "Only New Variable Loan products qualify for Alpha Housing.";
					AddMessage(errorMessage, errorMessage, messages);
				}
			}

			return 1;
		}
	}

	[RuleDBTag("CalculatorAlphaHousingLoanMustBeNewVariableLoan",
	"If a rule is alpha housing then it must be standard new variable (runs in the context of the calculator, before we have an application)",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Application.Application.CalculatorAlphaHousingLoanMustBeNewVariableLoan")]
	public class CalculatorAlphaHousingLoanMustBeNewVariableLoan : BusinessRuleBase
	{
		private readonly IApplicationRepository applicationRepository;

		public CalculatorAlphaHousingLoanMustBeNewVariableLoan(IApplicationRepository applicationRepository)
		{
			this.applicationRepository = applicationRepository;
		}

		public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
		{
			int productKey = Convert.ToInt16(parameters[0]);
			double ltv = Convert.ToDouble(parameters[1]);
			double householdIncome = Convert.ToDouble(parameters[2]);
			int employmentTypeKey = Convert.ToInt16(parameters[3]);
			if (applicationRepository.IsAlphaHousingLoan(ltv, employmentTypeKey, householdIncome))
			{
				if (productKey != (int)Globals.Products.NewVariableLoan)
				{
					string errorMessage = "Only New Variable Loan products qualify for Alpha Housing.";
					AddMessage(errorMessage, errorMessage, messages);
				}
			}

			return 1;
		}
	}

	[RuleDBTag("CalculatorAlphaHousingLoanMustNotBeInterestOnlyLoan",
	"Checks if an alpha housing loan is not interest only loan (runs in the context of the calculator, before we have an application)",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Application.Application.CalculatorAlphaHousingLoanMustNotBeInterestOnlyLoan")]
	public class CalculatorAlphaHousingLoanMustNotBeInterestOnlyLoan : BusinessRuleBase
	{
		private readonly IApplicationRepository applicationRepository;

		public CalculatorAlphaHousingLoanMustNotBeInterestOnlyLoan(IApplicationRepository applicationRepository)
		{
			this.applicationRepository = applicationRepository;
		}

		public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
		{
			int productKey = Convert.ToInt16(parameters[0]);
			double ltv = Convert.ToDouble(parameters[1]);
			double householdIncome = Convert.ToDouble(parameters[2]);
			int employmentTypeKey = Convert.ToInt16(parameters[3]);
			bool interestOnly = Convert.ToBoolean(parameters[4]);

			if (applicationRepository.IsAlphaHousingLoan(ltv, employmentTypeKey, householdIncome))
			{
				if (interestOnly)
				{
					string errorMessage = "Alpha Housing cannot have interest only.";
					AddMessage(errorMessage, errorMessage, messages);
				}
			}

			return 1;
		}
	}

	[RuleDBTag("AlphaHousingLoanMustNotBeInterestOnlyLoan",
   "Checks if an alpha housing loan is not interest only loan",
   "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Application.Application.AlphaHousingLoanMustNotBeInterestOnlyLoan")]
	public class AlphaHousingLoanMustNotBeInterestOnlyLoan : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
		{
			var application = parameters[0] as IApplication;
            if (application is IApplicationFurtherLending)
                return 1;
			if(application.IsAlphaHousing())
			{
				if (application.HasFinancialAdjustment(Globals.FinancialAdjustmentTypeSources.InterestOnly))
				{
					string errorMessage = "Alpha Housing cannot have interest only.";
					AddMessage(errorMessage, errorMessage, messages);
				}
			}

			return 1;
		}
	}
}