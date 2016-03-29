using BuildingBlocks.Services.Contracts;
using Common.Extensions;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using System.Collections.Generic;

namespace BuildingBlocks.Presenters.LoanServicing.RateChange
{
    public class RateChange : RateChangeControls
    {
        private readonly IWatiNService watinService;

        public RateChange()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Checks that the correct link rates are displayed in the drop down list
        /// </summary>
        /// <param name="linkRates"></param>
        public void AssertLinkRates(List<int> linkRates)
        {
            Assertions.WatiNAssertions.AssertSelectListContents(base.NewLinkRate, linkRates, true);
        }

        /// <summary>
        /// Selects a new link rate by value
        /// </summary>
        /// <param name="marginKey"></param>
        public void ChangeLinkRate(int marginKey)
        {
            base.NewLinkRate.SelectByValue(marginKey.ToString());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="linkRate"></param>
        /// <param name="discount"></param>
        public void VerifyVariableRates(decimal rate, decimal discount, decimal linkRate)
        {
            discount = Math.Round(discount * 100, 2);
            rate = Math.Round(rate * 100, 2);
            linkRate = Math.Round(linkRate * 100, 2);
            var newRate = (rate + linkRate) + discount;
            var marketRate = base.MarketRateVariable.Text.CleanPercentageString(false);
            var interestRate = base.NewInterestRateVariable.Text.CleanPercentageString(false);
            Assert.That(rate == Convert.ToDecimal(marketRate) && newRate == Convert.ToDecimal(interestRate));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="linkRate"></param>
        /// <param name="discount"></param>
        public void VerifyFixedRates(decimal rate, decimal discount, decimal linkRate)
        {
            discount = Math.Round(discount * 100, 2);
            rate = Math.Round(rate * 100, 2);
            linkRate = Math.Round(linkRate * 100, 2);
            var newRate = (rate + linkRate) + discount;
            var marketRate = base.MarketRateFixed.Text.CleanPercentageString(false);
            var interestRate = base.NewInterestRateFixed.Text.CleanPercentageString(false);
            Assert.That(rate == Convert.ToDecimal(marketRate) && newRate == Convert.ToDecimal(interestRate));
        }

        /// <summary>
        /// Confirm the rate change
        /// </summary>
        public void ClickChangeRate()
        {
            watinService.HandleConfirmationPopup(base.SubmitButton);
        }
    }
}