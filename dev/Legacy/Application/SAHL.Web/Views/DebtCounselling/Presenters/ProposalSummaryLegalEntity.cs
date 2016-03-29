using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class  ProposalSummaryLegalEntity : ProposalSummaryBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ProposalSummaryLegalEntity(IProposalSummary view, SAHL.Common.Web.UI.SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }


        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            // Get the DebtCounsellingPolicyHistory collections
            _lstDebtCounsellingProposalSummary = _DebtCounsellingRepository.GetProposalsByGenericKey(base.GenericKey,GenericKeyTypes.LegalEntity);
            // bind the DebtCounselling premium history grid
            _view.BindProposalSummaryGrid(_lstDebtCounsellingProposalSummary);
        }


        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

        }
    }
}
