using System;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using Castle.ActiveRecord;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common.Presenters.CommonReason
{
    /// <summary>
    /// 
    /// </summary>
    public class ProposalDecline : CommonReasonProposalBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ProposalDecline(ICommonReason view, SAHLCommonBaseController controller)
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

            if (!_view.ShouldRunPage) 
                return;

            _view.CancelButtonVisible = true;

            // limit the selection to one reason
            _view.OnlyOneReasonCanBeSelected = true;
        }

        /// <summary>
        /// Overrides the base OnSubmitButtonClicked event so that specific credit decline actions can be performed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void _view_OnSubmitButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            base._view_OnSubmitButtonClicked(sender, e);
        }

        public override void CancelActivity()
        {
            base.CancelActivity();
        }

        public override void CompleteActivityAndNavigate()
        {
            base.CompleteActivityAndNavigate();
        }
    }
}
