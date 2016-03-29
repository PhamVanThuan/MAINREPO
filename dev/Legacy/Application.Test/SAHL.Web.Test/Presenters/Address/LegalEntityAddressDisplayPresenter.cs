using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using Rhino.Mocks.Interfaces;
using SAHL.Web.Views.Common.Presenters.Address;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Test;
using SAHL.Common.Collections;
using Microsoft.ApplicationBlocks.UIProcess;
using Rhino.Mocks.Constraints;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Web.Test.Presenters.Address
{
    [Ignore]
    [TestFixture]
    public class LegalEntityAddressDisplayPresenter : AddressPresenterBase
    {
        private const string AuditButtonClicked = "AuditButtonClicked";

        [NUnit.Framework.Test]
        public void AuditButtonClick()
        {

            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser auditClickEventRaiser = dictEvents[AuditButtonClicked];

            ISimpleNavigator navigator = _mockery.CreateMock<ISimpleNavigator>();
            SetupResult.For(_view.Navigator).Return(navigator);
            navigator.Navigate("AddressAuditTrail");

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityAddressDisplay presenter = new LegalEntityAddressDisplay(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            auditClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();

        }

        /// <summary>
        /// Ensures that the following methods are called on initialisation.
        /// <list type="bullet">
        ///     <item><description><see cref="IAddressView.BindAddressList"/></description></item>
        ///     <item><description><see cref="IAddressView.BindAddressTypes"/></description></item>
        ///     <item><description><see cref="IAddressView.BindAddressFormats"/></description></item>
        /// </list>
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewInitialise()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            OnInitialiseCommon();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityAddressDisplay presenter = new LegalEntityAddressDisplay(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        /// <summary>
        /// Ensures that the correct properties are set before rendering the display.
        /// <para>
        /// The following properties must be set:
        /// <list type="bullet">
        ///     <item><description><see cref="IAddressView.ButtonAuditTrailVisible"/> - must be set to true.</description></item>
        /// </list>
        /// </para>
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewPreRender()
        {
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWPRERENDER];

            Expect.Call(_view.ShouldRunPage).Return(true);

            // specific values that must be set
            _view.AuditTrailButtonVisible = true;
            _view.AddressListVisible = true;
            _view.AddressDetailsVisible = true;

            // properties that must be set, but we don't care about testing the value
            _view.AddressListHeaderText = "Test";
            LastCall.IgnoreArguments();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityAddressDisplay presenter = new LegalEntityAddressDisplay(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        private IDictionary<string, IEventRaiser> OnInitialiseCommon()
        {
            Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();

            Expect.Call(_view.ShouldRunPage).Return(true);
            base.OnInitialiseCommon();
            
            // audit button click event
            _view.AuditButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erAuditButton = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(AuditButtonClicked, erAuditButton);

            return dict;
        }


    }
}
