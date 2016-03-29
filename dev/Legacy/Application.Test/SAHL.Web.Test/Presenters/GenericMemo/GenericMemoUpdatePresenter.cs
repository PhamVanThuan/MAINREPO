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
    public class MemoAcctUpdatePresenter : MemoBasePresenter
    {
        [NUnit.Framework.Test]
        public void BasicEventsHookupTest()
        {
            base.SetUpBaseEventExpectancies();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            GenericMemoUpdate memoAcctDisplayPresenter = new GenericMemoUpdate(_memoView, controller);
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void TestInitialise()
        {
            SetUpBaseEventExpectancies();
            OnitialiseCommon();

            ILookupRepository lookups = _mockery.CreateMock<ILookupRepository>();

            IEventList<IGeneralStatus> generalStatuses = _mockery.CreateMock<IEventList<IGeneralStatus>>();
            SetupResult.For(lookups.GeneralStatuses).IgnoreArguments().Return(generalStatuses);

            IEventList<SAHL.Common.BusinessModel.Interfaces.IMemo> memo = _mockery.CreateMock<IEventList<SAHL.Common.BusinessModel.Interfaces.IMemo>>();
            SetupResult.For(_memoRepository.GetMemoByGenericKey(0, 0, 0)).IgnoreArguments().Return(memo);

            _memoView.PopulateStatusDropDown();
            _memoView.PopulateStatusUpdateDropDown();

            _memoView.BindMemoLookUpControls(null);
            LastCall.IgnoreArguments();

            _memoView.OnMemoStatusChanged += null;
            LastCall.IgnoreArguments();

            _memoView.OnMemoGridsSelectedIndexChanged += null;
            LastCall.IgnoreArguments();

            _memoView.UpdateButtonClicked += null;
            LastCall.IgnoreArguments();

            _memoView.CancelButtonClicked += null;
            LastCall.IgnoreArguments();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();

            GenericMemoUpdate presenter = new GenericMemoUpdate(_memoView, controller);
            _initialiseEventRaiser.Raise(_memoView, new EventArgs());

            _mockery.VerifyAll();
       
       }

        [NUnit.Framework.Test]
        public void ViewPreRender()
        {
            SetUpBaseEventExpectancies();
            
            OnitialiseCommon();

            _memoView.ShowControlsDisplay = false;
            _memoView.ShowControlsUpdate = true;
            _memoView.ShowButtons = true;

            _memoView.SetGridPostBackType();
            _memoView.SetLabelData("Account", "Update");

            int _memoIndexSelected = 0;

            SAHL.Common.BusinessModel.Interfaces.IMemo memo = _mockery.CreateMock<SAHL.Common.BusinessModel.Interfaces.IMemo>();
            SetupResult.For(_memoLst[_memoIndexSelected]).IgnoreArguments().Return(memo);

            SetupResult.For(_memoLst.Count).IgnoreArguments().Return(1);

            _memoView.BindMemoGrid(null,"");
            LastCall.IgnoreArguments();

            _memoView.BindMemoFields(null);
            LastCall.IgnoreArguments();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();

            GenericMemoUpdate presenter = new GenericMemoUpdate(_memoView, controller);
            presenter.memo = _memoLst;
            _preRenderEventRaiser.Raise(_memoView, new EventArgs());

            _mockery.VerifyAll();
        }

    }
}