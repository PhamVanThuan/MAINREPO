using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Correspondence.Interfaces;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Correspondence.Presenters.Correspondence
{
    /// <summary>
    /// 
    /// </summary>
    public class CorrespondenceHistoryLifeOrigination : CorrespondenceHistoryBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CorrespondenceHistoryLifeOrigination(ICorrespondenceHistory view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.ShowCancelButton = true;
            _view.ShowLifeWorkFlowHeader = true;
        }



    }
}
