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
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.Capitec.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContactClient : IViewBase
    {
        #region properties
        DateTime? ContactDate { get; set; }
        string Comments { get; }
        #endregion  


        #region EventHandlers

        /// <summary>
        /// Raised when the Submit button is clicked
        /// </summary>
        event EventHandler SubmitButtonClicked;

        /// <summary>
        /// Raised when the cancel button is clicked.
        /// </summary>
        event EventHandler CancelButtonClicked;

        #endregion



    }
}
