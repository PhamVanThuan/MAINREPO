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
using SAHL.Common.BusinessModel.Interfaces.UI;

namespace SAHL.Web.Test.Presenters.Common
{
    [Ignore]
    [TestFixture]
    public class RelatedLegalEntityPresenter : TestViewBase
    {
        private IRelatedLegalEntity _view;
        private ILegalEntityRepository _legalEntityRepository;
        private IEventRaiser _viewInitialisedRaiser;
        private IEventRaiser _viewPreRenderRaiser;
        private IEventRaiser _onSelectLegalEntityRaiser;
        private ICBOService _cboService;

        [SetUp]
        protected void FixtureSetup()
        {
            _view = _mockery.CreateMock<IRelatedLegalEntity>();
            base.SetupMockedView(_view);
            base.SetupPrincipalCache(base.TestPrincipal);

            // set our typefactory stratergy to be mock based, 
            // in order to force the TypeFactory to return mocks rather than
            // concrete classes.
            base.SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);

            // Create a mocked ILegalEntityRepository object and add it to the MOCK Cache. Clear teh Cache just in case one exists already
            ClearMockCache();

            _cboService = _mockery.CreateMock<ICBOService>();
            base.MockCache.Add(typeof(ICBOService).ToString(), _cboService);

            _legalEntityRepository = _mockery.CreateMock<ILegalEntityRepository>();
            base.MockCache.Add(typeof(ILegalEntityRepository).ToString(), _legalEntityRepository);

        }

        [TearDown]
        protected void FixtureTearDown()
        {
            _view = null;
            _legalEntityRepository = null;
            ClearMockCache();
        }

        [NUnit.Framework.Test]
        public void EventsHookup()
        {
            SetupBaseEventsExpectations();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);

            RelatedLegalEntity relatedLegalEntity = new RelatedLegalEntity(_view, controller);
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void ViewInitializeAccountParentFound()
        {
            SetupBaseEventsExpectations();

            // Setup a node with an acoount parent
            CBOMenuNode cboCurrentMenuNode = _mockery.CreateMock <CBOMenuNode>(new Dictionary<string, object>());

            // Mock the CBO and get the LegalEntity
            SetupResult.For(cboCurrentMenuNode.GetNodeByType(GenericKeyTypes.LegalEntity)).IgnoreArguments().Return(cboCurrentMenuNode);
            SetupResult.For(_cboService.GetCurrentCBONode(null, null)).IgnoreArguments().Return(cboCurrentMenuNode);


            _mockery.ReplayAll();
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);

            RelatedLegalEntity relatedLegalEntity = new RelatedLegalEntity(_view, controller);
            _viewInitialisedRaiser.Raise(null, new EventArgs());

            _mockery.VerifyAll();

        }

        [NUnit.Framework.Test]
        public void ViewInitializeAccountParentNotFound()
        {
            SetupBaseEventsExpectations();

            // Mock the CBO and get the LegalEntity
            _cboService = _mockery.CreateMock<ICBOService>();

            // CBOService.

            _mockery.ReplayAll();
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);

            RelatedLegalEntity relatedLegalEntity = new RelatedLegalEntity(_view, controller);
            _viewInitialisedRaiser.Raise(null, new EventArgs());

            _mockery.VerifyAll();

        }

        [NUnit.Framework.Test]
        public void ViewPreRenderNoRecordsFound()
        {
            SetupBaseEventsExpectations();

            // Expect teh button to be hidden
            _view.AddToMenuButtonEnabled = false;

            // If LegalEntityRoles is asked for, return a mocked version
            IEventList<IRole> legalEntityRoles = _mockery.CreateMock<IEventList<IRole>>();
            SetupResult.For(legalEntityRoles.Count).Return(0);

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            RelatedLegalEntity relatedLegalEntity = new RelatedLegalEntity(_view, controller);
            relatedLegalEntity.LegalEntityRoles = legalEntityRoles;

            _viewPreRenderRaiser.Raise(null, new EventArgs());

            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void ViewPreRenderRecordsFound()
        {
            SetupBaseEventsExpectations();


            // If LegalEntityRoles is asked for, return a mocked version
            IEventList<IRole> legalEntityRoles = _mockery.CreateMock<IEventList<IRole>>();
            SetupResult.For(legalEntityRoles.Count).Return(1);

            // Expect teh button to be visible
            _view.AddToMenuButtonEnabled = true;

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            RelatedLegalEntity relatedLegalEntity = new RelatedLegalEntity(_view, controller);
            relatedLegalEntity.LegalEntityRoles = legalEntityRoles;

            _viewPreRenderRaiser.Raise(null, new EventArgs());

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

            _view.ViewLoaded += null;
            LastCall.Constraints(Is.NotNull());

            _view.OnSelectLegalEntity += null;
            LastCall.Constraints(Is.NotNull());
            _onSelectLegalEntityRaiser = LastCall.IgnoreArguments().GetEventRaiser();

        }
        #endregion

    }
}
