using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Common.Presenters.ITC
{
    public class ITCApplicationSummary : ITCApplication
    {
        /// <summary>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ITCApplicationSummary(SAHL.Web.Views.Common.Interfaces.IITC view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.DoEnquiryButtonVisible = false;
            _view.DoEnquiryColumnVisible = false;
        }
    }
}