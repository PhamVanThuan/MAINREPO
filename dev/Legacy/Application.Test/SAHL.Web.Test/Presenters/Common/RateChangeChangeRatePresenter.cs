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
using Microsoft.ApplicationBlocks.UIProcess;

namespace SAHL.Web.Test.Presenters.Common
{
    [Ignore]
    [TestFixture]
    public class RateChangeChangeRatePresenter : RateChangeBasePresenter
    {
        [NUnit.Framework.Test]
        public void TestInitialiseRateChange()
        {
            _view.PopulateLinkRates(null);
            LastCall.IgnoreArguments();

            _view.SetTermControlsVisibility = false;
            _view.SetRatesControlVisibility = true;
            _view.SetSubmitButtonText("Change Rate", "R");
        }

        [NUnit.Framework.Test]
        public void CancelButtonClick()
        {
            SetUpBaseEventExpectancies();

            ISimpleNavigator navigator = _mockery.CreateMock<ISimpleNavigator>();
            SetupResult.For(_view.Navigator).Return(navigator);

            navigator.Navigate("RateChangeRate");

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            RateChangeChangeRate RateChangeRateChange = new RateChangeChangeRate(_view, controller);
            _cancelButtonClickRaiser.Raise(_view, new EventArgs());

            _mockery.VerifyAll();

        }

        [NUnit.Framework.Test]
        public void SubmitButtonClickNoErrors()
        {
            SetUpBaseEventExpectancies();

            // Simulate no errors on save
            IDomainMessageCollection domainMessages = new DomainMessageCollection();
            SetupResult.For(_view.Messages).Return(domainMessages);

            // setup the expectation for Navigate("Update")
            ISimpleNavigator navigator = _mockery.CreateMock<ISimpleNavigator>();
            SetupResult.For(_view.Navigator).Return(navigator);
            navigator.Navigate("RateChangeRate");

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            RateChangeChangeRate ratechangechangerate = new RateChangeChangeRate(_view, controller);
            _submitButtonClickRaiser.Raise(_view, new EventArgs());

            _mockery.VerifyAll();

        }
    }
}
