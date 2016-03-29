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
using SAHL.Common.Web.UI.Events;
using Microsoft.ApplicationBlocks.UIProcess;
namespace SAHL.Web.Test.Presenters.Common
{
    [Ignore]
    [TestFixture]
    public class RateChangeChangeTerm : RateChangeBasePresenter
    {

        [NUnit.Framework.Test]
        public void TestInitialiseTermChange()
        {
            
            _view.SetTermControlsVisibility = true;
            _view.SetRatesControlVisibility = false;
            _view.SetAbilityofSubmitButton = false;
            _view.SetSubmitButtonText("Change Term", "T");

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
            SAHL.Web.Views.Common.Presenters.RateChangeChangeTerm RateChangeTermChange = new SAHL.Web.Views.Common.Presenters.RateChangeChangeTerm(_view, controller);
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
            SAHL.Web.Views.Common.Presenters.RateChangeChangeTerm ratechangechangeterm = new SAHL.Web.Views.Common.Presenters.RateChangeChangeTerm(_view, controller);
            _submitButtonClickRaiser.Raise(_view, new EventArgs());

            _mockery.VerifyAll();

        }
    }
}
