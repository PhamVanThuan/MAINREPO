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
	public interface IConvertStaffLoan : IViewBase
	{
		#region Methods

		void BindAccountDetails(IAccount account, IMortgageLoan vML, IMortgageLoan fML);

		#endregion

		#region Properties
		bool ConvertButtonEnabled { get; set; }
		bool UnConvertButtonEnabled { get; set; }

		event EventHandler onConvertButtonClicked;

		event EventHandler onUnConvertButtonClicked;

		#endregion
	}
}
