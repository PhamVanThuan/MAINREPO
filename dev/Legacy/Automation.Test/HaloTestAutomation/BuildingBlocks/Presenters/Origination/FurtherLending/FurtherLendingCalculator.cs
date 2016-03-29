using Automation.DataAccess;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using Common.Extensions;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using WatiN.Core;
using WatiN.Core.Logging;

namespace BuildingBlocks.Presenters.Origination.FurtherLending
{
    /// <summary>
    /// Contains methods for interacting with the Further Lending Calculator.
    /// </summary>
    public class FurtherLendingCalculator : FurtherLendingCalculatorControls
    {
        private IAccountService accountService;
        private IApplicationService applicationService;
        private ICommonService commonService;

        public FurtherLendingCalculator()
        {
            accountService = ServiceLocator.Instance.GetService<IAccountService>();
            applicationService = ServiceLocator.Instance.GetService<IApplicationService>();
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
        }

        /// <summary>
        /// Creates a single Further Lending Application using the FL Calculator
        /// </summary>
        /// <param name="amount">Value for the Application</param>
        /// <param name="contactOption">Fax or Email</param>
        /// <param name="b">IE TestBrowser Object</param>
        /// <param name="appType">Readvance, Further Advance or Further Loan</param>
        public void CreateApplication(string amount, string contactOption, string appType, int accountKey)
        {
            //populate the correct required amount depending on the app type we are creating
            switch (appType)
            {
                case "Readvance":
                    base.ReadvanceRequired.Value = amount;
                    break;

                case "Further Advance":
                    base.FurtherAdvReq.Value = amount;
                    break;

                case "Further Loan":
                    base.FurtherLoanReq.Value = amount;
                    break;
            }
            SetEmploymentType();
            base.CalculateButton.Click();
            string income = base.HouseholdIncome.Text.CleanCurrencyString(true);
            //set the field
            base.NewIncome1.Value = income;
            //if the PTI warning is displayed we will need to increase the Household Income
            bool warning = CheckForPTIWarning();
            while (warning)
            {
                IncreaseHouseholdIncome(2, income);
                base.CalculateButton.Click();
                warning = CheckForPTIWarning();
            }
            base.NextButton.Click();
            //check to see if the account is in VariFix
            var account = accountService.GetAccountByKey(accountKey);
            if (account.ProductKey == ProductEnum.VariFixLoan)
            {
                base.NextButton.Click();
            }
            //2nd tab
            VerifyConditions();
            base.NextButton.Click();
            //3rd tab
            SetLegalEntityContactOptions(contactOption);
            base.NextButton.WaitUntilExists();
            base.NextButton.WaitUntil(x => x.ClassName == "BtnNormal4");
            base.NextButton.Click();
            //final tab
            base.GenerateButton.Click();
            //handle any warnings
            base.Document.Page<BasePage>().DomainWarningClickYes();
        }

        /// <summary>
        /// If the employment type is Unknown this method will set it to Salaried for the FL Calculator
        /// </summary>
        public FurtherLendingCalculator SetEmploymentType()
        {
            if (base.EmploymentType.SelectedItem.ToString() == "Unknown")
            {
                base.EmploymentType.Option("Salaried").Select();
            }
            return this;
        }

        /// <summary>
        /// Clicks the conditions verified checkbox in the further lending calculator
        /// </summary>
        public FurtherLendingCalculator VerifyConditions()
        {
            base.ConditionCheckBox.Checked = true;
            return this;
        }

        /// <summary>
        /// Sets the Legal Entity Contact details for the Further Lending Application
        /// </summary>
        /// <param name="contactOption">Fax or Email</param>
        public FurtherLendingCalculator SetLegalEntityContactOptions(string contactOption)
        {
            switch (contactOption)
            {
                case "Email":
                    base.AlternateEmail.Clear();
                    base.AlternateEmail.TypeText("clintons@sahomeloans.com");
                    break;

                case "Fax":
                    base.AlternateFaxNUMB.Clear();
                    base.AlternateFaxNUMB.TypeText("0861234567");
                    break;
            }
            return this;
        }

