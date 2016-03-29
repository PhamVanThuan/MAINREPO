using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.Application.Credit
{
	[RuleDBTag("CreditDisqualificationMaxLTV",
   "CreditDisqualificationMaxLTV rules",
	"SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMaxLTV")]
	[RuleInfo]
	public class CreditDisqualificationMaxLTV : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (!(Parameters[0] is IApplication))
				throw new ArgumentException("Parameters[0] is not of type IApplication.");

			IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
			IControl ctrl;
			string err;

			IApplication application = Parameters[0] as IApplication;
			IApplicationProduct appProd = application.CurrentProduct;

			ISupportsVariableLoanApplicationInformation vli = appProd as ISupportsVariableLoanApplicationInformation;
			IApplicationInformationVariableLoan aivl = vli.VariableLoanInformation;

			if (aivl != null)
			{
				ctrl = ctrlRepo.GetControlByDescription("Calc - maxLTV");
				if ((aivl.LTV.HasValue ? (aivl.LTV.Value * 100) : 0) > (ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric.Value) : 0))
				{
					err = ctrl.ControlText + " LTV is " + (aivl.LTV.HasValue ? (aivl.LTV.Value) : 0).ToString(SAHL.Common.Constants.RateFormat) + ". The maximum allowed is: " + (ctrl.ControlNumeric.HasValue ? ctrl.ControlNumeric.Value / 100 : 0).ToString(SAHL.Common.Constants.RateFormat) + ".";
					AddMessage(err, err, Messages);
				}
			}
			else
				AddMessage("Application does not have variable information.", "Application does not have variable information.", Messages);

			return 1;
		}
	}

	[RuleDBTag("CreditDisqualificationMaxPTI",
	"CreditDisqualificationMaxPTI rules",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMaxPTI")]
	[RuleInfo]
	public class CreditDisqualificationMaxPTI : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (!(Parameters[0] is IApplication))
				throw new ArgumentException("Parameters[0] is not of type IApplication.");

			IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
			IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
			IControl ctrl;
			string err;

			IApplication application = Parameters[0] as IApplication;
			IApplicationProduct appProd = application.CurrentProduct;

			ISupportsVariableLoanApplicationInformation vli = appProd as ISupportsVariableLoanApplicationInformation;
			IApplicationInformationVariableLoan aivl = vli.VariableLoanInformation;

			if (aivl != null)
			{
				ctrl = ctrlRepo.GetControlByDescription("Calc - maxPTICredit");
				if ((aivl.PTI.HasValue ? (aivl.PTI.Value * 100) : 0) > (ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric.Value) : 0))
				{
					err = ctrl.ControlText + " PTI is " + (aivl.PTI.HasValue ? (aivl.PTI.Value) : 0).ToString(SAHL.Common.Constants.RateFormat) + ". The maximum allowed is: " + (ctrl.ControlNumeric.HasValue ? ctrl.ControlNumeric.Value / 100 : 0).ToString(SAHL.Common.Constants.RateFormat) + ".";
					AddMessage(err, err, Messages);
				}
			}
			else
				AddMessage("Application does not have variable information.", "Application does not have variable information.", Messages);

			return 1;
		}
	}

	[RuleDBTag("CreditDisqualificationMinIncome",
	"CreditDisqualificationMinIncome rules",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinIncome")]
	[RuleInfo]
	public class CreditDisqualificationMinIncome : BusinessRuleBase
	{
		private ICreditMatrixRepository creditMatrixRepository;

		public CreditDisqualificationMinIncome(ICreditMatrixRepository creditMatrixRepository)
		{
			this.creditMatrixRepository = creditMatrixRepository;
		}

		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (!(Parameters[0] is IApplication))
				throw new ArgumentException("Parameters[0] is not of type IApplication.");

			IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
			string error;

			IApplication application = Parameters[0] as IApplication;
			IApplicationProduct appProd = application.CurrentProduct;

			ISupportsVariableLoanApplicationInformation vli = appProd as ISupportsVariableLoanApplicationInformation;
			IApplicationInformationVariableLoan aivl = vli.VariableLoanInformation;

			if (aivl != null)
			{
				var creditMatrix = creditMatrixRepository.GetCreditMatrix(Globals.OriginationSources.SAHomeLoans);
				var exceptionsCreditCriteria = creditMatrix.CreditCriterias.Where(x => x.ExceptionCriteria == false);

				var minimumIncomeAmount = exceptionsCreditCriteria.Min(x => x.MinIncomeAmount);

				if ((aivl.HouseholdIncome.HasValue ? aivl.HouseholdIncome.Value : 0) < (minimumIncomeAmount))
				{
					error = "Income is " + (aivl.HouseholdIncome.HasValue ? aivl.HouseholdIncome.Value : 0).ToString(SAHL.Common.Constants.CurrencyFormat) + ". The minimum allowed is: " + minimumIncomeAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) + ".";
					AddMessage(error, error, Messages);
				}
			}
			else
				AddMessage("Application does not have variable information.", "Application does not have variable information.", Messages);

			return 1;
		}
	}

	[RuleDBTag("CreditDisqualificationMinLoanAgreeAmount",
	"CreditDisqualificationMinLoanAgreeAmount rules",
	"SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinLoanAgreeAmount", false)]
	[RuleInfo]
	public class CreditDisqualificationMinLoanAgreeAmount : BusinessRuleBase
	{
		private ICreditMatrixRepository creditMatrixRepository;
		private IApplicationRepository applicationRepository;

		public CreditDisqualificationMinLoanAgreeAmount(ICreditMatrixRepository creditMatrixRepository, IApplicationRepository applicationRepository)
		{
			this.creditMatrixRepository = creditMatrixRepository;
			this.applicationRepository = applicationRepository;
		}

		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (!(Parameters[0] is IApplication))
				throw new ArgumentException("Parameters[0] is not of type IApplication.");

			string errorMessage;

			IApplication application = Parameters[0] as IApplication;
			IApplicationProduct applicationProduct = application.CurrentProduct;

			ISupportsVariableLoanApplicationInformation variableLoanInformation = applicationProduct as ISupportsVariableLoanApplicationInformation;
			IApplicationInformationVariableLoan applicationInformationVariableLoan = variableLoanInformation.VariableLoanInformation;

			if (application.ApplicationType.Key == (int)OfferTypes.FurtherLoan ||
				application.ApplicationType.Key == (int)OfferTypes.FurtherAdvance ||
				application.ApplicationType.Key == (int)OfferTypes.ReAdvance)
				return 1;

			var creditMatrix = creditMatrixRepository.GetCreditMatrix(Globals.OriginationSources.SAHomeLoans);
			var exceptionsCreditCriteria = creditMatrix.CreditCriterias.Where(x => x.ExceptionCriteria == true);
			var minimumLoanAgreementAmount = exceptionsCreditCriteria.Min(x => x.MinLoanAmount);

			if (applicationInformationVariableLoan != null)
			{
				if ((applicationInformationVariableLoan.LoanAgreementAmount.HasValue ? applicationInformationVariableLoan.LoanAgreementAmount.Value : 0) < minimumLoanAgreementAmount)
				{
					errorMessage = String.Format("Loan amount is {0}. The minimum allowed is: {1}.", (applicationInformationVariableLoan.LoanAgreementAmount.HasValue ? applicationInformationVariableLoan.LoanAgreementAmount.Value : 0).ToString(SAHL.Common.Constants.CurrencyFormat),
																									 minimumLoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat));
					AddMessage(errorMessage, errorMessage, Messages);
				}
			}
			else
				AddMessage("Application does not have variable information.", "Application does not have variable information.", Messages);

			return 1;
		}
	}

	[RuleDBTag("CreditDisqualificationMaxLoanAgreeAmount",
	"CreditDisqualificationMaxLoanAgreeAmount rules",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMaxLoanAgreeAmount")]
	[RuleInfo]
	public class CreditDisqualificationMaxLoanAgreeAmount : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (!(Parameters[0] is IApplication))
				throw new ArgumentException("Parameters[0] is not of type IApplication.");

			IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
			IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
			IControl ctrl;
			string err;

			IApplication application = Parameters[0] as IApplication;
			IApplicationProduct appProd = application.CurrentProduct;

			ISupportsVariableLoanApplicationInformation vli = appProd as ISupportsVariableLoanApplicationInformation;
			IApplicationInformationVariableLoan aivl = vli.VariableLoanInformation;

			if (aivl != null)
			{
				ctrl = ctrlRepo.GetControlByDescription("Calc - maxLAA");
				if ((aivl.LoanAgreementAmount.HasValue ? aivl.LoanAgreementAmount.Value : 0) > (ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric.Value) : 0))
				{
					err = ctrl.ControlText + " Loan amount is " + (aivl.LoanAgreementAmount.HasValue ? aivl.LoanAgreementAmount.Value : 0).ToString(SAHL.Common.Constants.CurrencyFormat) + ". The maximum allowed is: " + (ctrl.ControlNumeric.HasValue ? ctrl.ControlNumeric.Value : 0).ToString(SAHL.Common.Constants.CurrencyFormat) + ".";
					AddMessage(err, err, Messages);
				}
			}
			else
				AddMessage("Application does not have variable information.", "Application does not have variable information.", Messages);

			return 1;
		}
	}

	[RuleDBTag("CreditDisqualificationMinValuation",
	"CreditDisqualificationMinValuation rules",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinValuation")]
	[RuleInfo]
	public class CreditDisqualificationMinValuation : BusinessRuleBase
	{
		private ICreditCriteriaRepository creditCriteriaRepository;
		private IApplicationRepository applicationRepository;

		public CreditDisqualificationMinValuation(ICreditCriteriaRepository creditCriteriaRepository, IApplicationRepository applicationRepository)
		{
			this.creditCriteriaRepository = creditCriteriaRepository;
			this.applicationRepository = applicationRepository;
		}
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (!(Parameters[0] is IApplication))
				throw new ArgumentException("Parameters[0] is not of type IApplication.");

			IApplication application = Parameters[0] as IApplication;
			IApplicationProduct applicationProduct = application.CurrentProduct;

			ISupportsVariableLoanApplicationInformation variableLoanInformation = applicationProduct as ISupportsVariableLoanApplicationInformation;
			IApplicationInformationVariableLoan applicationInformationVariableLoan = variableLoanInformation.VariableLoanInformation;

			if (applicationInformationVariableLoan == null)
			{
				AddMessage("Application does not have variable information.", "Application does not have variable information.", Messages);
				return 0;
			}

			var applicationCreditCriteria = creditCriteriaRepository.GetCreditCriteriaByKey(applicationInformationVariableLoan.CreditCriteria.Key);
			var minimumValuationAmount = applicationCreditCriteria.MinPropertyValue ?? 0;

			var applicationPropertyValuation = applicationInformationVariableLoan.PropertyValuation ?? 0;

			if (applicationPropertyValuation < minimumValuationAmount)
			{
				var ruleMessage = String.Format("This application's valuation of {0} is less than the minimum of {1} required for Category {2}",
					applicationPropertyValuation.ToString(Constants.CurrencyFormat),
					minimumValuationAmount.ToString(Constants.CurrencyFormat),
					applicationCreditCriteria.Category.Key);
				AddMessage(ruleMessage, ruleMessage, Messages);
			}

			return 1;
		}
	}

	[RuleDBTag("ApplicationHOCExists",
	"CreditDisqualification rules",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Application.Credit.ApplicationHOCExists")]
	[RuleInfo]
	public class ApplicationHOCExists : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (!(Parameters[0] is IApplication))
				throw new ArgumentException("Parameters[0] is not of type IApplication.");

			IApplication application = Parameters[0] as IApplication;

			foreach (IAccount acc in application.RelatedAccounts)
			{
				if (acc.AccountType == SAHL.Common.Globals.AccountTypes.HOC)
					return 1;
			}

			AddMessage("Application must have an HOC account.", "Application must have an HOC account.", Messages);
			return 1;
		}
	}

	[RuleDBTag("ApplicationSwitchSettlementBankAccount",
	"ApplicationSwitchSettlementBankAccount rules",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Application.Credit.ApplicationSwitchSettlementBankAccount")]
	[RuleInfo]
	public class ApplicationSwitchSettlementBankAccount : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (!(Parameters[0] is IApplication))
				throw new ArgumentException("Parameters[0] is not of type IApplication.");

			IApplication application = Parameters[0] as IApplication;

			bool checkRule = false;
			bool settlementBankAccountExists = false;

			IApplicationMortgageLoanSwitch appSW = application as IApplicationMortgageLoanSwitch;

			if (appSW != null)
			{
				checkRule = true;

				foreach (IApplicationExpense ae in application.ApplicationExpenses)
				{
					if (ae.ExpenseType.Key == (int)SAHL.Common.Globals.ExpenseTypes.Existingmortgageamount)
						if (ae.ApplicationDebtSettlements[0].BankAccount != null)
							settlementBankAccountExists = true;
				}
			}

			if (checkRule && !settlementBankAccountExists)
				AddMessage("Switch application must have an settlement bank account.", "Switch application must have an settlement bank account.", Messages);

			return 1;
		}
	}

	[RuleDBTag("ApplicationNewPurchaseSellerRoleExists",
	"ApplicationNewPurchaseSellerRoleExists rules",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Application.Credit.ApplicationNewPurchaseSellerRoleExists")]
	[RuleInfo]
	public class ApplicationNewPurchaseSellerRoleExists : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (!(Parameters[0] is IApplication))
				throw new ArgumentException("Parameters[0] is not of type IApplication.");

			IApplication application = Parameters[0] as IApplication;
			IApplicationMortgageLoanNewPurchase appNP = application as IApplicationMortgageLoanNewPurchase;

			if (appNP != null)
			{
				foreach (IApplicationRole ar in application.ApplicationRoles)
				{
					if (ar.ApplicationRoleType.Key == (int)SAHL.Common.Globals.OfferRoleTypes.Seller)
						return 1;
				}
				AddMessage("Switch application must have an settlement bank account.", "Switch application must have an settlement bank account.", Messages);
			}

			return 1;
		}
	}

	#region ApplicationHasActiveEmploymentRecord

	[RuleDBTag("ApplicationHasActiveEmploymentRecord",
	"ApplicationHasActiveEmploymentRecord",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Application.Credit.ApplicationHasActiveEmploymentRecord")]
	[RuleInfo]
	public class ApplicationHasActiveEmploymentRecord : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Check for allowed object type(s)

			if (Parameters.Length == 0)
				throw new ArgumentException("The ApplicationHasActiveEmploymentRecord rule expects a Domain object to be passed.");

			if (!(Parameters[0] is IApplication))
				throw new ArgumentException("The ApplicationHasActiveEmploymentRecord rule expects the following objects to be passed: IApplication.");

			#endregion Check for allowed object type(s)

			IApplication application = Parameters[0] as IApplication;

			foreach (IApplicationRole appRole in application.ApplicationRoles)
			{
				if (appRole.LegalEntity.Employment != null
					&& appRole.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)OfferRoleTypeGroups.Client)
				{
					foreach (IEmployment employment in appRole.LegalEntity.Employment)
					{
						if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current)
						{
							//At lest one Active Employment Record found so can return
							return 1;
						}
					}
				}
			}

			// If code reached this point then no active employment records where found for any client in the application.
			// Return Error Message.
			AddMessage("The Application should contain at least one active Employment Record.", "", Messages);
			return 1;
		}
	}

	#endregion ApplicationHasActiveEmploymentRecord

	#region ApplicationCheckMinLoanAmount

	[RuleDBTag("ApplicationCheckMinLoanAmount",
	"ApplicationCheckMinLoanAmount",
	"SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Application.Credit.ApplicationCheckMinLoanAmount")]
	[RuleInfo]
	public class ApplicationCheckMinLoanAmount : BusinessRuleBase
	{
		public ApplicationCheckMinLoanAmount(ICastleTransactionsService castleTransactionService)
		{
			this.castleTransactionService = castleTransactionService;
		}

		private ICastleTransactionsService castleTransactionService;

		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Check for allowed object type(s)

			if (Parameters.Length == 0)
				throw new ArgumentException("The ApplicationCheckMinLoanAmount rule expects a Domain object to be passed.");

			if (!(Parameters[0] is IApplication))
				throw new ArgumentException("The ApplicationCheckMinLoanAmount rule expects the following objects to be passed: IApplication.");

			#endregion Check for allowed object type(s)

			IApplication app = Parameters[0] as IApplication;

			string sqlQuery = UIStatementRepository.GetStatement("COMMON", "ApplicationCheckMinLoanAmount");
			ParameterCollection parameters = new ParameterCollection();
			parameters.Add(new SqlParameter("@appKey", app.Key));

			object o = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

			if (o != null)
			{
				string errorMessage = (string)o;
				AddMessage(errorMessage, errorMessage, Messages);
				return 1;
			}
			return 0;
		}
	}

	#endregion ApplicationCheckMinLoanAmount

	#region ApplicationCheckMinIncome

	[RuleDBTag("ApplicationCheckMinIncome",
   "ApplicationCheckMinIncome",
	"SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.Application.Credit.ApplicationCheckMinIncome")]
	[RuleInfo]
	public class ApplicationCheckMinIncome : BusinessRuleBase
	{
		public ApplicationCheckMinIncome(ICastleTransactionsService castleTransactionService)
		{
			this.castleTransactionService = castleTransactionService;
		}

		private ICastleTransactionsService castleTransactionService;

		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Check for allowed object type(s)

			if (Parameters.Length == 0)
				throw new ArgumentException("The ApplicationCheckMinIncome rule expects a Domain object to be passed.");

			if (!(Parameters[0] is IApplication))
				throw new ArgumentException("The ApplicationCheckMinIncome rule expects the following objects to be passed: IApplication.");

			#endregion Check for allowed object type(s)

			IApplication app = Parameters[0] as IApplication;

			string sqlQuery = UIStatementRepository.GetStatement("COMMON", "ApplicationCheckMinIncome");
			ParameterCollection parameters = new ParameterCollection();
			parameters.Add(new SqlParameter("@appKey", app.Key));

			object o = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

			if (o != null)
			{
				string errorMessage = (string)o;
				AddMessage(errorMessage, errorMessage, Messages);
				return 1;
			}
			return 0;
		}
	}

	#endregion ApplicationCheckMinIncome

	#region ApplicationConfirmIncome

	[RuleDBTag("ApplicationConfirmIncome",
  "Shows a warning if no incomes are confirmed.",
	"SAHL.Rules.DLL",
 "SAHL.Common.BusinessModel.Rules.Application.Credit.ApplicationConfirmIncome", false)]
	[RuleInfo]
	public class ApplicationConfirmIncome : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Check for allowed object type(s)

			if (Parameters.Length == 0)
				throw new ArgumentException("The ApplicationConfirmIncome rule expects a Domain object to be passed.");

			if (!(Parameters[0] is IApplication))
				throw new ArgumentException("The ApplicationConfirmIncome rule expects the following objects to be passed: IApplication.");

			#endregion Check for allowed object type(s)

			IApplication application = Parameters[0] as IApplication;

			//application.Account.Roles[0].LegalEntity.Employment[0].IsConfirmed

			string errorMessage = SAHL.Common.BusinessModel.Rules.Products.IncomeHelper.CheckIncome(application);
			if (errorMessage.Length > 0)
			{
				AddMessage(errorMessage, errorMessage, Messages);
				return 0;
			}
			return 0;
		}
	}

	#endregion ApplicationConfirmIncome

	#region ApplicationNonResidentLoanAgreementAmount

	[RuleDBTag("ApplicationNonResidentLoanAgreementAmount",
	"Shows a warning if any legal entity attached to this application are non-resident.",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Application.Credit.ApplicationNonResidentLoanAgreementAmount", false)]
	[RuleInfo]
	public class ApplicationNonResidentLoanAgreementAmount : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Check for allowed object type(s)

			if (Parameters.Length == 0)
				throw new ArgumentException("The ApplicationNonResidentLoanAgreementAmount rule expects a Domain object to be passed.");

			if (!(Parameters[0] is IApplication))
				throw new ArgumentException("The ApplicationNonResidentLoanAgreementAmount rule expects the following objects to be passed: IApplication.");

			#endregion Check for allowed object type(s)

			IApplication application = Parameters[0] as IApplication;

			//application.Account.Roles[0].LegalEntity.Employment[0].IsConfirmed

			IReadOnlyEventList<ILegalEntity> LEs = application.GetLegalEntitiesByRoleType(new OfferRoleTypes[] { OfferRoleTypes.MainApplicant, OfferRoleTypes.Suretor, OfferRoleTypes.LeadMainApplicant, OfferRoleTypes.LeadSuretor });

			foreach (ILegalEntity LE in LEs)
			{
				ILegalEntityNaturalPerson lenp = LE as ILegalEntityNaturalPerson;
				if (lenp != null) // seperate rule to check the LE is a Natural Person, dont need to worry here
				{
					if (lenp.CitizenType != null && String.IsNullOrEmpty(lenp.IDNumber) &&
						(lenp.CitizenType.Key == (int)CitizenTypes.Foreigner
						|| lenp.CitizenType.Key == (int)CitizenTypes.NonResident
						|| lenp.CitizenType.Key == (int)CitizenTypes.NonResidentConsulate
						|| lenp.CitizenType.Key == (int)CitizenTypes.NonResidentDiplomat
						|| lenp.CitizenType.Key == (int)CitizenTypes.NonResidentHighCommissioner
						|| lenp.CitizenType.Key == (int)CitizenTypes.NonResidentRefugee))
					{
						string msg = "Only 50% of the loan agreement amount to Purchase Price is permissible to Non-Resident applicants.";
						AddMessage(msg, msg, Messages);
						return 0;
					}
				}
			}

			return 1;
		}
	}

	#endregion ApplicationNonResidentLoanAgreementAmount
}