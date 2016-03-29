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
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Controls;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.Views.Administration
{
    public partial class Admin_UserOrganisationStructure : SAHLCommonBaseView,IAdmin_UserOrganisationStructure
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        #region Properties

        private bool _addNode = true;

		private Dictionary<string,string> _roleTypes;

        private Dictionary<int, bool> _userCheckedNodes;
		

        public void ADUserResultsGridClear()
        {
            ADUserResultsGrid.Selection.UnselectAll();
            ADUserResultsGrid.FocusedRowIndex = -1;
        }

        public bool CanAddNode
        {
            set { _addNode = value; }
        }

        public string SubmitButtonText
        {
            set { btnSubmit.Text = value; }
        }

        public bool OrgStructVisible
        {
            set { tblOrgStruct.Visible = value; }
        }

        public bool CompanyListVisble
        {
            set 
            { 
                tblCompanyList.Visible = value; 
                panelOrgStruct.Visible = value;
                panelOrgStructHeader.Visible = value;
            }
        }

        public bool ADUserSearchVisible
        {
            set 
            { 
                panelADUserSearch.Visible = value;
                panelADUserSearchHeader.Visible = value; 
            }
        }

        public bool UserSummaryGridVisible
        {
            set { tblUserSummaryGrid.Visible = value; }
        }

        public string LabelHeadingText
        {
            set { lblHeading.Text = value; }
        }

        public string ADUserResultsGridTitle
        {
            set { ADUserResultsGrid.SettingsText.Title = value; }
        }

        public string ADUserName
        {
            get { return txtADUserName.Text; }
        }

        public string CompanySelectedValue
        {
            get { return ddlCompany.SelectedValue; }
            set { ddlCompany.SelectedValue = value; }
        }

        public bool ADUserResultsGridButtonsVisible
        {
            set { tblADUserResultsGridBtn.Visible = value; }
        }

        public bool ADUserSearchResultsVisible
        {
            set 
            {
                panelADUserSearchResults.Visible = value;
                panelADUserSearchResultsHeader.Visible = value;
            }
        }

        public void UserCheckedNodes(Dictionary<int, bool> nodes)
        {
            _userCheckedNodes = nodes;
        }

		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Not a read only property.")]
		public  Dictionary<string, string> RoleTypes
		{
			get
			{
				return _roleTypes;
			}

			set
			{
				_roleTypes = value;
			}
		}

        public Dictionary<int,bool> SelectedNodes
        {
            get
            {
                Dictionary<int, bool> dictSelectedNodes = new Dictionary<int, bool>();
                foreach (string valuePath in tvOrgStruct.CheckedValuePaths)
                {
                    string[] values = valuePath.Split('/');
                    dictSelectedNodes.Add(Convert.ToInt32(values[values.Length - 1]),false);
                }
                return dictSelectedNodes;
            }
        }
		

        public bool SubmitButtonVisible
        {
            set { btnSubmit.Visible = value; }
        }

        public bool CancelButtonVisible
        {
            set { btnCancel.Visible = value; }
        }

        public int ADUserResultsGridPageIndex
        {
            get { return ADUserResultsGrid.PageIndex; }
            set { ADUserResultsGrid.PageIndex = value; }
        }

        public int ADUserResultsGridFocusedRowIndex 
        {
            get { return ADUserResultsGrid.FocusedRowIndex; }
            set { ADUserResultsGrid.FocusedRowIndex = value; }
        }

        #endregion

        #region Methods

        public void ClearOrganisationStructure() 
        {
            tvOrgStruct.Nodes.Clear();
        }

        public void BindOrganisationStructure(IList<IBindableTreeItem> orgStructLst)
        {
            BindTreeview(orgStructLst, tvOrgStruct.Nodes);
        }

		
        public void BindCompanyList(IEventList<IOrganisationStructure> companyList)
        {
            ddlCompany.DataSource = companyList;
            ddlCompany.DataTextField = "Description";
            ddlCompany.DataValueField = "Key";
            ddlCompany.DataBind();
        }

        public void BindUserSummaryGrid(DataTable dt)
        {
            UserSummaryGrid.Selection.UnselectAll();
            UserSummaryGrid.DataSource = dt;
            UserSummaryGrid.DataBind();
        }

        public void BindUserSummaryGridPostRowUpdate(DataTable dt)
        {
            UserSummaryGrid.CancelEdit();
            UserSummaryGrid.DataSource = dt;
            UserSummaryGrid.DataBind();
        }

        public void BindADUserResultsGrid(DataTable ADUsers)
        {
            ADUserResultsGrid.Selection.UnselectAll();
            ADUserResultsGrid.DataSource = ADUsers;
            ADUserResultsGrid.DataBind();
        }

        public void ClearADUserResultsGrid()
        {
            ADUserResultsGrid.FocusedRowIndex = -1;
            ADUserResultsGrid.Selection.UnselectAll();
        }

		//public void BindRoleTypes(Dictionary<string, string> RoleTypes)
		//{
		//    ddlRoleTypes.DataSource = RoleTypes;
		//    ddlRoleTypes.DataBind();
		//}

        public void SetUpADUserResultsGridSelect()
        {
            ADUserResultsGrid.PostBackType = GridPostBackType.None;
            ADUserResultsGrid.KeyFieldName = "ADUserKey";
            ADUserResultsGrid.SettingsBehavior.AllowFocusedRow = true;
            ADUserResultsGrid.ClientSideEvents.RowClick = "function(s, e){s._selectAllRowsOnPage(false); s.SelectRow(e.visibleIndex, true);}";
            ADUserResultsGrid.SettingsPager.PageSize = 20;
            ADUserResultsGrid.AddGridColumn("ADUserKey", "ADUserKey", 0, GridFormatType.GridString, null, HorizontalAlign.Left, false, true);
            ADUserResultsGrid.AddGridColumn("ADUserName", "ADUserName", 10, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            ADUserResultsGrid.AddGridColumn("IDNumber", "ID Number", 10, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            ADUserResultsGrid.AddGridColumn("FirstName", "First Name", 10, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            ADUserResultsGrid.AddGridColumn("Surname", "Surname", 10, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            ADUserResultsGrid.AddGridColumn("EmailAddress", "EMail Address", 10, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
        }

        public void SetUpADUserResultsGridView()
        {
            ADUserResultsGrid.PostBackType = GridPostBackType.None;
            panelADUserSearchResults.BorderStyle = BorderStyle.None;
            panelADUserSearchResults.GroupingText = null;
            panelADUserSearchResultsHeader.Visible = false;
            ADUserResultsGrid.KeyFieldName = "ADUserKey";
            ADUserResultsGrid.AddGridColumn("ADUserKey", "ADUserKey", 0, GridFormatType.GridString, null, HorizontalAlign.Left, false, true);
            ADUserResultsGrid.AddGridColumn("ADUserName", "ADUserName", 20, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            ADUserResultsGrid.AddGridColumn("IDNumber", "ID Number", 20, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            ADUserResultsGrid.AddGridColumn("FirstName", "First Name", 20, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            ADUserResultsGrid.AddGridColumn("Surname", "Surname", 20, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            ADUserResultsGrid.AddGridColumn("EmailAddress", "EMail Address", 20, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
        }

        public void SetUpUserOrgHistoryGridView()
        {
            ADUserResultsGrid.PostBackType = GridPostBackType.None;
            panelADUserSearchResults.BorderStyle = BorderStyle.None;
            panelADUserSearchResults.GroupingText = null;
            ADUserResultsGrid.SettingsPager.PageSize = 25;
            panelADUserSearchResultsHeader.Visible = false;
            ADUserResultsGrid.KeyFieldName = "UserOrganisationStructureHistoryKey";
            ADUserResultsGrid.AddGridColumn("UserOrganisationStructureHistoryKey", "UserOrganisationStructureHistoryKey", 0, GridFormatType.GridString, null, HorizontalAlign.Left, false, true);
            ADUserResultsGrid.AddGridColumn("ADUserName", "ADUserName", 10, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            ADUserResultsGrid.AddGridColumn("pathstr", "User Designation History", 50, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            ADUserResultsGrid.AddGridColumn("RoleType", "Role Type", 20, GridFormatType.GridString, null, HorizontalAlign.Center, true, true);
            ADUserResultsGrid.AddGridColumn("StartDate", "Start Date", 10, GridFormatType.GridString, null, HorizontalAlign.Center, true, true);
            ADUserResultsGrid.AddGridColumn("EndDate", "End Date", 10, GridFormatType.GridString, null, HorizontalAlign.Center, true, true);
        }

        public void SetUpUserSummaryGrid()
        {
            ADUserResultsGrid.SettingsPager.PageSize = 30;
            UserSummaryGrid.RowUpdating += new DevExpress.Web.Data.ASPxDataUpdatingEventHandler(UserSummaryGrid_RowUpdating);
            UserSummaryGrid.KeyFieldName = "OrganisationStructureKey";
            UserSummaryGrid.AddGridCommandColumnDateEdit("ChangeDate", "End Date", 10, HorizontalAlign.Left, true);
            UserSummaryGrid.AddGridColumn("pathstr", "User Designation", 55, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);

            if (RoleTypes.Count > 0)
                UserSummaryGrid.AddGridCommandColumnComboBox("RoleType", "Role Type", 35, HorizontalAlign.Left, true, RoleTypes, "Value", "Key");
            else
                UserSummaryGrid.AddGridColumn("RoleType", "Role Type", 35, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);

            UserSummaryGrid.AddGridColumn("OrganisationStructureKey", "OrganisationStructureKey", 0, GridFormatType.GridString, null, HorizontalAlign.Left, false, true);
        }

        public void CheckAndExpandNodes(Dictionary<int, bool> DictSelectedNodes)
        {
            btnSubmit.Visible = btnCancel.Visible = true;
            RecursiveCheckAndExpandNode(tvOrgStruct.Nodes, DictSelectedNodes);
        }

        public void SearchViewCustomSetUp()
        {
            txtADUserName.Attributes["onkeypress"] = String.Format("doSearch('{0}')", btnADUserSearch.ClientID);
        }

        #endregion

        #region Events

        public event KeyChangedEventHandler OnRowItemSelected;
        public event KeyChangedEventHandler ADUserSearchButtonClicked;
        public event KeyChangedEventHandler OnSelectedCompanyChanged;
        public event KeyChangedEventHandler OnSubmitButtonClicked;
        public event KeyChangedEventHandler OnCancelButtonClicked;
        public event KeyChangedEventHandler UserSummaryGridRowUpdating;
        public event KeyChangedEventHandler OnViewADUserHistClicked;
        public event KeyChangedEventHandler ADUserResultsGridPageIndexChanged;
        public event KeyChangedEventHandler OnAddButtonClicked;
        public event KeyChangedEventHandler OnRemoveButtonClicked;
        public event KeyChangedEventHandler OnValidate;

        protected void btnADUserSearch_Click(object sender, EventArgs e)
        {
            if (ADUserSearchButtonClicked != null)
                ADUserSearchButtonClicked(sender, null);
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnSelectedCompanyChanged != null)
                OnSelectedCompanyChanged(sender, new KeyChangedEventArgs(ddlCompany.SelectedValue));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (OnSubmitButtonClicked != null)
                OnSubmitButtonClicked(sender, null);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
                OnCancelButtonClicked(sender, null);
        }

        protected void btnViewADUserHist_Click(object sender, EventArgs e)
        {
            if (OnViewADUserHistClicked != null && ADUserResultsGrid.SelectedKeyValue != null)
                OnViewADUserHistClicked(sender, new KeyChangedEventArgs(ADUserResultsGrid.SelectedKeyValue));
            else
                OnValidate(sender, new KeyChangedEventArgs(1));
        }

        protected void UserSummaryGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            if (UserSummaryGridRowUpdating != null)
            {
                e.Cancel = true;
                int editingIndex = ((SAHL.Common.Web.UI.Controls.DXGridView)sender).FindVisibleIndexByKeyValue(e.Keys[0]);
                Dictionary<string, object> dictNewValues = new Dictionary<string, object>();
                if (e.NewValues["ChangeDate"] != null)
                {
                    DateTime dt = Convert.ToDateTime(e.NewValues["ChangeDate"]);
                    dictNewValues.Add("ChangeDate", dt);
                }
				dictNewValues.Add("RoleType", e.NewValues["RoleType"].ToString());
                UserSummaryGridRowUpdating(dictNewValues, new KeyChangedEventArgs(editingIndex));
            }
        }

        protected void ADUserResultsGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (OnRowItemSelected != null && ADUserResultsGrid.SelectedKeyValue != null)
                OnRowItemSelected(sender, new KeyChangedEventArgs(ADUserResultsGrid.SelectedKeyValue));
        }

        protected void ADUserResultsGrid_PageIndexChanged(object sender, EventArgs e)
        {
            if (ADUserResultsGridPageIndexChanged != null)
                ADUserResultsGridPageIndexChanged(sender, null);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (OnAddButtonClicked != null && ADUserResultsGrid.SelectedKeyValue != null)
                OnAddButtonClicked(sender, new KeyChangedEventArgs(ADUserResultsGrid.SelectedKeyValue));
            else
                OnValidate(sender, new KeyChangedEventArgs(2));
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            if (OnRemoveButtonClicked != null && ADUserResultsGrid.SelectedKeyValue != null)
                OnRemoveButtonClicked(sender, new KeyChangedEventArgs(ADUserResultsGrid.SelectedKeyValue));
            else
                OnValidate(sender, new KeyChangedEventArgs(3));
        }

        #endregion

        #region Helper Methods

        private SAHLTreeNode _parentNode;

        private void BindTreeview(IList<IBindableTreeItem> orgStructLst, SAHLTreeNodeCollection nodes)
        {
            foreach (IBindableTreeItem osItem in orgStructLst)
            {
                SAHLTreeNode tn = AddNode(osItem, nodes);
                if (osItem.Children.Count > 0)
                {
                    _parentNode = tn;
                    BindTreeview(osItem.Children, tn.Nodes);
                }
                else
                {
                    _parentNode = null;
                }
            }
        }

        private SAHLTreeNode AddNode(IBindableTreeItem OSItem, SAHLTreeNodeCollection nodes)
        {
            SAHLTreeNode tn = new SAHLTreeNode(OSItem.Desc, OSItem.Key.ToString());
            tn.AutoPostBack = false; ;

            if (_addNode)
                tn.CheckBoxVisible = (OSItem.Children.Count > 0 ? false : true);
            else if (_userCheckedNodes.ContainsKey(Convert.ToInt32(tn.Value)) && !_userCheckedNodes[Convert.ToInt32(tn.Value)])
                tn.CheckBoxVisible = true;

            tn.HasChildren = (OSItem.Children.Count > 0 ? true : false);

            if (_parentNode != null)
                tn.ParentNode = _parentNode;
            
            nodes.Add(tn);
            return tn;
        }

        private void RecursiveExpand(SAHLTreeNode tn)
        {
            tn.Expanded = true;

            if (tn.ParentNode != null)
                RecursiveExpand(tn.ParentNode);
        }

        private void RecursiveCheckAndExpandNode(SAHLTreeNodeCollection nodes, Dictionary<int,bool> DictSelectedNodes)
        {
            foreach (SAHLTreeNode tn in nodes)
            {
                if (tn.HasChildren)
                    RecursiveCheckAndExpandNode(tn.Nodes, DictSelectedNodes);
                else
                {
                    if (DictSelectedNodes.ContainsKey(Convert.ToInt32(tn.Value)))
                    {
                        tn.CheckBoxSelected = true;
                        tn.CheckBoxDisabled = DictSelectedNodes[Convert.ToInt32(tn.Value)];
                        RecursiveExpand(tn);
                    }
                }
            }
        }

        #endregion
    }
}
