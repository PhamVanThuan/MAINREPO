using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.Admin;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace LoanServicingTests.Views.Administration
{
    [RequiresSTA]
    public sealed class SubsidyProviderUpdateTests : TestBase<SubsidyProviderDetailsUpdate>
    {
        private Automation.DataModels.SubsidyProvider insertedSubsidyProvider;


        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
        }

        protected override void OnTestStart()
        {
            //Insert a subsidy provider for the next test.
            var subsidyProviderToAdd = Service<IEmploymentService>().GetFullyPopulatedSubsidyProvider();
            this.insertedSubsidyProvider = Service<ILegalEntityService>().InsertSubsidyProvider(subsidyProviderToAdd);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
            base.Browser.Navigate<BuildingBlocks.Navigation.CBO.AdministrationActions>().UpdateSubsidyProvider();
            base.OnTestStart();
        }

        protected override void OnTestTearDown()
        {
            Service<ILegalEntityService>().DeleteSubsidyProvider(insertedSubsidyProvider.LegalEntity.LegalEntityKey,
                                                    insertedSubsidyProvider.LegalEntity.LegalEntityAddress.Address.AddressKey);
            base.OnTestTearDown();
        }

        protected override void OnTestFixtureTearDown()
        {
            Service<ILegalEntityService>().DeleteSubsidyProvider(insertedSubsidyProvider.LegalEntity.LegalEntityKey,
                                                    insertedSubsidyProvider.LegalEntity.LegalEntityAddress.Address.AddressKey);
            base.OnTestFixtureTearDown();
        }

        [Test]
        public void SubsidyProvider_WhenUpdatingSubsidyProvider_ShouldShowSubsidyProviderUpdateView()
        {
            base.View.AssertView("SubsidyProviderDetailsUpdate");
        }

        [Test]
        public void SubsidyProviderUpdate_WhenNOTCapturingContactPerson_ShouldGetLegalEntityContactPersonMandatoryErrorMessage()
        {
            //---------------Set up test -------------------
            insertedSubsidyProvider.ContactPerson = String.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            //base.View.Populate(insertedSubsidyProvider);
            base.View.ClearContactPerson(insertedSubsidyProvider);
            base.View.Submit();

            //---------------Test Result -----------------------
            base.View.AssertSubsidyProviderContactPersonMessageExist();
        }

        [Test]
        public void SubsidyProviderUpdate_WhenNOTCapturingContactNumber_ShouldGetAtLeastOneContactNumberRequiredErrorMessage()
        {
            //---------------Set up test -------------------
            var legalentity = insertedSubsidyProvider.LegalEntity;
            legalentity.WorkPhoneCode = String.Empty;
            legalentity.WorkPhoneNumber = String.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            //base.View.Populate(insertedSubsidyProvider);
            base.View.ClearContactNumber(insertedSubsidyProvider);
            base.View.Submit();

            //---------------Test Result -----------------------
            base.View.AssertSubsidyProviderContactNumberMessageExist();
        }

        /**********Removed test as email field is no longer mandatory**********
         * [Test]
        public void SubsidyProviderUpdate_WhenCapturingEmailInWrongFormat_ShouldGetCaptureEmailIncorrectFormatErrorMessage()
        {
            //---------------Set up test -------------------
            var legalentity = insertedSubsidyProvider.LegalEntity;
            //legalentity.EmailAddress = "test.com";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.View.Populate(insertedSubsidyProvider);
            base.View.EnterWrongEmail(insertedSubsidyProvider);
            base.View.Submit();

            //---------------Test Result -----------------------
            base.View.AssertEmailIncorrectFormatMessageExist();
        }
         ********************************************************************/

        [Test]
        public void SubsidyProviderUpdate_WhenBoxNumberIsBlank_ShouldGetSubsidyProviderAddressRequiredWarningMessage()
        {
            //---------------Set up test -------------------
            var address = insertedSubsidyProvider.LegalEntity.LegalEntityAddress.Address;
            address.AddressFormatKey = (int)AddressFormatEnum.None;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.View.PopulateWithoutAddressBoxNumber(insertedSubsidyProvider);
            base.View.Submit();

            //---------------Test Result -----------------------
            base.View.AssertUpdateAddressBoxNumberRequiredMessageExist();
        }

        [Test]
        public void SubsidyProviderUpdate_WhenPostOfficeIsBlank_ShouldGetSubsidyProviderAddressRequiredWarningMessage()
        {
            //---------------Set up test -------------------
            var address = insertedSubsidyProvider.LegalEntity.LegalEntityAddress.Address;
            address.AddressFormatKey = (int)AddressFormatEnum.None;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.View.PopulateWithoutAddressPostOffice(insertedSubsidyProvider);
            base.View.Submit();

            //---------------Test Result -----------------------
            base.View.AssertUpdateAddressPostBoxRequiredMessageExist();
        }

        [Test]
        public void SubsidyProviderUpdate_WhenUpdatingSubsidyProvider_ShouldPersistChanges()
        {
            //---------------Set up test -------------------
            var address = insertedSubsidyProvider.LegalEntity.LegalEntityAddress.Address;
            var legalentity = insertedSubsidyProvider.LegalEntity;
            insertedSubsidyProvider.ContactPerson = String.Format("{0}Updated", insertedSubsidyProvider.ContactPerson);
            insertedSubsidyProvider.LegalEntity.EmailAddress = String.Format("{0}Updated", insertedSubsidyProvider.LegalEntity.EmailAddress);

            //address.BuildingName = String.Format("{0}Updated", address.BuildingName);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.View.Populate(insertedSubsidyProvider);
            base.View.Submit();

            //---------------Test Result -----------------------
            LegalEntityAssertions.AssertSubsidyProviderSaved(insertedSubsidyProvider);
            LegalEntityAssertions.AssertLegalEntityPostalAddress(insertedSubsidyProvider.LegalEntity, address);
        }
    }
}