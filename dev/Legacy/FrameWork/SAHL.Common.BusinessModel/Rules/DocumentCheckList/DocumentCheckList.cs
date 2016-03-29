using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Queries;
using NHibernate;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.DocumentCheckList
{
    #region GenericKeyType != Null

    [RuleDBTag("DocumentCheckListRequiredLegalEntitySAID",
    "Per NaturalPersonLegalEntity if they have IDNumber and  in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntitySAID")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntitySAID : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        ILegalEntityNaturalPerson LENP = appRole.LegalEntity as ILegalEntityNaturalPerson;
                        if ((LENP != null && LENP.CitizenType != null) &&
                            (LENP.CitizenType.Key == (int)CitizenTypes.SACitizen || LENP.CitizenType.Key == (int)CitizenTypes.SACitizenNonResident))
                        {
                            string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                            if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                DictList.Add(appRole.LegalEntity.Key, description);
                        }
                    }
                }
            }

            // If document is required then returned "1"
            // If document is not required then returned "0"
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntitySalarySliporLetterofAppointmentorIRP5",
    "Per NaturalPersonLegalEntity if Salaried Employment and in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntitySalarySliporLetterofAppointmentorIRP5")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntitySalarySliporLetterofAppointmentorIRP5 : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        foreach (IEmployment emp in appRole.LegalEntity.Employment)
                        {
                            if (emp.EmploymentStatus.Key == (int)EmploymentStatuses.Current && emp.EmploymentType.Key == (int)EmploymentTypes.Salaried)
                            {
                                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                    DictList.Add(appRole.LegalEntity.Key, description);

                                break;
                            }
                        }
                    }
                }
            }

            // If document is required then returned "1"
            // If document is not required then returned "0"
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityMarriageCertificate",
    "Per NaturalPersonLegalEntity if they are Married and in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityMarriageCertificate")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityMarriageCertificate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        ILegalEntityNaturalPerson LENP = appRole.LegalEntity as ILegalEntityNaturalPerson;
                        if (LENP != null && LENP.MaritalStatus != null)
                        {
                            if ((LENP.MaritalStatus.Key == (int)MaritalStatuses.MarriedAnteNuptualContract)
                               || (LENP.MaritalStatus.Key == (int)MaritalStatuses.MarriedCommunityofProperty)
                               || (LENP.MaritalStatus.Key == (int)MaritalStatuses.MarriedForeign))
                            {
                                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                    DictList.Add(appRole.LegalEntity.Key, description);
                            }
                        }
                    }
                }
            }

            // If document is required then returned "1"
            // If document is not required then returned "0"
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityANCContract",
    "Per NaturalPersonLegalEntity if they are Married with ANC and  in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityANCContract")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityANCContract : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        ILegalEntityNaturalPerson LENP = appRole.LegalEntity as ILegalEntityNaturalPerson;
                        if (LENP != null && LENP.MaritalStatus != null)
                        {
                            if (LENP.MaritalStatus.Key == (int)MaritalStatuses.MarriedAnteNuptualContract)
                            {
                                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                    DictList.Add(appRole.LegalEntity.Key, description);
                            }
                        }
                    }
                }
            }

            // If document is required then returned "1"
            // If document is not required then returned "0"
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityDivorceAgreement",
    "Per NaturalPersonLegalEntity if they are divorced and in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityDivorceAgreement")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityDivorceAgreement : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        ILegalEntityNaturalPerson LENP = appRole.LegalEntity as ILegalEntityNaturalPerson;
                        if (LENP != null && LENP.MaritalStatus != null)
                        {
                            if (LENP.MaritalStatus.Key == (int)MaritalStatuses.Divorced)
                            {
                                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                    DictList.Add(appRole.LegalEntity.Key, description);
                            }
                        }
                    }
                }
            }

            // If document is required then returned "1"
            // If document is not required then returned "0"
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityPassport",
    "Per NaturalPersonLegalEntity if they have PassportNumber and in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityPassport")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityPassport : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        ILegalEntityNaturalPerson LENP = appRole.LegalEntity as ILegalEntityNaturalPerson;
                        if ((LENP != null && LENP.CitizenType != null) && (LENP.CitizenType.Key == (int)CitizenTypes.Foreigner
                            || LENP.CitizenType.Key == (int)CitizenTypes.NonResidentConsulate
                            || LENP.CitizenType.Key == (int)CitizenTypes.NonResidentDiplomat
                            || LENP.CitizenType.Key == (int)CitizenTypes.NonResidentHighCommissioner
                            || LENP.CitizenType.Key == (int)CitizenTypes.NonResidentRefugee
                            || LENP.CitizenType.Key == (int)CitizenTypes.NonResident
                            ))
                        {
                            string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                            if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                DictList.Add(appRole.LegalEntity.Key, description);
                        }
                    }
                }
            }

            // If document is required then returned "1"
            // If document is not required then returned "0"
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredApplicationIncomeAndExpenditure",
    "Per NaturalPersonLegalEntity if they have PassportNumber and in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredApplicationIncomeAndExpenditure")]
    [RuleInfo]
    public class DocumentCheckListRequiredApplicationIncomeAndExpenditure : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                        if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                            DictList.Add(appRole.LegalEntity.Key, description);
                    }
                }
            }

            // If document is required then returned "1"
            // If document is not required then returned "0"
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityAssetsAndLiabilities",
    "Per LegalEntity ((where the NaturalPersonLegalEntity is older than 58 years OR Self Employed) OR (where the Application Loan Amount is greater than R1,500,00.00)) in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityAssetsAndLiabilities")]
    [RuleParameterTag(new string[] { "@DCLMaxLAA, 1500000.00, 7", "@DCLLEAge,58,9" })]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityAssetsAndLiabilities : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        //1
                        ILegalEntityNaturalPerson LENP = appRole.LegalEntity as ILegalEntityNaturalPerson;
                        if (LENP != null)
                        {
                            int DCLLEAge = Convert.ToInt32(RuleItem.RuleParameters[1].Value);
                            if (LENP.AgeNextBirthday.HasValue && LENP.AgeNextBirthday.Value > DCLLEAge)
                            {
                                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                    DictList.Add(appRole.LegalEntity.Key, description);
                                continue;
                            }

                            //2
                            bool cancelLoop = false;
                            foreach (IEmployment emp in LENP.Employment)
                            {
                                if (emp.EmploymentStatus.Key == (int)GeneralStatuses.Active && emp.EmploymentType.Key == (int)EmploymentTypes.SelfEmployed)
                                {
                                    string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                    if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                        DictList.Add(appRole.LegalEntity.Key, description);
                                    cancelLoop = true;
                                    break;
                                }
                            }
                            if (cancelLoop)
                                break;
                        }

                        //3
                        if (CheckLoanAgreementAmt(application))
                        {
                            string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                            if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                DictList.Add(appRole.LegalEntity.Key, description);
                            continue;
                        }
                    }
                }
            }

            // If document is required then returned "1"
            // If document is not required then returned "0"
            return 1;
        }

        private bool CheckLoanAgreementAmt(IApplication application)
        {
            double MaxLAA = Convert.ToDouble(RuleItem.RuleParameters[0].Value);
            ISupportsVariableLoanApplicationInformation vlai = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
            IApplicationInformationVariableLoan vli = vlai.VariableLoanInformation;

            // for fl we need to add LoanAgreeAmount from the application to the
            // existing values for the Account
            // We are not concerned about other applications, as only one application can be processed at a time.
            if (application.ApplicationType.Key == (int)OfferTypes.ReAdvance ||
                application.ApplicationType.Key == (int)OfferTypes.FurtherAdvance ||
                application.ApplicationType.Key == (int)OfferTypes.FurtherLoan)
            {
                //Get Account LTV & PTI
                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                int accountKey = application.ReservedAccount.Key;
                IAccount acc = accRepo.GetAccountByKey(accountKey);

                double fixLAA = 0;

                IMortgageLoanAccount mla = acc as IMortgageLoanAccount;
                IMortgageLoan vml = mla.SecuredMortgageLoan;

                //Get any varifix details, need to recalc using any application update values e.g.: Margin (LinkRate)
                if (acc.Product.Key == (int)SAHL.Common.Globals.Products.VariFixLoan)
                {
                    IAccountVariFixLoan fAccount = acc as IAccountVariFixLoan;
                    IMortgageLoan fml = fAccount.FixedSecuredMortgageLoan;
                    fixLAA = fml.CurrentBalance;
                }

                double totalLAA = vml.CurrentBalance + fixLAA + (vli.LoanAgreementAmount.HasValue ? vli.LoanAgreementAmount.Value : 0D);

                if (totalLAA > MaxLAA)
                {
                    return true;
                }
            }
            else
            {
                if (vlai != null && vli != null)
                {
                    //For new business we can just check the LoanAgreementAmount stored against the application
                    if ((vli.LoanAgreementAmount.HasValue ? vli.LoanAgreementAmount.Value : 0D) > MaxLAA)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityLatestComplete3MonthBankStatementsPersonal",
    "Per NaturalPersonLegalEntity in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityLatestComplete3MonthBankStatementsPersonal")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityLatestComplete3MonthBankStatementsPersonal : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;
            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    bool doAdd = true;
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        if (appRole.LegalEntity is ILegalEntityNaturalPerson)
                        {
                            foreach (IEmployment emp in appRole.LegalEntity.Employment)
                            {
                                if (emp.EmploymentStatus.Key == (int)EmploymentStatuses.Current && emp.EmploymentType.Key == (int)EmploymentTypes.SelfEmployed)
                                    doAdd = false;
                            }

                            if (doAdd)
                            {
                                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                    DictList.Add(appRole.LegalEntity.Key, description);
                            }
                        }
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityLatestComplete6MonthBankStatementsPersonal",
    "Per NaturalPersonLegalEntity in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityLatestComplete6MonthBankStatementsPersonal")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityLatestComplete6MonthBankStatementsPersonal : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;
            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        if (appRole.LegalEntity is ILegalEntityNaturalPerson)
                        {
                            foreach (IEmployment emp in appRole.LegalEntity.Employment)
                            {
                                if (emp.EmploymentStatus.Key == (int)EmploymentStatuses.Current && emp.EmploymentType.Key == (int)EmploymentTypes.SelfEmployed)
                                {
                                    string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                    if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                        DictList.Add(appRole.LegalEntity.Key, description);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityLatestComplete3MonthBankStatementsBusiness",
    "Per LegalEntityCompany, LegalEntityTrust, LegalEntityCC in Role or OfferRole with the application as Main Applicant or Suretor AND Per NaturalPersonLegalEntity in Role or OfferRole with the application as Main Applicant or Suretor that is Self Employed",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityLatestComplete3MonthBankStatementsBusiness")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityLatestComplete3MonthBankStatementsBusiness : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        if (appRole.LegalEntity is ILegalEntityNaturalPerson)
                        {
                            foreach (IEmployment emp in appRole.LegalEntity.Employment)
                            {
                                if (emp.EmploymentStatus.Key == (int)EmploymentStatuses.Current && emp.EmploymentType.Key == (int)EmploymentTypes.SelfEmployed)
                                {
                                    string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                    if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                        DictList.Add(appRole.LegalEntity.Key, description);
                                    break;
                                }
                            }
                        }
                        else if (appRole.LegalEntity is ILegalEntityCompany)
                        {
                            string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                            if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                DictList.Add(appRole.LegalEntity.Key, description);
                        }
                        else if (appRole.LegalEntity is ILegalEntityCloseCorporation)
                        {
                            string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                            if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                DictList.Add(appRole.LegalEntity.Key, description);
                        }
                        else if (appRole.LegalEntity is ILegalEntityTrust)
                        {
                            string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                            if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                DictList.Add(appRole.LegalEntity.Key, description);
                        }
                    }
                }
            }

            // If document is required then returned "1"
            // If document is not required then returned "0"
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityAnnualFinancialStatementsPast2Years",
    "Per LegalEntityCompany, LegalEntityTrust, LegalEntityCC in Role or OfferRole with the application as Main Applicant or Suretor AND Per NaturalPersonLegalEntity in Role or OfferRole with the application as Main Applicant or Suretor that is Self Employed",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityAnnualFinancialStatementsPast2Years")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityAnnualFinancialStatementsPast2Years : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        if (appRole.LegalEntity is ILegalEntityNaturalPerson)
                        {
                            foreach (IEmployment emp in appRole.LegalEntity.Employment)
                            {
                                if (emp.EmploymentStatus.Key == (int)EmploymentStatuses.Current && emp.EmploymentType.Key == (int)EmploymentTypes.SelfEmployed)
                                {
                                    string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                    if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                        DictList.Add(appRole.LegalEntity.Key, description);
                                    break;
                                }
                            }
                        }
                        else if (appRole.LegalEntity is ILegalEntityCompany)
                        {
                            string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                            if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                DictList.Add(appRole.LegalEntity.Key, description);
                        }
                        else if (appRole.LegalEntity is ILegalEntityCloseCorporation)
                        {
                            string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                            if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                DictList.Add(appRole.LegalEntity.Key, description);
                        }
                        else if (appRole.LegalEntity is ILegalEntityTrust)
                        {
                            string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                            if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                DictList.Add(appRole.LegalEntity.Key, description);
                        }
                    }
                }
            }

            // If document is required then returned "1"
            // If document is not required then returned "0"
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityManagementAccountsNotOlder2Months",
    "Per LegalEntityCompany, LegalEntityTrust, LegalEntityCC in Role or OfferRole with the application as Main Applicant or Suretor AND Per NaturalPersonLegalEntity in Role or OfferRole with the application as Main Applicant or Suretor that is Self Employed",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityManagementAccountsNotOlder2Months")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityManagementAccountsNotOlder2Months : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        if (appRole.LegalEntity is ILegalEntityNaturalPerson)
                        {
                            foreach (IEmployment emp in appRole.LegalEntity.Employment)
                            {
                                if (emp.EmploymentStatus.Key == (int)EmploymentStatuses.Current && emp.EmploymentType.Key == (int)EmploymentTypes.SelfEmployed)
                                {
                                    string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                    if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                        DictList.Add(appRole.LegalEntity.Key, description);
                                    break;
                                }
                            }
                        }
                        else if (appRole.LegalEntity is ILegalEntityCompany)
                        {
                            string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                            if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                DictList.Add(appRole.LegalEntity.Key, description);
                        }
                        else if (appRole.LegalEntity is ILegalEntityCloseCorporation)
                        {
                            string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                            if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                DictList.Add(appRole.LegalEntity.Key, description);
                        }
                        else if (appRole.LegalEntity is ILegalEntityTrust)
                        {
                            string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                            if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                DictList.Add(appRole.LegalEntity.Key, description);
                        }
                    }
                }
            }

            // If document is required then returned "1"
            // If document is not required then returned "0"
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityFullGeneral",
    "Per NaturalPersonLegalEntity if they are Self Employed and in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityFullGeneral")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityFullGeneral : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        if (appRole.LegalEntity is ILegalEntityNaturalPerson)
                        {
                            foreach (IEmployment emp in appRole.LegalEntity.Employment)
                            {
                                if (emp.EmploymentStatus.Key == (int)EmploymentStatuses.Current && emp.EmploymentType.Key == (int)EmploymentTypes.SelfEmployed)
                                {
                                    string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                    if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                        DictList.Add(appRole.LegalEntity.Key, description);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityFinancialStatements",
    "Per NaturalPersonLegalEntity if they are Self Employed and in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityFinancialStatements")]
    [RuleParameterTag(new string[] { "@DCLMaxLAA, 1500000.00, 7" })]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityFinancialStatements : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                if (CheckLoanAgreementAmt(application))
                {
                    foreach (IApplicationRole appRole in application.ApplicationRoles)
                    {
                        if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                            (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                            || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                            || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                            || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                        {
                            if (appRole.LegalEntity is ILegalEntityNaturalPerson)
                            {
                                foreach (IEmployment emp in appRole.LegalEntity.Employment)
                                {
                                    if (emp.EmploymentStatus.Key == (int)EmploymentStatuses.Current && emp.EmploymentType.Key == (int)EmploymentTypes.SelfEmployed)
                                    {
                                        string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                        if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                            DictList.Add(appRole.LegalEntity.Key, description);
                                        break;
                                    }
                                }
                            }
                            else if (appRole.LegalEntity is ILegalEntityCompany)
                            {
                                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                    DictList.Add(appRole.LegalEntity.Key, description);
                            }
                            else if (appRole.LegalEntity is ILegalEntityCloseCorporation)
                            {
                                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                    DictList.Add(appRole.LegalEntity.Key, description);
                            }
                            else if (appRole.LegalEntity is ILegalEntityTrust)
                            {
                                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                    DictList.Add(appRole.LegalEntity.Key, description);
                            }
                        }
                    }
                }
            }
            return 1;
        }

        private bool CheckLoanAgreementAmt(IApplication application)
        {
            double MaxLAA = Convert.ToDouble(RuleItem.RuleParameters[0].Value);
            ISupportsVariableLoanApplicationInformation vlai = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
            IApplicationInformationVariableLoan vli = vlai.VariableLoanInformation;

            // for fl we need to add LoanAgreeAmount from the application to the
            // existing values for the Account
            // We are not concerned about other applications, as only one application can be processed at a time.
            if (application.ApplicationType.Key == (int)OfferTypes.ReAdvance ||
                application.ApplicationType.Key == (int)OfferTypes.FurtherAdvance ||
                application.ApplicationType.Key == (int)OfferTypes.FurtherLoan)
            {
                //Get Account LTV & PTI
                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                int accountKey = application.ReservedAccount.Key;
                IAccount acc = accRepo.GetAccountByKey(accountKey);

                double fixLAA = 0;

                IMortgageLoanAccount mla = acc as IMortgageLoanAccount;
                IMortgageLoan vml = mla.SecuredMortgageLoan;

                //Get any varifix details, need to recalc using any application update values e.g.: Margin (LinkRate)
                if (acc.Product.Key == (int)SAHL.Common.Globals.Products.VariFixLoan)
                {
                    IAccountVariFixLoan fAccount = acc as IAccountVariFixLoan;
                    IMortgageLoan fml = fAccount.FixedSecuredMortgageLoan;
                    fixLAA = fml.CurrentBalance;
                }

                double totalLAA = vml.CurrentBalance + fixLAA + (vli.LoanAgreementAmount.HasValue ? vli.LoanAgreementAmount.Value : 0D);

                if (totalLAA > MaxLAA)
                {
                    return true;
                }
            }
            else
            {
                if (vlai != null && vli != null)
                {
                    //For new business we can just check the LoanAgreementAmount stored against the application
                    if ((vli.LoanAgreementAmount.HasValue ? vli.LoanAgreementAmount.Value : 0D) > MaxLAA)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityAccountantLetterorIT34",
    "Per NaturalPersonLegalEntity if they are Self Employed and in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityAccountantLetterorIT34")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityAccountantLetterorIT34 : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        if (appRole.LegalEntity is ILegalEntityNaturalPerson)
                        {
                            foreach (IEmployment emp in appRole.LegalEntity.Employment)
                            {
                                if (emp.EmploymentStatus.Key == (int)EmploymentStatuses.Current && emp.EmploymentType.Key == (int)EmploymentTypes.SelfEmployed)
                                {
                                    string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                    if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                        DictList.Add(appRole.LegalEntity.Key, description);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityOriginalOrAmendedCertCopyFoundingStatement",
    "Per LegalEntityCompany in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityOriginalOrAmendedCertCopyFoundingStatement")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityOriginalOrAmendedCertCopyFoundingStatement : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        if (appRole.LegalEntity is ILegalEntityCloseCorporation)
                        {
                            string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                            if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                DictList.Add(appRole.LegalEntity.Key, description);
                        }
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityDeedTrustOrLetterAuthorityMasterSupremeCourt",
    "Per LegalEntityCompany in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityDeedTrustOrLetterAuthorityMasterSupremeCourt")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityDeedTrustOrLetterAuthorityMasterSupremeCourt : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        if (appRole.LegalEntity is ILegalEntityTrust)
                        {
                            string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                            if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                DictList.Add(appRole.LegalEntity.Key, description);
                        }
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityCertOfIncorporation",
    "Per LegalEntityCompany in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityCertOfIncorporation")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityCertOfIncorporation : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        if (appRole.LegalEntity is ILegalEntityCompany)
                        {
                            string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                            if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                DictList.Add(appRole.LegalEntity.Key, description);
                        }
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityMemoAndArticlesofAssoc",
    "Per LegalEntityCompany in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityMemoAndArticlesofAssoc")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityMemoAndArticlesofAssoc : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        if (appRole.LegalEntity is ILegalEntityCompany)
                        {
                            string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                            if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                DictList.Add(appRole.LegalEntity.Key, description);
                        }
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListCertToCommenceBusiness",
    "Per LegalEntityCompany in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListCertToCommenceBusiness")]
    [RuleInfo]
    public class DocumentCheckListCertToCommenceBusiness : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        if (appRole.LegalEntity is ILegalEntityCompany)
                        {
                            string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                            if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                DictList.Add(appRole.LegalEntity.Key, description);
                        }
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityCopyOfChangeofName",
    "Per LegalEntityCompany in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityCopyOfChangeofName")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityCopyOfChangeofName : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        (appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor))
                    {
                        if (appRole.LegalEntity is ILegalEntityCompany)
                        {
                            string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                            if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                DictList.Add(appRole.LegalEntity.Key, description);
                        }
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityITCJudgementLetterOfSettlement",
    "Per LegalEntityCompany in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityITCJudgementLetterOfSettlement")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityITCJudgementLetterOfSettlement : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                List<int> reasons = new List<int>();
                reasons.Add((int)ReasonDescriptions.ITCJudgement);
                reasons.Add((int)ReasonDescriptions.ITCDefault);
                if (DCLHelper.HasReasons(application.Key, (int)GenericKeyTypes.Offer, reasons))
                {
                    foreach (IApplicationRole appRole in application.ApplicationRoles)
                    {
                        if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active && appRole.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)OfferRoleTypeGroups.Client)
                        {
                            string ITCDescription = "BureauResponse/TUBureau:JudgementsNJ07/TUBureau:JudgementsNJ07";
                            if (DCLHelper.ITCCheckFailed(application.ReservedAccount.Key, appRole.LegalEntity.Key, ITCDescription))
                            {
                                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                    DictList.Add(appRole.LegalEntity.Key, description);
                            }
                            else
                            {
                                //we need to check old FMT 500, and new FMT 700 sections

                                //FMT 500
                                //the FMT 500 section can be removed after Empirica v4 Go Live, beciase all FMT 500 ITC's will have been archived
                                ITCDescription = "BureauResponse/TUBureau:DefaultsND07/TUBureau:DefaultsND07/TUBureau:DefaultAmount";
                                if (DCLHelper.ITCCheckFailed(application.ReservedAccount.Key, appRole.LegalEntity.Key, ITCDescription))
                                {
                                    string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                    if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                        DictList.Add(appRole.LegalEntity.Key, description);
                                }
                                else
                                {
                                    //FMT 700
                                    ITCDescription = "BureauResponse/TUBureau:DefaultsD701Part1/TUBureau:DefaultD701Part1/TUBureau:DefaultAmount";
                                    if (DCLHelper.ITCCheckFailed(application.ReservedAccount.Key, appRole.LegalEntity.Key, ITCDescription))
                                    {
                                        string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                        if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                            DictList.Add(appRole.LegalEntity.Key, description);
                                    }
                                }
                            }
                        }
                    }
                }

                // Against application only - not LE
                /*
                if (DCLHelper.HasReasons(application.Key, (int)GenericKeyTypes.Offer, reasons))
                {
                    string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, application.Key, GenericKeyTypes.Offer);
                    DictList.Add(application.Key, description);
                }*/
            }
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityITCNoticeCertificateOfRehabilitation",
    "Per LegalEntityCompany in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityITCNoticeCertificateOfRehabilitation")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityITCNoticeCertificateOfRehabilitation : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                List<int> reasons = new List<int>();
                reasons.Add((int)ReasonDescriptions.ITCNotices);
                if (DCLHelper.HasReasons(application.Key, (int)GenericKeyTypes.Offer, reasons))
                {
                    foreach (IApplicationRole appRole in application.ApplicationRoles)
                    {
                        if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active && appRole.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)OfferRoleTypeGroups.Client)
                        {
                            string ITCDescription = "BureauResponse/TUBureau:NoticesNN08/TUBureau:NoticesNN08";
                            if (DCLHelper.ITCCheckFailed(application.ReservedAccount.Key, appRole.LegalEntity.Key, ITCDescription))
                            {
                                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                    DictList.Add(appRole.LegalEntity.Key, description);
                            }
                        }
                    }
                }

                /*if (DCLHelper.HasReasons(application.Key, (int)GenericKeyTypes.Offer, reasons))
                {
                    string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, application.Key, GenericKeyTypes.Offer);
                    DictList.Add(application.Key, description);
                }*/
            }
            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredLegalEntityITCArrearsProofOfPayment",
    "Per LegalEntityCompany in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredLegalEntityITCArrearsProofOfPayment")]
    [RuleInfo]
    public class DocumentCheckListRequiredLegalEntityITCArrearsProofOfPayment : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (documentSetConfig.DocumentType.GenericKeyType != null)
            {
                foreach (IApplicationRole appRole in application.ApplicationRoles)
                {
                    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active && appRole.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)OfferRoleTypeGroups.Client)
                    {
                        string ITCPaymentProfileMsg = DCLHelper.ITCPaymentProfileTest(appRole.LegalEntity.Key, application.ReservedAccount.Key);
                        if (!string.IsNullOrEmpty(ITCPaymentProfileMsg))
                        {
                            string description = string.Format("{0} - {1} {2}", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, ITCPaymentProfileMsg);
                            if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                DictList.Add(appRole.LegalEntity.Key, description);
                        }
                    }
                }

                /*List<int> reasons = new List<int>();
                reasons.Add((int)ReasonDescriptions.ITCPaymentProfileArrears);
                if (DCLHelper.HasReasons(application.Key, (int)GenericKeyTypes.Offer, reasons))
                {
                    foreach (IApplicationRole appRole in application.ApplicationRoles)
                    {
                        if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active && appRole.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)OfferRoleTypeGroups.Client)
                        {
                            string ITCDescription = "BureauResponse/TUBureau:NoticesNN08/TUBureau:NoticesNN08";
                            if (DCLHelper.ITCCheckFailed(application.ReservedAccount.Key, appRole.LegalEntity.Key, ITCDescription))
                            {
                                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, appRole.LegalEntity.DisplayName, appRole.ApplicationRoleType.Description);
                                if (!DictList.ContainsKey(appRole.LegalEntity.Key))
                                    DictList.Add(appRole.LegalEntity.Key, description);
                            }
                        }
                    }
                }
                */
                /*if (DCLHelper.HasReasons(application.Key, (int)GenericKeyTypes.Offer, reasons))
                {
                    string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, application.Key, GenericKeyTypes.Offer);
                    DictList.Add(application.Key, description);
                }*/
            }
            return 1;
        }
    }

    #endregion GenericKeyType != Null

    #region GenericKeyType == Null

    [RuleDBTag("DocumentCheckListRequiredApplicationRatesStatement",
    "Per NaturalPersonLegalEntity if they are Self Employed and in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredApplicationRatesStatement")]
    [RuleInfo]
    public class DocumentCheckListRequiredApplicationRatesStatement : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (application.ApplicationType.Key == (int)OfferTypes.SwitchLoan
                || application.ApplicationType.Key == (int)OfferTypes.RefinanceLoan
                || application.ApplicationType.Key == (int)OfferTypes.FurtherLoan
                || application.ApplicationType.Key == (int)OfferTypes.FurtherAdvance)
            {
                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, application.Key, GenericKeyTypes.Offer);
                DictList.Add(application.Key, description);
                return 1;
            }
            else
                return 0;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredApplicationUtilityStatement",
    "Per NaturalPersonLegalEntity if they are Self Employed and in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredApplicationUtilityStatement")]
    [RuleInfo]
    public class DocumentCheckListRequiredApplicationUtilityStatement : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (application.ApplicationType.Key == (int)OfferTypes.SwitchLoan
                || application.ApplicationType.Key == (int)OfferTypes.RefinanceLoan
                || application.ApplicationType.Key == (int)OfferTypes.FurtherLoan
                || application.ApplicationType.Key == (int)OfferTypes.FurtherAdvance)
            {
                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, application.Key, GenericKeyTypes.Offer);
                DictList.Add(application.Key, description);
                return 1;
            }
            else
                return 0;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredApplicationLatestcomplete3monthbondstatements",
    "Per NaturalPersonLegalEntity if they are Self Employed and in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredApplicationLatestcomplete3monthbondstatements")]
    [RuleInfo]
    public class DocumentCheckListRequiredApplicationLatestcomplete3monthbondstatements : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (application.ApplicationType.Key == (int)OfferTypes.SwitchLoan)
            {
                ISupportsVariableLoanApplicationInformation vlai = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                IApplicationInformationVariableLoan vli = vlai.VariableLoanInformation;
                if ((vlai != null && vli != null) && vli.EmploymentType.Key == (int)EmploymentTypes.Salaried)
                {
                    string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, application.Key, GenericKeyTypes.Offer);
                    DictList.Add(application.Key, description);
                    return 1;
                }
            }

            return 0;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredApplicationLatestcomplete12monthbondstatements",
     "(ApplicationType Switch) && (Application = Self Employed) || (Investment Property) || (Deeds Interdict and Attachment) || (Deed Over Bond)",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredApplicationLatestcomplete12monthbondstatements")]
    [RuleInfo]
    public class DocumentCheckListRequiredApplicationLatestcomplete12monthbondstatements : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            // 1
            if (application.ApplicationType.Key == (int)OfferTypes.SwitchLoan)
            {
                ISupportsVariableLoanApplicationInformation vlai = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                IApplicationInformationVariableLoan vli = vlai.VariableLoanInformation;
                if ((vlai != null && vli != null) && vli.EmploymentType.Key == (int)EmploymentTypes.SelfEmployed)
                {
                    string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, application.Key, GenericKeyTypes.Offer);
                    DictList.Add(application.Key, description);
                    return 1;
                }
            }

            // 2
            IApplicationMortgageLoan appML = application as IApplicationMortgageLoan;
            if (appML != null && appML.Property != null)
            {
                if (appML.Property.OccupancyType != null && appML.Property.OccupancyType.Key == (int)OccupancyTypes.InvestmentProperty)
                {
                    string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, application.Key, GenericKeyTypes.Offer);
                    DictList.Add(application.Key, description);
                    return 1;
                }
            }

            // 3
            List<int> reasons = new List<int>();
            reasons.Add((int)ReasonDescriptions.DeedsInterdictAttachment);
            reasons.Add((int)ReasonDescriptions.DeedsOverBond);
            if (DCLHelper.HasReasons(application.Key, (int)GenericKeyTypes.Offer, reasons))
            {
                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, application.Key, GenericKeyTypes.Offer);
                DictList.Add(application.Key, description);
                return 1;
            }

            return 0;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredApplicationSaleAgreement",
    "Per NaturalPersonLegalEntity if they are Self Employed and in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredApplicationSaleAgreement")]
    [RuleInfo]
    public class DocumentCheckListRequiredApplicationSaleAgreement : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (application.ApplicationType.Key == (int)OfferTypes.NewPurchaseLoan)
            {
                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, application.Key, GenericKeyTypes.Offer);
                DictList.Add(application.Key, description);
                return 1;
            }

            return 0;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredApplicationLeaseAgreements",
    "Per NaturalPersonLegalEntity if they are Self Employed and in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredApplicationLeaseAgreements")]
    [RuleInfo]
    public class DocumentCheckListRequiredApplicationLeaseAgreements : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            IApplicationMortgageLoan appML = application as IApplicationMortgageLoan;

            if ((appML != null && appML.Property != null) &&
                (appML.Property.OccupancyType != null && appML.Property.OccupancyType.Key == (int)OccupancyTypes.InvestmentProperty))
            {
                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, application.Key, GenericKeyTypes.Offer);
                DictList.Add(application.Key, description);
                return 1;
            }
            return 0;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredApplicationScheduleOfRentals",
    "Per NaturalPersonLegalEntity if they are Self Employed and in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredApplicationScheduleOfRentals")]
    [RuleInfo]
    public class DocumentCheckListRequiredApplicationScheduleOfRentals : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            IApplicationMortgageLoan appML = application as IApplicationMortgageLoan;
            if ((appML != null && appML.Property != null) &&
                (appML.Property.OccupancyType != null && appML.Property.OccupancyType.Key == (int)OccupancyTypes.InvestmentProperty))
            {
                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, application.Key, GenericKeyTypes.Offer);
                DictList.Add(application.Key, description);
                return 1;
            }
            return 0;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredApplicationDeedsInterdictAndAttachmentProofOfRemoval",
    "Per NaturalPersonLegalEntity if they are Self Employed and in Role or OfferRole with the application as Main Applicant or Suretor",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredApplicationDeedsInterdictAndAttachmentProofOfRemoval")]
    [RuleInfo]
    public class DocumentCheckListRequiredApplicationDeedsInterdictAndAttachmentProofOfRemoval : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (application.ApplicationType.Key == (int)OfferTypes.SwitchLoan)
            {
                List<int> reasons = new List<int>();
                reasons.Add((int)ReasonDescriptions.DeedsInterdictAttachment);
                if (DCLHelper.HasReasons(application.Key, (int)GenericKeyTypes.Offer, reasons))
                {
                    string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, application.Key, GenericKeyTypes.Offer);
                    DictList.Add(application.Key, description);
                    return 1;
                }
            }
            return 0;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredApplicationDeedExtentLetterOfMotivation",
    "Per Application if there is a Deed Extent found during QA",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredApplicationDeedExtentLetterOfMotivation")]
    [RuleInfo]
    public class DocumentCheckListRequiredApplicationDeedExtentLetterOfMotivation : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            if (application.ApplicationType.Key == (int)OfferTypes.SwitchLoan)
            {
                List<int> reasons = new List<int>();
                reasons.Add((int)ReasonDescriptions.DeedsExtent);
                if (DCLHelper.HasReasons(application.Key, (int)GenericKeyTypes.Offer, reasons))
                {
                    string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, application.Key, GenericKeyTypes.Offer);
                    DictList.Add(application.Key, description);
                    return 1;
                }
            }
            return 0;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredApplicationDeedOwnershipSaleAgreements",
    "Per Application if there is a requirement during QA to validate Deed Ownership.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredApplicationDeedOwnershipSaleAgreements")]
    [RuleInfo]
    public class DocumentCheckListRequiredApplicationDeedOwnershipSaleAgreements : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            List<int> reasons = new List<int>();
            reasons.Add((int)ReasonDescriptions.DeedsOwnership);
            if (DCLHelper.HasReasons(application.Key, (int)GenericKeyTypes.Offer, reasons))
            {
                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, application.Key, GenericKeyTypes.Offer);
                DictList.Add(application.Key, description);
                return 1;
            }
            return 0;
        }
    }

    [RuleDBTag("DocumentCheckListRequiredApplicationDeedOfGrantProofOfChangeFromAttorneys",
    "Per Application if there is a requirement during QA to validate Deed Ownership.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListRequiredApplicationDeedOfGrantProofOfChangeFromAttorneys")]
    [RuleInfo]
    public class DocumentCheckListRequiredApplicationDeedOfGrantProofOfChangeFromAttorneys : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            List<int> reasons = new List<int>();
            reasons.Add((int)ReasonDescriptions.DeedsBondGrantRights);
            if (DCLHelper.HasReasons(application.Key, (int)GenericKeyTypes.Offer, reasons))
            {
                string description = string.Format("{0} - {1} ({2})", documentSetConfig.DocumentType.Description, application.Key, GenericKeyTypes.Offer);
                DictList.Add(application.Key, description);
                return 1;
            }
            return 0;
        }
    }

    #endregion GenericKeyType == Null

    #region Helper Rules

    [RuleDBTag("DocumentCheckListAddDocumentTrue",
    "Check if all the required documents are recieved for the Application",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListAddDocumentTrue")]
    [RuleInfo]
    public class DocumentCheckListAddDocumentTrue : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            return 1;
        }
    }

    [RuleDBTag("DocumentCheckListAddDocumentFalse",
    "Check if all the required documents are recieved for the Application",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListAddDocumentFalse")]
    [RuleInfo]
    public class DocumentCheckListAddDocumentFalse : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DCLHelper.CheckParameters(Parameters);
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            IApplication application = Parameters[2] as IApplication;

            return 0;
        }
    }

    [RuleDBTag("DocumentCheckListValidate",
    "Check if all the required documents are recieved for the Application",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DocumentCheckList.DocumentCheckListValidate")]
    [RuleInfo]
    public class DocumentCheckListValidate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplication _application = Parameters[0] as IApplication;
            IDocumentCheckListService dcs = ServiceFactory.GetService<IDocumentCheckListService>();
            IList<string> MissingDocuments = dcs.ValidateListWithMessages(_application.Key);

            if (MissingDocuments != null && MissingDocuments.Count > 0)
            {
                foreach (string msg in MissingDocuments)
                {
                    AddMessage(msg, msg, Messages);
                }
            }
            return 1;
        }
    }

    public class DCLHelper
    {
        public static void CheckParameters(params object[] Parameters)
        {
            IDocumentSetConfig documentSetConfig = Parameters[0] as IDocumentSetConfig;
            if (documentSetConfig == null)
                throw new ArgumentException("First parameter must be of type IDocumentSetConfig");

            Dictionary<int, string> DictList = Parameters[1] as Dictionary<int, string>;
            if (DictList == null)
                throw new ArgumentException("Second parameter must be of type Dictionary<int, string>");

            IApplication application = Parameters[2] as IApplication;
            if (application == null)
                throw new ArgumentException("Second parameter must be of type IApplication");

            return;
        }

        public static bool HasReasons(int GenericKey, int GenericKeyTypeKey, List<int> reasons)
        {
            string sql = string.Format(@"SELECT rds.*
            FROM [2am].[dbo].[Reason] r (nolock)
            inner join [2am].[dbo].[ReasonDefinition] rd (nolock)
            on r.ReasonDefinitionKey = rd.ReasonDefinitionKey
            inner join [2am].[dbo].[ReasonType] rt (nolock)
            on rd.ReasonTypeKey = rt.ReasonTypeKey
            inner join [2am].[dbo].[ReasonDescription] rds (nolock)
            on rd.ReasonDescriptionKey = rds.ReasonDescriptionKey
            where r.GenericKey = {0} and rt.GenericKeyTypeKey = {1} and rds.ReasonDescriptionKey in (:reas)", GenericKey, GenericKeyTypeKey);
            SimpleQuery<ReasonDescription_DAO> artQ = new SimpleQuery<ReasonDescription_DAO>(QueryLanguage.Sql, sql);
            artQ.SetParameterList("reas", reasons);
            artQ.AddSqlReturnDefinition(typeof(ReasonDescription_DAO), "rds");
            ReasonDescription_DAO[] artRes = artQ.Execute();
            return (artRes.Length > 0 ? true : false);
        }

        public static bool ITCCheckFailed(int AccountKey, int LegalEntityKey, string ITCDescription)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"Declare @ITCDescriptionLen int");
            sb.Append(@";WITH XMLNAMESPACES('https://secure.transunion.co.za/TUBureau' AS ");
            sb.Append(@"""TUBureau"")");
            sb.AppendLine();
            sb.AppendFormat(" SELECT top 1 len(convert(varchar(max),ResponseXML.query(N'{0}')))", ITCDescription);
            sb.Append(" FROM ITC with (nolock)");
            sb.AppendFormat(" where AccountKey = {0} and LegalEntityKey = {1}", AccountKey, LegalEntityKey);
            ParameterCollection parameters = new ParameterCollection();

            // Execute Query
            object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sb.ToString(), typeof(GeneralStatus_DAO), parameters);

            return (Convert.ToInt32(o) > 0 ? true : false);
        }

        // TODO : Need to be looked at
        public static string ITCPaymentProfileTest(int LegalEntityKey, int AccountKey)
        {
            //string Suppliers = string.Empty;
            StringBuilder Suppliers = new StringBuilder();

            string query = UIStatementRepository.GetStatement("COMMON", "ITCPaymentProfile");
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@LegalEntityKey", LegalEntityKey));
            parameters.Add(new SqlParameter("@AccountKey", AccountKey));

            DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //string supplier = reader.GetString(0);
                    string supplier = dr[0].ToString();
                    supplier = supplier.Trim();

                    //string status = reader.GetString(1);
                    string status = dr[1].ToString();
                    status = status.Trim().ToUpper();

                    for (int i = 0; i < status.Length; i++)
                    {
                        char item = status[i];
                        bool loopPass = true;
                        bool breakLoop = false;

                        switch (item)
                        {
                            // Carry on looping, means no data
                            case ('='):
                                {
                                }
                                break;

                            // If this is the latest code, we can break out of loop safely
                            case ('0'):
                            case ('C'):
                            case ('F'):
                            case ('G'):
                            case ('H'):
                            case ('K'):
                            case ('M'):
                            case ('N'):
                            case ('P'):
                            case ('R'):
                            case ('S'):
                            case ('T'):
                            case ('V'):
                            case ('Z'):
                                {
                                    breakLoop = true;
                                }
                                break;

                            // If this is the latest code, this indicates a problem
                            // Bascially if anything else recieved we can assume that there is an issue
                            case ('A'):
                            case ('B'):
                            case ('D'):
                            case ('E'):
                            case ('I'):
                            case ('J'):
                            case ('L'):
                            case ('Q'):
                            case ('W'):
                            default:
                                {
                                    breakLoop = true;
                                    loopPass = false;
                                }
                                break;
                        }

                        if (!loopPass)
                        {
                            if (string.IsNullOrEmpty(Suppliers.ToString()))
                                Suppliers.AppendFormat("({0}", supplier);
                            else
                                Suppliers.AppendFormat(" ,{0}", supplier);
                            break;
                        }
                        if (breakLoop)
                            break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(Suppliers.ToString()))
                Suppliers.Append(")");

            return Suppliers.ToString();
        }
    }

    #endregion Helper Rules
}