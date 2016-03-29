using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using System.Linq;
using WatiN.Core.Logging;

namespace BuildingBlocks.Presenters.Origination
{
    public class ApplicationLoanDetailsUpdate : ApplicationLoanDetailsUpdateControls
    {
        private readonly ICommonService commonService;

        public ApplicationLoanDetailsUpdate()
        {
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
        }

        /// <summary>
        /// This is an initial method for the Update Loan Details screen for the Application Capture test. It will need to
        /// be extended in the future when more complex tests are needed.
        /// </summary>
        /// <param name="ExistingLoanValue">A new Existing Loan value</param>
        /// <param name="CashOutValue">A new Cash Out value</param>
        public void UpdateSwitchLoanDetails(string ExistingLoanValue, string CashOutValue)
        {
            //Change the existing loan
            if (ExistingLoanValue != null) base.SwitchExistingLoanRandValue.TypeText(ExistingLoanValue);
            //change the cash out value
            if (CashOutValue != null) base.SwitchCashOutRandValue.TypeText(CashOutValue);
            //press the Recalc button
            base.RecalculateButton.Click();
            //click the save button
            base.UpdateButton.Click();
        }

        /// <summary>
        /// This method will take an application and apply a surcharge/discount to the application's link rate by
        /// adding a Discounted Link Rate rate override type to the application.
        /// </summary>
        /// <param name="NewDiscountValue">New value for the discount/surcharge (+ or -)</param>
        public void ApplyDiscountedLinkRate(double NewDiscountValue)
        {
            //select the discounted link rate, if it is currently selected then do not select it.
            if (base.DiscountedLinkRateAttribute.Checked == false)
            {
                base.DiscountedLinkRateAttribute.Click();
            }
            //we need to get the current discount in order to apply the new one correctly.
            double current_discount = 0.0;
            current_discount = Convert.ToDouble(base.DiscountSurcharge.Value);
            //the difference between the new required value and the current one is the no. of times we click
            double difference = NewDiscountValue - current_discount;
            int num_clicks = Convert.ToInt32(Math.Abs(difference) / 0.1);
            //need to repeat this a number of times to apply the correct discount/surcharge
            for (int i = 0; i < num_clicks; i++)
            {
                if (current_discount > NewDiscountValue)
                {
                    base.MainDiscountDown.Click();
                }
                else
                {
                    base.MainDiscountUp.Click();
                }
            }
            //press the recalculate button
            base.RecalculateButton.Click();
            //save the updated details
            base.UpdateButton.Click();
        }

        /// <summary>
        /// This will apply/remove the Interest Only option on an application
        /// </summary>
        /// <param name="Set">True = Apply Interest Only, False = Remove Interest Only</param>
        public void UpdateInterestOnlyAttribute(bool Set)
        {
            if (Set && !base.InterestOnlyAttribute.Checked)
            {
                base.InterestOnlyAttribute.Click();
            }
            else if (!Set && base.InterestOnlyAttribute.Checked)
            {
                base.InterestOnlyAttribute.Click();
            }
            //recalc
            base.RecalculateButton.Click();
            //save
            base.UpdateButton.Click();
        }

        /// <summary>
        /// Selects/Removes the Override Fees attribute from an application. When selecting the attribute a new cancellation
        /// should be provided.
        /// </summary>
        /// <param name="NewCancellationFee">New Cancellation Fee value</param>
        /// <param name="Set">True = Override Fees, False = Remove Override Fees</param>
        public void OverrideCancellationFee(string NewCancellationFee, bool Set)
        {
            if (Set && !base.OverrideFeesAttribute.Checked)
            {
                base.OverrideFeesAttribute.Click();
            }
            else if (!Set && base.OverrideFeesAttribute.Checked)
            {
                base.OverrideFeesAttribute.Click();
            }
            //enter a new cancellation fee
            if (Set)
            {
                base.CancellationFeeRandValue.TypeText(NewCancellationFee);
                base.CancellationFeeCentsValue.TypeText("00");
            }
            //recalculate
            base.RecalculateButton.Click();
            //save
            base.UpdateButton.Click();
        }

        /// <summary>
        /// Changes the current product of the Application
        /// </summary>
        /// <param name="Product">New Product</param>
        public void ChangeApplicationProduct(string Product)
        {
            if (base.ProductDropdown.SelectedOption.Text != Product)
            {
                //change the product
                base.ProductDropdown.Option(Product).Select();
                RecalculateApplication();
                switch (Product)
                {
                    case "VariFix Loan":
                        ChangeTerm("240");
                        ChangeVariFixPercentage("50");
                        RecalculateApplication();
                        break;
                }
                SaveApplication();
            }
        }

