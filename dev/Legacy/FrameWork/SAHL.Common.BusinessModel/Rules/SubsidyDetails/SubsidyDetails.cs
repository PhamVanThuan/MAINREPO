using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.SubsidyDetails
{
    [RuleDBTag("SubsidyDetailsValidateStopOrderAmount",
     "Stop Order Amount Validation",
     "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.SubsidyDetails.SubsidyDetailsValidateStopOrderAmount")]
    [RuleParameterTag(new string[] { "@MinAmount, 0.01, 10", "@MaxAmount, 999999999.99, 10" })]
    [RuleInfo]
    public class SubsidyDetailsValidateStopOrderAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ISubsidy subsidy = (ISubsidy)Parameters[0];

            // ignore this rule if the employmentstatus has been set to previous
            if (subsidy.Employment.EmploymentStatus.Key == (int)EmploymentStatuses.Previous)
                return 1;

            double min = Convert.ToDouble(RuleItem.RuleParameters[0].Value);
            double max = Convert.ToDouble(RuleItem.RuleParameters[1].Value);

            if (subsidy.StopOrderAmount < min || subsidy.StopOrderAmount > max)
                AddMessage("Stop Order Amount must be greater than " + min.ToString(SAHL.Common.Constants.CurrencyFormat) + " and less than " + max.ToString(SAHL.Common.Constants.CurrencyFormat), "", Messages);

            return 1;
        }
    }

    [RuleDBTag("SubsidyDetailsMandatoryAccountOrApplication",
    "Subsidy details must be attached to an account or an application",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.SubsidyDetails.SubsidyDetailsMandatoryAccountOrApplication")]
    [RuleInfo]
    public class SubsidyDetailsMandatoryAccountOrApplication : BusinessRuleBase
    {
        public SubsidyDetailsMandatoryAccountOrApplication(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ISubsidy subsidy = (ISubsidy)Parameters[0];

            // ignore this rule if the employmentstatus has been set to previous
            if (subsidy.Employment.EmploymentStatus.Key == (int)EmploymentStatuses.Previous)
                return 1;

            if (subsidy.Account == null && subsidy.Application == null)
                AddMessage("Subsidy details must be attached to an account", "", Messages);

            string sqlQuery = "";

            // need to check if the offer/account has changed. if it has then throw an error that the offer/account cannot be changed.

            sqlQuery = "select accSub.AccountKey from AccountSubsidy accSub where SubsidyKey = @subKey";
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@subKey", subsidy.Key));
            object accountKey = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

            // if there is no account then check the offers. if there is an account then check if it matches the new key.
            if (accountKey != null)
            {
                if (subsidy.Account != null)
                {
                    if (subsidy.Account.Key != Convert.ToInt32(accountKey))
                    {
                        string errMsg = "The Account associated with this subsidy cannot be changed.";
                        AddMessage(errMsg, errMsg, Messages);
                        return 0;
                    }
                }
                if (subsidy.Application != null)
                {
                    if (subsidy.Application.Key != Convert.ToInt32(accountKey))
                    {
                        string errMsg = "The Account associated with this subsidy cannot be changed.";
                        AddMessage(errMsg, errMsg, Messages);
                        return 0;
                    }
                }
            }

            sqlQuery = "select offSub.OfferKey from OfferSubsidy offSub where SubsidyKey = @subKey";
            parameters.Clear();
            parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@subKey", subsidy.Key));
            accountKey = null;
            accountKey = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

            // if there is no account then check the offers. if there is an account then check if it matches the new key.
            if (accountKey != null)
            {
                if (subsidy.Account != null)
                {
                    if (subsidy.Account.Key != Convert.ToInt32(accountKey))
                    {
                        string errMsg = "The Offer associated with this subsidy cannot be changed.";
                        AddMessage(errMsg, errMsg, Messages);
                        return 0;
                    }
                }
                if (subsidy.Application != null)
                {
                    if (subsidy.Application.Key != Convert.ToInt32(accountKey))
                    {
                        string errMsg = "The Offer associated with this subsidy cannot be changed.";
                        AddMessage(errMsg, errMsg, Messages);
                        return 0;
                    }
                }
            }

            return 1;
        }
    }

    [RuleDBTag("SubsidyDetailsMandatorySalaryNumber",
        "Subsidy details must have a salary number",
        "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.SubsidyDetails.SubsidyDetailsMandatorySalaryNumber")]
    [RuleInfo]
    public class SubsidyDetailsMandatorySalaryNumber : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ISubsidy subsidy = (ISubsidy)Parameters[0];

            // ignore this rule if the employmentstatus has been set to previous
            if (subsidy.Employment.EmploymentStatus.Key == (int)EmploymentStatuses.Previous)
                return 1;

            if (String.IsNullOrEmpty(subsidy.SalaryNumber))
            {
                AddMessage("Salary Number is a mandatory field", "", Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("SubsidyDetailsUpdateSubsidyAmount",
    "When Subsidy is saved and there is a fixed debit order amount > 0, check that the fixed debit order + the new subsidy amount (to be 0 if setting employment to previous) is > total instalment due (including premiums).",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.SubsidyDetails.SubsidyDetailsUpdateSubsidyAmount")]
    [RuleInfo]
    public class SubsidyDetailsUpdateSubsidyAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ISubsidy subsidy = (ISubsidy)Parameters[0];

            IList<IAccount> _accounts = new List<IAccount>();
            double totalAmount = 0.00;
            double totalSubsidyAmount = 0.00;
            double fixedPayment = 0.00;

            if (subsidy.Account == null)
                return 1;

            foreach (IAccount account in subsidy.Account.RelatedChildAccounts)
            {
                _accounts.Add(account);
            }
            _accounts.Add(subsidy.Account);

            fixedPayment = subsidy.Account.FixedPayment; // debit order amount

            // if employment set to previous then set subsidy amount = 0.00 else get the subsidy amount
            if (subsidy.Employment.EmploymentStatus.Key == (int)EmploymentStatuses.Previous)
                totalSubsidyAmount = 0.00;
            else
                totalSubsidyAmount = subsidy.StopOrderAmount;
            foreach (IAccount account in _accounts)
            {
                if (account.IsActive && account.FinancialServices.Count > 0)
                {
                    foreach (IFinancialService fs in account.FinancialServices)
                    {
                        if (fs.AccountStatus.Key == (int)AccountStatuses.Open || fs.AccountStatus.Key == (int)AccountStatuses.Dormant)
                        {
                            totalAmount += fs.Payment;
                            if (fs.ManualDebitOrders.Count > 0)
                            {
                                foreach (var manDebitOrder in fs.ManualDebitOrders)
                                {
                                    if (manDebitOrder != null && manDebitOrder.Active && manDebitOrder.TransactionType.Key == (int)TransactionTypes.MonthlyServiceFee)
                                    {
                                        totalAmount += manDebitOrder.Amount;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            totalAmount += subsidy.Account.InstallmentSummary.TotalRegentPremium;

            if (totalAmount <= 0)
                return 0;

            if (fixedPayment + totalSubsidyAmount < totalAmount)
            {
                string errMsg = string.Format("Please adjust the Fixed debit order amount on account {0} before adjusting the subsidy amount.", subsidy.Account.Key);
                AddMessage(errMsg, errMsg, Messages);
                return 0;
            }

            return 1;
        }
    }
}