        /// <summary>
        /// Checks for the PTI warning on the Further Lending Calculator
        /// </summary>
        /// <returns>True = Message Exists, False = Message does not exist</returns>
        public bool CheckForPTIWarning()
        {
            string pti = base.NewPTI.Text;
            string warning = String.Format(@"PTI is too high. PTI is {0}. The maximum allowed is: 35.00%.", pti);
            bool resultMatch = false;
            //lets see if we can find the message
            if (base.divValidationSummaryBody.Exists)
            {
                resultMatch = base.listErrorMessages.Exists(Find.ByText(warning));
            }
            return resultMatch;
        }

        /// <summary>
        /// Applies a multiplication factor to the current household income for the FL Calculator
        /// </summary>
        /// <param name="factor">The factor by which to multiply the current household income</param>
        public double IncreaseHouseholdIncome(int factor, string income)
        {
            //clean the string.
            double newIncome = (Convert.ToDouble((income.RemoveWhiteSpace())) * factor);
            //update the income field
            base.NewIncome1.Value = Convert.ToString(newIncome);
            return newIncome;
        }

        /// <summary>
        /// This is used to create multiple further lending applications from a CSV file. The method requires a row
        /// of data from the CSV file in order to get values for the applications.
        /// </summary>
        /// <param name="contactOption">Contact Option for the FL application</param>
        /// <param name="b">IE TestBrowser Object</param>
        /// <param name="row">RowData from the CSV file</param>
        /// <param name="ltvLowerBound"></param>
        /// <param name="ltvUpperBound"></param>
        public void CreateMultipleApplications(string contactOption, QueryResults row, int ltvLowerBound, int ltvUpperBound)
        {
            /*
             * populate all of the applications required
             * the value for applications that they do not qualify for should be set to -1 in the CSV file
             * we check the value first before populating the required mount
            */
            PopulateFLValuesFromTestData(row, ltvLowerBound, ltvUpperBound);
            ClickCalculateAndConfirmWarnings();
            base.NextButton.Click();
            if (base.divValidationSummaryBody.Exists)
            {
                foreach (Element item in base.listErrorMessages)
                {
                    if (item.Text.RemoveWhiteSpace().Replace(",", "").Contains("Further advance requested is more than the allowed"))
                    {
                        //get allowed amount
                        var allowed = commonService.ConvertCurrencyStringToInt(base.QualifyFurtherAdvance.Text);
                        base.FurtherAdvReq.Clear();
                        base.FurtherAdvReq.Value = (allowed - 1).ToString(); ;
                        break;
                    }
                }
            }
            //check to see if the VariFix tab exists. If it does we will need to click an extra [Next] button.
            if (row.Rows(0).Column("Product").Value == "VariFix Loan")
            {
                base.NextButton.Click();
            }
            //2nd tab
            VerifyConditions();
            base.NextButton.Click();
            //3rd tab
            SetLegalEntityContactOptions(contactOption);
            base.NextButton.WaitUntilExists();
            base.NextButton.WaitUntil(x => x.ClassName == "BtnNormal4" || x.GetAttributeValue("disabled") == "False");
            base.NextButton.Click();
            //final tab
            base.GenerateButton.Click();
            //handle any warnings
            base.Document.Page<BasePage>().DomainWarningClickYes();
        }

        /// <summary>
        /// This method is called from the further lending application create method in order to create a further advance application that
        /// falls within a particular LTV bracket. The method calculates the required application amount for the LTV to fall within the
        /// boundaries supplied.
        /// </summary>
        /// <param name="applicationValue">Application Value</param>
        /// <param name="currentBalance">Current Balance</param>
        /// <param name="valuation">Valuation Amount</param>
        /// <param name="ltvLowerBound">LTV Lower Bound</param>
        /// <param name="ltvUpperBound">LTV Upper Bound</param>
        /// <returns>Require Application Amount</returns>
        public int CalculateNewAppAmtWithLtvLimit(double applicationValue, double currentBalance, double valuation, int ltvLowerBound, int ltvUpperBound)
        {
            int ltv;
            int newValue = 0;

            double dblLTV = (currentBalance + applicationValue) / (valuation) * 100;
            double newLTV;
            ltv = Convert.ToInt32(dblLTV);

            if (ltv < ltvLowerBound || ltv > ltvUpperBound)
            {
                var randomLTV = new Random();
                ltv = randomLTV.Next(ltvLowerBound, ltvUpperBound);
                newLTV = ltv;
                applicationValue = (valuation * (newLTV / 100)) - currentBalance;
                newValue = Convert.ToInt32(Math.Round(applicationValue));
            }
            return newValue;
        }

