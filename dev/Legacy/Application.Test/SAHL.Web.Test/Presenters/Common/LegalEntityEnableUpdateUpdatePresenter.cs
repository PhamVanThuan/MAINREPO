using System;
using System.Data;
using System.Configuration;
using System.Web;
using NUnit.Framework;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Rhino.Mocks.Constraints;
using SAHL.Web.Views.Common.Presenters;
using SAHL.Common.Web.UI;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Test;
using SAHL.Common.Exceptions;
using System.Reflection;
using Rhino.Mocks.Impl;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.UI;

namespace SAHL.Web.Test.Presenters.Common
{
    [Ignore]
    [TestFixture]
    public class LegalEntityEnableUpdateUpdatePresenter : TestViewBase
    {
        private ILegalEntityEnableUpdate _view;
        private ILegalEntityRepository _legalEntityRepository;
        private ILookupRepository _lookupRepository;
        private IResourceService _resourceService;
        private IEventRaiser _viewInitialisedRaiser;
        private IEventRaiser _viewPreRenderRaiser;
        private IEventRaiser _onSubmitButtonClickRaiser;
        private IEventRaiser _onCancelButtonClickRaiser;


        private ICBOService _cboService;

        [SetUp]
        protected void FixtureSetup()
        {
            _view = _mockery.CreateMock<ILegalEntityEnableUpdate>();

            base.SetupMockedView(_view);
            base.SetupPrincipalCache(base.TestPrincipal);

            // set our typefactory stratergy to be mock based, 
            // in order to force the TypeFactory to return mocks rather than
            // concrete classes.
            base.SetRepositoryStrategy(TypeFactoryStrategy.Mock);

            // Create a mocked ILegalEntityRepository object and add it to the MOCK Cache. Clear teh Cache just in case one exists already
            ClearMockCache();

            _cboService = _mockery.CreateMock<ICBOService>();
            base.MockCache.Add(typeof(ICBOService).ToString(), _cboService);

            _legalEntityRepository = _mockery.CreateMock<ILegalEntityRepository>();
            base.MockCache.Add(typeof(ILegalEntityRepository).ToString(), _legalEntityRepository);

            _resourceService  = _mockery.CreateMock<IResourceService>();
            base.MockCache.Add(typeof(IResourceService).ToString(), _resourceService);

            _lookupRepository = _mockery.CreateMock<ILookupRepository>();
            base.MockCache.Add(typeof(ILookupRepository).ToString(), _lookupRepository);

        }

        [TearDown]
        protected void FixtureTearDown()
        {
            _view = null;
            _legalEntityRepository = null;
            _cboService = null;
            ClearMockCache();
        }

        [NUnit.Framework.Test]
        public void EventsHookup()
        {
            SetupBaseEventsExpectations();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);

