using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.DebtCounselling.Interfaces
{
    public interface IDebtCounsellingDetails : IViewBase
    {
        string ReferenceNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitButtonClicked;
    }
}
