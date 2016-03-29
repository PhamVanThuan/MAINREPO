using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.LoanServicing.Correspondence;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace LoanServicingTests.Views.Account
{
    [RequiresSTA]
    public sealed class UpdatePropertyDetailTests : BuildingBlocks.TestBase<PropertyDetailsUpdate>
    {
        private int _accountKey = 0;
        private int _legalEntityKey = 0;
        private Random randomizer;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            this.randomizer = new Random();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);

            _accountKey = (from r in Service<IAccountService>().GetRandomVariableLoanAccountByMainApplicantCount(1, 1, AccountStatusEnum.Open)
                           select r.Column("accountkey").GetValueAs<int>()).FirstOrDefault();

            _legalEntityKey = Service<ILegalEntityService>().GetFirstLegalEntityMainApplicantOnAccount(_accountKey).Column("legalentitykey").GetValueAs<int>();

            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(_accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(_accountKey);
        }

        #region Test

        /// <summary>
        /// This test ensures that the details of a property can be updated in loan servicing.
        /// </summary>
        [Test]
        public void PropertyUpdateTest()
        {
            var accountProperty = Service<IPropertyService>().GetPropertyByAccountKey(_accountKey);
            accountProperty = Service<IPropertyService>().GetChangedProperty(accountProperty);
            base.Browser.Navigate<LoanServicingCBO>().ClickUpdateProperty();
            base.View.PopulateView(accountProperty);
            base.View.ClickUpdate();
            PropertyValuationAssertions.AssertProperty(accountProperty);
        }

        /// <summary>
        /// This test updates the deeds office details linked to a property using the functionality provided in loan servicing.
        /// </summary>
        [Test]
        public void PropertyDeedsUpdateTest()
        {
            var accountProperty = Service<IPropertyService>().GetPropertyByAccountKey(_accountKey);
            accountProperty = Service<IPropertyService>().GetChangedProperty(accountProperty, changeDeedsDetails: true);
            base.Browser.Navigate<LoanServicingCBO>().ClickUpdateDeedsOfficeDetails();

            base.View.PopulateView(accountProperty);
            base.View.ClickUpdate();
            PropertyValuationAssertions.AssertProperty(accountProperty);
        }

        /// <summary>
        /// A property has an address associated to it. This test will update that property address and ensure that the changes are saved.
        /// </summary>
        [Test]
        public void PropertyAddressUpdateTest()
        {
            var accountProperty = Service<IPropertyService>().GetPropertyByAccountKey(_accountKey);
            var propertyAddress = base.Service<IAddressService>().GetAddress("", "", "", "", "", "", "", "", addresskey: accountProperty.AddressKey);
            propertyAddress = Service<IAddressService>().GetChangedAddress(propertyAddress);
            base.Browser.Navigate<LoanServicingCBO>().ClickUpdatePropertyAddress();
            base.Browser.Page<PropertyAddressUpdate>().PopulateView(propertyAddress);
            base.Browser.Page<PropertyAddressUpdate>().ClickUpdate();
            PropertyValuationAssertions.AssertProperty(accountProperty, propertyAddress);
        }

        /// <summary>
        /// Navigates to the Update Property node and ensures that the default state of the presenter's controls matches the expected state.
        /// </summary>
        [Test]
        public void TestUpdatePropertyControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().ClickUpdateProperty();
            base.View.AssertPropertyControlsExists();
        }

        /// <summary>
        /// Navigates to the Update Deeds Office Details node and ensures that the default state of the presenter's controls matches the expected state.
        /// </summary>
        [Test]
        public void TestUpdatePropertyDeedsOfficeControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().ClickUpdateDeedsOfficeDetails();
            base.View.AssertPropertyDeedsOfficeControlsExists();
        }

        /// <summary>
        /// Navigates to the Update Property Address node and ensures that the default state of the presenter's controls matches the expected state.
        /// </summary>
        [Test]
        public void TestUpdatePropertyAddressControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().ClickUpdatePropertyAddress();
            base.Browser.Page<PropertyAddressUpdate>().AssertPropertyAddressControlsExists();
        }

        /// <summary>
        /// The test tries to update a property without providing the mandatory details. It then checks that the expected set of validation messages are
        /// displayed to the user.
        /// </summary>
        [Test]
        public void TestUpdatePropertyMandatoryControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().ClickUpdateProperty();
            var property = Service<IPropertyService>().GetEmptyProperty();
            base.View.PopulateView(property);
            base.View.ClickUpdate();

            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                   "Property Type is Required",
                   "Title Type is Required",
                   "Occupancy Type is Required",
                   "Area Classification is Required",
                   "Deeds Property Type is Required",
                   "Property Description line 1 is Required",
                   "Property Description line 2 is Required",
                   "Property Description line 3 is Required"
               );
        }

        /// <summary>
        /// The test tries to update the property's deeds office details without providing the mandatory details. It then checks that the expected set of validation messages are
        /// displayed to the user.
        /// </summary>
        [Test]
        public void TestUpdatePropertyDeedsOfficeMandatoryControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().ClickUpdateDeedsOfficeDetails();
            var property = Service<IPropertyService>().GetEmptyProperty();
            //Need to give it a value cause when we populate the screen without it we'll get a warning
            //and not errors, and we need to errors for this test.
            property.TitleDeedNumber = "1234567";

            base.View.PopulateView(property);
            base.View.ClickUpdate();

            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                    "Property Type is Required",
                    "Title Type is Required",
                    "Occupancy Type is Required",
                    "Area Classification is Required",
                    "Deeds Property Type is Required",
                    "Property Description line 1 is Required",
                    "Property Description line 2 is Required",
                    "Property Description line 3 is Required"
                );
        }

        /// <summary>
        /// The test tries to update the property's address without providing the mandatory details. It then checks that the expected set of validation messages are
        /// displayed to the user.
        /// </summary>
        [Test]
        public void TestUpdatePropertyAddressMandatoryControls()
        {
            var address = Service<IAddressService>().GetEmptyAddress();
            base.Browser.Navigate<LoanServicingCBO>().ClickUpdatePropertyAddress();
            address.RRR_ProvinceDescription = "Gauteng";

            base.Browser.Page<PropertyAddressUpdate>().PopulateView(address);
            base.Browser.Page<PropertyAddressUpdate>().ClickUpdate();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
               "When no unit number, building number or building name is supplied, Street Number is required.",
               "A street number, building number or unit number is required.",
               "Please enter a valid street name.",
               "Street Name is a mandatory field",
               "Suburb is a mandatory field"
           );
        }

        [Test]
        public void WarningDisplayedWhenUpdatingPropertyAddressThatIsSharedAsDomicilium()
        {
            var property = Service<IPropertyService>().GetPropertyByAccountKey(_accountKey);
            //remove all domiciliums for the LE
            Service<ILegalEntityAddressService>().DeleteLegalEntityDomiciliumAddress(_legalEntityKey);
            //make the property address a LEA
            var legalEntity = new Automation.DataModels.LegalEntity { LegalEntityKey = _legalEntityKey};
            var legalEntityAddress = new Automation.DataModels.LegalEntityAddress
            {
                AddressKey = property.AddressKey,
                LegalEntityKey = _legalEntityKey,
                LegalEntity = legalEntity,
                GeneralStatusKey = GeneralStatusEnum.Active,
                AddressTypeKey = AddressTypeEnum.Residential
            };
            //make it a domicilium
            legalEntityAddress = Service<ILegalEntityAddressService>().InsertLegalEntityAddress(legalEntityAddress);
            Service<ILegalEntityAddressService>().InsertLegalEntityDomiciliumAddress(legalEntityAddress.LegalEntityAddressKey, _legalEntityKey, GeneralStatusEnum.Active);
            base.Browser.Navigate<LoanServicingCBO>().ClickUpdatePropertyAddress();
            var address = Service<IAddressService>().GetAddress(addresskey: property.AddressKey);
            address = Service<IAddressService>().GetChangedAddress(address);
            base.Browser.Page<PropertyAddressUpdate>().PopulateView(address);
            base.Browser.Page<PropertyAddressUpdate>().ClickUpdate();
            var legalName = Service<ILegalEntityService>().GetLegalEntityLegalName(_legalEntityKey);
            base.Browser.Page<BasePageAssertions>().
                AssertValidationMessagesContains(string.Format(@"The same address is being used as the Active domicilium address for {0} and may also need to be updated.", legalName));
        }

        #endregion Test
    }
}