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
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.AJAX;

namespace SAHL.Web.Views.Administration
{
    public partial class AdUser : SAHLCommonBaseView, IAduser
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage)
                return;

            RegisterWebService(ServiceConstants.ActiveDirectory);

        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!ShouldRunPage)
                return;

            tblMaint.Visible = _visibleMaint;

            if (_userExistsInDatabase)
                btnSubmit.Text = "Update ADUser Record";
            else
                btnSubmit.Text = "Add ADUser Record";

            if (String.IsNullOrEmpty(_selectedUserName))
                txtActiveDirectorySearch.Text = "";
        }

        public void BindStatusDropDown(ICollection<IGeneralStatus> generalStatus)
        {
            ddlStatus.DataValueField = "Key";
            ddlStatus.DataTextField = "Description";
            ddlStatus.DataSource = generalStatus;
            ddlStatus.DataBind();
        }

        protected void acActiveDirectorySearch_ItemSelected(object sender, KeyChangedEventArgs e)
        {
            _selectedUserName = e.Key.ToString();

            if (OnAdUserSelected != null)
                OnAdUserSelected(sender, e);
        }

        #region IAduser Members

        private bool _visibleMaint;
        public bool VisibleMaint
        {
            set { _visibleMaint = value; }
        }

        private bool _userExistsInDatabase;
        public bool UserExistsInDatabase
        {
            set { _userExistsInDatabase = value; }
        }

        private string _selectedUserName;
        public string SelectedUserName
        {
            get { return _selectedUserName; }
            set { _selectedUserName = value; }
        }

        public void BindAdUser(ActiveDirectoryUserBindableObject ActiveDirectoryUser, IADUser ADUser)
        {
            // if we have an ADUser database record then bind the data from that record
            if (ADUser != null)
            {
                txtAdUserName.Text = ADUser.ADUserName;
                txtFirstName.Text = ADUser.LegalEntity.FirstNames;
                txtSurname.Text = ADUser.LegalEntity.Surname;
                txtCellNum.Text = ADUser.LegalEntity.CellPhoneNumber;
                txtEMail.Text = ADUser.LegalEntity.EmailAddress;
                ddlStatus.SelectedValue = ADUser.GeneralStatusKey.Key.ToString();
            }
            else // otherwise bind from the active directory object
            {
                txtAdUserName.Text = @"SAHL\" + ActiveDirectoryUser.ADUserName;
                txtFirstName.Text = ActiveDirectoryUser.FirstName;
                txtSurname.Text = ActiveDirectoryUser.Surname;
                txtCellNum.Text = ActiveDirectoryUser.CellNumber;
                txtEMail.Text = ActiveDirectoryUser.EmailAddress;
            }
        }

        public event KeyChangedEventHandler OnAdUserSelected;

        public event EventHandler OnSubmitClick;

        public string AdUserName 
        {
            get { return Page.Request.Form[txtAdUserName.UniqueID].ToString(); }
            set { txtAdUserName.Text = value; }
        }
        public int GeneralStatusKey { get { return Convert.ToInt32(ddlStatus.SelectedValue); } }
        public string FirstName { get { return txtFirstName.Text; } }
        public string Surname { get { return txtSurname.Text; } }
        public string CellPhoneNumber { get { return txtCellNum.Text; } }
        public string EMail { get { return txtEMail.Text; } }
        public bool VisibleSubmit { set { trSubmit.Visible = value; } }
        #endregion

        protected void Submit_Click(object sender, EventArgs e)
        {
            if (OnSubmitClick != null)
                OnSubmitClick(null, e);
        }
    }
}
