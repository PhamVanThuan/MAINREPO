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
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// Interface for ReOpenAccount
    /// </summary>
    public interface IReOpenAccount : IViewBase
    {
        /// <summary>
        /// YesNo button clicked to process reopening of account
        /// </summary>
        event EventHandler SubmitButtonClicked;
        /// <summary>
        /// No button Clicks - cancels event
        /// </summary>
        event EventHandler CancelButtonClicked;
    }
}
