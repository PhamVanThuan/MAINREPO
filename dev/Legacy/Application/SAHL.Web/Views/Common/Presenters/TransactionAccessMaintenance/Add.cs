using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common.Presenters.TransactionAccessMaintenance
{
    public class Add : Base
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public Add(ITransactionAccessMaintenance view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.AddGroup = true;
            _view.ShowButton = true;
            _view.AllowUpdate = true;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;


            BindGroups();
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;


            GetTransactionTypesByADCredentials("", true);
        }

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            NavigateCancel();
        }

        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            if (_view.NewGroupDescription.Length < 1)
            {
                _view.Messages.Add(new Error("Please enter a transaction access group name.", "Please enter a transaction access group name."));
                return;
            }
            if (GroupExists(_view.NewGroupDescription))
            {
                _view.Messages.Add(new Error("The Transaction Access Group you entered already exists.", "The Transaction Access Group you entered already exists."));
                return;
            }

            CommitToDB(_view.NewGroupDescription);

            NavigateAdd();
        }
    }
}
