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
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Test.Presenters.Address
{
    [Ignore]
    [TestFixture]
    // [Ignore("WIP")]
    public class LegalEntityAddressUpdatePresenter : AddressPresenterBase
    {

        private const string UpdateButtonClicked = "UpdateButtonClicked";
        private const string CancelButtonClicked = "CancelButtonClicked";
        private const string ExistingAddressSelected = "ExistingAddressSelected";

        [NUnit.Framework.Test]
        public void CancelButtonClick()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser cancelClickEventRaiser = dictEvents[CancelButtonClicked];

            SetupNavigationMock(_view, "Cancel");

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityAddressUpdate presenter = new LegalEntityAddressUpdate(_view, controller);
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

            Expect.Call(_view.SelectedAddress).Return(null);
            Expect.Call(AddressRepository.GetAddressByKey(1)).IgnoreArguments().Return(null);
            OnSaveCommon(true);

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityAddressUpdate presenter = new LegalEntityAddressUpdate(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            addrSelectEventRaiser.Raise(_view, new KeyChangedEventArgs(1));
            _mockery.VerifyAll();

        }

        private void UpdateCommon(bool valid)
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser updateClickEventRaiser = dictEvents[UpdateButtonClicked];

            Expect.Call(_view.GetCapturedAddress()).IgnoreArguments().Return(null);
            Expect.Call(_view.SelectedAddress).Return(null);
            OnSaveCommon(valid);

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityAddressUpdate presenter = new LegalEntityAddressUpdate(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            updateClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void UpdateButtonClickValid()
        {
            UpdateCommon(true);

        }

        [NUnit.Framework.Test]
        public void UpdateButtonClickInvalid()
        {
            UpdateCommon(false);
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
            LegalEntityAddressUpdate presenter = new LegalEntityAddressUpdate(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void ViewPreRenderCountrySelected()
        {
            OnPreRenderCommon("1");
        }

        [NUnit.Framework.Test]
        public void ViewPreRenderNoCountry()
        {
            OnPreRenderCommon("");
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
            MockGeneralStatusLookup();
            MockAddressTypeLookup();
            SetupResult.For(LookupRepository.Countries).IgnoreArguments().Return(null);
            //SetupResult.For(LookupRepository.AddressFormatsByAddressType(AddressTypes.Residential)).IgnoreArguments().Return(null);

            // methods
            _view.BindAddressTypes(new Dictionary<string, string>());
            LastCall.IgnoreArguments();
            _view.BindAddressStatuses(new Dictionary<string, string>());
            LastCall.IgnoreArguments();
            // _view.BindCountries(new EventList<ICountry>());
            LastCall.IgnoreArguments();

            // properties
            SetupResult.For(_view.SelectedAddressTypeValue).Return("1");

            // existing address selected event
            _view.ExistingAddressSelected += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erExistingAddressSelected = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(ExistingAddressSelected, erExistingAddressSelected);

            // add button click event
            _view.UpdateButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erAddButton = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(UpdateButtonClicked, erAddButton);

            // cancel button click event
            _view.CancelButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erCancelButton = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(CancelButtonClicked, erCancelButton);


            return dict;
        }

        #region Common Methods

        private void OnSaveCommon(bool valid)
        {
            LegalEntityRepository.SaveLegalEntityAddress(null, null);
            LastCall.IgnoreArguments();

            IValidationSummary valSummary = _mockery.CreateMock<IValidationSummary>();
            // SetupResult.For(_view.ValidationSummary).Return(valSummary);
            SetupResult.For(valSummary.IsValid).Return(valid);
            if (valid)
                SetupNavigationMock(_view, "Update");
        }

        private void OnPreRenderCommon(string countryKey)
        {
            Expect.Call(_view.ShouldRunPage).Return(true);
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWPRERENDER];

            // specific values that must be set
            _view.CancelButtonVisible = true;
            _view.UpdateButtonVisible = true;
            _view.AddressListVisible = true;
            _view.AddressFormVisible = true;
            _view.AddressStatusVisible = true;
            _view.AddressFormatReadOnly = true;
            _view.AddressTypeReadOnly = true;

            // properties that must be set, but we don't care about testing the value
            _view.AddressListHeaderText = "Test";
            LastCall.IgnoreArguments();

            Expect.Call(_view.SelectedAddressTypeValue).Return("1"); ;
            Expect.Call(LookupRepository.AddressFormatsByAddressType(AddressTypes.Postal)).IgnoreArguments().Return(null);
            _view.BindAddressFormats(null);
            LastCall.IgnoreArguments();

            // SetupResult.For(_view.SelectedCountryValue).Return(countryKey);

            // provinces are only bound if there is a country key
            if (!String.IsNullOrEmpty(countryKey))
            {
                EventList<IProvince> provinces = new EventList<IProvince>();
                SetupResult.For(LookupRepository.ProvincesByCountry(Int32.Parse(countryKey))).Return(provinces);
                // _view.BindProvinces(provinces);
                LastCall.IgnoreArguments();
            }

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityAddressUpdate presenter = new LegalEntityAddressUpdate(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        #endregion


    }
}
