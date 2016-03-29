using System;
using System.Collections.Generic;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using System.Data;
using SAHL.Web.Views.DebtCounselling.Presenters;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.DebtCounselling.Interfaces
{
	/// <summary>
	/// Create a Case for a Client
	/// </summary>
	public interface ICreateCase : IViewBase
	{
		SAHLTreeView TreeViewAccount { get; }

		event EventHandler<EventArgs> CreateCaseClick;
		event EventHandler<EventArgs> CancelClick;
		event KeyChangedEventHandler LegalEntityIDNumberSelected;
		event KeyChangedEventHandler PersonOfInterestClick;

		void UpdateDisplay(IList<AccountForView> accounts);
		void ShowDebtCounsellor(ILegalEntity debtCounsellor);
        DateTime? Date17pt1 { set; get; }
        string ReferenceNumber { get; }
	}
}