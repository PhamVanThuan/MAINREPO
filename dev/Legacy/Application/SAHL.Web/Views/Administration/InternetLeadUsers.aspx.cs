using System;
using System.Data;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Administration
{
    public partial class InternetLeadUsers : SAHLCommonBaseView, Interfaces.IInternetLeadUsers
    {
        //public event EventHandler btnRefreshClick;
        public event EventHandler btnUpdateClick;
        public event EventHandler btnCancelClick;

        public event EventHandler btnAddClick;
        public event EventHandler btnRemoveClick;


        public event KeyChangedEventHandler lstActiveUsersSelectedIndexChanged;
        public event KeyChangedEventHandler lstInactiveUsersSelectedIndexChanged;


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAddClick != null)
                btnAddClick(sender, e);
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            if (btnRemoveClick != null)
                btnRemoveClick(sender, e);
        }

        //protected void btnRefresh_Click(object sender, EventArgs e)
        //{
        //    if (btnRefreshClick != null)
        //        btnRefreshClick(sender, e);
        //}

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdateClick != null)
                btnUpdateClick(sender, e);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancelClick != null)
                btnCancelClick(sender, e);
        }

        public void PopulatelstActiveUsers(DataTable activeusers)
        {

            lstActiveUsers.AutoGenerateColumns = false;
            lstActiveUsers.Columns.Clear();
            lstActiveUsers.AddGridBoundColumn("Key", "Key", Unit.Percentage(0), HorizontalAlign.Left, false);
            lstActiveUsers.AddGridBoundColumn("ADUserName", "ADUserName", Unit.Percentage(100), HorizontalAlign.Left, true);
            lstActiveUsers.DataSource = activeusers;
            lstActiveUsers.DataBind();
        }



        public void PopulatelstInactiveUsers(DataTable inactiveusers)
        {

            lstInactiveUsers.AutoGenerateColumns = false;
            lstInactiveUsers.Columns.Clear();
            lstInactiveUsers.AddGridBoundColumn("Key", "Key", Unit.Percentage(0), HorizontalAlign.Left, false);
            lstInactiveUsers.AddGridBoundColumn("ADUserName", "ADUserName", Unit.Percentage(100), HorizontalAlign.Left, true);
            lstInactiveUsers.DataSource = inactiveusers;
            lstInactiveUsers.DataBind();
        }




        private int inactiveUsersIndex;
        /// <summary>
        /// Gets or sets the Inactive Users index
        /// </summary>
        public int InactiveUsersIndex
        {
            set { lstInactiveUsers.SelectedIndex = value; }
            get { return inactiveUsersIndex; }
        }

        private int activeUsersIndex;
        /// <summary>
        /// Gets or sets the Active Users index
        /// </summary>
        public int ActiveUsersIndex
        {
            set { lstActiveUsers.SelectedIndex  = value; }
            get { return activeUsersIndex; }
        }


        protected void lstActiveUsers_SelectedIndexChanged(object sender, EventArgs e)
        {

            activeUsersIndex = lstActiveUsers.SelectedIndex;
            if (lstActiveUsersSelectedIndexChanged != null)
                lstActiveUsersSelectedIndexChanged(sender, new KeyChangedEventArgs(lstActiveUsers.SelectedIndex));
        }



        protected void lstInactiveUsers_SelectedIndexChanged(object sender, EventArgs e)
        {

            inactiveUsersIndex = lstInactiveUsers.SelectedIndex;
            if (lstInactiveUsersSelectedIndexChanged != null)
                lstInactiveUsersSelectedIndexChanged(sender, new KeyChangedEventArgs(lstInactiveUsers.SelectedIndex));
        }



    }
}
