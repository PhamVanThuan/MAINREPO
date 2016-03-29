using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;

namespace SAHL.Web.Views.PersonalLoan
{
    public partial class AssignUser : SAHLCommonBaseView, IAssignUser
    {
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (OnSubmitButtonClicked != null)
            {
                OnSubmitButtonClicked(sender, e);
            }
        }

        protected void ddlExceptionsManagerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (onSelectedUserChanged != null)
                onSelectedUserChanged(sender, new KeyChangedEventArgs(ddlExceptionsManager.SelectedValue));
        }

        public event EventHandler OnCancelButtonClicked;

        public event EventHandler OnSubmitButtonClicked;

        public event KeyChangedEventHandler onSelectedUserChanged;

        public void BindUsers(IEventList<IADUser> users)
        {
            this.ddlExceptionsManager.DataSource = users;
            this.ddlExceptionsManager.DataTextField = "ADUserName";
            this.ddlExceptionsManager.DataValueField = "ADUserName";
            this.ddlExceptionsManager.DataBind();
        }
    }
}