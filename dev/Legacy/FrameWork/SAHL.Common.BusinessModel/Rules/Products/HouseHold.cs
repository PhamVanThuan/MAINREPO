using System;
using System.Data;
using System.Data.SqlClient;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.Products
{
    /// <summary>
    /// Called by workflow
    /// </summary>
    [RuleDBTag("HouseholdIncomeContributorMinimum",
        "HouseholdIncomeContributorMinimum",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.HouseholdIncomeContributorMinimum")]
    [RuleInfo]
    public class HouseholdIncomeContributorMinimum : BusinessRuleBase
    {
        public HouseholdIncomeContributorMinimum(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The HouseholdIncomeContributorMinimum rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The HouseholdIncomeContributorMinimum rule expects the following objects to be passed: IApplication.");

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

            if (applicationMortgageLoan == null || applicationMortgageLoan.Key == 0)
                return 1;

            string sqlQuery = UIStatementRepository.GetStatement("COMMON", "IncomeContributorExistsForOffer");
            ParameterCollection prms = new ParameterCollection();

            prms.Add(new SqlParameter("@applicationKey", applicationMortgageLoan.Key));

            object o = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), prms);

            if (o == null)
            {
                string errorMessage = "There must be at least one Income Contributor on an Application before it can be submitted.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    /// <summary>
    /// Called by workflow
    /// </summary>
    [RuleDBTag("HouseholdIncomeAtLeastOne",
        "HouseholdIncomeAtLeastOne",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.HouseholdIncomeAtLeastOne")]
    [RuleInfo]
    public class HouseholdIncomeAtLeastOne : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The HouseholdIncomeAtLeastOne rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The HouseholdIncomeAtLeastOne rule expects the following objects to be passed: IApplication.");

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

            if (applicationMortgageLoan == null || applicationMortgageLoan.Key == 0)
                return 1;

            IApplication app = applicationMortgageLoan as IApplication;

            //IEventList<IApplicationRole> appRoles = applicationMortgageLoan.ApplicationRoles;

            string errorMessage = IncomeHelper.CheckIncome(app);

            if (errorMessage.Length > 0)
            {
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }
            return 0;
        }
    }

    [RuleDBTag("HouseholdIncomeContributorRules",
        "HouseholdIncomeContributorRules",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Products.HouseholdIncomeContributorRules")]
    [RuleInfo]
    public class HouseholdIncomeContributorRules : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The HouseholdIncomeContributorRules rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The HouseholdIncomeContributorRules rule expects the following objects to be passed: IApplication.");

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

            if (applicationMortgageLoan == null || applicationMortgageLoan.Key == 0)
                return 1;

            bool isContributorOnIncorrectRoleType = false;

            // All the income contributors must be Main Applicant of Suretor
            foreach (IApplicationRole applicationRole in applicationMortgageLoan.ApplicationRoles)
            {
                foreach (IApplicationRoleAttribute applicationRoleAttribute in applicationRole.ApplicationRoleAttributes)
                {
                    if (applicationRoleAttribute.OfferRoleAttributeType.Key == (int)OfferRoleAttributeTypes.IncomeContributor)
                    {
                        // Ensure that that the Income Contributor plays a Main Applicant or Suretor role.
                        if (!(applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                            || applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor))
                        {
                            isContributorOnIncorrectRoleType = true;
                            break;
                        }
                    }
                    if (isContributorOnIncorrectRoleType)
                        break;
                }
                if (isContributorOnIncorrectRoleType)
                    break;
            }

            if (isContributorOnIncorrectRoleType)
            {
                string errorMessage = "Only Legal Entities that play Main Applicant and Suretor roles may be marked as Income Contributors.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }

    public class IncomeHelper
    {
        public static string CheckIncome(IApplication app)
        {
            bool confirmedIncomeContributorExists = false;
            string errorMessage = "";

            // Go though all the roles, ensuring that there's at least one incoem contributor
            foreach (IApplicationRole applicationRole in app.ApplicationRoles)
            {
                if ((applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                    || applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor))
                {
                    foreach (IApplicationRoleAttribute applicationRoleAttribute in applicationRole.ApplicationRoleAttributes)
                    {
                        if (applicationRoleAttribute.OfferRoleAttributeType.Key == (int)OfferRoleAttributeTypes.IncomeContributor)
                        {
                            // Is the employment confirmed?
                            foreach (IEmployment employment in applicationRole.LegalEntity.Employment)
                            {
                                if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current
                                    && employment.IsConfirmed)
                                {
                                    confirmedIncomeContributorExists = true;
                                    break;
                                }
                            }
                        }
                        if (confirmedIncomeContributorExists)
                            break;
                    }

                    if (confirmedIncomeContributorExists)
                        break;
                }

                if (confirmedIncomeContributorExists)
                    break;
            }

            if (!confirmedIncomeContributorExists)
            {
                errorMessage = "There must be at least one confirmed Employment from an Income Contributor before an Application can be submitted.";

                //AddMessage(errorMessage, errorMessage, Messages);
            }
            return errorMessage;
        }
    }
}