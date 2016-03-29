using BuildingBlocks;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LoanServicingTests.Views.HOC
{
    [RequiresSTA]
    public sealed class HOCUpdateTests : TestBase<HOCFSSummaryUpdate>
    {
        #region PrivateVariables

        private Automation.DataModels.Account Account;

        private int _legalEntityKey;

        #endregion PrivateVariables

        #region SetupTearDown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser, TestUsers.Password);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
        }

        #endregion SetupTearDown

        #region Tests

        /// <summary>
        /// Tests that the Update HOC screen loads correctly.
        /// </summary>
        [Test, Description("Tests that the Update HOC screen loads correctly.")]
        public void UpdatePresenterLoadedForSAHLHOC()
        {
            var HOC = GetTestCase(true, false);
            base.Browser.Navigate<LoanServicingCBO>().UpdateHOCDetails();
            base.View.AssertViewLoadedSAHLHOC();
        }

        /// <summary>
        /// Tests that the Update HOC screen loads correctly.
        /// </summary>
        [Test, Description("Tests that the Update HOC screen loads correctly.")]
        public void UpdatePresenterLoadedForOtherInsurers()
        {
            var HOC = GetTestCase(false, false);
            base.Browser.Navigate<LoanServicingCBO>().UpdateHOCDetails();
            base.View.AssertViewLoadedOtherInsurers();
        }

        /// <summary>
        /// When saving HOC and the associated valuation is older than 2 years, a warning should be displayed to the user.
        /// </summary>
        [Test, Description("When saving HOC and the associated valuation is older than 2 years, a warning should be displayed to the user.")]
        public void ValuationOlderThan2Years()
        {
            var HOC = GetTestCase(false, true);
            base.Browser.Navigate<LoanServicingCBO>().UpdateHOCDetails();
            base.View.ClickUpdate();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please note that the current valuation is older than 2 years.");
            base.Browser.Page<BasePage>().DomainWarningClickYes();
        }

        /// <summary>
        /// Test ensures that all of the mandatory fields are supplied when updating HOC details for a SAHL HOC Insurer.
        /// </summary>
        [Test, Description("Test ensures that all of the mandatory fields are supplied when updating HOC details for a SAHL HOC Insurer.")]
        public void MandatoryFieldsSAHLHOC()
        {
            var HOC = GetTestCase(true, false);
            base.Browser.Navigate<LoanServicingCBO>().UpdateHOCDetails();
            base.View.SetHOCInsurer("-select-");
            base.View.ClickUpdate();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please select a HOC Insurer.");
        }

        /// <summary>
        /// Test ensures that all of the mandatory fields are supplied when updating HOC details for a non-SAHL HOC Insurer.
        /// </summary>
        [Test, Description("Test ensures that all of the mandatory fields are supplied when updating HOC details for a non-SAHL HOC Insurer.")]
        public void MandatoryFieldsNonSAHLHOC()
        {
            var HOC = GetTestCase(false, false, new List<TitleTypeEnum> { TitleTypeEnum.Freehold, TitleTypeEnum.Unknown });
            base.Browser.Navigate<LoanServicingCBO>().UpdateHOCDetails();
            base.View.SetPolicyNumber(string.Empty);
            base.View.ClickUpdate();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("HOC PolicyNumber must not be null.");
        }

        /// <summary>
        /// Should the new sum insured be less than the previous value then a warning is displayed to the user prior to the update happening.
        /// </summary>
        [Test, Description("Should the new sum insured be less than the previous value then a warning is displayed to the user prior to the update happening.")]
        public void NewSumInsuredLessThanPreviousValue()
        {
            var HOC = GetTestCase(false, false, new List<TitleTypeEnum> { TitleTypeEnum.Freehold, TitleTypeEnum.Unknown });
            base.Browser.Navigate<LoanServicingCBO>().UpdateHOCDetails();
            double newHOCAmt = HOC.HOCTotalSumInsured - 5000;
            base.View.UpdateSumInsured(newHOCAmt);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                string.Format(@"New HOC SumInsured is less than previous value of R {0}.", HOC.HOCTotalSumInsured));
            base.Browser.Page<BasePage>().DomainWarningClickYes();
            //fetch our HOC Account
            HOC = base.Service<IHOCService>().GetHOCAccountDetails(HOC.AccountKey);
            //ensure that is has been updated to reflect the new value
            Assert.That(HOC.HOCTotalSumInsured == newHOCAmt,
                string.Format(@"HOC Total Sum Insured was expected to be {0} but was {1}", newHOCAmt, HOC.HOCTotalSumInsured));
        }

        /// <summary>
        /// You should be able to change the value of the HOC Policy Ceded indicator on the HOC record.
        /// </summary>
        [Test, Description("You should be able to change the value of the HOC Policy Ceded indicator on the HOC record.")]
        public void UpdateHOCPolicyCededNonSAHLInsurer()
        {
            var HOC = GetTestCase(false, false, new List<TitleTypeEnum> { TitleTypeEnum.Freehold, TitleTypeEnum.Unknown });
            base.Browser.Navigate<LoanServicingCBO>().UpdateHOCDetails();
            //update ceded value
            bool ceded = base.View.UpdateCededValue();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Are you sure policy ceded status is correct ?");
            base.Browser.Page<BasePage>().DomainWarningClickYes();
            //fetch our HOC Account
            HOC = base.Service<IHOCService>().GetHOCAccountDetails(HOC.AccountKey);
            //ensure that is has been updated to reflect the new value
            Assert.That(HOC.Ceded == ceded, string.Format(@"HOC Ceded value was expected to be {0} but was {1}", ceded.ToString(), HOC.Ceded.ToString()));
        }

        /// <summary>
        /// Changing the status of the HOC record to Paid Up with no HOC should also change the insurer.
        /// </summary>
        [Test, Description("Changing the status of the HOC record to Paid Up with no HOC should also change the insurer.")]
        public void UpdateToPaidUpWithHOC()
        {
            var HOC = GetTestCase(true, false, new List<TitleTypeEnum> { TitleTypeEnum.Freehold, TitleTypeEnum.Unknown });
            base.Browser.Navigate<LoanServicingCBO>().UpdateHOCDetails();
            base.View.SetHOCStatus(HOCStatusEnum.PaidUpwithnoHOC);
            base.View.AssertHOCInsurer(HOCInsurerEnum.PaidupwithnoHOC);
            base.View.ClickUpdate();
            base.Browser.Page<BasePage>().DomainWarningClickYes();
            //fetch our HOC Account
            HOC = base.Service<IHOCService>().GetHOCAccountDetails(HOC.AccountKey);
            //check for our changes
            Assert.That(HOC.HOCStatusKey == (int)HOCStatusEnum.PaidUpwithnoHOC && HOC.HOCInsurerKey == (int)HOCInsurerEnum.PaidupwithnoHOC,
                string.Format(@"HOCStatusKey was {0}, expected {1}, HOCInsurerKey was {2}, expected {3}",
                HOC.HOCStatusKey, (int)HOCStatusEnum.PaidUpwithnoHOC, HOC.HOCInsurerKey, (int)HOCInsurerEnum.PaidupwithnoHOC));
        }

        /// <summary>
        /// Changing the status of the HOC record to Closed should also change the insurer.
        /// </summary>
        [Test, Description("Changing the status of the HOC record to Closed should also change the insurer.")]
        public void UpdateToClosed()
        {
            var HOC = GetTestCase(true, false, new List<TitleTypeEnum> { TitleTypeEnum.Freehold, TitleTypeEnum.Unknown });
            base.Browser.Navigate<LoanServicingCBO>().UpdateHOCDetails();
            base.View.SetHOCStatus(HOCStatusEnum.Closed);
            base.View.AssertHOCInsurer(HOCInsurerEnum.LoanCancelledClosed);
            base.View.ClickUpdate();
            base.Browser.Page<BasePage>().DomainWarningClickYes();
            //fetch our HOC Account
            HOC = base.Service<IHOCService>().GetHOCAccountDetails(HOC.AccountKey);
            //check for our changes
            Assert.That(HOC.HOCStatusKey == (int)HOCStatusEnum.Closed && HOC.HOCInsurerKey == (int)HOCInsurerEnum.LoanCancelledClosed,
                string.Format(@"HOCStatusKey was {0}, expected {1}, HOCInsurerKey was {2}, expected {3}",
                HOC.HOCStatusKey, (int)HOCStatusEnum.Closed, HOC.HOCInsurerKey, (int)HOCInsurerEnum.LoanCancelledClosed));
        }

        /// <summary>
        /// When updating HOC on open account where the HOCNoHOC detail type is loaded the update screen should show an error notifying the user
        /// that the detail type needs to be removed.
        /// </summary>
        [Test, Description(@"When updating HOC on open account where the HOCNoHOC detail type is loaded the update screen should show an error notifying the user
        that the detail type needs to be removed.")]
        public void UpdateWithHOCNoHOCDetailTypeLoaded()
        {
            var HOC = GetTestCase(true, false, new List<TitleTypeEnum> { TitleTypeEnum.Freehold, TitleTypeEnum.Unknown });
            base.Browser.Navigate<LoanServicingCBO>().UpdateHOCDetails();
            //insert the detail type
            Service<IDetailTypeService>().InsertDetailType(DetailTypeEnum.HOCNoHOC, Account.AccountKey);
            base.View.ClickUpdate();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                "Please remove the detail type 'HOC - NO HOC' before proceeding to change the HOC Insurer to 'SAHL HOC'");
        }

        /// <summary>
        /// This method tests that when updating the HOC to SAHL HOC on an open account which has a detail type of either 'HOC – Cession of HOC',
        /// or 'HOC - Cession - Commercial Use', an error message is displayed to the user stating “Please remove the detail type 'HOC – <detail type>'.
        /// Once the detail type is removed, the insurer can be updated to SAHL HOC.
        /// </summary>
        [Test, Description(@"This method tests that when updating the HOC to SAHL HOC on an open account which has a detail type of either 'HOC – Cession of HOC', or 'HOC - Cession - Commercial Use', an error message is displayed to the user stating “Please remove the detail type 'HOC – <detail type>'. Once the detail type is removed, the insurer can be updated to SAHL HOC.")]
        public void UpdateToSAHLHOC()
        {
            List<DetailTypeEnum> detailTypes = new List<DetailTypeEnum>
                {
                    DetailTypeEnum.HOCCessionOfPolicy,
                    DetailTypeEnum.HOCCessionCommercialUse
                };

            foreach (DetailTypeEnum dt in detailTypes)
            {
                // Get an HOC record with a detail type of 'HOC Cession Of Policy' and navigate to the Account
                var HOC = GetNonSAHLAccountWithDetailType(dt);
                base.Browser.Navigate<LoanServicingCBO>().UpdateHOCDetails();

                // Set the HOC Insurer to SAHL and click the update button
                base.View.SetHOCInsurer(Convert.ToString((int)HOCInsurerEnum.SAHLHOC));
                base.View.ClickUpdate();

                // Assert that the correct validation message is displayed
                string hocType = "HOC - Cession - Commercial Use";
                if (dt.ToString() == "HOCCessionOfPolicy") { hocType = "HOC - Cession Of Policy"; }
                string message = String.Format("Please remove the detail type '{0}' before proceeding to change the HOC Insurer to 'SAHL HOC'", hocType);
                base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains(message);

                // Remove all the detail types and navigate to the Account
                Service<IDetailTypeService>().RemoveDetailTypes(Account.AccountKey);

                // Re-update the Insurer to SAHL HOC
                base.View.ClickUpdate();
                base.Browser.Page<BasePage>().DomainWarningClickYes();

                // Check that SAHL HOC is now set as the Insurer
                HOC = base.Service<IHOCService>().GetHOCAccountDetails(0, Account.AccountKey);
                Assert.That(HOC.HOCInsurerKey == (int)HOCInsurerEnum.SAHLHOC);
                base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            }
        }

        #endregion Tests

        #region Helper

        /// <summary>
        /// Gets the test cases for each of the tests.
        /// </summary>
        /// <param name="SAHLAsInsurer">TRUE = Require SAHL HOC as the insurer for the policy, FALSE = other insurer.</param>
        /// <param name="valOlderThan2Yrs">TRUE = we need an HOC record with a val older than 2 years</param>
        /// <returns>HOCAccount Model</returns>
        private Automation.DataModels.HOCAccount GetTestCase(bool SAHLAsInsurer, bool valOlderThan2Yrs, List<TitleTypeEnum> titleTypes = null)
        {
            Automation.DataModels.HOCAccount HOC = null;
            while (HOC == null)
            {
                //fetch a mortgage loan
                Account = base.Service<IAccountService>().GetVariableLoanAccountByMainApplicantCount(1, 1, AccountStatusEnum.Open);
                //fetch open HOC
                HOC = base.Service<IHOCService>().GetHOCAccountDetails(0, Account.AccountKey);
                //check the insurer
                if (SAHLAsInsurer && HOC.HOCInsurerKey != (int)HOCInsurerEnum.SAHLHOC)
                {
                    HOC = null;
                    continue;
                }
                else if (!SAHLAsInsurer && HOC.HOCInsurerKey == (int)HOCInsurerEnum.SAHLHOC)
                {
                    HOC = null;
                    continue;
                }
                //check if we require a case with a val older than 2 years
                if (valOlderThan2Yrs)
                {
                    //if null then leave here
                    if (HOC.ValuationDate == null)
                    {
                        HOC = null;
                        continue;
                    }
                    else
                    {
                        int result = DateTime.Compare(HOC.ValuationDate.AddYears(2), DateTime.Now);
                        HOC = result < 0 ? HOC : null;
                    }
                }
                //check the title type required.
                if (titleTypes != null)
                {
                    if (!titleTypes.Contains(HOC.PropertyDetails.TitleTypeKey))
                    {
                        HOC = null;
                        continue;
                    }
                }
            }
            _legalEntityKey = base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(Account.AccountKey);
            //remove detail types
            Service<IDetailTypeService>().RemoveDetailTypes(Account.AccountKey, new List<DetailTypeEnum>() { DetailTypeEnum.HOCNoHOC });
            //we need the HOC Account
            base.Browser.Navigate<LoanServicingCBO>().HomeOwnersCoverNode(HOC.AccountKey);
            return HOC;
        }

        /// <summary>
        /// Gets the test cases, based on detail type
        /// </summary>
        /// <param name="DetailType"></param>
        /// <returns></returns>
        private Automation.DataModels.HOCAccount GetNonSAHLAccountWithDetailType(DetailTypeEnum detailType)
        {
            Automation.DataModels.HOCAccount HOC = null;
            while (HOC == null)
            {
                // Fetch a mortgage loan and open HOC
                Account = base.Service<IAccountService>().GetNonSAHLAccountWithDetailType(detailType);
                HOC = base.Service<IHOCService>().GetHOCAccountDetails(0, Account.AccountKey);
            }
            _legalEntityKey = base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(Account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().HomeOwnersCoverNode(HOC.AccountKey);
            return HOC;
        }

        #endregion Helper
    }
}