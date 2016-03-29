using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace LoanServicingTests.Views.LegalEntity
{
    [RequiresSTA]
    public class DomiciliumUpdateAddressTests : TestBase<LegalEntityDomiciliumAddress>
    {
        private int legalEntityKey;
        private int accountKey;
        private int relatedAccountKey;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);

            accountKey = (from r in Service<IAccountService>().GetRandomVariableLoanAccountByMainApplicantCount(1, 1, AccountStatusEnum.Open)
                          select r.Column("accountkey").GetValueAs<int>()).FirstOrDefault();

            relatedAccountKey = (from r in Service<IAccountService>().GetRandomVariableLoanAccountByMainApplicantCount(1, 1, AccountStatusEnum.Open)
                                 select r.Column("accountkey").GetValueAs<int>()).FirstOrDefault();

            legalEntityKey = Service<ILegalEntityService>().GetFirstLegalEntityMainApplicantOnAccount(accountKey).Column("legalentitykey").GetValueAs<int>();

            var legalName = Service<ILegalEntityService>().GetLegalEntityLegalName(legalEntityKey);
            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);

            base.Browser.Navigate<BuildingBlocks.Navigation.CBO.LoanServicing.LoanServicingCBO>().LegalEntityParentNode(legalName);
        }

        protected override void OnTestStart()
        {
            //Clear all domicilium address
            Service<ILegalEntityAddressService>().DeleteLegalEntityDomiciliumAddress(legalEntityKey);
            //clean up legal entity addresses
            Service<ILegalEntityAddressService>().CleanupLegalEntityAddresses(legalEntityKey, true, GeneralStatusEnum.Inactive);
            base.OnTestStart();
        }

        [Test]
        public void when_capturing_a_new_domicilium_an_active_record_is_created()
        {
            //create a new link
            var legalEntityAddress = Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential, GeneralStatusEnum.Active);
            //load screen again
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
            base.View.SelectDomiciliumAddress(legalEntityAddress.DelimitedAddress);
            base.View.ClickSubmit();
            LegalEntityAssertions.AssertLegalEntityDomicilium(legalEntityAddress, GeneralStatusEnum.Active);
        }

        [Test]
        public void when_capturing_a_property_address_that_is_not_an_active_legalEntityAddress_a_new_legalEntityAddress_is_created()
        {
            try
            {
                Service<ILegalEntityAddressService>().CleanupLegalEntityAddresses(legalEntityKey, false, GeneralStatusEnum.Inactive);
                var propertyAddress = Service<IPropertyService>().GetFormattedPropertyAddressByAccountKey(accountKey);
                //load screen again
                base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
                base.View.SelectDomiciliumAddress(propertyAddress.FormattedPropertyAddress);
                base.View.ClickSubmit();
                //get the legal entity addresses
                var activelegalEntityAddresses = Service<ILegalEntityAddressService>().GetLegalEntityAddresses(legalEntityKey)
                    .Where(x => x.GeneralStatusKey == GeneralStatusEnum.Active && x.AddressKey == propertyAddress.AddressKey);
                //should only be one
                Assert.That(activelegalEntityAddresses.Count() == 1, string.Format(@"Expected only 1 active legal entity address, there were {0}", activelegalEntityAddresses.Count()));
                var activeLegalEntityAddress = activelegalEntityAddresses.FirstOrDefault();
                //format should match
                StringAssert.AreEqualIgnoringCase(propertyAddress.FormattedPropertyAddress, activeLegalEntityAddress.DelimitedAddress, "Active legal entity address description does not match the property address");
            }
            finally
            {
                Service<ILegalEntityAddressService>().CleanupLegalEntityAddresses(legalEntityKey, false, GeneralStatusEnum.Active);
            }
        }

        [Test]
        public void when_legal_entity_is_linked_to_multiple_properties_all_property_addresses_should_be_displayed()
        {
            try
            {
                Service<ILegalEntityAddressService>().CleanupLegalEntityAddresses(legalEntityKey, false, GeneralStatusEnum.Inactive);
                //we need to ensure our legal entity is linked to multiple accounts
                Service<IAccountService>().AddRoleToAccount(relatedAccountKey, RoleTypeEnum.MainApplicant, GeneralStatusEnum.Active, legalEntityKey);
                var propertyAddress = Service<IPropertyService>().GetFormattedPropertyAddressByAccountKey(accountKey).FormattedPropertyAddress;
                var propertyAddressRelated = Service<IPropertyService>().GetFormattedPropertyAddressByAccountKey(relatedAccountKey).FormattedPropertyAddress;
                //load screen again
                base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
                //view should contain both addresses
                base.View.AssertFormattedAddressDisplayed(string.Format(@"{0} {1}{2}", propertyAddress, string.Format(@"(Loan {0})", accountKey), "Property Address"));
                base.View.AssertFormattedAddressDisplayed(string.Format(@"{0} {1}{2}", propertyAddressRelated, string.Format(@"(Loan {0})", relatedAccountKey), "Property Address"));
            }
            finally
            {
                Service<ILegalEntityAddressService>().CleanupLegalEntityAddresses(legalEntityKey, false, GeneralStatusEnum.Active);
                Service<IAccountService>().RemoveRoleFromAccount(relatedAccountKey, RoleTypeEnum.MainApplicant, legalEntityKey);
            }
        }

        [Test]
        public void when_selecting_legal_entity_domicilium_only_valid_formats_are_shown()
        {
            //insert invalid formats
            var invalidFormats = new List<Automation.DataModels.LegalEntityAddress>
                {
                    Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.Box, AddressTypeEnum.Postal,GeneralStatusEnum.Active),
                    Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.ClusterBox, AddressTypeEnum.Postal, GeneralStatusEnum.Active),
                    Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.FreeText, AddressTypeEnum.Postal, GeneralStatusEnum.Active),
                    Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.PostNetSuite, AddressTypeEnum.Postal, GeneralStatusEnum.Active),
                    Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.PrivateBag, AddressTypeEnum.Postal, GeneralStatusEnum.Active),
                    Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.FreeText,AddressTypeEnum.Residential, GeneralStatusEnum.Active),
                    Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.FreeText,AddressTypeEnum.Postal, GeneralStatusEnum.Active)
                };
            //insert valid ones
            var validFormats = new List<Automation.DataModels.LegalEntityAddress>
                {
                    Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Postal, GeneralStatusEnum.Active),
                    Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential, GeneralStatusEnum.Active)
                };
            //get the legal entity addresses
            var legalEntityAddresses = Service<ILegalEntityAddressService>().GetLegalEntityAddresses(legalEntityKey);
            validFormats.AddRange(legalEntityAddresses.Where(x => x.AddressFormatKey == AddressFormatEnum.Street && x.GeneralStatusKey == GeneralStatusEnum.Active));
            //reload view
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
            //we should have all the valid ones
            base.View.AssertLegalEntityAddressesDisplayed(validFormats);
            //none of the invalid ones
            base.View.AssertLegalEntityAddressesAreNotDisplayed(invalidFormats);
        }

        [Test]
        public void when_no_address_is_selected_a_warning_is_displayed()
        {
            //load screen
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
            base.View.ClickSubmit();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains("Must select an Address.");
        }

        [Test]
        public void when_capturing_a_new_domicilium_the_previous_is_set_to_inactive()
        {
            //create a new link
            var legalEntityAddress1 = Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential, GeneralStatusEnum.Active);
            //load screen again
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
            base.View.SelectDomiciliumAddress(legalEntityAddress1.DelimitedAddress);
            base.View.ClickSubmit();
            var legalEntityAddress2 = Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential, GeneralStatusEnum.Active);
            //load screen again
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
            base.View.SelectDomiciliumAddress(legalEntityAddress2.DelimitedAddress);
            base.View.ClickSubmit();
            //first should be inactive
            LegalEntityAssertions.AssertLegalEntityDomicilium(legalEntityAddress1, GeneralStatusEnum.Inactive);
            //second should be active
            LegalEntityAssertions.AssertLegalEntityDomicilium(legalEntityAddress2, GeneralStatusEnum.Active);
        }

        [Test]
        public void when_loading_the_screen_only_displays_active_legalEntityAddresses()
        {
            //insert valid ones
            var activeAddresses = new List<Automation.DataModels.LegalEntityAddress>
                {
                    Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Postal, GeneralStatusEnum.Active),
                    Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential, GeneralStatusEnum.Active)
                };
            //inactive records
            var inactiveAddresses = new List<Automation.DataModels.LegalEntityAddress>
                {
                    Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Postal, GeneralStatusEnum.Inactive),
                    Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential, GeneralStatusEnum.Inactive)
                };
            //reload view
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
            //active should exist
            base.View.AssertLegalEntityAddressesDisplayed(activeAddresses);
            //inactive should not be displayed
            base.View.AssertLegalEntityAddressesAreNotDisplayed(inactiveAddresses);
        }

        [Test]
        public void when_all_legal_entity_addresses_are_inactive_the_property_address_Should_be_displayed()
        {
            try
            {
                Service<ILegalEntityAddressService>().CleanupLegalEntityAddresses(legalEntityKey, false, GeneralStatusEnum.Inactive);
                var propertyAddress = Service<IPropertyService>().GetFormattedPropertyAddressByAccountKey(accountKey).FormattedPropertyAddress;
                //reload view
                base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
                base.View.AssertFormattedAddressDisplayed(propertyAddress);
            }
            finally
            {
                Service<ILegalEntityAddressService>().CleanupLegalEntityAddresses(legalEntityKey, false, GeneralStatusEnum.Active);
            }
        }

        [Test]
        public void when_updating_an_existing_active_domicilium_a_new_active_record_is_created_and_prev_set_to_inactive()
        {
            var legalEntityAddress = Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential, GeneralStatusEnum.Active);
            var legalEntityDomicilium = Service<ILegalEntityAddressService>().InsertLegalEntityDomiciliumAddress(legalEntityAddress.LegalEntityAddressKey, legalEntityKey, GeneralStatusEnum.Active);
            //insert a new legal entity address to use
            var newLegalEntityAddress = Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential, GeneralStatusEnum.Active);
            //reload view
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
            base.View.SelectDomiciliumAddress(newLegalEntityAddress.DelimitedAddress);
            base.View.ClickSubmit();
            //check addresses
            LegalEntityAssertions.AssertLegalEntityDomicilium(newLegalEntityAddress, GeneralStatusEnum.Active);
            //get inactive addresses
            var inactiveDomicilium = Service<ILegalEntityAddressService>().GetLegalEntityDomiciliumAddress(legalEntityKey, GeneralStatusEnum.Inactive).FirstOrDefault();
            Assert.That(legalEntityDomicilium.LegalEntityAddressKey == inactiveDomicilium.LegalEntityAddressKey);
        }

        [Test]
        public void when_an_inactive_legalEntityAddress_exists_for_propertyAddress_a_new_Active_legalEntityAddress_is_created()
        {
            try
            {
                Service<ILegalEntityAddressService>().CleanupLegalEntityAddresses(legalEntityKey, false, GeneralStatusEnum.Inactive);
                var propertyAddress = Service<IPropertyService>().GetFormattedPropertyAddressByAccountKey(accountKey);
                //insert this record
                var legalEntityAddress = Service<ILegalEntityAddressService>().InsertLegalEntityAddress(new Automation.DataModels.LegalEntityAddress
                {
                    AddressKey = propertyAddress.AddressKey,
                    LegalEntityKey = legalEntityKey,
                    GeneralStatusKey = GeneralStatusEnum.Inactive,
                    AddressTypeKey = AddressTypeEnum.Residential
                });
                base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
                //select the property address
                base.View.SelectDomiciliumAddress(propertyAddress.FormattedPropertyAddress);
                base.View.ClickSubmit();
                //we should have a new active legal entity address, for the same address key as the property
                var activeLegalEntityAddress = Service<ILegalEntityAddressService>().GetLegalEntityAddresses(legalEntityKey)
                    .Where(x => x.GeneralStatusKey == GeneralStatusEnum.Active && x.AddressKey == propertyAddress.AddressKey)
                    .FirstOrDefault();
                Assert.That(activeLegalEntityAddress != null, "No active Legal Entity address was created for the property address");
                LegalEntityAssertions.AssertLegalEntityDomicilium(activeLegalEntityAddress, GeneralStatusEnum.Active);
            }
            finally
            {
                Service<ILegalEntityAddressService>().CleanupLegalEntityAddresses(legalEntityKey, false, GeneralStatusEnum.Active);
            }
        }

        [Test]
        public void when_an_active_legalEntityAddress_exists_for_propertyAddress_it_is_not_displayed_and_legalEntityAddress_is_used()
        {
            try
            {
                Service<ILegalEntityAddressService>().CleanupLegalEntityAddresses(legalEntityKey, false, GeneralStatusEnum.Inactive);
                var propertyAddress = Service<IPropertyService>().GetFormattedPropertyAddressByAccountKey(accountKey);
                //insert this record
                var legalEntityAddress = Service<ILegalEntityAddressService>().InsertLegalEntityAddress(new Automation.DataModels.LegalEntityAddress
                {
                    AddressKey = propertyAddress.AddressKey,
                    LegalEntityKey = legalEntityKey,
                    GeneralStatusKey = GeneralStatusEnum.Active,
                    AddressTypeKey = AddressTypeEnum.Residential
                });
                base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
                //property address should not be there
                base.View.AssertFormattedAddressNotDisplayed(string.Format(@"{0} {1}{2}", propertyAddress.FormattedPropertyAddress, string.Format(@"(Loan {0})", accountKey), "Property Address"));
                //matching legal entity address should be
                base.View.AssertFormattedAddressDisplayed(string.Format(@"{0}{1}", legalEntityAddress.DelimitedAddress, "Street"));
            }
            finally
            {
                Service<ILegalEntityAddressService>().CleanupLegalEntityAddresses(legalEntityKey, false, GeneralStatusEnum.Active);
            }
        }
    }
}