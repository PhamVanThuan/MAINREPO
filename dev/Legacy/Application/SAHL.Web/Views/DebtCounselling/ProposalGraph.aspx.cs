using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using System.Data;
using DevExpress.XtraCharts;
using System.Drawing;

namespace SAHL.Web.Views.DebtCounselling
{
	public partial class ProposalGraph : SAHLCommonBaseView, IProposalGraph
	{
		/// <summary>
		/// On Init
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.ShouldRunPage)
                return;
        }

		/// <summary>
		/// On Page Load
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Render Proposal Key
		/// </summary>
		/// <param name="proposalKey"></param>
		public void RenderGraph(int proposalKey)
		{
            try
            {
                proposalGraph.Render(proposalKey);
            }
            catch (SAHL.Common.Exceptions.DomainValidationException ex)
            {
                this.Messages.Add(new SAHL.Common.DomainMessages.Error(ex.Message, ex.Message));
            }
		}
	}
}