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
using SAHL.Web.Test.Presenters.LegalEntityDetailsTest;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Web.Test.Presenters.LegalEntityDetailsTest
{
    [Ignore]
    [TestFixture]
    public class LegalEntityDetailsLifeDisplayPresenterTest : LegalEntityDetailsPresenterTestBase
    {

        /// <summary>
        /// Test to see if the basic events are being hooked. 
        /// These events include ViewInitialised, ViewLoaded and ViewPreRender.
        /// </summary>
        [NUnit.Framework.Test]
        public void BasicEventsHookupTest()
        {
            // Setup default expectations: ViewInitialised, ViewLoaded and ViewPreRender.
            SetupBaseEventsExpectations();

            _mockery.ReplayAll();

            // check expectations
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsDisplayLife LegalEntityDetailsPresenter = new LegalEntityDetailsDisplayLife(_view, controller);
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

            base.SetupBaseEventsExpectations();

            // Whenever _view.Messages gets called, return new DomainMessageCollection()
            IDomainMessageCollection Messages = new DomainMessageCollection();
            SetupResult.For(_view.Messages).Return(Messages);

            // Expect a call to the NaturalPerson bind methods
            _view.BindLegalEntityReadOnlyNaturalPerson(null);
            LastCall.IgnoreArguments();

            // Expect to bind the insurable interest controls (readonly)
            _view.BindInsurableInterestReadonly("");
            LastCall.IgnoreArguments();

            // Setup expectations
            _mockery.ReplayAll();

            // check expectations
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsDisplayLife LegalEntityDetailsLifeDisplay = new LegalEntityDetailsDisplayLife(_view, controller);

            // Raise the event and check the expectation
            _viewInitialisedRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        /// <summary>
        /// Check to see if the expectations are being met on Initialize
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewInitializeCompany()
        {
            // set a result for GetLegalEntityByKey
            ILegalEntityCompany LegalEntityCompany = _mockery.CreateMock<ILegalEntityCompany>();
            SetupResult.For(_legalEntityRepository.GetLegalEntityByKey(-1)).IgnoreArguments().Return(LegalEntityCompany);

            base.SetupBaseEventsExpectations();

            // Whenever _view.Messages gets called, return new DomainMessageCollection()
            IDomainMessageCollection Messages = new DomainMessageCollection();
            SetupResult.For(_view.Messages).Return(Messages);

            // Expect a call to the NaturalPerson bind methods
            _view.BindLegalEntityReadOnlyCompany(null);
            LastCall.IgnoreArguments();

            // Expect to bind the insurable interest controls (readonly)
            _view.BindInsurableInterestReadonly("");
            LastCall.IgnoreArguments();

            // Setup expectations
            _mockery.ReplayAll();

            // check expectations
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsDisplayLife LegalEntityDetailsLifeDisplay = new LegalEntityDetailsDisplayLife(_view, controller);

            // Raise the event and check the expectation
            _viewInitialisedRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();

        }


        /// <summary>
        /// Check to see if the expectations are being met on Initialize
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewPreRenderCompany()
        {
            // Setup a mock for ILegalEntityCompany
            ILegalEntityCompany LegalEntityCompany = _mockery.CreateMock<ILegalEntityCompany>();

            base.SetupBaseEventsExpectations();

            _view.PanelAddVisible = false;
            _view.PanelNaturalPersonAddVisible = false;
            _view.PanelCompanyAddVisible = false;
            _view.UpdateControlsVisible = false;
            _view.LockedUpdateControlsVisible = false;
            _view.NonContactDetailsDisabled = false;
            _view.InsurableInterestDisplayVisible = true;
            _view.InsurableInterestUpdateVisible= true;
            _view.PanelCompanyDisplayVisible = true;

            // Setup expectations
            _mockery.ReplayAll();

            // check expectations
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsDisplayLife LegalEntityDetailsLifeDisplay = new LegalEntityDetailsDisplayLife(_view, controller);

            LegalEntityDetailsLifeDisplay.LegalEntity = LegalEntityCompany;

            // Raise the event and check the expectation
            // Simulate the ILegalEntityCompany scenario
            _viewPreRenderRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        /// <summary>
        /// Check to see if the expectations are being met on Initialize
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewPreRenderNaturalPerson()
        {
            // Setup a mock for ILegalEntityCompany
            ILegalEntityNaturalPerson LegalEntityNaturalPerson = _mockery.CreateMock<ILegalEntityNaturalPerson>();

            base.SetupBaseEventsExpectations();

            _view.PanelAddVisible = false;
            _view.PanelNaturalPersonAddVisible = false;
            _view.PanelCompanyAddVisible = false;
            _view.UpdateControlsVisible = false;
            _view.LockedUpdateControlsVisible = false;
            _view.NonContactDetailsDisabled= false;
            _view.InsurableInterestDisplayVisible = true;
            _view.InsurableInterestUpdateVisible= true;
            _view.PanelCompanyDisplayVisible = false;

            // Setup expectations
            _mockery.ReplayAll();

            // check expectations
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityDetailsDisplayLife LegalEntityDetailsLifeDisplay = new LegalEntityDetailsDisplayLife(_view, controller);

            // Simulate the ILegalEntityCompany scenario
            LegalEntityDetailsLifeDisplay.LegalEntity = LegalEntityNaturalPerson;

            // Raise the event and check the expectation
            _viewPreRenderRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }
    }
}
