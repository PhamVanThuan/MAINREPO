using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using ObjectMaps.Pages;
using System;
using System.Linq;
using WatiN.Core;

namespace BuildingBlocks.Presenters.LoanServicing.ManualDebitOrders
{
    /// <summary>
    ///
    /// </summary>
    public class ManualDebitOrderBaseView : ManualDebitOrderBaseControls
    {
        private IAccountService accountService;

        public ManualDebitOrderBaseView()
        {
            accountService = ServiceLocator.Instance.GetService<IAccountService>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="account"></param>
        /// <param name="manualDebitOrder"></param>
        public Automation.DataModels.ManualDebitOrder PopulateManualDebitOrderDetails(Automation.DataModels.Account account, Automation.DataModels.ManualDebitOrder manualDebitOrder = null)
        {
            var random = new Random();
            var bankAccount = accountService.GetBankAccountRecordsForAccount(account.AccountKey).FirstOrDefault();
            var actionDate = DateTime.Now.AddDays(1);
            //we need to check if we are after the 28th of the month
            if (DateTime.Now.Day >= 28)
            {
                //set action date to first of next month
                actionDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 1);
            }
            if (manualDebitOrder == null)
            {
                manualDebitOrder = new Automation.DataModels.ManualDebitOrder
                {
                    Amount = random.Next(4000, 6000),
                    Reference = account.AccountKey.ToString(),
                    ActionDate = actionDate,
                    Notes = string.Format(@"Add Manual Debit Order Test-{0}", random.Next(0, 1000000)),
                    TransactionType = TransactionTypeEnum.ManualDebitOrderPayment,
                    GeneralStatusKey = 1
                };
            }
            //add bank account
            manualDebitOrder.BankAccount = bankAccount;
            manualDebitOrder.BankAccountKey = bankAccount.BankAccountKey;
            //select the bank account
            base.Bank.Option(Find.ByValue(bankAccount.BankAccountKey.ToString())).Select();
            //effective date
            base.EffectiveDate.Value = manualDebitOrder.ActionDate.ToString(Formats.DateFormat);
            //amount
            decimal amount = manualDebitOrder.Amount;
            amount = Math.Round(amount, 2);
            string[] splitAmount = amount.ToString().Split('.');
            string rands = splitAmount[0];
            string cents = "00";
            if (splitAmount.Length > 1)
                cents = splitAmount[1];
            base.AmountRands.TypeText(rands);
            base.AmountCents.TypeText(cents);
            //reference
            base.Reference.Value = manualDebitOrder.Reference;
            //notes
            base.Note.Value = manualDebitOrder.Notes;
            return manualDebitOrder;
        }

        /// <summary>
        /// Selects a future dated transaction from the grid using the FinancialServiceRecurringTransactionKey.
        /// </summary>
        /// <param name="manualDebitOrderKey">FinancialServiceRecurringTransactionKey</param>
        public void SelectManualDebitOrder(int manualDebitOrderKey)
        {
            base.FutureDateTransactionsSelectRow(manualDebitOrderKey.ToString()).Click();
        }

        /// <summary>
        /// Selects the -select- option
        /// </summary>
        public void SelectNoBankAccount()
        {
            base.Bank.Option(Find.ByValue("-select-")).Select();
        }
    }
}