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
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Service.Interfaces;
using SAHL.Web.Views.Common.Presenters.Memo;
using SAHL.Common.BusinessModel.Interfaces.UI;

namespace SAHL.Web.Test.Presenters.Common
{
    [Ignore]
    [TestFixture]
    public class MemoBasePresenter : TestViewBase
    {
        protected  SAHL.Web.Views.Common.Interfaces.IMemo _memoView;
        protected IMemoRepository _memoRepository;
        protected IEventRaiser _initialiseEventRaiser;
        protected IEventRaiser _preRenderEventRaiser;
        protected IEventList<SAHL.Common.BusinessModel.Interfaces.IMemo> _memoLst;
        protected ICBOService _cboService;
        [SetUp]
        public void FixtureSetup()
        {
            base.ClearMockCache();

            _mockery = new MockRepository();
            _memoView = _mockery.CreateMock<SAHL.Web.Views.Common.Interfaces.IMemo>();

            SetupMockedView(_memoView);
            SetupPrincipalCache(base.TestPrincipal);

            // We're not going to be hitting db, mocking the hit
            base.SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            // Create fake Account Repository
            _memoRepository = _mockery.CreateMock<IMemoRepository>();

            // Add to mock cache
            base.MockCache.Add(typeof(IMemoRepository).ToString(), _memoRepository);
            _cboService = _mockery.CreateMock<ICBOService>();
            MockCache.Add(typeof(ICBOService).ToString(), _cboService);
        }

        [TearDown]
        public void FixtureTearDown()
        {
            _memoView = null;
            _mockery = null;
        }


        [NUnit.Framework.Test]
        public void TestEventsHookUp()
        {
            SetUpBaseEventExpectancies();

            _mockery.ReplayAll(); // Records everything done in View, where I mocked above in Set Up

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            MemoBase presenter = new MemoBase(_memoView, controller);
        }

        public void SetUpBaseEventExpectancies()
        {
            Expect.Call(_memoView.ShouldRunPage).Return(true);
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_memoView)[VIEWPRERENDER];

            _memoView.ViewInitialised += null;
            LastCall.IgnoreArguments();
            _initialiseEventRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _memoView.ViewLoaded += null;
            LastCall.IgnoreArguments();

            _memoView.ViewPreRender += null;
            LastCall.IgnoreArguments();
            _preRenderEventRaiser = LastCall.IgnoreArguments().GetEventRaiser();

        }

        [NUnit.Framework.Test]
        public void ViewInitialised()
        {
            SetUpBaseEventExpectancies();
        
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
           
            CBOMenuNode cboCurrentMenuNode = _mockery.CreateMock<CBOMenuNode>(new Dictionary<string, object>());
            SetupResult.For(cboCurrentMenuNode.GenericKey).Return(1);
            SetupResult.For(cboCurrentMenuNode.GenericKeyTypeKey).Return(1);
            SetupResult.For(_cboService.GetCurrentCBONode(null, null)).IgnoreArguments().Return(cboCurrentMenuNode);

            _memoView.PopulateStatusDropDown();
            _memoView.PopulateStatusUpdateDropDown();

            _mockery.ReplayAll();

            MemoBase presenter = new MemoBase(_memoView, controller);
            _initialiseEventRaiser.Raise(_memoView, new EventArgs());

            _mockery.VerifyAll();
        }

        public void OnitialiseCommon()
        {
            SetupResult.For(_memoView.Messages).Return(new DomainMessageCollection());
            _memoLst = _mockery.CreateMock<IEventList<SAHL.Common.BusinessModel.Interfaces.IMemo>>();
            SetupResult.For(_memoRepository.GetMemoByGenericKey(0, 0, 0)).IgnoreArguments().Return(_memoLst);
        }


    }
}
