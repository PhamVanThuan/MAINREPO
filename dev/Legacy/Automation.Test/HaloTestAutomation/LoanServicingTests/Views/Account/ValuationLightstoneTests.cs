using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing.Valuations;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace LoanServicingTests.Views.Account
{
    [RequiresSTA]
    public sealed class ValuationLightstoneTests : BuildingBlocks.TestBase<InstructLightstoneValuation>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();

            if (!Service<IValuationService>().HasXmlHistoryDummy())
            {
                /*
                 * We need to insert an xmlhsitory row with xmlhistorykey seeded to a value that is 10x higher then by max one in the table. THis way
                 * we can ensure that the key would not have been used by the exval test system.
                */
                Service<IValuationService>().CreateXmlHistoryDummy();
            }
            Service<ICorrespondenceService>().UpdateDefaultEmailAddress(22, "_SAHLITTesting@sahomeloans.com");
            Service<ICorrespondenceService>().UpdateDefaultEmailAddress(23, "_SAHLITTesting@sahomeloans.com");

            base.Browser = new TestBrowser(TestUsers.HaloUser);
        }

        [Test]
        public void when_instructing_easyval_assert_pending_valuations_rule()
        {
            this.InstructPhysicalValuation(true);
            base.Browser.Navigate<LoanServicingCBO>().ClickValuationDetails(NodeTypeEnum.Request);
            base.View.ClickInstruct();
            base.View.AssertPendingValuationInProgress();
        }

        [Test]
        public void when_instructing_easyval_assert_reason_created()
        {
            var val = this.InstructPhysicalValuation(true);
            ReasonAssertions.AssertReason("HOC", "Request Physical Valuation", val.ValuationKey, GenericKeyTypeEnum.Valuation_ValuationKey);
        }

        [Test]
        public void when_instructing_easyval_assert_reasons_dropdown_list()
        {
            this.InstructPhysicalValuation(true);
            base.Browser.Navigate<LoanServicingCBO>().ClickValuationDetails(NodeTypeEnum.Request);
            base.View.AssertReasonsValid();
        }

        [Test]
        public void when_instructing_easyval_assert_valuation_pending()
        {
            var prop = this.InstructPhysicalValuation(true);
            PropertyValuationAssertions.AssertLatestPhysicalLightstoneValuation(prop.PropertyKey, ValuationStatusEnum.Pending);
        }

        [Test]
        public void when_instructing_easyval_assert_valuation_instruct_xmlhistory_created()
        {
            var prop = this.InstructPhysicalValuation(true);
            var account = base.Service<IAccountService>().GetAccountByPropertyKey(prop.PropertyKey, AccountStatusEnum.Open, OriginationSourceEnum.SAHomeLoans);
            PropertyValuationAssertions.AssertLatestPhysicalLightstoneValuationXmlHistory(account.AccountKey);
        }

        [Test]
        public void when_instructing_easyval_for_2nd_time_assert_contact_details_displayed()
        {
            var prop = this.InstructPhysicalValuation(true);
            base.Browser.Navigate<LoanServicingCBO>().ClickValuationDetails(NodeTypeEnum.Request);
            var propertyAccessDetail = new Automation.DataModels.PropertyAccessDetail()
            {
                Contact1 = "contact1",
                Contact1Phone = "0121234567",
                Contact1WorkPhone = "7654321",
                Contact1MobilePhone = "0795242325",
                Contact2 = "contact2",
                Contact2Phone = "0127896543"
            };
            base.View.AssertInspectionContactDetailsDisplayed(propertyAccessDetail);
        }

        [Test]
        public void when_instructing_provided_contact02_a_corresponding_phone_must_be_entered()
        {
            this.InstructPhysicalValuation(true, false);
            var newPropertyAccessDetail = new Automation.DataModels.PropertyAccessDetail()
            {
                Contact1 = "contact1",
                Contact1Phone = "12345678",
                Contact1WorkPhone = "",
                Contact1MobilePhone = "",
                Contact2 = "contact2",
                Contact2Phone = ""
            };
            base.Browser.Navigate<LoanServicingCBO>().ClickValuationDetails(NodeTypeEnum.Request);
            base.View.PupulateValuationDetails("HOC", DateTime.Now.AddDays(1), newPropertyAccessDetail);
            base.View.ClickInstruct();
            //Assertions
            base.View.AssertContact02PhoneNumberProvided();
        }

        [Test]
        public void when_instructing_contact_and_phone_must_be_provided()
        {
            this.InstructPhysicalValuation(true, false);
            var newPropertyAccessDetail = new Automation.DataModels.PropertyAccessDetail()
            {
                Contact1 = "",
                Contact1Phone = "",
                Contact1WorkPhone = "",
                Contact1MobilePhone = "",
                Contact2 = "contact2",
                Contact2Phone = "12345678"
            };
            base.Browser.Navigate<LoanServicingCBO>().ClickValuationDetails(NodeTypeEnum.Request);
            base.View.PupulateValuationDetails("HOC", DateTime.Now.AddDays(1), newPropertyAccessDetail);
            base.View.ClickInstruct();
            //Assertions
            base.View.AssertContact01Required();
            base.View.AssertContact01PhoneRequired();
        }

        [Test]
        public void when_submitting_completed_valuation_on_SAHL_HOC_should_update_valuation_status_to_completed()
        {
            var val = this.InstructPhysicalValuation(true);
            var account = base.Service<IAccountService>().GetAccountByPropertyKey(val.PropertyKey, AccountStatusEnum.Open, OriginationSourceEnum.SAHomeLoans);

            var hoc = Service<IHOCService>().GetHOCAccountDetails(account.AccountKey, account.AccountKey);
            var valuationHOCConventionalAmount = (float)hoc.HOCConventionalAmount + 20.00f;
            var valuationHOCThatchAmount = (float)hoc.HOCThatchAmount + 20.00f;

            //We need the sum insured to be greater or equal than what is currently on the HOC account
            var hocSumInsured = valuationHOCConventionalAmount + valuationHOCThatchAmount;

            base.Service<IValuationService>().SubmitCompletedEzVal(account.AccountKey, HOCRoofEnum.Conventional, conventionalAmount: hocSumInsured);
 
            PropertyValuationAssertions.AssertLatestPhysicalLightstoneValuation(val.PropertyKey, ValuationStatusEnum.Complete);

            var expectedValuation = (from v in Service<IValuationService>().GetValuations(val.PropertyKey)
                                     where v.IsActive
                                     select v).FirstOrDefault();

            HOCAssertions.HOCUpdatedToValuationHOC(expectedValuation);
        }

        [Test]
        public void when_submitting_completed_valuation_on_NON_SAHL_HOC_should_update_valuation_status_to_completed()
        {
            var val = this.InstructPhysicalValuation(false);
            var account = base.Service<IAccountService>().GetAccountByPropertyKey(val.PropertyKey, AccountStatusEnum.Open, OriginationSourceEnum.SAHomeLoans);
            var hocSumInsured = 500000.00f + 500000.00f;
            base.Service<IValuationService>().SubmitCompletedEzVal(account.AccountKey, HOCRoofEnum.Conventional, conventionalAmount: hocSumInsured);
            
            PropertyValuationAssertions.AssertLatestPhysicalLightstoneValuation(val.PropertyKey, ValuationStatusEnum.Complete);

            var expectedValuation = (from v in Service<IValuationService>().GetValuations(val.PropertyKey)
                                     where v.IsActive
                                     select v).FirstOrDefault();

            HOCAssertions.HOCNotUpdatedToValuationHOC(expectedValuation);
        }

        [Test]
        public void when_submitting_completed_valuation_where_SAHL_HOC_lower_than_current_sum_insured_should_update_valuation_status_to_completed()
        {
            var val = this.InstructPhysicalValuation(true);

            var account = base.Service<IAccountService>().GetAccountByPropertyKey(val.PropertyKey, AccountStatusEnum.Open, OriginationSourceEnum.SAHomeLoans);

            var hoc = Service<IHOCService>().GetHOCAccountDetails(account.AccountKey, account.AccountKey);
            var valuationHOCConventionalAmount = (float)hoc.HOCConventionalAmount / 2;
            var valuationHOCThatchAmount = (float)hoc.HOCThatchAmount / 2;

            var hocSumInsured = valuationHOCConventionalAmount + valuationHOCThatchAmount;

            base.Service<IValuationService>().SubmitCompletedEzVal(account.AccountKey, HOCRoofEnum.Conventional, conventionalAmount: hocSumInsured);

            PropertyValuationAssertions.AssertLatestPhysicalLightstoneValuation(val.PropertyKey, ValuationStatusEnum.Complete);
        }

        [Test]
        public void when_submitting_completed_valuation_where_SAHL_HOC_lower_than_current_sum_insured_should_NOT_update_HOC()
        {
            var val = this.InstructPhysicalValuation(true);

            var account = base.Service<IAccountService>().GetAccountByPropertyKey(val.PropertyKey, AccountStatusEnum.Open, OriginationSourceEnum.SAHomeLoans);

            var hoc = Service<IHOCService>().GetHOCAccountDetails(account.AccountKey, account.AccountKey);
            var valuationHOCConventionalAmount = (float)hoc.HOCConventionalAmount / 2;
            var valuationHOCThatchAmount = (float)hoc.HOCThatchAmount / 2;

            var hocSumInsured = valuationHOCConventionalAmount + valuationHOCThatchAmount;

            base.Service<IValuationService>().SubmitCompletedEzVal(account.AccountKey, HOCRoofEnum.Conventional, conventionalAmount: hocSumInsured);

            var expectedValuation = (from v in Service<IValuationService>().GetValuations(val.PropertyKey)
                                     where v.IsActive
                                     select v).FirstOrDefault();

            HOCAssertions.HOCNotUpdatedToValuationHOC(expectedValuation);
        }

        [Test]
        public void when_submitting_rejected_valuation_should_update_valuation_status_to_returned()
        {
            var val = this.InstructPhysicalValuation(true);
            var account = base.Service<IAccountService>().GetAccountByPropertyKey(val.PropertyKey, AccountStatusEnum.Open, OriginationSourceEnum.SAHomeLoans);
            var hocSumInsured = 5000000.00f + 5000000.00f;
            base.Service<IValuationService>().SubmitRejectedEzVal(account.AccountKey);

            PropertyValuationAssertions.AssertLatestPhysicalLightstoneValuation(val.PropertyKey, ValuationStatusEnum.Returned);
        }

        /// <summary>
        /// This will navigate to the instruct screen populate all the fields and make a lightstone physical valuation request
        /// </summary>
        /// <param name="hasSAHLHOC"></param>
        /// <param name="captureContactDetails"></param>
        /// <returns></returns>
        private Automation.DataModels.Valuation InstructPhysicalValuation(bool hasSAHLHOC, bool captureContactDetails = true, int accountkey = 0)
        {
            var account = default(Automation.DataModels.Account);
            var property = default(Automation.DataModels.Property);
            if (accountkey == 0)
            {
                //GetRandomValuationForAccountWithHOC always want thatch because the test xml that is submitted back for every test is hardcoded with conventional.
                var valuation = default(Automation.DataModels.Valuation);
                if (hasSAHLHOC)
                    valuation = base.Service<IValuationService>().GetRandomValuationForAccountWithHOC(AccountStatusEnum.Open, HOCInsurerEnum.SAHLHOC, OriginationSourceEnum.SAHomeLoans, HOCRoofEnum.Thatch);
                else
                    valuation = base.Service<IValuationService>().GetRandomValuationForAccountWithHOC(AccountStatusEnum.Open, HOCInsurerEnum.SantamBeperk, OriginationSourceEnum.SAHomeLoans, HOCRoofEnum.Thatch);

                Assert.That(valuation != null, "Error messages from the EzVal prevented the valuation from being instructed");

                account = base.Service<IAccountService>().GetAccountByPropertyKey(valuation.PropertyKey, AccountStatusEnum.Open, OriginationSourceEnum.SAHomeLoans);
            }
            else
            {
                account = base.Service<IAccountService>().GetAccountByKey(accountkey);
            }

            property = base.Service<IPropertyService>().GetPropertyByAccountKey(account.AccountKey);

            //Update the all the valuations to completed so that pending rule does not fire
            base.Service<IValuationService>().UpdateAllValuationStatuses(ValuationStatusEnum.Complete, property.PropertyKey);

            var leKey = (from r in base.Service<ILegalEntityService>().GetLegalEntityRoles(account.AccountKey) select r.LegalEntityKey).FirstOrDefault();
            var le = base.Service<ILegalEntityService>().GetLegalEntity(legalentitykey: leKey);

            //Search navigate and load the legal entity and navigate the the udpate screen screen
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();

            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().PopulateSearch(le);
            base.Browser.Page<ClientSuperSearch>().PerformSearch();
            base.Browser.Page<ClientSuperSearch>().SelectByLegalEntityKey(le.LegalEntityKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().ClickValuationDetails(NodeTypeEnum.Request);

            if (captureContactDetails)
            {
                var propertyAccessDetail = new Automation.DataModels.PropertyAccessDetail()
                {
                    Contact1 = "contact1",
                    Contact1Phone = "0121234567",
                    Contact1WorkPhone = "7654321",
                    Contact1MobilePhone = "0795242325",
                    Contact2 = "contact2",
                    Contact2Phone = "0127896543"
                };

                base.View.PupulateValuationDetails("HOC", DateTime.Now.AddDays(1), propertyAccessDetail);
                base.View.ClickInstruct();
            }

            return (from val in base.Service<IValuationService>().GetValuations(property.PropertyKey)
                    where val.ValuationDate.Value > DateTime.Now.Subtract(TimeSpan.FromMinutes(2))
                    select val).FirstOrDefault();
        }
    }
}