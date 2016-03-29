using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SAHL.Common;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.Products
{
    #region Application MortgageLoan Spec

    [RuleDBTag("ApplicationAssetLiabilityRequiredLoanAmount",
    "Miniumum Loan Amount",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.ApplicationAssetLiabilityRequiredLoanAmount")]
    [RuleParameterTag(new string[] { "@MaxLoanAgreementAmount,1500000,9" })]
    [RuleInfo]
    public class ApplicationAssetLiabilityRequiredLoanAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationAssetLiabilityRequiredLoanAmount rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationMortgageLoan))
                throw new ArgumentException("The ApplicationAssetLiabilityRequiredLoanAmount rule expects the following objects to be passed: IApplicationMortgageLoan.");

            if (RuleItem.RuleParameters.Count < 1)
                throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

            IApplicationProductMortgageLoan applicationProductMortgageLoan = (IApplicationProductMortgageLoan)applicationMortgageLoan.CurrentProduct;
            double maxLoanAgreementAmount = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            bool isError = false;

            if (applicationProductMortgageLoan.LoanAgreementAmount >= maxLoanAgreementAmount)
            {
                isError = true;

                // Do we have at least one asset and liabilities rec?
                foreach (IApplicationRole applicationRole in applicationProductMortgageLoan.Application.ApplicationRoles)
                {
                    if (applicationRole.LegalEntity.LegalEntityAssetLiabilities.Count > 0)
                    {
                        isError = false;
                        break;
                    }
                }

                if (isError)
                    AddMessage(String.Format("The Loan Agreement Amount is over {0:c}; at least one Asset and Liabilities record is required", maxLoanAgreementAmount), "", Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationAssetLiabilityRequiredEmploymentType",
        "ApplicationAssetLiabilityRequiredEmploymentType",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ApplicationAssetLiabilityRequiredEmploymentType")]
    [RuleInfo]
    public class ApplicationAssetLiabilityRequiredEmploymentType : BusinessRuleBase
    {
        public override int ExecuteRule(SAHL.Common.Collections.Interfaces.IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationAssetLiabilityRequiredEmploymentType rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The ApplicationAssetLiabilityRequiredEmploymentType rule expects the following objects to be passed: IApplication.");

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

            if (applicationMortgageLoan == null || applicationMortgageLoan.Key == 0)
                return 1;

            ISupportsVariableLoanApplicationInformation svli = applicationMortgageLoan.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            if (svli.VariableLoanInformation.EmploymentType.Key == (int)EmploymentTypes.SelfEmployed)
            {
                // Do we have at least one asset and liabilities rec?
                foreach (IApplicationRole applicationRole in applicationMortgageLoan.ApplicationRoles)
                {
                    bool isSelfEmployed = false;

                    foreach (IEmployment employment in applicationRole.LegalEntity.Employment)
                    {
                        if (employment.EmploymentType.Key == (int)EmploymentTypes.SelfEmployed && employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current)
                            isSelfEmployed = true;
                    }

                    if (isSelfEmployed)
                    {
                        if (applicationRole.LegalEntity.LegalEntityAssetLiabilities.Count == 0)
                        {
                            AddMessage("For the Application Employment Type of Self Employed, an asset and liabilities statement is required for all self employed legal entities.", "", Messages);
                            break;
                        }
                    }
                }
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationCheckEmploymentTypeIsNotUnknown",
    "ApplicationCheckEmploymentTypeIsNotUnknown",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.ApplicationCheckEmploymentTypeIsNotUnknown")]
    [RuleInfo]
    public class ApplicationCheckEmploymentTypeIsNotUnknown : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCheckEmploymentTypeIsNotUnknown rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The ApplicationCheckEmploymentTypeIsNotUnknown rule expects the following objects to be passed: IApplication.");

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

            if (applicationMortgageLoan == null || applicationMortgageLoan.Key == 0)
                return 1;

            ISupportsVariableLoanApplicationInformation svli = applicationMortgageLoan.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            if (svli.VariableLoanInformation.EmploymentType.Key == (int)EmploymentTypes.Unknown)
            {
                AddMessage("The Employment Type cannot be set to Unknown against a Mortgage Loan.", "", Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("ApplicationProperty",
        "ApplicationProperty",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ApplicationProperty")]
    [RuleInfo]
    public class ApplicationProperty : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationProperty rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The ApplicationProperty rule expects the following objects to be passed: IApplication.");

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

            if (applicationMortgageLoan == null || applicationMortgageLoan.Key == 0)
                return 1;

            if (applicationMortgageLoan.Property == null)
            {
                AddMessage("A Mortgage Loan Application requires an associated validated Property to be captured against it.", "A Mortgage Loan Application requires an associated validated Property to be captured against it.", Messages);
            }
            return 0;
        }
    }

    [RuleDBTag("MortgageLoanLegalEntityEmploymentActive",
        "MortgageLoanLegalEntityEmploymentActive",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanLegalEntityEmploymentActive")]
    [RuleInfo]
    public class MortgageLoanLegalEntityEmploymentActive : BusinessRuleBase
    {
        public override int ExecuteRule(SAHL.Common.Collections.Interfaces.IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The MortgageLoanLegalEntityEmploymentActive rule expects a Domain object to be passed.");

            IApplication application = Parameters[0] as IApplication;
            IAccount account = Parameters[0] as IAccount;

            // make sure that the parameter is either an existing account or an existing application
            if (application == null && account == null)
                throw new ArgumentException("The MortgageLoanLegalEntityEmploymentActive rule expects one of the following objects to be passed: IApplication or IAccount.");

            // if the object is not a mortgage loan object, we can get out of here
            IApplicationMortgageLoan applicationMortgageLoan = application as IApplicationMortgageLoan;
            IMortgageLoanAccount mortgageLoanAccount = account as IMortgageLoanAccount;
            if ((application == null || application.Key == 0) && (account == null || account.Key == 0))
                return 1;

            if (applicationMortgageLoan != null)
            {
                foreach (IApplicationRole applicationRole in applicationMortgageLoan.ApplicationRoles)
                {
                    foreach (IEmployment employment in applicationRole.LegalEntity.Employment)
                    {
                        if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current)
                        {
                            // found active employment - exit here
                            return 1;
                        }
                    }
                }
            }
            else if (mortgageLoanAccount != null)
            {
                foreach (IRole role in mortgageLoanAccount.Roles)
                {
                    foreach (IEmployment employment in role.LegalEntity.Employment)
                    {
                        if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current)
                        {
                            // found active employment - exit here
                            return 1;
                        }
                    }
                }
            }

            // if we've got this far, the rule has failed as no active employment records have been found
            // and we need to add an error message
            AddMessage("There must be at least one active employment against a mortgage loan.", "", Messages);
            return 0;
        }
    }

    [RuleDBTag("AccountMailingAddressAddressFormatFreeText",
        "AccountMailingAddressAddressFormatFreeText",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.AccountMailingAddressAddressFormatFreeText")]
    [RuleInfo]
    public class AccountMailingAddressAddressFormatFreeText : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The AccountMailingAddressAddressFormatFreeText rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication || Parameters[0] is IAccount))
                throw new ArgumentException("The AccountMailingAddressAddressFormatFreeText rule expects one of the following objects to be passed: IApplication, IAccount.");

            #endregion Check for allowed object type(s)

            bool isMailingAddressFormatFreeText = false;

            if (Parameters[0] is IApplication)
            {
                IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;
                if (applicationMortgageLoan == null)
                    return 1;

                foreach (IApplicationMailingAddress mailingAddress in applicationMortgageLoan.ApplicationMailingAddresses)
                {
                    if (mailingAddress.Address.AddressFormat.Key == (int)AddressFormats.FreeText)
                    {
                        isMailingAddressFormatFreeText = true;
                        break;
                    }
                }
            }

            if (Parameters[0] is IAccount)
            {
                IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;
                if (mortgageLoanAccount == null)
                    return 1;

                foreach (IMailingAddress mailingAddress in mortgageLoanAccount.MailingAddresses)
                {
                    if (mailingAddress.Address.AddressFormat.Key == (int)AddressFormats.FreeText)
                    {
                        isMailingAddressFormatFreeText = true;
                        break;
                    }
                }
            }

            if (isMailingAddressFormatFreeText)
                AddMessage("A Freetext address cannot be used as the account mailing address.", "A Freetext address cannot be used as the account mailing address.", Messages);

            return 0;
        }
    }

    [RuleDBTag("ApplicationMortgageLoanPurchasePrice",
        "ApplicationMortgageLoanPurchasePrice",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ApplicationMortgageLoanPurchasePrice")]
    [RuleInfo]
    public class ApplicationMortgageLoanPurchasePrice : BusinessRuleBase
    {
        public override int ExecuteRule(SAHL.Common.Collections.Interfaces.IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationMortgageLoanPurchasePrice rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationMortgageLoanNewPurchase))
                throw new ArgumentException("The ApplicationMortgageLoanPurchasePrice rule expects the following objects to be passed: IApplicationMortgageLoanNewPurchase.");

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoanNewPurchase applicationMortgageLoanNewPurchase = Parameters[0] as IApplicationMortgageLoanNewPurchase;

            if (!(applicationMortgageLoanNewPurchase.PurchasePrice.HasValue && applicationMortgageLoanNewPurchase.PurchasePrice.Value > 0.0))
            {
                string errorMessage = "The Purchase Price is mandatory for New Purchase.";
                AddMessage(errorMessage, errorMessage, Messages);
            }
            return 0;
        }
    }

    [RuleDBTag("ApplicationMortgageLoanHouseholdIncome",
        "ApplicationMortgageLoanHouseholdIncome",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ApplicationMortgageLoanHouseholdIncome")]
    [RuleInfo]
    public class ApplicationMortgageLoanHouseholdIncome : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationMortgageLoanHouseholdIncome rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ISupportsVariableLoanApplicationInformation))
                throw new ArgumentException("The ApplicationMortgageLoanHouseholdIncome rule expects the following objects to be passed: ISupportsVariableLoanApplicationInformation.");

            #endregion Check for allowed object type(s)

            if (Parameters[0] is ISupportsVariableLoanApplicationInformation)
            {
                ISupportsVariableLoanApplicationInformation variableLoanApplicationInformation = (ISupportsVariableLoanApplicationInformation)Parameters[0];

                if (!(variableLoanApplicationInformation.VariableLoanInformation.HouseholdIncome.HasValue && variableLoanApplicationInformation.VariableLoanInformation.HouseholdIncome.Value > 0))
                    AddMessage("Household Income is a required field.", "Household Income is a required field.", Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationMortgageLoanBondToRegister",
    "Minimum Bond to Register",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.ApplicationMortgageLoanBondToRegister")]
    [RuleParameterTag(new string[] { "@MinBondToRegister,150000,9" })]
    [RuleInfo]
    public class ApplicationMortgageLoanBondToRegister : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationMortgageLoanBondToRegister rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ISupportsVariableLoanApplicationInformation))
                throw new ArgumentException("The ApplicationMortgageLoanBondToRegister rule expects the following objects to be passed: ISupportsVariableLoanApplicationInformation.");

            if (RuleItem.RuleParameters.Count == 0)
                throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

            #endregion Check for allowed object type(s)

            double bondToRegister = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = Parameters[0] as ISupportsVariableLoanApplicationInformation;

            if (!(supportsVariableLoanApplicationInformation.VariableLoanInformation.BondToRegister.HasValue && supportsVariableLoanApplicationInformation.VariableLoanInformation.BondToRegister.Value >= bondToRegister))
                AddMessage(String.Format("The Bond to Register amount must be greater than R {0}.", bondToRegister), "", Messages);

            return 0;
        }
    }

    [RuleDBTag("ApplicationCreateLegalEntityMinimum",
   "ApplicationCreateLegalEntityMinimum",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.ApplicationCreateLegalEntityMinimum")]
    [RuleInfo]
    public class ApplicationCreateLegalEntityMinimum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplicationMortgageLoan applicationMortgageLoan = null;
            IApplicationUnknown applicationUnknown = null;
            IReadOnlyEventList<IApplicationRole> applicationRoles = null;

            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationCreateLegalEntityMinimum rule expects a Domain object to be passed.");

            if (Parameters[0] is IApplication)
            {
                applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;
                if (applicationMortgageLoan == null)
                    return 1;

                applicationRoles = new ReadOnlyEventList<IApplicationRole>(applicationMortgageLoan.ApplicationRoles);
            }
            else if (Parameters[0] is IApplicationUnknown)
            {
                applicationUnknown = Parameters[0] as IApplicationUnknown;
                if (applicationUnknown == null)
                    return 1;

                applicationRoles = new ReadOnlyEventList<IApplicationRole>(applicationUnknown.ApplicationRoles);
            }
            else
            {
                throw new ArgumentException("The ApplicationCreateLegalEntityMinimum rule expects the following objects to be passed: IApplicationMortgageLoan or IApplicationUnknown.");
            }

            #endregion Check for allowed object type(s)

            // As per ticket 8798/8813 and documentation, the application should contain at least one Legal Entity
            // of type "Lead Main Applicant or Main Applicant".
            // This should execute of Add / Update / Delete.
            foreach (IApplicationRole appRole in applicationRoles)
            {
                if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                    (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant)
                    )
                {
                    return 1;
                }
            }

            // This would not work as it only takes into account, Main Applicant and Suretor.
            //if (applicationMortgageLoan.NumApplicants == 0)
            //    AddMessage("A Mortgage Loan must have at least one active Legal Entity role.", "A Mortgage Loan must have at least one active Legal Entity role.", Messages);
            String msg = string.Format("A Mortgage Loan must have at least one active Legal Entity role of type {0} or {1}", OfferRoleTypes.LeadMainApplicant, OfferRoleTypes.MainApplicant);
            AddMessage(msg, msg, Messages);
            return 1;
        }
    }

    [RuleDBTag("ApplicationMailingAddress",
   "ApplicationMailingAddress",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.ApplicationMailingAddress")]
    [RuleInfo]
    public class ApplicationMailingAddress : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationMailingAddress rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The ApplicationMailingAddress rule expects the following objects to be passed: IApplication.");

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

            if (applicationMortgageLoan == null || applicationMortgageLoan.Key == 0 || applicationMortgageLoan.ApplicationType == null)
                return 1;

            if (applicationMortgageLoan.ApplicationType.Key == (int)OfferTypes.NewPurchaseLoan ||
                applicationMortgageLoan.ApplicationType.Key == (int)OfferTypes.SwitchLoan ||
                applicationMortgageLoan.ApplicationType.Key == (int)OfferTypes.RefinanceLoan)
            {
                bool doesMailingAddressBelongToLE;

                if (applicationMortgageLoan.ApplicationMailingAddresses.Count == 0)
                    AddMessage("Each Application must have one valid postal or residential mailing address.", "Each Application must have one valid postal or residential mailing address.", Messages);
                else
                {
                    // Does the address belong to one of the roleplayers?
                    foreach (IApplicationMailingAddress applicationMailingAddress in applicationMortgageLoan.ApplicationMailingAddresses)
                    {
                        doesMailingAddressBelongToLE = false;

                        // applicationMailingAddress.Address.Key
                        foreach (IApplicationRole applicationRole in applicationMortgageLoan.ApplicationRoles)
                        {
                            if ((applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor
                                || applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                                || applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                                || applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor)
                                && applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
                            {
                                foreach (ILegalEntityAddress legalEntityAddress in applicationRole.LegalEntity.LegalEntityAddresses)
                                {
                                    if (legalEntityAddress.Address.Key == applicationMailingAddress.Address.Key)
                                    {
                                        doesMailingAddressBelongToLE = true;
                                        break;
                                    }
                                }
                            }

                            if (doesMailingAddressBelongToLE)
                                break;
                        }

                        if (!doesMailingAddressBelongToLE)
                        {
                            AddMessage("The Application Mailing Address must belong to one of the Applicants.", "The Application Mailing Address must belong to one of the Applicants.", Messages);
                            break;
                        }
                    }
                }
            }

            return 0;
        }
    }

    // [RuleDBTag("MortgageLoanLegalEntityBankAccount",
    //"MortgageLoanLegalEntityBankAccount",
    // "SAHL.Rules.DLL",
    // "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanLegalEntityBankAccount")]
    // [RuleInfo]
    // public class MortgageLoanLegalEntityBankAccount : BusinessRuleBase
    // {
    //     public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
    //     {
    //         #region Check for allowed object type(s)
    //         if (Parameters.Length == 0)
    //             throw new ArgumentException("The MortgageLoanLegalEntityBankAccount rule expects a Domain object to be passed.");

    //         if (!(Parameters[0] is IApplicationMortgageLoan))
    //             throw new ArgumentException("The MortgageLoanLegalEntityBankAccount rule expects the following objects to be passed: IApplicationMortgageLoan.");

    //         #endregion

    //         IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

    //         if(applicationMortgageLoan.ApplicationDebitOrders.Count == 0)
    //             AddMessage("There must be at least one Bank Account for an Application.", "There must be at least one Bank Account for an Application.", Messages);

    //         return 0;
    //     }
    // }

    [RuleDBTag("MortgageLoanMultipleApplication",
   "MortgageLoanMultipleApplication",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanMultipleApplication")]
    [RuleInfo]
    public class MortgageLoanMultipleApplication : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The MortgageLoanMultipleApplication rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The MortgageLoanMultipleApplication rule expects the following objects to be passed: IApplication.");

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

            if (applicationMortgageLoan == null)
                return 1;

            if (!(applicationMortgageLoan.Account == null))
            {
                // Try find another application that is currently open.
                foreach (IApplication application in applicationMortgageLoan.Account.Applications)
                {
                    if (applicationMortgageLoan.ApplicationStatus.Key == (int)OfferStatuses.Open
                        && application.ApplicationStatus.Key == (int)OfferStatuses.Open
                        && application.Key != applicationMortgageLoan.Key
                        && application is IApplicationMortgageLoan)
                    {
                        AddMessage("There's already another Mortgage Loan Application in progress.", "There's already another Mortgage Loan Application in progress.", Messages);
                        break;
                    }
                }
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationMortgageLoanExistingLoanAmount",
        "ApplicationMortgageLoanExistingLoanAmount",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ApplicationMortgageLoanExistingLoanAmount")]
    [RuleInfo]
    public class ApplicationMortgageLoanExistingLoanAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationMortgageLoanExistingLoanAmount rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationMortgageLoanSwitch))
                throw new ArgumentException("The ApplicationMortgageLoanExistingLoanAmount rule expects the following objects to be passed: IApplicationMortgageLoanSwitch.");

            IApplicationMortgageLoanSwitch applicationMortgageLoanSwitch = Parameters[0] as IApplicationMortgageLoanSwitch;

            if (!(applicationMortgageLoanSwitch.CurrentProduct is ISupportsVariableLoanApplicationInformation))
                throw new ArgumentException("The ApplicationMortgageLoanExistingLoanAmount rule expects the following objects to be passed: ISupportsVariableLoanApplicationInformation.");

            #endregion Check for allowed object type(s)

            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = applicationMortgageLoanSwitch.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            if (!supportsVariableLoanApplicationInformation.VariableLoanInformation.ExistingLoan.HasValue
                || supportsVariableLoanApplicationInformation.VariableLoanInformation.ExistingLoan.Value == 0.0)
                AddMessage("The Existing Loan Amount is mandatory for Switch Loan Applications.", "The Existing Loan Amount is mandatory for Switch Loan Applications.", Messages);

            return 0;
        }
    }

    [RuleDBTag("HOCCollectionMinimum",
   "There must  be HOC Details Captured against a Mortgage Loan Application before it can be submitted to Credit",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.HOCCollectionMinimum")]
    [RuleInfo]
    public class HOCCollectionMinimum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The HOCCollectionMinimum rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The HOCCollectionMinimum rule expects the following objects to be passed: IApplication.");

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

            if (applicationMortgageLoan == null || applicationMortgageLoan.Key == 0)
                return 1;

            IHOCRepository hocRepository = RepositoryFactory.GetRepository<IHOCRepository>();
            IAccountHOC hoc = hocRepository.RetrieveHOCByOfferKey(applicationMortgageLoan.Key);
            if (applicationMortgageLoan.Property == null
                || hoc == null)
            {
                string msg = "There must be HOC Details Captured against a Mortgage Loan Application before it can be submitted to Credit.";

                AddMessage(msg, msg, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("ValuationExists",
    "There must be at least 1 valid valuation against a Mortgage Loan Application before it can be submitted to Credit",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.ValuationExists")]
    [RuleInfo]
    public class ValuationExists : BusinessRuleBase
    {
        public ValuationExists(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The Valuation Exists rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The Valuation Exists rule expects the following objects to be passed: IApplication.");

            #endregion Check for allowed object type(s)

            IApplication app = Parameters[0] as IApplication;

            if (app == null || app.Key == 0)
                return 1;

            string sqlQuery = UIStatementRepository.GetStatement("COMMON", "ValuationActiveByOffer");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@appKey", app.Key));

            object o = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), prms);

            if (o == null)
            {
                string msg = "There must be at least 1 valid valuation against a Mortgage Loan Application before it can be submitted to Credit.";
                AddMessage(msg, msg, Messages);
                return 1;
            }
            return 0;
        }
    }

    [RuleDBTag("CancellationBankDetailsSwitch",
   "Cancellation Bank Details must be captured before submission to credit.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.CancellationBankDetailsSwitch")]
    [RuleInfo]
    public class CancellationBankDetailsSwitch : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The CancellationBankDetailsSwitch rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The CancellationBankDetailsSwitch rule expects the following objects to be passed: IApplication.");

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

            if (applicationMortgageLoan == null || applicationMortgageLoan.Key == 0 || applicationMortgageLoan.ApplicationType == null)
                return 1;

            //If the application type is not SWITCH then don't continue
            if (applicationMortgageLoan.ApplicationType.Key != (int)SAHL.Common.Globals.OfferTypes.SwitchLoan)
            {
                return 0;
            }

            foreach (IApplicationExpense applicationExpense in applicationMortgageLoan.ApplicationExpenses)
            {
                if (applicationExpense.ExpenseType.Key == (int)ExpenseTypes.Existingmortgageamount)
                {
                    return 0;
                }
            }

            // if we've got this far, there's an error
            string msg = "Cancellation Bank Details must be captured before submission to credit.";
            AddMessage(msg, msg, Messages);
            return 1;
        }
    }

    [RuleDBTag("SellersDetailsMandatoryNewPurchase",
    "Sellers details must be captured before submission to credit.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.SellersDetailsMandatoryNewPurchase")]
    [RuleInfo]
    public class SellersDetailsMandatoryNewPurchase : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The SellersDetailsMandatoryNewPurchase rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The SellersDetailsMandatoryNewPurchase rule expects the following objects to be passed: IApplication.");

            #endregion Check for allowed object type(s)

            // check the type of the application - if not a new purchase application then dont fire rule
            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

            if (applicationMortgageLoan == null || applicationMortgageLoan.Key == 0)
                return 1;

            if (applicationMortgageLoan is IApplicationMortgageLoanNewPurchase)
            {
                bool found = false;

                foreach (IApplicationRole applicationRole in applicationMortgageLoan.ApplicationRoles)
                {
                    if (applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Seller
                        && applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    string msg = "Sellers details must be captured before submission to credit.";
                    AddMessage(msg, msg, Messages);
                }
            }

            return 0;
        }
    }

    #region Valuation Rules

    [RuleDBTag("ApplicationMortgageLoanNewPurchaseNewLightstoneValuationRequired",
       "When carrying out a New Purchase application a LightStoneAutomatedValuation is required if there is no current lightstone valuation on record or if the current lightstone valuation is older than 12 months.",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ApplicationMortgageLoanNewPurchaseNewLightstoneValuationRequired")]
    [RuleInfo]
    public class ApplicationMortgageLoanNewPurchaseNewLightstoneValuationRequired : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationMortgageLoanNewPurchaseNewLightstoneValuationRequired rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationMortgageLoanNewPurchase))
                throw new ArgumentException("The ApplicationMortgageLoanNewPurchaseNewLightstoneValuationRequired rule expects the following objects to be passed: IApplicationMortgageLoanNewPurchase.");

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoanNewPurchase applicationMortgageLoanNewPurchase = Parameters[0] as IApplicationMortgageLoanNewPurchase;
            if (applicationMortgageLoanNewPurchase != null)
                if (applicationMortgageLoanNewPurchase.Key == 0 || applicationMortgageLoanNewPurchase.Property == null)
                    return 1;

            DateTime latestValuationDate = DateTime.MinValue;
            if (applicationMortgageLoanNewPurchase != null)
                foreach (IValuation valuation in applicationMortgageLoanNewPurchase.Property.Valuations)
                {
                    if (valuation is IValuationDiscriminatedLightstoneAVM
                        && valuation.ValuationDate > latestValuationDate)
                        latestValuationDate = valuation.ValuationDate;
                }

            if (latestValuationDate.AddMonths(12) < DateTime.Today)
            {
                string errorMessage = "When carrying out a New Purchase application a LightStoneAutomatedValuation is required if there is no current lightstone valuation on record or if the current lightstone valuation is older than 12 months.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationMortgageLoanSwitchNewLightstoneValuationRequired",
       "When carrying out a Switch Application a LightStoneAutomatedValuation is required if there is no current lightstone valuation on record or if the current lightstone valuation is older than 12 months.",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ApplicationMortgageLoanSwitchNewLightstoneValuationRequired")]
    [RuleInfo]
    public class ApplicationMortgageLoanSwitchNewLightstoneValuationRequired : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationMortgageLoanSwitchNewLightstoneValuationRequired rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationMortgageLoanSwitch))
                throw new ArgumentException("The ApplicationMortgageLoanSwitchNewLightstoneValuationRequired rule expects the following objects to be passed: IApplicationMortgageLoanSwitch.");

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoanSwitch applicationMortgageLoanSwitch = Parameters[0] as IApplicationMortgageLoanSwitch;
            if (applicationMortgageLoanSwitch.Key == 0 || applicationMortgageLoanSwitch.Property == null)
                return 1;

            DateTime latestValuationDate = DateTime.MinValue;
            foreach (IValuation valuation in applicationMortgageLoanSwitch.Property.Valuations)
            {
                if (valuation is IValuationDiscriminatedLightstoneAVM
                    && valuation.ValuationDate > latestValuationDate)
                    latestValuationDate = valuation.ValuationDate;
            }

            if (latestValuationDate.AddMonths(12) < DateTime.Today)
            {
                string errorMessage = "When carrying out a Switch application a LightStoneAutomatedValuation is required if there is no current lightstone valuation on record or if the current lightstone valuation is older than 12 months.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationMortgageLoanNewPurchaseNewAdCheckValuationRequired",
      "When carrying out a New Purchase application an AdCheckPhysicalValuation is required if there is no current AdCheck valuation on record or if the current AdCheck valuation is older than 12 months.",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ApplicationMortgageLoanNewPurchaseNewAdCheckValuationRequired")]
    [RuleInfo]
    public class ApplicationMortgageLoanNewPurchaseNewAdCheckValuationRequired : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationMortgageLoanNewPurchaseNewAdCheckValuationRequired rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationMortgageLoanNewPurchase))
                throw new ArgumentException("The ApplicationMortgageLoanNewPurchaseNewAdCheckValuationRequired rule expects the following objects to be passed: IApplicationMortgageLoanNewPurchase.");

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoanNewPurchase applicationMortgageLoanNewPurchase = Parameters[0] as IApplicationMortgageLoanNewPurchase;
            if (applicationMortgageLoanNewPurchase.Key == 0 || applicationMortgageLoanNewPurchase.Property == null)
                return 1;

            DateTime latestValuationDate = DateTime.MinValue;
            foreach (IValuation valuation in applicationMortgageLoanNewPurchase.Property.Valuations)
            {
                if (valuation is IValuationDiscriminatedAdCheckPhysical
                    && valuation.ValuationDate > latestValuationDate)
                    latestValuationDate = valuation.ValuationDate;
            }

            if (latestValuationDate.AddMonths(12) < DateTime.Today)
            {
                string errorMessage = "When carrying out a New Purchase application an AdCheckPhysicalValuation is required if there is no current AdCheck valuation on record or if the current AdCheck valuation is older than 12 months.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("ApplicationMortgageLoanSwitchNewAdCheckValuationRequired",
       "When carrying out a Switch application a AdCheckAutomatedValuation is required if there is no current AdCheck valuation on record or if the current AdCheck valuation is older than 12 months.",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.ApplicationMortgageLoanSwitchNewAdCheckValuationRequired")]
    [RuleInfo]
    public class ApplicationMortgageLoanSwitchNewAdCheckValuationRequired : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationMortgageLoanSwitchNewAdCheckValuationRequired rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationMortgageLoanSwitch))
                throw new ArgumentException("The ApplicationMortgageLoanSwitchNewAdCheckValuationRequired rule expects the following objects to be passed: IApplicationMortgageLoanSwitch.");

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoanSwitch applicationMortgageLoanSwitch = Parameters[0] as IApplicationMortgageLoanSwitch;
            if (applicationMortgageLoanSwitch.Key == 0 || applicationMortgageLoanSwitch.Property == null)
                return 1;

            DateTime latestValuationDate = DateTime.MinValue;
            foreach (IValuation valuation in applicationMortgageLoanSwitch.Property.Valuations)
            {
                if (valuation is IValuationDiscriminatedAdCheckPhysical
                    && valuation.ValuationDate > latestValuationDate)
                    latestValuationDate = valuation.ValuationDate;
            }

            if (latestValuationDate.AddMonths(12) < DateTime.Today)
            {
                string errorMessage = "When carrying out a Switch application a AdCheckAutomatedValuation is required if there is no current AdCheck valuation on record or if the current AdCheck valuation is older than 12 months.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    #endregion Valuation Rules

    #region QuickCash Rules

    [RuleDBTag("QuickCashCreditApproveAmount",
      "The Quick Cash approval amount must be less than or equal to the Maximum Quick Cash Amount calculated via the GetMaximumQuickCash method and greater than R 1000.",
      "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.Products.QuickCashCreditApproveAmount")]
    [RuleInfo]
    public class QuickCashCreditApproveAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The QuickCashCreditApproveAmount rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationInformationQuickCash))
                throw new ArgumentException("The QuickCashCreditApproveAmount rule expects the following objects to be passed: IApplicationInformationQuickCash.");

            #endregion Check for allowed object type(s)

            // The sane assumption is that only applications with a cash out portion will support quickcash
            IApplicationInformationQuickCash applicationInformationQuickCash = Parameters[0] as IApplicationInformationQuickCash;

            IReasonRepository reasonRepository = RepositoryFactory.GetRepository<IReasonRepository>();
            int appInfoKey = applicationInformationQuickCash.ApplicationInformation.Key;

            // this returns a list of the decline reasons for this offerinfokey.
            IReadOnlyEventList<IReason> reasonList = reasonRepository.GetReasonByGenericKeyAndReasonTypeKey(appInfoKey, (int)ReasonTypes.QuickCashDecline);

            // if the count of the reasons list > 0 then there are decline reasons - ensure the amount is zero - which amount? applicationInformationQuickCash.CreditApprovedAmount ?
            if (reasonList.Count > 0)
            {
                if (applicationInformationQuickCash.CreditApprovedAmount != 0)
                {
                    // throw error here as if there are reasons then amount must be zero.
                    string errorMessage = "If there are Quick Cash Decline reasons then the Quick Cash Amount must be zero.";
                    AddMessage(errorMessage, errorMessage, Messages);
                    return 1;
                }
            }
            else
            {
                ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                double minimumQuickCash = Convert.ToDouble(lookupRepository.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.QuickCash.MinimumQuickCash].ControlNumeric);

                if (applicationInformationQuickCash.CreditApprovedAmount < minimumQuickCash
                    || applicationInformationQuickCash.CreditApprovedAmount > applicationInformationQuickCash.GetMaximumQuickCash())
                {
                    string errorMessage = "The Quick Cash approval amount must be less than or equal to " + applicationInformationQuickCash.GetMaximumQuickCash().ToString(SAHL.Common.Constants.CurrencyFormat) + " and greater than " + minimumQuickCash.ToString(SAHL.Common.Constants.CurrencyFormat) + ".";
                    AddMessage(errorMessage, errorMessage, Messages);
                    return 1;
                }
            }
            return 0;
        }
    }

    [RuleDBTag("QuickCashUpFrontMaximum",
     "The Quick Cash Upfront amount exceeds maximum allowed",
     "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.QuickCashUpFrontMaximum"),]
    [RuleParameterTag(new string[] { "@MaxQuickCashUpfront,75000,7" })]
    [RuleInfo]
    public class QuickCashUpFrontMaximum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The QuickCashUpFrontMaximum rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationInformationQuickCash))
                throw new ArgumentException("The QuickCashUpFrontMaximum rule expects the following objects to be passed: IApplicationInformationQuickCash.");

            #endregion Check for allowed object type(s)

            IApplicationInformationQuickCash applicationInformationQuickCash = Parameters[0] as IApplicationInformationQuickCash;

            Double maxValue = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            if (applicationInformationQuickCash.CreditUpfrontApprovedAmount > maxValue)
            {
                string errorMessage = "The Cash Upfront amount must not exceed the maximum allowed of " + maxValue.ToString(Constants.CurrencyFormat) + ".";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("QuickUpFrontLessThanApprovedAmount",
 "The Quick Cash Upfront amount must be less than the approved amount",
 "SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.Products.QuickUpFrontLessThanApprovedAmount"),]
    [RuleInfo]
    public class QuickUpFrontLessThanApprovedAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The QuickUpFrontLessThanApprovedAmount rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationInformationQuickCash))
                throw new ArgumentException("The QuickCashUpFrontMaximum rule expects the following objects to be passed: IApplicationInformationQuickCash.");

            #endregion Check for allowed object type(s)

            IApplicationInformationQuickCash applicationInformationQuickCash = Parameters[0] as IApplicationInformationQuickCash;

            if (applicationInformationQuickCash != null)
                if (applicationInformationQuickCash.CreditUpfrontApprovedAmount > applicationInformationQuickCash.CreditApprovedAmount)
                {
                    const string errorMessage = "The Upfront Amount approved amount cannot be greater than the Total Approved amount.";
                    AddMessage(errorMessage, errorMessage, Messages);
                }
            return 0;
        }
    }

    [RuleDBTag("QuickCashUpFrontApprovalReduce",
    "The Quick Cash Upfront approval amount can not be reduced to less than what has been disbursed",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Products.QuickCashUpFrontApprovalReduce"),]
    [RuleInfo]
    public class QuickCashUpFrontApprovalReduce : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The QuickCashUpFrontMaximum rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationInformationQuickCash))
                throw new ArgumentException("The QuickCashUpFrontMaximum rule expects the following objects to be passed: IApplicationInformationQuickCash.");

            #endregion Check for allowed object type(s)

            IApplicationInformationQuickCash applicationInformationQuickCash = Parameters[0] as IApplicationInformationQuickCash;
            double amtDisbursed = 0;

            if (applicationInformationQuickCash.ApplicationInformationQuickCashDetails != null)
            {
                for (int i = 0; i < applicationInformationQuickCash.ApplicationInformationQuickCashDetails.Count; i++)
                {
                    if (Convert.ToBoolean(applicationInformationQuickCash.ApplicationInformationQuickCashDetails[i].Disbursed) && applicationInformationQuickCash.ApplicationInformationQuickCashDetails[i].QuickCashPaymentType.Key == (int)QuickCashPaymentTypes.UpfrontPayment)
                        amtDisbursed += Convert.ToDouble(applicationInformationQuickCash.ApplicationInformationQuickCashDetails[i].RequestedAmount);
                }
            }

            if (applicationInformationQuickCash.CreditUpfrontApprovedAmount < amtDisbursed)
            {
                string errorMessage = "The Quick Cash Upfront approval amount can not be reduced to less than what has been disbursed as an Upfront Payment";
                AddMessage(errorMessage, errorMessage, Messages);
            }
            return 0;
        }
    }

    [RuleDBTag("QuickCashTotalApprovalReduce",
   "The Quick Cash amount can not be reduced to less than what has been disbursed",
   "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Products.QuickCashTotalApprovalReduce"),]
    [RuleInfo]
    public class QuickCashTotalApprovalReduce : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The QuickCashUpFrontMaximum rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationInformationQuickCash))
                throw new ArgumentException("The QuickCashUpFrontMaximum rule expects the following objects to be passed: IApplicationInformationQuickCash.");

            #endregion Check for allowed object type(s)

            IApplicationInformationQuickCash applicationInformationQuickCash = Parameters[0] as IApplicationInformationQuickCash;
            double amtDisbursed = 0;

            for (int i = 0; i < applicationInformationQuickCash.ApplicationInformationQuickCashDetails.Count; i++)
            {
                if (Convert.ToBoolean(applicationInformationQuickCash.ApplicationInformationQuickCashDetails[i].Disbursed))
                    amtDisbursed += Convert.ToDouble(applicationInformationQuickCash.ApplicationInformationQuickCashDetails[i].RequestedAmount);
            }

            if (applicationInformationQuickCash.CreditApprovedAmount < amtDisbursed)
            {
                string errorMessage = "The Quick Cash approved amount can not be reduced to less than what has been disbursed as an Upfront Payment";
                AddMessage(errorMessage, errorMessage, Messages);
            }
            return 0;
        }
    }

    [RuleDBTag("QuickCashCashOutReduce",
  "The Cash Out Amount can not be reduced to less than what has been disbursed as Quick Cash",
  "SAHL.Rules.DLL",
 "SAHL.Common.BusinessModel.Rules.Products.QuickCashCashOutReduce"),]
    [RuleInfo]
    public class QuickCashCashOutReduce : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplication appML = Parameters[0] as IApplication;

            IApplicationMortgageLoanWithCashOut appMLWithCO = appML as IApplicationMortgageLoanWithCashOut;

            if (appMLWithCO == null)
                return 1;

            if (appMLWithCO != null && appMLWithCO.RequestedCashAmount.HasValue)
            {
                double cashOut = Convert.ToDouble(appMLWithCO.RequestedCashAmount);

                ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                double quickCashThresholdPerc = Convert.ToDouble(lookupRepository.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.QuickCash.QuickCashThresholdPercentage].ControlNumeric);

                double minimCashOut = cashOut * (quickCashThresholdPerc / 100);

                IApplicationInformationQuickCash appInfoQuickCash = null;

                for (int y = 0; y < appML.ApplicationInformations.Count; y++)
                {
                    if (appML.ApplicationInformations[y] is IApplicationInformationQuickCash && appML.ApplicationInformations[y].ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer)
                        appInfoQuickCash = appML.ApplicationInformations[y] as IApplicationInformationQuickCash;
                }

                double totalQuickCashDisbursed = 0;
                int numUpFrontPayments = 0;
                int numRegularPayments = 0;
                double disbursedUpfrontAmount = 0;
                double disbursedRegularAmount = 0;

                if (appInfoQuickCash != null)
                {
                    for (int i = 0; i < appInfoQuickCash.ApplicationInformationQuickCashDetails.Count; i++)
                    {
                        if (Convert.ToBoolean(appInfoQuickCash.ApplicationInformationQuickCashDetails[i].Disbursed))
                            totalQuickCashDisbursed += Convert.ToDouble(appInfoQuickCash.ApplicationInformationQuickCashDetails[i].RequestedAmount);
                        if (Convert.ToBoolean(appInfoQuickCash.ApplicationInformationQuickCashDetails[i].Disbursed) && appInfoQuickCash.ApplicationInformationQuickCashDetails[i].QuickCashPaymentType.Key == (int)QuickCashPaymentTypes.UpfrontPayment)
                        {
                            numUpFrontPayments += 1;
                            disbursedUpfrontAmount += Convert.ToDouble(appInfoQuickCash.ApplicationInformationQuickCashDetails[i].RequestedAmount);
                        }
                        if (Convert.ToBoolean(appInfoQuickCash.ApplicationInformationQuickCashDetails[i].Disbursed) && appInfoQuickCash.ApplicationInformationQuickCashDetails[i].QuickCashPaymentType.Key == (int)QuickCashPaymentTypes.RegularPayment)
                        {
                            numRegularPayments += 1;
                            disbursedRegularAmount += Convert.ToDouble(appInfoQuickCash.ApplicationInformationQuickCashDetails[i].RequestedAmount);
                        }
                    }
                }

                // Can not reduce the Cash out to less than what has already been disbursed as Quick Cash
                if (minimCashOut < totalQuickCashDisbursed)
                {
                    string errorMessage = quickCashThresholdPerc.ToString() + " % of the Cash Out Portion can not be less than the total Quick Cash Disbursed";
                    AddMessage(errorMessage, errorMessage, Messages);
                }

                // If Cash Out reduced to minimum and upfront payment is the only one made
                if (cashOut == minimCashOut && numUpFrontPayments > 0 && numRegularPayments == 0)
                {   // The Cash Upfront amt and the Total QC and the amount disbursed must be the same
                    if (appInfoQuickCash.CreditUpfrontApprovedAmount != totalQuickCashDisbursed || appInfoQuickCash.CreditApprovedAmount != totalQuickCashDisbursed)
                    {
                        string errorMessage = "The CashUpfront amount and the Total QuickCash Approved amount must equal the Total Amount disbursed of R" + totalQuickCashDisbursed.ToString() + ".";
                        AddMessage(errorMessage, errorMessage, Messages);
                    }
                }

                // If Cash Out reduced to minimum and upfront payment and Regular payments both made
                if (cashOut == minimCashOut && numUpFrontPayments > 0 && numRegularPayments > 0)
                {   // The Cash Upfront amt must equal dibursed Cash Upfront and Credit Approved must equal total disbursement
                    if (appInfoQuickCash.CreditUpfrontApprovedAmount != disbursedUpfrontAmount || appInfoQuickCash.CreditApprovedAmount != totalQuickCashDisbursed)
                    {
                        string errorMessage = "The CashUpfront and Total Credit Approved amounts must equal the respective disbursed amounts of R " + disbursedUpfrontAmount.ToString() + " and R " + totalQuickCashDisbursed.ToString() + ".";
                        AddMessage(errorMessage, errorMessage, Messages);
                    }
                }

                // If Cash Out reduced to minimum and only Regular payments made
                if (cashOut == minimCashOut && numUpFrontPayments == 0 && numRegularPayments > 0)
                {   // The Cash Upfront amt must equal dibursed Cash Upfront and Credit Approved must equal total disbursement
                    if (appInfoQuickCash.CreditUpfrontApprovedAmount > 0 || appInfoQuickCash.CreditApprovedAmount != totalQuickCashDisbursed)
                    {
                        string errorMessage = "The CashUpfront amount can not be greater than zero and the total Credit Approved amount must equal the total disbursed amount of R " + totalQuickCashDisbursed.ToString() + ".";
                        AddMessage(errorMessage, errorMessage, Messages);
                    }
                }
            }
            return 0;
        }
    }

    #endregion QuickCash Rules

    #endregion Application MortgageLoan Spec

    #region Account MortgageLoan Spec

    [RuleDBTag("MortgageLoanAccountBondMinimum",
        "MortgageLoanAccountBondMinimum",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanAccountBondMinimum")]
    [RuleInfo]
    public class MortgageLoanAccountBondMinimum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The MortgageLoanAccountBondMinimum rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IMortgageLoan))
                throw new ArgumentException("The MortgageLoanAccountBondMinimum rule expects the following objects to be passed: IMortgageLoan.");

            #endregion Check for allowed object type(s)

            IMortgageLoan mortgageLoan = Parameters[0] as IMortgageLoan;
            if (mortgageLoan.Bonds.Count == 0)
                AddMessage("A mortgage loan must always have at least one Bond.", "A mortgage loan must always have at least one Bond.", Messages);

            return 0;
        }
    }

    [RuleDBTag("AccountMailingAddress",
   "AccountMailingAddress",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.AccountMailingAddress")]
    [RuleInfo]
    public class AccountMailingAddress : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The AccountMailingAddress rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The AccountMailingAddress rule expects the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;

            if (mortgageLoanAccount == null)
                return 1;

            bool doesMailingAddressBelongToLE;

            if (mortgageLoanAccount.MailingAddresses.Count == 0)
                AddMessage("Each Account must have one valid postal or residential address.", "Each Account must have one valid postal or residential address.", Messages);
            else
            {
                // Does the address belong to one of the roleplayers?
                // applicationMortgageLoan.AccountMailingAddresses

                foreach (IMailingAddress mailingAddress in mortgageLoanAccount.MailingAddresses)
                {
                    doesMailingAddressBelongToLE = false;

                    // AccountMailingAddress.Address.Key
                    foreach (IRole role in mortgageLoanAccount.Roles)
                    {
                        if ((role.RoleType.Key == (int)OfferRoleTypes.LeadSuretor
                            || role.RoleType.Key == (int)OfferRoleTypes.LeadMainApplicant)
                            && role.GeneralStatus.Key == (int)GeneralStatuses.Active)
                        {
                            foreach (ILegalEntityAddress legalEntityAddress in role.LegalEntity.LegalEntityAddresses)
                            {
                                if (legalEntityAddress.Address.Key == mailingAddress.Address.Key)
                                {
                                    doesMailingAddressBelongToLE = true;
                                    break;
                                }
                            }
                        }

                        if (!doesMailingAddressBelongToLE)
                            break;
                    }

                    if (!doesMailingAddressBelongToLE)
                    {
                        AddMessage("The Account Mailing Address must belong to one of the legal entities playing one of Lead - Suretor or Lead - Main Applicant.", "The Account Mailing Address must belong to one of the legal entities playing one of Lead - Suretor or Lead - Main Applicant", Messages);
                        break;
                    }
                }
            }

            return 0;
        }
    }

    [RuleDBTag("MortgageLoanAccountMailingActive",
        "MortgageLoanAccountMailingActive",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanAccountMailingActive")]
    [RuleInfo]
    public class MortgageLoanAccountMailingActive : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The MortgageLoanAccountMailingActive rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication || Parameters[0] is IAccount))
                throw new ArgumentException("The MortgageLoanAccountMailingActive rule expects the following objects to be passed: IApplication, IAccount.");

            #endregion Check for allowed object type(s)

            bool isMailingAddressValid = true;
            if (Parameters[0] is IApplication)
            {
                IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;
                if (applicationMortgageLoan == null || applicationMortgageLoan.Key == 0)
                    return 1;

                if (applicationMortgageLoan.ApplicationMailingAddresses.Count == 0)
                    isMailingAddressValid = false;
            }

            if (Parameters[0] is IAccount)
            {
                IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;
                if (mortgageLoanAccount == null)
                    return 1;

                if (mortgageLoanAccount.MailingAddresses.Count == 0)
                    isMailingAddressValid = false;
            }

            if (!isMailingAddressValid)
                AddMessage("An Account must have a valid postal or residential address as the Account Mailing Address.", "An Account must have a valid postal or residential address as the Account Mailing Address.", Messages);

            return 0;
        }
    }

    [RuleDBTag("MortgageLoanAccountFixedDebitOrderValueNonSubsidy",
        "MortgageLoanAccountFixedDebitOrderValueNonSubsidy",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanAccountFixedDebitOrderValueNonSubsidy")]
    [RuleInfo]
    public class MortgageLoanAccountFixedDebitOrderValueNonSubsidy : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The MortgageLoanAccountFixedDebitOrderValueNonSubsidy rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The MortgageLoanAccountFixedDebitOrderValueNonSubsidy rule expects the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            IAccount account = Parameters[0] as IAccount;

            if (!(account.FixedPayment == 0.0 ||
                account.FixedPayment >= account.InstallmentSummary.TotalAmountDue))
                AddMessage("The Fixed Debit Order must be either zero (0) or greater or equal to the Total Amount Due.", "The Fixed Debit Order must be either zero (0) or greater or equal to the Total Amount Due.", Messages);

            return 0;
        }
    }

    [RuleDBTag("MortgageLoanAccountEmploymentCurrent",
        "MortgageLoanAccountEmploymentCurrent",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanAccountEmploymentCurrent")]
    [RuleInfo]
    public class MortgageLoanAccountEmploymentCurrent : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The MortgageLoanAccountEmploymentCurrent rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The MortgageLoanAccountEmploymentCurrent rule expects the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            bool isConfirmedEmploymentFound = false;

            IAccount account = Parameters[0] as IAccount;
            foreach (IRole role in account.Roles)
            {
                if (role.GeneralStatus.Key == (int)GeneralStatuses.Active)
                {
                    foreach (IEmployment employment in role.LegalEntity.Employment)
                    {
                        if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current
                            && employment.IsConfirmed)
                        {
                            isConfirmedEmploymentFound = true;
                            break;
                        }
                    }
                }
                if (isConfirmedEmploymentFound)
                    break;
            }

            if (!isConfirmedEmploymentFound)
                AddMessage("An Account must have at least one valid and confirmed Employment associated with one of the Legal Entities.", "An Account must have at least one valid and confirmed Employment associated with one of the Legal Entities.", Messages);

            return 0;
        }
    }

    [RuleDBTag("MortgageLoanPropertyLink",
        "MortgageLoanPropertyLink",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanPropertyLink")]
    [RuleInfo]
    public class MortgageLoanPropertyLink : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The MortgageLoanPropertyLink rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount || Parameters[0] is IApplication))
                throw new ArgumentException("The MortgageLoanPropertyLink rule expects the following objects to be passed: IAccount, IApplication.");

            #endregion Check for allowed object type(s)

            bool isPropertyFound = true;

            if (Parameters[0] is IMortgageLoanAccount)
            {
                IMortgageLoanAccount account = Parameters[0] as IMortgageLoanAccount;
                if (account == null)
                    return 1;

                if (account.SecuredMortgageLoan.Property == null)
                    isPropertyFound = false;
            }

            if (Parameters[0] is IApplicationMortgageLoan)
            {
                IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;
                if (applicationMortgageLoan == null || applicationMortgageLoan.Key == 0)
                    return 1;

                if (applicationMortgageLoan.Property == null)
                    isPropertyFound = false;
            }

            if (!isPropertyFound)
                AddMessage("At least one property to must be associated with the Mortgage Loan.", "At least one property to must be associated with the Mortgage Loan.", Messages);

            return 0;
        }
    }

    [RuleDBTag("MortgageLoanAccountMonthlyServiceFees",
        "MortgageLoanAccountMonthlyServiceFees",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanAccountMonthlyServiceFees")]
    [RuleInfo]
    public class MortgageLoanAccountMonthlyServiceFees : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The MortgageLoanAccountMonthlyServiceFees rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The MortgageLoanAccountMonthlyServiceFees rule expects the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            bool isMonthlyServiceFeeFound = false;

            IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;
            if (mortgageLoanAccount == null)
                return 1;

            foreach (IFinancialService financialService in mortgageLoanAccount.FinancialServices)
            {
                foreach (var manualDebitOrder in financialService.ManualDebitOrders)
                {
                    if (manualDebitOrder.TransactionType.Key == (int)TransactionTypes.MonthlyServiceFee)
                    {
                        isMonthlyServiceFeeFound = true;
                        break;
                    }
                }

                if (isMonthlyServiceFeeFound)
                    break;
            }

            if (!isMonthlyServiceFeeFound)
                AddMessage("Every account granted after the 2nd August 2007 must have a monthly service fee loaded.", "At least one property to must be associated with the Mortgage Loan.", Messages);

            return 0;
        }
    }

    [RuleDBTag("MortgageLoanLegalEntityBankAccount",
        "MortgageLoanLegalEntityBankAccount",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanLegalEntityBankAccount")]
    [RuleInfo]
    public class MortgageLoanLegalEntityBankAccount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The MortgageLoanLegalEntityBankAccount rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount || Parameters[0] is IApplication))
                throw new ArgumentException("The MortgageLoanLegalEntityBankAccount rule expects the following objects to be passed: IAccount or IApplication.");

            #endregion Check for allowed object type(s)

            bool isLinked = true;

            if (Parameters[0] is IMortgageLoanAccount)
            {
                IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;
                if (mortgageLoanAccount == null)
                    return 1;

                foreach (IFinancialService financialService in mortgageLoanAccount.FinancialServices)
                {
                    foreach (IFinancialServiceBankAccount financialServiceBankAccount in financialService.FinancialServiceBankAccounts)
                    {
                        isLinked = false;

                        // All Bank financialServiceBankAccount.BankAccount(s) need to be linked to the legal entity.
                        foreach (IRole role in mortgageLoanAccount.Roles)
                        {
                            foreach (ILegalEntityBankAccount legalEntityBankAccounts in role.LegalEntity.LegalEntityBankAccounts)
                            {
                                if (legalEntityBankAccounts.BankAccount.Key == financialServiceBankAccount.BankAccount.Key)
                                {
                                    isLinked = true;
                                    break;
                                }
                            }

                            if (!isLinked)
                                break;
                        }
                        if (!isLinked)
                            break;
                    }
                    if (!isLinked)
                        break;
                }
            }

            if (Parameters[0] is IApplicationMortgageLoan)
            {
                IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;
                if (applicationMortgageLoan == null || applicationMortgageLoan.Key == 0)
                    return 1;

                foreach (IApplicationDebitOrder applicationDebitOrder in applicationMortgageLoan.ApplicationDebitOrders)
                {
                    if (applicationDebitOrder.BankAccount != null)
                    {
                        isLinked = false;
                        foreach (IApplicationRole role in applicationMortgageLoan.ApplicationRoles)
                        {
                            foreach (ILegalEntityBankAccount legalEntityBankAccount in role.LegalEntity.LegalEntityBankAccounts)
                            {
                                if (legalEntityBankAccount.BankAccount.Key == applicationDebitOrder.BankAccount.Key)
                                {
                                    isLinked = true;
                                    break;
                                }
                            }

                            if (!isLinked)
                                break;
                        }
                    }
                }
            }

            if (!isLinked)
                AddMessage("Only Bank Account connected to a Legal Entity may be connected to the Mortgate Loan.", "Only Bank Account connected to a Legal Entity may be connected to the Mortgate Loan.", Messages);

            return 0;
        }
    }

    [RuleDBTag("MortgageLoanBankAccountPaymentTypeDebitOrder",
        "MortgageLoanBankAccountPaymentTypeDebitOrder",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanBankAccountPaymentTypeDebitOrder")]
    [RuleInfo]
    public class MortgageLoanBankAccountPaymentTypeDebitOrder : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The MortgageLoanBankAccountPaymentTypeDebitOrder rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The MortgageLoanBankAccountPaymentTypeDebitOrder rule expects the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;
            if (mortgageLoanAccount == null)
                return 1;

            bool isBankAccountFound = true;

            foreach (IFinancialService financialService in mortgageLoanAccount.FinancialServices)
            {
                foreach (IFinancialServiceBankAccount financialServiceBankAccount in financialService.FinancialServiceBankAccounts)
                {
                    if (financialServiceBankAccount.FinancialServicePaymentType.Key == (int)FinancialServicePaymentTypes.DebitOrderPayment
                        && financialServiceBankAccount.BankAccount == null)
                    {
                        isBankAccountFound = false;
                        break;
                    }
                }
                if (!isBankAccountFound)
                    break;
            }

            if (!isBankAccountFound)
                AddMessage("A bank account is required for Debit Order Payment Types.", "A bank account is required for Debit Order Payment Types.", Messages);

            return 0;
        }
    }

    [RuleDBTag("MortgageLoanBankAccountPaymentTypeSubsidyAdd",
        "MortgageLoanBankAccountPaymentTypeSubsidyAdd",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanBankAccountPaymentTypeSubsidyAdd")]
    [RuleInfo]
    public class MortgageLoanBankAccountPaymentTypeSubsidyAdd : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The MortgageLoanBankAccountPaymentTypeSubsidyAdd rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The MortgageLoanBankAccountPaymentTypeSubsidyAdd rule expects the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;
            if (mortgageLoanAccount == null)
                return 1;

            bool isEmploymentRecFound = true;

            foreach (IFinancialService financialService in mortgageLoanAccount.FinancialServices)
            {
                foreach (IFinancialServiceBankAccount financialServiceBankAccount in financialService.FinancialServiceBankAccounts)
                {
                    if (financialServiceBankAccount.FinancialServicePaymentType.Key == (int)FinancialServicePaymentTypes.SubsidyPayment)
                    {
                        isEmploymentRecFound = false;

                        // If FinancialServicePaymentTypes.SubsidyPayment, find a bank account
                        foreach (IRole role in mortgageLoanAccount.Roles)
                        {
                            if (role.GeneralStatus.Key == (int)GeneralStatuses.Active)
                            {
                                foreach (IEmployment employment in role.LegalEntity.Employment)
                                {
                                    if (employment.EmploymentType.Key == (int)EmploymentTypes.SalariedwithDeduction
                                        && employment.IsConfirmed
                                        && employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current)
                                        isEmploymentRecFound = true;
                                }
                            }
                        }

                        if (!isEmploymentRecFound)
                            break;
                    }
                    if (!isEmploymentRecFound)
                        break;
                }
                if (!isEmploymentRecFound)
                    break;
            }

            if (!isEmploymentRecFound)
                AddMessage("A Subsidised Payment Type may only be set for an account with at least one active subsidy employment record.", "A Subsidised Payment Type may only be set for an account with at least one active subsidy employment record.", Messages);

            return 0;
        }
    }

    [RuleDBTag("MortgageLoanDebitOrderAmount",
        "MortgageLoanDebitOrderAmount",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanDebitOrderAmount")]
    [RuleInfo]
    public class MortgageLoanDebitOrderAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The MortgageLoanDebitOrderAmount rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The MortgageLoanDebitOrderAmount rule expects the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;
            if (mortgageLoanAccount == null)
                return 1;

            if (mortgageLoanAccount.FixedPayment == 0.0
                && mortgageLoanAccount.InstallmentSummary.TotalAmountDue > 0.0)
            {
                string errorMessage = String.Format("The Debit Order Amount is currently 0. This value must at least be equal to the sum of the sum of all the amounts due ({0}).", mortgageLoanAccount.InstallmentSummary.TotalAmountDue);
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("MortgageLoanAccountClosedFurtherTransactions",
       "MortgageLoanAccountClosedFurtherTransactions",
        "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanAccountClosedFurtherTransactions")]
    [RuleInfo]
    public class MortgageLoanAccountClosedFurtherTransactions : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The MortgageLoanAccountClosedFurtherTransactions rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IFinancialTransaction))
                throw new ArgumentException("The MortgageLoanAccountClosedFurtherTransactions rule expects the following objects to be passed: ILoantransaction.");

            #endregion Check for allowed object type(s)

            IFinancialTransaction loantransaction = Parameters[0] as IFinancialTransaction;

            // IFinancialServiceRepository financialServiceRepository = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            IFinancialService financialService = loantransaction.FinancialService; // financialServiceRepository.GetFinancialServiceByKey(loantransaction.LoanNumber);

            if (financialService == null)
                AddMessage("The financial service does not exist.", "The financial service does not exist.", Messages);

            if (financialService != null &&
                (int)financialService.AccountStatus.Key == (int)AccountStatuses.Closed)
                AddMessage("No further transactions can be posted against closed accounts.", "No further transactions can be posted against closed accounts.", Messages);

            return 0;
        }
    }

    [RuleDBTag("MortgageLoanAccountBondProperty",
        "MortgageLoanAccountBondProperty",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanAccountBondProperty")]
    [RuleInfo]
    public class MortgageLoanAccountBondProperty : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The MortgageLoanAccountBondProperty rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IBond))
                throw new ArgumentException("The MortgageLoanAccountBondProperty rule expects the following objects to be passed: IBond.");

            #endregion Check for allowed object type(s)

            IBond bond = Parameters[0] as IBond;

            int openVariableFSCounter = 0;

            foreach (IMortgageLoan mortgageLoan in bond.MortgageLoans)
            {
                if (mortgageLoan.AccountStatus.Key == (int)AccountStatuses.Open
                    && mortgageLoan.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan)
                    openVariableFSCounter++;
            }

            if (openVariableFSCounter > 1)
                AddMessage("A Bond may only be linked to one Mortgage Loan record.", "A Bond may only be linked to one Mortgage Loan record.", Messages);

            return 0;
        }
    }

    [RuleDBTag("MortgageLoanActiveDebitOrdersMinimum",
        "MortgageLoanActiveDebitOrdersMinimum",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanActiveDebitOrdersMinimum")]
    [RuleInfo]
    public class MortgageLoanActiveDebitOrdersMinimum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The MortgageLoanActiveDebitOrdersMinimum rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IMortgageLoan))
                throw new ArgumentException("The MortgageLoanActiveDebitOrdersMinimum rule expects the following objects to be passed: IMortgageLoan.");

            #endregion Check for allowed object type(s)

            IMortgageLoan mortgageLoan = Parameters[0] as IMortgageLoan;

            // Ensure that only one DO exists per month
            int debitOrderCount = 0;
            foreach (IFinancialServiceBankAccount financialServiceBankAccount in mortgageLoan.FinancialServiceBankAccounts)
            {
                if (financialServiceBankAccount.FinancialServicePaymentType.Key == (int)FinancialServicePaymentTypes.DebitOrderPayment
                    && financialServiceBankAccount.GeneralStatus.Key == (int)GeneralStatuses.Active)
                    debitOrderCount++;
            }

            if (debitOrderCount > 1)
                AddMessage("Ony one Debit Order may exist per month.", "Ony one Debit Order may exist per month.", Messages);

            return 0;
        }
    }

    [RuleDBTag("MortgageLoanAccountFixedDebitOrderValueSubsidy",
        "MortgageLoanAccountFixedDebitOrderValueSubsidy",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanAccountFixedDebitOrderValueSubsidy")]
    [RuleInfo]
    public class MortgageLoanAccountFixedDebitOrderValueSubsidy : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The MortgageLoanAccountFixedDebitOrderValueSubsidy rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The MortgageLoanAccountFixedDebitOrderValueSubsidy rule expects the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            IAccount account = Parameters[0] as IAccount;

            // Is there an active Employment Record
            double SubsidyAmount = 0.0;
            foreach (IRole role in account.Roles)
            {
                if (role.GeneralStatus.Key == (int)GeneralStatuses.Active)
                {
                    foreach (IEmployment employment in role.LegalEntity.Employment)
                    {
                        if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current
                            && employment.EmploymentType.Key == (int)EmploymentTypes.SalariedwithDeduction
                            && employment.IsConfirmed)
                        {
                            //if (employment.RequiresExtended)
                            //  SubsidyAmount += (employment.ExtendedEmployment.ConfirmedBasicIncome ?? 0D);
                            SubsidyAmount += employment.ConfirmedIncome;
                        }
                    }
                }
            }

            if (!(account.FixedPayment == 0.0 ||
                account.FixedPayment <= (account.InstallmentSummary.TotalAmountDue - SubsidyAmount)))
                AddMessage("The Fixed Debit Order must be either zero (0) or greater or equal to the Total Amount Due.", "The Fixed Debit Order must be either zero (0) or greater or equal to the Total Amount Due.", Messages);

            return 0;
        }
    }

    [RuleDBTag("MortgageLoanAccountFixedDebitOrderBank",
        "MortgageLoanAccountFixedDebitOrderBank",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.MortgageLoanAccountFixedDebitOrderBank")]
    [RuleInfo]
    public class MortgageLoanAccountFixedDebitOrderBank : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The MortgageLoanAccountFixedDebitOrderBank rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The MortgageLoanAccountFixedDebitOrderBank rule expects the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;
            if (mortgageLoanAccount == null)
                return 1;

            bool isBankLinked = false;

            // Is this a fixed debit order ...
            if (mortgageLoanAccount.FixedPayment > 0.0)
            {
                foreach (IFinancialService financialService in mortgageLoanAccount.FinancialServices)
                {
                    foreach (IFinancialServiceBankAccount financialServiceBankAccount in financialService.FinancialServiceBankAccounts)
                    {
                        if (financialServiceBankAccount.GeneralStatus.Key == (int)GeneralStatuses.Active
                            && financialServiceBankAccount.FinancialServicePaymentType.Key == (int)FinancialServicePaymentTypes.DebitOrderPayment)
                        {
                            isBankLinked = false;

                            // Is this Bank Account linked to a legal entity
                            if (financialServiceBankAccount.BankAccount != null)
                            {
                                foreach (IRole role in mortgageLoanAccount.Roles)
                                {
                                    if (role.GeneralStatus.Key == (int)GeneralStatuses.Active)
                                    {
                                        foreach (ILegalEntityBankAccount legalEntityBankAccount in role.LegalEntity.LegalEntityBankAccounts)
                                        {
                                            if (legalEntityBankAccount.BankAccount.Key == financialServiceBankAccount.BankAccount.Key)
                                            {
                                                isBankLinked = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (isBankLinked)
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            if (!isBankLinked)
            {
                string errorMessage = "There must be an active bank account against the debit order. This Bank Account must be linked to an active Legal Entity";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    #endregion Account MortgageLoan Spec

    [RuleDBTag("ApplicationConditionMandatory",
    "There should be at least one condition on the Application.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.ApplicationConditionMandatory")]
    [RuleInfo]
    public class ApplicationConditionMandatory : BusinessRuleBase
    {
        public ApplicationConditionMandatory(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationConditionMandatory rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The ApplicationConditionMandatory rule expects the following objects to be passed: IApplication.");

            #endregion Check for allowed object type(s)

            // check the type of the application - if not a new purchase application then dont fire rule
            IApplicationMortgageLoan appML = Parameters[0] as IApplicationMortgageLoan;

            if (appML == null || appML.Key == 0)
                return 1;

            // Nazir J => 2008-07-21
            // There is a rule which requires at least one condition to be added to an application.
            // This rule should not apply to readvances.
            if (appML.ApplicationType.Key != (int)SAHL.Common.Globals.OfferTypes.ReAdvance)
            {
                string sqlQuery = UIStatementRepository.GetStatement("COMMON", "ApplicationConditionMandatory");
                ParameterCollection prms = new ParameterCollection();
                prms.Add(new SqlParameter("@appKey", appML.Key));

                object obj = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), prms);

                if (obj == null)//no conditions found
                {
                    string msg = "There are no conditions captured against this Application";
                    AddMessage(msg, msg, Messages);
                    return 0;
                }
            }

            return 0;
        }
    }

    //Added By Nazir S J
    /// <summary>
    /// Rule for the maximum amount for a detail.
    /// </summary>
    [RuleDBTag("ApplicationProductMortgageLoanTerm",
       "Ensures that the loan term is 360 months unless it is VariFix Loan which is 240 months",
       "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.Products.ApplicationProductMortgageLoanTerm")]
    [RuleInfo]
    [RuleParameterTag(new string[] { "@MortgageLoanTerm,360,9", "@VariFixLoanTerm,240,9" })]
    public class ApplicationProductMortgageLoanTerm : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            // this rule is not written right - if the parameter is not a mortgage loan, the parameter
            // received is an int - leaving this for now but it's not right
            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;
            int term = 0;

            SAHL.Common.Globals.Products product;

            // VariFix Loans have a max term of 240 months
            // Mortgage Loans have a max term of 360 months

            if (applicationMortgageLoan == null)
            {
                term = Convert.ToInt32(Parameters[0]);
                product = (SAHL.Common.Globals.Products)Parameters[1];
            }
            else
            {
                IApplicationProductMortgageLoan appProductMortgageLoan = (IApplicationProductMortgageLoan)applicationMortgageLoan.CurrentProduct;
                if (!appProductMortgageLoan.Term.HasValue)
                {
                    return 1;
                }
                term = appProductMortgageLoan.Term.Value;
                product = appProductMortgageLoan.ProductType;
            }

            int MortgageLoanTerm = Convert.ToInt16(RuleItem.RuleParameters[0].Value);
            int VariFixLoanTerm = Convert.ToInt16(RuleItem.RuleParameters[1].Value);

            if (product == SAHL.Common.Globals.Products.VariFixLoan)
            {
                if (term > VariFixLoanTerm)
                {
                    string msg = string.Format("A {0} Term should not be greater than {1} months", product, VariFixLoanTerm);
                    AddMessage(msg, msg, Messages);
                    return 1;
                }
            }
            else
            {
                if (term > MortgageLoanTerm)
                {
                    string msg = String.Format("A {0} Term should not be greater than {1} months", product, MortgageLoanTerm);
                    AddMessage(msg, msg, Messages);
                    return 1;
                }
            }

            return 0;
        }
    }

    //[RuleDBTag("MortgageLoanSPVChangeCheck",
    //"MortgageLoanSPVChangeCheck",
    //"SAHL.Rules.DLL",
    //"SAHL.Common.BusinessModel.Rules.Products.MortgageLoanSPVChangeCheck")]
    //[RuleInfo]
    //public class MortgageLoanSPVChangeCheck : BusinessRuleBase
    //{
    //    public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
    //    {
    //        if (Parameters.Length == 0 || Parameters.Length < 2)
    //            throw new ArgumentException("The MortgageLoanSPVChangeCheck rule expects a Domain object to be passed.");

    //        IMortgageLoan mortgageLoan = Parameters[0] as IMortgageLoan;
    //        int instanceID = Convert.ToInt32(Parameters[1]);

    //        if (mortgageLoan == null)
    //            throw new ArgumentException("The MortgageLoanSPVChangeCheck rule expects the following objects to be passed: IMortgageLoan.");

    //        IMortgageLoanRepository mlRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
    //        int newTerm = -1;
    //        int newSPV = -1;

    //        mlRepo.LookUpPendingTermChangeDetailFromX2(out newSPV, out newTerm, (long)instanceID);

    //        int SPVTerm = newTerm + (mortgageLoan.InitialInstallments - mortgageLoan.RemainingInstallments);

    //        if (!mlRepo.IsSPVTermWithinMax(SPVTerm,  mortgageLoan.Account.SPV.Key))
    //        {
    //            int newSPVKey = mlRepo.GetNewSPVKeyTermChange(mortgageLoan.Account.SPV.Key);
    //            if (newSPVKey != mortgageLoan.Account.SPV.Key)
    //            {
    //                string newSPVDescription = mlRepo.GetNewSPVDescription(newSPVKey);
    //                string message = string.Format(@"Approving this request will result in the SPV being changed from {0} to {1}. Do you want to continue?", mortgageLoan.Account.SPV.Description, newSPVDescription);
    //                AddMessage(message, message, Messages);
    //                return 0;
    //            }
    //        }
    //        return 1;
    //    }
    //}

    /// <summary>
    /// Account is not in Debt Counselling
    /// </summary>
    [RuleDBTag("AccountNotInDebtCounselling",
    "The Account is not in Debt Counselling",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.AccountNotInDebtCounselling")]
    [RuleInfo]
    public class AccountNotInDebtCounselling : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The AccountNotInDebtCounselling rule expects a Domain object to be passed.");

            IAccount account = Parameters[0] as IAccount;

            if (account == null)
                throw new ArgumentException("The AccountNotInDebtCounselling rule expects the following objects to be passed: IAccount.");

            if (!account.UnderDebtCounselling)
            {
                string message = "The Account is not in Debt Counselling.";
                AddMessage(message, message, Messages);
                return 0;
            }
            return 1;
        }
    }
}