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
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using Microsoft.Reporting.WebForms;

namespace SAHL.Web.Views.Correspondence.Interfaces
{
    public interface ICorrespondencePreview : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabContainer"></param>
        void AddReportsToPage(AjaxControlToolkit.TabContainer tabContainer);
    }
}
