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

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    /// <summary>
    /// ProposalDetails Maintenance
    /// </summary>
    public class ProposalDetailsMaintenance : ProposalDetailsMaintenanceBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ProposalDetailsMaintenance(IProposalDetails view, SAHLCommonBaseController controller)
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
            //set the required Proposal Types before doing anything
            _view.ShowProposals = ProposalTypeDisplays.Proposal;

            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

        }

        #endregion

    }
}
