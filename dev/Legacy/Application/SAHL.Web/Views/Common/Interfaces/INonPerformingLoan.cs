using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
	public interface INonPerformingLoan : IViewBase
	{
		bool CheckBoxVisible { set; }

		bool ProceedButtonVisible { set; }

		bool CheckBoxValue { set; get; }

		bool CheckBoxEnabled { set; }

		bool ShowConfirmWhenProceedClicked { set; }

		event EventHandler onProceedButtonClicked;

		event EventHandler onCancelButtonClicked;

		event EventHandler onCheckBoxChecked;

		void SetMTDInterest(Decimal amnt);

		void SetSuspendedInterestAmount(Decimal amnt);

		void SetMTDInterestFixed(Decimal amnt);
		void SetSuspendedInterestAmountFixed(Decimal amnt);
		void ShowFixedLeg();
	}
}
