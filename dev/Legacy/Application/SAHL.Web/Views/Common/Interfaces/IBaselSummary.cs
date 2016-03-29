using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using System.Data;

namespace SAHL.Web.Views.Common.Interfaces
{
	/// <summary>
	/// Basel Summary Contract
	/// </summary>
	public interface IBaselSummary : IViewBase
	{
		/// <summary>
		/// Bind Behavioural Scores
		/// </summary>
		/// <param name="behaviouralScoresTable"></param>
		void BindBehaviouralScores(DataTable behaviouralScoresTable);
	}
}