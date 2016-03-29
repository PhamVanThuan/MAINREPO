using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

using SAHL.Common.Globals;
using SAHL.Common;


namespace SAHL.Common.BusinessModel.Rules.Employment
{

    [RuleDBTag("EmploymentStatusAddCurrent",
       "Ensures the status is current when adding a new employment record",
       "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.EmploymentStatusAddCurrent")]
    [RuleInfo]
    public class EmploymentStatusAddCurrent : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = (IEmployment)Parameters[0];

            if (employment.Key == 0 && (employment.EmploymentStatus == null || employment.EmploymentStatus.Key != (int)EmploymentStatuses.Current))
            {
                AddMessage("When adding new employment details, employment status must be Current", "", Messages);
            }

            return 1;
        }
    }

    [RuleDBTag("EmploymentMonthlyIncomeMinimum",
      "Basic income must be greater than R0.00 for employment types other than Unemployed",
        "SAHL.Rules.DLL",
     "SAHL.Common.BusinessModel.Rules.Employment.EmploymentMonthlyIncomeMinimum")]
    [RuleParameterTag(new string[] { "@MinValue,0,9" })]
    [RuleInfo]
    public class EmploymentMonthlyIncomeMinimum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = (IEmployment)Parameters[0];

            // if unemployed or the employment status is previous, exit as the rule doesn't apply
            if (employment is IEmploymentUnemployed) 
                return 1;

            // if the status is previous, exit
            if (employment.EmploymentStatus != null && employment.EmploymentStatus.Key == (int)EmploymentStatuses.Previous)
                return 1;

            // a minimum value must be captured, and must be greater than 0
            int minimumValue = Convert.ToInt32(RuleItem.RuleParameters[0].Value);

            if (employment.MonthlyIncome <= minimumValue)
            {
                AddMessage("Basic Income Amount must be greater than " + minimumValue.ToString(Constants.CurrencyFormat), "", Messages);
                return 0;
            }

            return 1;

        }
    }

    [RuleDBTag("EmploymentConfirmedIncomeMinimum",
       "Confirmed income must be greater than R0.00 for employment types other than Unemployed",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Employment.EmploymentConfirmedIncomeMinimum")]
    [RuleParameterTag(new string[] { "@MinValue,0,9" })]
    [RuleInfo]
    public class EmploymentConfirmedIncomeMinimum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = (IEmployment)Parameters[0];

            // if unemployed or the employment hasn't been confirmed, exit 
            if (employment is IEmploymentUnemployed || !employment.IsConfirmed)
                return 1;

            // if employment status is previous, exit
            if (employment.EmploymentStatus != null && employment.EmploymentStatus.Key == (int)EmploymentStatuses.Previous )
                return 1;

            // a minimum value must be captured, and must be greater than 0
            int minimumValue = Convert.ToInt32(RuleItem.RuleParameters[0].Value);

            if (employment.ConfirmedIncome <= minimumValue)
            {
                string message = "Confirmed Income Amount must be greater than " + minimumValue.ToString(Constants.CurrencyFormat);
                AddMessage(message, "", Messages);
                return 0;
            }

            return 1;

        }
    }

    [RuleDBTag("EmploymentEmployerMandatory",
      "Employer is mandatory if remuneration type is not Rental Income, Investment Income, Pension, Maintenance, Unemployed",
        "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.EmploymentEmployerMandatory")]
    [RuleInfo]
    public class EmploymentEmployerMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = (IEmployment)Parameters[0];

            // if unemployed then exit
            if (employment is IEmploymentUnemployed)
                return 1;

            // if the employment status is previous, we must ignore this rule (old records don't have a remuneration type
            // and they are allowed to set those to previous)
            if (employment.EmploymentStatus != null && employment.EmploymentStatus.Key == (int)EmploymentStatuses.Previous)
                return 1;

            if (employment.RemunerationType != null)
            {
                switch ((RemunerationTypes)employment.RemunerationType.Key)
                {
                    case RemunerationTypes.RentalIncome:
                    case RemunerationTypes.InvestmentIncome:
                    case RemunerationTypes.Pension:
                    case RemunerationTypes.Maintenance:
                    case RemunerationTypes.Unknown:
                        break;
                    default:
                        if (employment.Employer == null)
                            AddMessage("Employer is mandatory for the selected remuneration type", "", Messages);
                        break;
                }
            }

            return 1;
        }
    }

    [RuleDBTag("EmploymentSalaryPayDayMandatory",
  "Salary Pay Day is mandatory ",
    "SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.Employment.EmploymentSalaryPayDayMandatory")]
    [RuleInfo]
    public class EmploymentSalaryPayDayMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = (IEmployment)Parameters[0];

            // if unemployed then exit
            if (employment is IEmploymentUnemployed)
                return 1;

            if (employment.SalaryPaymentDay == null)
            {
                AddMessage("Please capture salary payment day", "", Messages);
            }

            return 1;
        }
    }


   // [RuleDBTag("EmploymentRemunerationTypeMandatory",
   //   "Remuneration type is mandatory",
   //     "SAHL.Rules.DLL",
   //"SAHL.Common.BusinessModel.Rules.Employment.EmploymentRemunerationTypeMandatory")]
   // [RuleInfo]
   // public class EmploymentRemunerationTypeMandatory : BusinessRuleBase
   // {
   //     public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
   //     {
   //         IEmployment employment = (IEmployment)Parameters[0];

   //         // if the employment status is previous, we must ignore this rule (old records don't have a remuneration type
   //         // and they are allowed to set those to previous)
   //         if (employment.EmploymentStatus != null && employment.EmploymentStatus.Key == (int)EmploymentStatuses.Previous)
   //             return 1;

   //         if (employment.RemunerationType == null)
   //         {
   //             AddMessage("Remuneration type is mandatory", "", Messages);
   //             return 0;
   //         }

   //         return 1;
   //     }
   // }

    [RuleDBTag("EmploymentStartDateMinimum",
    "The employment start date must be after 01/01/1900.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.EmploymentStartDateMinimum")]
    [RuleParameterTag(new string[] { "@MinStartDate,01/01/1900,5" })]
    [RuleInfo]
    public class EmploymentStartDateMinimum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment empl = (IEmployment)Parameters[0];

            // if employment status is previous, exit
            if (empl.EmploymentStatus != null && empl.EmploymentStatus.Key == (int)EmploymentStatuses.Previous)
                return 1;

            // if there is no value, skip out as the mandatory rule will be raised
            if (!empl.EmploymentStartDate.HasValue)
                return 1;

            // a minimum value must be captured, and must be greater than 0
            DateTime minimumStartDate = Convert.ToDateTime(RuleItem.RuleParameters[0].Value);

            if (!empl.EmploymentStartDate.HasValue || empl.EmploymentStartDate <= minimumStartDate)
            {
                AddMessage("Employment start date must be after " + minimumStartDate.ToString(Constants.DateFormat), "", Messages);
            }

            return 1;
        }
    }

    [RuleDBTag("EmploymentStartDateMaximum",
    "The employment start date must be before today's date.",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Employment.EmploymentStartDateMaximum")]
    [RuleInfo]
    public class EmploymentStartDateMaximum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment empl = (IEmployment)Parameters[0];

            // if employment status is previous, exit
            if (empl.EmploymentStatus != null && empl.EmploymentStatus.Key == (int)EmploymentStatuses.Previous)
                return 1;

            // if there is no value, skip out as the mandatory rule will be raised
            if (!empl.EmploymentStartDate.HasValue)
                return 1;

            // a minimum value must be captured, and must be greater than 0
            if (empl.EmploymentStartDate >= DateTime.Today)
            {
                AddMessage("Employment start date must be before today", "", Messages);
            }

            return 1;
        }
    }

    [RuleDBTag("EmploymentLegalEntityCompanyRemunerationTypes",
        "Company, Close Corporations and Trusts must have a remuneration type of Business Profits",
       "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.Employment.EmploymentLegalEntityCompanyRemunerationTypes")]
    [RuleInfo]
    public class EmploymentLegalEntityCompanyRemunerationTypes : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment empl = (IEmployment)Parameters[0];

            // if we're setting the employment record to previous, then we can ignore this rule
            if (empl.EmploymentStatus != null && empl.EmploymentStatus.Key == (int)EmploymentStatuses.Previous)
                return 1;

            if (empl.LegalEntity != null &&
                empl.RemunerationType != null &&
                (empl.LegalEntity is ILegalEntityCloseCorporation || empl.LegalEntity is ILegalEntityCompany || empl.LegalEntity is ILegalEntityTrust) &&
                empl.RemunerationType.Key != (int)RemunerationTypes.BusinessProfits
                )
            {
                AddMessage("Remuneration type must be Business Profits for Close Corporations, Companies, and Trusts", "", Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("EmploymentSupportedRemunerationTypes",
        "Ensures that only supported remuneration types can be applied",
       "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.Employment.EmploymentSupportedRemunerationTypes")]
    [RuleInfo]
    public class EmploymentSupportedRemunerationTypes : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment empl = (IEmployment)Parameters[0];

            // we can ignore this rule for existing employment records or if the remuneration type has not been set
            if (empl.Key > 0 || empl.RemunerationType == null)
                return 1;

            // next check that the remuneration type we're receiving is supported
            if (!empl.SupportedRemunerationTypes.Contains((RemunerationTypes)empl.RemunerationType.Key))
            {
                string msg = String.Format("The employment type {0} does not support remuneration of type {1}", empl.EmploymentType.Description, empl.RemunerationType.Description);
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("EmploymentEndDateMinimum",
    "The employment end date must be after 01/01/1900, and after the start date.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.EmploymentEndDateMinimum")]
    [RuleParameterTag(new string[] { "@MinStartDate,01/01/1900,5" })]
    [RuleInfo]
    public class EmploymentEndDateMinimum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment empl = (IEmployment)Parameters[0];

            // a minimum value must be captured, and must be greater than 0
            DateTime minimumEndDate = Convert.ToDateTime(RuleItem.RuleParameters[0].Value);

            if (!empl.EmploymentEndDate.HasValue || !empl.EmploymentStartDate.HasValue)
                return 1;

            if (empl.EmploymentEndDate.Value <= minimumEndDate || empl.EmploymentEndDate < empl.EmploymentStartDate)
            {
                AddMessage("Employment end date must be after " + minimumEndDate.ToString(Constants.DateFormat) + " and after the start date.", "", Messages);
            }

            return 1;
        }
    }

    [RuleDBTag("EmploymentEndDateMaximum",
    "The employment end date must on or before today's date if it has been set.",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Employment.EmploymentEndDateMaximum")]
    [RuleParameterTag(new string[] { "@MinStartDate,01/01/1900,5" })]
    [RuleInfo]
    public class EmploymentEndDateMaximum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment empl = (IEmployment)Parameters[0];

            if (!empl.EmploymentEndDate.HasValue)
                return 1;

            if (empl.EmploymentEndDate.Value > DateTime.Now)
            {
                AddMessage("Employment end date must be before today", "", Messages);
            }

            return 1;
        }
    }

    [RuleDBTag("EmploymentPreviousStatusEndDateMandatory",
    "The employment end date must be set if the status is set to Previous.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.EmploymentPreviousStatusEndDateMandatory")]
    [RuleInfo]
    public class EmploymentPreviousStatusEndDateMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment empl = (IEmployment)Parameters[0];

            if (empl.EmploymentStatus == null)
                return 1;

            if (empl.EmploymentStatus.Key == (int)EmploymentStatuses.Previous && !empl.EmploymentEndDate.HasValue)
            {
                AddMessage("Employment end date must be entered if the status is set to Previous", "", Messages);
            }

            return 1;
        }
    }

    [RuleDBTag("EmploymentCurrentStatusEndDateInvalid",
    "The employment end date cannot be set if the status is set to Current.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.EmploymentCurrentStatusEndDateInvalid")]
    [RuleInfo]
    public class EmploymentCurrentStatusEndDateInvalid : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment empl = (IEmployment)Parameters[0];

            if (empl.EmploymentStatus == null)
                return 1;

            if (empl.EmploymentStatus.Key == (int)EmploymentStatuses.Current && empl.EmploymentEndDate.HasValue)
            {
                AddMessage("Employment end date cannot be entered if the status is set to Current", "", Messages);
            }

            return 1;
        }
    }

    [RuleDBTag("EmploymentPreviousPTICheck",
    "Cannot set an income to previous if is the only active employment against any associated LE account.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.EmploymentPreviousPTICheck")]
    [RuleInfo]
    public class EmploymentPreviousPTICheck : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment empl = (IEmployment)Parameters[0];


            // if the status is not being set to previous, exit
            if (empl.EmploymentStatus == null || empl.EmploymentStatus.Key != (int)EmploymentStatuses.Previous)
                return 1;

            // make sure the legal entity has been set
            if (empl.LegalEntity == null)
                return 1;

            // check against the employment repository
            IEmploymentRepository repo = RepositoryFactory.GetRepository<IEmploymentRepository>();
            IList<IAccount> affectedAccounts = repo.GetAccountsForPTI(empl);
            if (affectedAccounts.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (IAccount account in affectedAccounts)
                {
                    if (sb.Length > 0)
                        sb.Append(", ");
                    sb.Append(account.Key.ToString());
                }
                sb.Insert(0, "Cannot set employment to previous as it is the only active employment against the following accounts: ");

                string msg = sb.ToString();
                AddMessage(msg, msg, Messages);
                return 0;
            }

            //// look at other employment records - if we find other active items we can exit as the rule doesn't apply
            //foreach (IEmployment employment in empl.LegalEntity.Employment)
            //{
            //    // exclude the executing object
            //    if (empl.Key == employment.Key)
            //        continue;

            //    if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current)
            //        return 1;
            //}

            //// now we loop through the roles and make sure there are no open accounts - if there are we are not allowed to
            //// set the status to previous
            //foreach (IRole role in empl.LegalEntity.Roles)
            //{
            //    if (role.Account.AccountStatus.Key == (int)AccountStatuses.Open)
            //    {
            //        AddMessage("Cannot set employment to previous when it is the only active employment against any account", "", Messages);
            //        return 0;
            //    }
            //}

            return 1;
        }
    }

    [RuleDBTag("EmploymentMandatoryConfirmedBy",
          "When the employment record is confirmed, the user id of the person who confirmed it must be set.",
           "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Employment.EmploymentMandatoryConfirmedBy")]
    [RuleInfo]
    public class EmploymentMandatoryConfirmedBy : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = (IEmployment)Parameters[0];

            // if the status is being set to previous, exit
            if (employment.EmploymentStatus != null && employment.EmploymentStatus.Key == (int)EmploymentStatuses.Previous)
                return 1;

            if (employment.IsConfirmed && String.IsNullOrEmpty(employment.ConfirmedBy))
            {
                AddMessage("The name of the person confirming income must be set.", "", Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("EmploymentMandatoryConfirmedDate",
      "When the employment record is confirmed, the date of the confirmation must be set.",
       "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Employment.EmploymentMandatoryConfirmedDate")]
    [RuleInfo]
    public class EmploymentMandatoryConfirmedDate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = (IEmployment)Parameters[0];

            // if the status is being set to previous, exit
            if (employment.EmploymentStatus != null && employment.EmploymentStatus.Key == (int)EmploymentStatuses.Previous)
                return 1;

            if (employment.IsConfirmed && !employment.ConfirmedDate.HasValue)
            {
                AddMessage("The date of income confirmation income must be set.", "", Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("EmploymentRemunerationCommissionMandatory",
      "When the remuneration type is BasicAndCommission, the extended employment information must include a value for commission.",
      "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.EmploymentRemunerationCommissionMandatory")]
    [RuleInfo]
    public class EmploymentRemunerationCommissionMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = (IEmployment)Parameters[0];

            // if remuneration type not set, exit
            if (employment == null || employment.RemunerationType == null)
                return 1;

            // if the status is being set to previous, exit
            if (employment.EmploymentStatus != null && employment.EmploymentStatus.Key == (int)EmploymentStatuses.Previous)
                return 1;

            if (employment.RemunerationType.Key == (int)RemunerationTypes.BasicAndCommission)
            {
                double? commission = employment.ExtendedEmployment == null ? 0D : employment.ExtendedEmployment.Commission;

                if (!commission.HasValue || commission.Value <= 0D)
                {
                    string msg = String.Format("Commission is mandatory when the remuneration type is {0}", employment.RemunerationType.Description);
                    AddMessage(msg, msg, Messages);
                    return 0;
                }
            }
            return 1;
        }
    }

    [RuleDBTag("EmploymentPreviousConfirmedIncomeCannotChange",
    "Check on setting Employment to Previous or changing ConfirmedIncome",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.EmploymentPreviousConfirmedIncomeCannotChange")]
    [RuleInfo]
    public class EmploymentPreviousConfirmedIncomeCannotChange : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = Parameters[0] as IEmployment;
            int origEmploymentStatusKey = Convert.ToInt32(Parameters[1]);
            double origConfirmedIncome = Convert.ToDouble(Parameters[2]);

            if ((employment.EmploymentStatus.Key == (int)SAHL.Common.Globals.EmploymentStatuses.Previous && 
                origEmploymentStatusKey != (int)SAHL.Common.Globals.EmploymentStatuses.Previous) &&
                (origConfirmedIncome != employment.ConfirmedIncome))
            {
                string msg = "Cannot change the Confirmed Income value when setting Employment Status to previous.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("EmploymentConfirmedSetYesSave", 
    "If Employment is confirmed the Confirmed Income cannot be null or zero",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.EmploymentConfirmedSetYesSave")]
    [RuleInfo]
    public class EmploymentConfirmedSetYesSave : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = Parameters[0] as IEmployment;

            if (employment.ConfirmedIncomeFlag.HasValue &&
                Convert.ToInt32(employment.ConfirmedIncomeFlag.Value) == (int)SAHL.Common.Globals.ConfirmedIncome.Yes &&
                employment.ConfirmedIncome <= 0D)
            {
                string msg = "Please capture a value for Confirmed Income when setting Confirmed Income to Yes.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("EmploymentConfirmedSetBackToNo",
    "If Employment is set from Yes to No and Confirmed Income is capture then raise error.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.EmploymentConfirmedSetBackToNo")]
    [RuleInfo]
    public class EmploymentConfirmedSetBackToNo : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = Parameters[0] as IEmployment;
            int origConfirmedEmploymentFlag = Convert.ToInt32(Parameters[1]);

            if (origConfirmedEmploymentFlag == (int)SAHL.Common.Globals.ConfirmedEmployment.Yes &&
                (!employment.ConfirmedEmploymentFlag.HasValue || 
                Convert.ToInt32(employment.ConfirmedEmploymentFlag.Value) == (int)SAHL.Common.Globals.ConfirmedEmployment.No) &&
                (employment.ConfirmedIncome > 0))
            {
                string msg = "Cannot set Employment Confirmed to No if the Confirmed Income value has been captured.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("EmploymentConfirmedIncomeMandatory",
    "Confimed Income must have a value if Confirmed Employment is set.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.EmploymentConfirmedIncomeMandatory")]
    [RuleInfo]
    public class EmploymentConfirmedIncomeMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = Parameters[0] as IEmployment;

            if (employment.ConfirmedEmploymentFlag.HasValue && !employment.ConfirmedIncomeFlag.HasValue)
            {
                string msg = "A Confirmed Income value must be selected when setting the Confirmed Employment value.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("EmploymentSubsidisedSetEmploymentToPrevious",
    @"If the employment is a Subsidised Employment and the Employment Status has been changed from Current to Previous
    and the Subsidy Status is Active we must fire a warning message to say the the Subsidy will be also be made Inactive.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.EmploymentSubsidisedSetEmploymentToPrevious")]
    [RuleInfo]
    public class EmploymentSubsidisedSetEmploymentToPrevious : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = Parameters[0] as IEmployment;
            int origEmploymentStatusKey = Convert.ToInt32(Parameters[1]);

            if (employment is IEmploymentSubsidised
                && origEmploymentStatusKey == (int)SAHL.Common.Globals.EmploymentStatuses.Current
                && employment.EmploymentStatus.Key == (int)SAHL.Common.Globals.EmploymentStatuses.Previous)
            {
                string msg = "Setting this employment record to 'previous' will change the connected subsidy to INACTIVE.";
                AddMessage(msg, msg, Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("EmploymentContactPersonMandatory",
    "Contact Person must be captured if Confirmed Income is greater than zero & Employment Status is set as Current.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.EmploymentContactPersonMandatory")]
    [RuleInfo]
    public class EmploymentContactPersonMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = Parameters[0] as IEmployment;

            //if (employment.ConfirmedBasicIncome.HasValue && 
            //    employment.ConfirmedBasicIncome.Value > 0.0 &&
            //    employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current &&
            //    string.IsNullOrEmpty(employment.ContactPerson.Trim()))
            if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current 
                && string.IsNullOrEmpty(employment.ContactPerson.Trim()))
            {
                string msg = "Please capture a contact person.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("EmploymentContactPhoneMandatory",
    "Contact Phone Code/Number must be captured if Confirmed Income is greater than zero & Employment Status is set as Current.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.EmploymentContactPhoneMandatory")]
    [RuleInfo]
    public class EmploymentContactPhoneMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = Parameters[0] as IEmployment;

            //if (employment.ConfirmedBasicIncome.HasValue &&
            //    employment.ConfirmedBasicIncome.Value > 0.0 &&
            //    employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current &&
            //    (string.IsNullOrEmpty(employment.ContactPhoneCode.Trim()) || string.IsNullOrEmpty(employment.ContactPhoneNumber.Trim())))
            if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current &&
                (string.IsNullOrEmpty(employment.ContactPhoneCode.Trim()) || string.IsNullOrEmpty(employment.ContactPhoneNumber.Trim())))
            {
                string msg = "Please capture a phone number.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("EmploymentDepartmentMandatory",
    "Contact Person must be captured if Confirmed Income is greater than zero & Employment Status is set as Current.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.EmploymentDepartmentMandatory")]
    [RuleInfo]
    public class EmploymentDepartmentMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = Parameters[0] as IEmployment;

            //if (employment.ConfirmedBasicIncome.HasValue &&
            //    employment.ConfirmedBasicIncome.Value > 0.0 &&
            //    employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current &&
            //    string.IsNullOrEmpty(employment.Department.Trim()))
            if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current && 
                string.IsNullOrEmpty(employment.Department.Trim()))
            {
                string msg = "Please capture a department.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("EmploymentConfirmationSourceMandatory",
    "ConfirmationSource must be selected if Confirmed Income is greater than zero & Employment Status is set as Current.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.EmploymentConfirmationSourceMandatory")]
    [RuleInfo]
    public class EmploymentConfirmationSourceMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = Parameters[0] as IEmployment;

            //if (employment.ConfirmedBasicIncome.HasValue &&
            //    employment.ConfirmedBasicIncome.Value > 0.0 &&
            //    employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current &&
            //    employment.EmploymentConfirmationSource == null)
            if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current &&
                employment.EmploymentConfirmationSource == null)
            {
                string msg = "Please select a Confirmation Source.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("EmploymentVerificationProcessMinimum",
    "At least one Verification Process must be selected if Employment has been confirmed.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.EmploymentVerificationProcessMinimum")]
    [RuleInfo]
    public class EmploymentVerificationProcessMinimum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = Parameters[0] as IEmployment;
            List<int> vpList = Parameters[1] as List<int>;

            //if (employment.Key == 0 || employment.ConfirmedEmploymentFlag == null)
            //    return 1;
            //else if ((employment.ConfirmedEmploymentFlag.HasValue &&
            //    Convert.ToInt32(employment.ConfirmedEmploymentFlag.Value) == (int)SAHL.Common.Globals.ConfirmedEmployment.No))
            //    return 1;
            //else if ((employment.ConfirmedEmploymentFlag.HasValue &&
            //    Convert.ToInt32(employment.ConfirmedEmploymentFlag.Value) == (int)SAHL.Common.Globals.ConfirmedEmployment.Yes) &&
            //    (vpList != null && vpList.Count > 0))
            //    return 1;
            //else
            if (vpList != null && vpList.Count > 0)
                return 1;

                string msg = "Please select at least one of the verification process options.";
                AddMessage(msg, msg, Messages);
                return 0;
            }
        }

    [RuleDBTag("ExistingConfirmedEmployment",
    "Check for existing confirmed employment.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.ExistingConfirmedEmployment")]
    [RuleInfo]
    public class ExistingConfirmedEmployment : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = Parameters[0] as IEmployment;

            if (employment.Key != 0 && employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current && employment.ConfirmedIncome > 0D &&
               (!employment.ConfirmedIncomeFlag.HasValue || Convert.ToInt32(employment.ConfirmedIncomeFlag.Value) == (int)ConfirmedIncome.No))
            {
                string msg = "Please set the 'Confirmed Income' flag as you are resaving an already Confirmed Income.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("EmploymentPreviousValuesCannotChange",
    "Warn users values cannot be changed.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Employment.EmploymentPreviousValuesCannotChange")]
    [RuleInfo]
    public class EmploymentPreviousValuesCannotChange : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IEmployment employment = Parameters[0] as IEmployment;

            if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Previous)
            {
                string msg = "When setting an employment to previous, income values and confirmation details cannot be changed.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }



	[RuleDBTag("EmploymentSubsidyLinkToMultipleOpenAccounts",
	"Check if a subsidy is associated to an account.",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.Employment.EmploymentSubsidyLinkToMultipleOpenAccounts")]
	[RuleInfo]
	public class EmploymentSubsidyLinkToMultipleOpenAccounts : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			ISubsidy subsidy = Parameters[0] as ISubsidy;
			IAccount acc = Parameters[1] as IAccount;

			IEmploymentRepository emprepo = RepositoryFactory.GetRepository<IEmploymentRepository>();
			
            if (subsidy == null)
				throw new ArgumentException("The EmploymentSubsidyLinkToMultipleOpenAccounts rule expects an ISubsidy type");

			if (acc == null)
				throw new ArgumentException("The EmploymentSubsidyLinkToMultipleOpenAccounts rule expects an IAccount type");
			

			if ((int)subsidy.Account.Key != (int)acc.Key)
			{
				string msg = "The subsidy is currently associated to another open account.";
				AddMessage(msg, msg, Messages);
				return 0;
			}

			return 1;
		}
	}




}