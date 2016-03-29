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

using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common
{
    public partial class ReassignUser : SAHLCommonBaseView,IReassignUser
    {

        public string SetDropDownText
        {
            set { titleConsultant.Text = value; }
        }

        public string SetHeaderText
        {
            set { lblHeaderText.Text = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (OnSubmitButtonClicked != null)
            {
                OnSubmitButtonClicked(sender, e);
            }
        }

        protected void ddlConsultantSelectedIndexChanged(object sender, EventArgs e)
        {
            if (onSelectedConsultantChanged != null)
                onSelectedConsultantChanged(sender, new KeyChangedEventArgs (ddlConsultant.SelectedValue));
        }

        public bool SetPostBackType
        {
            set
            {
                ddlConsultant.AutoPostBack = value;
            }
        }

        public bool SetPostBackTypeRole
        {
            set 
            { 
                ddlRole.AutoPostBack = value; 
            }
        }

        public bool ConsultantsRowVisible
        {
           set
           {
               trConsultants.Visible = value;
            }
        }

        public bool RoleVisible
        {
            set
            {
                ddlRole.Visible = value;
            }
        }

        public bool SubmitButtonVisible
        {
            set
            {
                btnSubmit.Visible = value;
            }
        }

        public bool CancelButtonVisible
        {
            set
            {
                btnCancel.Visible = value;
            }
        }

        public bool ShowGrid
        {
            set { UserRolesGrid.Visible = value; }
        }

        public bool ShowCommentRow
        {
            set { trComment.Visible = value; }
        }

        public bool ShowCheckBoxRow
        {
            set { trChkBx.Visible = value; }
        }

        public bool CheckBoxValue
        {
            get { return chkReassignBC.Checked; }
        }

        public string MemoDescription
        {
            get { return txtMemo.Text; }
        }

        #region IRouteToConsultant Members

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;

        public event KeyChangedEventHandler onSelectedConsultantChanged;

        public event KeyChangedEventHandler onSelectedRoleChanged;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="appRoleLst"></param>
        public void BindGridApplicationRoles(DataTable appRoleLst)
        {
            UserRolesGrid.Columns.Clear();
            UserRolesGrid.AddGridBoundColumn("StatusChangeDate", "Date", Unit.Percentage(15), HorizontalAlign.Left, true);
            UserRolesGrid.AddGridBoundColumn("Branch", "Branch", Unit.Percentage(30), HorizontalAlign.Left, true);
            UserRolesGrid.AddGridBoundColumn("LegalEntityFullName", "Commissionable Consultant", Unit.Percentage(40), HorizontalAlign.Left, true);
            UserRolesGrid.AddGridBoundColumn("Status", "Status", Unit.Percentage(15), HorizontalAlign.Left, true);
            UserRolesGrid.DataSource = appRoleLst;
            UserRolesGrid.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        public void BindConsultantList(IEventList<SAHL.Common.BusinessModel.Interfaces.IADUser> users)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            for (int x = 0; x < users.Count; x++)
            {
                dict.Add(users[x].Key.ToString(), users[x].LegalEntity.GetLegalName(SAHL.Common.BusinessModel.Interfaces.LegalNameFormat.Full));
            }
            ddlConsultant.DataSource = dict ;
            ddlConsultant.DataValueField = "Key";
            ddlConsultant.DataTextField = "Value";
            ddlConsultant.DataBind();
        }

        public void BindConsultantsAsPerMandates(Dictionary<int, string> consultants)
        {
            ddlConsultant.DataSource = consultants;
            ddlConsultant.DataValueField = "Key";
            ddlConsultant.DataTextField = "Value";
            ddlConsultant.DataBind();
        }

        public void BindUsers(IEventList<IADUser> adUsers)
        {

            IDictionary<string, string> dict = new Dictionary<string, string>();
            for (int x = 0; x < adUsers.Count; x++)
            {
                if (adUsers[x].LegalEntity != null)
                    dict.Add(adUsers[x].Key.ToString(), adUsers[x].LegalEntity.GetLegalName(SAHL.Common.BusinessModel.Interfaces.LegalNameFormat.Full));
            }
            ddlConsultant.DataSource = dict;
            ddlConsultant.DataValueField = "Key";
            ddlConsultant.DataTextField = "Value";
            ddlConsultant.DataBind();
        }

        public void ShowRolesDropDown()
        {
            trRoles.Visible = true;
        }

        public void ShowConsultantsDropDown()
        {
            trConsultants.Visible = true;
        }

        public void BindAgencies(IEventList<IApplicationOriginator> agencies)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            for (int x = 0; x < agencies.Count; x++)
            {
                dict.Add(agencies[x].Key.ToString(), agencies[x].LegalEntity.GetLegalName(SAHL.Common.BusinessModel.Interfaces.LegalNameFormat.Full));
            }

            ddlConsultant.DataSource = dict;
            ddlConsultant.DataValueField = "Key";
            ddlConsultant.DataTextField = "Value";
            ddlConsultant.DataBind();
        }

        public void BindSelectedApplicationRole(IApplicationRole AppRole, IEventList<IApplicationOriginator> appOriginators)
        {
            for (int i = 0; i < appOriginators.Count; i++)
            {
                if (AppRole.LegalEntity.Key == appOriginators[i].LegalEntity.Key)
                {
                    ddlConsultant.SelectedValue = appOriginators[i].Key.ToString();
                    break;
                }
            }
        }

        public void BindSelectedBranchAdmin(IApplicationRole AppRole, IEventList<IADUser> adUsers)
        {
            for (int i = 0; i < adUsers.Count; i++)
            {
                if (AppRole.LegalEntity.Key == adUsers[i].LegalEntity.Key)
                {
                    ddlConsultant.SelectedValue = adUsers[i].Key.ToString();
                    break;
                }
            }
        }

        public void BindRoles(IEventList<IApplicationRoleType> appRoles)
        {
            ddlRole.DataSource = appRoles;
            ddlRole.DataValueField = "Key";
            ddlRole.DataTextField = "Description";
            ddlRole.DataBind();
        }

        public void BindApplicationRoles(IEventList<IApplicationRole> appRoles)
        {
            IDictionary<int, string> _appRoleDict = new Dictionary<int, string>();
            foreach (IApplicationRole _appRole in appRoles)
            {
                string desc = _appRole.LegalEntity.DisplayName + " - " + _appRole.ApplicationRoleType.Description;
                _appRoleDict.Add(_appRole.Key, desc);
            }

            ddlRole.DataSource = _appRoleDict;
            ddlRole.DataValueField = "Key";
            ddlRole.DataTextField = "Value";
            ddlRole.DataBind();
        }

        public int SelectedConsultantKey
        {
            //get { return Convert.ToInt32(ddlConsultant.SelectedValue); }
            get
            {
                // Since this event is not always called on Onit, we need to get the selected value off the forms collection
                if (Request.Form[ddlConsultant.UniqueID] != null)
                {
                    ddlConsultant.SelectedValue = Request.Form[ddlConsultant.UniqueID];
                    if (ddlConsultant.SelectedValue != "-select-")
                        return Convert.ToInt32(ddlConsultant.SelectedValue);
                    else
                        return -1;
                }
                else
                    return -1; 
            }

        }

        public int SelectedRoleTypeKey
        {
            get
            {
                // Since this event is not always called on Onit, we need to get the selected value off the forms collection
                if (Request.Form[ddlRole.UniqueID] != null)
                {
                    ddlRole.SelectedValue = Request.Form[ddlRole.UniqueID];
                    if (ddlRole.SelectedValue != "-select-")
                        return Convert.ToInt32(ddlRole.SelectedValue);
                    else
                        return -1;
                }
                else
                    return -1; 
            }
        }

        public void BindSelectedUser(IADUser user)
        {
            ddlConsultant.SelectedValue = user.Key.ToString();
        }

        public void BindSelectedUserByMandate(IADUser user)
        {
            ddlConsultant.SelectedValue = user.LegalEntity.Key.ToString();
        }    
        #endregion

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onSelectedRoleChanged != null)
                onSelectedRoleChanged(sender, new KeyChangedEventArgs(ddlRole.SelectedValue));
        }
    }
}