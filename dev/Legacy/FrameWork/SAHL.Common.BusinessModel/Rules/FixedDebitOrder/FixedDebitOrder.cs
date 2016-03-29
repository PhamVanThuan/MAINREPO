using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Helpers;


namespace SAHL.Common.BusinessModel.Rules.FixedDebitOrder
{
    [RuleDBTag("FinancialServiceBankAccount",
    "Financial Service must have one active FinancialService Bank Account ",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.FixedDebitOrder.FinancialServiceBankAccount")]
    [RuleInfo]
    public class FinancialServiceBankAccount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IFinancialService fs = (IFinancialService)Parameters[0];

            // make sure the financial service type exists
            if (fs.FinancialServiceType == null)
                return 1;

            if (fs.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan)
            {
                if (fs.FinancialServiceBankAccounts == null || fs.FinancialServiceBankAccounts.Count == 0)
                {
                    AddMessage("Financial Service must have one active FinancialService Bank Account ", "", Messages);
                }
                else
                {
                    // also make sure that there is only one active bank account attached
                    int activeCount = 0;
                    foreach (IFinancialServiceBankAccount bankAccount in fs.FinancialServiceBankAccounts)
                    {
                        if (bankAccount.GeneralStatus.Key == (int)GeneralStatuses.Active)
                            activeCount++;
                    }
                    if (activeCount != 1)
                        AddMessage("Financial Service must have one active FinancialService Bank Account ", "", Messages);
                }
            }
            return 1;
        }
    }

    [RuleDBTag("FinancialServiceBankAccountAddNewDebitOrder",
   "Payment Type : Debit Order - Bank Account details must be captured ",
   "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.FixedDebitOrder.FinancialServiceBankAccountAddNewDebitOrder")]
    [RuleInfo]
    public class FinancialServiceBankAccountAddNewDebitOrder : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IFinancialServiceBankAccount fsb = (IFinancialServiceBankAccount)Parameters[0];

            // make sure the financial service payment type exists
            if (fsb.FinancialServicePaymentType == null)
                return 1;

            if (fsb.FinancialServicePaymentType.Key == (int)FinancialServicePaymentTypes.DebitOrderPayment && fsb.BankAccount == null)
                AddMessage("Payment Type : Debit Order - Bank Account details must be captured ", "", Messages);

            return 1;
        }
    }

    [RuleDBTag("FinancialServiceBankvarifix",
    "Fixed and Variable Financial Service Bank Accounts must be the same ",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.FixedDebitOrder.FinancialServiceBankvarifix")]
    [RuleInfo]
    public class FinancialServiceBankvarifix : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IFinancialServiceBankAccount fsb = (IFinancialServiceBankAccount)Parameters[0];

            // make sure the required properties have been set
            if (fsb.FinancialService == null || fsb.FinancialService.Account == null)
                return 1;

            IReadOnlyEventList<IFinancialService> fsVar = fsb.FinancialService.Account.GetFinancialServicesByType(FinancialServiceTypes.VariableLoan, new AccountStatuses[] { AccountStatuses.Open });
            IReadOnlyEventList<IFinancialService> fsFixed = fsb.FinancialService.Account.GetFinancialServicesByType(FinancialServiceTypes.FixedLoan, new AccountStatuses[] { AccountStatuses.Open });

            bool isInSync = true;

            if (fsFixed != null)
            {
                if (fsVar[0].FinancialServiceBankAccounts[0].BankAccount != fsFixed[0].FinancialServiceBankAccounts[0].BankAccount)
                    isInSync = false;
                if (fsVar[0].FinancialServiceBankAccounts[0].DebitOrderDay != fsFixed[0].FinancialServiceBankAccounts[0].DebitOrderDay)
                    isInSync = false;
            }

            if (!isInSync)
                AddMessage("Banking and Debit Order Details for Fixed and Variable financial Services Differ ", "", Messages);

            return 1;
        }
    }

    #region FinancialServiceBankAccountUpdateDebitOrderAmount
    [RuleDBTag("FinancialServiceBankAccountUpdateDebitOrderAmount",
    "When updating the Fixed Debit Order Amount the amount must be greater than the total amount due.",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.FixedDebitOrder.FinancialServiceBankAccountUpdateDebitOrderAmount")]
    [RuleInfo]
    public class FinancialServiceBankAccountUpdateDebitOrderAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The FinancialServiceBankAccountUpdateDebitOrderAmount rule expects a Domain object to be passed.");

            IAccount acc = Parameters[0] as IAccount;
            IFutureDatedChangeDetail futureDatedChangeDetail = null;

            if (acc == null)
                futureDatedChangeDetail = Parameters[0] as IFutureDatedChangeDetail;

            if (acc == null && futureDatedChangeDetail == null)
                throw new ArgumentException("The FinancialServiceBankAccountUpdateDebitOrderAmount rule expects the following objects to be passed: IAccount or IFutureDatedChangeDetail.");

            double? updatedFixedDebitOrderAmount = new double?();
            if (Parameters.Length > 1 && Parameters[1] is double )
            {
                updatedFixedDebitOrderAmount = Convert.ToDouble(Parameters[1]);
            }

            #endregion

            //IList<IAccount> _accounts = new List<IAccount>();
            double totalAmount = 0.00;
            double totalSubsidyAmount = 0.00;
            double FixedPayment = 0.00;
            bool subsidy = false;

            if (acc != null)
                FixedPayment = acc.FixedPayment;
            else
                FixedPayment = Convert.ToDouble(futureDatedChangeDetail.Value);

            if (updatedFixedDebitOrderAmount.HasValue)
            {
                FixedPayment = updatedFixedDebitOrderAmount.Value;
            }

            if (acc == null)
            {
                IAccountRepository _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                acc = _accRepo.GetAccountByKey(futureDatedChangeDetail.ReferenceKey);
            }

            if (acc == null)
                return 0;

            if (acc.AccountStatus.Key != (int)AccountStatuses.Open)
                return 0;

            totalAmount = acc.InstallmentSummary.TotalAmountDue; // this will return the sum of the loan instalment, hoc premium, life premium, service fees and regent premiums (where applicable)

            if (totalAmount <= 0)
                return 0;

            // at this point we have the total amount.
            // if there is an active subsidy then we'll use that - otherwise we get the total amount
            if (acc.Subsidies.Count > 0)
            {

                foreach (ISubsidy _subsidy in acc.Subsidies)
                {
                    if (_subsidy.GeneralStatus.Key == (int)GeneralStatuses.Active)
                    {
                        subsidy = true;
                        totalSubsidyAmount += _subsidy.StopOrderAmount;
                        break;
                    }
                }
            }
            // acc.FixedPayment = Amount being inputted.
            if ((subsidy && FixedPayment < (totalAmount - totalSubsidyAmount)) && FixedPayment > 0)
            {
                string msg = String.Format("The fixed debit order amount must be at least the total amount due ({0:c}) for subsidy client.", (totalAmount - totalSubsidyAmount));
                AddMessage(msg, msg, Messages);
                Console.WriteLine(msg);
                return 0;
            }
            else if ((!subsidy && FixedPayment < totalAmount) && FixedPayment > 0)
            {
                string msg = String.Format("The fixed debit order amount must be at least the total amount due ({0:c})", totalAmount);
                AddMessage(msg, msg, Messages);
                Console.WriteLine(msg);
                return 0;
            }

            return 1;
        }
    }
    #endregion
}
