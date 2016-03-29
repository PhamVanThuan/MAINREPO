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

namespace SAHL.Web.Views.Common.Presenters.TransactionAccessMaintenance
{
    public class ADUserUpdate : Base
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ADUserUpdate(ITransactionAccessMaintenance view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.ShowUser = true;
            _view.ShowButton = true;
            _view.AllowUpdate = true;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
        }

        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            CommitToDB(_view.UserSelectedValue);
        }

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            NavigateCancel();
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {

            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            BindUsers();

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            GetTransactionTypesByADCredentials(_view.UserSelectedValue, true);
        }

        
    }
}
