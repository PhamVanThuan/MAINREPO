using System;
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.DomainMessages;
using System.Data;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    /// <summary>
    /// ProposalDetails View
    /// </summary>
    public class ProposalDetailsView : ProposalDetailsBase
    {
      
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ProposalDetailsView(IProposalDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        #region Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            // this must happen before call to base init
            _view.ReadOnlyMode = true;

            base.SelectedProposal = DebtCounsellingRepo.GetProposalByKey(Convert.ToInt32(this.GlobalCacheData[ViewConstants.ProposalKey]));

            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) 
                return;
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            //setup visible buttons
            _view.ShowAddButton = false;
            _view.ShowRemoveButton = false;
            _view.ShowSaveButton = false;
        }
        #endregion
    }
}
