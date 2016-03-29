using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Utils;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SAHL.Common.BusinessModel.Rules.LegalEntity
{
    #region Validations

    [RuleInfo]
    [RuleDBTag("LegalEntityNaturalPersonHouseholdContributorConfirmedIncome",
    "Each LE in an application marked as a household contributor must have confirmed income.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonHouseholdContributorConfirmedIncome")]
    public class LegalEntityNaturalPersonHouseholdContributorConfirmedIncome : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("Parameter[0] is not of type IApplication.");

            bool error = false;
            int numberOfIncomeContributors = 0;
            bool allIncomeContributionsConfirmed = false;
            bool isContributor = false;

            IApplication application = (IApplication)Parameters[0];

            IReadOnlyEventList<IApplicationRole> clientsOnApplication = application.GetApplicationRolesByGroup(OfferRoleTypeGroups.Client);

            var householdIncomeContributorsOnApplication = clientsOnApplication
                                                                .Where(client => client.ApplicationRoleAttributes
                                                                    .Any(applicationRoleAttrib => applicationRoleAttrib.OfferRoleAttributeType.Key
                                                                        == (int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor
                                                            ));

            householdIncomeContributorsOnApplication.ToList().ForEach(incomeContributor =>
            {
                isContributor = true;
                numberOfIncomeContributors += 1;
                var incomeContributions = incomeContributor.LegalEntity.Employment.Where(emp => emp.EmploymentStatus.Key == (int)EmploymentStatuses.Current).ToList();
                allIncomeContributionsConfirmed = incomeContributions.Count > 0 ? incomeContributions.TrueForAll(employment => CheckIfIncomeFromEmploymentWasConfirmed(employment)) : false;
            });

            if (isContributor && allIncomeContributionsConfirmed == false)
            {
                error = true;
            }

            if (error)
            {
                AddMessage("Each contributing legal entity must have confirmed basic income.", "Each contributing legal entity must have confirmed basic income.", Messages);
            }

            if (numberOfIncomeContributors == 0)
                AddMessage("At least one legal entity must be a household contributor.", "At least one legal entity must be a household contributor.", Messages);

            return 1;
        }

        private bool CheckIfIncomeFromEmploymentWasConfirmed(IEmployment employment)
        {
            return employment.ConfirmedBasicIncome.HasValue && employment.ConfirmedBasicIncome.Value > 0;
        }
    }

    [RuleInfo]
    [RuleDBTag("LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare",
    "Birth Date does not the match first 6 digits of the ID Number",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare", false)]
    public class LegalEntityNaturalPersonMandatoryIDNumberBirthDateCompare : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IApplicationMortgageLoan))
                throw new ArgumentException("Parameter[0] is not of type IApplicationMortgageLoan.");

            IApplicationMortgageLoan appML = (IApplicationMortgageLoan)Parameters[0];

            for (int i = 0; i < appML.ApplicationRoles.Count; i++)
            {
                if (appML.ApplicationRoles[i].LegalEntity is ILegalEntityNaturalPerson && appML.ApplicationRoles[i].ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)OfferRoleTypeGroups.Client)
                {
                    ILegalEntityNaturalPerson LENP = appML.ApplicationRoles[i].LegalEntity as ILegalEntityNaturalPerson;

                    if (LENP != null)
                    {
                        if ((LENP.CitizenType != null) && (LENP.CitizenType.Key == (int)CitizenTypes.SACitizen || LENP.CitizenType.Key == (int)CitizenTypes.SACitizenNonResident) && (LENP.IDNumber != null) && (LENP.IDNumber.Length > 0) && (LENP.DateOfBirth.HasValue))
                        {
                            string month = LENP.DateOfBirth.Value.Month.ToString();
                            if (month.Length == 1)
                                month = "0" + month;
                            string day = LENP.DateOfBirth.Value.Day.ToString();
                            if (day.Length == 1)
                                day = "0" + day;
                            string DOB = LENP.DateOfBirth.Value.Year.ToString().Substring(2, 2) + month + day;
                            if (DOB != LENP.IDNumber.Substring(0, 6))
                                AddMessage(string.Format(LENP.DisplayName + " : The birth date does not match the first 6 digits of the ID Number."), "", Messages);
                        }
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonValidateIDNumber",
    "Validates the format of the ID Number when saving a LegalEntity NaturalPerson",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonValidateIDNumber")]
    [RuleInfo]
    public class LegalEntityNaturalPersonValidateIDNumber : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
                ValidateIDNumber(Messages, leNP);
            }
            else if (role != null && role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = role.LegalEntity as ILegalEntityNaturalPerson;
                ValidateIDNumber(Messages, leNP);
            }
            return 0;
        }

        private void ValidateIDNumber(IDomainMessageCollection Messages, ILegalEntityNaturalPerson leNP)
        {
            if (!string.IsNullOrEmpty(leNP.IDNumber) && !ValidationUtils.ValidateID(leNP.IDNumber.Trim()))
            {
                string errorMessage = "Legal Entity ID Number Is Invalid";
                AddMessage(errorMessage, errorMessage, Messages);
            }
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonIsIDNumberUnique",
    "Validates that ID Number is unique when saving a LegalEntity NaturalPerson",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonIsIDNumberUnique")]
    [RuleInfo]
    public class LegalEntityNaturalPersonIsIDNumberUnique : BusinessRuleBase
    {
        public LegalEntityNaturalPersonIsIDNumberUnique(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
                IsIDNumberUnique(Messages, leNP);
            }
            else if (role != null && role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = role.LegalEntity as ILegalEntityNaturalPerson;
                IsIDNumberUnique(Messages, leNP);
            }
            return 0;
        }

        private void IsIDNumberUnique(IDomainMessageCollection Messages, ILegalEntityNaturalPerson leNP)
        {
            if (!string.IsNullOrEmpty(leNP.IDNumber))
            {
                /*LegalEntityNaturalPerson_DAO[] matches = LegalEntityNaturalPerson_DAO.FindAllByProperty("IDNumber", leNP.IDNumber);
                if (matches.Length > 0)
                {
                    foreach (LegalEntityNaturalPerson_DAO leNP_DAO in matches)
                    {
                        if (leNP_DAO.Key != leNP.Key)
                        {
                            string person = (leNP_DAO.Salutation != null ? leNP_DAO.Salutation.Description + " " : "") + leNP_DAO.FirstNames + " " + leNP_DAO.Surname;
                            string errorMessage = "A Legal Entity (" + person + ") already exists with this ID Number.";
                            AddMessage(errorMessage, errorMessage, Messages);
                            break;
                        }
                    }
                }
                */
                string sqlQuery = UIStatementRepository.GetStatement("COMMON", "LegalEntityNaturalPersonIsIDNumberUnique");
                ParameterCollection prms = new ParameterCollection();
                prms.Add(new SqlParameter("@LegalEntityKey", Convert.ToInt32(leNP.Key)));
                prms.Add(new SqlParameter("@IDNumber", Convert.ToString(leNP.IDNumber)));

                DataSet ds = castleTransactionService.ExecuteQueryOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), prms);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string person = dr[0].ToString();
                        string errorMessage = "A Legal Entity (" + person + ") already exists with this ID Number.";
                        AddMessage(errorMessage, errorMessage, Messages);
                    }
                }
            }
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonValidatePassportNumber",
    "Validates the format of the Passport Number when saving a LegalEntity NaturalPerson",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonValidatePassportNumber")]
    [RuleInfo]
    public class LegalEntityNaturalPersonValidatePassportNumber : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
                ValidatePassportNumber(Messages, leNP);
            }
            else if (role != null && role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = role.LegalEntity as ILegalEntityNaturalPerson;
                ValidatePassportNumber(Messages, leNP);
            }
            return 0;
        }

        private void ValidatePassportNumber(IDomainMessageCollection Messages, ILegalEntityNaturalPerson leNP)
        {
            if (!string.IsNullOrEmpty(leNP.PassportNumber))
            {
                if (leNP.PassportNumber.Length <= 6)
                {
                    string errorMessage = "Legal Entity Passport Number Length Must Be Greater Than 6 Characters";
                    AddMessage(errorMessage, errorMessage, Messages);
                }

                if (ValidationUtils.ValidateID(leNP.PassportNumber))
                {
                    string errorMessage = "Legal Entity Passport Number Cannot Be An ID Number For A Foreigner";
                    AddMessage(errorMessage, errorMessage, Messages);
                }
            }
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonIsPassportNumberUnique",
    "Validates that Passport Number is unique when saving a LegalEntity NaturalPerson",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonIsPassportNumberUnique")]
    [RuleInfo]
    public class LegalEntityNaturalPersonIsPassportNumberUnique : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
                IsPassportNumberUnique(Messages, leNP);
            }
            else if (role != null && role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = role.LegalEntity as ILegalEntityNaturalPerson;
                IsPassportNumberUnique(Messages, leNP);
            }
            return 0;
        }

        private void IsPassportNumberUnique(IDomainMessageCollection Messages, ILegalEntityNaturalPerson leNP)
        {
            if (!string.IsNullOrEmpty(leNP.PassportNumber))
            {
                LegalEntityNaturalPerson_DAO[] matches = LegalEntityNaturalPerson_DAO.FindAllByProperty("PassportNumber", leNP.PassportNumber);
                if (matches != null && matches.Length > 0)
                {
                    foreach (LegalEntityNaturalPerson_DAO leNP_DAO in matches)
                    {
                        // Belongs to another LE.
                        if (leNP_DAO.Key != leNP.Key)
                        {
                            string errorMessage = "The Passport Number must be unique.";
                            AddMessage(errorMessage, errorMessage, Messages);
                            break;
                        }
                    }
                }
            }
        }
    }

    [RuleInfo]
    [RuleDBTag("LegalEntityEmploymentMandatory",
    "Employment Type can not be Unknown",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityEmploymentMandatory")]
    public class LegalEntityEmploymentMandatory : BusinessRuleBase
    {
        public LegalEntityEmploymentMandatory(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IApplicationMortgageLoan))
                throw new ArgumentException("Parameter[0] is not of type IApplicationMortgageLoan.");

            IApplication app = (IApplication)Parameters[0];

            int retval = 1;
            string sqlQuery = UIStatementRepository.GetStatement("COMMON", "LegalEntityEmploymentMandatory");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@appKey", app.Key));

            DataSet ds = castleTransactionService.ExecuteQueryOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), prms);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string errorMessage = dr[0].ToString() + " : Employment Type is Unknown";
                    AddMessage(errorMessage, errorMessage, Messages);
                    retval = 0;
                }
            }
            return retval;
        }
    }

    #endregion Validations

    #region Mandatory Fields

    [RuleDBTag("LegalEntityNaturalPersonMandatorySaluation",
    "Requires that a Salutation is entered when saving a LegalEntity NaturalPerson",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonMandatorySaluation")]
    [RuleInfo]
    public class LegalEntityNaturalPersonMandatorySaluation : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
                return CheckSalutation(Messages, leNP);
            }
            else if (role != null && role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = role.LegalEntity as ILegalEntityNaturalPerson;
                return CheckSalutation(Messages, leNP);
            }
            return 1;
        }

        public int CheckSalutation(IDomainMessageCollection Messages, ILegalEntityNaturalPerson leNP)
        {
            if (leNP.Salutation == null)
            {
                string errorMessage = "Legal Entity Salutation Required";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonMandatoryInitials",
    "Requires that Initials are entered when saving a LegalEntity NaturalPerson",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonMandatoryInitials")]
    [RuleInfo]
    public class LegalEntityNaturalPersonMandatoryInitials : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
                return CheckInitials(Messages, leNP);
            }
            else if (role != null && role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = role.LegalEntity as ILegalEntityNaturalPerson;
                return CheckInitials(Messages, leNP);
            }
            return 1;
        }

        public int CheckInitials(IDomainMessageCollection Messages, ILegalEntityNaturalPerson leNP)
        {
            if (Utils.StringUtils.IsNullOrEmptyTrimmed(leNP.Initials))
            {
                string errorMessage = "Legal Entity Initials Required";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonMandatoryGender",
    "Requires that a Gender is entered when saving a LegalEntity NaturalPerson",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonMandatoryGender")]
    [RuleInfo]
    public class LegalEntityNaturalPersonMandatoryGender : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
                return CheckGender(Messages, leNP);
            }
            else if (role != null && role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = role.LegalEntity as ILegalEntityNaturalPerson;
                return CheckGender(Messages, leNP);
            }
            return 1;
        }

        private int CheckGender(IDomainMessageCollection Messages, ILegalEntityNaturalPerson leNP)
        {
            if (leNP.Gender == null)
            {
                string errorMessage = "Legal Entity Gender Required";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonMandatoryMaritalStatus",
    "Requires that a Marital Status is entered when saving a LegalEntity NaturalPerson",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonMandatoryMaritalStatus")]
    [RuleInfo]
    public class LegalEntityNaturalPersonMandatoryMaritalStatus : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
                return CheckMaritalStatus(Messages, leNP);
            }
            else if (role != null && role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = role.LegalEntity as ILegalEntityNaturalPerson;
                return CheckMaritalStatus(Messages, leNP);
            }
            return 1;
        }

        private int CheckMaritalStatus(IDomainMessageCollection Messages, ILegalEntityNaturalPerson leNP)
        {
            if (leNP.MaritalStatus == null)
            {
                string errorMessage = "Legal Entity Marital Status Required";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonMandatoryPopulationGroup",
    "Requires that a Population Group is entered when saving a LegalEntity NaturalPerson",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonMandatoryPopulationGroup")]
    [RuleInfo]
    public class LegalEntityNaturalPersonMandatoryPopulationGroup : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
                return CheckPopulationGroup(Messages, leNP);
            }
            else if (role != null && role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = role.LegalEntity as ILegalEntityNaturalPerson;
                return CheckPopulationGroup(Messages, leNP);
            }
            return 1;
        }

        private int CheckPopulationGroup(IDomainMessageCollection Messages, ILegalEntityNaturalPerson leNP)
        {
            if (leNP.PopulationGroup == null)
            {
                string errorMessage = "Legal Entity Population Group Required";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonMandatoryEducation",
    "Requires that an Education is entered when saving a LegalEntity NaturalPerson",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonMandatoryEducation")]
    [RuleInfo]
    public class LegalEntityNaturalPersonMandatoryEducation : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
                return CheckEducation(Messages, leNP);
            }
            else if (role != null && role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = role.LegalEntity as ILegalEntityNaturalPerson;
                return CheckEducation(Messages, leNP);
            }

            return 1;
        }

        private int CheckEducation(IDomainMessageCollection Messages, ILegalEntityNaturalPerson leNP)
        {
            if (leNP.Education == null)
            {
                string errorMessage = "Legal Entity Education Required";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonMandatoryCitizenType",
    "Requires that a Citizen Type is entered when saving a LegalEntity NaturalPerson",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonMandatoryCitizenType")]
    [RuleInfo]
    public class LegalEntityNaturalPersonMandatoryCitizenType : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
                return CheckCitizenType(Messages, leNP);
            }
            else if (role != null && role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = role.LegalEntity as ILegalEntityNaturalPerson;
                return CheckCitizenType(Messages, leNP);
            }
            return 1;
        }

        private int CheckCitizenType(IDomainMessageCollection Messages, ILegalEntityNaturalPerson leNP)
        {
            if (leNP.CitizenType == null)
            {
                string errorMessage = "Legal Entity Citizen Type Required";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonMandatoryIDNumber",
    "Requires that an ID Number is entered when saving a LegalEntity NaturalPerson",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonMandatoryIDNumber")]
    [RuleInfo]
    public class LegalEntityNaturalPersonMandatoryIDNumber : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
                return CheckIDNumber(Messages, leNP);
            }
            else if (role != null && role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = role.LegalEntity as ILegalEntityNaturalPerson;
                return CheckIDNumber(Messages, leNP);
            }
            return 1;
        }

        private int CheckIDNumber(IDomainMessageCollection Messages, ILegalEntityNaturalPerson leNP)
        {
            if (leNP.CitizenType != null && (leNP.CitizenType.Key == (int)CitizenTypes.SACitizen || leNP.CitizenType.Key == (int)CitizenTypes.SACitizenNonResident))
            {
                if (string.IsNullOrEmpty(leNP.IDNumber))
                {
                    string errorMessage = "Legal Entity ID Number Required For South African Citizens";
                    AddMessage(errorMessage, errorMessage, Messages);
                    return 0;
                }
            }
            return 1;
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonMandatoryPassportNumber",
    "Requires that a Passport Number is entered when saving a LegalEntity NaturalPerson",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonMandatoryPassportNumber")]
    [RuleInfo]
    public class LegalEntityNaturalPersonMandatoryPassportNumber : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
                return CheckPassportNumber(Messages, leNP);
            }
            else if (role != null && role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = role.LegalEntity as ILegalEntityNaturalPerson;
                return CheckPassportNumber(Messages, leNP);
            }
            return 1;
        }

        private int CheckPassportNumber(IDomainMessageCollection Messages, ILegalEntityNaturalPerson leNP)
        {
            if ((leNP != null && leNP.CitizenType != null) && (leNP.CitizenType.Key == (int)CitizenTypes.Foreigner
                            || leNP.CitizenType.Key == (int)CitizenTypes.NonResidentConsulate
                            || leNP.CitizenType.Key == (int)CitizenTypes.NonResidentDiplomat
                            || leNP.CitizenType.Key == (int)CitizenTypes.NonResidentHighCommissioner
                            || leNP.CitizenType.Key == (int)CitizenTypes.NonResidentRefugee
                            || leNP.CitizenType.Key == (int)CitizenTypes.NonResident
                            || leNP.CitizenType.Key == (int)CitizenTypes.NonResidentCMAResident_Citizen
                            || leNP.CitizenType.Key == (int)CitizenTypes.NonResidentContractWorker
                            ))
            {
                if (string.IsNullOrEmpty(leNP.PassportNumber))
                {
                    string errorMessage = "Legal Entity Passport Number Required";
                    AddMessage(errorMessage, errorMessage, Messages);
                    return 0;
                }
            }
            return 1;
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonMandatoryDateOfBirth",
    "Requires that a Date Of Birth is entered when saving a LegalEntity NaturalPerson",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonMandatoryDateOfBirth")]
    [RuleInfo]
    public class LegalEntityNaturalPersonMandatoryDateOfBirth : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
                return CheckDateOfBirth(Messages, leNP);
            }
            else if (role != null && role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = role.LegalEntity as ILegalEntityNaturalPerson;
                return CheckDateOfBirth(Messages, leNP);
            }
            return 1;
        }

        private int CheckDateOfBirth(IDomainMessageCollection Messages, ILegalEntityNaturalPerson leNP)
        {
            if (!leNP.DateOfBirth.HasValue || leNP.DateOfBirth == null || leNP.DateOfBirth.Value == System.DateTime.MinValue)
            {
                string errorMessage = "Legal Entity Date Of Birth Required";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonMandatoryFirstName",
    "Requires that a FirstName is entered when saving a LegalEntity NaturalPerson",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonMandatoryFirstName")]
    [RuleInfo]
    public class LegalEntityNaturalPersonMandatoryFirstName : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
                return CheckFirstName(Messages, leNP);
            }
            else if (role != null && role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = role.LegalEntity as ILegalEntityNaturalPerson;
                return CheckFirstName(Messages, leNP);
            }
            return 1;
        }

        private int CheckFirstName(IDomainMessageCollection Messages, ILegalEntityNaturalPerson leNP)
        {
            if (leNP.FirstNames == null || leNP.FirstNames.Trim().Length == 0)
            {
                string errorMessage = "First Name Required";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonMandatorySurnameName",
    "Requires that a Surname is entered when saving a LegalEntity NaturalPerson",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonMandatorySurname")]
    [RuleInfo]
    public class LegalEntityNaturalPersonMandatorySurname : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
                return CheckSurname(Messages, leNP);
            }
            else if (role != null && role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = role.LegalEntity as ILegalEntityNaturalPerson;
                return CheckSurname(Messages, leNP);
            }
            return 1;
        }

        private int CheckSurname(IDomainMessageCollection Messages, ILegalEntityNaturalPerson leNP)
        {
            if (leNP.Surname == null || leNP.Surname.Trim().Length == 0)
            {
                string errorMessage = "Surname Required";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonItsAForeigner",
    "Prevents the capture of new LegalEntities with Citizen type = Foreigner",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonItsAForeigner")]
    [RuleInfo]
    public class LegalEntityNaturalPersonItsAForeigner : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;

            if (le != null && le is ILegalEntityNaturalPerson && le.Key < 1)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;

                if (leNP.CitizenType != null && leNP.CitizenType.Key == (int)CitizenTypes.Foreigner)
                {
                    string errorMessage = "It is no longer possible to capture a Foreigner citizen type. ";
                    AddMessage(errorMessage, errorMessage, Messages);
                    return 0;
                }

                return 1;
            }

            return 1;
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonUpdateToForeigner",
    "Citizen type can not be updated to Citizen type foreigner",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonUpdateToForeigner")]
    [RuleInfo]
    public class LegalEntityNaturalPersonUpdateToForeigner : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;

            if (le != null && le.Key > 0 && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;

                SAHL.Common.BusinessModel.LegalEntity let = le as SAHL.Common.BusinessModel.LegalEntity;

                object obj = let.GetPreviousValue<ICitizenType, CitizenType_DAO>("CitizenType");

                ICitizenType ct = (ICitizenType)obj;

                //fail by default, citizen type must be there
                int ctKey = ct == null ? (int)CitizenTypes.SACitizen : ct.Key;

                if (leNP.CitizenType != null && leNP.CitizenType.Key == (int)CitizenTypes.Foreigner && ctKey != (int)CitizenTypes.Foreigner)
                {
                    string errorMessage = "It is no longer possible to update to a Foreigner citizen type.";
                    AddMessage(errorMessage, errorMessage, Messages);
                    return 0;
                }

                return 1;
            }

            return 1;
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonMandatoryHomeLanguage",
    "Requires that a Home Language is entered when saving a LegalEntity NaturalPerson",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonMandatoryHomeLanguage")]
    [RuleInfo]
    public class LegalEntityNaturalPersonMandatoryHomeLanguage : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
                CheckHomeLanguage(Messages, leNP);
            }
            else if (role != null && role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = role.LegalEntity as ILegalEntityNaturalPerson;
                CheckHomeLanguage(Messages, leNP);
            }
            return 0;
        }

        private void CheckHomeLanguage(IDomainMessageCollection Messages, ILegalEntityNaturalPerson leNP)
        {
            if (leNP.HomeLanguage == null)
            {
                string errorMessage = "Legal Entity Home Language Required";
                AddMessage(errorMessage, errorMessage, Messages);
            }
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonMandatoryDocumentLanguage",
    "Requires that a Document Language is entered when saving a LegalEntity NaturalPerson",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonMandatoryDocumentLanguage")]
    [RuleInfo]
    public class LegalEntityNaturalPersonMandatoryDocumentLanguage : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
                CheckDocumentLanguage(Messages, leNP);
            }
            else if (role != null && role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = role.LegalEntity as ILegalEntityNaturalPerson;
                CheckDocumentLanguage(Messages, leNP);
            }
            return 0;
        }

        private void CheckDocumentLanguage(IDomainMessageCollection Messages, ILegalEntityNaturalPerson leNP)
        {
            if (leNP.DocumentLanguage == null)
            {
                string errorMessage = "Legal Entity Document Language Required";
                AddMessage(errorMessage, errorMessage, Messages);
            }
        }
    }

    [RuleDBTag("LegalEntityNaturalPersonMandatoryLegalEntityStatus",
    "Requires that a Legal Entity Status is entered when saving a LegalEntity NaturalPerson",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonMandatoryLegalEntityStatus")]
    [RuleInfo]
    public class LegalEntityNaturalPersonMandatoryLegalEntityStatus : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity le = Parameters[0] as ILegalEntity;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
                CheckLegalEntityStatus(Messages, leNP);
            }
            else if (role != null && role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson leNP = role.LegalEntity as ILegalEntityNaturalPerson;
                CheckLegalEntityStatus(Messages, leNP);
            }
            return 0;
        }

        private void CheckLegalEntityStatus(IDomainMessageCollection Messages, ILegalEntityNaturalPerson leNP)
        {
            if (leNP.LegalEntityStatus == null)
            {
                string errorMessage = "Legal Entity Status Required";
                AddMessage(errorMessage, errorMessage, Messages);
            }
        }
    }

    #endregion Mandatory Fields

    #region LegalEntityNaturalPersonUpdateProfileContactDetails

    [RuleDBTag("LegalEntityNaturalPersonUpdateProfileContactDetails",
    "The following fields need to be validated: Work Phone Code and Work Phone Number, Cellphone no., Email Address ",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonUpdateProfileContactDetails")]
    [RuleInfo]
    public class LegalEntityNaturalPersonUpdateProfileContactDetails : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The LegalEntityNaturalPersonUpdateProfileContactDetails rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ILegalEntity))
                throw new ArgumentException("The LegalEntityNaturalPersonUpdateProfileContactDetails rule expects the following objects to be passed: ILegalEntity.");

            #endregion Check for allowed object type(s)

            ILegalEntity le = Parameters[0] as ILegalEntity;

            ILegalEntityNaturalPerson leNP = null;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                leNP = le as ILegalEntityNaturalPerson;
            }

            if (leNP == null)
                return 0;

            ArrayList Items = new ArrayList();

            if (string.IsNullOrEmpty(leNP.WorkPhoneCode))
                Items.Add("Work Phone Code");

            if (string.IsNullOrEmpty(leNP.WorkPhoneNumber))
                Items.Add("Work Phone Number");

            if (string.IsNullOrEmpty(leNP.CellPhoneNumber))
                Items.Add("Cellphone Number");

            if (string.IsNullOrEmpty(leNP.EmailAddress))
                Items.Add("Email Address");

            CommaDelimitedStringCollection commaStr = new CommaDelimitedStringCollection();
            foreach (string item in Items)
                commaStr.Add(item);

            if (!string.IsNullOrEmpty(commaStr.ToString()))
            {
                string errorMessage = String.Format("The following fields are required: {0}", commaStr.ToString());
                AddMessage(errorMessage, errorMessage, Messages);
                return 1;
            }
            return 0;
        }
    }

    #endregion LegalEntityNaturalPersonUpdateProfileContactDetails

    #region LegalEntityNaturalPersonEmailRequired

    [RuleDBTag("LegalEntityNaturalPersonEmailRequired",
    "Email Address is required.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonEmailRequired")]
    [RuleInfo]
    public class LegalEntityNaturalPersonEmailRequired : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The LegalEntityNaturalPersonEmailRequired rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ILegalEntity))
                throw new ArgumentException("The LegalEntityNaturalPersonEmailRequired rule expects the following objects to be passed: ILegalEntity.");

            #endregion Check for allowed object type(s)

            ILegalEntity le = Parameters[0] as ILegalEntity;

            ILegalEntityNaturalPerson leNP = null;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                leNP = le as ILegalEntityNaturalPerson;
            }

            if (leNP == null)
                return 0;

            if (string.IsNullOrEmpty(leNP.EmailAddress))
            {
                string errorMessage = String.Format("Email Address is required.");
                AddMessage(errorMessage, errorMessage, Messages);
                return 1;
            }
            return 0;
        }
    }

    #endregion LegalEntityNaturalPersonEmailRequired

    #region LegalEntityNaturalPersonUpdateProfilePreferedName

    [RuleDBTag("LegalEntityNaturalPersonUpdateProfilePreferedName",
    "The following fields need to be validated: PreferedName",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonUpdateProfilePreferedName")]
    [RuleInfo]
    public class LegalEntityNaturalPersonUpdateProfilePreferedName : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The LegalEntityNaturalPersonUpdateProfilePreferedName rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ILegalEntity))
                throw new ArgumentException("The LegalEntityNaturalPersonUpdateProfilePreferedName rule expects the following objects to be passed: ILegalEntity.");

            #endregion Check for allowed object type(s)

            ILegalEntity le = Parameters[0] as ILegalEntity;

            IOrganisationStructureRepository osr = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IADUser aduser = osr.GetAdUserByLegalEntityKey(le.Key);
            if (aduser == null)
                return 1;

            if (aduser.GeneralStatusKey.Key == (int)GeneralStatuses.Inactive)
                return 1;

            ILegalEntityNaturalPerson leNP = null;

            if (le != null && le is ILegalEntityNaturalPerson)
            {
                leNP = le as ILegalEntityNaturalPerson;
            }

            if (leNP == null)
                return 1;

            ArrayList Items = new ArrayList();

            string preferredName = leNP.PreferredName;

            if (!string.IsNullOrEmpty(preferredName))
                preferredName = preferredName.Trim();

            if (string.IsNullOrEmpty(preferredName))
            {
                Items.Add("Preferred Name");
            }
            CommaDelimitedStringCollection commaStr = new CommaDelimitedStringCollection();
            foreach (string item in Items)
                commaStr.Add(item);

            if (!string.IsNullOrEmpty(commaStr.ToString()))
            {
                string errorMessage = String.Format("The following fields are required: {0}", commaStr.ToString());
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }
            return 1;
        }
    }

    #endregion LegalEntityNaturalPersonUpdateProfilePreferedName
}