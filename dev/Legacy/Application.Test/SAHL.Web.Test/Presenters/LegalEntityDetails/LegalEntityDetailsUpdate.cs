using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Web.Views.Common.Interfaces;
using Rhino.Mocks.Interfaces;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using SAHL.Web.Views.Common.Presenters.LegalEntityDetails;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Presenters;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Web.Test.Presenters.LegalEntityDetailsTest;
using Rhino.Mocks.Impl;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Test.Presenters.LegalEntityDetailsTest
{
    [Ignore]
    [TestFixture]
    public class LegalEntityDetailsUpdateTest : LegalEntityDetailsUpdateTestBase
    {

        #region Unit Tests

        /// <summary>
        /// Check to see if the expectations are being met on Initialize
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewInitializeCompany()
        {

            // set a result for GetLegalEntityByKey
            ILegalEntityCompany LegalEntityCompany = _mockery.CreateMock<ILegalEntityCompany>();
            SetupResult.For(_legalEntityRepository.GetLegalEntityByKey(-1)).IgnoreArguments().Return(LegalEntityCompany);

            // Whenever _view.Messages gets called, return new DomainMessageCollection()
            IDomainMessageCollection Messages = new DomainMessageCollection();
            SetupResult.For(_view.Messages).Return(Messages);

            // Expect a call to the NaturalPerson bind methods
            _view.BindLegalEntityUpdatableCompany(null);
            LastCall.IgnoreArguments();

            SetupBaseEventsExpectations();

            BindLookupsExpectancies();

            // mock the ILookupRepository
            //lookupRepository
            IDictionary<int, string> fakeLookup = new Dictionary<int, string>();

            SetupResult.For(LookupRepository.LegalEntityTypes).Return(fakeLookup);
            SetupResult.For(LookupRepository.RoleTypes).Return(fakeLookup);
            SetupResult.For(LookupRepository.Salutations).Return(fakeLookup);
            SetupResult.For(LookupRepository.Genders).Return(fakeLookup);
            SetupResult.For(LookupRepository.MaritalStatuses).Return(fakeLookup);
            SetupResult.For(LookupRepository.PopulationGroups).Return(fakeLookup);
            SetupResult.For(LookupRepository.Educations).Return(fakeLookup);
            SetupResult.For(LookupRepository.CitizenTypes).Return(fakeLookup);
            SetupResult.For(LookupRepository.Languages).Return(fakeLookup);
            SetupResult.For(LookupRepository.ResidenceStatuses).Return(fakeLookup);
            SetupResult.For(LookupRepository.LegalEntityStatuses).Return(fakeLookup);
            SetupResult.For(LookupRepository.LifeInsurableInterestTypes).Return(fakeLookup);

            _mockery.ReplayAll();

            // check expectations
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsUpdate LegalEntityDetailsPresenter = new LegalEntityDetailsUpdate(_view, controller);

            // raise the init event and check teh results
            _viewInitialisedRaiser.Raise(_view, new EventArgs());

            _mockery.VerifyAll();

        }

        /// <summary>
        /// Check to see if the expectations are being met on Initialize
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewInitializeNaturalPerson()
        {

            // set a result for GetLegalEntityByKey
            ILegalEntityNaturalPerson LegalEntityNaturalPerson = _mockery.CreateMock<ILegalEntityNaturalPerson>();
            SetupResult.For(_legalEntityRepository.GetLegalEntityByKey(-1)).IgnoreArguments().Return(LegalEntityNaturalPerson);

            // Whenever _view.Messages gets called, return new DomainMessageCollection()
            IDomainMessageCollection Messages = new DomainMessageCollection();
            SetupResult.For(_view.Messages).Return(Messages);

            // Expect a call to the NaturalPerson bind methods
            _view.BindLegalEntityUpdatableNaturalPerson(null);
            LastCall.IgnoreArguments();

            SetupBaseEventsExpectations();

            BindLookupsExpectancies();

            // mock the ILookupRepository
            //lookupRepository
            IDictionary<int, string> fakeLookup = new Dictionary<int, string>();

            SetupResult.For(LookupRepository.LegalEntityTypes).Return(fakeLookup);
            SetupResult.For(LookupRepository.RoleTypes).Return(fakeLookup);
            SetupResult.For(LookupRepository.Salutations).Return(fakeLookup);
            SetupResult.For(LookupRepository.Genders).Return(fakeLookup);
            SetupResult.For(LookupRepository.MaritalStatuses).Return(fakeLookup);
            SetupResult.For(LookupRepository.PopulationGroups).Return(fakeLookup);
            SetupResult.For(LookupRepository.Educations).Return(fakeLookup);
            SetupResult.For(LookupRepository.CitizenTypes).Return(fakeLookup);
            SetupResult.For(LookupRepository.Languages).Return(fakeLookup);
            SetupResult.For(LookupRepository.ResidenceStatuses).Return(fakeLookup);
            SetupResult.For(LookupRepository.LegalEntityStatuses).Return(fakeLookup);
            SetupResult.For(LookupRepository.LifeInsurableInterestTypes).Return(fakeLookup);

            _mockery.ReplayAll();

            // check expectations
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsUpdate LegalEntityDetailsPresenter = new LegalEntityDetailsUpdate(_view, controller);

            // raise the init event and check teh results
            _viewInitialisedRaiser.Raise(_view, new EventArgs());

            _mockery.VerifyAll();

        }

        /// <summary>
        /// Test to see if the basic events are being hooked. 
        /// These events include ViewInitialised, ViewLoaded and ViewPreRender.
        /// </summary>
        [NUnit.Framework.Test]
        public void EventsHookupOnCreation()
        {
            // Setup default expectations: ViewInitialised, ViewLoaded and ViewPreRender.
            SetupBaseEventsExpectations();

            _mockery.ReplayAll();

            // check expectations
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsUpdate LegalEntityDetailsPresenter = new LegalEntityDetailsUpdate(_view, controller);

            _mockery.VerifyAll();
        }

        /// <summary>
        /// Test the expectancies when the Cancel Button is clicked.
        /// </summary>
        [NUnit.Framework.Test]
        public void CancelButtonClick()
        {
            SetupBaseEventsExpectations();

            ISimpleNavigator navigator = _mockery.CreateMock<ISimpleNavigator>();
            SetupResult.For(_view.Navigator).Return(navigator);

            navigator.Navigate("Cancel");

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsUpdate LegalEntityDetailsPresenter = new LegalEntityDetailsUpdate(_view, controller);
            _cancelButtonClickRaiser.Raise(_view, new EventArgs());

            _mockery.VerifyAll();

        }

        /// <summary>
        /// Test the expectancies when the Submit button is clicked. Should not Navigate to Update if there're errors
        /// </summary>
        [NUnit.Framework.Test]
        public void SubmitButtonClickErrors()
        {
            SetupBaseEventsExpectations();

            IDomainMessageCollection domainMessages = new DomainMessageCollection();
            IDomainMessage domainMessage = new DomainMessage("", "");
            domainMessages.Add(domainMessage);
            SetupResult.For(_view.Messages).Return(domainMessages);

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsUpdate LegalEntityDetailsPresenter = new LegalEntityDetailsUpdate(_view, controller);
            _submitButtonClickRaiser.Raise(_view, new EventArgs());

            _mockery.VerifyAll();

        }

        /// <summary>
        /// Test the expectancies when the Submit button is clicked. Should  Navigate to Update as there're no errors
        /// </summary>
        [NUnit.Framework.Test]
        public void SubmitButtonClickNoErrors()
        {
            SetupBaseEventsExpectations();

            // Simulate no errors on save
            IDomainMessageCollection domainMessages = new DomainMessageCollection();
            SetupResult.For(_view.Messages).Return(domainMessages);

            // setup the expectation for Navigate("Update")
            ISimpleNavigator navigator = _mockery.CreateMock<ISimpleNavigator>();
            SetupResult.For(_view.Navigator).Return(navigator);
            navigator.Navigate("Update");

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsUpdate LegalEntityDetailsPresenter = new LegalEntityDetailsUpdate(_view, controller);
            _submitButtonClickRaiser.Raise(_view, new EventArgs());

            _mockery.VerifyAll();

        }

        /// <summary>
        /// Check to see if the expectations are being met on PreRender
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewPreRenderCompany()
        {
            // Setup a mock for ILegalEntityCompany
            ILegalEntityCompany LegalEntityCompany = _mockery.CreateMock<ILegalEntityCompany>();
            ILegalEntityExceptionStatus LegalEntityExceptionStatus = _mockery.CreateMock<ILegalEntityExceptionStatus>();

            SetupBaseEventsExpectations();

            _view.PanelAddVisible = false;
            _view.PanelNaturalPersonAddVisible = false;
            _view.PanelCompanyAddVisible = false;
            _view.UpdateControlsVisible = false;
            _view.LockedUpdateControlsVisible = false;
            _view.InsurableInterestDisplayVisible = false;
            _view.InsurableInterestUpdateVisible= false;
            _view.PanelNaturalPersonDisplayVisible = false;
            _view.PanelCompanyDisplayVisible = false;

            // We don't care about these for now ...
            SetupResult.For(LegalEntityCompany.LegalEntityExceptionStatus).Return(LegalEntityExceptionStatus); // Has an Exception (override)
            SetupResult.For(LegalEntityExceptionStatus.Key).Return(1); // Has an Exception (override)
            //SetupResult.For(LegalEntityCompany.HasActiveAccounts).Return(true); // Has no ActiveAccounts

            // Setup expectations
            _mockery.ReplayAll();

            // check expectations
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsUpdate LegalEntityDetailsPresenter = new LegalEntityDetailsUpdate(_view, controller);
            LegalEntityDetailsPresenter.LegalEntity = LegalEntityCompany;

            // Raise the event and check the expectation
            // Simulate the ILegalEntityCompany scenario
            _viewPreRenderRaiser.Raise(_view, new EventArgs());

            _mockery.VerifyAll();
        }

        /// <summary>
        /// Check to see if the expectations are being met on PreRender
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewPreRenderNaturalPerson()
        {
            // Setup a mock for ILegalEntityCompany
            ILegalEntityNaturalPerson LegalEntityNaturalPerson = _mockery.CreateMock<ILegalEntityNaturalPerson>();
            ILegalEntityExceptionStatus LegalEntityExceptionStatus = _mockery.CreateMock<ILegalEntityExceptionStatus>();

            SetupBaseEventsExpectations();

            _view.PanelAddVisible = false;
            _view.PanelNaturalPersonAddVisible = false;
            _view.PanelCompanyAddVisible = false;
            _view.UpdateControlsVisible = false;
            _view.LockedUpdateControlsVisible = true;
            _view.InsurableInterestDisplayVisible = false;
            _view.InsurableInterestUpdateVisible= false;
            _view.PanelCompanyDisplayVisible = false;
            _view.PanelNaturalPersonDisplayVisible = false;

            // We don't care about these for now ...
            SetupResult.For(LegalEntityNaturalPerson.LegalEntityExceptionStatus).Return(LegalEntityExceptionStatus); // Has an Exception (override)
            SetupResult.For(LegalEntityExceptionStatus.Key).Return(3); // Has an Exception (override)
            //SetupResult.For(LegalEntityNaturalPerson.HasActiveAccounts).Return(true); // Has no ActiveAccounts

            // Setup expectations
            _mockery.ReplayAll();

            // check expectations
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsUpdate LegalEntityDetailsPresenter = new LegalEntityDetailsUpdate(_view, controller);

            // Simulate the ILegalEntityCompany scenario
            LegalEntityDetailsPresenter.LegalEntity = LegalEntityNaturalPerson;

            // Raise the event and check the expectation
            _viewPreRenderRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        /// <summary>
        /// Check to see if the expectations are being met on PreRender
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewPreRenderExceptionStatusThree()
        {
            // Setup a mock for ILegalEntityCompany
            ILegalEntityNaturalPerson LegalEntityNaturalPerson = _mockery.CreateMock<ILegalEntityNaturalPerson>();
            ILegalEntityExceptionStatus LegalEntityExceptionStatus = _mockery.CreateMock<ILegalEntityExceptionStatus>();

            SetupBaseEventsExpectations();

            _view.PanelAddVisible = false;
            _view.PanelNaturalPersonAddVisible = false;
            _view.PanelCompanyAddVisible = false;
            _view.UpdateControlsVisible = false;
            _view.InsurableInterestDisplayVisible= false;
            _view.InsurableInterestDisplayVisible= false;
            _view.PanelCompanyDisplayVisible = false;
            _view.PanelNaturalPersonDisplayVisible = false;

            // ExceptionStatusThree Expectancies
            _view.LockedUpdateControlsVisible = true;

            // Simulate the test conditions
            SetupResult.For(LegalEntityNaturalPerson.LegalEntityExceptionStatus).Return(LegalEntityExceptionStatus); // Has an Exception (override)
            SetupResult.For(LegalEntityExceptionStatus.Key).Return(3); // Has an Exception (override)
            //SetupResult.For(LegalEntityNaturalPerson.HasActiveAccounts).Return(true); // Has no ActiveAccounts

            // Setup expectations
            _mockery.ReplayAll();

            // check expectations
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsUpdate LegalEntityDetailsPresenter = new LegalEntityDetailsUpdate(_view, controller);

            // Simulate the ILegalEntityCompany scenario
            LegalEntityDetailsPresenter.LegalEntity = LegalEntityNaturalPerson;

            // Raise the event and check the expectation
            _viewPreRenderRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        /// <summary>
        /// Check to see if the expectations are being met on PreRender
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewPreRenderExceptionStatusNotThree()
        {
            // Setup a mock for ILegalEntityCompany
            ILegalEntityNaturalPerson LegalEntityNaturalPerson = _mockery.CreateMock<ILegalEntityNaturalPerson>();
            ILegalEntityExceptionStatus LegalEntityExceptionStatus = _mockery.CreateMock<ILegalEntityExceptionStatus>();

            SetupBaseEventsExpectations();

            _view.PanelAddVisible = false;
            _view.PanelNaturalPersonAddVisible = false;
            _view.PanelCompanyAddVisible = false;
            _view.UpdateControlsVisible = false;
            _view.InsurableInterestDisplayVisible= false;
            _view.InsurableInterestUpdateVisible= false;
            _view.PanelCompanyDisplayVisible = false;
            _view.PanelNaturalPersonDisplayVisible = false;

            // ExceptionStatusThree Expectancies
            _view.LockedUpdateControlsVisible = false;

            // Simulate the test conditions
            SetupResult.For(LegalEntityNaturalPerson.LegalEntityExceptionStatus).Return(LegalEntityExceptionStatus); // Has an Exception (override)
            SetupResult.For(LegalEntityExceptionStatus.Key).Return(1); // Has an ExceptionStatus of 0
            //SetupResult.For(LegalEntityNaturalPerson.HasActiveAccounts).Return(true); // Has ActiveAccounts

            // Setup expectations done
            _mockery.ReplayAll();

            // check expectations
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsUpdate LegalEntityDetailsPresenter = new LegalEntityDetailsUpdate(_view, controller);

            // Simulate the ILegalEntityCompany scenario
            LegalEntityDetailsPresenter.LegalEntity = LegalEntityNaturalPerson;

            // Raise the event and check the expectation
            _viewPreRenderRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        /// <summary>
        /// Check to see if the expectations are being met on PreRender
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewPreRenderHasNoActiveAccounts()
        {
            // Setup a mock for ILegalEntityCompany
            ILegalEntityNaturalPerson LegalEntityNaturalPerson = _mockery.CreateMock<ILegalEntityNaturalPerson>();
            ILegalEntityExceptionStatus LegalEntityExceptionStatus = _mockery.CreateMock<ILegalEntityExceptionStatus>();

            SetupBaseEventsExpectations();

            _view.PanelAddVisible = false;
            _view.PanelNaturalPersonAddVisible = false;
            _view.PanelCompanyAddVisible = false;
            _view.UpdateControlsVisible = false;
            _view.InsurableInterestDisplayVisible= false;
            _view.InsurableInterestUpdateVisible= false;
            _view.PanelCompanyDisplayVisible = false;
            _view.PanelNaturalPersonDisplayVisible = false;

            // ExceptionStatusThree Expectancies
            _view.LockedUpdateControlsVisible = true;

            // Simulate the test conditions
            SetupResult.For(LegalEntityNaturalPerson.LegalEntityExceptionStatus).Return(LegalEntityExceptionStatus); // Has an Exception (override)
            SetupResult.For(LegalEntityExceptionStatus.Key).Return(0); // Has an ExceptionStatus of 3
            //SetupResult.For(LegalEntityNaturalPerson.HasActiveAccounts).Return(false); // Has no ActiveAccounts

            // Setup expectations done
            _mockery.ReplayAll();

            // check expectations
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsUpdate LegalEntityDetailsPresenter = new LegalEntityDetailsUpdate(_view, controller);

            // Simulate the ILegalEntityCompany scenario
            LegalEntityDetailsPresenter.LegalEntity = LegalEntityNaturalPerson;

            // Raise the event and check the expectation
            _viewPreRenderRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        #endregion


        #region Helper Functions
        #endregion
    }

}