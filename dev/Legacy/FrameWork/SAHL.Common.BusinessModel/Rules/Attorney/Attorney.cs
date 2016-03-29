using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
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

namespace SAHL.Common.BusinessModel.Rules.Attorney.Attorney
{
    [RuleDBTag("AttorneyValidateTransferAttorney",
  "If an application of type New Purchase has transferring attorney captured as a SAHL attorney. Ensure that OfferMortgageLoan.TransferringAttorney is not null on submission to credit",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Attorney.Attorney.AttorneyValidateTransferAttorney")]
    [RuleInfo]
    public class AttorneyValidateTransferAttorney : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #   region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The AttorneyValidateTransferAttorney rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationMortgageLoan))
                throw new ArgumentException("The AttorneyValidateTransferAttorney rule expects the following objects to be passed: IApplicationMortgageLoan.");

            //if (RuleItem.RuleParameters.Count < 1)
            //    throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

            #endregion

            # region Rule Check

            IApplicationMortgageLoan application = Parameters[0] as IApplicationMortgageLoan;

            if (String.IsNullOrEmpty(application.TransferringAttorney))
                AddMessage(String.Format("No Transferring Attorney has been assigned to this application."), "", Messages);
            else
                return 0;

            #endregion

            return 0;
        }
    }

    [RuleDBTag("AttorneyMandatoryFields",
    "This rule determines whether or not all the mandatory fields are populated for an Attorney.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Attorney.Attorney.AttorneyMandatoryFields")]
    [RuleInfo]
    public class AttorneyMandatoryFields : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #   region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The AttorneyMandatoryFields rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAttorney))
                throw new ArgumentException("The AttorneyMandatoryFields rule expects the following objects to be passed: IAttorney.");

            #endregion

            # region Rule Check
            IAttorney attorney = Parameters[0] as IAttorney;
            ILegalEntityCompany attorneyLegalEntityCompany = attorney.LegalEntity as ILegalEntityCompany;

            if (String.IsNullOrEmpty(attorneyLegalEntityCompany.RegisteredName))
                AddMessage(String.Format("Attorney Name required."), "", Messages);

            if (String.IsNullOrEmpty(attorney.AttorneyContact))
                AddMessage(String.Format("Attorney Contact required."), "", Messages);
            else if (!CommonValidation.IsAlphaWithSpecial(attorney.AttorneyContact))
                AddMessage(String.Format("Attorney Contact must be alphabetic characters only."), "", Messages);

            if (String.IsNullOrEmpty(attorneyLegalEntityCompany.WorkPhoneCode) || String.IsNullOrEmpty(attorneyLegalEntityCompany.WorkPhoneNumber))
                AddMessage(String.Format("Phone Number required."), "", Messages);

            if (String.IsNullOrEmpty(attorneyLegalEntityCompany.EmailAddress))
                AddMessage(String.Format("Email Address required."), "", Messages);
            else if (!CommonValidation.IsEmail(attorneyLegalEntityCompany.EmailAddress))
                AddMessage(String.Format("Email Address is invalid."), "", Messages);

            if (String.IsNullOrEmpty(attorney.AttorneyWorkFlowEnabled.ToString()))
                AddMessage(String.Format("Workflow Enabled required."), "", Messages);

            if (String.IsNullOrEmpty(attorney.AttorneyMandate.ToString()))
                AddMessage(String.Format("Attorney Mandate required."), "", Messages);

            if (String.IsNullOrEmpty(attorney.AttorneyRegistrationInd.ToString()))
                AddMessage(String.Format("Registration Attorney required."), "", Messages);

            if (String.IsNullOrEmpty(attorney.AttorneyLitigationInd.ToString()))
                AddMessage(String.Format("Litigation Attorney required."), "", Messages);

            if (Messages.Count == 0)
                return 0;
            else
                return 1;

