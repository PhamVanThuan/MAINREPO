using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using System;

namespace BuildingBlocks.Assertions
{
    /// <summary>
    /// Assertions related to the Market Rates View
    /// </summary>
    public static class MarketRatesAssertions
    {
        private static ICommonService commonService;

        static MarketRatesAssertions()
        {
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="expectedMarketRateValue"></param>
        /// <param name="mRate"></param>
        public static void AssertMarketRateValue(double expectedMarketRateValue, MarketRateEnum mRate)
        {
            double marketRate = commonService.GetMarketRate(mRate);
            //get the value back
            Assert.AreEqual(Math.Round((expectedMarketRateValue / 100), 5), Math.Round(marketRate), 5);
        }
    }
}