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
    public sealed class EmployerAddTests : TestBase<EmployerAdd>
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
            base.Browser.Navigate<AdministrationActions>().AddEmployerDetails();
            base.OnTestStart();
        }

        #region Tests

        [Test]
        public void EmployerAdd_WhenEmployerNameNOTCaptured_ShouldShowEmployerNameMandatoryMessage()
        {
            //---------------Set up test -------------------
            var employer = Service<IEmploymentService>().GetFullyPopulatedEmployer();
            employer.Name = String.Empty;

            //---------------Assert Precondition----------------
            base.View.Populate(employer);

            //---------------Execute Test ----------------------
            base.View.ClickAdd();

            //---------------Test Result -----------------------
            base.View.AssertEmployerNameMandatoryMessage();
        }

        [Test]
        public void EmployerAdd_WhenBusinessTypeNOTCaptured_ShouldShowBusinessTypeMandatoryMessage()
        {
            //---------------Set up test -------------------
            var employer = Service<IEmploymentService>().GetFullyPopulatedEmployer();
            employer.EmployerBusinessTypeDescription = "- Please select -";

            //---------------Assert Precondition----------------
            base.View.Populate(employer);

            //---------------Execute Test ----------------------
            base.View.ClickAdd();

            //---------------Test Result -----------------------
        }

        [Test]
        public void EmployerAdd_WhenEmploymentSectorNOTCaptured_ShouldShowEmploymentSectorMandatoryMessage()
        {
            //---------------Set up test -------------------
            var employer = Service<IEmploymentService>().GetFullyPopulatedEmployer();
            employer.EmploymentSectorDescription = "- Please select -";

            //---------------Assert Precondition----------------
            base.View.Populate(employer);

            //---------------Execute Test ----------------------
            base.View.ClickAdd();

            //---------------Test Result -----------------------
            base.View.AssertEmploymentSectorMandatoryMessage();
        }

        [Test]
        public void EmployerAdd_WhenContactPersonNOTCaptured_ShouldShowContactPersonMandatoryMessage()
        {
            //---------------Set up test -------------------
            var employer = Service<IEmploymentService>().GetFullyPopulatedEmployer();
            employer.ContactPerson = String.Empty;

            //---------------Assert Precondition----------------
            base.View.Populate(employer);

            //---------------Execute Test ----------------------
            base.View.ClickAdd();

            //---------------Test Result -----------------------
            base.View.AssertContactPersonMandatoryMessage();
        }

        [Test]
        public void EmployerAdd_WhenTeleponeCodeNOTCaptured_ShouldShowTelephoneCodeMandatoryMessage()
        {
            //---------------Set up test -------------------
            var employer = Service<IEmploymentService>().GetFullyPopulatedEmployer();
            employer.TelephoneCode = String.Empty;

            //---------------Assert Precondition----------------
            base.View.Populate(employer);

            //---------------Execute Test ----------------------
            base.View.ClickAdd();

            //---------------Test Result -----------------------
            base.View.AssertTelephoneCodeMandatoryMessage();
        }

        [Test]
        public void EmployerAdd_WhenTelephoneNumberNOTCaptured_ShouldShowTelephoneNumberMandatoryMessage()
        {
            //---------------Set up test -------------------
            var employer = Service<IEmploymentService>().GetFullyPopulatedEmployer();
            employer.TelephoneNumber = String.Empty;

            //---------------Assert Precondition----------------
            base.View.Populate(employer);

            //---------------Execute Test ----------------------
            base.View.ClickAdd();

            //---------------Test Result -----------------------
            base.View.AssertTelephoneNumberMandatoryMessage();
        }

        [Test]
        public void EmployerAdd_WhenSubmittingNewEmployer_ShouldPersistSucessfully()
        {
            //---------------Set up test -------------------
            var employer = Service<IEmploymentService>().GetFullyPopulatedEmployer();

            //---------------Assert Precondition----------------
            base.View.Populate(employer);

            //---------------Execute Test ----------------------
            base.View.ClickAdd();

            //---------------Test Result -----------------------
            BuildingBlocks.Assertions.EmploymentAssertions.AssertEmployer(employer);
        }

        #endregion Tests
    }
}