            LegalEntityEnableUpdateUpdate LegalEntityEnableUpdateUpdate = new LegalEntityEnableUpdateUpdate(_view, controller);

            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void ViewInitializeNodeFound()
        {
            SetupBaseEventsExpectations();

            // Setup the CBO current and LegalEntity node
            CBOMenuNode cboLegalEntityMenuNode = _mockery.CreateMock<CBOMenuNode>(new Dictionary<string, object>());

            // Setup the ILegalEntity to be returned
            ILegalEntity legalEntity = _mockery.CreateMock<ILegalEntity>();

            // Mock the CBO and get the LegalEntity
            Expect.Call(_cboService.GetCurrentCBONode(null, null)).IgnoreArguments().Return(cboLegalEntityMenuNode);
            Expect.Call(_legalEntityRepository.GetLegalEntityByKey(-1)).IgnoreArguments().Return(legalEntity);
            
            SetupResult.For(_view.Messages).Return(new DomainMessageCollection());
            SetupResult.For(cboLegalEntityMenuNode.GenericKey).Return((long)0);

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityEnableUpdateUpdate LegalEntityEnableUpdateUpdate = new LegalEntityEnableUpdateUpdate(_view, controller);

            _viewInitialisedRaiser.Raise(null, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void ViewPreRender()
        {
            SetupBaseEventsExpectations();

            // Expect a call to teh following bind methods
            _view.BindLabelMessage(String.Empty);
            LastCall.IgnoreArguments();

            _view.BindLabelQuestion(String.Empty);
            LastCall.IgnoreArguments();

            _view.BindCancelButtonText("No");
            _view.BindSubmitButtonText("Yes");

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityEnableUpdateUpdate legalEntityEnableUpdateUpdate = new LegalEntityEnableUpdateUpdate(_view, controller);

            _viewPreRenderRaiser.Raise(null, new EventArgs());
            _mockery.VerifyAll();

        }

        [NUnit.Framework.Test]
        public void ViewOnSubmitButtonClick()
        {
            SetupBaseEventsExpectations();

            ILegalEntity legalEntity = _mockery.CreateMock<ILegalEntity>();

            IEventList<ILegalEntityStatus> legalEntityExceptionStatuses = _mockery.CreateMock<IEventList<ILegalEntityStatus>>();
            // SetupResult.For(_lookupRepository.LegalEntityExceptionStatuses).Return(legalEntityExceptionStatuses);

            IDictionary<string, ILegalEntityStatus> objectDictionary = _mockery.CreateMock<IDictionary<string, ILegalEntityStatus>>();
            SetupResult.For(legalEntityExceptionStatuses.ObjectDictionary).Return(objectDictionary);
            
            legalEntity.LegalEntityExceptionStatus = null;
            LastCall.IgnoreArguments();

            
            // Cater for calls to _view.Messages
            SetupResult.For(_view.Messages).Return(new DomainMessageCollection());

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityEnableUpdateUpdate legalEntityEnableUpdateUpdate = new LegalEntityEnableUpdateUpdate(_view, controller);

            legalEntityEnableUpdateUpdate.LegalEntityMock = legalEntity;

            _onSubmitButtonClickRaiser.Raise(null, new EventArgs());

            _mockery.VerifyAll();

            Assert.AreEqual((int)LegalEntityExceptionStatuses.InvalidIDNumber, legalEntityEnableUpdateUpdate.LegalEntityMock.LegalEntityExceptionStatus.Key);

        }

        /// <summary>
        /// Expect a CBONodeNotFoundException Exception
        /// </summary>
        [NUnit.Framework.Test]
        [ExpectedException(typeof(CBONodeNotFoundException))]
        public void ViewInitializeNodeNotFound()
        {
            SetupBaseEventsExpectations();

            // Setup the CBO current and LegalEntity node
            CBOMenuNode cboLegalEntityMenuNode = _mockery.CreateMock<CBOMenuNode>(new Dictionary<string, object>());

            // Mock the CBOService and get the LegalEntityNode
            Expect.Call(_cboService.GetCurrentCBONode(null, null)).IgnoreArguments().Return(null);

            // Ignore the rest 
            SetupResult.For(_resourceService.GetString(ResourceConstants.NodeNotFoundException)).Return(String.Empty);

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LegalEntityEnableUpdateUpdate legalEntityEnableUpdateUpdate = new LegalEntityEnableUpdateUpdate(_view, controller);

            try
            {
                _viewInitialisedRaiser.Raise(null, new EventArgs());
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }

            _mockery.VerifyAll();

        }

        #region Helper Functions
        protected virtual void SetupBaseEventsExpectations()
        {
            // setup expectations
            _view.ViewInitialised += null;
            LastCall.Constraints(Is.NotNull());
            _viewInitialisedRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.ViewPreRender += null;
            LastCall.Constraints(Is.NotNull());
            _viewPreRenderRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.OnSubmitButtonClick += null;
            LastCall.Constraints(Is.NotNull());
            _onSubmitButtonClickRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.OnCancelButtonClick += null;
            LastCall.Constraints(Is.NotNull());
            _onCancelButtonClickRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.ViewLoaded += null;
            LastCall.Constraints(Is.NotNull());

        }
        #endregion

    }
}
