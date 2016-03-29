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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Web.Views.Common.Presenters.Memo;
namespace SAHL.Web.Test.Presenters.Common
{
    [Ignore]
    [TestFixture]
    public class MemoAcctAddPresenter : MemoBasePresenter
    {

        [NUnit.Framework.Test]
        public void BasicEventsHookupTest()
        {
            base.SetUpBaseEventExpectancies();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            GenericMemoAdd memoAcctDisplayPresenter = new GenericMemoAdd(_memoView, controller);
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void TestInitialise()
        {
            SetUpBaseEventExpectancies();
            OnitialiseCommon();

            _memoView.PopulateStatusDropDown();
            _memoView.PopulateStatusUpdateDropDown();

            ILookupRepository lookups = _mockery.CreateMock<ILookupRepository>();

            IEventList<IGeneralStatus> generalStatuses = _mockery.CreateMock<IEventList<IGeneralStatus>>();
            SetupResult.For(lookups.GeneralStatuses).IgnoreArguments().Return(generalStatuses);

            _memoView.BindMemoLookUpControls(null);
            LastCall.IgnoreArguments();

            _memoView.OnMemoStatusChanged += null;
            LastCall.IgnoreArguments();

            _memoView.AddButtonClicked += null;
            LastCall.IgnoreArguments();

            _memoView.CancelButtonClicked += null;
            LastCall.IgnoreArguments();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            GenericMemoAdd presenter = new GenericMemoAdd(_memoView, controller);
            presenter.memo = _memoLst;

            _initialiseEventRaiser.Raise(_memoView, new EventArgs());

            _mockery.VerifyAll();
       
        }

        [NUnit.Framework.Test]
        public void ViewPreRender()
        {
            SetUpBaseEventExpectancies();

             OnitialiseCommon();

            _memoView.ShowControlsDisplay = false;
            _memoView.ShowControlsUpdate = false;
            _memoView.ShowControlsAdd = true;
            _memoView.ShowButtons = true;

            _memoView.SetDefaultDateForAdd();

            _memoView.SetLabelData("Account", "Add");
            _memoView.BindMemoGrid(null,"");
            LastCall.IgnoreArguments();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();

            GenericMemoAdd presenter = new GenericMemoAdd(_memoView, controller);
            presenter.memo = _memoLst;
            _preRenderEventRaiser.Raise(_memoView, new EventArgs());

            _mockery.VerifyAll();
        }

    }
}