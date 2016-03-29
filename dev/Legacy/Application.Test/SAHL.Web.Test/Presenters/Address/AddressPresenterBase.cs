using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Web.Views.Common.Interfaces;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.UI;
using SAHL.Common;
using SAHL.Common.CacheData;
using SAHL.Web.Views;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.UI;

namespace SAHL.Web.Test.Presenters.Address
{
    public class AddressPresenterBase : TestViewBase
    {
        protected IAddressView _view;
        private ILegalEntityRepository _legalEntityRepository;
        private IAddressRepository _addressRepository;
        private ILookupRepository _lookupRepository;
        private ICBOService _cboService;

        #region Setup/TearDown

        [SetUp]
        public void Setup()
        {
            _mockery = new MockRepository();
            _view = _mockery.CreateMock<IAddressView>();
            SetupMockedView(_view);
            SetupPrincipalCache(base.TestPrincipal);
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            ClearMockCache();

            // create repositories
            _addressRepository = _mockery.CreateMock<IAddressRepository>();
            MockCache.Add(typeof(IAddressRepository).ToString(), _addressRepository);
            _legalEntityRepository = _mockery.CreateMock<ILegalEntityRepository>();
            MockCache.Add(typeof(ILegalEntityRepository).ToString(), _legalEntityRepository);
            _lookupRepository = _mockery.CreateMock<ILookupRepository>();
            MockCache.Add(typeof(ILookupRepository).ToString(), _lookupRepository);
            _cboService = _mockery.CreateMock<ICBOService>();
            MockCache.Add(typeof(ICBOService).ToString(), _cboService);

        }

        [TearDown]
        public void TearDown()
        {
            _mockery = null;
            _view = null;
        }

        #endregion

        #region Properties

        protected IAddressRepository AddressRepository
        {
            get
            {
                return _addressRepository;
            }
        }

        protected ILegalEntityRepository LegalEntityRepository
        {
            get
            {
                return _legalEntityRepository;
            }
        }

        protected ILookupRepository LookupRepository
        {
            get
            {
                return _lookupRepository;
            }
        }

        #endregion

        #region Methods

        protected void MockAddressTypeLookup()
        {
            IEventList<IAddressType> list = _mockery.CreateMock<IEventList<IAddressType>>();
            SetupResult.For(LookupRepository.AddressTypes).IgnoreArguments().Return(list);
            IDictionary<string, string> dict = _mockery.CreateMock<IDictionary<string, string>>();
            SetupResult.For(list.BindableDictionary).IgnoreArguments().Return(dict);
        }

        protected void MockGeneralStatusLookup()
        {
            IEventList<IGeneralStatus> list = _mockery.CreateMock<IEventList<IGeneralStatus>>();
            SetupResult.For(LookupRepository.GeneralStatuses).IgnoreArguments().Return(list);
            IDictionary<string, string> dict = _mockery.CreateMock<IDictionary<string, string>>();
            SetupResult.For(list.BindableDictionary).IgnoreArguments().Return(dict);
        }

        protected virtual void OnInitialiseCommon()
        {
            // add a key to the global cache just so the retrieval of a legal entity key works
            //SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(TestPrincipal);
            //GlobalData cache = spc.GetGlobalData();
            //cache.Add(ViewConstants.SelectedLifeLegalEntityKey, 0, new List<ICacheObjectLifeTime>());

            // simulate getting the CBO node
            CBOMenuNode cboCurrentMenuNode = _mockery.CreateMock<CBOMenuNode>(new Dictionary<string, object>());
            SetupResult.For(cboCurrentMenuNode.GenericKey).Return(0L); 
            SetupResult.For(_cboService.GetCurrentCBONode(null, null)).IgnoreArguments().Return(cboCurrentMenuNode);

            // set up mock objects
            ILegalEntity legalEntity = _mockery.CreateMock<ILegalEntity>();
            SetupResult.For(LegalEntityRepository.GetLegalEntityByKey(0)).IgnoreArguments().Return(legalEntity);
            SetupResult.For(legalEntity.LegalEntityAddresses).IgnoreArguments().Return(null);

            _view.BindAddressList(new EventList<ILegalEntityAddress>());
            LastCall.IgnoreArguments();


        }

        #endregion


    }
}
