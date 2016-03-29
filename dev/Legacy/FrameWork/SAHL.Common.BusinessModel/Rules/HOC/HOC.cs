using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common;
using SAHL.Common.Globals;
using SAHL.Common.DataAccess;
using System.Data.SqlClient;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.DAO;
using System.Linq;

namespace SAHL.Common.BusinessModel.Rules.HOC
{

	[RuleDBTag("HOCLessThanLatestValuation",
	"HOC Total Sum Insured should not be less than HOC from the latest Valuation",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.HOC.HOCLessThanLatestValuation")]
	[RuleInfo]
	public class HOCLessThanLatestValuation : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			IApplicationMortgageLoan applicationMortgageLoan = (IApplicationMortgageLoan)Parameters[0];
			IHOCRepository hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
			IHOC hoc = null;
			IValuation valuation = null;

			if (applicationMortgageLoan != null)
			{
				if ((applicationMortgageLoan.Property != null) &&
					(applicationMortgageLoan.Property.LatestCompleteValuation != null
					&& applicationMortgageLoan.Property.LatestCompleteValuation.IsActive))
				{
					valuation = applicationMortgageLoan.Property.LatestCompleteValuation;
				}

				foreach (IAccount account in applicationMortgageLoan.RelatedAccounts)
				{
					if (account.Product.Key == (int)SAHL.Common.Globals.Products.HomeOwnersCover)
					{
						hoc = hocRepo.GetHOCByKey(account.FinancialServices[0].Key);
						break;
					}
				}

				if (valuation != null && hoc != null)
				{
					double totalSumInsured = 0;
					if (valuation != null && valuation is IValuationDiscriminatedLightstoneAVM)
						totalSumInsured = Convert.ToDouble(valuation.ValuationHOCValue);
					else if (valuation != null)
						totalSumInsured = Convert.ToDouble(valuation.HOCConventionalAmount) + Convert.ToDouble(valuation.HOCShingleAmount) + Convert.ToDouble(valuation.HOCThatchAmount);

					if (hoc.HOCTotalSumInsured < totalSumInsured)
					{
						string msg = string.Format("The HOC - Total Sum Insured Amount ({0}) is less than the latest Valuation Amount ({1}).", hoc.HOCTotalSumInsured.ToString(Constants.CurrencyFormat), totalSumInsured.ToString(Constants.CurrencyFormat));
						AddMessage(msg, msg, Messages);
						return 0;
					}
				}
			}
			return 1;
		}
	}

	[RuleDBTag("HOCLessThanLoanAgreementAmount",
	"HOC Total Sum Insured should not be less than LoanAgreementAmount",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.HOC.HOCLessThanLoanAgreementAmount")]
	[RuleInfo]
	public class HOCLessThanLoanAgreementAmount : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			IApplicationMortgageLoan applicationMortgageLoan = (IApplicationMortgageLoan)Parameters[0];
			IHOCRepository hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
			IHOC hoc = null;
			ISupportsVariableLoanApplicationInformation vlai = null;
			IApplicationInformationVariableLoan vli = null;

			if (applicationMortgageLoan != null)
			{
				vlai = applicationMortgageLoan.CurrentProduct as ISupportsVariableLoanApplicationInformation;

				if (vlai != null)
					vli = vlai.VariableLoanInformation;

				foreach (IAccount account in applicationMortgageLoan.RelatedAccounts)
				{
					if (account.Product.Key == (int)SAHL.Common.Globals.Products.HomeOwnersCover)
					{
						hoc = hocRepo.GetHOCByKey(account.FinancialServices[0].Key);
						break;
					}
				}

				if ((vlai != null && vli != null) && (hoc != null))
				{
					double laa = vli.LoanAgreementAmount.HasValue ? vli.LoanAgreementAmount.Value : 0D;

					if (hoc.HOCTotalSumInsured < laa)
					{
						string msg = string.Format("The HOC - Total Sum Insured Amount ({0}) is less than the Loan Agreement Amount ({1}).", hoc.HOCTotalSumInsured.ToString(Constants.CurrencyFormat), laa.ToString(Constants.CurrencyFormat));
						AddMessage(msg, msg, Messages);
						return 0;
					}
				}
			}
			return 1;
		}
	}

