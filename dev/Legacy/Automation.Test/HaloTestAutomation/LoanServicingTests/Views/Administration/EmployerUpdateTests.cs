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
    public sealed class EmployerUpdateTests : TestBase<EmployerUpdate>
    {
        private Automation.DataModels.Employer insertedEmployer;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            this.insertedEmployer = Service<IEmploymentService>().GetFullyPopulatedEmployer();
            this.insertedEmployer.Name = "test employer update";
            this.insertedEmployer = Service<IEmploymentService>().InsertEmployer(insertedEmployer);
        }

        protected override void OnTestFixtureTearDown()
        {
            Service<IEmploymentService>().DeleteEmployer(this.insertedEmployer.EmployerKey);
            base.OnTestFixtureTearDown();
        }

        protected override void OnTestStart()
        {
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
            base.Browser.Navigate<AdministrationActions>().UpdateEmployerDetails();
            base.OnTestStart();
        }

        #region Tests

        [Test]
        public void EmployerUpdate_WhenBusinessTypeNOTCaptured_ShouldShowBusinessTypeMandatoryMessage()
        {
            //---------------Set up test -------------------
            insertedEmployer.EmployerBusinessTypeDescription = "- Please Select -";

            //---------------Assert Precondition----------------
            base.View.Populate(insertedEmployer);

            //---------------Execute Test ----------------------
            base.View.ClickUpdate();

            //---------------Test Result -----------------------
            base.View.AssertBusinessTypeMandatoryMessage();
        }

        [Test]
        public void EmployerUpdate_WhenEmploymentSectorNOTCaptured_ShouldShowEmploymentSectorMandatoryMessage()
        {
            //---------------Set up test -------------------
            insertedEmployer.EmploymentSectorDescription = "- Please Select -";

            //---------------Assert Precondition----------------
            base.View.Populate(insertedEmployer);

            //---------------Execute Test ----------------------
            base.View.ClickUpdate();

            //---------------Test Result -----------------------
            base.View.AssertEmploymentSectorMandatoryMessage();
        }

        [Test]
        public void EmployerUpdate_WhenContactPersonNOTCaptured_ShouldShowContactPersonMandatoryMessage()
        {
            //---------------Set up test -------------------
            insertedEmployer.ContactPerson = String.Empty;

            //---------------Assert Precondition----------------
            base.View.Populate(insertedEmployer);

            //---------------Execute Test ----------------------
            base.View.ClickUpdate();

            //---------------Test Result -----------------------
            base.View.AssertContactPersonMandatoryMessage();
        }

        [Test]
        public void EmployerUpdate_WhenTeleponeCodeNOTCaptured_ShouldShowTelephoneCodeMandatoryMessage()
        {
            //---------------Set up test -------------------
            insertedEmployer.TelephoneCode = String.Empty;

            //---------------Assert Precondition----------------
            base.View.Populate(insertedEmployer);

            //---------------Execute Test ----------------------
            base.View.ClickUpdate();

            //---------------Test Result -----------------------
            base.View.AssertTelephoneCodeMandatoryMessage();
        }

        [Test]
        public void EmployerUpdate_WhenTelephoneNumberNOTCaptured_ShouldShowTelephoneNumberMandatoryMessage()
        {
            //---------------Set up test -------------------
            insertedEmployer.TelephoneNumber = String.Empty;

            //---------------Assert Precondition----------------
            base.View.Populate(insertedEmployer);

            //---------------Execute Test ----------------------
            base.View.ClickUpdate();

            //---------------Test Result -----------------------
            base.View.AssertTelephoneNumberMandatoryMessage();
        }

        [Test]
        public void EmployerUpdate_WhenUpdatingExistingEmployer_ShouldPersistSucessfully()
        {
            //---------------Set up test -------------------
            insertedEmployer.TelephoneCode = "210";
            insertedEmployer.TelephoneNumber = "7895462";
            insertedEmployer.EmployerBusinessTypeDescription = "Trust";
            insertedEmployer.EmployerBusinessTypeKey = Common.Enums.EmployerBusinessTypeEnum.Trust;
            insertedEmployer.EmploymentSectorDescription = "Mining";
            insertedEmployer.EmploymentSectorKey = Common.Enums.EmploymentSectorEnum.Mining;
            insertedEmployer.ContactPerson = "Contact Person Update";

            //---------------Assert Precondition----------------
            base.View.Populate(insertedEmployer);

            //---------------Execute Test ----------------------
            base.View.ClickUpdate();

            //---------------Test Result -----------------------
            BuildingBlocks.Assertions.EmploymentAssertions.AssertEmployer(insertedEmployer);
        }

        #endregion Tests
    }
}