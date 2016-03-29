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
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Reports.Interfaces
{

    /// <summary>
    /// 
    /// </summary>
    public interface IApplicationAudit : IViewBase
    {
        /// <summary>
        /// Event raised when the submit button is clicked.
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// Gets the application key entered in the input box.
        /// </summary>
        string ApplicationKey { get; }

        /// <summary>
        /// Binds a list of audit information to the screen.
        /// </summary>
        /// <param name="auditData"></param>
        void BindAuditData(IEventList<IAudit> auditData);

    }
}
