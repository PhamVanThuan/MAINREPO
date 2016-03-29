using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using System.Collections.Generic;
using WatiN.Core;

namespace BuildingBlocks.Presenters.LoanServicing
{
    public class PostTransaction : PostTransactionControls
    {
        private ICommonService commonService;

        public PostTransaction()
        {
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
        }

        /// <summary>
        /// This method is used to post a transaction against using the Post Transaction screen.
        /// </summary>
        /// <param name="b">IE TestBrowser Object</param>
        /// <param name="transactionType">TransactionType.Description</param>
        /// <param name="TransactionAmount">Transaction Amount</param>
        /// <param name="TransactionDate">Transaction Date</param>
        /// <param name="Reference">Reference</param>
        public void PostLoanTransaction(TransactionTypeEnum transactionType, decimal TransactionAmount, DateTime TransactionDate, string Reference, int financialServiceKey = 0)
        {
            SelectTransaction(transactionType);
            //posting on an individual financial service
            if (financialServiceKey > 0)
            {
                base.FinancialService.SelectByValue(financialServiceKey.ToString());
            }
            base.EffectiveDate.Value = TransactionDate.ToString(Formats.DateFormat);
            string rands; string cents;
            commonService.SplitRandsCents(out rands, out cents, TransactionAmount.ToString());
            base.TransactionAmountRands.TypeText(rands);
            base.TransactionAmountCents.TypeText(cents);
            base.Reference.Value = Reference;
            ClickPost();
        }

        /// <summary>
        /// Check that the transaction types on the view match those passed as a parameter
        /// </summary>
        public void AssertTransactionTypes(List<int> expectedTransactionTypes, bool atLeastOneMatch = false)
        {
            OptionCollection optionlist = base.TransactionTypeDropDown.Options;
            var actualTranTypes = new List<int>();
            int exists = 0;
            foreach (var option in optionlist)
            {
                if (option.Value != "-select-")
                {
                    actualTranTypes.Add(int.Parse(option.Value));
                }
            }
            //now we have our actual list
            foreach (var item in expectedTransactionTypes)
            {
                if (actualTranTypes.Contains(item))
                    exists++;
            }
            Assert.AreEqual(expectedTransactionTypes.Count, exists);
            if (!atLeastOneMatch)
                Assert.AreEqual(expectedTransactionTypes.Count, actualTranTypes.Count);
        }

        /// <summary>
        ///
        /// </summary>
        public void AssertControlsValid()
        {
            Assert.That(base.TransactionTypeDropDown.Exists, "TransactionTypeDropDown doesnt exist");
            Assert.That(base.EffectiveDate.Exists, "EffectiveDate doesnt exist");
            Assert.That(base.TransactionAmountRands.Exists, "TransactionAmountRands doesnt exist");
            Assert.That(base.TransactionAmountCents.Exists, "TransactionAmountCents doesnt exist");
            Assert.That(base.Reference.Exists, "Reference doesnt exist");
            Assert.That(base.btnPost.Exists, "Post Button doesnt exist");
        }

        /// <summary>
        ///
        /// </summary>
        public void ClickPost()
        {
            base.btnPost.Click();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fsKeyList"></param>
        public void AssertFinancialServiceDropdownList(List<int> fsKeyList)
        {
            Assertions.WatiNAssertions.AssertSelectListContents(base.FinancialService, fsKeyList, true);
        }

        /// <summary>
        /// selects a transaction from the drop down
        /// </summary>
        /// <param name="transactionType"></param>
        public void SelectTransaction(TransactionTypeEnum transactionType)
        {
            if (base.TransactionTypeDropDown.SelectedOption.Value != ((int)transactionType).ToString())
                base.TransactionTypeDropDown.SelectByValue(((int)transactionType).ToString());
        }

        /// <summary>
        /// Asserts the selected value of the financial service dropdown
        /// </summary>
        /// <param name="expectedOption"></param>
        public void AssertFinancialServiceDefaultSelection(string expectedOption)
        {
            Assertions.WatiNAssertions.AssertSelectedValue(expectedOption, base.FinancialService);
        }

        /// <summary>
        /// Checks to see if the financial service dropdown list exists on the screen
        /// </summary>
        public void AssertFinancialServiceDropdownExists(bool exists)
        {
            Assert.That(base.FinancialService.Exists == exists);
        }
    }
}