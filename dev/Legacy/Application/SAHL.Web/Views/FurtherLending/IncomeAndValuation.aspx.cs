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
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common;
using System.Text;
using SAHL.Common.Authentication;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.FurtherLending.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.FurtherLending
{
    public partial class IncomeAndValuation : SAHLCommonBaseView, IIncomeAndValuation
    { 
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        #region IIncomeAndValuation Members

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            OnSubmitButtonClicked(sender, e);
        }

       
        /// <summary>
        /// Return the Valuation checkbox value
        /// </summary>
        public bool ValuationChecked
        {
            get { return chkValuation.Checked; }
        }
        /// <summary>
        /// Return the Income checkbox value
        /// </summary>
        public bool IncomeChecked
        {
            get { return chkIncome.Checked; }
        }

        #endregion
    }
}
