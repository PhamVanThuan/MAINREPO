using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SAHL.Common.BusinessModel.Rules.ApplicationRole
{
    [RuleDBTag("LegalEntityApplicantNaturalPersonDeclarations",
   "Applicant declarations prevents application from being submitted",
   "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.ApplicationRole.LegalEntityApplicantNaturalPersonDeclarations")]
    [RuleInfo]
    public class LegalEntityApplicantNaturalPersonDeclarations : BusinessRuleBase
    {
        public LegalEntityApplicantNaturalPersonDeclarations(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplicationMortgageLoan appML = Parameters[0] as IApplicationMortgageLoan;

            //IDbConnection con = Helper.GetSQLDBConnection();
            //con.Open();

            string sqlQuery = UIStatementRepository.GetStatement("COMMON", "LEDeclarations");
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@applicationKey", appML.Key));

            DataSet dsRule = this.castleTransactionService.ExecuteQueryOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);
            if (dsRule != null)
            {
                if (dsRule.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsRule.Tables[0].Rows)
                    {
                        AddMessage((string)dr[0], (string)dr[0], Messages);
                    }
                }
            }

            //ParameterCollection prms = new ParameterCollection();
            //Helper.AddIntParameter(prms, "@applicationKey", appML.Key);

            //reader = Helper.ExecuteReader(con, sqlQuery, prms);
            //while (reader.Read())
            //{
            //    AddMessage((string)reader.GetString(0), (string)reader.GetString(0), Messages);
            //}

            return 0;
        }
    }

    [RuleDBTag("ValidateUniqueClientRole",
   "ValidateUniqueClientRole prevents the same legalentity (client roletypegroup) from being added to the application",
   "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.ApplicationRole.ValidateUniqueClientRole")]
    [RuleInfo]
    public class ValidateUniqueClientRole : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ValidateUniqueClientRole rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationRole))
                throw new ArgumentException("The ValidateUniqueClientRole rule expects the following objects to be passed: IApplicationRole.");

            // get the application
            IApplicationRole newApplicationRole = Parameters[0] as IApplicationRole;
            IApplication application = newApplicationRole.Application;

            bool clientRoleExists = false;

            // check if this person already exists
            foreach (IApplicationRole existingApplicationRole in application.ApplicationRoles)
            {
                if (existingApplicationRole.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)SAHL.Common.Globals.OfferRoleTypeGroups.Client
                    && existingApplicationRole.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
                {
                    if (existingApplicationRole.LegalEntity.Key == newApplicationRole.LegalEntity.Key)
                    {
                        clientRoleExists = true;
                        break;
                    }
                }
            }

            if (clientRoleExists == true)
            {
                AddMessage("This client already exists on this application.", "This client already exists on this application.", Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("ValidateRemovedRoleMailingAddress",
    "ValidateRemovedRoleMailingAddress prevents the legalentity from being removed if their address is being used as the application mailing address",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.ApplicationRole.ValidateRemovedRoleMailingAddress")]
    [RuleInfo]
    public class ValidateRemovedRoleMailingAddress : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ValidateRemovedRoleMailingAddress rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationRole))
                throw new ArgumentException("The ValidateRemovedRoleMailingAddress rule expects the following objects to be passed: IApplicationRole.");

            // get the applicationrole
            IApplicationRole applicationRole = Parameters[0] as IApplicationRole;

            // ignore applicationrole if it is not a 'Client' offerroletype group
            if (applicationRole.ApplicationRoleType.ApplicationRoleTypeGroup.Key != (int)Globals.OfferRoleTypeGroups.Client)
                return 1;

            // if there is no application mailing address then exit and pass the validation
            if (applicationRole.Application.ApplicationMailingAddresses.Count <= 0)
                return 1;

            bool applicantHasMailingAddress = false, otherApplicantHasMailingAddress = false;
            int applicationMailingAddressKey = applicationRole.Application.ApplicationMailingAddresses[0].Address.Key;

            foreach (IApplicationRole appRole in applicationRole.Application.ApplicationRoles)
            {
                if (appRole.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)SAHL.Common.Globals.OfferRoleTypeGroups.Client
                    && appRole.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
                {
                    // check if the mailing address key belongs to the legalentity we are removing and/or one of the other legalentiies
                    foreach (ILegalEntityAddress legalEntityAddress in appRole.LegalEntity.LegalEntityAddresses)
                    {
                        if (legalEntityAddress.Address.Key == applicationMailingAddressKey)
                        {
                            if (appRole.LegalEntity.Key == applicationRole.LegalEntity.Key)
                                applicantHasMailingAddress = true;
                            else
                                otherApplicantHasMailingAddress = true;
                        }
                    }
                }
            }

            // if the address belongs to the legalentity we are removing and not to any of the other legalenties then fail validation
            if (applicantHasMailingAddress == true && otherApplicantHasMailingAddress == false)
            {
                string msg = "This applicant cannot be removed - their address is the application mailing address.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    //[RuleDBTag("ApplicationDeclarationAnswers",
    //"Answer is No. Date must be null",
    //"SAHL.Rules.DLL",
    //"SAHL.Common.BusinessModel.Rules.ApplicationRole.ApplicationDeclarationAnswers")]
    //[RuleInfo]
    //public class ApplicationDeclarationAnswers : BusinessRuleBase
    //{
    //    public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
    //    {
    //        IApplicationDeclaration appDec = Parameters[0] as IApplicationDeclaration;
    //        IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

    //        IList<IApplicationDeclaration> appDecLst = appRepo.GetApplicationDeclarationsByapplicationRoleKey(appDec.ApplicationRole.Key);

    //        bool declaredInsolvent = false;
    //        bool underAdmin = false;

    //        if (appDecLst != null)
    //        {
    //            for (int i = 0; i < appDecLst.Count; i++)
    //            {
    //                if (appDecLst[i].ApplicationDeclarationQuestion.Key == 1 && appDecLst[i].ApplicationDeclarationAnswer.Key == (int)OfferDeclarationAnswers.Yes)
    //                    declaredInsolvent = true;

    //                if (!declaredInsolvent && appDecLst[i].ApplicationDeclarationQuestion.Key == 2 && appDecLst[i].ApplicationDeclarationDate != null)
    //                {
    //                    AddMessage("Rehabilitation Date must be null.", "Rehabilitation Date must be null.", Messages);
    //                    return 0;
    //                }

    //                if (appDecLst[i].ApplicationDeclarationQuestion.Key == 3 && appDecLst[i].ApplicationDeclarationAnswer.Key == (int)OfferDeclarationAnswers.Yes)
    //                    underAdmin = true;

    //                if (!underAdmin && appDecLst[i].ApplicationDeclarationQuestion.Key == 4 && appDecLst[i].ApplicationDeclarationDate != null)
    //                {
    //                    AddMessage("Administration Order rescinded Date must be null.", "Administration Order rescinded Date must be null.", Messages);
    //                    return 0;
    //                }
    //            }
    //        }
    //        return 1;
    //    }
    //}

    [RuleDBTag("ValidateRemovedRoleDebitOrder",
  "ValidateRemovedRoleDebitOrder prevents the legalentity from being removed if their bank account is used in the Application Debit Order",
   "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.ApplicationRole.ValidateRemovedRoleDebitOrder")]
    [RuleInfo]
    public class ValidateRemovedRoleDebitOrder : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplicationRole applicationRole = Parameters[0] as IApplicationRole;

            // ignore applicationrole if it is not a 'Client' offerroletype group
            if (applicationRole.ApplicationRoleType.ApplicationRoleTypeGroup.Key != (int)Globals.OfferRoleTypeGroups.Client)
                return 1;

            // if there is no application debit order then exit and pass the validation
            if (applicationRole.Application.ApplicationDebitOrders.Count <= 0)
                return 1;

            IBankAccount applicationDebitOrderBankAccount = applicationRole.Application.ApplicationDebitOrders[0].BankAccount;

            // if there is no debit order back account then exit and pass the validation
            if (applicationDebitOrderBankAccount == null)
                return 1;

            bool applicantHasDebitOrder = false, otherApplicantHasDebitOrder = false;

            //int applicationDebitOrderBankAccountKey = applicationRole.Application.ApplicationDebitOrders[0].BankAccount.Key;

            foreach (IApplicationRole appRole in applicationRole.Application.ApplicationRoles)
            {
                if (appRole.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)SAHL.Common.Globals.OfferRoleTypeGroups.Client
                    && appRole.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
                {
                    // check if the mailing address key belongs to the legalentity we are removing and/or one of the other legalentiies
                    foreach (ILegalEntityBankAccount legalEntityBA in appRole.LegalEntity.LegalEntityBankAccounts)
                    {
                        if (legalEntityBA.BankAccount.Key == applicationDebitOrderBankAccount.Key)
                        {
                            if (appRole.LegalEntity.Key == applicationRole.LegalEntity.Key)
                                applicantHasDebitOrder = true;
                            else
                                otherApplicantHasDebitOrder = true;
                        }
                    }
                }
            }

            // if the address belongs to the legalentity we are removing and not to any of the other legalenties then fail validation
            if (applicantHasDebitOrder == true && otherApplicantHasDebitOrder == false)
            {
                string msg = "This applicant cannot be removed -  Applicant's bank account is the application debit order bank account.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    #region ApplicationRoleRemoveLegalEntityMinimum

    [RuleDBTag("ApplicationRoleRemoveLegalEntityMinimum",
    "Checks that the application has at least one LE of type Lead Main Applicant or Main Applicant",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.ApplicationRole.ApplicationRoleRemoveLegalEntityMinimum")]
    [RuleInfo]
    public class ApplicationRoleRemoveLegalEntityMinimum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ValidateUniqueClientRole rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationRole))
                throw new ArgumentException("The ValidateUniqueClientRole rule expects the following objects to be passed: IApplicationRole.");

            IApplicationRole applicationRole = Parameters[0] as IApplicationRole;
            IApplication application = applicationRole.Application;

            // ignore life applications as these have no roles - the roles are stored against the account
            if (application is IApplicationLife)
                return 1;

            // ignore applicationrole if it is not a 'Client' offerroletype group
            if (applicationRole.ApplicationRoleType.ApplicationRoleTypeGroup.Key != (int)Globals.OfferRoleTypeGroups.Client)
                return 1;

            ILegalEntity le = applicationRole.LegalEntity;

            foreach (IApplicationRole appRole in application.ApplicationRoles)
            {
                if ((appRole.LegalEntity.Key != le.Key && appRole.GeneralStatus.Key == (int)GeneralStatuses.Active) &&
                    (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant))
                {
                    return 1;
                }
            }

            String msg = string.Format("A Mortgage Loan must have at least one active Legal Entity role of type {0} or {1}", OfferRoleTypes.LeadMainApplicant, OfferRoleTypes.MainApplicant);
            AddMessage(msg, msg, Messages);
            return 1;
        }
    }

    #endregion ApplicationRoleRemoveLegalEntityMinimum

    #region ApplicationRoleUpdateLegalEntityMinimum

    [RuleDBTag("ApplicationRoleUpdateLegalEntityMinimum",
    "Checks that the application has at least one LE of type Lead Main Applicant or Main Applicant",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.ApplicationRole.ApplicationRoleUpdateLegalEntityMinimum")]
    [RuleInfo]
    public class ApplicationRoleUpdateLegalEntityMinimum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ValidateUniqueClientRole rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationRole))
                throw new ArgumentException("The ValidateUniqueClientRole rule expects the following objects to be passed: IApplicationRole.");

            IApplicationRole applicationRole = Parameters[0] as IApplicationRole;
            IApplication application = applicationRole.Application;

            // ignore life applications as these have no roles - the roles are stored against the account
            if (application is IApplicationLife)
                return 1;

            // ignore applicationrole if it is not a 'Client' offerroletype group
            if (applicationRole.ApplicationRoleType.ApplicationRoleTypeGroup.Key != (int)Globals.OfferRoleTypeGroups.Client)
                return 1;

            foreach (IApplicationRole appRole in application.ApplicationRoles)
            {
                if ((appRole.GeneralStatus.Key == (int)GeneralStatuses.Active) &&
                    (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant))
                {
                    return 1;
                }
            }

            String msg = string.Format("A Mortgage Loan must have at least one active Legal Entity role of type {0} or {1}", OfferRoleTypes.LeadMainApplicant, OfferRoleTypes.MainApplicant);
            AddMessage(msg, msg, Messages);
            return 1;
        }
    }

    #endregion ApplicationRoleUpdateLegalEntityMinimum

    /// <summary>
    /// This rules only applies to the Reassign User Presenter
    /// </summary>
    [RuleDBTag("ReassignUserValidateLoggedInUser",
    "ReassignUserValidateLoggedInUser prevents an unauthorised user from attempting to reassign an application role",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.ApplicationRole.ReassignUserValidateLoggedInUser")]
    [RuleInfo]
    public class ReassignUserValidateLoggedInUser : BusinessRuleBase
    {
        public ReassignUserValidateLoggedInUser(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ReassignUserValidateLoggedInUser rule expects a Domain object to be passed.");

            IApplicationRole applicationRole = Parameters[0] as IApplicationRole;
            IADUser aduser = Parameters[1] as IADUser;

            if (applicationRole == null)
                throw new ArgumentException("The ReassignUserValidateLoggedInUser rule expects a Domain object to be passed : IApplicationRole");

            if (aduser == null)
                throw new ArgumentException("The ReassignUserValidateLoggedInUser rule expects a Domain object to be passed : IADUser");

            IOrganisationStructureRepository osr = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IOrganisationStructure organisationStructure = osr.GetOrganisationStructForADUser(applicationRole);

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@ADUserKey", aduser.Key));
            prms.Add(new SqlParameter("@OrganisationStructureKey", organisationStructure.Key));
            prms.Add(new SqlParameter("@ApplicationRoleKey", applicationRole.Key));
            string query = UIStatementRepository.GetStatement("Rules.ApplicationRole", "ReassignUserValidateLoggedInUser");
            object o = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(OrganisationStructure_DAO), prms);

            if (o != null && Convert.ToInt16(o) > 0)
                return 0;

            IADUser currentADUser = osr.GetAdUserByLegalEntityKey(applicationRole.LegalEntity.Key);
            string msg = string.Format(@"{0} attempting to reassign application role from {1} is not allowed as they both are not part of the same organisation structure."
                                        , aduser.ADUserName, currentADUser.ADUserName);
            AddMessage(msg, msg, Messages);
            return 1;

            //foreach (IUserOrganisationStructure userOrgStruct in aduser.UserOrganisationStructure)
            //{
            //    if (userOrgStruct.OrganisationStructure.Parent != null &&
            //        organisationStructure.Parent != null &&
            //        userOrgStruct.OrganisationStructure.Parent.Key == organisationStructure.Parent.Key)
            //        return 0;
            //}
        }
    }

    /// <summary>
    ///
    /// </summary>
    [RuleDBTag("OfferroleMatchAccountroleandLEKeySuretyCheck",
    "This rule must determine whether the sureties on the application are the same sureties on the account.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.ApplicationRole.OfferroleMatchAccountroleandLEKeySuretyCheck")]
    [RuleInfo]
    public class OfferroleMatchAccountroleandLEKeySuretyCheck : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The OfferroleMatchAccountroleandLEKeySuretyCheck rule expects a Domain object to be passed.");

            IApplication application = Parameters[0] as IApplication;

            if (application == null)
                throw new ArgumentException("The OfferroleMatchAccountroleandLEKeySuretyCheck rule expects the following objects to be passed: IApplication.");

            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            DataTable dt = appRepo.GetOfferRolesNotInAccount(application);

            if (dt != null && dt.Rows.Count > 0)
            {
                string msg = string.Format("CAUTION: {0} NEW APPLICANT/S IN PLACE", dt.Rows.Count);
                AddMessage(msg, msg, Messages);
            }

            return 1;
        }
    }

    /// <summary>
    ///
    /// </summary>
    [RuleDBTag("OfferRoleFLAddMainApplicant",
    "This rule must prevent users adding Main Applicants to FL Offers.",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.ApplicationRole.OfferRoleFLAddMainApplicant")]
    [RuleInfo]
    public class OfferRoleFLAddMainApplicant : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The OfferRoleFLAddMainApplicant rule expects a Domain object to be passed.");

            IApplicationRole ar = Parameters[0] as IApplicationRole;

            if (ar == null)
                throw new ArgumentException("The OfferRoleFLAddMainApplicant rule expects the following objects to be passed: IApplicationRole.");

            //if the application type is null this is a new business creation, so ignore also
            if (ar.Application == null || ar.Application.ApplicationType == null)
                return 1;

            //This rule is only valid for readvance and further advance
            if ((ar.Application.ApplicationType.Key == (int)OfferTypes.ReAdvance || ar.Application.ApplicationType.Key == (int)OfferTypes.FurtherAdvance) && ar.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant)
            {
                string msg = "Main applicants can not be added to Readvance & Further Advance applications.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("CheckFurtherLendingApplicationRoleBeforeDelete",
    "Cannot delete an application role from a Further Lending Application if Role exists on Account.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.ApplicationRole.CheckFurtherLendingApplicationRoleBeforeDelete")]
    [RuleInfo]
    public class CheckFurtherLendingApplicationRoleBeforeDelete : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The CheckFurtherLendingApplicationRoleBeforeDelete rule expects a Domain object to be passed.");

            IApplicationRole appRole = Parameters[0] as IApplicationRole;

            if (appRole != null && (appRole.Application.ApplicationType.Key == (int)OfferTypes.ReAdvance ||
                appRole.Application.ApplicationType.Key == (int)OfferTypes.FurtherAdvance ||
                appRole.Application.ApplicationType.Key == (int)OfferTypes.FurtherLoan))
            {
                foreach (IRole role in appRole.Application.Account.Roles)
                {
                    if (appRole.LegalEntity.Key == role.LegalEntity.Key)
                    {
                        string msg = string.Format("Cannot delete a legal entity from the application that exists on the account.");
                        AddMessage(msg, msg, Messages);
                        return 0;
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("CheckFurtherLendingApplicationRoleBeforeUpdate",
    "Cannot update Legal Entity on a Further Lending Application if Account Role exists.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.ApplicationRole.CheckFurtherLendingApplicationRoleBeforeUpdate")]
    [RuleInfo]
    public class CheckFurtherLendingApplicationRoleBeforeUpdate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The CheckFurtherLendingApplicationRoleBeforeUpdate rule expects a Domain object to be passed.");

            IApplicationRole appRole = Parameters[0] as IApplicationRole;
            ILegalEntity legalEntity = Parameters[0] as ILegalEntity;

            if ((appRole != null && appRole.Key > 0) && (appRole.Application.ApplicationType.Key == (int)OfferTypes.ReAdvance ||
                appRole.Application.ApplicationType.Key == (int)OfferTypes.FurtherAdvance ||
                appRole.Application.ApplicationType.Key == (int)OfferTypes.FurtherLoan))
            {
                foreach (IRole role in appRole.Application.Account.Roles)
                {
                    if (role.GeneralStatus.Key == (int)GeneralStatuses.Active && appRole.LegalEntity.Key == role.LegalEntity.Key)
                    {
                        string msg = string.Format("Cannot update a legal entity in the application that exists on the account.");
                        AddMessage(msg, msg, Messages);
                        return 0;
                    }
                }
            }
            else if (legalEntity != null)
            {
                foreach (IApplicationRole leAppRole in legalEntity.ApplicationRoles)
                {
                    if (leAppRole.Application.ApplicationType.Key == (int)OfferTypes.ReAdvance ||
                        leAppRole.Application.ApplicationType.Key == (int)OfferTypes.FurtherAdvance ||
                        leAppRole.Application.ApplicationType.Key == (int)OfferTypes.FurtherLoan)
                    {
                        foreach (IRole leRole in leAppRole.Application.Account.Roles)
                        {
                            if (leRole.GeneralStatus.Key == (int)GeneralStatuses.Active && leAppRole.LegalEntity.Key == leRole.LegalEntity.Key)
                            {
                                string msg = string.Format("Cannot update a legal entity in the application that exists on the account.");
                                AddMessage(msg, msg, Messages);
                                return 0;
                            }
                        }
                    }
                }
            }

            return 1;
        }
    }

    /// <summary>
    ///
    /// </summary>
    [RuleDBTag("CheckIsReturningClient",
    "Checks if a legal entity for a given offer role on a new business application is a main applicant on a mortgage loan.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.ApplicationRole.CheckIsReturningClient")]
    [RuleInfo]
    public class CheckIsReturningClient : BusinessRuleBase
    {
        public CheckIsReturningClient(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The CheckIsReturningClient rule expects a Domain object to be passed.");

            IApplicationRole applicationRole = Parameters[0] as IApplicationRole;

            if (applicationRole == null)
                throw new ArgumentException("The CheckIsReturningClient rule expects a Domain object to be passed : IApplicationRole");

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@OfferRoleKey", applicationRole.Key));
            string query = UIStatementRepository.GetStatement("Rules.ApplicationRole", "CheckIsReturningClient");
            object o = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), prms);

            if (o != null && Convert.ToInt16(o) > 0)
                return 1;

            return 0;
        }
    }
}