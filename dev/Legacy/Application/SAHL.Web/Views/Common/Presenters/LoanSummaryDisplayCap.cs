using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.UI;

using SAHL.Common;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class LoanSummaryDisplayCap : LoanSummaryBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LoanSummaryDisplayCap(ILoanSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            InstanceNode _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);
            
            Dictionary<string, object> _x2Data = _node.X2Data as Dictionary<string, object>;
            int _accountKey = Convert.ToInt32(_x2Data["AccountKey"]);

            IAccount account = _accRepository.GetAccountByKey(_accountKey);
            BindAccountToView(account);
        }

    }
}
