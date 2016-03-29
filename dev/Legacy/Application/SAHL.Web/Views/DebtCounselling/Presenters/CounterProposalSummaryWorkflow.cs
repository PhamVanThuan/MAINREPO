using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    public class CounterProposalSummaryWorkflow : ProposalSummaryWorkflowBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CounterProposalSummaryWorkflow(IProposalSummary view, SAHLCommonBaseController controller)
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
            //we need to set the type before the base init fires so that 
            //the correct proposal types are loaded
            _view.ShowProposals = ProposalTypeDisplays.CounterProposal;
            _view.ShowCreateCounterProposalButton = false;
            _view.ShowReasonsButton = true;
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;
        }
    }
}