	[RuleDBTag("HOCExists",
	 "HOC record must be captured",
	 "SAHL.Rules.DLL",
	 "SAHL.Common.BusinessModel.Rules.HOC.HOCExists")]
	[RuleInfo]
	public class HOCExists : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			IApplicationMortgageLoan appML = Parameters[0] as IApplicationMortgageLoan;

			if (appML.ApplicationType.Key == (int)OfferTypes.SwitchLoan
				|| appML.ApplicationType.Key == (int)OfferTypes.NewPurchaseLoan
				|| appML.ApplicationType.Key == (int)OfferTypes.RefinanceLoan)
			{
				foreach (IAccount account in appML.RelatedAccounts)
				{
					if (account.Product.Key == (int)SAHL.Common.Globals.Products.HomeOwnersCover)
						return 0;
				}

			}
			else if (appML.ApplicationType.Key == (int)OfferTypes.ReAdvance
				|| appML.ApplicationType.Key == (int)OfferTypes.FurtherAdvance
				|| appML.ApplicationType.Key == (int)OfferTypes.FurtherLoan)
			{
				foreach (IAccount account in appML.Account.RelatedChildAccounts)
				{
					if (account.Product.Key == (int)SAHL.Common.Globals.Products.HomeOwnersCover)
						return 0;
				}
			}

