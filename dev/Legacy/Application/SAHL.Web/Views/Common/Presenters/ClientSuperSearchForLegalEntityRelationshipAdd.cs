using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// must only allow searching for Natural Persons
    /// must enable 'Cancel' &amp; 'New Assured Life' Buttons
    /// </summary>
    public class ClientSuperSearchForLegalEntityRelationshipAdd : ClientSuperSearch
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ClientSuperSearchForLegalEntityRelationshipAdd(IClientSuperSearch view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            // setup the buttons
            _view.CancelButtonVisible = true;
            _view.CreateNewClientButtonVisible = true;
            _view.CreateNewClientButtonText = "New Legal Entity";
            _view.AccountTypesVisible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        protected override void OnClientSelectedClicked(object sender, ClientSuperSearchSelectedEventArgs ea)
        {
            // clear the legalentity out of global cache 
            GlobalCacheData.Remove(ViewConstants.LegalEntity);
            GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);

            // add the selected legal entity to globalcache and navigate back to the calling page
            ILegalEntity selectedLegalEntity = LegalEntityRepo.GetLegalEntityByKey(ea.LegalEntityKey);
            GlobalCacheData.Add(ViewConstants.LegalEntity, selectedLegalEntity, new List<ICacheObjectLifeTime>());
            GlobalCacheData.Add(ViewConstants.SelectedLegalEntityKey, selectedLegalEntity.Key, new List<ICacheObjectLifeTime>());

            base.Navigator.Navigate("EntitySelected");
        }
    }
}
