using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.Admin
{
    public class MarketRatesView : MarketRatesViewControls
    {
        /// <summary>
        /// Select a market rate from the grid
        /// </summary>
        /// <param name="marketRate"></param>
        /// <param name="b"></param>
        public void SelectMarketRate(string marketRate)
        {
            base.gridSelectMarketRate(marketRate).Click();
        }

        /// <summary>
        /// Returns the value of the Market Rate Value Label
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public string GetMarketRateValueLabel()
        {
            return base.lblMarketRateValue.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public string GetMarketRateTextField()
        {
            return base.MarketRateValue.Value.ToString();
        }

        /// <summary>
        /// Updates the market rate to reduce it by 0.2
        /// </summary>
        /// <param name="marketRate"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public double UpdateMarketRate(string marketRate)
        {
            string mRate = GetMarketRateTextField();
            double newRate = Convert.ToDouble(mRate);
            newRate = newRate - 0.2;
            //enter the new value
            base.MarketRateValue.Clear();
            base.MarketRateValue.Value = newRate.ToString();
            //click the button
            base.Update.Click();
            return newRate;
        }

        /// <summary>
        /// Returns TRUE if enabled or FALSE if disabled
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool MarketRateEnabled()
        {
            return base.MarketRateValue.Enabled;
        }
    }
}