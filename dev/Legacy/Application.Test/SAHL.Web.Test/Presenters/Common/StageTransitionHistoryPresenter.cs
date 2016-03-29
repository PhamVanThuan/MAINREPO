using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using Rhino.Mocks.Interfaces;
using SAHL.Web.Views.Common.Presenters;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks.Impl;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Web.Test.Presenters.Common
{
    [Ignore]
    [TestFixture]
    public class StageTransitionHistoryPresenter : TestViewBase
    {
        private IStageTransitionHistory _StageTransitionHistoryView;
        private IEventRaiser _initialiseEventRaiser;

        private IStageDefinitionGroup _stageDefinitionGroup;
        private IStageDefinitionRepository _stageDefinitionRepository;
        private IList<IStageTransition> _stageTransitionHistory;


        [SetUp]
        public void FixtureSetup()
        {
            base.ClearMockCache();

            _mockery = new MockRepository();
            _StageTransitionHistoryView = _mockery.CreateMock<IStageTransitionHistory>();

            SetupMockedView(_StageTransitionHistoryView);
            SetupPrincipalCache(base.TestPrincipal);

            // We're not going to be hittinh db, mocking the hit
            base.SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            // Create fake Account Repository
            _stageDefinitionRepository = _mockery.CreateMock<IStageDefinitionRepository>();

            // Add to mock cache
            base.MockCache.Add(typeof(IStageDefinitionRepository).ToString(), _stageDefinitionRepository);

        }

        [TearDown]
        public void FixtureTearDown()
        {
            _StageTransitionHistoryView = null;
            _mockery = null;
        }

        private void SetUpBaseEventExpectancies()
        {
            _StageTransitionHistoryView.ViewInitialised += null;
            LastCall.IgnoreArguments();
            _initialiseEventRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _StageTransitionHistoryView.ViewLoaded += null;
            LastCall.IgnoreArguments();

            _StageTransitionHistoryView.ViewPreRender += null;
            LastCall.IgnoreArguments();

        }

        [NUnit.Framework.Test]
        public void TestEventsHookUp()
        {
            SetUpBaseEventExpectancies();

            _mockery.ReplayAll(); // Records everything done in View, where I mocked above in Set Up

            // Simulates somebody calling my presenter
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            StageTransitionHistory presenter = new StageTransitionHistory(_StageTransitionHistoryView, controller);
        }

        [NUnit.Framework.Test]
        public void ViewInitialised()
        {
            SetUpBaseEventExpectancies();

            IEventRaiser eventRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();

            StageTransitionHistory presenter = new StageTransitionHistory(_StageTransitionHistoryView, controller);
            eventRaiser.Raise(_StageTransitionHistoryView, new EventArgs());

            _mockery.VerifyAll();
        }
      
      

    }
}
