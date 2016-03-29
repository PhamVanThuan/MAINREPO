using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCaptureTests.Views.LegalEntityFloBo
{
    [RequiresSTA]
    public class DomiciliumUpdateAddressTests : TestBase<LegalEntityDomiciliumAddress>
    {
        private int offerkey;
        private int legalEntityKey;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);

            //get an application in app capture
            offerkey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType
                (
                    WorkflowStates.ApplicationCaptureWF.ApplicationCapture,
                    Workflows.ApplicationCapture,
                    OfferTypeEnum.NewPurchase,
                    ""
                );
            Service<IApplicationService>().CleanupNewBusinessOffer(offerkey);
            Service<IApplicationService>().DeleteAllOfferClientDomiciliumAddresses(offerkey);

            //get first legal entity key on the offer
            var offerRoleRow = Service<IApplicationService>().GetActiveOfferRolesByOfferRoleType(offerkey, OfferRoleTypeEnum.LeadMainApplicant).First();
            legalEntityKey = offerRoleRow.Column("legalentitykey").GetValueAs<int>();

            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.WorkflowSuperSearch>().WorkflowSearch(offerkey);
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityKey);
        }

        protected override void OnTestStart()
        {
            Service<ILegalEntityAddressService>().CleanupLegalEntityAddresses(legalEntityKey, true, GeneralStatusEnum.Inactive);
            Service<IApplicationService>().DeleteAllOfferClientDomiciliumAddresses(offerkey);
            //Insert invalid ones
            Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.Box, AddressTypeEnum.Postal, GeneralStatusEnum.Active);
            Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.ClusterBox, AddressTypeEnum.Postal, GeneralStatusEnum.Active);
            Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.FreeText, AddressTypeEnum.Postal, GeneralStatusEnum.Active);
            Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.PostNetSuite, AddressTypeEnum.Postal, GeneralStatusEnum.Active);
            Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.PrivateBag, AddressTypeEnum.Postal, GeneralStatusEnum.Active);
            Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.FreeText, AddressTypeEnum.Residential, GeneralStatusEnum.Active);
            Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.FreeText, AddressTypeEnum.Postal, GeneralStatusEnum.Active);
            //insert valid ones
            Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Postal, GeneralStatusEnum.Active);
            Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Postal, GeneralStatusEnum.Inactive);
            Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential, GeneralStatusEnum.Active);
            Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential, GeneralStatusEnum.Inactive);
        }

        protected override void OnTestTearDown()
        {
            Service<ILegalEntityAddressService>().CleanupLegalEntityAddresses(legalEntityKey, true, GeneralStatusEnum.Inactive);
            Service<IApplicationService>().DeleteAllOfferClientDomiciliumAddresses(offerkey);
        }

        [Test]
        public void when_capturing_new_domicilium_address_should_create_pending_domicilium_address()
        {
            var validLegalEntityAddress = GetLegalEntityAddressFromInsertedAddresses(GeneralStatusEnum.Active, AddressFormatEnum.Street).First();

            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
            base.View.SelectDomiciliumAddress(validLegalEntityAddress.DelimitedAddress);
            base.View.ClickSubmit();

            OfferAssertions.AssertOfferRoleDomiciliumLegalEntityDomiciliumLinkExist(offerkey, legalEntityKey);
            LegalEntityAssertions.AssertLegalEntityDomicilium(validLegalEntityAddress, GeneralStatusEnum.Pending);
        }

        [Test]
        public void when_view_loads_should_display_active_legal_entity_addresses_only()
        {
            var activeLegalEntityAddresses = GetLegalEntityAddressFromInsertedAddresses(GeneralStatusEnum.Active, AddressFormatEnum.Street);
            var inactiveLegalEntityAddresses = GetLegalEntityAddressFromInsertedAddresses(GeneralStatusEnum.Inactive, AddressFormatEnum.Street);

            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
            base.View.AssertLegalEntityAddressesDisplayed(activeLegalEntityAddresses);
            base.View.AssertLegalEntityAddressesAreNotDisplayed(inactiveLegalEntityAddresses);
        }

        [Test]
        public void when_view_loads_should_display_legal_entity_addresses_with_valid_formats()
        {
            var validLegalEntityAddresses = GetLegalEntityAddressFromInsertedAddresses(GeneralStatusEnum.Active, AddressFormatEnum.Street);

            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
            base.View.AssertLegalEntityAddressesDisplayed(validLegalEntityAddresses);
        }

        [Test]
        public void when_view_loads_should_NOT_display_legal_entity_addresses_with_invalid_formats()
        {
            var invalidLegalEntityAddresses = GetLegalEntityAddressFromInsertedAddresses(GeneralStatusEnum.Active, AddressFormatEnum.ClusterBox);

            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
            base.View.AssertLegalEntityAddressesAreNotDisplayed(invalidLegalEntityAddresses);
        }

        [Test]
        public void when_all_legal_entity_addresses_are_inactive_should_show_property_address()
        {
            Service<ILegalEntityAddressService>().CleanupLegalEntityAddresses(legalEntityKey, false, GeneralStatusEnum.Inactive);
            var inactiveLegalEntityAddresses = GetLegalEntityAddressFromInsertedAddresses(GeneralStatusEnum.Inactive, AddressFormatEnum.Street);

            var propertyAddressKey = Service<IPropertyService>().GetProperty(offerkey: offerkey).AddressKey;
            var formattedAddress = Service<IAddressService>().GetFormattedAddressByKey(propertyAddressKey);

            formattedAddress = String.Format("{0}Property Address", formattedAddress);

            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
            base.View.AssertFormattedAddressDisplayed(formattedAddress);
            base.View.AssertLegalEntityAddressesAreNotDisplayed(inactiveLegalEntityAddresses);
        }

        [Test]
        public void when_linking_to_current_active_domicilium_should_create_new_pending_legal_entity_domicilium()
        {
            var leDomiciliumAddress = GetLegalEntityAddressFromInsertedAddresses(GeneralStatusEnum.Active, AddressFormatEnum.Street).First();
            var formattedAddress = SetupLegalEntityOfferDomiciliumAddress(leDomiciliumAddress.LegalEntityAddressKey, GeneralStatusEnum.Active, false);

            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
            base.View.AssertFormattedAddressDisplayed(String.Format("{0}Active Domicilium Address", leDomiciliumAddress.DelimitedAddress));

            base.View.SelectDomiciliumAddress(leDomiciliumAddress.DelimitedAddress);
            base.View.ClickSubmit();

            OfferAssertions.AssertOfferRoleDomiciliumLegalEntityDomiciliumLinkExist(offerkey, legalEntityKey);
            LegalEntityAssertions.AssertLegalEntityDomicilium(leDomiciliumAddress, GeneralStatusEnum.Pending);
        }

        [Test]
        public void when_updating_multiple_times_only_a_single_pending_exists_and_no_inactive_records_should_exist()
        {
            var firstLegalEntityAddressesToUse = GetLegalEntityAddressFromInsertedAddresses(GeneralStatusEnum.Active, AddressFormatEnum.Street).First();
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
            base.View.SelectDomiciliumAddress(firstLegalEntityAddressesToUse.DelimitedAddress);
            base.View.ClickSubmit();

            var secondLegalEntityAddressesToUse = GetLegalEntityAddressFromInsertedAddresses(GeneralStatusEnum.Active, AddressFormatEnum.Street, firstLegalEntityAddressesToUse.LegalEntityAddressKey).First();
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
            base.View.SelectDomiciliumAddress(secondLegalEntityAddressesToUse.DelimitedAddress);
            base.View.ClickSubmit();

            OfferAssertions.AssertOfferRoleDomiciliumLegalEntityDomiciliumLinkExist(offerkey, legalEntityKey);
            LegalEntityAssertions.AssertLegalEntityDomicilium(secondLegalEntityAddressesToUse, GeneralStatusEnum.Pending);
        }

        [Test]
        public void when_selecting_a_proposed_domicilium_different_to_active_a_Warning_is_displayed()
        {
            var activeAddress = GetLegalEntityAddressFromInsertedAddresses(GeneralStatusEnum.Active, AddressFormatEnum.Street).First();
            var formattedAddress = SetupLegalEntityOfferDomiciliumAddress(activeAddress.LegalEntityAddressKey, GeneralStatusEnum.Active, true);

            var proposedAddress = (from address in GetLegalEntityAddressFromInsertedAddresses(GeneralStatusEnum.Active, AddressFormatEnum.Street)
                                   where address.AddressKey != activeAddress.AddressKey
                                   select address).FirstOrDefault();

            var addressToUse = GetLegalEntityAddressFromInsertedAddresses(GeneralStatusEnum.Active, AddressFormatEnum.Street).FirstOrDefault();
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
            base.View.SelectDomiciliumAddress(proposedAddress.DelimitedAddress);
            base.View.ClickSubmit();

            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Selected Pending Domicilium is not the same as the Active Domicilum Address.");
            base.Browser.Page<BasePageAssertions>().AssertValidationIsWarning();
        }

        [Test]
        public void when_selecting_a_proposed_domicilium_different_to_active_and_submitting_it_with_warning_should_make_pending_offerrole_domicilium()
        {
            var activeAddress = GetLegalEntityAddressFromInsertedAddresses(GeneralStatusEnum.Active, AddressFormatEnum.Street).First();
            var formattedAddress = SetupLegalEntityOfferDomiciliumAddress(activeAddress.LegalEntityAddressKey, GeneralStatusEnum.Active, true);

            var proposedAddress = (from address in GetLegalEntityAddressFromInsertedAddresses(GeneralStatusEnum.Active, AddressFormatEnum.Street)
                                   where address.AddressKey != activeAddress.AddressKey
                                   select address).FirstOrDefault();

            var addressToUse = GetLegalEntityAddressFromInsertedAddresses(GeneralStatusEnum.Active, AddressFormatEnum.Street).FirstOrDefault();
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
            base.View.SelectDomiciliumAddress(proposedAddress.DelimitedAddress);
            base.View.ClickSubmit();
            base.Browser.Page<BasePage>().DomainWarningClickYes();

            OfferAssertions.AssertOfferRoleDomiciliumLegalEntityDomiciliumLinkExist(offerkey, legalEntityKey);
            LegalEntityAssertions.AssertLegalEntityDomicilium(proposedAddress, GeneralStatusEnum.Pending);
        }

        [Test]
        public void when_an_inactive_legalEntityAddress_exists_for_propertyAddress_a_new_Active_legalEntityAddress_is_created()
        {
            var offerPropertyKey = Service<IPropertyService>().GetPropertyKeyByOfferKey(offerkey);
            var inactiveLegalEntityAddress = GetLegalEntityAddressFromInsertedAddresses(GeneralStatusEnum.Inactive, AddressFormatEnum.Street).First();
            var property = Service<IPropertyService>().UpdatePropertyAddress(offerPropertyKey, inactiveLegalEntityAddress.AddressKey);

            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.WorkflowSuperSearch>().WorkflowSearch(offerkey);
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityKey);
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);

            //select the property address
            base.View.SelectDomiciliumAddress(property.FormattedPropertyAddress);
            base.View.ClickSubmit();

            //we should have a new active legal entity address, for the same address key as the property
            var activeLegalEntityAddress = Service<ILegalEntityAddressService>().GetLegalEntityAddresses(legalEntityKey)
                .First(x => x.GeneralStatusKey == GeneralStatusEnum.Active && x.AddressKey == property.AddressKey);
            Assert.That(activeLegalEntityAddress != null, "No active Legal Entity address was created for the property address");
            LegalEntityAssertions.AssertLegalEntityDomicilium(activeLegalEntityAddress, GeneralStatusEnum.Pending);
        }

        private string SetupLegalEntityOfferDomiciliumAddress(int legalEntityAddressKey, GeneralStatusEnum legalEntityDomiciliumStatus, bool createOfferRoleDomiciliums)
        {
            if (createOfferRoleDomiciliums)
            {
                var offerRoleDomiciliums = new List<Automation.DataModels.LegalEntityDomicilium>();
                var offerRoles = from or in base.Service<IApplicationService>().GetActiveOfferRolesByOfferKey(offerkey, OfferRoleTypeGroupEnum.Client)
                                 where or.OfferRoleTypeKey != OfferRoleTypeEnum.AssuredLife
                                 select or;
                foreach (var or in offerRoles)
                {
                    var legalentityDom = Service<ILegalEntityAddressService>().InsertLegalEntityDomiciliumAddress(legalEntityAddressKey, legalEntityKey, legalEntityDomiciliumStatus);
                    var offerRoleDomicilium = Service<IApplicationService>().InsertOfferRoleDomicilium(legalentityDom.LegalEntityDomiciliumKey, or.OfferRoleKey, offerkey);
                    offerRoleDomiciliums.Add(offerRoleDomicilium);
                }
                return offerRoleDomiciliums.FirstOrDefault().DelimitedAddress;
            }
            else
            {
                var legalentityDom = Service<ILegalEntityAddressService>().InsertLegalEntityDomiciliumAddress(legalEntityAddressKey, legalEntityKey, legalEntityDomiciliumStatus);
                return legalentityDom.DelimitedAddress;
            }
        }

        private IEnumerable<Automation.DataModels.LegalEntityAddress> GetLegalEntityAddressFromInsertedAddresses(GeneralStatusEnum legalEntityAddressStatus, AddressFormatEnum addressFormat, int excludeAddressFromlegalEntityAddressKey = 0)
        {
            return from address in Service<ILegalEntityAddressService>().GetLegalEntityAddresses(legalEntityKey)
                   where address.GeneralStatusKey == legalEntityAddressStatus
                          && address.AddressFormatKey == addressFormat
                         && address.LegalEntityAddressKey != excludeAddressFromlegalEntityAddressKey
                   select address;
        }
    }
}