using BuildingBlocks.Navigation;
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
    public class MarketingSourceMaintenanceTests : BuildingBlocks.TestBase<MarketingSourceAddView>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new BuildingBlocks.TestBrowser(TestUsers.HaloUser);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser.Navigate<MenuNode>().AdministrationNode();
        }

        [Test]
        public void when_adding_a_new_marketing_source_it_should_be_added_to_the_database()
        {
            var random = new Random();
            base.Browser.Navigate<AdministrationActions>().AddMarketingSource();
            string newMarketingSource = string.Format(@"Test Marketing Source {0}", random.Next(0, 100).ToString());
            base.View.AddMarketingSource(newMarketingSource);
            Automation.DataModels.MarketingSource marketingSource = Service<ICommonService>().GetMarketingSourceByDescriptionAndStatus(newMarketingSource, GeneralStatusEnum.Active);
            Assert.That(newMarketingSource != null);
        }

        [Test]
        public void when_updating_a_marketing_source_the_name_can_be_changed()
        {
            Automation.DataModels.MarketingSource marketingSource = Service<ICommonService>().GetMarketingSourceByStatus(GeneralStatusEnum.Active);
            base.Browser.Navigate<AdministrationActions>().UpdateMarketingSource();
            base.View.SelectMarketingSource(marketingSource.Description);
            string newDescription = "New Description";
            base.View.UpdateMarketingSource(newDescription, GeneralStatusConst.Active);
            var updatedMarketingSource = Service<ICommonService>().GetMarketingSourceByDescriptionAndStatus(newDescription, GeneralStatusEnum.Active);
            Assert.That(updatedMarketingSource != null);
        }

        [Test]
        public void when_updating_a_marketing_source_the_status_can_be_changed()
        {
            Automation.DataModels.MarketingSource marketingSource = Service<ICommonService>().GetMarketingSourceByStatus(GeneralStatusEnum.Active);
            base.Browser.Navigate<AdministrationActions>().UpdateMarketingSource();
            base.View.SelectMarketingSource(marketingSource.Description);
            base.View.UpdateMarketingSource(string.Empty, GeneralStatusConst.InActive);
            var updatedMarketingSource = Service<ICommonService>().GetMarketingSourceByDescriptionAndStatus(marketingSource.Description, GeneralStatusEnum.Inactive);
            Assert.That(updatedMarketingSource != null);
        }

        [Test]
        public void when_adding_a_new_marketing_source_with_no_description_a_validation_warning_is_received()
        {
            base.Browser.Navigate<AdministrationActions>().AddMarketingSource();
            base.View.ClickSubmit();
            base.View.AssertValidationDisplayed();
        }

        [Test]
        public void when_updating_a_marketing_source_and_the_description_is_removed_a_validation_warning_is_received()
        {
            base.Browser.Navigate<AdministrationActions>().UpdateMarketingSource();
            base.View.ClearMarketingSourceDescription();
            base.View.ClickSubmit();
            base.View.AssertValidationDisplayed();
        }

        [Test]
        public void when_adding_an_existing_marketing_source_a_validation_warning_is_displayed()
        {
            Automation.DataModels.MarketingSource marketingSource = Service<ICommonService>().GetMarketingSourceByStatus(GeneralStatusEnum.Active);
            base.Browser.Navigate<AdministrationActions>().AddMarketingSource();
            base.View.AddMarketingSource(marketingSource.Description);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains("already exists");
        }
    }
}