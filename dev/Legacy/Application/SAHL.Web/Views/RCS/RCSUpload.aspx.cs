using System;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.RCS.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using System.Text;

namespace SAHL.Web.Views.RCS
{
    /// <summary>
    /// Upload RCS data
    /// </summary>
    public partial class RCSUpload : SAHLCommonBaseView, IRCSUpload
    {
        #region Members

        public string UploadFileFileName
        {
            get { return UploadFile.FileName; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.RCS.Interfaces.IRCSUpload.UploadButtonEnabled">IRCSUpload.UploadButtonEnabled</see>.
        /// </summary>
        public bool UploadButtonEnabled
        {
            set { UploadButton.Enabled = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.RCS.Interfaces.IRCSUpload.SubmitButtonEnabled">IRCSUpload.SubmitButtonEnabled</see>.
        /// </summary>
        public bool SubmitButtonEnabled
        {
            set { SubmitButton.Enabled = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.RCS.Interfaces.IRCSUpload.ReplaceButtonEnabled">IRCSUpload.ReplaceButtonEnabled</see>.
        /// </summary>
        public bool ReplaceButtonEnabled
        {
            set { ReplaceButton.Enabled = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.RCS.Interfaces.IRCSUpload.ResultsButtonEnabled">IRCSUpload.ResultsButtonEnabled</see>.
        /// </summary>
        public bool ResultsButtonEnabled
        {
            set { ResultsButton.Enabled = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.RCS.Interfaces.IRCSUpload.FileNameEnabled">IRCSUpload.FileNameEnabled</see>.
        /// </summary>
        public bool FileNameEnabled
        {
            set { UploadFile.Enabled = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.RCS.Interfaces.IRCSUpload.FileNameVisible">IRCSUpload.FileNameVisible</see>.
        /// </summary>
        public bool FileNameVisible
        {
            set { UploadFile.Visible = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.RCS.Interfaces.IRCSUpload.FileNameValEnabled">IRCSUpload.FileNameValEnabled</see>.
        /// </summary>
        public bool FileNameValEnabled
        {
            set { FileNameVal.Enabled = value; }
        }

		/// <summary>
		/// Implements <see cref="SAHL.Web.Views.RCS.Interfaces.IRCSUpload.FileNameValIsValid">IRCSUpload.FileNameValIsValid</see>.
		/// </summary>
		public bool FileNameValIsValid
		{
			set { FileNameVal.IsValid = value; }
		}

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.RCS.Interfaces.IRCSUpload.FileNameDisplayVisible">IRCSUpload.FileNameDisplayVisible</see>.
        /// </summary>
        public bool FileNameDisplayVisible
        {
            set { FileNameDisplay.Visible = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.RCS.Interfaces.IRCSUpload.FileNameDisplayText">IRCSUpload.FileNameDisplayText</see>.
        /// </summary>
        public string FileNameDisplayText
        {
            set { FileNameDisplay.Text = value; }
            get { return FileNameDisplay.Text; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.RCS.Interfaces.IRCSUpload.ReplaceListRowsAdd">IRCSUpload.ReplaceListRowsAdd</see>.
        /// </summary>
        public TableRow ReplaceListRowsAdd
        {
            set { ReplaceList.Rows.Add(value); }
        }

        public bool ReplacementTableVisible
        {
            get { return ReplacementTable.Visible; }
            set { ReplacementTable.Visible = value; }
        }

		//FileNameVal.IsValid = false;


        #endregion

        #region EventHandlers

        public event EventHandler UploadClick;

        public event EventHandler CancelClick;

        public event EventHandler ReplaceClick;

        public event EventHandler SubmitClick;

        public event EventHandler ResultsClick;

        #endregion

        #region Events

        /// <summary>
        /// Format Grid data in rowdata bound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UploadHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;
            DateTime dateImported;
            IImportFile importFile = e.Row.DataItem as IImportFile;

            if (e.Row.DataItem != null)
            {
                cells[0].Text = importFile.FileName.ToString();

                dateImported = Convert.ToDateTime(importFile.DateImported.ToString());
                cells[1].Text = dateImported.ToString(SAHL.Common.Constants.DateFormat);

                cells[2].Text = importFile.Status.ToString();
                cells[3].Text = importFile.UserID == null ? " " : importFile.UserID.ToString();
            }
        }

        /// <summary>
        /// Upload button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Upload_Click(object sender, EventArgs e)
        {
            if (UploadClick != null)
                UploadClick(sender, e);
        }

        /// <summary>
        /// Cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Cancel_Click(object sender, EventArgs e)
        {
            if (CancelClick != null)
                CancelClick(sender, e);
        }

        /// <summary>
        /// View Results button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Results_Click(object sender, EventArgs e)
        {
            if (ResultsClick != null)
                ResultsClick(sender, e);
        }

        /// <summary>
        /// Replace button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Replace_Click(object sender, EventArgs e)
        {
            if (ReplaceClick != null)
                ReplaceClick(sender, e);
        }

        /// <summary>
        /// Submit button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Submit_Click(object sender, EventArgs e)
        {
            if (SubmitClick != null)
                SubmitClick(sender, e);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the ImportFile history
        /// </summary>
        /// <param name="importFileHistory"></param>
        public void BindUploadHistoryGrid(IReadOnlyEventList<IImportFile> importFileHistory)
        {
            UploadHistoryGrid.Columns.Clear();
            UploadHistoryGrid.AddGridBoundColumn("FileName", "File Name", Unit.Percentage(40), HorizontalAlign.Left, true);
            UploadHistoryGrid.AddGridBoundColumn("DateImported", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Date Imported", false, Unit.Percentage(20), HorizontalAlign.Center, true);
            UploadHistoryGrid.AddGridBoundColumn("Status", "Status", Unit.Percentage(20), HorizontalAlign.Left, true);
            UploadHistoryGrid.AddGridBoundColumn("UserID", "User ID", Unit.Percentage(20), HorizontalAlign.Left, true);
            UploadHistoryGrid.DataSource = importFileHistory;
            UploadHistoryGrid.DataBind();
        }

        /// <summary>
        /// Save Upload File
        /// </summary>
        /// <param name="uploadFile"></param>
        public Boolean SaveUploadFile(string uploadFile)
        {
            try
            {
                if (uploadFile.Length > 0)
                {
                    UploadFile.SaveAs(uploadFile);
                    return true;
                }
                else
                    return false;
            }

            catch
            {
                return false;
            }
        }

        public void DropControlItemsAdd(SAHLDropDownList dropDownControl, string s)
        {
            ListItem sList = new ListItem(s, s);

            dropDownControl.Items.Add(sList);
        }

        public string GetReplaceValues(string controlName)
        {
            if (Request.Form[controlName] != null && Request.Form[controlName] != "-select-")
                return Convert.ToString(Request.Form[controlName]);
            else
            {
                ReplaceValidator.IsValid = false;
                return "";
            }
        }

        public void RegisterClientScripts(string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(message);
            if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "MessageDisplay"))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MessageDisplay", sb.ToString(), true);
            }
        }

        public void ViewResults(IReadOnlyEventList<IImportLegalEntity> importLegalEntity)
        {
			string fileName = "filename=" + DateTime.Now.ToString(SAHL.Common.Constants.DateFormat).Replace("/", "_") + "_results.csv" + ";";

			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Identity Number" + "," + "ErrorMessage" + ",");
			for (int i = 0; i < importLegalEntity.Count; i++)
			{
				sb.AppendLine(importLegalEntity[i].IDNumber + "," + importLegalEntity[i].ImportApplication.ErrorMsg + ",");
			}
			Response.Clear();
			Response.ClearHeaders();
			Response.ClearContent();
			Response.ContentType = "application/csv";
			Response.AddHeader("Content-Disposition", fileName);
			Response.Write(sb.ToString());
			try
			{
				Response.End();
			}
			catch
			{
				// do nothing - sink error
			}
        }

        #endregion
    }
}
