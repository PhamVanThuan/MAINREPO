using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.DebtCounselling.Interfaces
{
	/// <summary>
	/// External Role Contract
	/// </summary>
	public interface IExternalRole : IViewBase
	{
		IDebtCounselling DebtCounsellingCase { get; set; }
	}
}