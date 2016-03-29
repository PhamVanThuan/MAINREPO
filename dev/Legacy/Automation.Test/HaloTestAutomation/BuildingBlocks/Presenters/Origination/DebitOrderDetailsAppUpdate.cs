using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using WatiN.Core.UtilityClasses;

namespace BuildingBlocks.Presenters.Origination
{
    public class DebitOrderDetailsAppUpdate : DebitOrderDetailsAppUpdateControls
    {
        public void UpdateDebitOrderDetails(string PaymentType, int BankAccount, int DebitOrderday, ButtonTypeEnum Button)
        {
            base.ctl00MainDOPaymentTypeUpdate.Option(PaymentType).Select();
            SimpleTimer Timer = new SimpleTimer(TimeSpan.FromSeconds(30));
            while (base.ctl00MainBankAccountUpdate.Options.Count <= 1)
            {
                if (Timer.Elapsed) throw new WatiN.Core.Exceptions.TimeoutException("trying to select Option 2 from 'Bank Account' select list");
                else System.Threading.Thread.Sleep(1000);
            }
            base.ctl00MainBankAccountUpdate.Options[BankAccount].Select();
            base.ctl00MainDebitOrderDayUpdate.Option(DebitOrderday.ToString()).Select();

            switch (Button)
            {
                case ButtonTypeEnum.Update:
                    base.ctl00MainbtnUpdate.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.ctl00MainCancelButton.Click();
                    break;
            }
        }

        /// <summary>
        /// This will populate the fields on this view with the provided parameters.
        /// </summary>
        /// <param name="debitOrderday"></param>
        /// <param name="paymentType"></param>
        public void PopulateView(int debitOrderday, FinancialServicePaymentTypeEnum paymentType, bool populateBankAccount)
        {
            if (paymentType != FinancialServicePaymentTypeEnum.None)
                base.ctl00MainDOPaymentTypeUpdate.SelectByValue(((int)paymentType).ToString());
            Thread.Sleep(2000);
            if (populateBankAccount)
                base.ctl00MainBankAccountUpdate.Options[1].Select();
            if (debitOrderday != 0)
                base.ctl00MainDebitOrderDayUpdate.Option(new Regex(debitOrderday.ToString())).Select();
            base.ctl00MainbtnUpdate.Click();
        }

        /// <summary>
        /// ensures that the max value in the debit order day dropdown is 28
        /// </summary>
        public void AssertMaxAllowedDebitOrderDay()
        {
            var debitOrderDays = base.ctl00MainDebitOrderDayUpdate.Options;
            int max = (from dod in debitOrderDays
                       where dod.Value.Contains("select") == false
                       select int.Parse(dod.Value)).Max();
            Assert.That(max == 28);
        }
    }
}