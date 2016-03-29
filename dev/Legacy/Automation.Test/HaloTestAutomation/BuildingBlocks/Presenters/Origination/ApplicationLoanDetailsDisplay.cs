using Common.Constants;
using Common.Extensions;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using WatiN.Core.Logging;

namespace BuildingBlocks.Presenters.Origination
{
    public class ApplicationLoanDetailsDisplay : ApplicationLoanDetailsDisplayControls
    {
        public void AssertSwitchLoanDetailsUpdate(string cashOut, string existingLoan)
        {
            //get the existing loan value
            string actExistingLoan = base.lblSwitchExistingLoan.Text;
            string actCashOut = base.lblSwitchCashOut.Text;
            //clean up the strings for comparison
            actCashOut = actCashOut.CleanCurrencyString(true); ;
            actExistingLoan = actExistingLoan.CleanCurrencyString(true);
            //run the assertions
            Logger.LogAction("Asserting that the Existing Loan has been updated");
            StringAssert.AreEqualIgnoringCase(existingLoan, actExistingLoan);
            Logger.LogAction("Asserting that the Cash Out Value has been updated");
            StringAssert.AreEqualIgnoringCase(cashOut, actCashOut);
        }

        /// <summary>
        /// Checks the Latest Valuation amount displayed on the ApplicationLoanDetailsDisplay presenter equals the decimal value passed to the method
        /// </summary>
        /// <param name="latestValuation">MarketValue or Valuation.ValuationAmount</param>
        public void AssertLatestValuationDisplayedValue(decimal latestValuation)
        {
            string actualValue = base.lblPropertyValue.Text;
            string expectedValue = latestValuation.ToString(Formats.CurrencyFormat);
            Logger.LogAction("Expected Valuation Value: {0}", expectedValue);

            Logger.LogAction("Asserting that the Latest Valuation amount displayed on the ApplicationLoanDetailsDisplay presenter is {0}", actualValue);
            StringAssert.AreEqualIgnoringCase(expectedValue, actualValue, "The actaul Latest Valuation displayed on the ApplicationLoanDetailsDisplay presenter differs from the expected value");
        }

        /// <summary>
        /// Checks the LTV amount displayed on the ApplicationLoanDetailsDisplay presenter equals the (Lastest Valuation) decimal value passed to the method,
        /// divided by the LoanAgreementAmount retrieved from the latest OfferInformationVariableLoan record for the Offer
        /// </summary>
        /// <param name="offerkey">Offer.OfferKey</param>
        /// <param name="latestValuation">MarketValue or Valuation.ValuationAmount</param>
        public void AssertLTVDisplayedValue(int offerkey, decimal latestValuation, decimal loanAgreementAmount)
        {
            string actualValue = base.lblLTV.Text;
            decimal calculatedValue = Math.Round(loanAgreementAmount / latestValuation, 4);
            string expectedValue = calculatedValue.ToString("LTV = ##0.00%");
            Logger.LogAction(expectedValue);
            Logger.LogAction("Asserting that the LTV amount displayed on the ApplicationLoanDetailsDisplay presenter is {0}", actualValue);
            StringAssert.AreEqualIgnoringCase(expectedValue, actualValue, "The actaul LTV displayed on the ApplicationLoanDetailsDisplay presenter differs from the expected value");
        }
    }
}