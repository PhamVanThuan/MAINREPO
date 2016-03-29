using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BuildingBlocks.Assertions
{
    public static class WebsiteCalculatorAssertions
    {
        /// Assert that when the amounts are set to 0 on the calculator for the provided loan purpose, the right validation messages are displayed.
        /// </summary>
        /// <param name="TestBrowser"></param>
        /// <param name="loanPurposeType"></param>
        public static void AssertCalculatorZeroAmountFieldsContainsValidationMessages(MortgageLoanPurposeEnum loanPurposeType, ProductEnum product,
                float affordabilityMaxInstalmentAmount = 0.00f)
        {
            try
            {
                var messages = new List<string>();
                switch (loanPurposeType)
                {
                    case MortgageLoanPurposeEnum.Switchloan:
                        Assert.Contains(SAHLWebsiteValidationMessages.HomeValue_ValidationMessage, messages);
                        Assert.Contains(SAHLWebsiteValidationMessages.CurrentLoanToSwitch_ValidationMessage, messages);
                        Assert.Contains(SAHLWebsiteValidationMessages.GrossIncome_ValidationMessage, messages);
                        Assert.Contains(SAHLWebsiteValidationMessages.LoanMonthTerm_ValidationMessage, messages);
                        if (product == ProductEnum.VariFixLoan)
                        {
                            var fixedAmountMessage = SAHLWebsiteValidationMessages.FixedAmount_ValidationMessage;

                            //Replace part of the string with the rand value.
                            fixedAmountMessage = fixedAmountMessage.Replace("[Token]", "0 ");
                            Assert.Contains(fixedAmountMessage, messages);
                        }
                        break;

                    case MortgageLoanPurposeEnum.Newpurchase:
                        Assert.Contains(SAHLWebsiteValidationMessages.PurchasePrice_ValidationMessage, messages);
                        Assert.Contains(SAHLWebsiteValidationMessages.GrossIncome_ValidationMessage, messages);
                        Assert.Contains(SAHLWebsiteValidationMessages.LoanMonthTerm_ValidationMessage, messages);
                        Assert.Contains(SAHLWebsiteValidationMessages.DepositCantBeEqualPurchasePrice, messages);
                        if (product == ProductEnum.VariFixLoan)
                        {
                            var fixedAmountMessage = SAHLWebsiteValidationMessages.FixedAmount_ValidationMessage;

                            //Replace part of the string with the rand value.
                            fixedAmountMessage = fixedAmountMessage.Replace("[Token]", "0 ");
                            Assert.Contains(fixedAmountMessage, messages);
                        }
                        break;

                    case MortgageLoanPurposeEnum.Unknown:
                        Assert.Contains(SAHLWebsiteValidationMessages.GrossIncome_ValidationMessage, messages);
                        Assert.Contains(SAHLWebsiteValidationMessages.LoanYearTerm_ValidationMessage, messages);
                        var installmentMessage = SAHLWebsiteValidationMessages.MonthlyInstallment_ValidationMessage;

                        //Replace part of the string with the rand value.
                        installmentMessage = installmentMessage.Replace("[Token]", Math.Round(affordabilityMaxInstalmentAmount, 0).ToString());

                        Assert.Contains(installmentMessage, messages);
                        Assert.Contains(SAHLWebsiteValidationMessages.InterestRate_ValidationMessage, messages);
                        break;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Assert that when the fields are left blank on the calculator for the provided loan purpose, the right validation messages are displayed.
        /// </summary>
        /// <param name="TestBrowser"></param>
        /// <param name="loanPurposeType"></param>
        public static void AssertCalculatorBlankFieldsContainsValidationMessages(MortgageLoanPurposeEnum loanPurposeType, ProductEnum product)
        {
            try
            {
                var messages = new List<string>();
                switch (loanPurposeType)
                {
                    case MortgageLoanPurposeEnum.Switchloan:
                        Assert.Contains(SAHLWebsiteValidationMessages.HomeValueRequired_ValidationMessage, messages);
                        Assert.Contains(SAHLWebsiteValidationMessages.CurrentLoanAmountRequired_ValidationMessage, messages);
                        Assert.Contains(SAHLWebsiteValidationMessages.GrossIncomeRequired_ValidationMessage, messages);
                        Assert.Contains(SAHLWebsiteValidationMessages.TermOfLoanRequired_ValidationMessage, messages);
                        if (product == ProductEnum.VariFixLoan)
                            Assert.Fail("need to remove varifix tests");
                        break;

                    case MortgageLoanPurposeEnum.Newpurchase:
                        Assert.Contains(SAHLWebsiteValidationMessages.PurchasePriceRequired_ValidationMessage, messages);
                        Assert.Contains(SAHLWebsiteValidationMessages.GrossIncomeRequired_ValidationMessage, messages);
                        Assert.Contains(SAHLWebsiteValidationMessages.TermOfLoanRequired_ValidationMessage, messages);
                        if (product == ProductEnum.VariFixLoan)
                            Assert.Fail("need to remove varifix tests");
                        break;

                    case MortgageLoanPurposeEnum.Unknown:
                        Assert.Contains(SAHLWebsiteValidationMessages.IncomeAmountRequired_ValidationMessage, messages);
                        Assert.Contains(SAHLWebsiteValidationMessages.ProfitFromSaleRequired_ValidationMessage, messages);
                        Assert.Contains(SAHLWebsiteValidationMessages.OtherContributionsRequired_ValidationMessage, messages);
                        Assert.Contains(SAHLWebsiteValidationMessages.MonthlyInstallmentRequired_ValidationMessage, messages);
                        Assert.Contains(SAHLWebsiteValidationMessages.LoanTermRequired_ValidationMessage, messages);
                        Assert.Contains(SAHLWebsiteValidationMessages.InterestRateRequired_ValidationMessage, messages);
                        break;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Assert that the view does not have any validation messages.
        /// </summary>
        /// <param name="TestBrowser"></param>
        /// <param name="loanPurposeType"></param>
        public static void AssertCalculatorNotContainsValidationMessages(TestBrowser TestBrowser, MortgageLoanPurposeEnum loanPurposeType)
        {
            try
            {
                var messages = new List<string>();
                switch (loanPurposeType)
                {
                    case MortgageLoanPurposeEnum.Switchloan:
                        Assert.AreEqual(0, messages.Count, "", "Expected 0 Valdiation Messages");
                        break;

                    case MortgageLoanPurposeEnum.Newpurchase:
                        Assert.AreEqual(0, messages.Count, "", "Expected 0 Valdiation Messages");
                        break;

                    case MortgageLoanPurposeEnum.Unknown:
                        Assert.AreEqual(0, messages.Count, "", "Expected 0 Valdiation Messages");
                        break;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}