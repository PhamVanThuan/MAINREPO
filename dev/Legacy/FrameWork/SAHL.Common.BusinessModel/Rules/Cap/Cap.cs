using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Castle.ActiveRecord.Queries;
using SAHL.Common;
using SAHL.Common.BusinessModel;
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

namespace SAHL.Common.BusinessModel.Rules.Cap
{
    [RuleDBTag("ApplicationCAP2VerifyCapBroker",
    "ApplicationCAP2VerifyCapBroker",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCAP2VerifyCapBroker")]
    [RuleInfo]
    public class ApplicationCAP2VerifyCapBroker : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IADUser adUser = null;

            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCAP2VerifyCapBroker rule expects a Domain object to be passed.");

            adUser = Parameters[1] as IADUser;

            if (adUser == null)
                throw new ArgumentException("The ApplicationCAP2VerifyCapBroker rule expects the following objects to be passed: IADUser");

            #endregion Check for allowed object type(s)

            ICapRepository _capRepo = RepositoryFactory.GetRepository<ICapRepository>();
            IBroker broker = _capRepo.GetBrokerByADUserKey(adUser.Key);

            if (broker == null)
            {
                string errorMessage = String.Format("ADUser {0} not added as a Broker", adUser.ADUserName);
                AddMessage(errorMessage, errorMessage, Messages);
            }
            return 0;
        }
    }

    [RuleDBTag("ApplicationCAP2QualifyCAPOfferDetail",
    "ApplicationCAP2NextQuarter",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCAP2QualifyCAPOfferDetail")]
    [RuleInfo]
    public class ApplicationCAP2QualifyCAPOfferDetail : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCAP2QualifyCAPOfferDetail rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ICapApplicationDetail))
                throw new ArgumentException("The ApplicationCAP2QualifyCAPOfferDetail rule expects the following objects to be passed: ICapApplicationDetail.");

            #endregion Check for allowed object type(s)

            ICapApplicationDetail cad = Parameters[0] as ICapApplicationDetail;
            IMortgageLoanAccount mla = cad.CapApplication.Account as IMortgageLoanAccount;
            double totalBondAmount = 0.0;

            foreach (IBond _bond in mla.SecuredMortgageLoan.Bonds)
            {
                totalBondAmount += _bond.BondRegistrationAmount;
            }

