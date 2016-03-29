using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Web.Views.Administration.Interfaces;
using System.Data;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.DomainMessages;
using DevExpress.Web.Data;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using DevExpress.Web.ASPxGridView;
namespace SAHL.Web.Views.Administration
{
    public partial class AttorneyContact : SAHLCommonBaseView, SAHL.Web.Views.Administration.Interfaces.IAttorneyContact
    {
        public string AttorneyName
        {
            set
            {
                lblAttorneyNameValue.Text = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Not a read only property.")]
        public Dictionary<int, string> ExternalRoleTypeKeys
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<LegalEntityEventArgs> LegalEntityAdd;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ShouldRunPage) return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!ShouldRunPage) return;

            if (Request.Form["__EVENTTARGET"] == "dxImg")
            {
                btnAddToCBO_Click(this, new KeyChangedEventArgs(Request.Form["__EVENTARGUMENT"]));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;
			legalEntitySection.Visible = !readOnly;			
        }

        #region Legal Entity Code
        /// <summary>
        /// Bind External Role Types
        /// </summary>
        /// <param name="externalRoleTypes"></param>
        public void BindExternalRoleTypes(IDictionary<int, string> externalRoleTypes)
        {
            cmbRoleType.DataSource = externalRoleTypes;
            cmbRoleType.DataTextField = "Value";
            cmbRoleType.DataValueField = "Key";
            cmbRoleType.DataBind();
        }

        /// <summary>
        /// On Add Legal Entity Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnAddLegalEntityClick(object sender, EventArgs e)
        {
            if (LegalEntityAdd != null)
            {
                if (cmbRoleType.SelectedIndex == 0)
                {
                    Messages.Add(new Error("Please select a Role Type for this contact", "Please select a Role Type for this contact"));
                    return;
                }
                var legalEntityEventArgs = new LegalEntityEventArgs
                {
                    FirstName = txtFirstName.Text,
                    Surname = txtSurname.Text,
                    EmailAddress  =txtEmailAddress.Text,
                    FaxNumber = txtFaxNumber.Number,
                    FaxCode = txtFaxNumber.Code,
                    TelephoneNumber = txtTelephoneNumber.Number,
                    TelephoneCode = txtTelephoneNumber.Code,
                    RoleTypeKey = int.Parse(cmbRoleType.SelectedValue)
                };
                LegalEntityAdd(this, legalEntityEventArgs);
            }
        }

        /// <summary>
        /// Clear Legal Entity Fields
        /// </summary>
        public void ClearLegalEntityFields()
        {
            txtFirstName.Text = String.Empty;
            txtSurname.Text = String.Empty;
            txtTelephoneNumber.Code = String.Empty;
            txtTelephoneNumber.Number = String.Empty;
            txtEmailAddress.Text = String.Empty;
            txtFaxNumber.Code = String.Empty;
            txtFaxNumber.Number = String.Empty;
            cmbRoleType.SelectedIndex = 0;
        }
        #endregion

        #region gridSection

        /// <summary>
        /// Binds and takes the grid off Edit mode
        /// </summary>
        /// <param name="dt"></param>
        public void BindSetUplitigationAttorneyGridPostRowUpdate(DataTable dt)
        {
            litigationAttorneyContactsGrid.CancelEdit();
            litigationAttorneyContactsGrid.DataSource = dt;
            litigationAttorneyContactsGrid.DataBind();
        }
        
