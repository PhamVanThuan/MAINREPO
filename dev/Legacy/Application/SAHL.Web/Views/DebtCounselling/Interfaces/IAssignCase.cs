using System;
using System.Data;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.DebtCounselling.Interfaces
{
    public interface IAssignCase : IViewBase
    {
        event EventHandler<EventArgs> SubmitClick;
        event EventHandler<EventArgs> CancelClick;

        void BindUsers(DataTable dt, string selectedUser);

        string Message { set; get; }

        ListItem UserSelected { get; }
    }
}
