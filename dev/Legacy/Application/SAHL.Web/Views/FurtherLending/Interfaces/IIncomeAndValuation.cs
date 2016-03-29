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
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.FurtherLending.Interfaces
{
    public interface IIncomeAndValuation : IViewBase
    {
        /// <summary>
        /// Event raised when Submit button is clicked
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// Get the value of tbe Valuation checkbox
        /// </summary>
        bool ValuationChecked
        {
            get;
        }
        /// <summary>
        /// Get the value of tbe Income checkbox
        /// </summary>
        bool IncomeChecked
        {
            get;
        }
    }
}
