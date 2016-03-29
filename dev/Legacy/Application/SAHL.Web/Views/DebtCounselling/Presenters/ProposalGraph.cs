using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Data;
using SAHL.Common.DataSets;
using SAHL.Common.BusinessModel.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
	public class ProposalGraph : SAHLCommonBasePresenter<IProposalGraph>
	{
		/// <summary>
		/// Constructor for Proposal Graph
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
		public ProposalGraph(IProposalGraph view, SAHLCommonBaseController controller) : base(view, controller)
        {
        }

		/// <summary>
		/// On View Initialized
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			_view.RenderGraph(57);
		}
	}
}