using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.CacheData;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.UI;
using Castle.ActiveRecord;
using SAHL.Common.Globals;


namespace SAHL.Web.Views.Common.Presenters.Address
{
    public class LegalEntityAddressBase : SAHLCommonBasePresenter<IAddressView>
    {
        #region Private / Protected Attributes

        private ILookupRepository _lookupRepository;
        private ILegalEntity _legalEntity;
        private IAddressRepository _addressRepository;
        private ILegalEntityRepository _legalEntityRepository;


        #endregion

        #region Constructor

        public LegalEntityAddressBase(IAddressView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view = view;
        }

        #endregion

        #region Properties

        protected IAddressRepository AddressRepository 
        {
            get
            {
                if (_addressRepository == null)
                    _addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();
                return _addressRepository;
            }
        }

        protected ILegalEntityRepository LegalEntityRepository
        {
            get
            {
                if (_legalEntityRepository == null)
                    _legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                return _legalEntityRepository;
            }
        }

        protected ILookupRepository LookupRepository
        {
            get
            {
                if (_lookupRepository == null)
                    _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                return _lookupRepository;
            }
        }

        protected ILegalEntity LegalEntity
        {
            get
            {
                return _legalEntity;
            }
        }

        private List<ICacheObjectLifeTime> _lifeTimes;
        protected List<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                {
                    List<string> views = new List<string>();
                    views.Add(View.ViewName);
                    _lifeTimes = new List<ICacheObjectLifeTime>();
                    _lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
                }

                return _lifeTimes;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears all items out of the cache.
        /// </summary>
        protected void ClearCachedData()
        {
            GlobalCacheData.Remove(ViewConstants.NavigateTo);
            GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (_view.IsMenuPostBack)
                ClearCachedData();

            if (!_view.ShouldRunPage) return;

            int legalEntityKey = 0;

            // check the global data cache for the legal entity key - if it's not there, then revert to the CBO
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
                legalEntityKey = Convert.ToInt32(GlobalCacheData[ViewConstants.SelectedLegalEntityKey]);
            else
            {
                CBOMenuNode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
                legalEntityKey = Convert.ToInt32(node.GenericKey);
            }

            _legalEntity = LegalEntityRepository.GetLegalEntityByKey(legalEntityKey);



            _view.ShowInactive = true;
            _view.BindAddressList(_legalEntity.LegalEntityAddresses, _legalEntity.FailedLegalEntityAddresses);

        }


        #endregion


    }
}
