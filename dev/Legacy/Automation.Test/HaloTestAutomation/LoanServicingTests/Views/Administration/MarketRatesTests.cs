using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO;
using BuildingBlocks.Presenters.Admin;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace LoanServicingTests.Views.Administration
{
    [RequiresSTA]
    public class MarketRateTests : TestBase<MarketRatesView>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
            base.Browser.Navigate<AdministrationActions>().UpdateMarketRates();
        }

        protected override void OnTestFixtureTearDown()
        {
            base.OnTestFixtureTearDown();
            Service<ICommonService>().SyncMarketRates();
        }

        /// <summary>
        /// This test ensures that the correct values for the market rates are retrieved from the database and displayed on the screen.
        /// </summary>
        [Test, Sequential, Description("This test ensures that the correct values for the market rates are retrieved from the database and displayed on the screen.")]
        public void _001_VerifyMarketRates([Values(
                                                    MarketRateEnum._3MonthJibar_Rounded,
                                                    MarketRateEnum._3MonthJibar,
                                                    MarketRateEnum._20YearFixedMortgageRate,
                                                    MarketRateEnum._5YearResetFixedMortgageRate,
                                                    MarketRateEnum.RepoRate,
                                                    MarketRateEnum.PrimeLendingRate
                                                    )] MarketRateEnum marketRateDescription)
        {
            base.View.SelectMarketRate(((int)marketRateDescription).ToString());
            string marketRateValue = base.View.GetMarketRateTextField();
            MarketRatesAssertions.AssertMarketRateValue(Double.Parse(marketRateValue), marketRateDescription);
        }

        /// <summary>
        /// This test will update each of the market rates and then check that the updated value has been committed to the database. In the case of the JIBAR91DayDiscount21
        /// and JIBARNacmYieldrounded21 market rates, the test also ensures that the related 18th reset rate is also updated via the database trigger.
        /// </summary>
        /// <param name="marketRateDescription"></param>
        [Test, Sequential, Description(@"This test will update each of the market rates and then check that the updated value has been committed to the database. In the case of the JIBAR91DayDiscount21
		and JIBARNacmYieldrounded21 market rates, the test also ensures that the related 18th reset rate is also updated via the database trigger.")]
        public void _002_UpdateMarketRates([Values(
                                                    MarketRateEnum._3MonthJibar_Rounded,
                                                    MarketRateEnum._3MonthJibar,
                                                    MarketRateEnum._20YearFixedMortgageRate,
                                                    MarketRateEnum._5YearResetFixedMortgageRate,
                                                    MarketRateEnum.RepoRate,
                                                    MarketRateEnum.PrimeLendingRate
                                                    )] MarketRateEnum marketRateDescription)
        {
            base.Browser.Navigate<AdministrationActions>().UpdateMarketRates();
            base.View.SelectMarketRate(((int)marketRateDescription).ToString());
            string marketRateValue = base.View.GetMarketRateTextField();
            double newRate = base.View.UpdateMarketRate(marketRateValue);
            MarketRatesAssertions.AssertMarketRateValue(newRate, marketRateDescription);
        }
    }
}