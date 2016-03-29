using System;
using System.Data;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;

namespace SAHL.Web.Views.DebtCounselling
{
    public partial class AssignCase : SAHLCommonBaseView, IAssignCase
    {
        public event EventHandler<EventArgs> SubmitClick;
        public event EventHandler<EventArgs> CancelClick;

        public string Message
        {
            set { lblMessage.Text = value.ToString(); }
            get { return lblMessage.Text; }
        }

        public ListItem UserSelected
        {
            get { return  ddlUser.SelectedItem; }
        }
        

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// On Submit Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnSubmitClick(object sender, EventArgs e)
        {

            if (SubmitClick != null)
            {
                SubmitClick(this, e);
            }
        }

        /// <summary>
        /// On Cancel Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnCancelClick(object sender, EventArgs e)
        {
            if (CancelClick != null)
            {
                CancelClick(sender, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="selectedUser"></param>
        public void BindUsers(DataTable dt, string selectedUser)
        {
            ddlUser.DataTextField = "Description";
            ddlUser.DataValueField = "ADUser";
            ddlUser.DataSource = dt;
            ddlUser.DataBind();

            if (!string.IsNullOrEmpty(selectedUser))
                ddlUser.SelectedValue = selectedUser;
        }
    }
}