			AddMessage("Application has no HOC Record", "Application has no HOC Record", Messages);
			return 1;
		}
	}

	[RuleDBTag("HOCInsurerMandatory",
	  "HOCInsurer is Mandatory",
	  "SAHL.Rules.DLL",
	  "SAHL.Common.BusinessModel.Rules.HOC.HOCInsurerMandatory")]
	[RuleInfo]
	public class HOCInsurerMandatory : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			IHOC hoc = (IHOC)Parameters[0];
			if (null == hoc.HOCInsurer || hoc.HOCInsurer.Key <= 1)
			{
				AddMessage("HOC Insurer must be selected and can not be Unknown.", "", Messages);
			}
			return 1;
		}
	}
	[RuleDBTag("HOCApplicationMortgageLoanCollection",
	"HOC record must be captured",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.HOC.HOCApplicationMortgageLoanCollection")]
	[RuleInfo]
	public class HOCApplicationMortgageLoanCollection : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			IAccount account = (IAccount)Parameters[0];
			IFinancialService hoc = account.GetFinancialServiceByType(FinancialServiceTypes.HomeOwnersCover);

			if (hoc == null)
			{
				AddMessage("An HOC Record is Required.", "", Messages);
			}
			return 1;
		}
	}

	[RuleDBTag("HOCRoofDescriptionConventional",
	"Conventional Amount Required",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.HOC.HOCRoofDescriptionConventional")]
	[RuleInfo]
	public class HOCRoofDescriptionConventional : BusinessRuleBase
	{
		private readonly IPropertyRepository propertyRepository;
		public HOCRoofDescriptionConventional(IPropertyRepository propertyRepository)
		{
			this.propertyRepository = propertyRepository;
		}
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (Parameters.Length == 0)
				throw new Exception("Rule expects a parameter to be passed: IHOC ");

			IHOC hoc = Parameters[0] as IHOC;

			if (hoc == null)
				throw new Exception("Rule expects IHOC object to be passed");

			IProperty property = propertyRepository.GetPropertyByHOC(hoc);

			if (property == null)
				throw new Exception("IHOC needs to be linked to a IProperty object");

			bool isLightStoneValuation = false;

			if (property.LatestCompleteValuation is IValuationDiscriminatedLightstoneAVM || property.LatestCompleteValuation is IValuationDiscriminatedLightStonePhysical)
				isLightStoneValuation = true;

			if ((!isLightStoneValuation && property.LatestCompleteValuation != null && property.LatestCompleteValuation.IsActive)
				&& (hoc.HOCInsurer.Key == (int)HOCInsurers.SAHLHOC && hoc.HOCRoof.Key == (int)HOCRoofs.Conventional && hoc.HOCConventionalAmount <= 0))
			{
				AddMessage("Conventional Amount is Required.", "", Messages);
			}
			return 1;
		}
	}

	[RuleDBTag("HOCRoofDescriptionThatch",
	"Thatc Amount Required",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.HOC.HOCRoofDescriptionThatch")]
	[RuleInfo]
	public class HOCRoofDescriptionThatch : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (Parameters.Length == 0)
				throw new Exception("Rule expects a parameter to be passed: IHOC ");

			IHOC hoc = Parameters[0] as IHOC;

			if (hoc == null)
				throw new Exception("Rule expects IHOC object to be passed");

			IPropertyRepository propRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
			IProperty prop = propRepo.GetPropertyByHOC(hoc);

			if (prop == null)
				throw new Exception("IHOC needs to be linked to a IProperty object");

			bool isLightStoneValuation = false;

			if (prop.LatestCompleteValuation is IValuationDiscriminatedLightstoneAVM)
				isLightStoneValuation = true;

			if ((!isLightStoneValuation && prop.LatestCompleteValuation != null && prop.LatestCompleteValuation.IsActive)
				&& (hoc.HOCInsurer.Key == (int)HOCInsurers.SAHLHOC && hoc.HOCRoof.Key == (int)HOCRoofs.Thatch && hoc.HOCThatchAmount <= 0))
			{
				AddMessage("Thatch Amount is Required.", "", Messages);
			}
			return 1;
		}
	}

	[RuleDBTag("HOCRoofDescriptionCombined",
	"Thatch and Conventional Amounts Required",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.HOC.HOCRoofDescriptionCombined")]
	[RuleInfo]
	public class HOCRoofDescriptionCombined : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (Parameters.Length == 0)
				throw new Exception("Rule expects a parameter to be passed: IHOC ");

			IHOC hoc = Parameters[0] as IHOC;

			if (hoc == null)
				throw new Exception("Rule expects IHOC object to be passed");

			IPropertyRepository propRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
			IProperty prop = propRepo.GetPropertyByHOC(hoc);

			if (prop == null)
				throw new Exception("IHOC needs to be linked to a IProperty object");

			bool isLightStoneValuation = false;

			if ((prop.LatestCompleteValuation != null) && (prop.LatestCompleteValuation is IValuationDiscriminatedLightstoneAVM))
				isLightStoneValuation = true;

			if ((!isLightStoneValuation && prop.LatestCompleteValuation != null && prop.LatestCompleteValuation.IsActive)
				&& (hoc.HOCInsurer.Key == (int)HOCInsurers.SAHLHOC && hoc.HOCRoof.Key == (int)HOCRoofs.Partial && (hoc.HOCThatchAmount <= 0 || hoc.HOCConventionalAmount <= 0)))
			{
				AddMessage("Both Thatch and Conventional Amounts are Required.", "", Messages);
			}
			return 1;
		}
	}
	[RuleDBTag("HOCTotalSumInsuredMinimum",
	"HOCTotalSumInsured must be greater than R100 000.00",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.HOC.HOCTotalSumInsuredMinimum")]
	[RuleParameterTag(new string[] { "@MinHOCTotalSumInsured,100000,7" })]
	[RuleInfo]
	public class HOCTotalSumInsuredMinimum : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (Parameters.Length == 0 || !(Parameters[0] is IHOC))
				throw new Exception("Rule expects IHOC object to be passed");

			// Expecting one parameter else fail.
			if (RuleItem.RuleParameters.Count < 1)
				throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

			Double minimumValue = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

			IHOC hoc = (IHOC)Parameters[0];

			if (hoc.HOCTotalSumInsured <= minimumValue)
			{
				AddMessage("HOCTotalSumInsured must be greater than " + minimumValue.ToString(Constants.CurrencyFormat), "", Messages);
			}
			return 1;
		}
	}
	[RuleDBTag("HOCLoanClosed",
	 "HOC Status must be set to Closed",
	 "SAHL.Rules.DLL",
	 "SAHL.Common.BusinessModel.Rules.HOC.HOCLoanClosed")]
	[RuleInfo]
	public class HOCLoanClosed : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (Parameters.Length == 0 || !(Parameters[0] is IHOC))
				throw new Exception("Rule expects IHOC object to be passed");

			IHOC hoc = (IHOC)Parameters[0];

			if (hoc.HOCInsurer.Key == (int)HOCInsurers.LoanCancelled_Closed && hoc.HOCStatus.Key != (int)HocStatuses.Closed)
			{
				AddMessage("HOCStatus must be set to Closed.", "", Messages);
			}
			return 1;
		}
	}
	[RuleDBTag("HOCPolicyNumberNotNull",
	"HOCPolicyNumber must not be Null",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.HOC.HOCPolicyNumberNotNull")]
	[RuleInfo]
	public class HOCPolicyNumberNotNull : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (Parameters.Length == 0 || !(Parameters[0] is IHOC))
				throw new Exception("Rule expects IHOC object to be passed");

			IHOC hoc = (IHOC)Parameters[0];

			if ((hoc.HOCInsurer.Key != (int)HOCInsurers.SAHLHOC && hoc.HOCInsurer.Key != (int)HOCInsurers.LoanCancelled_Closed &&
				hoc.HOCInsurer.Key != (int)HOCInsurers.SectionalTitle && hoc.HOCInsurer.Key != (int)HOCInsurers.PaidupwithnoHOC)
				&& (hoc.HOCPolicyNumber.Length == 0))
			{
				AddMessage("HOC PolicyNumber must not be null.", "", Messages);
			}
			return 1;
		}
	}
	[RuleDBTag("HOCPaidUpNoHOC",
	"HOC Status must ne PaidUpNoHOC",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.HOC.HOCPaidUpNoHOC")]
	[RuleInfo]
	public class HOCPaidUpNoHOC : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (Parameters.Length == 0 || !(Parameters[0] is IHOC))
				throw new Exception("Rule expects IHOC object to be passed");

			IHOC hoc = (IHOC)Parameters[0];

			if (hoc.HOCInsurer.Key == (int)HOCInsurers.PaidupwithnoHOC && hoc.HOCStatus.Key != (int)HocStatuses.PaidUpwithnoHOC)
			{
				AddMessage("HOCStatus must be set to PaidUpWithNoHOC.", "", Messages);
			}
			return 1;
		}
	}
	[RuleDBTag("HOCSAHLCalculatePremium",
	"HOC Premium must be calculated",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.HOC.HOCSAHLCalculatePremium")]
	[RuleInfo]
	public class HOCSAHLCalculatePremium : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (Parameters.Length == 0 || !(Parameters[0] is IHOC))
				throw new Exception("Rule expects IHOC object to be passed");

			IHOC hoc = (IHOC)Parameters[0];

			if (hoc.Key > 0 && hoc.FinancialService.Account.AccountStatus.Key == (int)AccountStatuses.Open && hoc.HOCInsurer.Key == (int)HOCInsurers.SAHLHOC && hoc.HOCMonthlyPremium <= 0)
			{
				AddMessage("HOC Monthly Premium can not be zero.", "", Messages);
			}
			return 1;
		}
	}

	[RuleDBTag("HOCCededStatus",
	"HOCCededStatus Status Confirm",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.HOC.HOCCededStatus", false)]
	[RuleInfo]
	public class HOCCededStatus : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (Parameters.Length == 0 || !(Parameters[0] is IHOC))
				throw new Exception("Rule expects IHOC object to be passed");

			IHOC hoc = (IHOC)Parameters[0]; // New Object

			if (hoc.HOCInsurer.Key != (int)HOCInsurers.SAHLHOC && hoc.HOCInsurer.Key != (int)HOCInsurers.SectionalTitle &&
				hoc.HOCInsurer.Key != (int)HOCInsurers.PaidupwithnoHOC && hoc.HOCInsurer.Key != (int)HOCInsurers.LoanCancelled_Closed)
			{

				AddMessage("Are you sure policy ceded status is correct ?", "", Messages);
			}
			return 1;
		}
	}


	[RuleDBTag("HOCTotalSumAssuredLessThanPrevious",
	"Warning Rule which Checks if the New HOC SumInsured is less than the Previous HOC SumInsured",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.HOC.HOCTotalSumAssuredLessThanPrevious", false)]
	[RuleInfo]
	public class HOCTotalSumAssuredLessThanPrevious : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (Parameters.Length == 0)
				throw new Exception("Rule expects a parameter to be passed: IHOC ");

			IHOC hoc = Parameters[0] as IHOC;

			if (hoc == null)
				throw new Exception("Rule expects IHOC object to be passed");

			if (hoc.Key == 0)
				return 1;

			IPropertyRepository propRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
			IProperty prop = propRepo.GetPropertyByHOC(hoc);

			if (prop == null)
				throw new Exception("IHOC needs to be linked to a IProperty object");

			// ignore this rule for a lightstone valuation
			if (prop.LatestCompleteValuation != null && prop.LatestCompleteValuation is IValuationDiscriminatedLightstoneAVM)
				return 1;

			if (hoc.Original != null && hoc.HOCTotalSumInsured < hoc.Original.HOCTotalSumInsured)
			{
				string errorMsg = "New HOC SumInsured is less than previous value of R " + hoc.Original.HOCTotalSumInsured.ToString() + ".";
				AddMessage(errorMsg, errorMsg, Messages);
				return 0;
			}
			return 1;
		}
	}

	[RuleDBTag("HOCTitleTypeSectionalTitle",
	"The properly is listed as 'Sectional Title', HOC only allowed on title type of 'Sectional Title with HOC' ",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.HOC.HOCTitleTypeSectionalTitle")]
	[RuleInfo]
	public class HOCTitleTypeSectionalTitle : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (Parameters.Length == 0)
				throw new Exception("Rule expects an object of type IHOC or IApplication to be passed");

			IApplication app = Parameters[0] as IApplication;
			IHOC hoc = Parameters[0] as IHOC;

			if (app == null && hoc == null)
				throw new Exception("Rule expects an object of type IHOC or IApplication to be passed");

			// This means that an IApplication object has been passed to the rules
			// Application in Order : CheckCreditSubmissionRules
			if (hoc == null && app != null)
			{
				IHOCRepository hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
				IAccountHOC accHOC = hocRepo.RetrieveHOCByOfferKey(app.Key);
				if (accHOC != null)
					hoc = accHOC.HOC;
			}

			if (hoc != null)
			{
				IPropertyRepository propRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
				IProperty prop = propRepo.GetPropertyByHOC(hoc);

				if (prop == null)
					throw new Exception("IHOC needs to be linked to a IProperty object");

				if (prop.TitleType != null && hoc.HOCInsurer != null)
				{
					if (prop.TitleType.Key == (int)TitleTypes.SectionalTitle && hoc.HOCInsurer.Key != (int)HOCInsurers.SectionalTitle)
					{
						// Throw the error here as the Prop.TitleType = Sectional Title is 
						string errorMsg = "The property is listed as 'Sectional Title', HOC only allowed on title type of 'Sectional Title with HOC'.";
						AddMessage(errorMsg, errorMsg, Messages);
						return 0;
					}
				}
			}
			return 1;
		}
	}

	[RuleDBTag("HOCValuationExpiredCheck",
	"Check to make sure that valuation is not older than a certain period (2 years)",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.HOC.HOCValuationExpiredCheck")]
	[RuleParameterTag(new string[] { "@ExpiryPeriod,-2,9" })]
	[RuleInfo]
	public class HOCValuationExpiredCheck : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (Parameters.Length == 0)
				throw new Exception("Rule expects a parameter to be passed: IHOC ");

			IHOC hoc = Parameters[0] as IHOC;
			int expiryPeriod = Convert.ToInt32(RuleItem.RuleParameters[0].Value);

			if (hoc == null)
				throw new Exception("Rule expects IHOC object to be passed");

			IPropertyRepository propRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
			IProperty prop = propRepo.GetPropertyByHOC(hoc);

			if (prop == null)
				throw new Exception("IHOC needs to be linked to a IProperty object");

			if (prop.LatestCompleteValuation == null)
				return 1;

			if (prop.LatestCompleteValuation.ValuationDate < DateTime.Now.AddYears(expiryPeriod))
			{
				string errorMsg = string.Format("Please note that the current valuation is older than {0} years.", Math.Abs(expiryPeriod));
				AddMessage(errorMsg, errorMsg, Messages);
				return 0;
			}
			return 1;
		}
	}

	[RuleDBTag("HOCNoHOCDetailTypeToSAHLHOC",
	"Ensure that when moving from no HOC to SAHL HOC that a message is displayed",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.HOC.HOCNoHOCDetailTypeToSAHLHOC")]
	[RuleInfo]
	public class HOCNoHOCDetailTypeToSAHLHOC : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			if (Parameters.Length == 0)
				throw new Exception("Rule expects a parameter to be passed: IHOC ");

			IHOC hoc = Parameters[0] as IHOC;

			if (hoc == null)
				throw new Exception("Rule expects IHOC object to be passed");

			IAccountRepository accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();

            if (hoc.HOCInsurer.Key == (int)HOCInsurers.SAHLHOC && hoc.FinancialService.Account.OriginationSourceProduct != null &&
				hoc.FinancialService.Account.ParentAccount != null)
			{
                //I am assuming that the Detail Type is still stored against the parent key
				IReadOnlyEventList<IDetail> detailTypes = accountRepository.GetDetailByAccountKeyAndDetailType(hoc.FinancialService.Account.ParentAccount.Key, (int)DetailTypes.HOCNoHoc);

				if (detailTypes.Count > 0)
				{
					string errorMsg = string.Format("Please remove the detail type 'HOC - NO HOC' before proceeding to change the HOC Insurer to 'SAHL HOC'");
					AddMessage(errorMsg, errorMsg, Messages);
					return 0;
				}
			}

			return 1;
		}
	}


    [RuleDBTag("HOCNonHOCCessionDetailTypeToSAHLHOC",
    "Ensure that when moving to SAHL HOC and a 'HOC Cession of Policy' detail type exists, prompt user to remove the detail type.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.HOC.HOCNonHOCCessionDetailTypeToSAHLHOC")]
    [RuleInfo]
    public class HOCNonHOCCessionDetailTypeToSAHLHOC : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("Rule expects a parameter to be passed: IHOC ");

            IHOC hoc = Parameters[0] as IHOC;

            if (hoc == null)
                throw new ArgumentException("Rule expects IHOC object to be passed");

			if (hoc.FinancialService == null
                || hoc.FinancialService.FinancialServiceParent == null
                || hoc.FinancialService.FinancialServiceParent.Account == null)
                return 1;
				
            if (hoc.HOCInsurer.Key == (int)HOCInsurers.SAHLHOC)
            {
                IAccount parentAccount = hoc.FinancialService.FinancialServiceParent.Account;

                IList<IDetail> details = (from x in parentAccount.Details
                                          where x.DetailType.Key == (int)DetailTypes.HOCCessionOfPolicy || x.DetailType.Key == (int)DetailTypes.HOCCessionCommercialUse
                                          select x).ToList();

                if (details.Count > 0)
                {
                    foreach (IDetail detail in details)
                    {
                        string errorMsg = string.Format(@"Please remove the detail type '{0}' before proceeding to change the HOC Insurer to 'SAHL HOC'", detail.DetailType.Description);
                        AddMessage(errorMsg, errorMsg, Messages);
                    }
                    return 0;
                }
            }

            return 1;
        }
    }


}
