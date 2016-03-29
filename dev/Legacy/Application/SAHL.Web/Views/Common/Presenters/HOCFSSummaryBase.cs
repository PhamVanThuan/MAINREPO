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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;
using SAHL.Common.Collections;
using SAHL.Common.Exceptions;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common.Presenters
{
    public class HOCFSSummaryBase : SAHLCommonBasePresenter<IHOCFSSummary>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public HOCFSSummaryBase(IHOCFSSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected bool Validate()
        {
            string errMsg = string.Empty;

            if (Convert.ToInt32(_view.SelectedHOCInsurerValue) == -1)
                errMsg = "Please select a HOC Insurer.";

            if (string.IsNullOrEmpty(errMsg))
                return true;
            else
            {
                _view.Messages.Add(new Error(errMsg, errMsg));
                return false;
            }
        }
    }
}
