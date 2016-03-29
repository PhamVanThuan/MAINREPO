using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.Marketing
{
//    [RuleDBTag("CampaignDispositionCode",
//    "Miniumum Loan Amount",
//    "SAHL.Rules.DLL",
//   "SAHL.Common.BusinessModel.Rules.Marketing.CampaignDispositionCode")]
//    [RuleParameterTag(new string[] { "@MaxLoanAgreementAmount,1500000,9" })]
//    [RuleInfo]
//    public class CampaignDispositionCode : BusinessRuleBase
//    {
//        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
//        {

//            #   region Check for allowed object type(s)
//            if (Parameters.Length == 0)
//                throw new ArgumentException("The ApplicationAssetLiabilityRequiredLoanAmount rule expects a Domain object to be passed.");

//            if (!(Parameters[0] is IApplicationProductMortgageLoan))
//                throw new ArgumentException("The ApplicationAssetLiabilityRequiredLoanAmount rule expects the following objects to be passed: IApplicationMortgageLoan.");

//            GetRule(Messages, RuleName, IsError);
//            if (RuleItem.RuleParameters.Count < 1)
//                throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

//            #endregion

//            double maxLoanAgreementAmount = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

//            IApplicationProductMortgageLoan applicationProductMortgageLoan = Parameters[0] as IApplicationProductMortgageLoan;
//            I
//            bool isError = false;

//            if (applicationProductMortgageLoan.LoanAgreementAmount > maxLoanAgreementAmount)
//            {
//                isError  = true;

//                // Do we have at least one asset and liabilities rec?
//                foreach (IApplicationRole applicationRole in applicationProductMortgageLoan.Application.ApplicationRoles)
//                {
//                    if (applicationRole.LegalEntity.LegalEntityAssetLiabilities.Count > 0)
//                    {
//                        isError = false;
//                        break;
//                    }
//                }

//                if(isError)
//                    AddMessage(String.Format("For loan a Loan Agreement Amount over {0}, at least one asset and liabilities record is required", maxLoanAgreementAmount), "", Messages);
//            }

//            return 0;
//        }
//    }

//    [RuleDBTag("ApplicationAssetLiabilityRequiredEmploymentType",
//        "ApplicationAssetLiabilityRequiredEmploymentType",
//        "SAHL.Rules.DLL",
//        "SAHL.Common.BusinessModel.Rules.Products.ApplicationAssetLiabilityRequiredEmploymentType")]
//    [RuleInfo]
//    public class ApplicationAssetLiabilityRequiredEmploymentType : BusinessRuleBase
//    {
//        public override int ExecuteRule(SAHL.Common.Collections.Interfaces.IDomainMessageCollection Messages, params object[] Parameters)
//        {
//            #region Check for allowed object type(s)
//            if (Parameters.Length == 0)
//                throw new ArgumentException("The ApplicationAssetLiabilityRequiredEmploymentType rule expects a Domain object to be passed.");

//            if (!(Parameters[0] is IApplicationMortgageLoan))
//                throw new ArgumentException("The ApplicationAssetLiabilityRequiredEmploymentType rule expects the following objects to be passed: IApplicationMortgageLoan.");

//            GetRule(Messages, RuleName, IsError);

//            #endregion

//            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

//            if (applicationMortgageLoan.GetEmploymentType(false) == EmploymentTypes.SelfEmployed)
//            {
//                // Do we have at least one asset and liabilities rec?
//                foreach (IApplicationRole applicationRole in applicationMortgageLoan.ApplicationRoles)
//                {
//                    bool isSelfEmployed = false;

//                    foreach (IEmployment employment in applicationRole.LegalEntity.Employment)
//                    {
//                        if (employment.EmploymentType.Key == (int)EmploymentTypes.SelfEmployed && employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current)
//                            isSelfEmployed = true;
//                    }

//                    if (isSelfEmployed)
//                    {
//                        if (applicationRole.LegalEntity.LegalEntityAssetLiabilities.Count == 0)
//                        {
//                            AddMessage("For the Application Employment Type of Self Employed, an asset and liabilities statement is required for all self employed legal entities.", "", Messages);
//                            break;
//                        }
//                    }
//                }
//            }

//            return 0;
//        }
//    }

//    [RuleDBTag("ApplicationProperty",
//        "ApplicationProperty",
//        "SAHL.Rules.DLL",
//        "SAHL.Common.BusinessModel.Rules.Products.ApplicationProperty")]
//    [RuleInfo]
//    public class ApplicationProperty : BusinessRuleBase
//    {
//        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
//        {
//            #region Check for allowed object type(s)
//            if (Parameters.Length == 0)
//                throw new ArgumentException("The ApplicationProperty rule expects a Domain object to be passed.");

//            if (!(Parameters[0] is IApplicationMortgageLoan))
//                throw new ArgumentException("The ApplicationProperty rule expects the following objects to be passed: IApplicationMortgageLoan.");

//            GetRule(Messages, RuleName, IsError);

//            #endregion

//            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

//            if (applicationMortgageLoan.Property == null)
//            {
//                AddMessage("A Mortagage Loan Application requires an associated validated Property to be captured against it.", "A Mortagage Loan Application requires an associated validated Property to be captured against it.", Messages);
//            }
//            return 0;
//        }
//    }

//    [RuleDBTag("MortgageLoanLegalEntityEmploymentActive",
//        "MortgageLoanLegalEntityEmploymentActive",
//        "SAHL.Rules.DLL",
//        "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanLegalEntityEmploymentActive")]
//    [RuleInfo]
//    public class MortgageLoanLegalEntityEmploymentActive : BusinessRuleBase
//    {
//        public override int ExecuteRule(SAHL.Common.Collections.Interfaces.IDomainMessageCollection Messages, params object[] Parameters)
//        {
//            #region Check for allowed object type(s)
//            if (Parameters.Length == 0)
//                throw new ArgumentException("The MortgageLoanLegalEntityEmploymentActive rule expects a Domain object to be passed.");

//            if (!(Parameters[0] is IApplicationMortgageLoan || Parameters[0] is IMortgageLoanAccount))
//                throw new ArgumentException("The MortgageLoanLegalEntityEmploymentActive rule expects one of the following objects to be passed: IApplicationMortgageLoan or IMortgageLoanAccount.");

//            GetRule(Messages, RuleName, IsError);

//            #endregion


//            bool isEmploymentActive = false;

//            if (Parameters[0] is IApplicationMortgageLoan)
//            {
//                IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

//                foreach (IApplicationRole applicationRole in applicationMortgageLoan.ApplicationRoles)
//                {
//                    foreach (IEmployment employment in applicationRole.LegalEntity.Employment)
//                    {
//                        if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current)
//                        {
//                            isEmploymentActive = true;
//                            break;
//                        }
//                    }
//                }
//            }

//            if (Parameters[0] is IMortgageLoanAccount)
//            {
//                IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;

//                foreach (IRole role in mortgageLoanAccount.Roles)
//                {
//                    foreach (IEmployment employment in role.LegalEntity.Employment)
//                    {
//                        if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current)
//                        {
//                            isEmploymentActive = true;
//                            break;
//                        }
//                    }
//                }
//            }

//            if (!isEmploymentActive)
//                AddMessage("There must be at least one active employment against a mortgage loan.", "", Messages);

//            return 0;
//        }
//    }

//    [RuleDBTag("AccountMailingAddressAddressFormatFreeText",
//        "AccountMailingAddressAddressFormatFreeText",
//        "SAHL.Rules.DLL",
//        "SAHL.Common.BusinessModel.Rules.Products.AccountMailingAddressAddressFormatFreeText")]
//    [RuleInfo]
//    public class AccountMailingAddressAddressFormatFreeText : BusinessRuleBase
//    {
//        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
//        {
//            #region Check for allowed object type(s)
//            if (Parameters.Length == 0)
//                throw new ArgumentException("The AccountMailingAddressAddressFormatFreeText rule expects a Domain object to be passed.");

//            if (!(Parameters[0] is IApplicationMortgageLoan || Parameters[0] is IMortgageLoanAccount))
//                throw new ArgumentException("The AccountMailingAddressAddressFormatFreeText rule expects one of the following objects to be passed: IApplicationMortgageLoan, IMortgageLoanAccount.");

//            GetRule(Messages, RuleName, IsError);

//            #endregion

//            bool isMailingAddressFormatFreeText = false;

//            if (Parameters[0] is IApplicationMortgageLoan)
//            {
//                IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

//                foreach (IApplicationMailingAddress mailingAddress in applicationMortgageLoan.ApplicationMailingAddresses)
//                {
//                    if (mailingAddress.Address.AddressFormat.Key == (int)AddressFormats.FreeText)
//                    {
//                        isMailingAddressFormatFreeText = true;
//                        break;
//                    }
//                }
//            }

//            if (Parameters[0] is IMortgageLoanAccount)
//            {
//                IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;

//                foreach (IMailingAddress mailingAddress in mortgageLoanAccount.MailingAddresses)
//                {
//                    if (mailingAddress.Address.AddressFormat.Key == (int)AddressFormats.FreeText)
//                    {
//                        isMailingAddressFormatFreeText = true;
//                        break;
//                    }
//                }
//            }

//            if (isMailingAddressFormatFreeText)
//                AddMessage("A Freetext address cannot be used as the account mailing address.", "A Freetext address cannot be used as the account mailing address.", Messages);

//            return 0;
//        }
//    }

//    [RuleDBTag("ApplicationMortgageLoanPurchasePrice",
//        "ApplicationMortgageLoanPurchasePrice",
//        "SAHL.Rules.DLL",
//        "SAHL.Common.BusinessModel.Rules.Products.ApplicationMortgageLoanPurchasePrice")]
//    [RuleInfo]
//    public class ApplicationMortgageLoanPurchasePrice : BusinessRuleBase
//    {
//        public override int ExecuteRule(SAHL.Common.Collections.Interfaces.IDomainMessageCollection Messages, params object[] Parameters)
//        {
//            #region Check for allowed object type(s)
//            if (Parameters.Length == 0)
//                throw new ArgumentException("The ApplicationMortgageLoanPurchasePrice rule expects a Domain object to be passed.");

//            if (!(Parameters[0] is IApplicationMortgageLoanNewPurchase))
//                throw new ArgumentException("The ApplicationMortgageLoanPurchasePrice rule expects the following objects to be passed: IApplicationMortgageLoanNewPurchase.");

//            if (!(Parameters[0] is ISupportsVariableLoanApplicationInformation))
//                throw new ArgumentException("The ApplicationMortgageLoanPurchasePrice rule expects the following objects to be passed: ISupportsVariableLoanApplicationInformation.");

//            GetRule(Messages, RuleName, IsError);

//            #endregion

//            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = Parameters[0] as ISupportsVariableLoanApplicationInformation;

//#warning Need to finish this up ...
//            // if (supportsVariableLoanApplicationInformation.VariableLoanInformation.)
//            // {
//                //if (applicationMortgageLoan)
//                // {
//                    //IApplicationMortgageLoanNewPurchase d;
//                    // d.pu
//                // }

//                // AddMessage("There must be at least one active employment against a mortgage loan.", "", Messages);
//            // }
//            return 0;
//        }
//    }

//    [RuleDBTag("ApplicationMortgageLoanHouseholdIncome",
//        "ApplicationMortgageLoanHouseholdIncome",
//        "SAHL.Rules.DLL",
//        "SAHL.Common.BusinessModel.Rules.Products.ApplicationMortgageLoanHouseholdIncome")]
//    [RuleInfo]
//    public class ApplicationMortgageLoanHouseholdIncome : BusinessRuleBase
//    {
//        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
//        {
//            #region Check for allowed object type(s)
//            if (Parameters.Length == 0)
//                throw new ArgumentException("The ApplicationMortgageLoanHouseholdIncome rule expects a Domain object to be passed.");

//            if (!(Parameters[0] is ISupportsVariableLoanApplicationInformation))
//                throw new ArgumentException("The ApplicationMortgageLoanHouseholdIncome rule expects the following objects to be passed: ISupportsVariableLoanApplicationInformation.");

//            GetRule(Messages, RuleName, IsError);

//            #endregion

//            if (Parameters[0] is ISupportsVariableLoanApplicationInformation)
//            {
//                ISupportsVariableLoanApplicationInformation variableLoanApplicationInformation = (ISupportsVariableLoanApplicationInformation)Parameters[0];

//                if (!(variableLoanApplicationInformation.VariableLoanInformation.HouseholdIncome.HasValue && variableLoanApplicationInformation.VariableLoanInformation.HouseholdIncome.Value > 0))
//                    AddMessage("Household Income is a required field.", "Household Income is a required field.", Messages);
//            }
            
//            return 0;
//        }
//    }

//    [RuleDBTag("ApplicationMortgageLoanBondToRegister",
//    "Minimum Bond to Register",
//    "SAHL.Rules.DLL",
//    "SAHL.Common.BusinessModel.Rules.Products.ApplicationMortgageLoanBondToRegister")]
//    [RuleParameterTag(new string[] { "@MinBondToRegister,150000,9" })]
//    [RuleInfo]
//    public class ApplicationMortgageLoanBondToRegister : BusinessRuleBase
//    {
//        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
//        {

//            #region Check for allowed object type(s)
//            if (Parameters.Length == 0)
//                throw new ArgumentException("The ApplicationMortgageLoanBondToRegister rule expects a Domain object to be passed.");

//            if (!(Parameters[0] is ISupportsVariableLoanApplicationInformation))
//                throw new ArgumentException("The ApplicationMortgageLoanBondToRegister rule expects the following objects to be passed: ISupportsVariableLoanApplicationInformation.");

//            GetRule(Messages, RuleName, IsError);
//            if (RuleItem.RuleParameters.Count == 0)
//                throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

//            #endregion

//            double bondToRegister = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

//            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = Parameters[0] as ISupportsVariableLoanApplicationInformation;

//            if (!(supportsVariableLoanApplicationInformation.VariableLoanInformation.BondToRegister.HasValue && supportsVariableLoanApplicationInformation.VariableLoanInformation.BondToRegister.Value >= bondToRegister))
//                AddMessage(String.Format("The Bond to Register amount must be greater than R {0}.", bondToRegister), "", Messages);

//            return 0;
//        }
//    }

//    [RuleDBTag("ApplicationCreateLegalEntityMinimum",
//   "ApplicationCreateLegalEntityMinimum",
//    "SAHL.Rules.DLL",
//    "SAHL.Common.BusinessModel.Rules.Products.ApplicationCreateLegalEntityMinimum")]
//    [RuleInfo]
//    public class ApplicationCreateLegalEntityMinimum : BusinessRuleBase
//    {
//        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
//        {

//            #region Check for allowed object type(s)
//            if (Parameters.Length == 0)
//                throw new ArgumentException("The ApplicationCreateLegalEntityMinimum rule expects a Domain object to be passed.");

//            if (!(Parameters[0] is IApplicationMortgageLoan))
//                throw new ArgumentException("The ApplicationCreateLegalEntityMinimum rule expects the following objects to be passed: IApplicationMortgageLoan.");

//            GetRule(Messages, RuleName, IsError);

//            #endregion

//            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

//            if (applicationMortgageLoan.NumApplicants == 0)
//                AddMessage("A Mortgage Loan must have at least one active Legal Entity role.", "A Mortgage Loan must have at least one active Legal Entity role.", Messages);

//            return 0;
//        }
//    }

//    [RuleDBTag("ApplicationMailingAddress",
//   "ApplicationMailingAddress",
//    "SAHL.Rules.DLL",
//    "SAHL.Common.BusinessModel.Rules.Products.ApplicationMailingAddress")]
//    [RuleInfo]
//    public class ApplicationMailingAddress : BusinessRuleBase
//    {
//        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
//        {

//            #region Check for allowed object type(s)
//            if (Parameters.Length == 0)
//                throw new ArgumentException("The ApplicationMailingAddress rule expects a Domain object to be passed.");

//            if (!(Parameters[0] is IApplicationProductMortgageLoan))
//                throw new ArgumentException("The ApplicationMailingAddress rule expects the following objects to be passed: IApplicationMortgageLoan.");

//            GetRule(Messages, RuleName, IsError);

//            #endregion

//            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

//            bool doesMailingAddressBelongToLE;

//            if (applicationMortgageLoan.ApplicationMailingAddresses.Count == 0)
//                AddMessage("Each Application must have one valid postal or residential address.", "Each Appllication must have one valid postal or residential address.", Messages);
//            else
//            {
//                // Does the address belong to one of the roleplayers?
//                // applicationMortgageLoan.ApplicationMailingAddresses

//                foreach (IApplicationMailingAddress applicationMailingAddress in applicationMortgageLoan.ApplicationMailingAddresses)
//                {

//                    doesMailingAddressBelongToLE = false;

//                    // applicationMailingAddress.Address.Key
//                    foreach (IApplicationRole applicationRole in applicationMortgageLoan.ApplicationRoles)
//                    {
//                        if ((applicationRole.ApplicationRoleType.Key == (int)RoleTypes.LeadSuretor 
//                            || applicationRole.ApplicationRoleType.Key == (int)RoleTypes.LeadMainApplicant)
//                            && applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
//                        {
//                            foreach (ILegalEntityAddress legalEntityAddress in applicationRole.LegalEntity.LegalEntityAddresses)
//                            {
//                                if (legalEntityAddress.Address.Key == applicationMailingAddress.Address.Key)
//                                {
//                                    doesMailingAddressBelongToLE = true;
//                                    break;
//                                }
//                            }
//                        }

//                        if (!doesMailingAddressBelongToLE)
//                            break;
//                    }

//                    if (!doesMailingAddressBelongToLE)
//                    {
//                        AddMessage("The Application Mailing Address must belong to one of the legal entities playing one of Lead - Suretor or Lead - Main Applicant.", "The Application Mailing Address must belong to one of the legal entities playing one of Lead - Suretor or Lead - Main Applicant", Messages);
//                        break;
//                    }
//                }
//            }

//            return 0;
//        }
//    }

//    [RuleDBTag("MortgageLoanLegalEntityBankAccount",
//   "MortgageLoanLegalEntityBankAccount",
//    "SAHL.Rules.DLL",
//    "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanLegalEntityBankAccount")]
//    [RuleInfo]
//    public class MortgageLoanLegalEntityBankAccount : BusinessRuleBase
//    {
//        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
//        {

//            #region Check for allowed object type(s)
//            if (Parameters.Length == 0)
//                throw new ArgumentException("The MortgageLoanLegalEntityBankAccount rule expects a Domain object to be passed.");

//            if (!(Parameters[0] is IApplicationMortgageLoan))
//                throw new ArgumentException("The MortgageLoanLegalEntityBankAccount rule expects the following objects to be passed: IApplicationMortgageLoan.");

//            GetRule(Messages, RuleName, IsError);

//            #endregion

//            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

//#warning This doesn't work - come back and fix
//            // if(applicationMortgageLoan.BankAccounts.Count == 0)
////                AddMessage("There must be at least one Bank Account for an Application.", "There must be at least one Bank Account for an Application.", Messages);

//            return 0;
//        }
//    }

//    [RuleDBTag("MortgageLoanMultipleApplication",
//   "MortgageLoanMultipleApplication",
//    "SAHL.Rules.DLL",
//    "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanMultipleApplication")]
//    [RuleInfo]
//    public class MortgageLoanMultipleApplication : BusinessRuleBase
//    {
//        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
//        {

//            #region Check for allowed object type(s)
//            if (Parameters.Length == 0)
//                throw new ArgumentException("The MortgageLoanMultipleApplication rule expects a Domain object to be passed.");

//            if (!(Parameters[0] is IApplicationProductMortgageLoan))
//                throw new ArgumentException("The MortgageLoanMultipleApplication rule expects the following objects to be passed: IApplicationMortgageLoan.");

//            GetRule(Messages, RuleName, IsError);

//            #endregion

//            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;
//            if (!(applicationMortgageLoan.Account == null))
//            {
//                // Try find another application that is currently open.
//                foreach (IApplication application in applicationMortgageLoan.Account.Applications)
//                {
//                    if (applicationMortgageLoan.ApplicationStatus.Key == (int)OfferStatuses.Open
//                        && application.ApplicationStatus.Key == (int)OfferStatuses.Open
//                        && application.Key != applicationMortgageLoan.Key
//                        && application is IApplicationMortgageLoan)
//                    {
//                        AddMessage("There's already another Mortgage Loan Application in progress.", "There's already another Mortgage Loan Application in progress.", Messages);
//                        break;
//                    }
//                }
//            }

//            return 0;
//        }
//    }

//    [RuleDBTag("ApplicationMortgageLoanExistingLoanAmount",
//        "ApplicationMortgageLoanExistingLoanAmount",
//        "SAHL.Rules.DLL",
//        "SAHL.Common.BusinessModel.Rules.Products.ApplicationMortgageLoanExistingLoanAmount")]
//    [RuleInfo]
//    public class ApplicationMortgageLoanExistingLoanAmount : BusinessRuleBase
//    {
//        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
//        {
//            #region Check for allowed object type(s)
//            if (Parameters.Length == 0)
//                throw new ArgumentException("The ApplicationMortgageLoanExistingLoanAmount rule expects a Domain object to be passed.");

//            if (!(Parameters[0] is IApplicationMortgageLoanSwitch))
//                throw new ArgumentException("The ApplicationMortgageLoanExistingLoanAmount rule expects the following objects to be passed: IApplicationMortgageLoanSwitch.");

//            if (!(Parameters[0] is ISupportsVariableLoanApplicationInformation))
//                throw new ArgumentException("The ApplicationMortgageLoanExistingLoanAmount rule expects the following objects to be passed: ISupportsVariableLoanApplicationInformation.");
            
//            GetRule(Messages, RuleName, IsError);

//            #endregion

//            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = Parameters[0] as ISupportsVariableLoanApplicationInformation;

//            if (!supportsVariableLoanApplicationInformation.VariableLoanInformation.ExistingLoan.HasValue
//                || supportsVariableLoanApplicationInformation.VariableLoanInformation.ExistingLoan.Value == 0)
//                AddMessage("There must be at least one active employment against a mortgage loan.", "", Messages);

//            return 0;
//        }
//    }

}
