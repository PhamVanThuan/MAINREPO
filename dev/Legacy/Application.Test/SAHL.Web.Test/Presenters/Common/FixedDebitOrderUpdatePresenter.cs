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
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Test.Presenters.Common
{
    [Ignore]
    [TestFixture]
    public class FixedDebitOrderUpdatePresenter : FixedDebitOrderDisplayPresenter
    {
        private IFixedDebitOrderSummary _view;
       
        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            _mockery = new MockRepository();
            _view = _mockery.CreateMock<IFixedDebitOrderSummary>();

            SetupMockedView(_view);
            SetupPrincipalCache(base.TestPrincipal);

             base.ClearMockCache();

            base.SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            _accountRepo = _mockery.CreateMock<IAccountRepository>();
            _fdcRepo = _mockery.CreateMock<IFutureDatedChangeRepository>();

            base.MockCache.Add(typeof(IAccountRepository).ToString(), _accountRepo);
            base.MockCache.Add(typeof(IFutureDatedChangeRepository).ToString(), _fdcRepo);
        }

        [TearDown]
        public void FixtureTearDown()
        {
            _view = null;
            _mockery = null;
        }

        #endregion

        private void SetUpBaseEventExpectancies()
        {
            _view.ViewInitialised += null;
            LastCall.IgnoreArguments();
            _initialiseEventRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.ViewLoaded += null;
            LastCall.IgnoreArguments();

            _view.ViewPreRender += null;
            LastCall.IgnoreArguments();
            _preRenderEventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
        }


        [NUnit.Framework.Test]
        private void TestEventsHookUp()
        {
            SetUpBaseEventExpectancies();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            FixedDebitOrderSummaryUpdate presenter = new FixedDebitOrderSummaryUpdate(_view, controller);

        }

        [NUnit.Framework.Test]
        public void TestInitialise()
        {
            SetUpBaseEventExpectancies();
            OnInitialiseCommon();

            _view.selectedFirstRow = false;
            _view.SetGridPostBack();
            _view.BindFutureDatedDOGrid(null);
            LastCall.IgnoreArguments();

            _view.SetUpInitialDataOnView(null);
            LastCall.IgnoreArguments();

            _view.OnFutureOrderGridSelectedIndexChanged += null;
            LastCall.IgnoreArguments();
            _view.UpdateButtonClicked += null;
            LastCall.IgnoreArguments();
            _view.CancelButtonClicked += null;
            LastCall.IgnoreArguments();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            FixedDebitOrderSummaryUpdate presenter = new FixedDebitOrderSummaryUpdate(_view, controller);
            presenter.accountVal = _account;
            presenter.futureDCLst = _futureDatedChangeLst;
            _initialiseEventRaiser.Raise(_view, new EventArgs());

            _mockery.VerifyAll();

        }

        [NUnit.Framework.Test]
        public void TestPreRender()
        {
            SetUpBaseEventExpectancies();

            _view.ShowButtons = false;
            _view.ShowUpdateableControl = false;

            _view.ShowButtons = true;
            _view.ShowUpdateableControl = true;
            _view.SetControlForUpdate();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            FixedDebitOrderSummaryUpdate presenter = new FixedDebitOrderSummaryUpdate(_view, controller);

            _preRenderEventRaiser.Raise(_view, new EventArgs());

            _mockery.VerifyAll();

        }

    }
}
