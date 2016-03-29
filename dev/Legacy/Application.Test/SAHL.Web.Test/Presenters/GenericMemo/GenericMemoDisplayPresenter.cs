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
using SAHL.Web.Views.Common.Presenters.Memo;

namespace SAHL.Web.Test.Presenters.Common
{
    [Ignore]
    [TestFixture]
    public class MemoAcctDisplayPresenter : MemoBasePresenter
    {
        [SetUp]
        public void FixtureSetup()
        {
            base.ClearMockCache();

            _mockery = new MockRepository();
            _memoView = _mockery.CreateMock<SAHL.Web.Views.Common.Interfaces.IMemo>();

            SetupMockedView(_memoView);
            SetupPrincipalCache(base.TestPrincipal);

            base.SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            _memoRepository = _mockery.CreateMock<IMemoRepository>();

            base.MockCache.Add(typeof(IMemoRepository).ToString(), _memoRepository);
        }

        [NUnit.Framework.Test]
        public void BasicEventsHookupTest()
        {
            base.SetUpBaseEventExpectancies();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            GenericMemoDisplay memoAcctDisplayPresenter = new GenericMemoDisplay(_memoView, controller);
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void TestInitialise()
        {
            SetUpBaseEventExpectancies();

            OnitialiseCommon();

            _memoView.PopulateStatusDropDown();
            _memoView.PopulateStatusUpdateDropDown();

            _memoView.OnMemoStatusChanged += null;
            LastCall.IgnoreArguments();

            _memoView.OnMemoGridsSelectedIndexChanged += null;
            LastCall.IgnoreArguments();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);

            GenericMemoDisplay presenter = new GenericMemoDisplay(_memoView, controller);
            _initialiseEventRaiser.Raise(_memoView, new EventArgs());

            _mockery.VerifyAll();

        }     
        
        [NUnit.Framework.Test]
        public void ViewPreRenderNoRecordsFound()
        {
            SetUpBaseEventExpectancies();

            OnitialiseCommon();

            SetupResult.For(_memoLst.Count).IgnoreArguments().Return(0);

            _memoView.ShowControlsDisplay = true;
            _memoView.ShowControlsUpdate = false;
            _memoView.ShowButtons = false;
            _memoView.SetGridPostBackType();

            _memoView.SetLabelData("Account Memo", "");
            _memoView.BindMemoGrid(null,"");
            LastCall.IgnoreArguments();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();

            GenericMemoDisplay presenter = new GenericMemoDisplay(_memoView, controller);
            presenter.memo = _memoLst;
            _preRenderEventRaiser.Raise(_memoView, new EventArgs());

            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void ViewPreRenderRecordsFound()
        {
            SetUpBaseEventExpectancies();

            OnitialiseCommon();

            SetupResult.For(_memoLst.Count).IgnoreArguments().Return(1);

            _memoView.ShowControlsDisplay = true;
            _memoView.ShowControlsUpdate = false;
            _memoView.ShowButtons = false;
            _memoView.SetGridPostBackType();

            int _memoIndexSelected = 0;

            SAHL.Common.BusinessModel.Interfaces.IMemo memo = _mockery.CreateMock<SAHL.Common.BusinessModel.Interfaces.IMemo>();
            SetupResult.For(_memoLst[_memoIndexSelected]).IgnoreArguments().Return(memo);

            _memoView.SetLabelData("Account Memo", "");

            _memoView.BindMemoGrid(null,"");
            LastCall.IgnoreArguments();

            _memoView.BindMemoFields(null);
            LastCall.IgnoreArguments();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();

            GenericMemoDisplay presenter = new GenericMemoDisplay(_memoView, controller);
            presenter.memo = _memoLst;
            _preRenderEventRaiser.Raise(_memoView, new EventArgs());

            _mockery.VerifyAll();
        }

    }
}