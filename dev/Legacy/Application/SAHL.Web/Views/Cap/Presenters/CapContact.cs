using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.CacheData;

using SAHL.Common;

namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CapContact : SAHLCommonBasePresenter<IContact>
    {
        private IReadOnlyEventList<ILegalEntity> _lstLegalEntities;
        private InstanceNode _node;
        private SAHL.Common.BusinessModel.Interfaces.IAccount _account;
        private int _accountKey;
        private IAccountRepository _accountRepo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CapContact(IContact view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Get the CBO Node    
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            _accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnLegalEntityGridSelectedIndexChanged += new KeyChangedEventHandler(OnLegalEntityGridSelectedIndexChanged);

            Dictionary<string, object> _x2Data = _node.X2Data as Dictionary<string, object>;
            _accountKey = Convert.ToInt32(_x2Data["AccountKey"]);
            _account = _accountRepo.GetAccountByKey(_accountKey);
          
            // check for assured lives
            _view.AssuredLivesMode = false;
            _lstLegalEntities = _account.GetLegalEntitiesByRoleType(_view.Messages, new int[] { 2, 3 });

            // bind the legal entities grid
            _view.BindLegalEntityGrid(_lstLegalEntities, _accountKey);

            _view.ShowWorkFlowHeader = false;
            _view.ShowUpdateContactButton = false;
            _view.ShowAddAddressButton = false;
            _view.ShowUpdateAddressButton = false;
            _view.ShowNextButton = false;

            if (_lstLegalEntities.Count > 0)
            {
                _view.BindAssuredLivesDetails(_lstLegalEntities[0]);
                _view.BindAddressData(_lstLegalEntities[0].LegalEntityAddresses);
            }
            else
            {
                _view.BindAssuredLivesDetails(null);
                _view.BindAddressData(null);
            }
        }

        /// <summary>
        /// Handles the event fired by the view when the Legal Entity Grid Selected Index is changed
        /// Populate the BindAssuredDetails controls based on the selected row in the Legal Entity Grid
        /// </summary>
        void OnLegalEntityGridSelectedIndexChanged(object sender, KeyChangedEventArgs e) 
        {
            for (int x = 0; x < _lstLegalEntities.Count; x++)
            {
                if (_lstLegalEntities[x].Key == int.Parse(e.Key.ToString()))
                {
                    _view.BindAssuredLivesDetails(_lstLegalEntities[x]);
                    // Bind Legal Entity Address Data
                    _view.BindAddressData(_lstLegalEntities[x].LegalEntityAddresses);

                    if (_lstLegalEntities[x].LegalEntityAddresses.Count == 0)
                        _view.ShowUpdateAddressButton = false;

                    break;
                }
            }
        }          
    }
}