        /// <summary>
        /// Sets up grid and binds
        /// </summary>
        /// <param name="dt"></param>
        public void SetUplitigationAttorneyGrid(DataTable dt)
        {
            // Settings
            litigationAttorneyContactsGrid.KeyFieldName = "LegalEntityKey";
            litigationAttorneyContactsGrid.SettingsPager.PageSize = 10;
            litigationAttorneyContactsGrid.SettingsBehavior.AllowFocusedRow = true;

            if (!ReadOnly)
            {
                litigationAttorneyContactsGrid.ClientSideEvents.RowClick = "function(s, e){s.StartEditRow(e.visibleIndex);}";
                // Events
                litigationAttorneyContactsGrid.StartRowEditing += new DevExpress.Web.Data.ASPxStartRowEditingEventHandler(litigationAttorneyContactsGrid_StartRowEditing);
                litigationAttorneyContactsGrid.RowUpdating += new DevExpress.Web.Data.ASPxDataUpdatingEventHandler(litigationAttorneyContactsGrid_RowUpdating);
            }

            // Add Columns
            litigationAttorneyContactsGrid.AddGridColumn("LegalEntityKey", "LegalEntityKey", 0, GridFormatType.GridString, null, HorizontalAlign.Left, false, true);
            litigationAttorneyContactsGrid.AddGridColumn("Name", "Name", 20, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);

            // Dynamically adding columns 
            int width = (int)Math.Round(70.0/ExternalRoleTypeKeys.Count);
            foreach (var item in ExternalRoleTypeKeys)
            {
                litigationAttorneyContactsGrid.AddGridCheckBoxColumn(item.Value, width, HorizontalAlign.Left, true, item.Key.ToString(), true, false);
            }

            // This is dynamically adding a data item template column
            DXGridViewFormattedTextColumn button = new DXGridViewFormattedTextColumn();
            button.FieldName = "";
            button.ReadOnly = true;
            button.Caption = "Add To Menu";
            button.Visible = true;
            button.DataItemTemplate = new AddLinkTemplate();
            button.Width = Unit.Percentage(10);
            button.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            litigationAttorneyContactsGrid.Columns.Add(button);

            
            // Bind Grid
            litigationAttorneyContactsGrid.Selection.UnselectAll();
            litigationAttorneyContactsGrid.DataSource = dt;
            litigationAttorneyContactsGrid.DataBind();
        }

        public event KeyChangedEventHandler OnSelectionChanged;
        /// <summary>
        /// When the row is clicked it goes into edit mode and further we have to cache the selected LE key
        /// as well as set the focused row index
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void litigationAttorneyContactsGrid_StartRowEditing(object sender, ASPxStartRowEditingEventArgs e)
        {
            if (OnSelectionChanged != null)
            {
                OnSelectionChanged(sender, new KeyChangedEventArgs(e.EditingKeyValue));
                int editingIndex = ((SAHL.Common.Web.UI.Controls.DXGridView)sender).FindVisibleIndexByKeyValue(e.EditingKeyValue);
                litigationAttorneyContactsGrid.FocusedRowIndex = editingIndex;
            }

        }

        public event KeyChangedEventHandler OnCheckedChanged;
        /// <summary>
        /// Once a value is changed on the grid the even fired to update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void litigationAttorneyContactsGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            if (litigationAttorneyContactsGrid != null)
            {
                e.Cancel = true;
                Dictionary<string, string> dictNewValues = new Dictionary<string, string>();
                foreach (var item in ExternalRoleTypeKeys)
                {
                    // There is a change so let's postback
                    if (e.OldValues[item.Key.ToString()].ToString() != e.NewValues[item.Key.ToString()].ToString())
                    {
                        if (Convert.ToInt32(e.NewValues[item.Key.ToString()]) == Convert.ToInt32(true))
                            dictNewValues.Add("Checked", true.ToString());
                        else
                            dictNewValues.Add("Checked", false.ToString());

                        dictNewValues.Add("LegalEntityKey", e.Keys[0].ToString());
                        dictNewValues.Add("ExternalRoleTypeKey", item.Key.ToString());
                        break;
                    }
                }
                
                if (dictNewValues.Count > 0)
                    OnCheckedChanged(dictNewValues,new KeyChangedEventArgs(e.Keys[0]));
            }
        }

        public event KeyChangedEventHandler OnAddToCBO;
        /// <summary>
        /// Add the LE to the CBO for editing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddToCBO_Click(object sender, KeyChangedEventArgs e)
        {
            if (OnAddToCBO != null)
                OnAddToCBO(sender, new KeyChangedEventArgs(e.Key));
        }

        #endregion

        private bool readOnly;
        public bool ReadOnly
        {
            get
            { return readOnly; }
            set
            {
                readOnly = value;
            }
        }

        public event EventHandler OnDone;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnDoneClick(object sender, EventArgs e)
        {
            if (OnDone != null)
                OnDone(sender, e);
        }

    }

    class AddLinkTemplate : ITemplate
    {
        public void InstantiateIn(Control container)
        {
            DevExpress.Web.ASPxGridView.GridViewDataItemTemplateContainer cont = (GridViewDataItemTemplateContainer)container;
            Image dxImg = new Image();
            string js = string.Format("javascript:__doPostBack('dxImg','{0}')", cont.KeyValue);
            dxImg.Attributes.Add("onclick", js);
            dxImg.ImageUrl = "../../Images/add_blue1.gif";
            cont.Controls.Add(dxImg);

        }
    }
}