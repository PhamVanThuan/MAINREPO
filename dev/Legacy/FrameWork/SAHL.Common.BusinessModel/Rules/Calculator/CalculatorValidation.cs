using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Repositories;
using System.Linq;

namespace SAHL.Common.BusinessModel.Rules.Calculator
{
	/// <summary>
	/// The cash amount required exceeds calculated further lending limit of ???
	/// </summary>
	[RuleInfo]
	[RuleDBTag("ExceedFurtherLendingLimit",
	"The cash amount required exceeds calculated further lending limit of ???",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.ExceedFurtherLendingLimit")]
	public class ExceedFurtherLendingLimit : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			double amount = (double)Parameters[0];
			string err = "The cash amount required exceeds calculated further lending limit of " + (Math.Round(amount, 2)).ToString(SAHL.Common.Constants.CurrencyFormat);
			AddMessage(err, err, Messages);

			return 0;
		}
	}

	[RuleDBTag("CalcCreditDisqualificationMaxLTV",
	"CalcCreditDisqualificationMaxLTV rules",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationMaxLTV")]
	[RuleInfo]
	public class CalcCreditDisqualificationMaxLTV : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
			IControl ctrl;

			double amount = (double)Parameters[0];

			ctrl = ctrlRepo.GetControlByDescription("Calc - maxLTV");
			if (amount * 100 > (ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric.Value) : 0))
			{
				string err = ctrl.ControlText + " LTV is " + amount.ToString(SAHL.Common.Constants.RateFormat) + ". The maximum allowed is: " + (ctrl.ControlNumeric.HasValue ? ctrl.ControlNumeric.Value / 100 : 0).ToString(SAHL.Common.Constants.RateFormat) + ".";
				AddMessage(err, err, Messages);
			}

			return 1;

		}
	}

	[RuleDBTag("CalcCreditDisqualificationMaxPTI",
	"CalcCreditDisqualificationMaxPTI rules",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationMaxPTI")]
	[RuleInfo]
	public class CalcCreditDisqualificationMaxPTI : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
			IControl ctrl;

			double amount = (double)Parameters[0];

			ctrl = ctrlRepo.GetControlByDescription("Calc - maxPTI");
			if (amount * 100 > (ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric.Value) : 0))
			{
				string err = ctrl.ControlText + " PTI is " + amount.ToString(SAHL.Common.Constants.RateFormat) + ". The maximum allowed is: " + (ctrl.ControlNumeric.HasValue ? ctrl.ControlNumeric.Value / 100 : 0).ToString(SAHL.Common.Constants.RateFormat) + ".";
				AddMessage(err, err, Messages);
			}

			return 1;

		}
	}

	[RuleDBTag("CalcCreditDisqualificationMinIncome",
	"CalcCreditDisqualificationMinIncome rules",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationMinIncome")]
	[RuleInfo]
	public class CalcCreditDisqualificationMinIncome : BusinessRuleBase
	{
		private ICreditMatrixRepository creditMatrixRepository;
		public CalcCreditDisqualificationMinIncome(ICreditMatrixRepository creditMatrixRepository)
		{
			this.creditMatrixRepository = creditMatrixRepository;
		}

		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			double amount = (double)Parameters[0];

			var creditMatrix = creditMatrixRepository.GetCreditMatrix(Globals.OriginationSources.SAHomeLoans);
			var exceptionsCreditCriteria = creditMatrix.CreditCriterias.Where(x => x.ExceptionCriteria == true);

			var minimumIncomeAmount = exceptionsCreditCriteria.Min(x => x.MinIncomeAmount);

			if (amount < minimumIncomeAmount)
			{
				string err = "Income is " + amount.ToString(SAHL.Common.Constants.CurrencyFormat) + ". The minimum allowed is: " + minimumIncomeAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) + ".";
				AddMessage(err, err, Messages);
			}

			return 1;
		}
	}

	[RuleDBTag("CalcCreditDisqualificationMinLAA",
	"CalcCreditDisqualificationMinLAA rules",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationMinLAA")]
	[RuleInfo]
	public class CalcCreditDisqualificationMinLAA : BusinessRuleBase
	{
		private ICreditMatrixRepository creditMatrixRepository;

		public CalcCreditDisqualificationMinLAA(ICreditMatrixRepository creditMatrixRepository)
		{
			this.creditMatrixRepository = creditMatrixRepository;
		}

		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();

			double amount = (double)Parameters[0];

			var creditMatrix = creditMatrixRepository.GetCreditMatrix(Globals.OriginationSources.SAHomeLoans);
			var exceptionsCreditCriteria = creditMatrix.CreditCriterias.Where(x => x.ExceptionCriteria == true);

			var minimumLoanAmount = exceptionsCreditCriteria.Min(x => x.MinLoanAmount);

			if (amount < (minimumLoanAmount.HasValue ? Convert.ToDouble(minimumLoanAmount.Value) : 0))
			{
				var errorMessage = String.Format("Loan amount is {0}. The minimum allowed is: {1}.", amount.ToString(SAHL.Common.Constants.CurrencyFormat),
																									 minimumLoanAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat));
				AddMessage(errorMessage, errorMessage, Messages);
			}

			return 1;
		}
	}

	[RuleDBTag("CalcCreditDisqualificationMinLAFL",
	"CalcCreditDisqualificationMinLAFL rules",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationMinLAFL")]
	[RuleInfo]
	public class CalcCreditDisqualificationMinLAFL : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
			IControl ctrl;

			double amount = (double)Parameters[0];

			ctrl = ctrlRepo.GetControlByDescription("Calc - minFurtherLendingLAA");
			if (amount < (ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric.Value) : 0))
			{
				string err = ctrl.ControlText + " Loan amount is " + amount.ToString(SAHL.Common.Constants.CurrencyFormat) + ". The minimum allowed is: " + (ctrl.ControlNumeric.HasValue ? ctrl.ControlNumeric.Value : 0).ToString(SAHL.Common.Constants.CurrencyFormat) + ".";
				AddMessage(err, err, Messages);
			}

			return 1;

		}
	}

	[RuleDBTag("CalcCreditDisqualificationMaxLAA",
	"CalcCreditDisqualificationMaxLAA rules",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationMaxLAA")]
	[RuleInfo]
	public class CalcCreditDisqualificationMaxLAA : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
			IControl ctrl;

			double amount = (double)Parameters[0];

			ctrl = ctrlRepo.GetControlByDescription("Calc - maxLAA");
			if (amount > (ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric.Value) : 0))
			{
				string err = ctrl.ControlText + " Loan amount is " + amount.ToString(SAHL.Common.Constants.CurrencyFormat) + ". The maximum allowed is: " + (ctrl.ControlNumeric.HasValue ? ctrl.ControlNumeric.Value : 0).ToString(SAHL.Common.Constants.CurrencyFormat) + ".";
				AddMessage(err, err, Messages);
			}

			return 1;
		}
	}

	[RuleDBTag("CalcCreditDisqualificationMinValuation",
	"CalcCreditDisqualificationMinValuation rules",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationMinValuation")]
	[RuleInfo]
	public class CalcCreditDisqualificationMinValuation : BusinessRuleBase
	{
		private ICreditMatrixRepository creditMatrixRepository;

		public CalcCreditDisqualificationMinValuation(ICreditMatrixRepository creditMatrixRepository)
		{
			this.creditMatrixRepository = creditMatrixRepository;
		}
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			double amount = (double)Parameters[0];

			var creditMatrix = creditMatrixRepository.GetCreditMatrix(Globals.OriginationSources.SAHomeLoans);
			var exceptionsCreditCriteria = creditMatrix.CreditCriterias.Where(x => x.ExceptionCriteria == true);
			var minimumValuationAmount = exceptionsCreditCriteria.Min(x => x.MinPropertyValue);

			if (amount < minimumValuationAmount)
			{
				var ruleMessage = String.Format("The valuation amount is {0}. The minimum valuation amount allowed is {1}", amount.ToString(Constants.CurrencyFormat), minimumValuationAmount.Value.ToString(Constants.CurrencyFormat));
				AddMessage(ruleMessage, ruleMessage, Messages);
			}

			return 1;

		}
	}

	[RuleDBTag("CalcCreditDisqualificationEmploymentType",
	"CalcCreditDisqualificationEmploymentType rules",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.CalcCreditDisqualificationEmploymentType")]
	[RuleInfo]
	public class CalcCreditDisqualificationEmploymentType : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			int key = (int)Parameters[0];

			if (key == (int)SAHL.Common.Globals.EmploymentTypes.Unemployed || key == (int)SAHL.Common.Globals.EmploymentTypes.Unknown)
			{
				string err = "Employment type can not be Unemployed or Unknown";
				AddMessage(err, err, Messages);
			}

			return 1;

		}
	}


	/// <summary>
	/// The cash amount required must be less than MaxLAA - currentBalance
	/// </summary>
	[RuleInfo]
	[RuleDBTag("ExceedFurtherLendingLimitLAA",
	"The cash amount required must be less than MaxLAA - currentBalance",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.ExceedFurtherLendingLimitLAA")]
	public class ExceedFurtherLendingLimitLAA : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			double amount = (double)Parameters[0];
			string err = "The cash amount required must be less than " + (Math.Round(amount, 2)).ToString(SAHL.Common.Constants.CurrencyFormat);
			AddMessage(err, err, Messages);

			return 0;
		}
	}

	/// <summary>
	/// Readvance requested is more than the allowed 
	///Warning //*** we dont want to stop the process we just want to show the problem...
	/// </summary>
	[RuleInfo]
	[RuleDBTag("ExceedReadvanceLimit",
	"Readvance requested is more than the allowed ",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.ExceedReadvanceLimit")]
	public class ExceedReadvanceLimit : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			double amount = (double)Parameters[0];
			string err = "Readvance requested is more than the allowed " + (Math.Round(amount, 2)).ToString(SAHL.Common.Constants.CurrencyFormat);
			AddMessage(err, err, Messages);

			return 0;
		}
	}

	/// <summary>
	/// Further advance requested is more than the allowed 
	///Warning //*** we dont want to stop the process we just want to show the problem...
	/// </summary>
	[RuleInfo]
	[RuleDBTag("ExceedFurtherAdvanceLimit",
	"Further advance requested is more than the allowed ",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.ExceedFurtherAdvanceLimit")]
	public class ExceedFurtherAdvanceLimit : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			double amount = (double)Parameters[0];
			string err = "Further advance requested is more than the allowed " + (Math.Round(amount, 2)).ToString(SAHL.Common.Constants.CurrencyFormat);
			AddMessage(err, err, Messages);

			return 0;
		}
	}

	/// <summary>
	/// Further Loan requested is more than the allowed 
	///Warning //*** we dont want to stop the process we just want to show the problem...
	/// </summary>
	[RuleInfo]
	[RuleDBTag("ExceedFurtherLoanLimit",
	"Further loan requested is more than the allowed ",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.ExceedFurtherLoanLimit")]
	public class ExceedFurtherLoanLimit : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			double amount = (double)Parameters[0];
			string err = "Further loan requested is more than the allowed " + (Math.Round(amount, 2)).ToString(SAHL.Common.Constants.CurrencyFormat);
			AddMessage(err, err, Messages);

			return 0;
		}
	}

	/// <summary>
	/// No further loan is required but a bond to register has been entered. 
	///Warning //*** we dont want to stop the process we just want to show the problem...
	/// </summary>
	[RuleInfo]
	[RuleDBTag("NoBondRequired",
	"No further loan is required but a bond to register has been entered. ",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.NoBondRequired")]
	public class NoBondRequired : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			string err = "No further loan is required but a bond to register has been entered.";
			AddMessage(err, err, Messages);

			return 0;
		}
	}

	/// <summary>
	/// No further loan is required but a bond to register has been entered. 
	///Warning //*** we dont want to stop the process we just want to show the problem...
	/// </summary>
	[RuleInfo]
	[RuleDBTag("BondLessThanFurtherLoan",
	"The Bond amount can not be less than the further loan amount.",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.BondLessThanFurtherLoan")]
	public class BondLessThanFurtherLoan : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			string err = "The Bond amount can not be less than the further loan amount.";
			AddMessage(err, err, Messages);

			return 0;
		}
	}

	/// <summary>
	/// Fixed amount less than the minimum allowed.
	/// </summary>
	[RuleInfo]
	[RuleDBTag("VarifixMinimumFixAmount",
	"Fixed amount less than the minimum allowed.",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.VarifixMinimumFixAmount")]
	public class VarifixMinimumFixAmount : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			double fixAmount = (double)Parameters[0];
			double minFixAmount = (double)Parameters[1];

			string err = "The fixed amount is " + fixAmount.ToString(SAHL.Common.Constants.CurrencyFormat) + ", less than the minimum R " + minFixAmount + ".";
			AddMessage(err, err, Messages);

			return 0;
		}
	}

	/// <summary>
	/// Min loan amount for Varifix is R
	/// </summary>
	[RuleInfo]
	[RuleDBTag("VarifixMinimumLoanAmount",
	"Min loan amount for Varifix is R",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.VarifixMinimumLoanAmount")]
	public class VarifixMinimumLoanAmount : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			double minFixTotalLoanAmount = (double)Parameters[0];

			string err = "Minimum loan amount for Varifix product is R " + minFixTotalLoanAmount + ".";
			AddMessage(err, err, Messages);

			return 0;
		}
	}

	/// <summary>
	/// Refinance Cash out must be greater than R 
	/// </summary>
	[RuleInfo]
	[RuleDBTag("RefinanceMinimumCashOut",
	"Refinance Cash out must be greater than R",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.RefinanceMinimumCashOut")]
	public class RefinanceMinimumCashOut : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			double minPurchasePrice = (double)Parameters[0];

			string err = "Cash out must be greater than R " + minPurchasePrice + ".";
			AddMessage(err, err, Messages);

			return 0;
		}
	}

	/// <summary>
	/// Switch Current loan must be greater than R 0.00
	/// </summary>
	[RuleInfo]
	[RuleDBTag("SwitchCurrentLoanAmountMinimum",
	"Switch Current loan must be greater than R 0.00",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.SwitchCurrentLoanAmountMinimum")]
	public class SwitchCurrentLoanAmountMinimum : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			string err = "Current loan must be greater than R 0.00";
			AddMessage(err, err, Messages);

			return 0;
		}
	}

	/// <summary>
	/// Checks if there are Title Deeds on File - warning
	/// </summary>
	[RuleInfo]
	[RuleDBTag("TitleDeedsOnFile",
	"Are there title deeds on file",
	"SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Calculator.TitleDeedsOnFile", false)]
	public class TitleDeedsOnFile : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			int accountKey = (int)Parameters[0];

			IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

			//bool titleDeedsOnFile = accRepo.TitleDeedsOnFile(accountKey);

			if (!accRepo.TitleDeedsOnFile(accountKey))
			{
				string err = "There are currently no Title Deeds On File.";
				AddMessage(err, err, Messages);
				return 1;
			}
			return 0;
		}
	}

	/// <summary>
	/// Warning when LTP exceeds 85%, to run for a period of 12 months from date of registration.  - warning
	/// </summary>
	[RuleInfo]
	[RuleDBTag("CheckMaxLTP",
	"Warning when LTP exceeds 85%, to run for a period of 12 months from date of registration.",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Calculator.CheckMaxLTP", false)]
	[RuleParameterTag(new string[] { "@MaxLTP,0.85,7", "@MonthsLTPValid, 12, 9" })]
	public class CheckMaxLTP : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			double LTP = (double)Parameters[0];
			DateTime openDate = (DateTime)Parameters[1];

			double maxLTP = Convert.ToDouble(RuleItem.RuleParameters[0].Value);
			int monthsLTPValid = Convert.ToInt32(RuleItem.RuleParameters[1].Value);

			DateTime includeDate = DateTime.Today.AddMonths(-(monthsLTPValid));
			if (openDate > includeDate && LTP > maxLTP)
			{
				string err = String.Format("The LTP is greater than {0}%", maxLTP);
				AddMessage(err, err, Messages);
				return 1;
			}

			return 0;
		}
	}

}
