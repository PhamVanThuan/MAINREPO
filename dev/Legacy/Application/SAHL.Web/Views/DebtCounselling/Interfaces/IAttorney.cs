using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Web.UI;
using presenter = SAHL.Web.Views.DebtCounselling.Interfaces;
using models = SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.DebtCounselling.Interfaces
{
	/// <summary>
	/// Attorney Contract
	/// </summary>
	public interface IAttorney : IViewBase
	{
		/// <summary>
		/// Indicates whether the attorney can be updated
		/// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        bool Readonly { get; set; }
		int SelectedAttorneyKey { get; set; }
		string SelectedAttorneyName { get; set; }

		event EventHandler<EventArgs> AttorneyUpdateClick;

		/// <summary>
		/// Bind Attornies
		/// </summary>
		/// <param name="attornies"></param>
		/// <param name="selectedAttorneyKey"></param>
		void BindAttornies(IDictionary<int, string> attornies, int selectedAttorneyKey);
	}
}