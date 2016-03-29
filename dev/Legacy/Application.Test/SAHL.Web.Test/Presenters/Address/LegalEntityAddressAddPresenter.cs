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
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Test;
using SAHL.Common.Globals;
using Microsoft.ApplicationBlocks.UIProcess;
using Rhino.Mocks.Constraints;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Collections;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Test.Presenters.Address
{
    [Ignore]
    [TestFixture]
    public class LegalEntityAddressAddPresenter : AddressPresenterBase
    {

        private const string AddButtonClicked = "AddButtonClicked";
        private const string CancelButtonClicked = "CancelButtonClicked";
        private const string SelectedAddressTypeChanged = "SelectedAddressTypeChanged";
        private const string ExistingAddressSelected = "ExistingAddressSelected";

        [NUnit.Framework.Test]
        public void AddButtonClickValid()
        {
            AddButtonClickCommon(true);
        }

        [NUnit.Framework.Test]
        public void AddButtonClickInvalid()
        {
            AddButtonClickCommon(false);
        }

        [NUnit.Framework.Test]
        public void CancelButtonClick()
        {

            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser cancelClickEventRaiser = dictEvents[CancelButtonClicked];

            SetupNavigationMock(_view, "Cancel");

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityAddressAdd presenter = new LegalEntityAddressAdd(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            cancelClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();

        }

        [NUnit.Framework.Test]
        public void ExistingAddressSelect()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];
            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser addrSelectEventRaiser = dictEvents[ExistingAddressSelected];

            Expect.Call(AddressRepository.GetAddressByKey(1)).IgnoreArguments().Return(null);
            OnSaveCommon(true);

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityAddressAdd presenter = new LegalEntityAddressAdd(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            addrSelectEventRaiser.Raise(_view, new KeyChangedEventArgs(1));
            _mockery.VerifyAll();

        }

        [NUnit.Framework.Test]
        public void SelectedAddressTypeChange()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];
            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser addrTypeChangedEventRaiser = dictEvents[SelectedAddressTypeChanged];

            IDictionary<string, string> addressFormats = _mockery.CreateMock<IDictionary<string, string>>();
            SetupResult.For(LookupRepository.AddressFormatsByAddressType(AddressTypes.Postal)).IgnoreArguments().Return(null);
            _view.BindAddressFormats(new Dictionary<string, string>());
            LastCall.IgnoreArguments();
            // LastCall.Repeat.


            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityAddressAdd presenter = new LegalEntityAddressAdd(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            addrTypeChangedEventRaiser.Raise(_view, new KeyChangedEventArgs(1));
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
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            OnInitialiseCommon();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityAddressAdd presenter = new LegalEntityAddressAdd(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void ViewPreRenderNoCountry()
        {
            ViewPreRenderValues("");
        }

        [NUnit.Framework.Test]
        public void ViewPreRenderCountrySelected()
        {
            ViewPreRenderValues("1");
        }

        private void ViewPreRenderValues(string countryKey)
        {
            Expect.Call(_view.ShouldRunPage).Return(true);
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWPRERENDER];

            // specific values that must be set
            _view.AddButtonVisible = true;
            _view.CancelButtonVisible = true;
            _view.AddressListVisible = true;
            _view.AddressFormVisible = true;

            // properties that must be set, but we don't care about testing the value
            _view.AddressListHeaderText = "Test";
            LastCall.IgnoreArguments();

            // SetupResult.For(_view.SelectedCountryValue).Return(countryKey);

            // provinces are only bound if there is a country key
            if (countryKey.Length > 0)
            {
                EventList<IProvince> provinces = new EventList<IProvince>();
                SetupResult.For(LookupRepository.ProvincesByCountry(Int32.Parse(countryKey))).Return(provinces);
                // _view.BindProvinces(provinces);
                LastCall.IgnoreArguments();
            }

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityAddressAdd presenter = new LegalEntityAddressAdd(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        /// <summary>
        /// Common initialisation procedure.
        /// </summary>
        /// <returns></returns>
        private new IDictionary<string, IEventRaiser> OnInitialiseCommon()
        {
            Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();

            Expect.Call(_view.ShouldRunPage).Return(true);
            base.OnInitialiseCommon();

            // add lookup calls to ignore
            MockAddressTypeLookup();

            SetupResult.For(LookupRepository.Countries).IgnoreArguments().Return(null);
            SetupResult.For(LookupRepository.AddressFormatsByAddressType(AddressTypes.Residential)).IgnoreArguments().Return(null);

            // properties
            _view.GridPostBack = false;

            // methods
            _view.BindAddressTypes(new Dictionary<string, string>());
            LastCall.IgnoreArguments();
            _view.BindAddressFormats(new Dictionary<string, string>());
            LastCall.IgnoreArguments();
            // _view.BindCountries(new EventList<ICountry>());
            LastCall.IgnoreArguments();

            // properties
            SetupResult.For(_view.SelectedAddressTypeValue).Return("1");

            // select address type changed event
            _view.SelectedAddressTypeChanged += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erSelAddrTypeChanged = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(SelectedAddressTypeChanged, erSelAddrTypeChanged);

            // existing address selected event
            _view.ExistingAddressSelected += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erExistingAddressSelected = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(ExistingAddressSelected, erExistingAddressSelected);

            // add button click event
            _view.AddButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erAddButton = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(AddButtonClicked, erAddButton);

            // cancel button click event
            _view.CancelButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erCancelButton = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(CancelButtonClicked, erCancelButton);


            return dict;
        }

        private void AddButtonClickCommon(bool valid)
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser addClickEventRaiser = dictEvents[AddButtonClicked];

            Expect.Call(_view.GetCapturedAddress()).IgnoreArguments().Return(null);
            OnSaveCommon(valid);

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityAddressAdd presenter = new LegalEntityAddressAdd(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            addClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        private void OnSaveCommon(bool valid)
        {
            IAddressType addressType = _mockery.CreateMock<IAddressType>();
            SetupResult.For(_view.AddressRepository).Return(AddressRepository);
            SetupResult.For(AddressRepository.GetAddressTypeByKey(1)).IgnoreArguments().Return(addressType);

            LegalEntityRepository.SaveAddress( null, null, null, DateTime.Now);
            LastCall.IgnoreArguments();

            IValidationSummary valSummary = _mockery.CreateMock<IValidationSummary>();
            // SetupResult.For(_view.ValidationSummary).Return(valSummary);
            SetupResult.For(valSummary.IsValid).Return(valid);
            if (valid)
                SetupNavigationMock(_view, "Add");
        }

    }

}
