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
using SAHL.Common.Web.UI;
using System.Collections.Generic;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using DevExpress.Web.ASPxTreeList;
using SAHL.Common.Web.UI.Events;
using DevExpress.Web.ASPxEditors;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.Administration
{
    public partial class EstateAgent : SAHLCommonBaseView, IEstateAgent
    {
        private string _selectedNodeKey, _selectedNodeParentKey;
      
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            if (!ShouldRunPage) 
                return;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage) return;

            if (!String.IsNullOrEmpty(_selectedNodeKey))
            {
                // look for the selected treenode to naviaget to
                TreeListNode tn = tlOrgStructure.FindNodeByKeyValue(_selectedNodeKey);
                if (tn == null)
                    tn = tlOrgStructure.FindNodeByKeyValue(_selectedNodeParentKey);

                // if the selected treenode has been removed then navigate to its parent
                if (tn != null)
                {
                    tn.Focus();
                    //recurse up the tree and expand
                    tlOrgStructure.ExpandToSelected(tn);
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!this.ShouldRunPage)
                return;

            lblTip.Visible = AllowNodeDragging;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (SubmitButtonClicked != null)
                SubmitButtonClicked(sender, e);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (AddButtonClicked != null)
                AddButtonClicked(sender, e);
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            if (RemoveButtonClicked != null)
                RemoveButtonClicked(sender, e);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (UpdateButtonClicked != null)
                UpdateButtonClicked(sender, e);
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            if (SelectButtonClicked != null)
                SelectButtonClicked(sender, e);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (CancelButtonClicked != null)
                CancelButtonClicked(sender, e);
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            if (ViewButtonClicked != null)
                ViewButtonClicked(sender, e);
        }

        protected void tlOrgStructure_ProcessDragNode(object sender, TreeListNodeDragEventArgs e)
        {
            if (TreeNodeDragged != null)
                TreeNodeDragged(sender, e);
        }

        protected void btnAddToCBO_Click(object sender, EventArgs e)
        {
            if (OnAddToCBO != null)
                OnAddToCBO(sender, e);
        }

        #region Interface Members

        public bool AddButtonVisible
        {
            set { btnAdd.Visible = value; }
        }

        public string SelectedNodeKey
        {
            set { _selectedNodeKey = value; }
            get { return _selectedNodeKey; }
        }

        public string SelectedNodeParentKey
        {
            set { _selectedNodeParentKey = value; }
            get { return _selectedNodeParentKey; }
        }

        public bool RemoveButtonVisible
        {
            set { btnRemove.Visible = value; }
        }

        public bool UpdateButtonVisible
        {
            set { btnUpdate.Visible = value; }
        }

        public bool SelectButtonVisible
        {
            set { btnSelect.Visible = value; }
        }

        public bool ViewButtonVisible
        {
            set { btnView.Visible = value; }
        }

        public bool CancelButtonVisible
        {
            set { btnCancel.Visible = value; }
        }

        public bool AllowNodeDragging
        {
            set { tlOrgStructure.SettingsEditing.AllowNodeDragDrop = value; }
            get { return tlOrgStructure.SettingsEditing.AllowNodeDragDrop; }
        }

        public bool AllowAddToCBO
        {
            set { btnAddToCBO.Visible = value; }
            get { return btnAddToCBO.Visible; }
        }

        public void BindOrganisationStructure(DataSet orgStructLst)
        {
            tlOrgStructure.ClearNodes();
            tlOrgStructure.SettingsSelection.Enabled = false;
            tlOrgStructure.SettingsBehavior.AllowFocusedNode = true;
            tlOrgStructure.AutoGenerateColumns = false;
            tlOrgStructure.KeyFieldName = "PrimaryKey";
            tlOrgStructure.ParentFieldName = "ParentKey";
            tlOrgStructure.DataSource = orgStructLst;
            tlOrgStructure.DataBind();         
        }

        protected void tlOrgStructure_HtmlRowPrepared(object sender, TreeListHtmlRowEventArgs e)
        {
            // add the Organisation Type to the treenode row attribute so we can use this later 
            // in clientside validation
            if (e.RowKind == TreeListRowKind.Data)
            {
                e.Row.Attributes.Add("organisationType", e.GetValue("OSTypeDescription").ToString());
            }
        } 

        protected void tlOrgStructure_HtmlDataCellPrepared(object sender, TreeListHtmlDataCellEventArgs e)
        {
            if (e.Column.Name=="colImage")
            {
                string osDescription = e.GetValue("OSDescription").ToString();
                string osTypeDescription = e.GetValue("OSTypeDescription").ToString();

                string fileName = String.Empty;
                switch (osTypeDescription)
                {
                    case "Region/Channel":
                        fileName = "~/Images/Workflow.gif";
                        break;
                    case "Company": 
                        fileName = "~/Images/OpenVariable.gif";
                        break;
                    case "Branch/Originator":
                        fileName = "~/Images/OpenProspectVariable.gif";
                        break;
                    case "Designation":
                        if (String.Compare(osDescription,"consultant",true)==0)
                            fileName = "~/Images/Male LE.png";  
                        else
                            fileName = "~/Images/LegalEntity.gif";  // principal
                        break;
                }
                ASPxImage img = (ASPxImage)tlOrgStructure.FindDataCellTemplateControl(e.NodeKey, e.Column, "imgType");
                if (!String.IsNullOrEmpty(fileName))
                    img.ImageUrl = fileName;
                else
                    img.Visible = false;

                SAHLLabel lbl = (SAHLLabel)tlOrgStructure.FindDataCellTemplateControl(e.NodeKey, e.Column, "lblOSDescription");
                lbl.Text = osDescription;
            
            }
        }

        public DataRow GetFocusedNode
        {
            get
            {
                if (tlOrgStructure.FocusedNode != null)
                {
                    DataRowView drv = tlOrgStructure.FocusedNode.DataItem as DataRowView;
                    if (drv != null)
                        return drv.Row;
                }
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler SubmitButtonClicked;

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEstateAgent.AddButtonClicked">IEstateAgent.AddButtonClicked</see>.
        /// </summary>
        public event EventHandler AddButtonClicked;

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEstateAgent.CancelButtonClicked">IEstateAgent.CancelButtonClicked</see>.
        /// </summary>
        public event EventHandler CancelButtonClicked;

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEstateAgent.RemoveButtonClicked">IEstateAgent.RemoveButtonClicked</see>.
        /// </summary>
        public event EventHandler RemoveButtonClicked;

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEstateAgent.UpdateButtonClicked">IEstateAgent.UpdateButtonClicked</see>.
        /// </summary>
        public event EventHandler UpdateButtonClicked;

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEstateAgent.SelectButtonClicked">IEstateAgent.SelectButtonClicked</see>.
        /// </summary>
        public event EventHandler SelectButtonClicked;

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEstateAgent.ViewButtonClicked">IEstateAgent.ViewButtonClicked</see>.
        /// </summary>
        public event EventHandler ViewButtonClicked;

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEstateAgent.TreeNodeDragged">IEstateAgent.TreeNodeDragged</see>.
        /// </summary>
        public event TreeListNodeDragEventHandler TreeNodeDragged;

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEstateAgent.OnAddToCBO">IEstateAgent.OnAddToCBO</see>.
        /// </summary>
        public event EventHandler OnAddToCBO;

        #endregion



    }
}
