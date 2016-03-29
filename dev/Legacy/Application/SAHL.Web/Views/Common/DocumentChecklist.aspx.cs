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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common
{
    public partial class DocumentChecklist : SAHLCommonBaseView, IDocumentChecklist
    {

        private bool _viewOnly;

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
            {
                OnCancelButtonClicked(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (OnSubmitButtonClicked != null)
            {
                OnSubmitButtonClicked(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentList"></param>
        public void BindDocumentList(IList<IApplicationDocument> documentList)
        {
            DocCheckListGrid.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            DocCheckListGrid.AddGridBoundColumn("Description", "Description", Unit.Percentage(45), HorizontalAlign.Left, true);
            if (_viewOnly)
            {
                DocCheckListGrid.AddGridBoundColumn("DocumentReceivedBy", "Received By", Unit.Percentage(10), HorizontalAlign.Left, true);
                DocCheckListGrid.AddGridBoundColumn("DocumentReceivedDate", SAHL.Common.Constants.DateTimeFormat, GridFormatType.GridDate, "Received Date", false, Unit.Percentage(10), HorizontalAlign.Center, true);
            }
            else
            {
                DocCheckListGrid.AddGridBoundColumn("DocumentReceivedBy", "Received By", Unit.Percentage(10), HorizontalAlign.Left, false);
                DocCheckListGrid.AddGridBoundColumn("DocumentReceivedDate", SAHL.Common.Constants.DateTimeFormat, GridFormatType.GridDate, "Received Date", false, Unit.Percentage(10), HorizontalAlign.Center, false);
                DocCheckListGrid.AddCheckBoxColumn("chkSelect", "Received", false, Unit.Percentage(10), HorizontalAlign.Left, true);
            }
            DocCheckListGrid.AddGridBoundColumn("DocumentReceivedBy", "Last Updated By", Unit.Percentage(10), HorizontalAlign.Left, true);
            DocCheckListGrid.DataSource = documentList;
            DocCheckListGrid.DataBind();

            /*foreach (IApplicationDocument appDoc in documentList)
            {
                ListItem item = new ListItem(appDoc.Description, appDoc.Key.ToString());
                item.Selected = (appDoc.DocumentReceivedDate.HasValue ? true : false);
                clbDocuments.Items.Add(item);
            }
            */
            /*
            clbDocuments.DataTextField = "Description";
            clbDocuments.DataValueField = "Key";
            clbDocuments.DataSource = documentList;
            clbDocuments.DataBind();
            */
            
            /*
            for (int i = 0; i < documentList.Count; i++)
            {
                clbDocuments.Items.Add(new ListItem(
                                documentList[i].DocumentType.Description.ToString(),
                                documentList[i].Key.ToString()
                                ));
                if (documentList[i].DocumentReceived == true)
                    clbDocuments.Items[i].Selected = true;
            }
            */
        }

        public bool HideControls
        {
            set
            {
                SubmitButton.Visible = value;
                CancelButton.Visible = value;
            }
        }

        public bool SetViewOnly
        {
            set { _viewOnly = value; }
        }

        public Dictionary<int, bool> GetCheckedItems
        {
            get
            {
                Dictionary<int,bool> dict = new Dictionary<int,bool>();
                foreach (GridViewRow row in DocCheckListGrid.Rows)
                {
                    CheckBox chkBox = (CheckBox)row.FindControl("chkSelect");
                    dict.Add(Convert.ToInt32(row.Cells[0].Text), chkBox.Checked);
                }
                return dict;
            }
        }

        protected void DocCheckListGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Check recieved date, to determine if document recieved or not
                if (!string.IsNullOrEmpty(e.Row.Cells[3].Text) && e.Row.Cells[3].Text.Trim().Length > DateTime.Now.ToString(SAHL.Common.Constants.DateTimeFormat).Length)
                {
                    CheckBox chkBox = (CheckBox)e.Row.FindControl("chkSelect");
                    
                    if (chkBox != null)
                        chkBox.Checked = true;
                }
                else
                {
                    e.Row.Cells[2].Text = "";
                }
            }
        }
    }
}
