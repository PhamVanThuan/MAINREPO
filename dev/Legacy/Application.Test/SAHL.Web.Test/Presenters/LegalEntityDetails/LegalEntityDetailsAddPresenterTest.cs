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
using Rhino.Mocks.Impl;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Test.Presenters.LegalEntityDetailsTest
{

    [Ignore]
    [TestFixture]
    public class LegalEntityDetailsAddPresenterTest : LegalEntityDetailsPresenterTestBase
    {
        private IEventRaiser _cancelButtonClickRaiser;
        private IEventRaiser _submitButtonClickRaiser;
        private IEventRaiser _legalEntityTypeChangeRaiser;

        #region Unit Tests

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
            LegalEntityDetailsAdd LegalEntityDetailsPresenter = new LegalEntityDetailsAdd(_view, controller);

            _mockery.VerifyAll();
        }

        /// <summary>
        /// Check to see if the expectations are being met on Initialize
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewInitialize()
        {
            // Move to PreRender ...
            // base.SetupBasicAddExpectancies();

            SetupBaseEventsExpectations();

            BindLookupsExpectancies();

            // mock the ILookupRepository
            //lookupRepository
            IDictionary<int, string> fakeLookup = new Dictionary<int, string>();

            SetupResult.For(base.LookupRepository.LegalEntityTypes).Return(fakeLookup);
            SetupResult.For(base.LookupRepository.RoleTypes).Return(fakeLookup);
            SetupResult.For(base.LookupRepository.Salutations).Return(fakeLookup);
            SetupResult.For(base.LookupRepository.Genders).Return(fakeLookup);
            SetupResult.For(base.LookupRepository.MaritalStatuses).Return(fakeLookup);
            SetupResult.For(base.LookupRepository.PopulationGroups).Return(fakeLookup);
            SetupResult.For(base.LookupRepository.Educations).Return(fakeLookup);
            SetupResult.For(base.LookupRepository.CitizenTypes).Return(fakeLookup);
            SetupResult.For(base.LookupRepository.Languages).Return(fakeLookup);
            SetupResult.For(base.LookupRepository.ResidenceStatuses).Return(fakeLookup);
            SetupResult.For(base.LookupRepository.LifeInsurableInterestTypes).Return(fakeLookup);
            SetupResult.For(base.LookupRepository.LegalEntityStatuses).Return(fakeLookup);

            _mockery.ReplayAll();

            // check expectations
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsAdd LegalEntityDetailsPresenter = new LegalEntityDetailsAdd(_view, controller);

            // raise the init event and check teh results
            _viewInitialisedRaiser.Raise(_view, new EventArgs());

            _mockery.VerifyAll();

        }

        /// <summary>
        /// Check to see if the expectations are being met on Initialize
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewPreRender()
        {
            SetupBaseEventsExpectations();
            SetupBasicAddExpectancies();

            _view.CancelButtonVisible = true;
            _view.SubmitButtonText = "Update";

            _mockery.ReplayAll();

            // check expectations
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsAdd LegalEntityDetailsPresenter = new LegalEntityDetailsAdd(_view, controller);

            _viewPreRenderRaiser.Raise(_view, new EventArgs());

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
            LegalEntityDetailsAdd LegalEntityDetailsPresenter = new LegalEntityDetailsAdd(_view, controller);
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
            LegalEntityDetailsAdd LegalEntityDetailsPresenter = new LegalEntityDetailsAdd(_view, controller);
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
            LegalEntityDetailsAdd LegalEntityDetailsPresenter = new LegalEntityDetailsAdd(_view, controller);
            _submitButtonClickRaiser.Raise(_view, new EventArgs());

            _mockery.VerifyAll();

        }

        /// <summary>
        /// Test the expectancies when the Cancel Button is clicked. NaturalPerson.
        /// </summary>
        [NUnit.Framework.Test]
        public void LegalEntityTypeChangeNaturalPerson()
        {
            SetupBaseEventsExpectations();

            _view.PanelNaturalPersonAddVisible = true;
            _view.PanelCompanyAddVisible = false;
            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsAdd LegalEntityDetailsPresenter = new LegalEntityDetailsAdd(_view, controller);
            _legalEntityTypeChangeRaiser.Raise(_view, new KeyChangedEventArgs(2));

            _mockery.VerifyAll();
        
        }


        /// <summary>
        /// Test the expectancies when the Cancel Button is clicked. Non-NaturalPerson.
        /// </summary>
        [NUnit.Framework.Test]
        public void LegalEntityTypeChangeNonNaturalPerson()
        {
            SetupBaseEventsExpectations();

            _view.PanelNaturalPersonAddVisible = false;
            _view.PanelCompanyAddVisible = true;

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsAdd LegalEntityDetailsPresenter = new LegalEntityDetailsAdd(_view, controller);
            _legalEntityTypeChangeRaiser.Raise(_view, new KeyChangedEventArgs(4));

            _mockery.VerifyAll();

        }

        #endregion

        #region Helper Functions
        protected override void SetupBaseEventsExpectations()
        {
            base.SetupBaseEventsExpectations();

            // Hook the additional events
            _view.onCancelButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            _cancelButtonClickRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.onSubmitButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            _submitButtonClickRaiser = LastCall.IgnoreArguments().GetEventRaiser(); 

            _view.onLegalEntityTypeChange += null;
            LastCall.Constraints(Is.NotNull());
            _legalEntityTypeChangeRaiser = LastCall.IgnoreArguments().GetEventRaiser();
        }
        #endregion
    }

}