            if ((cad.Fee + cad.CapApplication.CurrentBalance) > totalBondAmount)
            {
                string msg = string.Format("Client does not qualify for {0}. Bond Registration Amount would be exceeded.", cad.CapTypeConfigurationDetail.CapType.Description);
                AddMessage(msg, msg, Messages);
                return 1;
            }
            else
                return 0;
        }
    }

    [RuleDBTag("ApplicationCAP2NextQuarter",
    "ApplicationCAP2NextQuarter",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCAP2NextQuarter")]
    [RuleInfo]
    public class ApplicationCAP2NextQuarter : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCAP2NextQuarter rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ICapApplication))
                throw new ArgumentException("The ApplicationCAP2NextQuarter rule expects the following objects to be passed: ICapApplication.");

            #endregion Check for allowed object type(s)

            ICapApplication capApplication = Parameters[0] as ICapApplication;
            ICapRepository capRepository = RepositoryFactory.GetRepository<ICapRepository>();
            IList<ICapApplication> openCapList = capRepository.GetCapOfferByAccountKey(capApplication.Account.Key);
            if (openCapList != null && openCapList.Count > 0)
            {
                for (int i = 0; i < openCapList.Count; i++)
                {
                    if (capApplication.Key != openCapList[i].Key &&
                        (openCapList[i].CapStatus.Key == Convert.ToInt32(CapStatuses.Open) ||
                        openCapList[i].CapStatus.Key == Convert.ToInt32(CapStatuses.ReadvanceRequired) ||
                        openCapList[i].CapStatus.Key == Convert.ToInt32(CapStatuses.PrepareForCredit) ||
                        openCapList[i].CapStatus.Key == Convert.ToInt32(CapStatuses.CreditApproval) ||
                        openCapList[i].CapStatus.Key == Convert.ToInt32(CapStatuses.CheckCashPayment) ||
                        openCapList[i].CapStatus.Key == Convert.ToInt32(CapStatuses.GrantedCap2Offer) ||
                        openCapList[i].CapStatus.Key == Convert.ToInt32(CapStatuses.AwaitingDocuments) ||
                        openCapList[i].CapStatus.Key == Convert.ToInt32(CapStatuses.AwaitingLA)
                        ))
                    {
                        string errorMessage = "An open cap offer already exists for this account";
                        AddMessage(errorMessage, errorMessage, Messages);
                    }
                }
            }
            return 0;
        }
    }

    [RuleDBTag("ApplicationCAP2SPVBack2Back",
      "ApplicationCAP2SPVBack2Back",
        "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCAP2SPVBack2Back")]
    [RuleInfo]
    public class ApplicationCAP2SPVBack2Back : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCAP2SPVBack2Back rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ICapApplication))
                throw new ArgumentException("The ApplicationCAP2SPVBack2Back rule expects the following objects to be passed: ICapApplication.");

            #endregion Check for allowed object type(s)

            ICapApplication capApplication = Parameters[0] as ICapApplication;
            ICapRepository capRepository = RepositoryFactory.GetRepository<ICapRepository>();
            IList<ICapApplication> openCapList = capRepository.GetCapOfferByAccountKey(capApplication.Account.Key);
            if (openCapList != null && openCapList.Count > 0)
            {
                for (int i = 0; i < openCapList.Count; i++)
                {
                    if (capApplication.Key != openCapList[i].Key &&
                        (openCapList[i].CapStatus.Key == Convert.ToInt32(CapStatuses.TakenUp)
                        ))
                    {
                        if (
                            (openCapList[i].CapTypeConfiguration.CapEffectiveDate < capApplication.CapTypeConfiguration.CapEffectiveDate &&
                            openCapList[i].CapTypeConfiguration.CapClosureDate > capApplication.CapTypeConfiguration.CapEffectiveDate.AddDays(5)) ||
                            (openCapList[i].CapTypeConfiguration.CapEffectiveDate < capApplication.CapTypeConfiguration.CapClosureDate &&
                            openCapList[i].CapTypeConfiguration.CapClosureDate > capApplication.CapTypeConfiguration.CapClosureDate)
                            )
                        {
                            string errorMessage = "A taken up offer already exists for this account that will be in effect in the next quarter";
                            AddMessage(errorMessage, errorMessage, Messages);
                        }
                    }
                }
            }
            return 0;
        }
    }

    [RuleDBTag("ApplicationCap2QualifyActiveCap",
       "ApplicationCap2QualifyActiveCap",
            "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2QualifyActiveCap")]
    [RuleInfo]
    public class ApplicationCap2QualifyActiveCap : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCap2QualifyActiveCap rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ICapApplication))
                throw new ArgumentException("The ApplicationCap2QualifyActiveCap rule expects the following objects to be passed: ICapApplication.");

            #endregion Check for allowed object type(s)

            ICapApplication capApplication = Parameters[0] as ICapApplication;
            IMortgageLoanAccount mla = capApplication.Account as IMortgageLoanAccount;
            IEventList<IFinancialAdjustment> financialAdjustments = mla.SecuredMortgageLoan.FinancialAdjustments;
            for (int i = 0; i < financialAdjustments.Count; i++)
            {
                if ((financialAdjustments[i].FinancialAdjustmentTypeSource.Key == Convert.ToInt32(FinancialAdjustmentTypeSources.CAP) ||
                    financialAdjustments[i].FinancialAdjustmentTypeSource.Key == Convert.ToInt32(FinancialAdjustmentTypeSources.CAP2)) &&
                    financialAdjustments[i].FinancialAdjustmentStatus.Key == Convert.ToInt32(FinancialAdjustmentStatuses.Active))
                {
                    if (financialAdjustments[i].FromDate < capApplication.CapTypeConfiguration.CapEffectiveDate &&
                        financialAdjustments[i].EndDate > capApplication.CapTypeConfiguration.CapEffectiveDate.AddDays(5)
                        )
                    {
                        string errorMessage = "The account has an active cap for the same period";
                        AddMessage(errorMessage, errorMessage, Messages);
                    }
                }
            }
            return 0;
        }
    }

    [RuleDBTag("ApplicationCAP2QualifyStatus",
          "ApplicationCAP2QualifyStatus",
            "SAHL.Rules.DLL",
          "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCAP2QualifyStatus")]
    [RuleInfo]
    public class ApplicationCAP2QualifyStatus : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ICapApplication capApplication = null;
            IAccount _account = null;

            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCAP2QualifyStatus rule expects a Domain object to be passed.");

            capApplication = Parameters[0] as ICapApplication;
            _account = Parameters[0] as IAccount;

            if (capApplication == null && _account == null)
                throw new ArgumentException("The ApplicationCAP2QualifyStatus rule expects the following objects to be passed: ICapApplication or IAccount");

            #endregion Check for allowed object type(s)

            if (capApplication != null)
                _account = capApplication.Account;

            if (_account.AccountStatus.Key != Convert.ToInt32(AccountStatuses.Open))
            {
                string errorMessage = String.Format("Cap is only permitted on Open accounts. Account {0} is currently in {1} status",
                    _account.Key, _account.AccountStatus.Description);
                AddMessage(errorMessage, errorMessage, Messages);
            }
            return 0;
        }
    }

    [RuleDBTag("ApplicationCap2QualifyUnderCancel",
         "ApplicationCap2QualifyUnderCancel",
            "SAHL.Rules.DLL",
         "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2QualifyUnderCancel")]
    [RuleInfo]
    public class ApplicationCap2QualifyUnderCancel : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ICapApplication capApplication = null;
            IAccount _account = null;

            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCap2QualifyUnderCancel rule expects a Domain object to be passed.");

            capApplication = Parameters[0] as ICapApplication;
            _account = Parameters[0] as IAccount;

            if (capApplication == null && _account == null)
                throw new ArgumentException("The ApplicationCap2QualifyUnderCancel rule expects the following objects to be passed: ICapApplication or IAccount.");

            #endregion Check for allowed object type(s)

            if (capApplication != null)
                _account = capApplication.Account;

            IAccountRepository accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            if (_account != null) // Another rule (above) will catch the (Account == null)
            {
                IReadOnlyEventList<IDetail> details = accountRepository.GetDetailByAccountKeyAndDetailType(_account.Key, (int)DetailTypes.UnderCancellation);

                if (details.Count > 0)
                {
                    string errorMessage = string.Format("Cap2 create not allowed. Account - {0} is under cancellation", _account.Key);
                    AddMessage(errorMessage, errorMessage, Messages);
                }
            }
            return 0;
        }
    }

    [RuleDBTag("ApplicationCap2QualifyInterestOnly",
        "ApplicationCap2QualifyInterestOnly",
            "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2QualifyInterestOnly")]
    [RuleInfo]
    public class ApplicationCap2QualifyInterestOnly : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCap2QualifyInterestOnly rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ICapApplication))
                throw new ArgumentException("The ApplicationCap2QualifyInterestOnly rule expects the following objects to be passed: ICapApplication.");

            #endregion Check for allowed object type(s)

            ICapApplication capApplication = Parameters[0] as ICapApplication;
            IMortgageLoanAccount mla = capApplication.Account as IMortgageLoanAccount;
            IEventList<IFinancialAdjustment> financialAdjustments = mla.SecuredMortgageLoan.FinancialAdjustments;
            for (int i = 0; i < financialAdjustments.Count; i++)
            {
                if (financialAdjustments[i].FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.InterestOnly &&
                    financialAdjustments[i].FinancialAdjustmentStatus.Key == (int)FinancialAdjustmentStatuses.Active)
                {
                    string errorMessage = "Cap is not permitted on Interest only accounts";
                    AddMessage(errorMessage, errorMessage, Messages);
                }
            }
            return 0;
        }
    }

    [RuleDBTag("ApplicationCap2QualifyProduct",
       "ApplicationCap2QualifyProduct",
            "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2QualifyProduct")]
    [RuleInfo]
    public class ApplicationCap2QualifyProduct : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ICapApplication capApplication = null;
            IAccount account = null;

            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCap2QualifyProduct rule expects a Domain object to be passed.");

            capApplication = Parameters[0] as ICapApplication;
            account = Parameters[0] as IAccount;

            if (capApplication == null && account == null)
                throw new ArgumentException("The ApplicationCap2QualifyProduct rule expects the following objects to be passed: ICapApplication or IAccount");

            #endregion Check for allowed object type(s)

            if (capApplication != null)
                account = capApplication.Account;

            ICapRepository capRepository = RepositoryFactory.GetRepository<ICapRepository>();
            bool productPermitsCap = capRepository.ProductQualifyForCap(account.Product.Key);
            if (productPermitsCap == false)
            {
                string errorMessage = "Account Product does not permit CAP";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationCap2QualifySPVLTV",
      "ApplicationCap2QualifySPVLTV",
            "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2QualifySPVLTV")]
    [RuleInfo]
    public class ApplicationCap2QualifySPVLTV : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCap2QualifySPVLTV rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ICapApplication))
                throw new ArgumentException("The ApplicationCap2QualifySPVLTV rule expects the following objects to be passed: ICapApplication.");

            #endregion Check for allowed object type(s)

            ICapApplication capApplication = Parameters[0] as ICapApplication;

            IMortgageLoanAccount mla = capApplication.Account as IMortgageLoanAccount;
            IEventList<ISPVMandate> mandates = mla.SecuredMortgageLoan.Account.SPV.SPVMandates;
            if (mandates.Count > 0)
            {
                if (mandates[0].MaxLTV.HasValue)
                {
                    double maxLTV = mandates[0].MaxLTV.Value;
                    double totalCurrentBalance = mla.LoanCurrentBalance;
                    double latestValuation = mla.SecuredMortgageLoan.GetActiveValuationAmount();
                    for (int i = 0; i < capApplication.CapApplicationDetails.Count; i++)
                    {
                        if (capApplication.CapApplicationDetails[i].CapStatus.Key == (int)CapStatuses.ReadvanceRequired
                            || capApplication.CapApplicationDetails[i].CapStatus.Key == (int)CapStatuses.PrepareForCredit)
                        {
                            double LTV = (totalCurrentBalance + capApplication.CapApplicationDetails[i].Fee) / latestValuation;
                            if (LTV >= maxLTV)
                            {
                                string errorMessage = "LTV greater than Max LTV for cap type " + capApplication.CapApplicationDetails[i].CapTypeConfigurationDetail.CapType.Description;
                                AddMessage(errorMessage, errorMessage, Messages);
                            }
                        }
                    }
                }
            }
            return 0;
        }
    }

    [RuleDBTag("ApplicationCap2QualifySPVPTI",
     "ApplicationCap2QualifySPVPTI",
            "SAHL.Rules.DLL",
     "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2QualifySPVPTI")]
    [RuleInfo]
    public class ApplicationCap2QualifySPVPTI : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCap2QualifySPVPTI rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ICapApplication))
                throw new ArgumentException("The ApplicationCap2QualifySPVPTI rule expects the following objects to be passed: ICapApplication.");

            #endregion Check for allowed object type(s)

            ICapApplication capApplication = Parameters[0] as ICapApplication;

            IMortgageLoanAccount mla = capApplication.Account as IMortgageLoanAccount;
            IEventList<ISPVMandate> mandates = mla.SecuredMortgageLoan.Account.SPV.SPVMandates;
            if (mandates.Count > 0)
            {
                if (mandates[0].MaxPTI.HasValue)
                {
                    double maxPTI = mandates[0].MaxPTI.Value;
                    double payment = mla.FixedPayment;
                    double householdIncome = mla.GetHouseholdIncome();

                    for (int i = 0; i < capApplication.CapApplicationDetails.Count; i++)
                    {
                        if (capApplication.CapApplicationDetails[i].CapStatus.Key == (int)CapStatuses.ReadvanceRequired
                            || capApplication.CapApplicationDetails[i].CapStatus.Key == (int)CapStatuses.PrepareForCredit)
                        {
                            double PTI = (capApplication.CapApplicationDetails[i].Payment) / householdIncome;
                            if (PTI >= maxPTI)
                            {
                                string errorMessage = "PTI greater than Max PTI for cap type " + capApplication.CapApplicationDetails[i].CapTypeConfigurationDetail.CapType.Description;
                                AddMessage(errorMessage, errorMessage, Messages);
                            }
                        }
                    }
                }
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationCap2QualifySPVBondAmount",
    "ApplicationCap2QualifySPVBondAmount",
            "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2QualifySPVBondAmount")]
    [RuleInfo]
    public class ApplicationCap2QualifySPVBondAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCap2QualifySPVBondAmount rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ICapApplication))
                throw new ArgumentException("The ApplicationCap2QualifySPVBondAmount rule expects the following objects to be passed: ICapApplication.");

            #endregion Check for allowed object type(s)

            ICapApplication capApplication = Parameters[0] as ICapApplication;

            IMortgageLoanAccount mla = capApplication.Account as IMortgageLoanAccount;
            IEventList<ISPVMandate> mandates = mla.SecuredMortgageLoan.Account.SPV.SPVMandates;
            if (mandates.Count > 0)
            {
                if (mandates[0].ExceedBondAmount.HasValue)
                {
                    double exceedBondAmount = mandates[0].ExceedBondAmount.Value;
                    IBond bond = mla.SecuredMortgageLoan.GetLatestRegisteredBond();
                    double totalCurrentBalance = mla.LoanCurrentBalance;
                    double bondAmount = bond.BondRegistrationAmount;
                    for (int i = 0; i < capApplication.CapApplicationDetails.Count; i++)
                    {
                        if ((totalCurrentBalance + capApplication.CapApplicationDetails[i].Fee - bondAmount) >= exceedBondAmount)
                        {
                            string errorMessage = "The CAP Fees cause the Current Balance to exceed the Bond Amount for cap type " + capApplication.CapApplicationDetails[i].CapTypeConfigurationDetail.CapType.Description;
                            AddMessage(errorMessage, errorMessage, Messages);
                        }
                    }
                }
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationCap2QualifySPVBondPercent",
   "ApplicationCap2QualifySPVBondPercent",
            "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2QualifySPVBondPercent")]
    [RuleInfo]
    public class ApplicationCap2QualifySPVBondPercent : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCap2QualifySPVBondPercent rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ICapApplication))
                throw new ArgumentException("The ApplicationCap2QualifySPVBondPercent rule expects the following objects to be passed: ICapApplication.");

            #endregion Check for allowed object type(s)

            ICapApplication capApplication = Parameters[0] as ICapApplication;

            IMortgageLoanAccount mla = capApplication.Account as IMortgageLoanAccount;
            IEventList<ISPVMandate> mandates = mla.SecuredMortgageLoan.Account.SPV.SPVMandates;
            if (mandates.Count > 0)
            {
                if (mandates[0].ExceedBondPercent.HasValue)
                {
                    double exceedBondPercent = mandates[0].ExceedBondPercent.Value;
                    IBond bond = mla.SecuredMortgageLoan.GetLatestRegisteredBond();
                    double totalCurrentBalance = mla.LoanCurrentBalance;
                    double bondAmount = bond.BondRegistrationAmount;
                    for (int i = 0; i < capApplication.CapApplicationDetails.Count; i++)
                    {
                        if (((totalCurrentBalance + capApplication.CapApplicationDetails[i].Fee - bondAmount) / bondAmount) >= exceedBondPercent)
                        {
                            string errorMessage = "The CAP Fees cause the Current Balance to exceed the Bond Percentage for cap type " + capApplication.CapApplicationDetails[i].CapTypeConfigurationDetail.CapType.Description;
                            AddMessage(errorMessage, errorMessage, Messages);
                        }
                    }
                }
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationCap2QualifySPVLoanAgreementPercent",
  "ApplicationCap2QualifySPVLoanAgreementPercent",
            "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2QualifySPVLoanAgreementPercent")]
    [RuleInfo]
    public class ApplicationCap2QualifySPVLoanAgreementPercent : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCap2QualifySPVLoanAgreementPercent rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ICapApplication))
                throw new ArgumentException("The ApplicationCap2QualifySPVLoanAgreementPercent rule expects the following objects to be passed: ICapApplication.");

            #endregion Check for allowed object type(s)

            ICapApplication capApplication = Parameters[0] as ICapApplication;

            IMortgageLoanAccount mla = capApplication.Account as IMortgageLoanAccount;
            IEventList<ISPVMandate> mandates = mla.SecuredMortgageLoan.Account.SPV.SPVMandates;
            if (mandates.Count > 0)
            {
                if (mandates[0].ExceedLoanAgreementPercent.HasValue)
                {
                    double exceedLoanAgreementPercent = mandates[0].ExceedLoanAgreementPercent.Value;
                    IBond bond = mla.SecuredMortgageLoan.GetLatestRegisteredBond();
                    double totalCurrentBalance = mla.LoanCurrentBalance;
                    double laa = bond.BondLoanAgreementAmount;
                    for (int i = 0; i < capApplication.CapApplicationDetails.Count; i++)
                    {
                        if (((totalCurrentBalance + capApplication.CapApplicationDetails[i].Fee - laa) / laa) >= exceedLoanAgreementPercent)
                        {
                            string errorMessage = "The CAP Fees cause the Current Balance to exceed the Loan Agreement Percentage for cap type " + capApplication.CapApplicationDetails[i].CapTypeConfigurationDetail.CapType.Description;
                            AddMessage(errorMessage, errorMessage, Messages);
                        }
                    }
                }
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationCap2QualifySPVMinBal",
            "ApplicationCap2QualifySPVMinBal",
            "SAHL.Rules.DLL",
            "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2QualifySPVMinBal")]
    [RuleInfo]
    [RuleParameterTag(new string[] { "@MinimumAmount,75000.00,10" })]
    public class ApplicationCap2QualifySPVMinBal : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCap2QualifySPVMinBal rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ICapApplication))
                throw new ArgumentException("The ApplicationCap2QualifySPVMinBal rule expects the following objects to be passed: ICapApplication.");

            #endregion Check for allowed object type(s)

            ICapApplication capApplication = Parameters[0] as ICapApplication;
            double minimumAmount = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            IMortgageLoanAccount mla = capApplication.Account as IMortgageLoanAccount;

            double totalCurrentBalance = mla.LoanCurrentBalance;
            for (int i = 0; i < capApplication.CapApplicationDetails.Count; i++)
            {
                if ((totalCurrentBalance + capApplication.CapApplicationDetails[i].Fee) <= minimumAmount)
                {
                    string errorMessage = String.Format("The Total Balance + Cap Fee for each Cap Type is not greater than {0} for cap type {1}",
                        minimumAmount.ToString(Constants.CurrencyFormat), capApplication.CapApplicationDetails[i].CapTypeConfigurationDetail.CapType.Description);
                    AddMessage(errorMessage, errorMessage, Messages);
                }
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationCap2QualifySPVMaxBal",
           "ApplicationCap2QualifySPVMaxBal",
            "SAHL.Rules.DLL",
           "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2QualifySPVMaxBal")]
    [RuleInfo]
    [RuleParameterTag(new string[] { "@MaximumAmount,5000000.00,10" })]
    public class ApplicationCap2QualifySPVMaxBal : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCap2QualifySPVMaxBal rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ICapApplication))
                throw new ArgumentException("The ApplicationCap2QualifySPVMaxBal rule expects the following objects to be passed: ICapApplication.");

            #endregion Check for allowed object type(s)

            ICapApplication capApplication = Parameters[0] as ICapApplication;
            double maximumAmount = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            IMortgageLoanAccount mla = capApplication.Account as IMortgageLoanAccount;

            double totalCurrentBalance = mla.LoanCurrentBalance;
            for (int i = 0; i < capApplication.CapApplicationDetails.Count; i++)
            {
                if ((totalCurrentBalance + capApplication.CapApplicationDetails[i].Fee) >= maximumAmount)
                {
                    string errorMessage = String.Format("The Total Balance + Cap Fee for each Cap Type is greater than {0} for cap type {1}",
                        maximumAmount.ToString(Constants.CurrencyFormat), capApplication.CapApplicationDetails[i].CapTypeConfigurationDetail.CapType.Description);
                    AddMessage(errorMessage, errorMessage, Messages);
                }
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationCap2QualifyDebtCounselling",
      "ApplicationCap2QualifyDebtCounselling",
        "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2QualifyDebtCounselling")]
    [RuleInfo]
    public class ApplicationCap2QualifyDebtCounselling : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCap2QualifyDebtCounselling rule expects a Domain object to be passed.");

            IAccount account = null;
            string msg = string.Empty;

            //CapApplication
            if (Parameters[0] is ICapApplication)
            {
                ICapApplication capApplication = Parameters[0] as ICapApplication;
                account = capApplication.Account;

                // We dont want to run this rule in these circumstances
                if (capApplication.CapStatus.Key == (int)CapStatuses.Expired || capApplication.CapStatus.Key == (int)CapStatuses.NotTakenUp
                    || capApplication.CapStatus.Key == (int)CapStatuses.OfferDeclined)
                    return 0;
            }

            //Account
            else if (Parameters[0] is IAccount)
            {
                account = Parameters[0] as IAccount;
            }

            if (account == null)
                return 0;

            //Check if the Account is under Debt Counselling
            if (account.UnderDebtCounselling)
            {
                msg = "This Account is undergoing Debt Counselling.";
                AddMessage(msg, msg, Messages);
            }

            // Check if any Legal Entities against the Account is under debt counselling
            foreach (IRole role in account.Roles)
            {
                if (role.LegalEntity.DebtCounsellingCases != null)
                {
                    foreach (IDebtCounselling dc in role.LegalEntity.DebtCounsellingCases)
                    {
                        msg = string.Format("{0} ({1}) on account ({2}) is under debt counselling.", role.LegalEntity.DisplayName, role.RoleType.Description, account.Key);
                        AddMessage(msg, msg, Messages);
                    }
                }
            }

            if (!string.IsNullOrEmpty(msg))
                return 1;

            return 0;
        }
    }

    /// <summary>
    /// If the payment type against this account is [Subsidy Payment] or [Direct Payment], Debit Order Account Payment option is not applicable
    /// </summary>
    [RuleDBTag("ApplicationCap2DebitOrderPaymentOption",
    "If the payment type against this account is [Subsidy Payment] or [Direct Payment], Debit Order Account Payment option is not applicable",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2DebitOrderPaymentOption")]
    [RuleInfo]
    public class ApplicationCap2DebitOrderPaymentOption : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCap2DebitOrderPaymentOption rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ICapApplication))
                throw new ArgumentException("The ApplicationCap2DebitOrderPaymentOption rule expects the following objects to be passed: ICapApplication.");

            #endregion Check for allowed object type(s)

            ICapApplication capApplication = Parameters[0] as ICapApplication;
            if (capApplication.CAPPaymentOption == null)
            {
                string errorMessage = "A refund option is required before you can continue.";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }

            //Only check the Variable Financial Service
            foreach (IFinancialService ifs in capApplication.Account.FinancialServices.Where(x => x.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan))
            {
                // if there is no FinancialServiceBankAccounts then throw a specific error.
                if (ifs.FinancialServiceBankAccounts.Count == 0)
                {
                    string errorMessage = "The Debit Order account has not been configured, please choose the Loan Account Refund Option.";
                    AddMessage(errorMessage, errorMessage, Messages);
                    return 0;
                }

                foreach (IFinancialServiceBankAccount ifsba in ifs.FinancialServiceBankAccounts)
                {
                    if (ifsba.GeneralStatus.Key != (int)GeneralStatuses.Active)
                    {
                        continue; //We don't care about inactive Financial Service Bank Accounts
                    }

                    if ((capApplication.CAPPaymentOption.Key == (int)CAPPaymentOptions.DebitOrderBankAccount) &&
                        (ifsba.FinancialServicePaymentType.Key == (int)FinancialServicePaymentTypes.SubsidyPayment
                        || ifsba.FinancialServicePaymentType.Key == (int)FinancialServicePaymentTypes.DirectPayment))
                    {
                        string errorMessage = String.Format("The payment type against this account is {0}, Debit Order Account Payment option is not applicable.", Enum.GetName(typeof(FinancialServicePaymentTypes), ifsba.FinancialServicePaymentType.Key));
                        AddMessage(errorMessage, errorMessage, Messages);
                        return 0;
                    }
                }
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationCap2CheckReadvancePosted",
    "Check that a Readvance has been posted before completing action Readvance done",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2CheckReadvancePosted")]
    [RuleInfo]
    public class ApplicationCap2CheckReadvancePosted : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCap2DebitOrderPaymentOption rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ICapApplication))
                throw new ArgumentException("The ApplicationCap2DebitOrderPaymentOption rule expects the following objects to be passed: ICapApplication.");

            #endregion Check for allowed object type(s)

            ICapApplication capApplication = Parameters[0] as ICapApplication;
            string query = UIStatementRepository.GetStatement("Rules.Cap", "ApplicationCap2CheckReadvancePosted");
            string sql = String.Format(query, capApplication.Key, (int)TransactionTypes.ReadvanceCAP);

            SimpleQuery<FinancialTransaction_DAO> ltQ = new SimpleQuery<FinancialTransaction_DAO>(QueryLanguage.Sql, sql);
            ltQ.AddSqlReturnDefinition(typeof(FinancialTransaction_DAO), "LT");
            FinancialTransaction_DAO[] res = ltQ.Execute();

            if (res == null || res.Length == 0)
            {
                string msg = "Cannot complete action as Readvance has not been posted";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("ApplicationCap2CurrentBalance",
    "Check SecuredMortgageLoan CurrentBalance on the Account before creating a CAP2 Offer",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2CurrentBalance")]
    [RuleInfo]
    public class ApplicationCap2CurrentBalance : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IAccount _account = null;
            IMortgageLoanAccount mortgageLoanAccount = null;

            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCap2CurrentBalance rule expects a Domain object to be passed.");

            _account = Parameters[0] as IAccount;

            if (_account == null)
                throw new ArgumentException("The ApplicationCap2CurrentBalance rule expects the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            if (_account.AccountStatus.Key == (int)AccountStatuses.Open)
            {
                mortgageLoanAccount = _account as IMortgageLoanAccount;

                if (mortgageLoanAccount.SecuredMortgageLoan.CurrentBalance == 0.0)
                {
                    string msg = String.Format("Cap2 Offer create not allowed as Mortgage Loan CurrentBalance is {0} on Account - {1}",
                        mortgageLoanAccount.SecuredMortgageLoan.CurrentBalance, _account.Key);
                    AddMessage(msg, msg, Messages);
                    return 0;
                }
            }
            return 1;
        }
    }

    [RuleDBTag("ApplicationCap2CapTypeConfig",
    "Check Cap Type Configuration exsists before creating a CAP2 Offer",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2CapTypeConfig")]
    [RuleInfo]
    public class ApplicationCap2CapTypeConfig : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IAccount _account = null;
            IMortgageLoanAccount mortgageLoanAccount = null;
            ICapRepository _capRepo = RepositoryFactory.GetRepository<ICapRepository>();

            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCap2CapTypeConfig rule expects a Domain object to be passed.");

            _account = Parameters[0] as IAccount;

            if (_account == null)
                throw new ArgumentException("The ApplicationCap2CapTypeConfig rule expects the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            if (_account.AccountStatus.Key == (int)AccountStatuses.Open)
            {
                mortgageLoanAccount = _account as IMortgageLoanAccount;
                ICapTypeConfiguration currentConfig = _capRepo.GetCurrentCapTypeConfigByResetConfigKey(mortgageLoanAccount.SecuredMortgageLoan.ResetConfiguration.Key);

                if (currentConfig == null)
                {
                    string errorMessage = String.Format("No Cap Type Configuration found for Reset Configuration - {0} on Account - {1}",
                        mortgageLoanAccount.SecuredMortgageLoan.ResetConfiguration.Description, _account.Key);
                    AddMessage(errorMessage, errorMessage, Messages);
                    return 0;
                }
            }
            return 1;
        }
    }

    [RuleDBTag("ApplicationCap2CheckReadvanceRequired",
    "Check to be completed before application can pass Readvance Required",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2CheckReadvanceRequired")]
    [RuleInfo]
    public class ApplicationCap2CheckReadvanceRequired : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCap2DebitOrderPaymentOption rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ICapApplication))
                throw new ArgumentException("The ApplicationCap2DebitOrderPaymentOption rule expects the following objects to be passed: ICapApplication.");

            #endregion Check for allowed object type(s)

            ICapApplication capApplication = Parameters[0] as ICapApplication;

            #region Code moved to Cap Repo

            //for (int i = 0; i < capApplication.CapApplicationDetails.Count; i++)
            //{
            //    if (capApplication.CapApplicationDetails[i].CapStatus.Key == (int)CapStatuses.ReadvanceRequired)
            //    {
            //        double currentBalance = 0;
            //        double loanAgreementAmount = 0.0;
            //        double incInLAA = 0.0;

            //        IMortgageLoanAccount mortgageLoanAccount = capApplication.Account as IMortgageLoanAccount;

            //        if (mortgageLoanAccount != null)
            //        {
            //            currentBalance += mortgageLoanAccount.SecuredMortgageLoan.CurrentBalance;

            //            foreach (IBond _bond in mortgageLoanAccount.SecuredMortgageLoan.Bonds)
            //            {
            //                loanAgreementAmount += _bond.BondLoanAgreementAmount;
            //            }
            //        }

            //        IAccountVariFixLoan varifixLoanAccount = capApplication.Account as IAccountVariFixLoan;
            //        if (varifixLoanAccount != null)
            //        {
            //            IMortgageLoan fixedmortgageLoan = varifixLoanAccount.FixedSecuredMortgageLoan;
            //            if (fixedmortgageLoan != null)
            //                currentBalance += fixedmortgageLoan.CurrentBalance;
            //        }

            //        incInLAA = (Math.Round(Convert.ToDouble(capApplication.CapApplicationDetails[i].Fee + currentBalance), 2) - Math.Round(loanAgreementAmount, 2));
            //        if (incInLAA > 0D)
            //        {
            //            string errorMessage = string.Format("Increase in LAA is greater than {0}. Further Advance Decision required. ", 0.0);
            //            AddMessage(errorMessage, errorMessage, Messages);
            //        }

            //        break;
            //    }
            //}

            #endregion Code moved to Cap Repo

            ICapRepository capRepo = RepositoryFactory.GetRepository<ICapRepository>();
            if (!capRepo.IsReAdvance(capApplication))
            {
                string errorMessage = string.Format("Increase in CLV is greater than {0}. Further Advance Decision required. ", 0.0);
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 1;
        }
    }

    [RuleDBTag("ApplicationCap2AllowFurtherLendingSPV",
    "Prevent a CAP 2 offer from proceeding to the point where the readvance is posted if it requires an increase in the LAA AND the SPV for the account no longer allows further lending",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2AllowFurtherLendingSPV")]
    [RuleInfo]
    public class ApplicationCap2AllowFurtherLendingSPV : BusinessRuleBase
    {
        public ApplicationCap2AllowFurtherLendingSPV(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            int accKey = (int)Parameters[0];
            int disTransTypeKey = (int)Parameters[1];
            int offerTypeKey = (int)Parameters[2];

            string sqlQuery = UIStatementRepository.GetStatement("Rule", "DisbursementSPVCheck");
            ParameterCollection prms = new ParameterCollection();

            //Helper.AddIntParameter(prms, "@accKey", accKey);
            //Helper.AddIntParameter(prms, "@disTransTypeKey", disTransTypeKey);
            //Helper.AddIntParameter(prms, "@OfferTypeKey", offerTypeKey);
            prms.Add(new SqlParameter("@accKey", accKey));
            prms.Add(new SqlParameter("@disTransTypeKey", disTransTypeKey));
            prms.Add(new SqlParameter("@OfferTypeKey", offerTypeKey));
            object obj = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(CapApplication_DAO), prms);
            string errMsg = string.Empty;

            if (obj != null)
                errMsg = Convert.ToString(obj);

            if (!string.IsNullOrEmpty(errMsg))
            {
                AddMessage(errMsg, errMsg, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationCap2AccountResetDateCheck",
    "A CAP offer cannot be created on an account that has not been through a reset date",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2AccountResetDateCheck")]
    [RuleInfo]
    public class ApplicationCap2AccountResetDateCheck : BusinessRuleBase
    {
        public ApplicationCap2AccountResetDateCheck(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCap2AccountResetCheck rule expects a Domain object to be passed.");

            ICapApplication cap = Parameters[0] as ICapApplication;
            IAccount acc = Parameters[0] as IAccount;

            if (cap == null && acc == null)
                throw new ArgumentException("The ApplicationCap2AccountResetCheck rule expects the following objects to be passed: ICapApplication or IAccount.");

            if (acc == null)
                acc = cap.Account;

            string sqlQuery = UIStatementRepository.GetStatement("Rules.Cap", "ApplicationCap2AccountResetDateCheck");
            ParameterCollection prms = new ParameterCollection();

            //Helper.AddIntParameter(prms, "@AccountKey", acc.Key);
            prms.Add(new SqlParameter("@AccountKey", acc.Key));
            object obj = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(CapApplication_DAO), prms);

            if (obj != null && Convert.ToInt32(obj) > 0)
                return 0;

            string errMsg = "Cap is not permitted on an account that has not been through a reset date.";
            AddMessage(errMsg, errMsg, Messages);
            return 0;
        }
    }

    [RuleDBTag("ApplicationCap2CheckLTVThreshold",
    "Readvances where the LTV is greater than 80.00% are going to automatically go to credit for approval.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Cap.ApplicationCap2CheckLTVThreshold")]
    [RuleInfo]
    public class ApplicationCap2CheckLTVThreshold : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCap2CheckLTVThreshold rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ICapApplication))
                throw new ArgumentException("The ApplicationCap2CheckLTVThreshold rule expects the following objects to be passed: ICapApplication.");

            #endregion Check for allowed object type(s)

            ICapApplication capApplication = Parameters[0] as ICapApplication;
            ICapRepository capRepo = RepositoryFactory.GetRepository<ICapRepository>();
            if (!capRepo.CheckLTVThreshold(capApplication))
            {
                IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
                double LTVThreshold = Convert.ToDouble(ctrlRepo.GetControlByDescription("CapLTVThreshold").ControlNumeric);
                string errorMessage = string.Format("Readvances where the LTV is greater than {0}% will go automatically to credit for approval", LTVThreshold * 100D);
                AddMessage(errorMessage, errorMessage, Messages);
            }
            return 1;
        }
    }
}