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
using SAHL.X2.Framework.Interfaces;
using Castle.ActiveRecord;
using System.Collections.Generic;

namespace SAHL.Web.Views.Correspondence.Presenters.Correspondence
{
    public class CorrespondenceProcessingMultipleWorkflowDebtCounsellor : CorrespondenceProcessingMultipleWorkflow
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CorrespondenceProcessingMultipleWorkflowDebtCounsellor(ICorrespondenceProcessing view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _view.DisplayDebtCounsellors = true;

            base.OnViewInitialised(sender, e);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
        }
    }
}