            #endregion
        }
    }

    [RuleDBTag("ApplicatonDuplicateInstructionCheck",
    "Prevent instructing application if there is already an instructed application for the same property.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Attorney.Attorney.ApplicatonDuplicateInstructionCheck")]
    [RuleInfo]
    public class ApplicatonDuplicateInstructionCheck : BusinessRuleBase
    {
        public ApplicatonDuplicateInstructionCheck(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicatonDuplicateInstructionCheck rule expects a Domain object to be passed.");

            IApplication application = Parameters[0] as IApplication;

            if (application == null)
                throw new ArgumentException("The ApplicatonDuplicateInstructionCheck rule expects the following objects to be passed: IApplication.");

            IApplicationMortgageLoan appML = application as IApplicationMortgageLoan;
            string query = UIStatementRepository.GetStatement("Rules.Attorney", "ApplicatonDuplicateInstructionCheck");
            ParameterCollection pc = new ParameterCollection();
            pc.Add(new SqlParameter("@GenericKey", application.Key));
            pc.Add(new SqlParameter("@PropertyKey", appML.Property.Key));
            pc.Add(new SqlParameter("@StageDefinitionStageDefinitionGroupKey", (int)StageDefinitionStageDefinitionGroups.InstructAttorney));
            object o = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), pc);

            if (o != null)
            {
                string msg = string.Format("Application {0}, linked to the same property has already been instructed.", Convert.ToInt32(o));
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("AttorneyStatusInactive",
    "The attorney status cannot be set to Inactive if the Litigation Attorney is active.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Attorney.Attorney.AttorneyStatusInactive")]
    [RuleInfo]
    public class AttorneyStatusInactive : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The AttorneyStatusInactive rule expects a Domain object to be passed.");

            IAttorney att = Parameters[0] as IAttorney;

            if (att == null)
                throw new ArgumentException("The AttorneyStatusInactive rule expects the following objects to be passed: IAttorney.");

            if ((att.GeneralStatus == null || att.GeneralStatus.Key == (int)GeneralStatuses.Inactive)
                && (att.AttorneyLitigationInd.HasValue && att.AttorneyLitigationInd == true))
            {
                string msg = "The attorney status cannot be set to Inactive if the Litigation Attorney is active.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    // THES
    // [RuleDBTag("AttorneyValidateCancellingCancellationAttorney",
    //"For a switch loan there must be at least 1 cancellation attorney.",
    // "SAHL.Rules.DLL",
    //"SAHL.Common.BusinessModel.Rules.Attorney.Attorney.AttorneyValidateCancellingCancellationAttorney")]
    // [RuleInfo]
    // public class AttorneyValidateCancellingCancellationAttorney : BusinessRuleBase
    // {
    //     public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
    //     {
    //         #   region Check for allowed object type(s)
    //         if (Parameters.Length == 0)
    //             throw new ArgumentException("The AttorneyValidateCancellingCancellationAttorney rule expects a Domain object to be passed.");

    //         if (!(Parameters[0] is IApplicationMortgageLoanSwitch))
    //             throw new ArgumentException("The AttorneyValidateCancellingCancellationAttorney rule expects the following objects to be passed: IApplicationMortgageLoanSwitch");

    //         //if (RuleItem.RuleParameters.Count < 1)
    //         //    throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

    //         #endregion

    //         # region Rule Check

    //         IApplicationMortgageLoanSwitch applicationMortgageLoanSwitch = Parameters[0] as IApplicationMortgageLoanSwitch;

    //         foreach (IApplicationRole applicationRole in applicationMortgageLoanSwitch.ApplicationRoles)
    //         {
    //             if (applicationRole.ApplicationRoleType.Key == (int)RoleTypes.CancellationAttorney.TransferringAttorney)
    //                 return 0;
    //         }

    //         #endregion

    //         AddMessage(String.Format("No Cancellation Attorney has been assigned to this Switch Loan."), "", Messages);
    //         return 0;

    //     }
    // }

    // [RuleDBTag("AttorneyValidateDefendCancellationAttorney",
    //"If an account is a defending cancellation, it must be distributed between the selected attorneys who deal with defending cancellation instructions.",
    // "SAHL.Rules.DLL",
    //"SAHL.Common.BusinessModel.Rules.Attorney.Attorney.AttorneyValidateDefendCancellationAttorney")]
    // [RuleInfo]
    // public class AttorneyValidateDefendCancellationAttorney : BusinessRuleBase
    // {
    //     public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
    //     {
    //         #   region Check for allowed object type(s)
    //         if (Parameters.Length == 0)
    //             throw new ArgumentException("The AttorneyValidateDefendCancellationAttorney rule expects a Domain object to be passed.");

    //         if (!(Parameters[0] is IRateOverride))
    //             throw new ArgumentException("The AttorneyValidateDefendCancellationAttorney rule expects the following objects to be passed: IRateOverride");

    //         //if (RuleItem.RuleParameters.Count < 1)
    //         //    throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

    //         #endregion

    //         # region Rule Check

    //         IRateOverride rateOverride = Parameters[0] as IRateOverride;
    //         if (rateOverride.RateOverrideType == 4)
    //         {
    //             foreach (IRole role in rateOverride.FinancialService.Account.Roles)
    //             {
    //                 if (role.RoleType.Key == (int)RoleTypes.DefendingCancellationAttorney)
    //                     return 0;
    //             }
    //         }

    //         #endregion

    //         AddMessage(String.Format("Defending cancellation accounts can only be distributed to attorney's handing defending cancellation instructions."), "", Messages);
    //         return 0;

    //     }
    // }
}