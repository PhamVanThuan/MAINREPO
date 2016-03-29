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
using SAHL.Web.Views.Common.Presenters.LegalEntityRelationships;
using SAHL.Common.Collections;
using SAHL.Common.Factories;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.UI;

namespace SAHL.Web.Test.Presenters.LegalEntityRelationships
{
    [Ignore]
    [TestFixture]
    public class LegalEntityRelationshipPresenterBase : TestViewBase
    {
        private ILegalEntityRelationships _view;
        private ILegalEntityRepository _legalEntityRepository;
        private IEventRaiser _viewInitialisedRaiser;
        private IEventRaiser _viewPreRenderRaiser;
        private IEventRaiser _onSubmitButtonClickRaiser;
        private IEventRaiser _onCancelButtonClickRaiser;
        private IEventRaiser _onAddToCBORaiser;
        private ICBOService _cboService;

        [SetUp]
        protected void FixtureSetup()
        {
            _view = _mockery.CreateMock<ILegalEntityRelationships>();

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
            _cboService = null;
            ClearMockCache();
        }

        [NUnit.Framework.Test]
        public void EventsHookup()
        {
            SetupBaseEventsExpectations();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);

            LegalEntityRelationshipsDisplay legalEntityRelationshipsDisplay = new LegalEntityRelationshipsDisplay(_view, controller);

            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void ViewInitializeNodeFound()
        {
            SetupBaseEventsExpectations();

            // Setup the CBO current node
            CBOMenuNode cboCurrentMenuNode = _mockery.CreateMock<CBOMenuNode>(new Dictionary<string, object>());

            // Setup the ILegalEntity to be returned
            ILegalEntity legalEntity = _mockery.CreateMock<ILegalEntity>();

            // Mock the CBO and get the LegalEntity
            SetupResult.For(cboCurrentMenuNode.GetNodeByType(GenericKeyTypes.LegalEntity)).IgnoreArguments().Return(cboCurrentMenuNode);
            SetupResult.For(_cboService.GetCurrentCBONode(null, null)).IgnoreArguments().Return(cboCurrentMenuNode);
            SetupResult.For(cboCurrentMenuNode.GenericKey).Return((long)0);
            SetupResult.For(_view.Messages).Return(new DomainMessageCollection());
            SetupResult.For(legalEntity.LegalEntityRelationships).Return(null);

            Expect.Call(_legalEntityRepository.GetLegalEntityByKey(-1)).IgnoreArguments().Return(legalEntity);

            _view.BindRelationshipGrid(null);
            LastCall.IgnoreArguments();

            _mockery.ReplayAll();
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);

            LegalEntityRelationshipsDisplay legalEntityRelationshipsDisplay = new LegalEntityRelationshipsDisplay(_view, controller);
            _viewInitialisedRaiser.Raise(null, new EventArgs());

            _mockery.VerifyAll();

        }

        [NUnit.Framework.Test]
        public void ViewInitializeNodeNotFound()
        {
            SetupBaseEventsExpectations();

            // Setup the CBO current node
            CBOMenuNode cboCurrentMenuNode = _mockery.CreateMock<CBOMenuNode>(new Dictionary<string, object>());

            // Setup the ILegalEntity to be returned
            ILegalEntity legalEntity = _mockery.CreateMock<ILegalEntity>();

            // Mock the CBO and get the LegalEntity
            SetupResult.For(cboCurrentMenuNode.GetNodeByType(GenericKeyTypes.LegalEntity)).IgnoreArguments().Return(cboCurrentMenuNode);
            SetupResult.For(_cboService.GetCurrentCBONode(null, null)).IgnoreArguments().Return(null);

            _mockery.ReplayAll();
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);

            LegalEntityRelationshipsDisplay legalEntityRelationshipsDisplay = new LegalEntityRelationshipsDisplay(_view, controller);
            _viewInitialisedRaiser.Raise(null, new EventArgs());

            _mockery.VerifyAll();

        }


        /*
        [NUnit.Framework.Test]
        public void TestSave()
        {

            base.SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Default);

            using (new TransactionScope(TransactionMode.Inherits))
            {
                
                ILegalEntityRepository legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                ILegalEntity legalEntity = null;
                ILegalEntityMarketingOption legalEntityMarketingOption = null;

                legalEntity = legalEntityRepository.GetLegalEntityByKey(new DomainMessageCollection(), 78546);

                legalEntity.Surname = "Pearson";

                // legalEntityMarketingOption = legalEntityRepository.GetEmptyLegalEntityMarketingOption(new DomainMessageCollection());
                // legalEntityMarketingOption.MarketingOptionKey = 6;

                legalEntity.LegalEntityMarketingOptions.Add(new DomainMessageCollection(), legalEntityMarketingOption);

                legalEntityRepository.SaveLegalEntity(new DomainMessageCollection(), legalEntity);
            }
        }
        */

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

            _view.OnAddToCBO += null;
            LastCall.Constraints(Is.NotNull());
            _onAddToCBORaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.ViewLoaded += null;
            LastCall.Constraints(Is.NotNull());
        }
        #endregion

    }
}
