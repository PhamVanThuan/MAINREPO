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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.DomainMessages;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.Origination
{
    public partial class DailyAdminAssign : SAHLCommonBaseView, IDailyAdminAssign
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ddlRoleTypes.SelectedValue = Page.Request.Form[ddlRoleTypes.UniqueID];
        }

        public void BindGridPostRowUpdate(DataTable dt)
        {
            gvADUserStatusUpdate.CancelEdit();
            gvADUserStatusUpdate.DataSource = dt;
            gvADUserStatusUpdate.DataBind();
        }

        public void BindRoleTypes(Dictionary<string, string> RoleTypes)
        {
            ddlRoleTypes.DataSource = RoleTypes;
            ddlRoleTypes.DataBind();
        }

        public void SetUpUserStatusGrid(ICollection<IGeneralStatus> generalStatus)
        {
            gvADUserStatusUpdate.SettingsPager.PageSize = 20;
            gvADUserStatusUpdate.KeyFieldName = "UserOrganisationStructureRoundRobinStatusKey";
            gvADUserStatusUpdate.AddGridColumn("UserOrganisationStructureRoundRobinStatusKey", "UserOrganisationStructureRoundRobinStatusKey", 0, GridFormatType.GridString, null, HorizontalAlign.Left, false, true);
            gvADUserStatusUpdate.AddGridColumn("ADUserName", "ADUserName", 0, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            gvADUserStatusUpdate.AddGridColumn("UserName", "UserName", 0, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            gvADUserStatusUpdate.AddGridCommandColumnComboBox("ADUserStatus", "User Status", 0, HorizontalAlign.Left, true, generalStatus, "Description", "Description");
            gvADUserStatusUpdate.AddGridCommandColumnComboBox("RoundRobinStatus", "Round Robin Status", 0, HorizontalAlign.Left, true, generalStatus, "Description", "Description");
            gvADUserStatusUpdate.AddGridCommandColumnComboBox("CapitecRoundRobinStatus", "Capitec Round Robin Status", 0, HorizontalAlign.Left, true, generalStatus, "Description", "Description");
        }

        public void PopulateUsersInGrid(DataTable dt)
        {
            gvADUserStatusUpdate.Selection.UnselectAll();
            gvADUserStatusUpdate.DataSource = dt;
            gvADUserStatusUpdate.DataBind();
        }

        protected void ddlRoleTypesSelectedIndexChanged(object sender, EventArgs e)
        {
            if (onRoleTypeSelectedIndexChange != null)
                onRoleTypeSelectedIndexChange(sender, new KeyChangedEventArgs(ddlRoleTypes.SelectedValue));
        }


        #region Events

        public event KeyChangedEventHandler onRoleTypeSelectedIndexChange;
        public event EventHandler onSubmitButtonClicked;
        public event EventHandler onCancelButtonClicked;
        public event KeyChangedEventHandler OnRowUpdating;

        protected void Cancel_Click(object sender, EventArgs e)
        {
            if (onCancelButtonClicked != null)
                onCancelButtonClicked(sender, e);
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            if (ddlRoleTypes.SelectedValue == "-select-")
                Messages.Add(new Error("Please select a RoleType", ""));

            if (Messages.ErrorMessages.Count == 0 && onSubmitButtonClicked != null)
            {
                onSubmitButtonClicked(sender, e);
            }
        }

        protected void gvADUserStatusUpdate_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            if (OnRowUpdating != null)
             {
                e.Cancel = true;
                int editingIndex = ((SAHL.Common.Web.UI.Controls.DXGridView)sender).FindVisibleIndexByKeyValue(e.Keys[0]);
                Dictionary<string, string> dictNewValues = new Dictionary<string, string>();
                dictNewValues.Add("ADUserStatus", e.NewValues["ADUserStatus"].ToString());
                dictNewValues.Add("RoundRobinStatus", e.NewValues["RoundRobinStatus"].ToString());
                dictNewValues.Add("CapitecRoundRobinStatus", e.NewValues["CapitecRoundRobinStatus"].ToString());
                OnRowUpdating(dictNewValues, new KeyChangedEventArgs(editingIndex));
                }
             }

        #endregion


    }
}
