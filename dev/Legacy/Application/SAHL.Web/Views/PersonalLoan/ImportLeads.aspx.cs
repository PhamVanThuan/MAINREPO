using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.DomainMessages;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

namespace SAHL.Web.Views.PersonalLoan
{
    public partial class ImportLeads : SAHLCommonBaseView, IPersonalLoanImportLeads
    {
        public event KeyChangedEventHandler OnDownLoadClicked;

        public event EventHandler OnImportClicked
        {
            add { this.btnImport.Click += value; }
            remove { this.btnImport.Click -= value; }
        }

        public HttpPostedFile File
        {
            get { return this.FileName.PostedFile; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            leadImport.Visible = EditMode;
            LeadSummary.Visible = !EditMode;
        }

        public void SetUpLeadSummaryGrid()
        {
            LeadSummaryGrid.PostBackType = GridPostBackType.NoneWithClientSelect;
            LeadSummaryGrid.BorderStyle = BorderStyle.Solid;
            LeadSummaryGrid.SettingsPager.PageSize = 10;

            //LeadSummaryGrid.SettingsBehavior.AllowFocusedRow = true;
            LeadSummaryGrid.KeyFieldName = "BatchServiceKey";
            LeadSummaryGrid.AddGridColumn("BatchServiceKey", "", 0, GridFormatType.GridString, null, HorizontalAlign.Left, false, true);
            LeadSummaryGrid.AddGridColumn("RequestedDate", "Date Requested", 10, GridFormatType.GridDateTime, null, HorizontalAlign.Left, true, true);
            LeadSummaryGrid.AddGridColumn("RequestedBy", "User", 5, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            LeadSummaryGrid.AddGridColumn("FileName", "File", 5, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            LeadSummaryGrid.AddGridColumn("BatchCount", "Leads", 5, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            LeadSummaryGrid.AddGridColumn("Complete", "Complete", 5, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            LeadSummaryGrid.AddGridColumn("Pending", "Pending", 5, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            LeadSummaryGrid.AddGridColumn("Failed", "Failed", 5, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            LeadSummaryGrid.AddGridColumn("Unsuccessful", "Unsuccessful", 5, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
        }

        public void BindLeadSummaryGrid(List<BatchServiceResult> results)
        {
            LeadSummaryGrid.Selection.UnselectAll();
            LeadSummaryGrid.DataSource = results;
            LeadSummaryGrid.DataBind();
        }

        public bool EditMode
        {
            get;
            set;
        }

        protected void SAHLRefresh_Click(object sender, EventArgs e)
        {
        }

        public string ImportFileName
        {
            get { return this.FileName.FileName; }
        }

        protected void OnDownload_Click(object sender, EventArgs e)
        {
            if (OnDownLoadClicked != null)
            {
                if (LeadSummaryGrid.SelectedKeyValue != null)
                {
                    OnDownLoadClicked(sender, new KeyChangedEventArgs(LeadSummaryGrid.SelectedKeyValue));
                }
                else
                {
                    var message = "Please select a file to download.";
                    this.lblError.Text = message;
                }
            }
        }

        public void FileReadyForDownload(IBatchService batchService)
        {
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + batchService.FileName);
            Response.BinaryWrite(batchService.FileContent);
            Response.Flush();
            Response.End();
        }
    }
}