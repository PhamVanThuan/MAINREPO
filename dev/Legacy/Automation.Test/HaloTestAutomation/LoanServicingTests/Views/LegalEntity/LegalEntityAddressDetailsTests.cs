using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core;

namespace LoanServicingTests.Views.LegalEntity
{
    [RequiresSTA]
    public sealed class LegalEntityAddressDetailsTests : TestBase<LegalEntityAddressDetails>
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
            TestStart();
        }

        #endregion SetupTearDown

        #region LegalEntityAddressAdd

        /// <summary>
        /// This test ensures that the user can add a new legal entity address record. The test checks that the expected address has been added and is
        /// linked to the correct legal entity.
        /// </summary>
        [Test]
        public void AddLegalEntityAddress()
        {
            Navigate(NodeTypeEnum.Add);
            var r = new Random();
            var address = new Automation.DataModels.Address
                            {
                                StreetNumber = r.Next(0, 999).ToString(),
                                StreetName = string.Format("{0} Street", r.Next(0, 999)),
                                RRR_SuburbDescription = "La Lucia Ridge",
                                RRR_ProvinceDescription = Province.Kwazulunatal,
                                UnitNumber = string.Format("{0} Unit", r.Next(0, 100)),
                                BuildingName = string.Format("{0} Building Name", r.Next(0, 999)),
                                BuildingNumber = string.Format("{0}", r.Next(0, 999))
                            };
            base.View.AddResidentialAddress(address);
            int addressKey = AddressAssertions.AssertResidentialAddressRecordExists(address.StreetNumber, address.StreetName, address.RRR_ProvinceDescription,
                address.RRR_SuburbDescription);
            AddressAssertions.AssertAddressLegalEntityLinkByGeneralStatus(_legalEntityKey, addressKey, GeneralStatusEnum.Active);
            AddressAssertions.AssertAddressType(_legalEntityKey, addressKey, AddressType.Residential);
        }

        #endregion LegalEntityAddressAdd

        #region LegalEntityAddressUpdate

        /// <summary>
        /// This test ensures that the user can update an existing legal entity address record. The test checks that a new address record exists with the
        /// updated address details and that the new record is now linked to the legal entity.
        /// </summary>
        [Test]
        public void UpdateLegalEntityAddress()
        {
            //add an address
            Navigate(NodeTypeEnum.Add);
            var r = new Random();
            var address = new Automation.DataModels.Address
            {
                StreetNumber = r.Next(0, 999).ToString(),
                StreetName = string.Format("{0} Street", r.Next(0, 999)),
                RRR_SuburbDescription = "La Lucia Ridge",
                RRR_ProvinceDescription = Province.Kwazulunatal,
                UnitNumber = string.Format("{0} Unit", r.Next(0, 100)),
                BuildingName = string.Format("{0} Building Name", r.Next(0, 999)),
                BuildingNumber = string.Format("{0}", r.Next(0, 999))
            };
            base.View.AddResidentialAddress(address);
            int addressKey = AddressAssertions.AssertResidentialAddressRecordExists(address.StreetNumber, address.StreetName, address.RRR_ProvinceDescription,
                address.RRR_SuburbDescription);
            //update the address
            string addressDescription = Service<ILegalEntityAddressService>().GetFormattedAddressByKey(addressKey);
            var updatedAddress = new Automation.DataModels.Address
            {
                StreetNumber = r.Next(0, 999).ToString(),
                StreetName = string.Format("{0} Street", r.Next(0, 999)),
                RRR_SuburbDescription = "La Lucia Ridge",
                RRR_ProvinceDescription = Province.Kwazulunatal
            };
            base.Browser.Navigate<LoanServicingCBO>().AddressDetails(NodeTypeEnum.Update);
            base.View.SelectAndUpdateResidentialStreetAddress(addressDescription, updatedAddress);
            int updatedAddressKey = AddressAssertions.AssertResidentialAddressRecordExists(updatedAddress.StreetNumber, updatedAddress.StreetName, updatedAddress.RRR_ProvinceDescription,
                updatedAddress.RRR_SuburbDescription);
            AddressAssertions.AssertAddressLegalEntityLinkByGeneralStatus(_legalEntityKey, updatedAddressKey, GeneralStatusEnum.Active);
            AddressAssertions.AssertAddressType(_legalEntityKey, updatedAddressKey, AddressType.Residential);
            Assert.That(addressKey != updatedAddressKey, "");
        }

        [Test]
        public void when_updating_a_legalEntityAddress_linked_to_an_active_domicilium_a_warning_is_displayed()
        {
            var random = new Random();
            //we need a new legal entity address
            var legalEntityAddress = Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(_legalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential,
                GeneralStatusEnum.Active);
            //remove any existing domicilium
            Service<ILegalEntityAddressService>().DeleteLegalEntityDomiciliumAddress(_legalEntityKey);
            string legalEntityName = Service<ILegalEntityService>().GetLegalEntityLegalName(_legalEntityKey);
            //link new address to active domicilium
            var updatedAddress = new Automation.DataModels.Address
            {
                StreetNumber = random.Next(0, 999).ToString(),
                StreetName = string.Format("{0} Street", random.Next(0, 999)),
                RRR_SuburbDescription = "La Lucia Ridge",
                RRR_ProvinceDescription = Province.Kwazulunatal
            };
            var domiciliumAddress = Service<ILegalEntityAddressService>().InsertLegalEntityDomiciliumAddress(legalEntityAddress.LegalEntityAddressKey, _legalEntityKey, GeneralStatusEnum.Active);
            //update it
            base.Browser.Navigate<LoanServicingCBO>().AddressDetails(NodeTypeEnum.Update);
            base.View.SelectAndUpdateResidentialStreetAddress(domiciliumAddress.DelimitedAddress, updatedAddress);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains(
                string.Format(@"Changing this address will update the active domicilium address for {0}.", legalEntityName));
        }

        #endregion LegalEntityAddressUpdate

        #region LegalEntityAddressDelete

        /// <summary>
        /// This test ensures that the user can delete a legal entity address record. The test checks that the expected address record is no longer actively
        /// linked to the legal entity.
        /// </summary>
        [Test]
        public void DeleteLegalEntityAddress()
        {
            Navigate(NodeTypeEnum.Add);
            var r = new Random();
            var address = new Automation.DataModels.Address
            {
                StreetNumber = r.Next(0, 999).ToString(),
                StreetName = string.Format("{0} Street", r.Next(0, 999)),
                RRR_SuburbDescription = "La Lucia Ridge",
                RRR_ProvinceDescription = Province.Kwazulunatal,
                UnitNumber = string.Format("{0} Unit", r.Next(0, 100)),
                BuildingName = string.Format("{0} Building Name", r.Next(0, 999)),
                BuildingNumber = string.Format("{0}", r.Next(0, 999)),
                UserID = @"SAHL\ClintonS",
                ChangeDate = DateTime.Now.ToString(Formats.DateTimeFormatSQL)
            };
            base.View.AddResidentialAddress(address);
            int addressKey = AddressAssertions.AssertResidentialAddressRecordExists(address.StreetNumber, address.StreetName, address.RRR_ProvinceDescription,
                address.RRR_SuburbDescription);
            string addressDescription = Service<ILegalEntityAddressService>().GetFormattedAddressByKey(addressKey);
            base.Browser.Navigate<LoanServicingCBO>().AddressDetails(NodeTypeEnum.Delete);
            base.View.DeleteAddress(addressDescription);
            AddressAssertions.AssertAddressLegalEntityLinkByGeneralStatus(_legalEntityKey, addressKey, GeneralStatusEnum.Inactive);
        }

        /// <summary>
        /// This test ensures that the user can not delete a legal entity address record if it is the only address record for the legal entity.
        /// </summary>
        [Test]
        public void DeleteOnlyLegalEntityAddress()
        {
            Service<ILegalEntityAddressService>().SetupAddressData(_legalEntityKey);
            base.Browser.Navigate<LoanServicingCBO>().AddressDetails(NodeTypeEnum.Delete);
            var address = Service<ILegalEntityAddressService>().GetLegalEntityAddresses(_legalEntityKey).FirstOrDefault();
            base.View.DeleteAddress(address.DelimitedAddress);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                string.Format(@"The address cannot be removed on a MainApplicant or Suretor when it is the only Legal Entity Address."));
        }

        /// <summary>
        /// This test ensures that the user can not delete a legal entity address record that is used as the mailing address for the account.
        /// </summary>
        [Test]
        public void DeleteLegalEntityAddressMailingAddress()
        {
            Service<ILegalEntityAddressService>().SetupAddressData(_legalEntityKey);
            Service<ILegalEntityAddressService>().SetupAddressAsAccountMailingAddress(_legalEntityKey, Account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().AddressDetails(NodeTypeEnum.Delete);
            var address = Service<ILegalEntityAddressService>().GetLegalEntityAddresses(_legalEntityKey).FirstOrDefault();
            base.View.DeleteAddress(address.DelimitedAddress);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                string.Format(@"The address cannot be removed from the legal entity as it is the mailing address for an open account ({0})", Account.AccountKey));
        }

        [Test]
        public void when_deleting_a_legalEntityAddress_linked_to_an_active_domicilium_the_user_is_not_allowed_to_continue()
        {
            //we need a new legal entity address
            var legalEntityAddress = Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(_legalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential,
                GeneralStatusEnum.Active);
            //remove any existing domicilium
            Service<ILegalEntityAddressService>().DeleteLegalEntityDomiciliumAddress(_legalEntityKey);
            var legalEntityName = Service<ILegalEntityService>().GetLegalEntityLegalName(_legalEntityKey);
            //link new address to active domicilium
            var domiciliumAddress = Service<ILegalEntityAddressService>().InsertLegalEntityDomiciliumAddress(legalEntityAddress.LegalEntityAddressKey, _legalEntityKey, GeneralStatusEnum.Active);
            //update it
            base.Browser.Navigate<LoanServicingCBO>().AddressDetails(NodeTypeEnum.Delete);
            base.View.DeleteAddress(domiciliumAddress.DelimitedAddress);
            //warning is displayed
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains
                (string.Format(@"This address is linked to the Active domicilium address for {0} and cannot be deleted.", legalEntityName));
            //confirm and ensure update
        }

        #endregion LegalEntityAddressDelete

        #region Validation

        /// <summary>
        /// This test ensures that the expected address types are available in the select list.
        /// </summary>
        [Test]
        public void ValidateAddressTypes()
        {
            Navigate(NodeTypeEnum.Add);
            List<string> expectedAddressTypes = new List<string> { AddressType.Residential, AddressType.Postal };
            var list = base.View.GetAddressTypeList();
            WatiNAssertions.AssertSelectListContents(list, expectedAddressTypes);
        }

        /// <summary>
        /// This test ensures that the expected residential address formats are available in the select list.
        /// </summary>
        [Test]
        public void ValidateResidentialAddressFormats()
        {
            Navigate(NodeTypeEnum.Add);
            List<string> expectedAddressFormats = new List<string> { AddressFormat.Street, AddressFormat.FreeText };
            base.View.SelectAddressType(AddressType.Residential);
            var list = base.View.GetAddressFormatList();
            WatiNAssertions.AssertSelectListContents(list, expectedAddressFormats);
        }

        /// <summary>
        /// This test ensures that the expected postal address formats are available in the select list.
        /// </summary>
        [Test]
        public void ValidatePostalAddressFormats()
        {
            Navigate(NodeTypeEnum.Add);
            List<string> expectedAddressFormats = new List<string> { AddressFormat.Box, AddressFormat.ClusterBox,
                AddressFormat.PostNetSuite, AddressFormat.PrivateBag, AddressFormat.Street};
            base.View.SelectAddressType(AddressType.Postal);
            var list = base.View.GetAddressFormatList();
            WatiNAssertions.AssertSelectListContents(list, expectedAddressFormats);
        }

        /// <summary>
        /// This test validates the expected mandatory fields for a residential street address.
        /// </summary>
        [Test]
        public void ValidateMandatoryFieldsResidentialStreet()
        {
            Navigate(NodeTypeEnum.Add);
            base.View.SelectAddressType(AddressType.Residential);
            base.View.SelectAddressFormat(AddressFormat.Street);
            base.View.ClickAdd();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("When no unit number, building number or building name is supplied, Street Number is required.",
                "A street number, building number or unit number is required.",
                "Please enter a valid street name.",
                "Street Name is a mandatory field",
                "Suburb is a mandatory field");
        }

        /// <summary>
        /// This test validates the expected mandatory fields for a residential free text address.
        /// </summary>
        [Test]
        public void ValidateMandatoryFieldsResidentialFreeText()
        {
            Navigate(NodeTypeEnum.Add);
            base.View.SelectAddressType(AddressType.Residential);
            base.View.SelectAddressFormat(AddressFormat.FreeText);
            base.View.ClickAdd();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("A Freetext address must have at least one line specified",
                "A Freetext address must have a country specified");
        }

        /// <summary>
        /// This test validates the expected mandatory fields for a postal box address.
        /// </summary>
        [Test]
        public void ValidateMandatoryFieldsPostalBox()
        {
            Navigate(NodeTypeEnum.Add);
            base.View.SelectAddressType(AddressType.Postal);
            base.View.SelectAddressFormat(AddressFormat.Box);
            base.View.ClickAdd();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Box Number is a mandatory field",
                "Post Office is a mandatory field");
        }

        /// <summary>
        /// This test validates the expected mandatory fields for a postal cluster box address.
        /// </summary>
        [Test]
        public void ValidateMandatoryFieldsPostalClusterBox()
        {
            Navigate(NodeTypeEnum.Add);
            base.View.SelectAddressType(AddressType.Postal);
            base.View.SelectAddressFormat(AddressFormat.ClusterBox);
            base.View.ClickAdd();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Cluster Box Number is a mandatory field",
                "Post Office is a mandatory field");
        }

        /// <summary>
        /// This test validates the expected mandatory fields for a postal post net suite address.
        /// </summary>
        [Test]
        public void ValidateMandatoryFieldsPostalPostNetSuite()
        {
            Navigate(NodeTypeEnum.Add);
            base.View.SelectAddressType(AddressType.Postal);
            base.View.SelectAddressFormat(AddressFormat.PostNetSuite);
            base.View.ClickAdd();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Private Bag Number is a mandatory field",
                "Suite Number is a mandatory field",
                "Post Office is a mandatory field");
        }

        /// <summary>
        /// This test validates the expected mandatory fields for a postal private bag address.
        /// </summary>
        [Test]
        public void ValidateMandatoryFieldsPostalPrivateBag()
        {
            Navigate(NodeTypeEnum.Add);
            base.View.SelectAddressType(AddressType.Postal);
            base.View.SelectAddressFormat(AddressFormat.PrivateBag);
            base.View.ClickAdd();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Private Bag Number is a mandatory field",
                "Post Office is a mandatory field");
        }

        /// <summary>
        /// This test validates the expected mandatory fields for a postal street address.
        /// </summary>
        [Test]
        public void ValidateMandatoryFieldsPostalStreet()
        {
            Navigate(NodeTypeEnum.Add);
            base.View.SelectAddressType(AddressType.Postal);
            base.View.SelectAddressFormat(AddressFormat.Street);
            base.View.ClickAdd();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("When no unit number, building number or building name is supplied, Street Number is required.",
                "A street number, building number or unit number is required.",
                "Please enter a valid street name.",
                "Street Name is a mandatory field",
                "Suburb is a mandatory field");
        }

        #endregion Validation

        #region Helper

        /// <summary>
        /// Search for the account load it onto the CBO and navigate to the specified Address Details node.
        /// </summary>
        private void Navigate(NodeTypeEnum node)
        {
            base.Browser.Navigate<LoanServicingCBO>().AddressDetails(node);
        }

        private void TestStart()
        {
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            Account = base.Service<IAccountService>().GetVariableLoanAccountByMainApplicantCount(1, 1, Common.Enums.AccountStatusEnum.Open);
            _legalEntityKey = base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(Account.AccountKey);
        }

        #endregion Helper
    }
}