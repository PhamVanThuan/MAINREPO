using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.QuickCash.Interfaces;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.QuickCash.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class WaiveCharges : SAHLCommonBasePresenter<IWaiveCharges>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="View"></param>
        /// <param name="Controller"></param>
        public WaiveCharges(IWaiveCharges View, SAHLCommonBaseController Controller)
            : base(View, Controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }


    }
}
