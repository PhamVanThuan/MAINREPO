using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using System.Collections.Generic;

namespace BuildingBlocks.Presenters.LoanServicing.ManualDebitOrders
{
    public class ManualDebitOrderAddView : ManualDebitOrderAddControls
    {
        private readonly IWatiNService watinService;

        public ManualDebitOrderAddView()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Clicks the Add button.
        /// </summary>
        public void ClickAdd(bool handlePopUp)
        {
            if (handlePopUp)
            {
                watinService.HandleConfirmationPopup(base.Add);
            }
            else
            {
                base.Add.Click();
            }
        }

        /// <summary>
        /// Clears the Reference field.
        /// </summary>
        public void ClearReference()
        {
            base.Reference.Clear();
        }

        /// <summary>
        /// Enters the effective date.
        /// </summary>
        /// <param name="effectiveDate">Effective Date</param>
        public void EnterEffectiveDate(DateTime effectiveDate)
        {
            base.EffectiveDate.Value = effectiveDate.ToString(Formats.DateFormat);
        }

        /// <summary>
        /// Asserts that the Reference field is populated with the accountkey.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        public void AssertReferenceEqualsAccountKey(int accountKey)
        {
            string reference = base.Reference.Value;
            Assert.AreEqual(accountKey.ToString(), reference, "Reference field is not the AccountKey.");
        }

        /// <summary>
        /// Asserts that the contents of the bank account SelectList is equal to the expected list.
        /// </summary>
        /// <param name="expectedContents">Expected list of bank accounts</param>
        public void AssertBankAccountListContents(List<string> expectedContents)
        {
            Assertions.WatiNAssertions.AssertSelectListContents(base.Bank, expectedContents);
        }

        /// <summary>
        /// Ensures that the arrear balance on the screen matches the one provided.
        /// </summary>
        /// <param name="arrearTransactionBalance"></param>
        public void AssertArrearBalance(decimal arrearTransactionBalance)
        {
            Assertions.WatiNAssertions.AssertCurrencyLabel(base.ArrearBalanceLabel, arrearTransactionBalance);
        }
    }
}