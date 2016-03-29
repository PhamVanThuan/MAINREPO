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

namespace SAHL.Web.Views.Administration.Interfaces
{
    public interface IViewCBOSummary : IViewBase
    {
        /// <summary>
        /// Binds a CBO to the summary page.
        /// </summary>
        /// <param name="CBO"></param>
        void BindCBO(ICBOMenu CBO);
        /// <summary>
        /// FIres when the user clicks the finish button. This will save the CBO
        /// </summary>
        event EventHandler OnFinishClick;
    }
}
