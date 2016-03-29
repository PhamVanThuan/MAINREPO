using BuildingBlocks;
using BuildingBlocks.Navigation.CBO;
using BuildingBlocks.Presenters.Admin;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using System;

namespace LoanServicingTests.Views.Administration
{
    [RequiresSTA]
    public sealed class SubsidyProviderAddTests : TestBase<SubsidyProviderDetailsAdd>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
        }

        protected override void OnTestStart()
        {
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
            base.Browser.Navigate<AdministrationActions>().AddSubsidyProvider();
            base.OnTestStart();
        }

        [Test]
        public void SubsidyProvider_WhenAddingSubsidyProvider_ShouldShowSubsidyProviderAddView()
        {
            base.View.AssertView("SubsidyProviderDetailsAdd");
        }

        [Test]
        public void SubsidyProviderAdd_WhenNOTCapturingSubsidyProviderName_ShouldGetLegalEntityCompanyNameRequiredErrorMessage()
        {
            //---------------Set up test -------------------
            var subsidyToAdd = Service<IEmploymentService>().GetFullyPopulatedSubsidyProvider();
            subsidyToAdd.LegalEntity.RegisteredName = String.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.View.Populate(subsidyToAdd);
            base.View.Submit();

            //---------------Test Result -----------------------
            base.View.AssertSubsidyProviderMandatoryMessageExist();
        }

        [Test]
        public void SubsidyProviderAdd_WhenNOTCapturingContactPerson_ShouldGetLegalEntityContactPersonMandatoryErrorMessage()
        {
            //---------------Set up test -------------------
            var subsidyToAdd = Service<IEmploymentService>().GetFullyPopulatedSubsidyProvider();
            subsidyToAdd.ContactPerson = String.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.View.Populate(subsidyToAdd);
            base.View.Submit();

            //---------------Test Result -----------------------
            base.View.AssertSubsidyProviderContactPersonMessageExist();
        }

        [Test]
        public void SubsidyProviderAdd_WhenNOTCapturingContactNumber_ShouldGetAtLeastOneContactNumberRequiredErrorMessage()
        {
            //---------------Set up test -------------------
            var subsidyToAdd = Service<IEmploymentService>().GetFullyPopulatedSubsidyProvider();
            subsidyToAdd.LegalEntity.WorkPhoneCode = String.Empty;
            subsidyToAdd.LegalEntity.WorkPhoneNumber = String.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.View.Populate(subsidyToAdd);
            base.View.Submit();

            //---------------Test Result -----------------------
            base.View.AssertSubsidyProviderContactNumberMessageExist();
        }

        [Test]
        public void SubsidyProviderAdd_WhenNOTCapturingSubsidyType_ShouldGetSubsidyProviderTypeMandatoryErrorMessage()
        {
            //---------------Set up test -------------------
            var subsidyToAdd = Service<IEmploymentService>().GetFullyPopulatedSubsidyProvider();
            subsidyToAdd.SubsidyProviderTypeDescription = "- Please Select -";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.View.Populate(subsidyToAdd);
            base.View.Submit();

            //---------------Test Result -----------------------
            base.View.AssertSubsidyProviderTypeMandatoryMessageExist();
        }

        [Test]
        public void SubsidyProviderAdd_WhenCapturingEmailInWrongFormat_ShouldGetCaptureEmailInCorrectFormatErrorMessage()
        {
            //---------------Set up test -------------------
            var subsidyToAdd = Service<IEmploymentService>().GetFullyPopulatedSubsidyProvider();
            subsidyToAdd.LegalEntity.EmailAddress = "test.com";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.View.Populate(subsidyToAdd);
            base.View.Submit();

            //---------------Test Result -----------------------
            base.View.AssertEmailIncorrectFormatMessageExist();
        }

        [Test]
        public void SubsidyProviderAdd_WhenNOTCapturingAddress_ShouldGetSubsidyProviderAddressRequiredWarningMessage()
        {
            //---------------Set up test -------------------
            var randomizer = new Random();
            var subsidyToAdd = Service<IEmploymentService>().GetFullyPopulatedSubsidyProvider();
            subsidyToAdd.LegalEntity.RegisteredName = String.Format("{0}{1}", subsidyToAdd.LegalEntity.RegisteredName, randomizer.Next(Int32.MinValue, Int32.MaxValue));
            subsidyToAdd.LegalEntity.LegalEntityAddress = null;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.View.Populate(subsidyToAdd);
            base.View.Submit();

            //---------------Test Result -----------------------
            base.View.AssertAddAddressRequiredMessageExist();
            base.View.Cancel();
        }

        [Test]
        public void SubsidyProvider_WhenAddingSubsidyProvider_ShouldPersistNewSubsidyProvider()
        {
            //---------------Set up test -------------------
            var randomizer = new Random();
            var subsidyToAdd = Service<IEmploymentService>().GetFullyPopulatedSubsidyProvider();
            subsidyToAdd.LegalEntity.RegisteredName = String.Format("{0}{1}", subsidyToAdd.LegalEntity.RegisteredName, randomizer.Next(Int32.MinValue, Int32.MaxValue));

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.View.Populate(subsidyToAdd);
            base.View.Submit();

            //---------------Test Result -----------------------
            BuildingBlocks.Assertions.LegalEntityAssertions.AssertSubsidyProviderSaved(subsidyToAdd);
            //BuildingBlocks.Assertions.LegalEntityAssertions.AssertLegalEntityPostalAddress(subsidyToAdd.LegalEntity, subsidyToAdd.LegalEntity.LegalEntityAddress.Address);
        }

        [Test]
        public void SubsidyProvider_WhenAddingSubsidyProviderThatAlreadyExist_ShouldGetSubsidyProviderAlreadyExistMessage()
        {
            //---------------Set up test -----------------------
            var subsidyToAdd = Service<IEmploymentService>().GetFullyPopulatedSubsidyProvider();
            var insertedSubsidyProvider = Service<ILegalEntityService>().InsertSubsidyProvider(subsidyToAdd);
            var legalentity = insertedSubsidyProvider.LegalEntity;
            var address = insertedSubsidyProvider.LegalEntity.LegalEntityAddress.Address;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.View.Populate(subsidyToAdd);
            base.View.Submit();

            //---------------Test Result -----------------------
            base.View.AssertSubsidyProviderAlreadyExist();

            //cleanup
            Service<ILegalEntityService>().DeleteSubsidyProvider(legalentity.LegalEntityKey, address.AddressKey);
        }
    }
}