        /// <summary>
        /// This method allows a user to rework a FL application.
        /// </summary>
        /// <param name="b">IE TestBrowser Object</param>
        /// <param name="offerKey">Offer.OfferKey</param>
        /// <param name="offerType">The Application Type (2,3,4)</param>
        /// <returns>The updated application amount (int)</returns>
        public int ReworkFLApplication(int offerKey, OfferTypeEnum offerType)
        {
            //first we get the current FL Application Amounts
            string amt;
            int intAmt = 0;

            switch (offerType)
            {
                case OfferTypeEnum.Readvance:
                    amt = base.ReadvanceRequired.Value;
                    if (string.IsNullOrEmpty(amt) || amt == "0")
                    {
                        amt = base.QualifyReadvance.Text.CleanCurrencyString(true);
                    }
                    intAmt = (Convert.ToInt32(amt)) - 100;
                    base.ReadvanceRequired.Value = intAmt.ToString();
                    break;

                case OfferTypeEnum.FurtherAdvance:
                    amt = base.FurtherAdvReq.Value;
                    if (string.IsNullOrEmpty(amt) || amt == "0")
                    {
                        amt = base.QualifyFurtherAdvance.Text.CleanCurrencyString(true);
                    }
                    intAmt = (Convert.ToInt32(amt)) - 100;
                    base.FurtherAdvReq.Value = intAmt.ToString();
                    break;

                case OfferTypeEnum.FurtherLoan:
                    amt = base.FurtherLoanReq.Value;
                    if (string.IsNullOrEmpty(amt) || amt == "0")
                    {
                        amt = base.QualifyFurtherLoan.Text.CleanCurrencyString(true);
                    }
                    intAmt = (Convert.ToInt32(amt)) - 100;
                    base.FurtherLoanReq.Value = intAmt.ToString();
                    break;
            }
            int newAppAmt = intAmt;
            SetEmploymentType();
            base.CalculateButton.Click();
            string income = base.HouseholdIncome.Text.CleanCurrencyString(true);
            //set the field
            base.NewIncome1.Value = income;
            //if the PTI warning is displayed we will need to increase the Household Income
            bool warning = CheckForPTIWarning();
            while (warning)
            {
                double newIncome = IncreaseHouseholdIncome(2, income);
                income = newIncome.ToString();
                base.CalculateButton.Click();
                warning = CheckForPTIWarning();
            }
            base.NextButton.Click();
            //check to see if it is VariFix. If it does we will need to click an extra [Next] button.
            QueryResults r = applicationService.GetLatestOfferInformationByOfferKey(offerKey);
            int productKey = r.Rows(0).Column("ProductKey").GetValueAs<int>();
            r.Dispose();
            if (productKey == (int)ProductEnum.VariFixLoan)
            {
                base.NextButton.Click();
            }
            //2nd tab
            VerifyConditions();
            base.NextButton.Click();
            //final tab
            base.GenerateButton.Click();
            //handle any warnings
            base.Document.Page<BasePage>().DomainWarningClickYes();

            return newAppAmt;
        }