        /// <summary>
        /// Changes the term value on the application
        /// </summary>
        /// <param name="Term">New Term Value</param>
        public void ChangeTerm(string Term)
        {
            //update the term value
            base.Term.TypeText(Term);
        }

        /// <summary>
        /// Recalculates and Saves the Application
        /// </summary>
        public void RecalcAndSave(bool capitaliseFees = true)
        {
            if (base.CapitaliseFeesAttribute.Checked != capitaliseFees)
            {
                base.CapitaliseFeesAttribute.Checked = capitaliseFees;
            }
            //recalculate
            base.RecalculateButton.Click();
            //save
            base.UpdateButton.Click();
        }

        /// <summary>
        /// Recalculates the application only
        /// </summary>
        public void RecalculateApplication()
        {
            //recalculate
            base.RecalculateButton.Click();
        }

        /// <summary>
        /// Saves the application only
        /// </summary>
        public void SaveApplication()
        {
            //save
            base.UpdateButton.Click();
            base.Document.Page<BasePage>().DomainWarningClickYes();
        }

        /// <summary>
        /// Changes the VariFix Fixed Percentage value
        /// </summary>
        /// <param name="Percent">New Fixed Percent</param>
        public void ChangeVariFixPercentage(string Percent)
        {
            base.VariFixFixedPercentage.TypeText(Percent);
        }

        /// <summary>
        /// Changes the Refinance's Cash Out value. Supplying a string containing a '.' will seperate the cash out value into rands and cents.
        /// </summary>
        /// <param name="CashOutValue">The new Cash Out value</param>
        public void ChangeRefinanceCashOutValue(string CashOutValue)
        {
            if (CashOutValue.IndexOf('.') > 0)
            {
                string RandValue;
                string CentsValue;
                commonService.SplitRandsCents(out RandValue, out CentsValue, CashOutValue);
                base.RefinanceCashOutRandValue.TypeText(RandValue);
                base.RefinanceCashOutCentValue.TypeText(CentsValue);
            }
            else
            {
                base.RefinanceCashOutRandValue.TypeText(CashOutValue);
            }
        }

        /// <summary>
        /// Reworks the current Product without saving the change
        /// </summary>
        /// <param name="Product">New Product</param>
        public void ReworkProduct(string Product)
        {
            //rework the product
            base.ProductDropdown.Option(Product).Select();
        }

        /// <summary>
        /// Retrieve the value from the Term text field off of the Application Loan Details Update screen
        /// </summary>
        /// <returns>Term Value</returns>
        public int CurrentTerm()
        {
            int Term;
            string sTerm = base.Term.Value;
            Term = Convert.ToInt32(sTerm);
            return Term;
        }

        /// <summary>
        /// Checks that the ExpectedTerm value is equal to the actual value in the Term text field on the Application Loan Details Update screen
        /// </summary>
        /// <param name="expectedTerm">New Expected Term</param>
        /// <param name="browser">IE TestBrowser</param>
        public void AssertTermValue(int expectedTerm)
        {
            int actualTerm = CurrentTerm();
            Logger.LogAction(String.Format("Asserting that that the term value is the expected value"));
            Assert.AreEqual(expectedTerm, actualTerm, "Term value is not the expected value of: " + expectedTerm);
        }

        public void Populate(bool isStaffLoan)
        {
            base.StaffLoan.Checked = isStaffLoan;
        }

        public System.Collections.Generic.IEnumerable<string> GetProductsFromDropDownList()
        {
            return (from option in base.ProductDropdown.Options
                    where !option.Text.Contains("select")
                    select option.Text).ToList();
        }

        public void CapitaliseInitiationFees(bool initiationFeesAreCapitalised)
        {
            base.CapitaliseInitiationFees.Checked = initiationFeesAreCapitalised;
        }

        public int GetTotalLoanRequired()
        {
            return Convert.ToInt32(commonService.ConvertCurrencyStringToInt(base.TotalLoanRequired.Text));
        }

        public int GetInitiationFee()
        {
            return Convert.ToInt32(commonService.ConvertCurrencyStringToInt(base.InitiationFeeSpan.Text));
        }
    }
}