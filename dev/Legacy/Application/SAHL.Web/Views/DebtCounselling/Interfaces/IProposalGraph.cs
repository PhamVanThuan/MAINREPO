using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SAHL.Web.Views.DebtCounselling.Interfaces
{
	/// <summary>
	/// Contract for a Proposal Graph
	/// </summary>
	public interface IProposalGraph
	{
		/// <summary>
		/// Render Proposal Graph
		/// </summary>
		/// <param name="proposalKey"></param>
		void RenderGraph(int proposalKey);
	}
}