        ///<summary>
        /// Populates the Further Lending Application Values from the Required values that appear on the FL Calculator
        ///</summary>
        ///<param name="b">IE TestBrowser Object</param>
        ///<param name="offerType">OfferType to Create</param>
        ///<returns></returns>
        public bool PopulateFLValuesFromRequiredValues(OfferTypeEnum offerType)
        {
            string amt;
            int intAmt;
            switch (offerType)
            {
                case OfferTypeEnum.Readvance:
                    amt = base.QualifyReadvance.Text.CleanCurrencyString(true);
                    intAmt = (Convert.ToInt32(amt)) - 100;
                    if (intAmt < 0)
                    {
                        return false;
                    }
                    base.ReadvanceRequired.Value = intAmt.ToString();
                    return true;

                case OfferTypeEnum.FurtherAdvance:
                    amt = base.QualifyFurtherAdvance.Text.CleanCurrencyString(true);
                    intAmt = (Convert.ToInt32(amt)) - 100;
                    if (intAmt < 0)
                    {
                        return false;
                    }
                    base.FurtherAdvReq.Value = intAmt.ToString();
                    return true;

                case OfferTypeEnum.FurtherLoan:
                    amt = base.QualifyFurtherLoan.Text.CleanCurrencyString(true);
                    intAmt = (Convert.ToInt32(amt)) - 100;
                    if (intAmt < 0)
                    {
                        return false;
                    }
                    base.FurtherLoanReq.Value = intAmt.ToString();
                    return true;
            }
            return false;
        }

        ///<summary>
        /// Clicks the Calculate button on the further lending calculator
        ///</summary>
        public FurtherLendingCalculator ClickCalculateAndConfirmWarnings()
        {
            base.CalculateButton.Click();
            base.Document.Page<BasePage>().DomainWarningClickYes();
            return this;
        }

        /// <summary>
        /// Clicks the Calculate button on the further lending calculator
        /// </summary>
        public FurtherLendingCalculator ClickCalculate()
        {
            base.CalculateButton.Click();
            return this;
        }

        /// <summary>
        /// Asserts that the next button is disable
        /// </summary>
        public void AssertNextButtonState(bool state)
        {
            Logger.LogAction("Asserting that NextButton.Enabled is set to false");
            Assert.That(base.NextButton.Enabled == state, "NextButton.Enabled was set to {0} when we expected it to be {1}", (!state).ToString(), state.ToString());
        }

        public FurtherLendingCalculator ClickNext()
        {
            base.NextButton.Click();
            return this;
        }

        public void GenerateApplication()
        {
            base.GenerateButton.Click();
        }

        public FurtherLendingCalculator PopulateFLValuesFromTestData(QueryResults row, int ltvLowerBound, int ltvUpperBound)
        {
            /*
             * populate all of the applications required
             * the value for applications that they do not qualify for should be set to -1 in the CSV file
             * we check the value first before populating the required mount
            */
            if (row.Rows(0).Column("ReadvanceAmount").GetValueAs<int>() != -1)
            {
                base.ReadvanceRequired.Value = row.Rows(0).Column("ReadvanceAmount").Value;
            }
            if (row.Rows(0).Column("FurtherAdvanceAmount").GetValueAs<int>() != -1)
            {
                int newApplicationValue = ltvLowerBound != 0.00 ?
                    newApplicationValue = CalculateNewAppAmtWithLtvLimit(row.Rows(0).Column("FurtherAdvanceAmount").GetValueAs<double>(),
                        row.Rows(0).Column("CurrentBalance").GetValueAs<double>(),
                        row.Rows(0).Column("ValuationAmount").GetValueAs<double>(),
                        ltvLowerBound, ltvUpperBound)
                        : 0;
                string advanceAmount = newApplicationValue > 0 ? newApplicationValue.ToString() : row.Rows(0).Column("FurtherAdvanceAmount").Value;
                base.FurtherAdvReq.Value = advanceAmount;
            }
            if (row.Rows(0).Column("FurtherLoanAmount").GetValueAs<int>() != -1)
            {
                base.FurtherLoanReq.Value = row.Rows(0).Column("FurtherLoanAmount").Value;
            }
            SetEmploymentType();
            string householdIncome = "100000";
            base.NewIncome1.Value = householdIncome;
            return this;
        }

        public FurtherLendingCalculator IncludeNaedoForms()
        {
            base.IncludeNaedoForm.Checked = true;
            return this;
        }
    }
}