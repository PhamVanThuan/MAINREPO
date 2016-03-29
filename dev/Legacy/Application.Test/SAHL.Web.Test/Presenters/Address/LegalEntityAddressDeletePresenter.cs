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
    public class LegalEntityAddressDeletePresenter : AddressPresenterBase
    {
        private const string CancelButtonClicked = "CancelButtonClicked";
        private const string DeleteButtonClicked = "DeleteButtonClicked";

        [NUnit.Framework.Test]
        public void CancelButtonClick()
        {

            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser cancelClickEventRaiser = dictEvents[CancelButtonClicked];

            SetupNavigationMock(_view, "Cancel");

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityAddressDelete presenter = new LegalEntityAddressDelete(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            cancelClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();

        }

        private void DeleteButtonClickCommon(bool valid)
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser deleteClickEventRaiser = dictEvents[DeleteButtonClicked];

            IAddress address = _mockery.CreateMock<IAddress>();
            SetupResult.For(_view.SelectedAddress).IgnoreArguments().Return(address);
            LegalEntityRepository.DeleteAddress(null);
            LastCall.IgnoreArguments();

            IValidationSummary valSummary = _mockery.CreateMock<IValidationSummary>();
            // SetupResult.For(_view.ValidationSummary).Return(valSummary);
            SetupResult.For(valSummary.IsValid).Return(valid);
            if (valid)
                SetupNavigationMock(_view, "LegalEntityAddressDisplay");

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityAddressDelete presenter = new LegalEntityAddressDelete(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            deleteClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void DeleteButtonClickValid()
        {
            DeleteButtonClickCommon(true);
        }

        [NUnit.Framework.Test]
        public void DeleteButtonClickInvalid()
        {
            DeleteButtonClickCommon(false);
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
            LegalEntityAddressDelete presenter = new LegalEntityAddressDelete(_view, controller);
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
            Expect.Call(_view.ShouldRunPage).Return(true);
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWPRERENDER];

            // specific values that must be set
            _view.DeleteButtonVisible = true;
            _view.CancelButtonVisible = true;
            _view.AddressListVisible = true;
            _view.AddressDetailsVisible = true;

            // properties that must be set, but we don't care about testing the value
            _view.AddressListHeaderText = "Test";
            LastCall.IgnoreArguments();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityAddressDelete presenter = new LegalEntityAddressDelete(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        private new IDictionary<string, IEventRaiser> OnInitialiseCommon()
        {
            Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();

            Expect.Call(_view.ShouldRunPage).Return(true);
            base.OnInitialiseCommon();

            // delete button click event
            _view.DeleteButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erDeleteButton = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(DeleteButtonClicked, erDeleteButton);

            // delete button click event
            _view.CancelButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erCancelButton = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(CancelButtonClicked, erCancelButton);

            return dict;
        }


    }
}
